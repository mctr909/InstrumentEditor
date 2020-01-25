using System;
using System.Drawing;
using System.Windows.Forms;

using Instruments;

namespace InstrumentEditor {
    public partial class RegionKeyAssignForm : Form {
        private File mFile;
        private Inst mInst;
        private bool mOnRange;
        private const int KEY_WIDTH = 10;

        private readonly string[] NOTE_NAME = new string[] {
            "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
        };

        public RegionKeyAssignForm(File file, Inst inst) {
            mFile = file;
            mInst = inst;
            InitializeComponent();
            SetTabSize();
            DispRegionInfo();
            timer1.Interval = 30;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void InstInfoForm_SizeChanged(object sender, EventArgs e) {
            SetTabSize();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            txtRegion.Text = "";
            if (mOnRange) {
                var posRegion = PosToRegion();
                txtRegion.Text = string.Format(
                    "強弱:{0} 音程:{1}({2}{3})",
                    posRegion.Y.ToString("000"),
                    posRegion.X.ToString("000"),
                    NOTE_NAME[posRegion.X % 12],
                    (posRegion.X / 12 - 2)
                );
            }
        }

        private void SetTabSize() {
            var offsetX = 28;
            var offsetY = 70;
            var width = Width - offsetX;
            var height = Height - offsetY;

            if (width < 100) {
                return;
            }

            if (height < 100) {
                return;
            }

            SetInstRegionSize();
        }

        private void SetInstRegionSize() {
            var offsetX = 16;
            var offsetY = 68;
            var width = Width - offsetX;
            var height = Height - offsetY;

            picRegion.Width = picRegion.BackgroundImage.Width;
            picRegion.Height = picRegion.BackgroundImage.Height;

            pnlRegion.Left = 0;
            pnlRegion.Top = toolStrip1.Height + 4;
            pnlRegion.Width = width;
            pnlRegion.Height = height;

            lstRegion.Left = 0;
            lstRegion.Top = toolStrip1.Height + 4;
            lstRegion.Width = width;
            lstRegion.Height = height;
        }

        #region 音程/強弱割り当て
        private void tsbAddRange_Click(object sender, EventArgs e) {
            AddRegion();
        }

        private void tsbDeleteRange_Click(object sender, EventArgs e) {
            DeleteRegion();
        }

        private void tsbRangeList_Click(object sender, EventArgs e) {
            tsbRangeKey.Checked = false;
            tsbRangeList.Checked = true;
            tsbAddRange.Enabled = true;
            tsbDeleteRange.Enabled = true;
            pnlRegion.Visible = false;
            lstRegion.Visible = true;
        }

        private void tsbRangeKey_Click(object sender, EventArgs e) {
            tsbAddRange.Enabled = false;
            tsbDeleteRange.Enabled = false;
            tsbRangeList.Checked = false;
            tsbRangeKey.Checked = true;
            lstRegion.Visible = false;
            pnlRegion.Visible = true;
        }

        private void lstRegion_DoubleClick(object sender, EventArgs e) {
            EditRegion(ListToRange());
        }

        private void picRegion_DoubleClick(object sender, EventArgs e) {
            EditRegion(PosToRange());
        }

        private void picRegion_MouseEnter(object sender, EventArgs e) {
            mOnRange = true;
        }

        private void picRegion_MouseLeave(object sender, EventArgs e) {
            mOnRange = false;
        }

        private void DispRegionInfo() {
            Text = mInst.Info.Name.Trim();

            var bmp = new Bitmap(picRegion.Width, picRegion.Height);
            var g = Graphics.FromImage(bmp);
            var blueLine = new Pen(Color.FromArgb(255, 0, 0, 255), 2.0f);
            var greenFill = new Pen(Color.FromArgb(64, 0, 255, 0), 1.0f).Brush;

            var idx = lstRegion.SelectedIndex;
            lstRegion.Items.Clear();

            foreach (var region in mInst.Region.Array) {
                var range = region.Header;
                g.FillRectangle(
                    greenFill,
                    range.KeyLo * KEY_WIDTH,
                    bmp.Height - (range.VelHi + 1) * 4 - 1,
                    (range.KeyHi - range.KeyLo + 1) * KEY_WIDTH,
                    (range.VelHi - range.VelLo + 1) * 4
                );
                g.DrawRectangle(
                    blueLine,
                    range.KeyLo * KEY_WIDTH,
                    bmp.Height - (range.VelHi + 1) * 4,
                    (range.KeyHi - range.KeyLo + 1) * KEY_WIDTH,
                    (range.VelHi - range.VelLo + 1) * 4
                );

                var waveIndex = int.MaxValue;
                foreach (var art in region.Art.Array) {
                    if (art.Type == ART_TYPE.WAVE_INDEX) {
                        waveIndex = (int)art.Value;
                        break;
                    }
                }

                var waveName = "";
                if (mFile.Wave.ContainsKey(waveIndex)) {
                    var wave = mFile.Wave[waveIndex];
                    waveName = wave.Info.Name;
                }

                var regionInfo = string.Format(
                    "音程 {0} {1}    強弱 {2} {3}",
                    region.Header.KeyLo.ToString("000"),
                    region.Header.KeyHi.ToString("000"),
                    region.Header.VelLo.ToString("000"),
                    region.Header.VelHi.ToString("000")
                );
                if (int.MaxValue != waveIndex) {
                    regionInfo = string.Format(
                        "{0}    波形 {1} {2}",
                        regionInfo,
                        waveIndex.ToString("0000"),
                        waveName
                    );
                }
                lstRegion.Items.Add(regionInfo);
            }

            if (null != picRegion.Image) {
                picRegion.Image.Dispose();
                picRegion.Image = null;
            }
            picRegion.Image = bmp;

            if (lstRegion.Items.Count <= idx) {
                idx = lstRegion.Items.Count - 1;
            }
            lstRegion.SelectedIndex = idx;
        }

        private void AddRegion() {
            var region = new Instruments.Region();
            region.Header.KeyLo = byte.MaxValue;
            var fm = new RegionInfoForm(mFile, region);
            fm.ShowDialog();

            if (byte.MaxValue != region.Header.KeyLo) {
                mInst.Region.Add(region);
                DispRegionInfo();
            }
        }

        private void EditRegion(RANGE range) {
            if (mInst.Region.ContainsKey(range)) {
                var region = mInst.Region.FindFirst(range);
                var fm = new RegionInfoForm(mFile, region);
                fm.ShowDialog();
                DispRegionInfo();
            } else {
                AddRegion();
            }
        }

        private void DeleteRegion() {
            var index = lstRegion.SelectedIndex;

            foreach (int idx in lstRegion.SelectedIndices) {
                var cols = lstRegion.Items[idx].ToString().Split(' ');
                var range = new RANGE {
                    KeyLo = byte.Parse(cols[1]),
                    KeyHi = byte.Parse(cols[2]),
                    VelLo = byte.Parse(cols[7]),
                    VelHi = byte.Parse(cols[8])
                };
                mInst.Region.Remove(range);
            }

            DispRegionInfo();

            if (index < lstRegion.Items.Count) {
                lstRegion.SelectedIndex = index;
            } else {
                lstRegion.SelectedIndex = lstRegion.Items.Count - 1;
            }
        }

        private Point PosToRegion() {
            var posRegion = picRegion.PointToClient(Cursor.Position);
            if (posRegion.X < 0) {
                posRegion.X = 0;
            }
            if (posRegion.Y < 0) {
                posRegion.Y = 0;
            }
            if (picRegion.Width <= posRegion.X) {
                posRegion.X = picRegion.Width - 1;
            }
            if (picRegion.Height <= posRegion.Y) {
                posRegion.Y = picRegion.Height - 1;
            }

            posRegion.Y = picRegion.Height - posRegion.Y - 1;
            posRegion.X = posRegion.X / KEY_WIDTH;
            posRegion.Y = (int)(posRegion.Y / 4.0);

            return posRegion;
        }

        private RANGE PosToRange() {
            var range = new RANGE();
            var posRegion = PosToRegion();
            foreach (var rgn in mInst.Region.Array) {
                if (rgn.Header.KeyLo <= posRegion.X && posRegion.X <= rgn.Header.KeyHi
                && rgn.Header.VelLo <= posRegion.Y && posRegion.Y <= rgn.Header.VelHi) {
                    range = rgn.Header;
                    break;
                }
            }
            return range;
        }

        private RANGE ListToRange() {
            if (lstRegion.SelectedIndex < 0) {
                return new RANGE();
            }
            var cols = lstRegion.Items[lstRegion.SelectedIndex].ToString().Split(' ');
            var region = new RANGE {
                KeyLo = byte.Parse(cols[1]),
                KeyHi = byte.Parse(cols[2]),
                VelLo = byte.Parse(cols[7]),
                VelHi = byte.Parse(cols[8])
            };
            return region;
        }
        #endregion
    }
}
