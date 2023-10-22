﻿using System;
using System.Windows.Forms;

using DLS;
using InstPack;

namespace InstrumentEditor {
    public partial class LayerInfoDialog : Form {
        private Pack mFile;
        private RGN mLayer;

        public LayerInfoDialog(Pack file, RGN layer) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

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

        private void btnSelect_Click(object sender, EventArgs e) {
            var fm = new InstSelectDialog(mFile, mLayer);
            fm.ShowDialog();
            //var instIndex = mLayer.InstIndex;
            //if (instIndex < mFile.Inst.Count) {
            //    var inst = mFile.Inst[instIndex];
            //    txtInst.Text = string.Format(
            //        "{0} {1}",
            //        instIndex.ToString("0000"),
            //        inst.Info[Info.TYPE.INAM]
            //    );
            //} else {
            //    txtInst.Text = "";
            //}
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            mLayer.Header.KeyLo = (byte)numKeyLow.Value;
            mLayer.Header.KeyHi = (byte)numKeyHigh.Value;
            mLayer.Header.VelLo = (byte)numVelocityLow.Value;
            mLayer.Header.VelHi = (byte)numVelocityHigh.Value;
            var gain = (int)(20 * numVolume.Value) / 400.0;
            if (0 == gain) {
                mLayer.Articulations.Delete(DST_TYPE.GAIN);
            } else {
                mLayer.Articulations.Update(DST_TYPE.GAIN, (float)Math.Pow(10.0, gain));
            }
            var fineTune = (int)(1200 * numFineTune.Value) / 1440000.0;
            if (0 == fineTune && 0 == numTranspose.Value) {
                mLayer.Articulations.Delete(DST_TYPE.PITCH);
            } else {
                mLayer.Articulations.Update(DST_TYPE.PITCH,
                    (float)Math.Pow(2.0, (double)numTranspose.Value / 12.0 + fineTune)
                );
            }
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
            grbInst.Top = grbVelocity.Top + grbVelocity.Height + 4;
            grbInst.Width = grbKey.Width + grbVelocity.Width + 4;
            grbInst.Height = txtInst.Top + txtInst.Height + 4;

            grbUnityNote.Top = grbInst.Top + grbInst.Height + 4;
            grbFineTune.Top = grbUnityNote.Top;
            grbFineTune.Left = grbUnityNote.Left + grbUnityNote.Width + 4;
            grbVolume.Top = grbUnityNote.Top;
            grbVolume.Left = grbFineTune.Left + grbFineTune.Width + 4;

            btnAdd.Top = grbVolume.Bottom + 4;
            btnAdd.Left = grbVolume.Right - btnAdd.Width;

            Width = btnAdd.Right + 24;
            Height = btnAdd.Top + btnAdd.Height + 48;
        }

        private void SetKeyLow() {
            var oct = (int)numKeyLow.Value / 12 - 2;
            var note = (int)numKeyLow.Value % 12;
            lblKeyLow.Text = string.Format("{0}{1}", Const.NoteName[note], oct);

            if (numKeyHigh.Value < numKeyLow.Value) {
                numKeyHigh.Value = numKeyLow.Value;
            }
        }

        private void SetKeyHigh() {
            var oct = (int)numKeyHigh.Value / 12 - 2;
            var note = (int)numKeyHigh.Value % 12;
            lblKeyHigh.Text = string.Format("{0}{1}", Const.NoteName[note], oct);

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

                //var instIndex = mLayer.InstIndex;
                //var instName = "";
                //if (instIndex < mFile.Inst.Count) {
                //    var inst = mFile.Inst[instIndex];
                //    instName = inst.Info[Info.TYPE.INAM];
                //}

                //if (int.MaxValue == instIndex) {
                //    txtInst.Text = "";
                //} else {
                //    txtInst.Text = string.Format(
                //        "{0} {1}",
                //        instIndex.ToString("0000"),
                //        instName
                //    );
                //}

                var cent = 0;
                foreach(var art in mLayer.Articulations.List) {
                    switch (art.Destination) {
                    case DST_TYPE.GAIN:
                        numVolume.Value = (decimal)(20 * Math.Log10(art.Value));
                        break;
                    case DST_TYPE.PITCH:
                        cent = (int)(1200.0 / Math.Log(2.0, art.Value));
                        break;
                    }
                }

                numFineTune.Value = cent % 100;
                numTranspose.Value = cent / 100;

                btnAdd.Text = "反映";
            }
            SetKeyLow();
            SetKeyHigh();
            SetVelocityLow();
            SetVelocityHigh();
        }
    }
}
