using System;
using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
    public partial class GroupAssignDialog : Form {
        File mFile;
        INS mPreset;

        public GroupAssignDialog(File file, INS preset) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
            mPreset = preset;
            SetGroupList();
        }

        private void cmbGroup_Leave(object sender, EventArgs e) {
            SetGroupList();
        }

        private void btnApply_Click(object sender, EventArgs e) {
            mPreset.Info[Info.TYPE.ICAT] = cmbGroup.Text;
            Close();
        }

        void SetGroupList() {
            var tmpGroup = cmbGroup.SelectedText;
            cmbGroup.Items.Clear();
            if (!string.IsNullOrWhiteSpace(tmpGroup)) {
                cmbGroup.Items.Add(tmpGroup);
            }
            foreach (var preset in mFile.Inst.List.Values) {
                var cat = preset.Info[Info.TYPE.ICAT];
                if ("" != cat) {
                    if (!cmbGroup.Items.Contains(cat.Trim())) {
                        cmbGroup.Items.Add(cat.Trim());
                    }
                }
            }
            cmbGroup.SelectedText = tmpGroup;
        }
    }
}
