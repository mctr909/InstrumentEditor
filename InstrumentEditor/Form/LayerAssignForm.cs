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
            DispRegionInfo();
            timer1.Interval = 30;
            timer1.Enabled = true;
            timer1.Start();
            tscLayer.Visible = false;
            txtRegion.Visible = false;
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
            tscLayer.Visible = false;
            txtRegion.Visible = false;
        }

        private void tsbRangeKey_Click(object sender, EventArgs e) {
            tsbAddRange.Enabled = false;
            tsbDeleteRange.Enabled = false;
            tsbRangeList.Checked = false;
            tsbRangeKey.Checked = true;
            lstRegion.Visible = false;
            pnlRegion.Visible = true;
            tscLayer.Visible = true;
            txtRegion.Visible = true;
        }

        private void tscLayer_SelectedIndexChanged(object sender, EventArgs e) {
            var bmp = new Bitmap(picRegion.Width, picRegion.Height);
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

            if (null != picRegion.Image) {
                picRegion.Image.Dispose();
                picRegion.Image = null;
            }
            picRegion.Image = bmp;
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
            Text = mPreset.Info.Name.Trim();

            var idx = lstRegion.SelectedIndex;
            lstRegion.Items.Clear();

            foreach (var layer in mPreset.Layer.Array) {
                var range = layer.Header;
                var waveIndex = int.MaxValue;
                foreach (var art in layer.Art.Array) {
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

                var instIndex = -1;
                var instName = "";
                foreach (var art in layer.Art.Array) {
                    if (art.Type == ART_TYPE.INST_INDEX) {
                        instIndex = (int)art.Value;
                        instName = mFile.Inst[instIndex].Info.Name;
                        break;
                    }
                }

                var regionInfo = string.Format(
                    "音程 {0} {1}    強弱 {2} {3}    {4}",
                    layer.Header.KeyLo.ToString("000"),
                    layer.Header.KeyHi.ToString("000"),
                    layer.Header.VelLo.ToString("000"),
                    layer.Header.VelHi.ToString("000"),
                    instName
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
                tscLayer.Items.Add(string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                    layer.Header.KeyLo.ToString("000"),
                    layer.Header.KeyHi.ToString("000"),
                    layer.Header.VelLo.ToString("000"),
                    layer.Header.VelHi.ToString("000"),
                    instIndex.ToString("0000"),
                    instName
                ));
            }

            tscLayer.SelectedIndex = 0 < tscLayer.Items.Count ? 0 : -1;

            if (lstRegion.Items.Count <= idx) {
                idx = lstRegion.Items.Count - 1;
            }
            lstRegion.SelectedIndex = idx;
        }

        private void AddRegion() {
            var layer = new Layer();
            layer.Header.KeyLo = byte.MaxValue;
            //FORM//var fm = new RegionInfoForm(mFile, layer);
            //FORM//fm.ShowDialog();

            if (byte.MaxValue != layer.Header.KeyLo) {
                mPreset.Layer.Add(layer);
                DispRegionInfo();
            }
        }

        private void EditRegion(LYRH range) {
            if (mPreset.Layer.ContainsKey(range)) {
                var region = mPreset.Layer.Find(range);
                //FORM//var fm = new RegionInfoForm(mFile, region);
                //FORM//fm.ShowDialog();
                DispRegionInfo();
            } else {
                AddRegion();
            }
        }

        private void DeleteRegion() {
            var index = lstRegion.SelectedIndex;

            foreach (int idx in lstRegion.SelectedIndices) {
                var cols = lstRegion.Items[idx].ToString().Split(' ');
                var range = new LYRH {
                    KeyLo = byte.Parse(cols[1]),
                    KeyHi = byte.Parse(cols[2]),
                    VelLo = byte.Parse(cols[7]),
                    VelHi = byte.Parse(cols[8])
                };
                //FORM//mPreset.Layer.Remove(range);
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

        private LYRH PosToRange() {
            var range = new LYRH();
            var posRegion = PosToRegion();
            foreach (var layer in mPreset.Layer.Array) {
                if (layer.Header.KeyLo <= posRegion.X && posRegion.X <= layer.Header.KeyHi &&
                    layer.Header.VelLo <= posRegion.Y && posRegion.Y <= layer.Header.VelHi) {
                    range = layer.Header;
                    break;
                }
            }
            return range;
        }

        private LYRH ListToRange() {
            if (lstRegion.SelectedIndex < 0) {
                return new LYRH();
            }
            var cols = lstRegion.Items[lstRegion.SelectedIndex].ToString().Split(' ');
            var region = new LYRH {
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
