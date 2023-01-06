using System;
using System.Windows.Forms;

using InstPack;

namespace InstrumentEditor {
	public partial class PresetInfoDialog : Form {
        private Pack mFile;
        private Preset mPreset;

        public PresetInfoDialog(Pack file, Preset preset, bool multi = false) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
            mPreset = preset;
            DispInfo();
            if (multi) {
                txtName.Enabled = false;
            }
        }

        private void PresetInfoDialog_FormClosing(object sender, FormClosingEventArgs e) {
            Reflect();
        }

        private void btnReflect_Click(object sender, EventArgs e) {
            Reflect();
            Close();
        }

        private void txtName_Leave(object sender, EventArgs e) {
            mPreset.Info[Info.TYPE.INAM] = txtName.Text.Trim();
            Text = string.Format("プリセット");
        }

        private void cmbCategory_Leave(object sender, EventArgs e) {
            mPreset.Info[Info.TYPE.ICAT] = cmbCategory.Text.Trim();
            setCategoryList();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e) {
            mPreset.Info[Info.TYPE.ICAT] = cmbCategory.Text;
        }

        private void Reflect() {
            mPreset.Info[Info.TYPE.INAM] = txtName.Text.Trim();
            mPreset.Info[Info.TYPE.ICAT] = cmbCategory.Text.Trim();
        }

        private void DispInfo() {
            txtName.Text = mPreset.Info[Info.TYPE.INAM].Trim();
            cmbCategory.Text = mPreset.Info[Info.TYPE.ICAT].Trim();
            setCategoryList();
            Text = string.Format("プリセット");
        }

        private void setCategoryList() {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add(mPreset.Info[Info.TYPE.ICAT]);
            foreach (var preset in mFile.Preset.Values) {
                if ("" != preset.Info[Info.TYPE.ICAT]) {
                    if (!cmbCategory.Items.Contains(preset.Info[Info.TYPE.ICAT].Trim())) {
                        cmbCategory.Items.Add(preset.Info[Info.TYPE.ICAT].Trim());
                    }
                }
            }
            cmbCategory.SelectedItem = mPreset.Info[Info.TYPE.ICAT];
        }
    }
}
