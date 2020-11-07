using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Instruments;

namespace InstrumentEditor {
    public partial class RegionInfoForm : Form {
        private File mFile;
        private Region mRegion;

        private readonly string[] NoteName = new string[] {
            "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
        };

        public RegionInfoForm(File file, Region region) {
            InitializeComponent();

            mFile = file;
            mRegion = region;

            SetPosition();
            DispRegionInfo();
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

        private void btnSelectWave_Click(object sender, EventArgs e) {
            var waveIndex = 0;
            foreach(var art in mRegion.Art.Array) {
                if (art.Type == ART_TYPE.WAVE_INDEX) {
                    waveIndex = (int)art.Value;
                    break;
                }
            }

            var fm = new WaveSelectDialog(mFile, mRegion);
            fm.ShowDialog();

            if (mFile.Wave.ContainsKey(waveIndex)) {
                var wave = mFile.Wave[waveIndex];
                btnEditWave.Enabled = true;
                txtWave.Text = string.Format(
                    "{0} {1}",
                    waveIndex.ToString("0000"),
                    wave.Info.Name
                );
            } else {
                btnEditWave.Enabled = false;
                txtWave.Text = "";
            }
        }

        private void btnEditWave_Click(object sender, EventArgs e) {
            foreach(var art in mRegion.Art.Array) {
                if (art.Type == ART_TYPE.WAVE_INDEX) {
                    var fm = new WaveInfoForm(mFile, (int)art.Value);
                    fm.ShowDialog();
                    return;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (byte.MaxValue == mRegion.Header.KeyLo) {
                mRegion.Header.KeyLo = (byte)numKeyLow.Value;
                mRegion.Header.KeyHi = (byte)numKeyHigh.Value;
                mRegion.Header.VelLo = (byte)numVelocityLow.Value;
                mRegion.Header.VelHi = (byte)numVelocityHigh.Value;
            }

            mRegion.Art.Update(ART_TYPE.OVERRIDE_KEY, (byte)numUnityNote.Value);
            mRegion.Art.Update(ART_TYPE.FINE_TUNE, (float)Math.Pow(2.0, (byte)numFineTune.Value / 1200.0));
            mRegion.Art.Update(ART_TYPE.GAIN_CONST, (float)(numVolume.Value / 100.0m));

            envelope1.SetList(mRegion.Art);

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
            grbVelocity.Left = grbKey.Left + grbKey.Width + 36;
            grbVelocity.Top = grbKey.Top;
            grbVelocity.Height = grbKey.Height;

            txtWave.Top = 12;
            btnSelectWave.Top = 12;
            btnEditWave.Top = 12;
            btnSelectWave.Left = txtWave.Left + txtWave.Width + 4;
            btnEditWave.Left = btnSelectWave.Left + btnSelectWave.Width + 4;
            grbWave.Top = grbVelocity.Top + grbVelocity.Height + 6;
            grbWave.Width = grbKey.Width + grbVelocity.Width + 36;
            grbWave.Height = txtWave.Top + txtWave.Height + 6;

            grbUnityNote.Top = grbWave.Top + grbWave.Height + 6;
            grbFineTune.Top = grbUnityNote.Top;
            grbFineTune.Left = grbUnityNote.Left + grbUnityNote.Width + 6;
            grbVolume.Top = grbUnityNote.Top;
            grbVolume.Left = grbFineTune.Left + grbFineTune.Width + 6;

            envelope1.Top = grbVolume.Top + grbVolume.Height + 6;
            envelope1.Left = grbUnityNote.Left;

            envelope1.Art = mRegion.Art;

            chkLoop.Top = envelope1.Top + envelope1.Height + 6;

            btnAdd.Top = chkLoop.Top;
            btnAdd.Left = envelope1.Right - btnAdd.Width;

            Width = envelope1.Left + envelope1.Width + 24;
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

        private void DispRegionInfo() {
            if (byte.MaxValue == mRegion.Header.KeyLo) {
                numKeyLow.Value = 63;
                numKeyHigh.Value = 63;
                numVelocityLow.Value = 0;
                numVelocityHigh.Value = 127;
                btnEditWave.Enabled = false;

                btnAdd.Text = "追加";
            } else {
                numKeyLow.Value = mRegion.Header.KeyLo;
                numKeyHigh.Value = mRegion.Header.KeyHi;
                numVelocityLow.Value = mRegion.Header.VelLo;
                numVelocityHigh.Value = mRegion.Header.VelHi;
                numKeyLow.Enabled = false;
                numKeyHigh.Enabled = false;
                numVelocityLow.Enabled = false;
                numVelocityHigh.Enabled = false;

                var waveIndex = int.MaxValue;
                foreach (var art in mRegion.Art.Array) {
                    if (art.Type == ART_TYPE.WAVE_INDEX) {
                        waveIndex = (int)art.Value;
                        break;
                    }
                }

                var waveName = "";
                if (mFile.Wave.ContainsKey(waveIndex)) {
                    var wave = mFile.Wave[waveIndex];
                    waveName = wave.Info.Name;
                    btnEditWave.Enabled = true;
                } else {
                    btnEditWave.Enabled = false;
                }

                if (int.MaxValue == waveIndex) {
                    txtWave.Text = "";
                } else {
                    txtWave.Text = string.Format(
                        "{0} {1}",
                        waveIndex.ToString("0000"),
                        waveName
                    );
                }

                foreach(var art in mRegion.Art.Array) {
                    switch (art.Type) {
                    case ART_TYPE.GAIN_CONST:
                        numVolume.Value = (decimal)(art.Value * 100.0);
                        break;
                    case ART_TYPE.FINE_TUNE:
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
