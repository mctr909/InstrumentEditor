using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
    public partial class Articulations : UserControl {
        private List<Connection> mLart;

        public Articulations() {
            InitializeComponent();
            disp();
        }

        public List<Connection> Art {
            get {
                return mLart;
            }
            set {
                mLart = value;
                disp();
            }
        }

        public void SetList(LART art) {
            art.Clear();
            foreach (DataGridViewRow row in dataGridView1.Rows) {
                var c = row.Cells;
                var oSrc = c["入力"].Value;
                var oDst = c["種類"].Value;
                var oVal = c["値"].Value;
                if (null == oSrc || null == oDst || null == oVal) {
                    continue;
                }
                var src = (SRC_TYPE)Enum.Parse(typeof(SRC_TYPE), (string)oSrc);
                var dst = (DST_TYPE)Enum.Parse(typeof(DST_TYPE), (string)oDst);
                art.Add(new Connection() {
                    Source = src,
                    Destination = dst,
                    Value = double.Parse((string)oVal)
                });
            }
        }

        private void disp() {
            dataGridView1.Top = 0;
            dataGridView1.Left = 0;
            dataGridView1.Width = Width;
            dataGridView1.Height = Height;
            dataGridView1.ScrollBars = ScrollBars.Both;

            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            var cmbSrc = new DataGridViewComboBoxColumn();
            cmbSrc.Name = "入力";
            foreach (var src in Enum.GetNames(typeof(SRC_TYPE))) {
                cmbSrc.Items.Add(src);
            }
            cmbSrc.MinimumWidth = 100;
            cmbSrc.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns.Add(cmbSrc);

            var cmbType = new DataGridViewComboBoxColumn();
            cmbType.Name = "種類";
            foreach (var type in Enum.GetNames(typeof(DST_TYPE))) {
                cmbType.Items.Add(type);
            }
            cmbType.MinimumWidth = 100;
            cmbType.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns.Add(cmbType);

            var val = new DataGridViewTextBoxColumn();
            val.Name = "値";
            val.MinimumWidth = 30;
            val.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns.Add(val);

            var unit = new DataGridViewTextBoxColumn();
            unit.Name = "単位";
            unit.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            unit.ReadOnly = true;
            dataGridView1.Columns.Add(unit);

            if (null == mLart) {
                return;
            }

            foreach (var art in mLart) {
                var idx = dataGridView1.Rows.Add();
                dataGridView1.Rows[idx].Cells["入力"].Value = Enum.GetName(typeof(SRC_TYPE), art.Source);
                dataGridView1.Rows[idx].Cells["入力"].ReadOnly = true;
                dataGridView1.Rows[idx].Cells["種類"].Value = Enum.GetName(typeof(DST_TYPE), art.Destination);
                dataGridView1.Rows[idx].Cells["種類"].ReadOnly = true;
                dataGridView1.Rows[idx].Cells["値"].Value = art.Value.ToString("0.000");
                dataGridView1.Rows[idx].Cells["単位"].Value = art.Unit;
            }
        }
    }
}
