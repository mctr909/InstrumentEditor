using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Instruments;

namespace InstrumentEditor {
    public partial class LayerInfoDialog : Form {
        private File mFile;
        private Layer mLayer;

        private readonly string[] NoteName = new string[] {
            "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
        };

        public LayerInfoDialog(File file, Layer layer) {
            InitializeComponent();

            mFile = file;
            mLayer = layer;

            SetPosition();
            DispInfo();
        }

        private void numKeyLow_ValueChanged(object sender, EventArgs e) {
            SetKeyLow();
        }

        private void numKeyHigh_ValueChanged(object sender, EventArgs e) {
            SetKeyHigh();
        }

        private void numVelocityLow_ValueChanged(object sender, EventArgs e) {
            SetVelocityLow();
        }

        private void numVelocityHigh_ValueChanged(object sender, EventArgs e) {
            SetVelocityHigh();
        }

        private void numUnityNote_ValueChanged(object sender, EventArgs e) {
            SetUnityNote();
        }

        private void btnSelect_Click(object sender, EventArgs e) {
            var instIndex = 0;
            foreach(var art in mLayer.Art.Array) {
                if (art.Type == ART_TYPE.INST_INDEX) {
                    instIndex = (int)art.Value;
                    break;
                }
            }

            var fm = new InstSelectDialog(mFile, mLayer);
            fm.StartPosition = FormStartPosition.CenterParent;
            fm.ShowDialog();

            if (instIndex < mFile.Inst.Count) {
                var inst = mFile.Inst[instIndex];
                txtInst.Text = string.Format(
                    "{0} {1}",
                    instIndex.ToString("0000"),
                    inst.Info.Name
                );
            } else {
                txtInst.Text = "";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            mLayer.Header.KeyLo = (byte)numKeyLow.Value;
            mLayer.Header.KeyHi = (byte)numKeyHigh.Value;
            mLayer.Header.VelLo = (byte)numVelocityLow.Value;
            mLayer.Header.VelHi = (byte)numVelocityHigh.Value;
            Close();
        }

        private void SetPosition() {
            numKeyLow.Top = 12;
            numKeyHigh.Top = 12;
            lblKeyLow.Top = numKeyLow.Top + numKeyLow.Height + 2;
            lblKeyHigh.Top = numKeyHigh.Top + numKeyHigh.Height + 2;

            grbKey.Top = 4;
            grbKey.Height
                = numKeyLow.Top
                + numKeyLow.Height + 2
                + lblKeyLow.Height + 4
            ;

            numVelocityLow.Top = numKeyLow.Top;
            numVelocityHigh.Top = numKeyHigh.Top;
            grbVelocity.Left = grbKey.Right + 6;
            grbVelocity.Top = grbKey.Top;
            grbVelocity.Height = grbKey.Height;

            txtInst.Top = 12;
            btnSelect.Top = 12;
            btnSelect.Left = txtInst.Left + txtInst.Width + 4;
            grbInst.Top = grbVelocity.Top + grbVelocity.Height + 6;
            grbInst.Width = grbKey.Width + grbVelocity.Width + 6;
            grbInst.Height = txtInst.Top + txtInst.Height + 6;

            grbUnityNote.Top = grbInst.Top + grbInst.Height + 6;
            grbFineTune.Top = grbUnityNote.Top;
            grbFineTune.Left = grbUnityNote.Left + grbUnityNote.Width + 6;
            grbVolume.Top = grbUnityNote.Top;
            grbVolume.Left = grbFineTune.Left + grbFineTune.Width + 6;

            btnAdd.Top = grbVolume.Bottom + 4;
            btnAdd.Left = grbVolume.Right - btnAdd.Width;

            Width = btnAdd.Right + 24;
            Height = btnAdd.Top + btnAdd.Height + 48;
        }

        private void SetKeyLow() {
            var oct = (int)numKeyLow.Value / 12 - 2;
            var note = (int)numKeyLow.Value % 12;
            lblKeyLow.Text = string.Format("{0}{1}", NoteName[note], oct);

            if (numKeyHigh.Value < numKeyLow.Value) {
                numKeyHigh.Value = numKeyLow.Value;
            }
        }

        private void SetKeyHigh() {
            var oct = (int)numKeyHigh.Value / 12 - 2;
            var note = (int)numKeyHigh.Value % 12;
            lblKeyHigh.Text = string.Format("{0}{1}", NoteName[note], oct);

            if (numKeyHigh.Value < numKeyLow.Value) {
                numKeyLow.Value = numKeyHigh.Value;
            }
        }

        private void SetVelocityLow() {
            if (numVelocityHigh.Value < numVelocityLow.Value) {
                numVelocityHigh.Value = numVelocityLow.Value;
            }
        }

        private void SetVelocityHigh() {
            if (numVelocityHigh.Value < numVelocityLow.Value) {
                numVelocityLow.Value = numVelocityHigh.Value;
            }
        }

        private void SetUnityNote() {
            var oct = (int)numUnityNote.Value / 12 - 2;
            var note = (int)numUnityNote.Value % 12;
            lblUnityNote.Text = string.Format("{0}{1}", NoteName[note], oct);
        }

        private void DispInfo() {
            if (byte.MaxValue == mLayer.Header.KeyLo) {
                numKeyLow.Value = 63;
                numKeyHigh.Value = 63;
                numVelocityLow.Value = 0;
                numVelocityHigh.Value = 127;

                btnAdd.Text = "追加";
            } else {
                numKeyLow.Value = mLayer.Header.KeyLo;
                numKeyHigh.Value = mLayer.Header.KeyHi;
                numVelocityLow.Value = mLayer.Header.VelLo;
                numVelocityHigh.Value = mLayer.Header.VelHi;

                var instIndex = int.MaxValue;
                foreach (var art in mLayer.Art.Array) {
                    if (art.Type == ART_TYPE.INST_INDEX) {
                        instIndex = (int)art.Value;
                        break;
                    }
                }

                var instName = "";
                if (instIndex < mFile.Inst.Count) {
                    var inst = mFile.Inst[instIndex];
                    instName = inst.Info.Name;
                }

                if (int.MaxValue == instIndex) {
                    txtInst.Text = "";
                } else {
                    txtInst.Text = string.Format(
                        "{0} {1}",
                        instIndex.ToString("0000"),
                        instName
                    );
                }

                foreach(var art in mLayer.Art.Array) {
                    switch (art.Type) {
                    case ART_TYPE.GAIN_CONST:
                        numVolume.Value = (decimal)(art.Value * 100.0);
                        break;
                    case ART_TYPE.PITCH_CONST:
                        numFineTune.Value = (decimal)(1200.0 / Math.Log(2.0, art.Value));
                        break;
                    case ART_TYPE.OVERRIDE_KEY:
                        numUnityNote.Value = (int)art.Value;
                        break;
                    }
                }

                btnAdd.Text = "反映";
            }
            SetKeyLow();
            SetKeyHigh();
            SetVelocityLow();
            SetVelocityHigh();
        }
    }
}
