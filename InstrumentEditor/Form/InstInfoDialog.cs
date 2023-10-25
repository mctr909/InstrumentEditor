using System;
using System.Windows.Forms;

using DLS;

namespace InstrumentEditor {
    public partial class InstInfoDialog : Form {
        File mFile;
        INS mPreset;

        readonly string[] GM_INST_NAME = new string[] {
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

        readonly string[] GM2_DRUM_NAME = new string[] {
            "Standard",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "Room",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "Power",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "Electronic",
            "Analog",
            "",
            "",
            "",
            "",
            "",
            "",
            "Jazz",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "Brush",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "Orchestra",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "SFX",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
        };

        public InstInfoDialog(File file) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
            SetLayout(rbTypeNote.Checked);
            SetProgramList();
            SetBankMsbList();
            SetBankLsbList();
            SetGroupList();
        }

        public InstInfoDialog(File file, INS preset) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
            mPreset = preset;
            var enableArt = preset.Locale.BankFlg != 0x80;
            artList.Art = preset.Articulations.List;
            rbTypeDrum.Checked = preset.Locale.BankFlg == 0x80;
            rbTypeDrum.Enabled = false;
            rbTypeNote.Enabled = false;
            txtInstName.Text = preset.Info[Info.TYPE.INAM].Trim();
            SetLayout(enableArt);
            SetProgramList();
            SetBankMsbList();
            SetBankLsbList();
            SetGroupList();
            cmbGroup.SelectedText = preset.Info[Info.TYPE.ICAT];
        }

        private void rbType_CheckedChanged(object sender, EventArgs e) {
            SetProgramList();
            SetBankMsbList();
            SetBankLsbList();
            SetLayout(rbTypeNote.Checked);
        }

        private void cmbGroup_Leave(object sender, EventArgs e) {
            SetGroupList();
        }

        private void lstPrgNo_SelectedIndexChanged(object sender, EventArgs e) {
            SetBankMsbList();
            SetBankLsbList();
        }

        private void lstBankMSB_SelectedIndexChanged(object sender, EventArgs e) {
            SetBankLsbList();
        }

        private void btnApply_Click(object sender, EventArgs e) {
            var newId = new MidiLocale {
                ProgNum = (byte)lstPrgNo.SelectedIndex,
                BankMSB = (byte)lstBankMSB.SelectedIndex,
                BankLSB = (byte)lstBankLSB.SelectedIndex,
                BankFlg = (byte)(rbTypeDrum.Checked ? 0x80 : 0x00)
            };
            INS preset;
            if (null == mPreset) {
                if (mFile.Inst.ContainsKey(newId)) {
                    MessageBox.Show("同一識別子の音色が存在しています");
                    return;
                }
                preset = new INS();
            } else {
                if (mPreset.Locale.Equals(newId)) {
                    mFile.Inst.Remove(newId);
                } else {
                    if (mFile.Inst.ContainsKey(newId)) {
                        MessageBox.Show("同一識別子の音色が存在しています");
                        return;
                    }
                }
                preset = mPreset;
            }
            preset.Locale = newId;
            preset.Info[Info.TYPE.INAM] = txtInstName.Text;
            preset.Info[Info.TYPE.ICAT] = cmbGroup.Text;
            preset.Articulations.Clear();
            if (null != artList.Art) {
                artList.SetList(preset.Articulations);
            }
            mFile.Inst.Add(preset);
            Close();
        }

        void SetLayout(bool enableArt) {
            if (enableArt) {
                artList.Top = grbProg.Bottom + 4;
                btnApply.Top = artList.Bottom + 4;
                btnApply.Left = artList.Right - btnApply.Width;
            } else {
                btnApply.Top = grbLSB.Bottom + 4;
                btnApply.Left = grbLSB.Right - btnApply.Width;
            }
            artList.Visible = enableArt;
            Width = btnApply.Right + 20;
            Height = btnApply.Bottom + 44;
        }

        void SetProgramList() {
            lstPrgNo.Items.Clear();
            for (byte i = 0; i < 128; ++i) {
                var strUse = " ";
                foreach (var preset in mFile.Inst.List.Keys) {
                    if (rbTypeDrum.Checked) {
                        if (i == preset.ProgNum &&
                            preset.BankFlg == 0x80) {
                            strUse = "-";
                            break;
                        }
                    } else {
                        if (i == preset.ProgNum &&
                            preset.BankFlg != 0x80) {
                            strUse = "-";
                            break;
                        }
                    }
                }
                if (null != mPreset && i == mPreset.Locale.ProgNum) {
                    strUse = "*";
                }
                if (rbTypeDrum.Checked) {
                    lstPrgNo.Items.Add(string.Format("{0} {1} {2}", i.ToString("000"), strUse, GM2_DRUM_NAME[i]));
                } else {
                    lstPrgNo.Items.Add(string.Format("{0} {1} {2}", i.ToString("000"), strUse, GM_INST_NAME[i]));
                }
            }
            if (null == mPreset) {
                lstPrgNo.SelectedIndex = 0;
            } else {
                lstPrgNo.SelectedIndex = mPreset.Locale.ProgNum;
            }
        }

        void SetBankMsbList() {
            var prgNo = lstPrgNo.SelectedIndex;
            if (prgNo < 0) {
                prgNo = 0;
            }
            lstBankMSB.Items.Clear();
            for (byte i = 0; i < 128; ++i) {
                var strUse = " ";
                foreach (var preset in mFile.Inst.List.Keys) {
                    if (rbTypeDrum.Checked) {
                        if (preset.BankFlg == 0x80) {
                            if (prgNo == preset.ProgNum && i == preset.BankMSB) {
                                strUse = "-";
                                break;
                            }
                        }
                    } else {
                        if (preset.BankFlg != 0x80) {
                            if (prgNo == preset.ProgNum && i == preset.BankMSB) {
                                strUse = "-";
                                break;
                            }
                        }
                    }
                }
                if (null != mPreset && prgNo == mPreset.Locale.ProgNum && i == mPreset.Locale.BankMSB) {
                    strUse = "*";
                }
                lstBankMSB.Items.Add(string.Format("{0}{1}", i.ToString("000"), strUse));
            }
            if (null == mPreset) {
                lstBankMSB.SelectedIndex = 0;
            } else {
                lstBankMSB.SelectedIndex = mPreset.Locale.BankMSB;
            }
        }

        void SetBankLsbList() {
            var prgNo = lstPrgNo.SelectedIndex;
            if (prgNo < 0) {
                prgNo = 0;
            }
            var msbIndex = lstBankMSB.SelectedIndex;
            if (msbIndex < 0) {
                msbIndex = 0;
            }
            lstBankLSB.Items.Clear();
            for (byte i = 0; i < 128; ++i) {
                var strUse = " ";
                foreach (var preset in mFile.Inst.List.Keys) {
                    if (rbTypeDrum.Checked) {
                        if (preset.BankFlg == 0x80) {
                            if (prgNo == preset.ProgNum && msbIndex == preset.BankMSB && i == preset.BankLSB) {
                                strUse = "-";
                                break;
                            }
                        }
                    } else {
                        if (preset.BankFlg != 0x80) {
                            if (prgNo == preset.ProgNum && msbIndex == preset.BankMSB && i == preset.BankLSB) {
                                strUse = "-";
                                break;
                            }
                        }
                    }
                }
                if (null != mPreset &&
                    prgNo == mPreset.Locale.ProgNum &&
                    msbIndex == mPreset.Locale.BankMSB &&
                    i == mPreset.Locale.BankLSB) {
                    strUse = "*";
                }
                lstBankLSB.Items.Add(string.Format("{0}{1}", i.ToString("000"), strUse));
            }
            if (null == mPreset) {
                lstBankLSB.SelectedIndex = 0;
            } else {
                lstBankLSB.SelectedIndex = mPreset.Locale.BankLSB;
            }
        }

        void SetGroupList() {
            var tmpGroup = cmbGroup.SelectedText;
            cmbGroup.Items.Clear();
            if (!string.IsNullOrWhiteSpace(tmpGroup)) {
                cmbGroup.Items.Add(tmpGroup);
            }
            foreach (var preset in mFile.Inst.List.Values) {
                var cat = preset.Info[Info.TYPE.ICAT];
                if ("" != cat) {
                    if (!cmbGroup.Items.Contains(cat.Trim())) {
                        cmbGroup.Items.Add(cat.Trim());
                    }
                }
            }
            cmbGroup.SelectedText = tmpGroup;
        }
    }
}
