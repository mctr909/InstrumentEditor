﻿using InstPack;
using System;
using System.Windows.Forms;

namespace InstrumentEditor {
    public partial class InstInfoDialog : Form {
        private Pack mFile;
        private Inst mInst;

        public InstInfoDialog(Pack file) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
        }

        public InstInfoDialog(Pack file, Inst inst) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
            mInst = inst;
            if (null == mInst.Info) {
                mInst.Info = new Riff.Info();
            }
            txtInstName.Text = mInst.Info.Name;
            envelope1.Art = mInst.Art;
            btnAdd.Text = "反映";
        }

        private void InstInfoDialog_Load(object sender, EventArgs e) {
            if (null != mInst && null != mInst.Info) {
                cmbCategory.SelectedText = mInst.Info.Category;
            }
            setCategoryList();
            setPos();
        }

        private void cmbCategory_Leave(object sender, EventArgs e) {
            setCategoryList();
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (null == mInst) {
                mInst = new Inst();
                mInst.Info = new Riff.Info();
                mInst.Info.Name = txtInstName.Text;
                mInst.Info.Category = cmbCategory.Text;
                mFile.Inst.Add(mInst);
                mInst.Art = new Lart();
                envelope1.SetList(mInst.Art);
            } else {
                mInst.Info.Name = txtInstName.Text;
                mInst.Info.Category = cmbCategory.Text;
                envelope1.SetList(mInst.Art);
            }
            Close();
        }

        private void setCategoryList() {
            var tmpCategory = cmbCategory.SelectedText;
            cmbCategory.Items.Clear();
            if (!string.IsNullOrWhiteSpace(tmpCategory)) {
                cmbCategory.Items.Add(tmpCategory);
            }
            foreach (var inst in mFile.Inst.ToArray()) {
                if (null != inst.Info && "" != inst.Info.Category) {
                    if (!cmbCategory.Items.Contains(inst.Info.Category.Trim())) {
                        cmbCategory.Items.Add(inst.Info.Category.Trim());
                    }
                }
            }
            cmbCategory.SelectedText = tmpCategory;
        }

        private void setPos() {
            btnAdd.Top = envelope1.Bottom + 4;
            btnAdd.Left = envelope1.Right - btnAdd.Width;
            Width = envelope1.Right + 24;
            Height = btnAdd.Bottom + 48;
        }
    }
}
