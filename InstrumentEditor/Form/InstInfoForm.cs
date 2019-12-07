using System;
using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
	public partial class InstInfoForm : Form {
        private DLS.DLS mDLS;
        private INS mINS;

        public InstInfoForm(DLS.DLS dls, INS ins) {
            mDLS = dls;
            mINS = ins;
            InitializeComponent();
            DispInfo();
        }

        private void DispInfo() {
            txtInstName.Text = mINS.Info.Name.Trim();
            cmbInstKeyword.Text = mINS.Info.Keywords.Trim();
            txtInstComment.Text = mINS.Info.Comments.Trim();
            setCategoryList();
            Text = mINS.Info.Name.Trim();
        }

        private void txtInstName_Leave(object sender, EventArgs e) {
            if (null == mINS.Info) {
                mINS.Info = new INFO();
            }
            mINS.Info.Name = txtInstName.Text.Trim();
            Text = mINS.Info.Name;
        }

        private void cmbInstKeyword_Leave(object sender, EventArgs e) {
            if (null == mINS.Info) {
                mINS.Info = new INFO();
            }
            mINS.Info.Keywords = cmbInstKeyword.Text.Trim();
            setCategoryList();
        }

        private void cmbInstKeyword_SelectedIndexChanged(object sender, EventArgs e) {
            if (null == mINS.Info) {
                mINS.Info = new INFO();
            }
            mINS.Info.Keywords = cmbInstKeyword.Text;
        }

        private void txtInstComment_Leave(object sender, EventArgs e) {
            if (null == mINS.Info) {
                mINS.Info = new INFO();
            }
            mINS.Info.Comments = txtInstComment.Text;
        }

        private void setCategoryList() {
            cmbInstKeyword.Items.Clear();
            cmbInstKeyword.Items.Add(mINS.Info.Keywords);
            foreach (var inst in mDLS.Instruments.List.Values) {
                if (null != inst.Info && "" != inst.Info.Keywords) {
                    if (!cmbInstKeyword.Items.Contains(inst.Info.Keywords.Trim())) {
                        cmbInstKeyword.Items.Add(inst.Info.Keywords.Trim());
                    }
                }
            }
            cmbInstKeyword.SelectedItem = mINS.Info.Keywords;
        }
    }
}
