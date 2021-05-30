using System;
using System.Drawing;
using System.Windows.Forms;

using InstPack;

namespace InstrumentEditor {
    public partial class LayerAssignForm : Form {
        private Pack mFile;
        private Preset mPreset;
        private bool mOnRange;
        private const int KEY_WIDTH = 8;
        private const int VEL_HEIGHT = 4;

        private readonly string[] NOTE_NAME = new string[] {
            "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
        };

        public LayerAssignForm(Pack file, Preset preset) {
            mFile = file;
            mPreset = preset;
            InitializeComponent();
            DrawBackground();
            SetTabSize();
            DispLayerInfo();
            timer1.Interval = 30;
            timer1.Enabled = true;
            timer1.Start();
            tscLayer.Visible = true;
            txtLayer.Visible = true;
            pnlLayer.Visible = true;
            StartPosition = FormStartPosition.CenterParent;
        }

        private void InstInfoForm_SizeChanged(object sender, EventArgs e) {
            SetTabSize();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            txtLayer.Text = "";
            if (mOnRange) {
                var pos = LayerPos();
                txtLayer.Text = string.Format(
                    "強弱:{0} 音程:{1}({2}{3})",
                    pos.Y.ToString("000"),
                    pos.X.ToString("000"),
                    NOTE_NAME[pos.X % 12],
                    (pos.X / 12 - 2)
                );
            }
        }

        #region 音程/強弱割り当て
        private void tsbAddLayer_Click(object sender, EventArgs e) {
            AddLayer();
        }

        private void tsbDeleteLayer_Click(object sender, EventArgs e) {
            DeleteLayer();
        }

        private void tscLayer_SelectedIndexChanged(object sender, EventArgs e) {
            DispLayerRanges();
        }

        private void picLayer_DoubleClick(object sender, EventArgs e) {
            EditLayer(PosToRange());
        }

        private void picLayer_MouseEnter(object sender, EventArgs e) {
            mOnRange = true;
        }

        private void picLayer_MouseLeave(object sender, EventArgs e) {
            mOnRange = false;
        }
        #endregion

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

            SetInstLayerSize();
        }

        private void SetInstLayerSize() {
            var offsetX = 16;
            var offsetY = 68;
            var width = Width - offsetX;
            var height = Height - offsetY;

            picLayer.Width = picLayer.Width;
            picLayer.Height = picLayer.Height;

            pnlLayer.Left = 0;
            pnlLayer.Top = toolStrip1.Height + 4;
            pnlLayer.Width = width;
            pnlLayer.Height = height;
        }

        private void DispLayerInfo() {
            Text = string.Format("レイヤー[{0}]", mPreset.Info.Name.Trim());

            tscLayer.Items.Clear();

            foreach (var layer in mPreset.Layer.ToArray()) {
                var instIndex = layer.Header.InstIndex;
                var instName = "";
                if (instIndex < mFile.Inst.Count) {
                    var inst = mFile.Inst[instIndex];
                    instName = inst.Info.Name;
                }

                tscLayer.Items.Add(string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                    layer.Header.KeyLo.ToString("000"),
                    layer.Header.KeyHi.ToString("000"),
                    layer.Header.VelLo.ToString("000"),
                    layer.Header.VelHi.ToString("000"),
                    (int.MaxValue == instIndex) ? "    " : instIndex.ToString("0000"),
                    instName
                ));
            }

            tscLayer.SelectedIndex = 0 < tscLayer.Items.Count ? 0 : -1;
        }

        private void DispLayerRanges() {
            var bmp = new Bitmap(picLayer.Width, picLayer.Height);
            var g = Graphics.FromImage(bmp);
            var blueLine = new Pen(Color.FromArgb(255, 0, 0, 255), 2.0f);
            var greenFill = new Pen(Color.FromArgb(64, 0, 255, 0), 1.0f).Brush;

            var cols = ((string)tscLayer.SelectedItem).Split('|');
            var keyLo = int.Parse(cols[0]);
            var keyHi = int.Parse(cols[1]);
            var velLo = int.Parse(cols[2]);
            var velHi = int.Parse(cols[3]);

            g.FillRectangle(
                greenFill,
                keyLo * KEY_WIDTH,
                bmp.Height - (velHi + 1) * VEL_HEIGHT - 1,
                (keyHi - keyLo + 1) * KEY_WIDTH,
                (velHi - velLo + 1) * VEL_HEIGHT
            );
            g.DrawRectangle(
                blueLine,
                keyLo * KEY_WIDTH,
                bmp.Height - (velHi + 1) * VEL_HEIGHT,
                (keyHi - keyLo + 1) * KEY_WIDTH,
                (velHi - velLo + 1) * VEL_HEIGHT
            );

            if (null != picLayer.Image) {
                picLayer.Image.Dispose();
                picLayer.Image = null;
            }
            picLayer.Image = bmp;
        }

        private void AddLayer() {
            var layer = new Layer();
            layer.Header.KeyLo = byte.MaxValue;
            var fm = new LayerInfoDialog(mFile, layer);
            fm.ShowDialog();
            if (byte.MaxValue != layer.Header.KeyLo) {
                mPreset.Layer.Add(layer);
                DispLayerInfo();
            }
        }

        private void EditLayer(LYRH range) {
            if (mPreset.Layer.ContainsKey(range)) {
                var fm = new LayerInfoDialog(mFile, mPreset.Layer.Find(range)[0]);
                fm.ShowDialog();
                DispLayerInfo();
                DispLayerRanges();
            } else {
                AddLayer();
            }
        }

        private void DeleteLayer() {
            //mPreset.Layer.Remove(idx);
            DispLayerInfo();
        }

        private Point LayerPos() {
            var pos = picLayer.PointToClient(Cursor.Position);
            if (pos.X < 0) {
                pos.X = 0;
            }
            if (pos.Y < 0) {
                pos.Y = 0;
            }
            if (picLayer.Width <= pos.X) {
                pos.X = picLayer.Width - 1;
            }
            if (picLayer.Height <= pos.Y) {
                pos.Y = picLayer.Height - 1;
            }

            pos.Y = picLayer.Height - pos.Y - 1;
            pos.X = pos.X / KEY_WIDTH;
            pos.Y = pos.Y / VEL_HEIGHT;

            return pos;
        }

        private LYRH PosToRange() {
            var range = new LYRH();
            var pos = LayerPos();
            foreach (var layer in mPreset.Layer.ToArray()) {
                if (layer.Header.KeyLo <= pos.X && pos.X <= layer.Header.KeyHi &&
                    layer.Header.VelLo <= pos.Y && pos.Y <= layer.Header.VelHi) {
                    range = layer.Header;
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
            picLayer.BackgroundImage = bmp;
            picLayer.Width = bmp.Width;
            picLayer.Height = bmp.Height;
            Width = bmp.Width + 24;
            Height = bmp.Height + toolStrip1.Height + 48;
        }
    }
}
