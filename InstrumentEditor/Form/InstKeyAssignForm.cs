using System;
using System.Drawing;
using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
    public partial class InstKeyAssignForm : Form {
        private DLS.DLS mDLS;
        private INS mINS;
        private bool mOnRange;
        private const int KEY_WIDTH = 10;

        private readonly string[] NOTE_NAME = new string[] {
            "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
        };

        public InstKeyAssignForm(DLS.DLS dls, INS ins) {
            mDLS = dls;
            mINS = ins;
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
            EditRegion(ListToRegeonId());
        }

        private void picRegion_DoubleClick(object sender, EventArgs e) {
            EditRegion(PosToRegionId());
        }

        private void picRegion_MouseEnter(object sender, EventArgs e) {
            mOnRange = true;
        }

        private void picRegion_MouseLeave(object sender, EventArgs e) {
            mOnRange = false;
        }

        private void DispRegionInfo() {
            Text = mINS.Info.Name.Trim();

            var bmp = new Bitmap(picRegion.Width, picRegion.Height);
            var g = Graphics.FromImage(bmp);
            var blueLine = new Pen(Color.FromArgb(255, 0, 0, 255), 2.0f);
            var greenFill = new Pen(Color.FromArgb(64, 0, 255, 0), 1.0f).Brush;

            var idx = lstRegion.SelectedIndex;
            lstRegion.Items.Clear();

            foreach (var region in mINS.Regions.List.Values) {
                var key = region.Header.Key;
                var vel = region.Header.Velocity;
                g.FillRectangle(
                    greenFill,
                    key.Low * KEY_WIDTH,
                    bmp.Height - (vel.High + 1) * 4 - 1,
                    (key.High - key.Low + 1) * KEY_WIDTH,
                    (vel.High - vel.Low + 1) * 4
                );
                g.DrawRectangle(
                    blueLine,
                    key.Low * KEY_WIDTH,
                    bmp.Height - (vel.High + 1) * 4,
                    (key.High - key.Low + 1) * KEY_WIDTH,
                    (vel.High - vel.Low + 1) * 4
                );
                var waveName = "";
                if (mDLS.WavePool.List.ContainsKey((int)region.WaveLink.TableIndex)) {
                    var wave = mDLS.WavePool.List[(int)region.WaveLink.TableIndex];
                    waveName = wave.Info.Name;
                }

                var regionInfo = string.Format(
                    "音程 {0} {1}    強弱 {2} {3}",
                    region.Header.Key.Low.ToString("000"),
                    region.Header.Key.High.ToString("000"),
                    region.Header.Velocity.Low.ToString("000"),
                    region.Header.Velocity.High.ToString("000")
                );
                if (uint.MaxValue != region.WaveLink.TableIndex) {
                    regionInfo = string.Format(
                        "{0}    波形 {1} {2}",
                        regionInfo,
                        region.WaveLink.TableIndex.ToString("0000"),
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
            var region = new RGN();
            region.Header.Key.Low = ushort.MaxValue;
            region.WaveLink.TableIndex = uint.MaxValue;
            var fm = new RegionInfoForm(mDLS, region);
            fm.ShowDialog();

            if (ushort.MaxValue != region.Header.Key.Low) {
                mINS.Regions.List.Add(region.Header, region);
                DispRegionInfo();
            }
        }

        private void EditRegion(CK_RGNH regionId) {
            if (mINS.Regions.List.ContainsKey(regionId)) {
                var region = mINS.Regions.List[regionId];
                var fm = new RegionInfoForm(mDLS, region);
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

                var rgn = new CK_RGNH();
                rgn.Key.Low = byte.Parse(cols[1]);
                rgn.Key.High = byte.Parse(cols[2]);
                rgn.Velocity.Low = byte.Parse(cols[7]);
                rgn.Velocity.High = byte.Parse(cols[8]);

                if (mINS.Regions.List.ContainsKey(rgn)) {
                    mINS.Regions.List.Remove(rgn);
                }
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

        private CK_RGNH PosToRegionId() {
            var region = new CK_RGNH();
            var posRegion = PosToRegion();
            foreach (var rgn in mINS.Regions.List.Values) {
                var key = rgn.Header.Key;
                var vel = rgn.Header.Velocity;
                if (key.Low <= posRegion.X && posRegion.X <= key.High
                && vel.Low <= posRegion.Y && posRegion.Y <= vel.High) {
                    region = rgn.Header;
                    break;
                }
            }

            return region;
        }

        private CK_RGNH ListToRegeonId() {
            if (lstRegion.SelectedIndex < 0) {
                return new CK_RGNH();
            }

            var cols = lstRegion.Items[lstRegion.SelectedIndex].ToString().Split(' ');
            var region = new CK_RGNH();
            region.Key.Low = ushort.Parse(cols[1]);
            region.Key.High = ushort.Parse(cols[2]);
            region.Velocity.Low = ushort.Parse(cols[7]);
            region.Velocity.High = ushort.Parse(cols[8]);

            return region;
        }
        #endregion
    }
}
