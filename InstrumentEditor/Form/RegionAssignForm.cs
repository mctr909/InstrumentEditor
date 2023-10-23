using System;
using System.Drawing;
using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
    public partial class RegionAssignForm : Form {
        private File mFile;
        private INS mInst;
        private bool mOnRange;
        private const int KEY_WIDTH = 6;
        private const int VEL_HEIGHT = 4;

        public RegionAssignForm(File file, INS inst) {
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
                var posKeyVel = PosToKeyVel();
                txtRegion.Text = string.Format(
                    "強弱:{0} 音程:{1}({2}{3})",
                    posKeyVel.Y.ToString("000"),
                    posKeyVel.X.ToString("000"),
                    Const.NoteName[posKeyVel.X % 12],
                    (posKeyVel.X / 12 - 2)
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
            tsbAddRange.Checked = true;
            tsbDeleteRange.Checked = false;
        }

        private void tsbDeleteRange_Click(object sender, EventArgs e) {
            tsbAddRange.Checked = false;
            tsbDeleteRange.Checked = true;
        }

        private void picRegion_DoubleClick(object sender, EventArgs e) {
            EditRegion(PosToRgnh());
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
            var redLine = new Pen(Color.FromArgb(255, 0, 0, 255), 1.0f);
            var greenFill = new Pen(Color.FromArgb(64, 0, 255, 0), 1.0f).Brush;

            foreach (var region in mInst.Regions.Array) {
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
                    bmp.Height - (range.VelHi + 1) * VEL_HEIGHT - 1,
                    (range.KeyHi - range.KeyLo + 1) * KEY_WIDTH,
                    (range.VelHi - range.VelLo + 1) * VEL_HEIGHT
                );
            }

            if (null != picRegion.Image) {
                picRegion.Image.Dispose();
                picRegion.Image = null;
            }
            picRegion.Image = bmp;
        }

        private void EditRegion(RGN.HEADER range) {
            if (tsbDeleteRange.Checked) {
                mInst.Regions.Remove(range);
                DispRegionInfo();
            } else {
                if (mInst.Regions.ContainsKey(range)) {
                    var rgn = mInst.Regions.FindFirst(range);
                    var fm = new RegionInfoDialog(mFile, rgn);
                    fm.ShowDialog();
                    DispRegionInfo();
                } else {
                    var pos = PosToKeyVel();
                    var rgn = new RGN();
                    rgn.Header.KeyLo = (byte)pos.X;
                    rgn.Header.KeyHi = byte.MaxValue;
                    var fm = new RegionInfoDialog(mFile, rgn);
                    fm.ShowDialog();
                    if (byte.MaxValue != rgn.Header.KeyLo) {
                        mInst.Regions.Add(rgn);
                        DispRegionInfo();
                    }
                }
            }
        }

        private Point PosToKeyVel() {
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

        private RGN.HEADER PosToRgnh() {
            var range = new RGN.HEADER();
            var posKeyVel = PosToKeyVel();
            foreach (var rgn in mInst.Regions.Array) {
                if (rgn.Header.KeyLo <= posKeyVel.X && posKeyVel.X <= rgn.Header.KeyHi &&
                    rgn.Header.VelLo <= posKeyVel.Y && posKeyVel.Y <= rgn.Header.VelHi) {
                    range = rgn.Header;
                    break;
                }
            }
            return range;
        }

        private void DrawBackground() {
            var bmp = new Bitmap(KEY_WIDTH * 128 + 1, VEL_HEIGHT * 128 + 1);
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
