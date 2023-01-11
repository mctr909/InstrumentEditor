using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
    public partial class Articulations : UserControl {
        private List<Connection> mLart;
        private const int TimeGamma = 1000;

        public Articulations() {
            InitializeComponent();
            disp();
        }

        public List<Connection> Art {
            get { return mLart; }
            set {
                mLart = value;
                disp();
            }
        }

        public void SetList(LART art) {
            //if (chkAmpAttack.Checked) {
            //    art.Update(DST_TYPE.EG1_ATTACK_TIME, ampAttack);
            //}
            //if (chkAmpHold.Checked) {
            //    art.Update(DST_TYPE.EG1_HOLD_TIME, ampHold);
            //}
            //if (chkAmpDecay.Checked) {
            //    art.Update(DST_TYPE.EG1_DECAY_TIME, ampDecay);
            //}
            //if (chkAmpSustain.Checked) {
            //    art.Update(DST_TYPE.EG1_SUSTAIN_LEVEL, ampSustain);
            //}
            //if (chkAmpReleace.Checked) {
            //    art.Update(DST_TYPE.EG1_RELEASE_TIME, ampReleace);
            //}

            //if (chkEqAttack.Checked) {
            //    art.Update(DST_TYPE.EG2_ATTACK_TIME, eqAttack);
            //}
            //if (chkEqHold.Checked) {
            //    art.Update(DST_TYPE.EG2_HOLD_TIME, eqHold);
            //}
            //if (chkEqDecay.Checked) {
            //    art.Update(DST_TYPE.EG2_DECAY_TIME, eqDecay);
            //}
            //if (chkEqSustain.Checked) {
            //    art.Update(DST_TYPE.EG2_SUSTAIN_LEVEL, eqSustain);
            //}
            //if (chkEqReleace.Checked) {
            //    art.Update(DST_TYPE.EG2_RELEASE_TIME, eqReleace);
            //}
        }


        private string trbToText(TrackBar trb) {
            var v = trbToValue(trb);

            if (v < 1.0) {
                return string.Format("{0}ms", (int)(1000 * v));
            }
            else if (v < 10.0) {
                return string.Format("{0}s", ((int)(100 * v) / 100.0).ToString("0.00"));
            }
            else {
                return string.Format("{0}s", ((int)(10 * v) / 10.0).ToString("0.0"));
            }
        }

        private double trbToValue(TrackBar trb) {
            return (Math.Pow(TimeGamma, (double)trb.Value / trb.Maximum) - 1.0) / (TimeGamma - 1.0) * 40.0;
        }

        private int valueToTrb(double hsb, TrackBar trb) {
            return (int)(Math.Log(hsb / 40.0 * (TimeGamma - 1) + 1.0, TimeGamma) * trb.Maximum);
        }

        private void setValue(TrackBar trb, CheckBox chk, Label lbl, double value) {
            if (value <= 0) {
                trb.Value = 1;
                trb.Enabled = false;
                chk.Checked = false;
                lbl.Text = "----";
            }
            else if (39 < value) {
                trb.Value = valueToTrb(39, trb);
                trb.Enabled = true;
                chk.Checked = true;
            }
            else {
                trb.Value = valueToTrb(value, trb);
                trb.Enabled = true;
                chk.Checked = true;
            }
        }

        private void disp() {
            dataGridView1.Width = Width;
            dataGridView1.Height = Height;
            dataGridView1.ScrollBars = ScrollBars.Both;

            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            var cmbSrc = new DataGridViewComboBoxColumn();
            cmbSrc.Name = "Source";
            foreach (var src in Enum.GetNames(typeof(SRC_TYPE))) {
                cmbSrc.Items.Add(src);
            }
            cmbSrc.MinimumWidth = 100;
            cmbSrc.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns.Add(cmbSrc);
            
            var cmbType = new DataGridViewComboBoxColumn();
            cmbType.Name = "Type";
            foreach (var type in Enum.GetNames(typeof(DST_TYPE))) {
                cmbType.Items.Add(type);
            }
            cmbType.MinimumWidth = 100;
            cmbType.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns.Add(cmbType);
            
            var val = new DataGridViewTextBoxColumn();
            val.Name = "Value";
            val.MinimumWidth = 30;
            val.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns.Add(val);
            
            var unit = new DataGridViewTextBoxColumn();
            unit.Name = "Unit";
            unit.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            unit.ReadOnly = true;
            dataGridView1.Columns.Add(unit);

            if (null == mLart) {
                return;
            }

            foreach (var art in mLart) {
                var idx = dataGridView1.Rows.Add();
                dataGridView1.Rows[idx].Cells["Source"].Value = Enum.GetName(typeof(SRC_TYPE), art.Source);
                dataGridView1.Rows[idx].Cells["Source"].ReadOnly = true;
                dataGridView1.Rows[idx].Cells["Type"].Value = Enum.GetName(typeof(DST_TYPE), art.Destination);
                dataGridView1.Rows[idx].Cells["Type"].ReadOnly = true;
                dataGridView1.Rows[idx].Cells["Value"].Value = art.Value.ToString("0.000");
                dataGridView1.Rows[idx].Cells["Unit"].Value = art.Unit;
            }
        }
    }
}
