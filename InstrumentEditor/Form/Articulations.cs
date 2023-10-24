using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
    public partial class Articulations : UserControl {
        List<Connection> mLart;

        public Articulations() {
            InitializeComponent();
            Display();
        }

        public List<Connection> Art {
            get {
                return mLart;
            }
            set {
                mLart = value;
                Display();
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

        void Display() {
            dataGridView1.Top = 0;
            dataGridView1.Left = 0;
            dataGridView1.Width = Width;
            dataGridView1.Height = Height;
            dataGridView1.ScrollBars = ScrollBars.Both;

            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            var cmbSrc = new DataGridViewComboBoxColumn() {
                Name = "入力",
                MinimumWidth = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            };
            foreach (var src in Enum.GetNames(typeof(SRC_TYPE))) {
                cmbSrc.Items.Add(src);
            }
            dataGridView1.Columns.Add(cmbSrc);

            var cmbType = new DataGridViewComboBoxColumn() {
                Name = "種類",
                MinimumWidth = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            };
            foreach (var type in Enum.GetNames(typeof(DST_TYPE))) {
                cmbType.Items.Add(type);
            }
            dataGridView1.Columns.Add(cmbType);

            var val = new DataGridViewTextBoxColumn() {
                Name = "値",
                MinimumWidth = 30,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            };
            dataGridView1.Columns.Add(val);

            var unit = new DataGridViewTextBoxColumn() {
                Name = "単位",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                ReadOnly = true
            };
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
