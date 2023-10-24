using System;
using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
    public partial class WaveSelectDialog : Form {
        private File mFile;
        private RGN mRegion;

        public WaveSelectDialog(File file, RGN region) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
            mRegion = region;
        }

        private void WaveSelectDialog_Load(object sender, EventArgs e) {
            DispWaveList("");
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
                mRegion.WaveLink.TableIndex = uint.Parse(cols[0]);
            }
            Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) {
            DispWaveList(txtSearch.Text);
        }

        private void DispWaveList(string keyword) {
            lstWave.Items.Clear();
            int count = 0;
            for (uint iWave = 0; iWave < mFile.Wave.Count; iWave++) {
                var wave = mFile.Wave[(int)iWave];
                var name = "";
                if (string.IsNullOrWhiteSpace(wave.Info[Info.TYPE.INAM])) {
                    name = string.Format("Wave[{0}]", count);
                } else {
                    name = wave.Info[Info.TYPE.INAM];
                }

                if (!string.IsNullOrEmpty(keyword) && name.IndexOf(keyword) < 0) {
                    continue;
                }

                var use = false;
                foreach (var inst in mFile.Inst.List.Values) {
                    foreach (var rgn in inst.Regions.Array) {
                        if (count == rgn.WaveLink.TableIndex) {
                            use = true;
                            break;
                        }
                    }
                    if (use) {
                        break;
                    }
                }

                lstWave.Items.Add(string.Format(
                    "{0}|{1}|{2}|{3}|{4}",
                    iWave.ToString("0000"),
                    use ? "use" : "   ",
                    0 < wave.Loops.Count ? "loop" : "    ",
                    Const.NoteName[wave.Sampler.UnityNote % 12]
                        + (wave.Sampler.UnityNote / 12 - 2).ToString().PadLeft(2, ' '),
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
