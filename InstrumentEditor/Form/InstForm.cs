using System;
using System.Windows.Forms;

namespace InstrumentEditor {
	public partial class InstForm : Form {
		private DLS.DLS mDLS;
		private DLS.INS mINS;

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

		public InstForm(DLS.DLS dls) {
			InitializeComponent();
			StartPosition = FormStartPosition.CenterParent;
			mDLS = dls;
		}

		public InstForm(DLS.DLS dls, DLS.INS ins) {
			InitializeComponent();
			StartPosition = FormStartPosition.CenterParent;
			mDLS = dls;
			mINS = ins;
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
			var inst = new DLS.INS(
				(byte)lstPrgNo.SelectedIndex,
				(byte)lstBankMSB.SelectedIndex,
				(byte)lstBankLSB.SelectedIndex,
				rbDrum.Checked
			);
			inst.Info.Name = txtInstName.Text;

			if (mDLS.Instruments.List.ContainsKey(inst.Header.Locale)) {
				MessageBox.Show("既に同じ識別子の音色が存在します。");
				return;
			}

			if (null != mINS) {
				mINS.Header.Locale = inst.Header.Locale;
				if (null == mINS.Info) {
					mINS.Info = new DLS.INFO();
				}
				mINS.Info.Name = inst.Info.Name;
				inst = mINS;
			}

			mDLS.Instruments.List.Add(inst.Header.Locale, inst);

			Close();
		}

		private void setProgramList() {
			lstPrgNo.Items.Clear();

			if (null != mINS) {
				rbDrum.Checked = (mINS.Header.Locale.BankFlags & 0x80) == 0x80;
				rbDrum.Enabled = false;
				rbNote.Enabled = false;
				if (null != mINS.Info) {
					txtInstName.Text = mINS.Info.Name.Trim();
				}
			}

			for (byte i = 0; i < 128; ++i) {
				var strUse = "   ";
				foreach (var inst in mDLS.Instruments.List.Keys) {
					if (rbDrum.Checked) {
						if (0x80 == inst.BankFlags) {
							if (i == inst.ProgramNo) {
								strUse = "use";
								break;
							}
						}
					}
					else {
						if (0x00 == inst.BankFlags) {
							if (i == inst.ProgramNo) {
								strUse = "use";
								break;
							}
						}
					}
				}

				if (rbDrum.Checked) {
					lstPrgNo.Items.Add(string.Format("{0} {1}", i.ToString("000"), strUse));
				}
				else {
					lstPrgNo.Items.Add(string.Format("{0} {1} {2}", i.ToString("000"), strUse, GM_INST_NAME[i]));
				}
			}

			if (null != mINS) {
				lstPrgNo.SelectedIndex = mINS.Header.Locale.ProgramNo;
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
				foreach (var inst in mDLS.Instruments.List.Keys) {
					if (rbDrum.Checked) {
						if (0x80 == inst.BankFlags) {
							if (prgIndex == inst.ProgramNo &&
								i == inst.BankMSB
							) {
								strUse = "use";
								break;
							}
						}
					}
					else {
						if (0x00 == inst.BankFlags) {
							if (prgIndex == inst.ProgramNo &&
								i == inst.BankMSB
							) {
								strUse = "use";
								break;
							}
						}
					}
				}
				lstBankMSB.Items.Add(string.Format("{0} {1}", i.ToString("000"), strUse));
			}

			if (null != mINS) {
				lstBankMSB.SelectedIndex = mINS.Header.Locale.BankMSB;
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
				foreach (var inst in mDLS.Instruments.List.Keys) {
					if (rbDrum.Checked) {
						if (0x80 == inst.BankFlags) {
							if (prgIndex == inst.ProgramNo &&
								msbIndex == inst.BankMSB &&
								i == inst.BankLSB
							) {
								strUse = "use";
								break;
							}
						}
					}
					else {
						if (0x00 == inst.BankFlags) {
							if (prgIndex == inst.ProgramNo &&
								msbIndex == inst.BankMSB &&
								i == inst.BankLSB
							) {
								strUse = "use";
								break;
							}
						}
					}
				}
				lstBankLSB.Items.Add(string.Format("{0} {1}", i.ToString("000"), strUse));
			}

			if (null != mINS) {
				lstBankLSB.SelectedIndex = mINS.Header.Locale.BankLSB;
			}
		}
	}
}
