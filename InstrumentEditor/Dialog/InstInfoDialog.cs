﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DLS;
using InstPack;

namespace InstrumentEditor {
    public partial class InstInfoDialog : Form {
        private Pack mFile;
        private INS mInst;

        public InstInfoDialog(Pack file) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
        }

        public InstInfoDialog(Pack file, INS inst) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
            mInst = inst;
            txtInstName.Text = mInst.Info[Info.TYPE.INAM];
            envelope1.Art = mInst.Articulations.List;
            btnAdd.Text = "反映";
        }

        private void InstInfoDialog_Load(object sender, EventArgs e) {
            if (null != mInst) {
                cmbCategory.SelectedText = mInst.Info[Info.TYPE.ICAT];
            }
            setCategoryList();
            setPos();
        }

        private void cmbCategory_Leave(object sender, EventArgs e) {
            setCategoryList();
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (null == mInst) {
                mInst = new INS();
                mInst.Info[Info.TYPE.INAM] = txtInstName.Text;
                mInst.Info[Info.TYPE.ICAT] = cmbCategory.Text;
                mFile.Inst.Add(mInst);
                envelope1.SetList(mInst.Articulations);
            } else {
                mInst.Info[Info.TYPE.INAM] = txtInstName.Text;
                mInst.Info[Info.TYPE.ICAT] = cmbCategory.Text;
                envelope1.SetList(mInst.Articulations);
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
                if ("" != inst.Info[Info.TYPE.ICAT]) {
                    if (!cmbCategory.Items.Contains(inst.Info[Info.TYPE.ICAT].Trim())) {
                        cmbCategory.Items.Add(inst.Info[Info.TYPE.ICAT].Trim());
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
