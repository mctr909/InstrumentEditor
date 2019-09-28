using System;
using System.Windows.Forms;

namespace InstrumentEditor {
    public partial class WaveSelectForm : Form {
        private DLS.DLS mDLS;
        private DLS.RGN mRegion;

        public WaveSelectForm(DLS.DLS dls, DLS.RGN region) {
            InitializeComponent();

            mDLS = dls;
            mRegion = region;
        }

        private void WaveSelectForm_Load(object sender, EventArgs e) {
            DispWaveList("");
            SetSize();
        }

        private void WaveSelectForm_SizeChanged(object sender, EventArgs e) {
            SetSize();
        }

        private void lstWave_DoubleClick(object sender, EventArgs e) {
            if (0 == lstWave.Items.Count) {
                return;
            }

            var cols = lstWave.SelectedItem.ToString().Split('\t');
            var idx = int.Parse(cols[0]);
            var fm = new WaveInfoForm(mDLS, idx);
            var index = lstWave.SelectedIndex;
            fm.ShowDialog();
            DispWaveList(txtSearch.Text);
            lstWave.SelectedIndex = index;
        }

        private void btnSelect_Click(object sender, EventArgs e) {
            if (0 <= lstWave.SelectedIndex) {
                var cols = lstWave.SelectedItem.ToString().Split('\t');
                mRegion.WaveLink.TableIndex = uint.Parse(cols[0]);
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
            foreach (var wave in mDLS.WavePool.List) {
                var name = "";
                if (null == wave.Value.Info || string.IsNullOrWhiteSpace(wave.Value.Info.Name)) {
                    name = string.Format("Wave[{0}]", count);
                } else {
                    name = wave.Value.Info.Name;
                }

                if (!string.IsNullOrEmpty(keyword) && name.IndexOf(keyword) < 0) {
                    continue;
                }

                var use = false;
                foreach (var inst in mDLS.Instruments.List.Values) {
                    foreach (var rgn in inst.Regions.List.Values) {
                        if (count == rgn.WaveLink.TableIndex) {
                            use = true;
                            break;
                        }
                    }
                }

                lstWave.Items.Add(string.Format(
                    "{0}\t{1}\t{2}\t{3}",
                    wave.Key.ToString("0000"),
                    (use ? "use" : "   "),
                    (0 < wave.Value.Sampler.LoopCount ? "loop" : "    "),
                    name
                ));
                ++count;
            }

            if (mRegion.WaveLink.TableIndex < lstWave.Items.Count) {
                lstWave.SelectedIndex = (int)mRegion.WaveLink.TableIndex;
            }
        }
    }
}
