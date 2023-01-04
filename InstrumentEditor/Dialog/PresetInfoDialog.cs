using System;
using System.Windows.Forms;

using InstPack;

namespace InstrumentEditor {
	public partial class PresetInfoDialog : Form {
        private Pack mFile;
        private Preset mPreset;

        public PresetInfoDialog(Pack file, Preset preset) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
            mPreset = preset;
            DispInfo();
            if (0 == mPreset.Art.Count) {
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
            mPreset.InfoName = txtName.Text.Trim();
            Text = string.Format("プリセット");
        }

        private void cmbCategory_Leave(object sender, EventArgs e) {
            mPreset.InfoCat = cmbCategory.Text.Trim();
            setCategoryList();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e) {
            mPreset.InfoCat = cmbCategory.Text;
        }

        private void Reflect() {
            mPreset.InfoName = txtName.Text.Trim();
            mPreset.InfoCat = cmbCategory.Text.Trim();
        }

        private void DispInfo() {
            txtName.Text = mPreset.InfoName.Trim();
            cmbCategory.Text = mPreset.InfoCat.Trim();
            setCategoryList();
            Text = string.Format("プリセット");
        }

        private void setCategoryList() {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add(mPreset.InfoCat);
            foreach (var preset in mFile.Preset.Values) {
                if ("" != preset.InfoCat) {
                    if (!cmbCategory.Items.Contains(preset.InfoCat.Trim())) {
                        cmbCategory.Items.Add(preset.InfoCat.Trim());
                    }
                }
            }
            cmbCategory.SelectedItem = mPreset.InfoCat;
        }
    }
}
