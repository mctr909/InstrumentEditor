﻿using System;
using System.Windows.Forms;

using Instruments;

namespace InstrumentEditor {
    public partial class InstSelectDialog : Form {
        private File mFile;
        private Layer mLayer;

        public InstSelectDialog(File file, Layer layer) {
            InitializeComponent();
            mFile = file;
            mLayer = layer;
        }

        private void InstSelectDialog_Load(object sender, EventArgs e) {
            DispList("");
            SetSize();
        }

        private void InstSelectDialog_SizeChanged(object sender, EventArgs e) {
            SetSize();
        }

        private void lstInst_DoubleClick(object sender, EventArgs e) {
            if (0 == lstInst.Items.Count) {
                return;
            }
            var cols = lstInst.SelectedItem.ToString().Split('|');
            var idx = int.Parse(cols[0]);
            //FORM//var fm = new WaveInfoForm(mFile, idx);
            var index = lstInst.SelectedIndex;
            //FORM//fm.ShowDialog();
            DispList(txtSearch.Text);
            lstInst.SelectedIndex = index;
        }

        private void btnSelect_Click(object sender, EventArgs e) {
            if (0 <= lstInst.SelectedIndex) {
                var cols = lstInst.SelectedItem.ToString().Split('|');
                mLayer.Art.Update(ART_TYPE.INST_INDEX, uint.Parse(cols[0]));
            }
            Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) {
            DispList(txtSearch.Text);
        }

        private void SetSize() {
            var offsetX = 24;
            var offsetY = 48;

            btnSelect.Width = 60;
            btnSelect.Height = 24;

            lstInst.Left = 4;
            lstInst.Top = 4;
            lstInst.Width = Width - offsetX;
            lstInst.Height = Height - btnSelect.Height - 6 - offsetY;

            txtSearch.Top = lstInst.Top + lstInst.Height + 6;
            txtSearch.Left = lstInst.Left;
            txtSearch.Width = 200;

            btnSelect.Top = txtSearch.Top;
            btnSelect.Left = Width - btnSelect.Width - offsetX;
        }

        private void DispList(string keyword) {
            lstInst.Items.Clear();
            int count = 0;
            for (var iInst = 0; iInst < mFile.Inst.Count; iInst++) {
                var inst = mFile.Inst[iInst];
                var name = "";
                if (null == inst.Info || string.IsNullOrWhiteSpace(inst.Info.Name)) {
                    name = string.Format("Inst[{0}]", count);
                } else {
                    name = inst.Info.Name;
                }

                if (!string.IsNullOrEmpty(keyword) && name.IndexOf(keyword) < 0) {
                    continue;
                }

                var use = false;
                foreach (var ins in mFile.Inst.Array) {
                    foreach (var rgn in ins.Region.Array) {
                        foreach(var art in rgn.Art.Array) {
                            if (art.Type != ART_TYPE.INST_INDEX) {
                                continue;
                            }
                            if (count == (int)art.Value) {
                                use = true;
                                break;
                            }
                        }
                    }
                }

                lstInst.Items.Add(string.Format(
                    "{0}\t{1}\t{2}",
                    iInst.ToString("0000"),
                    use ? "*" : " ",
                    name
                ));
                ++count;
            }

            foreach (var art in mLayer.Art.Array) {
                if (art.Type != ART_TYPE.INST_INDEX) {
                    continue;
                }
                if (art.Value < lstInst.Items.Count) {
                    lstInst.SelectedIndex = (int)art.Value;
                }
            }
        }
    }
}
