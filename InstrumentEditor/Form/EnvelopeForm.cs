using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
	public partial class EnvelopeForm : Form {
        private DLS.DLS mDLS;
        private INS mINS;

        public EnvelopeForm(DLS.DLS dls, INS ins) {
            mDLS = dls;
            mINS = ins;
            InitializeComponent();
            DispRegionInfo();
        }

        private void DispRegionInfo() {
            ampEnvelope.Art = mINS.Articulations.ART;
            Text = mINS.Info.Name.Trim();
        }
    }
}
