using System;
using System.Windows.Forms;

using InstPack;

namespace InstrumentEditor {
    public partial class WaveSelectDialog : Form {
        private Pack mFile;
        private Region mRegion;

        public WaveSelectDialog(Pack file, Region region) {
            InitializeComponent();
            mFile = file;
            mRegion = region;
        }

        private void WaveSelectDialog_Load(object sender, EventArgs e) {
            DispWaveList("");
            SetSize();
        }

        private void WaveSelectDialog_SizeChanged(object sender, EventArgs e) {
            SetSize();
        }

        private void lstWave_DoubleClick(object sender, EventArgs e) {
            if (0 == lstWave.Items.Count) {
                return;
            }
            var cols = lstWave.SelectedItem.ToString().Split('|');
            var idx = int.Parse(cols[0]);
            var fm = new WaveInfoForm(mFile, idx);
            var index = lstWave.SelectedIndex;
            fm.ShowDialog();
            DispWaveList(txtSearch.Text);
            lstWave.SelectedIndex = index;
        }

        private void btnSelect_Click(object sender, EventArgs e) {
            if (0 <= lstWave.SelectedIndex) {
                var cols = lstWave.SelectedItem.ToString().Split('|');
                mRegion.Art.Update(ART_TYPE.WAVE_INDEX, uint.Parse(cols[0]));
            }
            Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) {
            DispWaveList(txtSearch.Text);
        }

        private void SetSize() {
            var offsetX = 24;
            var offsetY = 48;

            btnSelect.Width = 60;
            btnSelect.Height = 24;

            lstWave.Left = 4;
            lstWave.Top = 4;
            lstWave.Width = Width - offsetX;
            lstWave.Height = Height - btnSelect.Height - 6 - offsetY;

            txtSearch.Top = lstWave.Top + lstWave.Height + 6;
            txtSearch.Left = lstWave.Left;
            txtSearch.Width = 200;

            btnSelect.Top = txtSearch.Top;
            btnSelect.Left = Width - btnSelect.Width - offsetX;
        }

        private void DispWaveList(string keyword) {
            lstWave.Items.Clear();
            int count = 0;
            for (var iWave = 0; iWave < mFile.Wave.Count; iWave++) {
                var wave = mFile.Wave[iWave];
                var name = "";
                if (null == wave.Info || string.IsNullOrWhiteSpace(wave.Info.Name)) {
                    name = string.Format("Wave[{0}]", count);
                } else {
                    name = wave.Info.Name;
                }

                if (!string.IsNullOrEmpty(keyword) && name.IndexOf(keyword) < 0) {
                    continue;
                }

                var use = false;
                foreach (var inst in mFile.Inst.ToArray()) {
                    foreach (var rgn in inst.Region.Array) {
                        foreach(var art in rgn.Art.ToArray()) {
                            if (art.Type != ART_TYPE.WAVE_INDEX) {
                                continue;
                            }
                            if (count == (int)art.Value) {
                                use = true;
                                break;
                            }
                        }
                    }
                }

                lstWave.Items.Add(string.Format(
                    "{0}\t{1}\t{2}\t{3}",
                    iWave.ToString("0000"),
                    use ? "use" : "   ",
                    0 < wave.Header.LoopEnable ? "loop" : "    ",
                    name
                ));
                ++count;
            }

            foreach (var art in mRegion.Art.ToArray()) {
                if (art.Type != ART_TYPE.WAVE_INDEX) {
                    continue;
                }
                if (art.Value < lstWave.Items.Count) {
                    lstWave.SelectedIndex = (int)art.Value;
                }
            }
        }
    }
}
