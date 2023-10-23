using System;
using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
    public partial class RegionInfoDialog : Form {
        private File mFile;
        private RGN mRegion;

        private readonly string[] NoteName = new string[] {
            "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
        };

        public RegionInfoDialog(File file, RGN region) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

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
            var waveIndex = mRegion.WaveLink.TableIndex;

            var fm = new WaveSelectDialog(mFile, mRegion);
            fm.ShowDialog();

            if (waveIndex < mFile.Wave.Count) {
                var wave = mFile.Wave[(int)waveIndex];
                btnEditWave.Enabled = true;
                txtWave.Text = string.Format(
                    "{0} {1}",
                    waveIndex.ToString("0000"),
                    wave.Info[Info.TYPE.INAM]
                );
            } else {
                btnEditWave.Enabled = false;
                txtWave.Text = "";
            }
        }

        private void btnEditWave_Click(object sender, EventArgs e) {
            var fm = new WaveInfoForm(mFile, (int)mRegion.WaveLink.TableIndex);
            fm.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (byte.MaxValue == mRegion.Header.KeyLo) {
                mRegion.Header.KeyLo = (byte)numKeyLow.Value;
                mRegion.Header.KeyHi = (byte)numKeyHigh.Value;
                mRegion.Header.VelLo = (byte)numVelocityLow.Value;
                mRegion.Header.VelHi = (byte)numVelocityHigh.Value;
            }

            mRegion.Sampler.UnityNote = (ushort)numUnityNote.Value;
            mRegion.Sampler.FineTune = (short)numFineTune.Value;
            var gain = (int)(20 * numVolume.Value) / 400.0;
            mRegion.Sampler.Gain = (float)Math.Pow(10.0, gain);

            artList.SetList(mRegion.Articulations);

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

            artList.Top = grbVolume.Top + grbVolume.Height + 6;
            artList.Left = grbUnityNote.Left;

            artList.Art = mRegion.Articulations.List;

            chkLoop.Top = artList.Top + artList.Height + 6;

            btnAdd.Top = chkLoop.Top;
            btnAdd.Left = artList.Right - btnAdd.Width;

            Width = artList.Left + artList.Width + 24;
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
            if (byte.MaxValue == mRegion.Header.KeyHi) {
                mRegion.Header.KeyHi = mRegion.Header.KeyLo;
                numKeyLow.Value = mRegion.Header.KeyLo;
                numKeyHigh.Value = mRegion.Header.KeyHi;
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

                numUnityNote.Value = mRegion.Sampler.UnityNote;
                numFineTune.Value = mRegion.Sampler.FineTune;
                numVolume.Value = (decimal)(20 * Math.Log10(mRegion.Sampler.Gain));

                var waveIndex = (int)mRegion.WaveLink.TableIndex;

                var waveName = "";
                if (waveIndex < mFile.Wave.Count) {
                    var wave = mFile.Wave[waveIndex];
                    waveName = wave.Info[Info.TYPE.INAM];
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

                chkLoop.Checked = 0 < mRegion.Loops.Count;

                btnAdd.Text = "反映";
            }
            SetKeyLow();
            SetKeyHigh();
            SetVelocityLow();
            SetVelocityHigh();
        }
    }
}
