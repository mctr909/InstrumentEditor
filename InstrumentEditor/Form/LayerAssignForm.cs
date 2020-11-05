using System;
using System.Drawing;
using System.Windows.Forms;

using Instruments;

namespace InstrumentEditor {
    public partial class LayerAssignForm : Form {
        private File mFile;
        private Preset mPreset;
        private bool mOnRange;
        private const int KEY_WIDTH = 10;

        private readonly string[] NOTE_NAME = new string[] {
            "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
        };

        public LayerAssignForm(File file, Preset preset) {
            mFile = file;
            mPreset = preset;
            InitializeComponent();
            SetTabSize();
            DispLayerInfo();
            timer1.Interval = 30;
            timer1.Enabled = true;
            timer1.Start();
            tscLayer.Visible = false;
            txtLayer.Visible = false;
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

        private void tsbLayerList_Click(object sender, EventArgs e) {
            tsbRangeKey.Checked = false;
            tsbRangeList.Checked = true;
            tsbAddRange.Enabled = true;
            tsbDeleteRange.Enabled = true;
            pnlLayer.Visible = false;
            lstLayer.Visible = true;
            tscLayer.Visible = false;
            txtLayer.Visible = false;
        }

        private void tsbRangeKey_Click(object sender, EventArgs e) {
            tsbAddRange.Enabled = false;
            tsbDeleteRange.Enabled = false;
            tsbRangeList.Checked = false;
            tsbRangeKey.Checked = true;
            lstLayer.Visible = false;
            pnlLayer.Visible = true;
            tscLayer.Visible = true;
            txtLayer.Visible = true;
        }

        private void tscLayer_SelectedIndexChanged(object sender, EventArgs e) {
            DispLayerRanges();
        }

        private void lstLayer_DoubleClick(object sender, EventArgs e) {
            EditLayer(ListToRange());
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

            picLayer.Width = picLayer.BackgroundImage.Width;
            picLayer.Height = picLayer.BackgroundImage.Height;

            pnlLayer.Left = 0;
            pnlLayer.Top = toolStrip1.Height + 4;
            pnlLayer.Width = width;
            pnlLayer.Height = height;

            lstLayer.Left = 0;
            lstLayer.Top = toolStrip1.Height + 4;
            lstLayer.Width = width;
            lstLayer.Height = height;
        }

        private void DispLayerInfo() {
            Text = string.Format("レイヤー[{0}]", mPreset.Info.Name.Trim());

            var idx = lstLayer.SelectedIndex;
            lstLayer.Items.Clear();
            tscLayer.Items.Clear();

            foreach (var layer in mPreset.Layer.Array) {
                var range = layer.Header;
                var instIndex = int.MaxValue;
                foreach (var art in layer.Art.Array) {
                    if (art.Type == ART_TYPE.INST_INDEX) {
                        instIndex = (int)art.Value;
                        break;
                    }
                }

                var instName = "";
                if (instIndex < mFile.Inst.Count) {
                    var inst = mFile.Inst[instIndex];
                    instName = inst.Info.Name;
                }

                var regionInfo = string.Format(
                    "音程 {0} {1}    強弱 {2} {3}    {4}",
                    layer.Header.KeyLo.ToString("000"),
                    layer.Header.KeyHi.ToString("000"),
                    layer.Header.VelLo.ToString("000"),
                    layer.Header.VelHi.ToString("000"),
                    instName
                );
                if (int.MaxValue != instIndex) {
                    regionInfo = string.Format(
                        "{0}    波形 {1} {2}",
                        regionInfo,
                        instIndex.ToString("0000"),
                        instName
                    );
                }
                lstLayer.Items.Add(regionInfo);
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

            if (lstLayer.Items.Count <= idx) {
                idx = lstLayer.Items.Count - 1;
            }
            lstLayer.SelectedIndex = idx;
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
                bmp.Height - (velHi + 1) * 4 - 1,
                (keyHi - keyLo + 1) * KEY_WIDTH,
                (velHi - velLo + 1) * 4
            );
            g.DrawRectangle(
                blueLine,
                keyLo * KEY_WIDTH,
                bmp.Height - (velHi + 1) * 4,
                (keyHi - keyLo + 1) * KEY_WIDTH,
                (velHi - velLo + 1) * 4
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
            fm.StartPosition = FormStartPosition.CenterParent;
            fm.ShowDialog();
            if (byte.MaxValue != layer.Header.KeyLo) {
                mPreset.Layer.Add(layer);
                DispLayerInfo();
            }
        }

        private void EditLayer(LYRH range) {
            if (mPreset.Layer.ContainsKey(range)) {
                var fm = new LayerInfoDialog(mFile, mPreset.Layer.Find(range)[0]);
                fm.StartPosition = FormStartPosition.CenterParent;
                fm.ShowDialog();
                DispLayerInfo();
                DispLayerRanges();
            } else {
                AddLayer();
            }
        }

        private void DeleteLayer() {
            var index = lstLayer.SelectedIndex;

            foreach (int idx in lstLayer.SelectedIndices) {
                var cols = lstLayer.Items[idx].ToString().Split(' ');
                var select = new LYRH {
                    KeyLo = byte.Parse(cols[1]),
                    KeyHi = byte.Parse(cols[2]),
                    VelLo = byte.Parse(cols[7]),
                    VelHi = byte.Parse(cols[8])
                };
                var layerH = mPreset.Layer[idx].Header;
                if (select.KeyLo <= layerH.KeyLo && layerH.KeyHi <= select.KeyHi &&
                    select.VelLo <= layerH.VelLo && layerH.VelHi <= select.VelHi) {
                    mPreset.Layer.Remove(idx);
                }
            }

            DispLayerInfo();

            if (index < lstLayer.Items.Count) {
                lstLayer.SelectedIndex = index;
            } else {
                lstLayer.SelectedIndex = lstLayer.Items.Count - 1;
            }
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
            pos.Y = (int)(pos.Y / 4.0);

            return pos;
        }

        private LYRH PosToRange() {
            var range = new LYRH();
            var pos = LayerPos();
            foreach (var layer in mPreset.Layer.Array) {
                if (layer.Header.KeyLo <= pos.X && pos.X <= layer.Header.KeyHi &&
                    layer.Header.VelLo <= pos.Y && pos.Y <= layer.Header.VelHi) {
                    range = layer.Header;
                    break;
                }
            }
            return range;
        }

        private LYRH ListToRange() {
            if (lstLayer.SelectedIndex < 0) {
                return new LYRH();
            }
            var cols = lstLayer.Items[lstLayer.SelectedIndex].ToString().Split(' ');
            var region = new LYRH {
                KeyLo = byte.Parse(cols[1]),
                KeyHi = byte.Parse(cols[2]),
                VelLo = byte.Parse(cols[7]),
                VelHi = byte.Parse(cols[8])
            };
            return region;
        }
    }
}
