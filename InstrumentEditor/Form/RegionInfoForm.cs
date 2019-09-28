using System;
using System.Windows.Forms;

namespace InstrumentEditor {
    public partial class RegionInfoForm : Form {
        private DLS.DLS mDLS;
        private DLS.RGN mRegion;
        private DLS.CK_RGNH mHeader;
        private DLS.CK_WSMP mSampler;
        private DLS.CK_WLNK mWaveLink;
        private DLS.ART mArt;

        private readonly string[] NoteName = new string[] {
            "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
        };

        public RegionInfoForm(DLS.DLS dls, DLS.RGN region) {
            InitializeComponent();

            mDLS = dls;
            mRegion = region;
            mHeader = region.Header;
            mSampler = region.Sampler;
            mWaveLink = region.WaveLink;

            if (null != mRegion.Articulations && null != mRegion.Articulations.ART) {
                mArt = new DLS.ART();
                foreach (var art in mRegion.Articulations.ART.List) {
                    mArt.List.Add(art.Key, art.Value);
                }
            }

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
            var ret = new DLS.RGN();
            ret.WaveLink = mWaveLink;
            var fm = new WaveSelectForm(mDLS, ret);
            fm.ShowDialog();

            mWaveLink = ret.WaveLink;

            if (mDLS.WavePool.List.ContainsKey((int)mWaveLink.TableIndex)) {
                var wave = mDLS.WavePool.List[(int)mWaveLink.TableIndex];
                btnEditWave.Enabled = true;
                txtWave.Text = string.Format(
                    "{0} {1}",
                    mWaveLink.TableIndex.ToString("0000"),
                    wave.Info.Name
                );
            }
            else {
                btnEditWave.Enabled = false;
                txtWave.Text = "";
            }
        }

        private void btnEditWave_Click(object sender, EventArgs e) {
            var fm = new WaveInfoForm(mDLS, (int)mWaveLink.TableIndex);
            fm.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (ushort.MaxValue == mHeader.Key.Low) {
                mHeader.Key.Low = (ushort)numKeyLow.Value;
                mHeader.Key.High = (ushort)numKeyHigh.Value;
                mHeader.Velocity.Low = (ushort)numVelocityLow.Value;
                mHeader.Velocity.High = (ushort)numVelocityHigh.Value;
            }

            if (!chkLoop.Checked) {
                mSampler.LoopCount = 0;
            }

            mSampler.UnityNote = (ushort)numUnityNote.Value;
            mSampler.FineTune = (short)numFineTune.Value;
            mSampler.Gain = (double)numVolume.Value / 100.0;

            mRegion.Header = mHeader;
            mRegion.Sampler = mSampler;
            mRegion.WaveLink = mWaveLink;

            if (null != envelope1.Art && null != mRegion.Articulations) {
                if (null == mRegion.Articulations.ART) {
                    mRegion.Articulations.ART = new DLS.ART();
                }

                mArt.List.Clear();
                envelope1.SetList(mArt.List);

                mRegion.Articulations.ART.List.Clear();
                foreach (var art in mArt.List) {
                    mRegion.Articulations.ART.List.Add(art.Key, art.Value);
                }
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

            if (null == mArt) {
                envelope1.Visible = false;

                chkLoop.Top = grbVolume.Top + grbVolume.Height + 6;

                btnAdd.Top = chkLoop.Top;
                btnAdd.Left = grbWave.Right - btnAdd.Width;

                Width = grbWave.Left + grbWave.Width + 24;
                Height = btnAdd.Top + btnAdd.Height + 48;
            }
            else {
                envelope1.Top = grbVolume.Top + grbVolume.Height + 6;
                envelope1.Left = grbUnityNote.Left;

                envelope1.Art = mArt;

                chkLoop.Top = envelope1.Top + envelope1.Height + 6;

                btnAdd.Top = chkLoop.Top;
                btnAdd.Left = envelope1.Right - btnAdd.Width;

                Width = envelope1.Left + envelope1.Width + 24;
                Height = btnAdd.Top + btnAdd.Height + 48;
            }
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
            if (ushort.MaxValue == mHeader.Key.Low) {
                numKeyLow.Value = 63;
                numKeyHigh.Value = 63;
                numVelocityLow.Value = 0;
                numVelocityHigh.Value = 127;
                btnEditWave.Enabled = false;

                btnAdd.Text = "追加";
            }
            else {
                numKeyLow.Value = mHeader.Key.Low;
                numKeyHigh.Value = mHeader.Key.High;
                numVelocityLow.Value = mHeader.Velocity.Low;
                numVelocityHigh.Value = mHeader.Velocity.High;
                numKeyLow.Enabled = false;
                numKeyHigh.Enabled = false;
                numVelocityLow.Enabled = false;
                numVelocityHigh.Enabled = false;

                var waveName = "";
                if (mDLS.WavePool.List.ContainsKey((int)mWaveLink.TableIndex)) {
                    var wave = mDLS.WavePool.List[(int)mWaveLink.TableIndex];
                    waveName = wave.Info.Name;
                    btnEditWave.Enabled = true;
                }
                else {
                    btnEditWave.Enabled = false;
                }

                if (uint.MaxValue == mWaveLink.TableIndex) {
                    txtWave.Text = "";
                }
                else {
                    txtWave.Text = string.Format(
                        "{0} {1}",
                        mWaveLink.TableIndex.ToString("0000"),
                        waveName
                    );
                }

                if (0 < mSampler.LoopCount) {
                    chkLoop.Checked = true;
                }
                else {
                    chkLoop.Checked = false;
                }

                numUnityNote.Value = mSampler.UnityNote;
                numFineTune.Value = mSampler.FineTune;
                numVolume.Value = (decimal)(mSampler.Gain * 100.0);

                btnAdd.Text = "反映";
            }
            SetKeyLow();
            SetKeyHigh();
            SetVelocityLow();
            SetVelocityHigh();
        }
    }
}