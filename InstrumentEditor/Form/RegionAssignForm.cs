using System;
using System.Drawing;
using System.Windows.Forms;

using InstPack;

namespace InstrumentEditor {
    public partial class RegionAssignForm : Form {
        private Pack mFile;
        private Inst mInst;
        private bool mOnRange;
        private const int KEY_WIDTH = 8;
        private const int VEL_HEIGHT = 4;

        private readonly string[] NOTE_NAME = new string[] {
            "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
        };

        public RegionAssignForm(Pack file, Inst inst) {
            mFile = file;
            mInst = inst;
            InitializeComponent();
            DrawBackground();
            SetTabSize();
            DispRegionInfo();
            timer1.Interval = 30;
            timer1.Enabled = true;
            timer1.Start();
            StartPosition = FormStartPosition.CenterParent;
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

            picRegion.Width = picRegion.Width;
            picRegion.Height = picRegion.Height;

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
            Text = string.Format("リージョン[{0}]", mInst.Info.Name.Trim());

            var bmp = new Bitmap(picRegion.Width, picRegion.Height);
            var g = Graphics.FromImage(bmp);
            var redLine = new Pen(Color.FromArgb(255, 0, 0, 255), 2.0f);
            var greenFill = new Pen(Color.FromArgb(64, 0, 255, 0), 1.0f).Brush;

            var idx = lstRegion.SelectedIndex;
            lstRegion.Items.Clear();

            foreach (var region in mInst.Region.Array) {
                var range = region.Header;
                g.FillRectangle(
                    greenFill,
                    range.KeyLo * KEY_WIDTH,
                    bmp.Height - (range.VelHi + 1) * VEL_HEIGHT - 1,
                    (range.KeyHi - range.KeyLo + 1) * KEY_WIDTH,
                    (range.VelHi - range.VelLo + 1) * VEL_HEIGHT
                );
                g.DrawRectangle(
                    redLine,
                    range.KeyLo * KEY_WIDTH,
                    bmp.Height - (range.VelHi + 1) * VEL_HEIGHT,
                    (range.KeyHi - range.KeyLo + 1) * KEY_WIDTH,
                    (range.VelHi - range.VelLo + 1) * VEL_HEIGHT
                );

                var waveIndex = int.MaxValue;
                foreach (var art in region.Art.ToArray()) {
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
            var region = new InstPack.Region();
            region.Header.KeyLo = byte.MaxValue;
            var fm = new RegionInfoDialog(mFile, region);
            fm.ShowDialog();

            if (byte.MaxValue != region.Header.KeyLo) {
                mInst.Region.Add(region);
                DispRegionInfo();
            }
        }

        private void EditRegion(RGNH range) {
            if (mInst.Region.ContainsKey(range)) {
                var region = mInst.Region.FindFirst(range);
                var fm = new RegionInfoDialog(mFile, region);
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
                var range = new RGNH {
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
            posRegion.Y = posRegion.Y / VEL_HEIGHT;

            return posRegion;
        }

        private RGNH PosToRange() {
            var range = new RGNH();
            var posRegion = PosToRegion();
            foreach (var rgn in mInst.Region.Array) {
                if (rgn.Header.KeyLo <= posRegion.X && posRegion.X <= rgn.Header.KeyHi &&
                    rgn.Header.VelLo <= posRegion.Y && posRegion.Y <= rgn.Header.VelHi) {
                    range = rgn.Header;
                    break;
                }
            }
            return range;
        }

        private RGNH ListToRange() {
            if (lstRegion.SelectedIndex < 0) {
                return new RGNH();
            }
            var cols = lstRegion.Items[lstRegion.SelectedIndex].ToString().Split(' ');
            var region = new RGNH {
                KeyLo = byte.Parse(cols[1]),
                KeyHi = byte.Parse(cols[2]),
                VelLo = byte.Parse(cols[7]),
                VelHi = byte.Parse(cols[8])
            };
            return region;
        }

        private void DrawBackground() {
            var bmp = new Bitmap(KEY_WIDTH * 128, VEL_HEIGHT * 128);
            var g = Graphics.FromImage(bmp);
            for (int k = 0; k < 128; k++) {
                switch (k % 12) {
                case 0:
                    g.DrawLine(Pens.Black, k * KEY_WIDTH, 0, k * KEY_WIDTH, VEL_HEIGHT * 128);
                    break;
                case 5:
                    g.DrawLine(Pens.LightSlateGray, k * KEY_WIDTH, 0, k * KEY_WIDTH, VEL_HEIGHT * 128);
                    break;
                case 2:
                case 4:
                case 7:
                case 9:
                case 11:
                    break;
                default:
                    g.FillRectangle(Brushes.Gray, k * KEY_WIDTH, 0, KEY_WIDTH, VEL_HEIGHT * 128);
                    break;
                }
            }
            for (int v = 0; v < 128; v += 16) {
                g.DrawLine(Pens.Black, 0, v * VEL_HEIGHT, KEY_WIDTH * 128, v * VEL_HEIGHT);
            }
            picRegion.BackgroundImage = bmp;
            picRegion.Width = bmp.Width;
            picRegion.Height = bmp.Height;
            Width = bmp.Width + 24;
            Height = bmp.Height + toolStrip1.Height + 48;
        }
        #endregion
    }
}
