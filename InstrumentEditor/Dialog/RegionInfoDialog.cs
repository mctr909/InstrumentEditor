using System;
using System.Windows.Forms;
using DLS;
using InstPack;

namespace InstrumentEditor {
    public partial class RegionInfoDialog : Form {
        private Pack mFile;
        private RGN mRegion;

        private readonly string[] NoteName = new string[] {
            "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
        };

        public RegionInfoDialog(Pack file, RGN region) {
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

            if (waveIndex < mFile.Wave.List.Count) {
                var wave = mFile.Wave.List[(int)waveIndex];
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
            if (byte.MaxValue == mRegion.Header.Key.Lo) {
                mRegion.Header.Key.Lo = (byte)numKeyLow.Value;
                mRegion.Header.Key.Hi = (byte)numKeyHigh.Value;
                mRegion.Header.Vel.Lo = (byte)numVelocityLow.Value;
                mRegion.Header.Vel.Hi = (byte)numVelocityHigh.Value;
            }
            ///TODO:ART
            //if (0 == numUnityNote.Value) {
            //    mRegion.Art.Delete(ART_TYPE.UNITY_KEY);
            //} else {
            //    mRegion.Art.Update(ART_TYPE.UNITY_KEY, (int)numUnityNote.Value);
            //}
            //var fineTune = (int)(1200 * numFineTune.Value) / 1440000.0;
            //if (0 == fineTune) {
            //    mRegion.Art.Delete(ART_TYPE.FINE_TUNE);
            //} else {
            //    mRegion.Art.Update(ART_TYPE.FINE_TUNE, (float)Math.Pow(2.0, fineTune));
            //}
            //var gain = (int)(20 * numVolume.Value) / 400.0;
            //if (0 == gain) {
            //    mRegion.Art.Delete(ART_TYPE.GAIN);
            //} else {
            //    mRegion.Art.Update(ART_TYPE.GAIN, (float)Math.Pow(10.0, gain));
            //}

            //envelope1.SetList(mRegion.Art);

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

            ///TODO:ART
            //envelope1.Art = mRegion.Art;

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
            if (byte.MaxValue == mRegion.Header.Key.Lo) {
                numKeyLow.Value = 63;
                numKeyHigh.Value = 63;
                numVelocityLow.Value = 0;
                numVelocityHigh.Value = 127;
                btnEditWave.Enabled = false;

                btnAdd.Text = "追加";
            } else {
                numKeyLow.Value = mRegion.Header.Key.Lo;
                numKeyHigh.Value = mRegion.Header.Key.Hi;
                numVelocityLow.Value = mRegion.Header.Vel.Lo;
                numVelocityHigh.Value = mRegion.Header.Vel.Hi;
                numKeyLow.Enabled = false;
                numKeyHigh.Enabled = false;
                numVelocityLow.Enabled = false;
                numVelocityHigh.Enabled = false;

                var waveIndex = (int)mRegion.WaveLink.TableIndex;

                var waveName = "";
                if (waveIndex < mFile.Wave.List.Count) {
                    var wave = mFile.Wave.List[waveIndex];
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

                ///TODO:ART
                //foreach(var art in mRegion.Art.ToArray()) {
                //    switch (art.Destination) {
                //    case DST_TYPE.GAIN:
                //        numVolume.Value = (decimal)(20 * Math.Log10(art.Value));
                //        break;
                //    case ART_TYPE.FINE_TUNE:
                //        if (1.0 == art.Value) {
                //            numFineTune.Value = 0;
                //        } else {
                //            numFineTune.Value = (int)(1200.0 / Math.Log(2.0, art.Value));
                //        }
                //        break;
                //    case ART_TYPE.UNITY_KEY:
                //        numUnityNote.Value = (int)art.Value;
                //        break;
                //    }
                //}

                btnAdd.Text = "反映";
            }
            SetKeyLow();
            SetKeyHigh();
            SetVelocityLow();
            SetVelocityHigh();
        }
    }
}
