using System;
using System.Windows.Forms;

using Instruments;

namespace InstrumentEditor {
	public partial class PresetInfoForm : Form {
        private File mFile;
        private Preset mPreset;

        public PresetInfoForm(File file, Preset preset) {
            mFile = file;
            mPreset = preset;
            InitializeComponent();
            DispInfo();
        }

        private void PresetInfoForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (null == mPreset.Info) {
                mPreset.Info = new Riff.Info();
            }
            mPreset.Info.Name = txtInstName.Text.Trim();
            mPreset.Info.Category = cmbInstCategory.Text.Trim();
            mPreset.Info.Comments = txtInstComment.Text;
        }

        private void DispInfo() {
            txtInstName.Text = mPreset.Info.Name.Trim();
            cmbInstCategory.Text = mPreset.Info.Category.Trim();
            txtInstComment.Text = mPreset.Info.Comments.Trim();
            setCategoryList();
            Text = mPreset.Info.Name.Trim();
        }

        private void txtInstName_Leave(object sender, EventArgs e) {
            if (null == mPreset.Info) {
                mPreset.Info = new Riff.Info();
            }
            mPreset.Info.Name = txtInstName.Text.Trim();
            Text = mPreset.Info.Name;
        }

        private void cmbInstCategory_Leave(object sender, EventArgs e) {
            if (null == mPreset.Info) {
                mPreset.Info = new Riff.Info();
            }
            mPreset.Info.Category = cmbInstCategory.Text.Trim();
            setCategoryList();
        }

        private void cmbInstCategory_SelectedIndexChanged(object sender, EventArgs e) {
            if (null == mPreset.Info) {
                mPreset.Info = new Riff.Info();
            }
            mPreset.Info.Category = cmbInstCategory.Text;
        }

        private void txtInstComment_Leave(object sender, EventArgs e) {
            if (null == mPreset.Info) {
                mPreset.Info = new Riff.Info();
            }
            mPreset.Info.Comments = txtInstComment.Text;
        }

        private void setCategoryList() {
            cmbInstCategory.Items.Clear();
            cmbInstCategory.Items.Add(mPreset.Info.Category);
            foreach (var preset in mFile.Preset.Values) {
                if (null != preset.Info && "" != preset.Info.Category) {
                    if (!cmbInstCategory.Items.Contains(preset.Info.Category.Trim())) {
                        cmbInstCategory.Items.Add(preset.Info.Category.Trim());
                    }
                }
            }
            cmbInstCategory.SelectedItem = mPreset.Info.Category;
        }
    }
}
