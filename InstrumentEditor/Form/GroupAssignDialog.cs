using System;
using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
    public partial class GroupAssignDialog : Form {
        File mFile;
        Riff mRiff;

        public GroupAssignDialog(File file, Riff riff) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
            mRiff = riff;
            SetGroupList();
        }

        private void cmbGroup_Leave(object sender, EventArgs e) {
            SetGroupList();
        }

        private void btnApply_Click(object sender, EventArgs e) {
            mRiff.Info[Info.TYPE.ICAT] = cmbGroup.Text;
            Close();
        }

        void SetGroupList() {
            var tmpGroup = cmbGroup.SelectedText;
            cmbGroup.Items.Clear();
            if (!string.IsNullOrWhiteSpace(tmpGroup)) {
                cmbGroup.Items.Add(tmpGroup);
            }
            if (mRiff.GetType() == typeof(INS)) {
                foreach (var preset in mFile.Inst.List.Values) {
                    var cat = preset.Info[Info.TYPE.ICAT];
                    if ("" != cat) {
                        if (!cmbGroup.Items.Contains(cat.Trim())) {
                            cmbGroup.Items.Add(cat.Trim());
                        }
                    }
                }
            }
            if (mRiff.GetType() == typeof(WAVE)) {
                for (uint iWave = 0; iWave < mFile.Wave.Count; iWave++) {
                    var wave = mFile.Wave[(int)iWave];
                    var cat = wave.Info[Info.TYPE.ICAT];
                    if ("" != cat) {
                        if (!cmbGroup.Items.Contains(cat.Trim())) {
                            cmbGroup.Items.Add(cat.Trim());
                        }
                    }
                }
            }
            cmbGroup.SelectedText = tmpGroup;
        }
    }
}
