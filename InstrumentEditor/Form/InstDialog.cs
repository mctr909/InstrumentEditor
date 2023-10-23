using InstPack;
using System;
using System.Windows.Forms;

namespace InstrumentEditor {
    public partial class InstDialog : Form {
        private Pack mFile;
        private DLS.INS mPreset;

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

        private readonly string[] GM2_DRUM_NAME = new string[] {
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

        public InstDialog(Pack file) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
            setLayout(rbTypeNote.Checked);
            setBankMsbList();
            setBankLsbList();
            setProgramList();
            setCategoryList();
        }

        public InstDialog(Pack file, DLS.INS preset) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            mFile = file;
            mPreset = preset;
            var enableArt = false;
            if (null != mPreset) {
                enableArt = mPreset.Locale.BankFlg != 0x80;
                artList.Art = mPreset.Articulations.List;
                rbTypeDrum.Checked = mPreset.Locale.BankFlg == 0x80;
                rbTypeDrum.Enabled = false;
                rbTypeNote.Enabled = false;
                txtInstName.Text = mPreset.Info[Info.TYPE.INAM].Trim();
            }
            setLayout(enableArt);
            setBankMsbList();
            setBankLsbList();
            setProgramList();
            setCategoryList();
        }

        private void rbType_CheckedChanged(object sender, EventArgs e) {
            setBankMsbList();
            setBankLsbList();
            setProgramList();
            setLayout(rbTypeNote.Checked);
        }

        private void cmbCategory_Leave(object sender, EventArgs e) {
            setCategoryList();
        }

        private void lstBankMSB_SelectedIndexChanged(object sender, EventArgs e) {
            setBankLsbList();
            setProgramList();
        }

        private void lstBankLSB_SelectedIndexChanged(object sender, EventArgs e) {
            setProgramList();
        }

        private void btnApply_Click(object sender, EventArgs e) {
            var id = new DLS.MidiLocale {
                ProgNum = (byte)lstPrgNo.SelectedIndex,
                BankMSB = (byte)lstBankMSB.SelectedIndex,
                BankLSB = (byte)lstBankLSB.SelectedIndex,
                BankFlg = (byte)(rbTypeDrum.Checked ? 0x80 : 0x00)
            };
            if (mFile.Inst.List.ContainsKey(id)) {
                MessageBox.Show("既に同じ識別子のプリセットが存在します。");
                return;
            }

            DLS.INS preset;
            if (null == mPreset || !mFile.Inst.List.ContainsKey(mPreset.Locale)) {
                preset = new DLS.INS();
            } else {
                preset = mFile.Inst.List[mPreset.Locale];
                mFile.Inst.List.Remove(mPreset.Locale);
            }
            preset.Locale = id;
            preset.Info[Info.TYPE.INAM] = txtInstName.Text;
            preset.Info[Info.TYPE.ICAT] = cmbCategory.Text;
            mFile.Inst.List.Add(id, preset);
            Close();
        }

        private void setLayout(bool enableArt) {
            if (enableArt) {
                artList.Top = grbProg.Bottom + 4;
                btnApply.Top = artList.Bottom + 4;
                btnApply.Left = artList.Right - btnApply.Width;
            } else {
                btnApply.Top = grbProg.Bottom + 4;
                btnApply.Left = grbProg.Right - btnApply.Width;
            }
            artList.Visible = enableArt;
            Width = btnApply.Right + 20;
            Height = btnApply.Bottom + 44;
        }

        private void setBankMsbList() {
            lstBankMSB.Items.Clear();
            for (byte i = 0; i < 128; ++i) {
                var strUse = " ";
                foreach (var preset in mFile.Inst.List.Keys) {
                    if (rbTypeDrum.Checked) {
                        if (preset.BankFlg == 0x80) {
                            if (i == preset.BankMSB) {
                                strUse = "+";
                                break;
                            }
                        }
                    } else {
                        if (preset.BankFlg != 0x80) {
                            if (i == preset.BankMSB) {
                                strUse = "+";
                                break;
                            }
                        }
                    }
                }
                if (null != mPreset && i == mPreset.Locale.BankMSB) {
                    strUse = "*";
                }
                lstBankMSB.Items.Add(string.Format("{0}{1}", i.ToString("000"), strUse));
            }

            if (null != mPreset) {
                lstBankMSB.SelectedIndex = mPreset.Locale.BankMSB;
            }
        }

        private void setBankLsbList() {
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
                            if (msbIndex == preset.BankMSB &&
                                i == preset.BankLSB
                            ) {
                                strUse = "+";
                                break;
                            }
                        }
                    } else {
                        if (preset.BankFlg != 0x80) {
                            if (msbIndex == preset.BankMSB &&
                                i == preset.BankLSB
                            ) {
                                strUse = "+";
                                break;
                            }
                        }
                    }
                }
                if (null != mPreset &&
                    lstBankMSB.SelectedIndex == mPreset.Locale.BankMSB &&
                    i == mPreset.Locale.BankLSB) {
                    strUse = "*";
                }
                lstBankLSB.Items.Add(string.Format("{0}{1}", i.ToString("000"), strUse));
            }

            if (null != mPreset) {
                lstBankLSB.SelectedIndex = mPreset.Locale.BankLSB;
            }
        }

        private void setProgramList() {
            var msbIndex = lstBankMSB.SelectedIndex;
            if (msbIndex < 0) {
                msbIndex = 0;
            }
            var lsbIndex = lstBankLSB.SelectedIndex;
            if (lsbIndex < 0) {
                lsbIndex = 0;
            }
            lstPrgNo.Items.Clear();
            for (byte i = 0; i < 128; ++i) {
                var strUse = " ";
                foreach (var preset in mFile.Inst.List.Keys) {
                    if (rbTypeDrum.Checked) {
                        if (msbIndex == preset.BankMSB &&
                            lsbIndex == preset.BankLSB &&
                            i == preset.ProgNum &&
                            preset.BankFlg == 0x80) {
                            strUse = "+";
                            break;
                        }
                    } else {
                        if (msbIndex == preset.BankMSB &&
                            lsbIndex == preset.BankLSB &&
                            i == preset.ProgNum &&
                            preset.BankFlg != 0x80) {
                            strUse = "+";
                            break;
                        }
                    }
                }
                if (null != mPreset &&
                    lstBankMSB.SelectedIndex == mPreset.Locale.BankMSB &&
                    lstBankLSB.SelectedIndex == mPreset.Locale.BankLSB &&
                    i == mPreset.Locale.ProgNum) {
                    strUse = "*";
                }
                if (rbTypeDrum.Checked) {
                    lstPrgNo.Items.Add(string.Format("{0} {1} {2}", i.ToString("000"), strUse, GM2_DRUM_NAME[i]));
                } else {
                    lstPrgNo.Items.Add(string.Format("{0} {1} {2}", i.ToString("000"), strUse, GM_INST_NAME[i]));
                }
            }
            if (null != mPreset) {
                lstPrgNo.SelectedIndex = mPreset.Locale.ProgNum;
            }
        }

        private void setCategoryList() {
            var tmpCategory = cmbCategory.SelectedText;
            cmbCategory.Items.Clear();
            if (!string.IsNullOrWhiteSpace(tmpCategory)) {
                cmbCategory.Items.Add(tmpCategory);
            }
            foreach (var preset in mFile.Inst.List.Values) {
                var cat = preset.Info[Info.TYPE.ICAT];
                if ("" != cat) {
                    if (!cmbCategory.Items.Contains(cat.Trim())) {
                        cmbCategory.Items.Add(cat.Trim());
                    }
                }
            }
            cmbCategory.SelectedText = tmpCategory;
        }
    }
}
