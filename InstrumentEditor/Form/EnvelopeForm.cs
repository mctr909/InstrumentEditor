using System.Windows.Forms;

using Instruments;

namespace InstrumentEditor {
	public partial class EnvelopeForm : Form {
        private File mFile;
        private Region mRegion;

        public EnvelopeForm(File file, Region region) {
            mFile = file;
            mRegion = region;
            InitializeComponent();
            DispRegionInfo();
        }

        private void DispRegionInfo() {
            ampEnvelope.Art = mRegion.Art;
        }
    }
}
