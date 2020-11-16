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
            if (null == mPreset.Info) {
                mPreset.Info = new Riff.Info();
            }
            mPreset.Info.Name = txtName.Text.Trim();
            Text = string.Format("プリセット[{0}]", mPreset.Info.Name);
        }

        private void cmbCategory_Leave(object sender, EventArgs e) {
            if (null == mPreset.Info) {
                mPreset.Info = new Riff.Info();
            }
            mPreset.Info.Category = cmbCategory.Text.Trim();
            setCategoryList();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e) {
            if (null == mPreset.Info) {
                mPreset.Info = new Riff.Info();
            }
            mPreset.Info.Category = cmbCategory.Text;
        }

        private void Reflect() {
            if (null == mPreset.Info) {
                mPreset.Info = new Riff.Info();
            }
            mPreset.Info.Name = txtName.Text.Trim();
            mPreset.Info.Category = cmbCategory.Text.Trim();
        }

        private void DispInfo() {
            txtName.Text = mPreset.Info.Name.Trim();
            cmbCategory.Text = mPreset.Info.Category.Trim();
            setCategoryList();
            Text = string.Format("プリセット[{0}]", mPreset.Info.Name.Trim());
        }

        private void setCategoryList() {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add(mPreset.Info.Category);
            foreach (var preset in mFile.Preset.Values) {
                if (null != preset.Info && "" != preset.Info.Category) {
                    if (!cmbCategory.Items.Contains(preset.Info.Category.Trim())) {
                        cmbCategory.Items.Add(preset.Info.Category.Trim());
                    }
                }
            }
            cmbCategory.SelectedItem = mPreset.Info.Category;
        }
    }
}
