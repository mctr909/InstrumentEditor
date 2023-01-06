using System;
using System.Drawing;
using System.Windows.Forms;

using DLS;
using InstPack;

namespace InstrumentEditor {
    public partial class RegionAssignForm : Form {
        private Pack mFile;
        private INS mInst;
        private bool mOnRange;
        private const int KEY_WIDTH = 8;
        private const int VEL_HEIGHT = 4;

        public RegionAssignForm(Pack file, INS inst) {
            mFile = file;
            mInst = inst;
            InitializeComponent();
            DrawBackground();
            SetTabSize();
            DispRegionInfo();
            pnlRegion.Visible = true;
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
                    Const.NoteName[posRegion.X % 12],
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
        }

        #region 音程/強弱割り当て
        private void tsbAddRange_Click(object sender, EventArgs e) {
            AddRegion();
        }

        private void tsbDeleteRange_Click(object sender, EventArgs e) {
            DeleteRegion();
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
            Text = string.Format("リージョン[{0}]", mInst.Info[Info.TYPE.INAM].Trim());

            var bmp = new Bitmap(picRegion.Width, picRegion.Height);
            var g = Graphics.FromImage(bmp);
            var redLine = new Pen(Color.FromArgb(255, 0, 0, 255), 2.0f);
            var greenFill = new Pen(Color.FromArgb(64, 0, 255, 0), 1.0f).Brush;

            foreach (var region in mInst.Regions.Array) {
                var range = region.Header;
                g.FillRectangle(
                    greenFill,
                    range.Key.Lo * KEY_WIDTH,
                    bmp.Height - (range.Vel.Hi + 1) * VEL_HEIGHT - 1,
                    (range.Key.Hi - range.Key.Lo + 1) * KEY_WIDTH,
                    (range.Vel.Hi - range.Vel.Lo + 1) * VEL_HEIGHT
                );
                g.DrawRectangle(
                    redLine,
                    range.Key.Lo * KEY_WIDTH,
                    bmp.Height - (range.Vel.Hi + 1) * VEL_HEIGHT,
                    (range.Key.Hi - range.Key.Lo + 1) * KEY_WIDTH,
                    (range.Vel.Hi - range.Vel.Lo + 1) * VEL_HEIGHT
                );
            }

            if (null != picRegion.Image) {
                picRegion.Image.Dispose();
                picRegion.Image = null;
            }
            picRegion.Image = bmp;
        }

        private void AddRegion() {
            var region = new RGN();
            region.Header.Key.Lo = byte.MaxValue;
            var fm = new RegionInfoDialog(mFile, region);
            fm.ShowDialog();

            if (byte.MaxValue != region.Header.Key.Lo) {
                mInst.Regions.Add(region);
                DispRegionInfo();
            }
        }

        private void EditRegion(CK_RGNH range) {
            if (mInst.Regions.ContainsKey(range)) {
                var region = mInst.Regions.FindFirst(range);
                var fm = new RegionInfoDialog(mFile, region);
                fm.ShowDialog();
                DispRegionInfo();
            } else {
                AddRegion();
            }
        }

        private void DeleteRegion() {
            //mInst.Region.Remove(range);

            DispRegionInfo();
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

        private CK_RGNH PosToRange() {
            var range = new CK_RGNH();
            var posRegion = PosToRegion();
            foreach (var rgn in mInst.Regions.Array) {
                if (rgn.Header.Key.Lo <= posRegion.X && posRegion.X <= rgn.Header.Key.Hi &&
                    rgn.Header.Vel.Lo <= posRegion.Y && posRegion.Y <= rgn.Header.Vel.Hi) {
                    range = rgn.Header;
                    break;
                }
            }
            return range;
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
