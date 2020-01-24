using System;
using System.Windows.Forms;

using Instruments;

namespace InstrumentEditor {
    public partial class AddPresetForm : Form {
        private File mFile;
        private Preset mPreset;

        private readonly string[] GM_INST_NAME = new string[] {
            "Acoustic Grand Piano",
            "Bright Acoustic Piano",
            "Electoric Grand Piano",
            "Honky-tonk Piano",
            "Electoric Piano 1",
            "Electoric Piano 2",
            "Harpsichord",
            "Clavi",
            "Celesta",
            "Glockenspiel",
            "Music Box",
            "Vibraphone",
            "Marimba",
            "Xylophone",
            "Tubular Bells",
            "Dulcimer",
            "Drawbar Organ",
            "Percussive Organ",
            "Rock Organ",
            "Church Organ",
            "Reed Organ",
            "Accordion",
            "Harmonica",
            "Tango Accordion",
            "Acoustic Giutar(Nylon)",
            "Acoustic Giutar(Steel)",
            "Electoric Giutar(Jazz)",
            "Electoric Giutar(Clean)",
            "Electoric Giutar(Muted)",
            "Overdriven Guitar",
            "Distortion Guitar",
            "Guitar Harmonics",
            "Acoustic Bass",
            "Electoric Bass(Fingar)",
            "Electoric Bass(Pick)",
            "Fretless Bass",
            "Slap Bass 1",
            "Slap Bass 2",
            "Synth Bass 1",
            "Synth Bass 2",
            "Violin",
            "Viola",
            "Cello",
            "Contrabass",
            "Tremolo Strings",
            "Pizzicato Strings",
            "Orchestral Harp",
            "Timpani",
            "String Ensemble 1",
            "String Ensemble 2",
            "Synth Strings 1",
            "Synth Strings 2",
            "Choir Aahs",
            "Voice Oohs",
            "Synth Voice",
            "Orchestra Hit",
            "Trumpet",
            "Trombone",
            "Tuba",
            "Muted Trumpet",
            "French Horn",
            "Brass Section",
            "Synth Brass 1",
            "Synth Brass 2",
            "Soprano Sax",
            "Alto Sax",
            "Tenor Sax",
            "Baritone Sax",
            "Oboe",
            "English Horn",
            "Bassoon",
            "Clarinet",
            "Piccolo",
            "Flute",
            "Recorder",
            "Pan Flute",
            "Blown Bottle",
            "Shakuhach",
            "Whistle",
            "Ocarina",
            "Square",
            "Sawtooth",
            "Calliope",
            "Chiff",
            "Charang",
            "Voice",
            "Fifths",
            "Bass+Lead",
            "New age",
            "Warm",
            "Polysynth",
            "Choir",
            "Bowed",
            "Metallic",
            "Halo",
            "Sweep",
            "Rain",
            "Soundtrack",
            "Crystal",
            "Atmosphere",
            "Brightness",
            "Goblins",
            "Echoes",
            "Sci-Fi",
            "Sitar",
            "Banjo",
            "Shamisen",
            "Koto",
            "Kalimba",
            "Bag Pipe",
            "Fiddle",
            "Shanai",
            "Tinkle Bell",
            "Agogo",
            "Steel Drums",
            "Woodblock",
            "Taiko Drum",
            "Melodic Tom",
            "Synth Drum",
            "Reverse Cymbal",
            "Guitar Fret Noise",
            "Breath Noise",
            "Seashore",
            "Bird Tweet",
            "Telephone Ring",
            "Helicopter",
            "Applause",
            "Gunshot"
        };

        public AddPresetForm(File file) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
        }

        public AddPresetForm(File file, Preset preset) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
            mPreset = preset;
        }

        private void InstAddForm_Load(object sender, EventArgs e) {
            setProgramList();
            setBankMsbList();
            setBankLsbList();
        }

        private void rbDrum_CheckedChanged(object sender, EventArgs e) {
            setProgramList();
            setBankMsbList();
            setBankLsbList();
        }

        private void lstPrgNo_SelectedIndexChanged(object sender, EventArgs e) {
            setBankMsbList();
            setBankLsbList();
        }

        private void lstBankMSB_SelectedIndexChanged(object sender, EventArgs e) {
            setBankLsbList();
        }

        private void lstBankLSB_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void btnAdd_Click(object sender, EventArgs e) {
            var id = new PREH {
                ProgNum = (byte)lstPrgNo.SelectedIndex,
                BankMSB = (byte)lstBankMSB.SelectedIndex,
                BankLSB = (byte)lstBankLSB.SelectedIndex,
                BankFlg = (byte)(rbDrum.Checked ? 1 : 0)
            };

            if (mFile.Preset.ContainsKey(id)) {
                MessageBox.Show("既に同じ識別子の音色が存在します。");
                return;
            }

            var preset = new Preset();
            preset.Header = id;
            preset.Info.Name = txtInstName.Text;
            preset.Info.CreationDate = DateTime.Now.ToString("YYYY/MM/DD HH:mm");

            if (null != mPreset) {
                mPreset.Header = id;
                mPreset.Info.Name = preset.Info.Name;
                mPreset.Info.CreationDate = preset.Info.CreationDate;
                preset = mPreset;
            }

            mFile.Preset.Add(id, preset);

            Close();
        }

        private void setProgramList() {
            lstPrgNo.Items.Clear();

            if (null != mPreset) {
                rbDrum.Checked = (mPreset.Header.BankFlg & 1) == 1;
                rbDrum.Enabled = false;
                rbNote.Enabled = false;
                if (null != mPreset.Info) {
                    txtInstName.Text = mPreset.Info.Name.Trim();
                }
            }

            for (byte i = 0; i < 128; ++i) {
                var strUse = "   ";
                foreach (var preset in mFile.Preset.Keys) {
                    if (rbDrum.Checked) {
                        if (1 == preset.BankFlg) {
                            if (i == preset.ProgNum) {
                                strUse = "use";
                                break;
                            }
                        }
                    } else {
                        if (0 == preset.BankFlg) {
                            if (i == preset.ProgNum) {
                                strUse = "use";
                                break;
                            }
                        }
                    }
                }

                if (rbDrum.Checked) {
                    lstPrgNo.Items.Add(string.Format("{0} {1}", i.ToString("000"), strUse));
                } else {
                    lstPrgNo.Items.Add(string.Format("{0} {1} {2}", i.ToString("000"), strUse, GM_INST_NAME[i]));
                }
            }

            if (null != mPreset) {
                lstPrgNo.SelectedIndex = mPreset.Header.ProgNum;
            }
        }

        private void setBankMsbList() {
            var prgIndex = lstPrgNo.SelectedIndex;
            if (prgIndex < 0) {
                prgIndex = 0;
            }

            lstBankMSB.Items.Clear();

            for (byte i = 0; i < 128; ++i) {
                var strUse = "   ";
                foreach (var preset in mFile.Preset.Keys) {
                    if (rbDrum.Checked) {
                        if (1 == preset.BankFlg) {
                            if (prgIndex == preset.ProgNum &&
                                i == preset.BankMSB
                            ) {
                                strUse = "use";
                                break;
                            }
                        }
                    } else {
                        if (0 == preset.BankFlg) {
                            if (prgIndex == preset.ProgNum &&
                                i == preset.BankMSB
                            ) {
                                strUse = "use";
                                break;
                            }
                        }
                    }
                }
                lstBankMSB.Items.Add(string.Format("{0} {1}", i.ToString("000"), strUse));
            }

            if (null != mPreset) {
                lstBankMSB.SelectedIndex = mPreset.Header.BankMSB;
            }
        }

        private void setBankLsbList() {
            var prgIndex = lstPrgNo.SelectedIndex;
            var msbIndex = lstBankMSB.SelectedIndex;
            if (prgIndex < 0) {
                prgIndex = 0;
            }
            if (msbIndex < 0) {
                msbIndex = 0;
            }

            lstBankLSB.Items.Clear();

            for (byte i = 0; i < 128; ++i) {
                var strUse = "   ";
                foreach (var preset in mFile.Preset.Keys) {
                    if (rbDrum.Checked) {
                        if (1 == preset.BankFlg) {
                            if (prgIndex == preset.ProgNum &&
                                msbIndex == preset.BankMSB &&
                                i == preset.BankLSB
                            ) {
                                strUse = "use";
                                break;
                            }
                        }
                    } else {
                        if (0 == preset.BankFlg) {
                            if (prgIndex == preset.ProgNum &&
                                msbIndex == preset.BankMSB &&
                                i == preset.BankLSB
                            ) {
                                strUse = "use";
                                break;
                            }
                        }
                    }
                }
                lstBankLSB.Items.Add(string.Format("{0} {1}", i.ToString("000"), strUse));
            }

            if (null != mPreset) {
                lstBankLSB.SelectedIndex = mPreset.Header.BankLSB;
            }
        }
    }
}
