using System;
using System.Windows.Forms;

using InstPack;

namespace InstrumentEditor {
    public partial class InstSelectDialog : Form {
        private Pack mFile;
        private Layer mLayer;

        public InstSelectDialog(Pack file, Layer layer) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
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

        private void btnSelect_Click(object sender, EventArgs e) {
            if (0 <= lstInst.SelectedIndex) {
                var cols = lstInst.SelectedItem.ToString().Split('|');
                mLayer.InstIndex = int.Parse(cols[0]);
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
                if (string.IsNullOrWhiteSpace(inst.Info[Info.TYPE.INAM])) {
                    name = string.Format("Inst[{0}]", count);
                } else {
                    name = inst.Info[Info.TYPE.INAM];
                }

                if (!string.IsNullOrEmpty(keyword) && name.IndexOf(keyword) < 0) {
                    continue;
                }

                var use = false;
                foreach (var ins in mFile.Inst.ToArray()) {
                    foreach (var rgn in ins.Region.Array) {
                        if (count == rgn.WaveLink.TableIndex) {
                            use = true;
                            break;
                        }
                    }
                    if (use) {
                        break;
                    }
                }

                lstInst.Items.Add(string.Format(
                    "{0}|{1}|{2}",
                    iInst.ToString("0000"),
                    use ? "*" : " ",
                    name
                ));
                ++count;
            }
        }
    }
}
