using System;
using System.Collections.Generic;
using System.IO;

namespace DLS {
	public class LINS : Riff {
		public sealed class Sort : IComparer<MidiLocale> {
			// IComparerの実装
			public int Compare(MidiLocale x, MidiLocale y) {
				var xKey = ((x.BankFlg & 0x80) << 17) | (x.ProgNum << 16) | (x.BankMSB << 8) | x.BankLSB;
				var yKey = ((y.BankFlg & 0x80) << 17) | (y.ProgNum << 16) | (y.BankMSB << 8) | y.BankLSB;
				return xKey - yKey;
			}
		}

		public SortedDictionary<MidiLocale, INS> List = new SortedDictionary<MidiLocale, INS>(new Sort());

		protected override void Init(out string id, List<Chunk> chunks, List<RList> riffs) {
			id = "lins";
		}

		public LINS() { }

		public LINS(IntPtr ptr, long size) {
			Load(ptr, size);
		}

		public override void Write(BinaryWriter bw) {
			var msLins = new MemoryStream();
			var bwLins = new BinaryWriter(msLins);
			foreach (var ins in List) {
				ins.Value.Write(bwLins);
			}

			if (0 < msLins.Length) {
				bw.Write("LIST".ToCharArray());
				bw.Write((uint)(msLins.Length + 4));
				bw.Write("lins".ToCharArray());
				bw.Write(msLins.ToArray());
			}
		}

		protected override void LoadChunk(IntPtr ptr, string type, long size) {
			switch (type) {
			case "ins ":
				var inst = new INS(ptr, size);
				if (List.ContainsKey(inst.Locale)) {
					return;
				}
				List.Add(inst.Locale, inst);
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkType [{0}]", type));
			}
		}
	}

	public class INS : Riff {
		public MidiLocale Locale;
		public LRGN Regions = new LRGN();
		public LART Articulations = new LART();

		protected override void Init(out string id, List<Chunk> chunks, List<RList> riffs) {
			id = "ins ";
			chunks.Add(new Chunk("insh", (i) => {
				i.Write(Regions.Count);
				i.Write(Locale);
			}, (i) => {
				i.Seek(4);
				i.Read(ref Locale);
			}));
			riffs.Add(new RList("lrgn", (i) => {
				i.Write(Regions);
			}, (i) => {
				i.Read(Regions);
			}));
			riffs.Add(new RList("lart", (i) => {
				i.Write(Articulations);
			}, (i) => {
				i.Read(Articulations);
			}));
		}

		public INS(byte programNo = 0, byte bankMSB = 0, byte bankLSB = 0, bool isDrum = false) {
			Locale.BankFlg = (byte)(isDrum ? 0x80 : 0x00);
			Locale.ProgNum = programNo;
			Locale.BankMSB = bankMSB;
			Locale.BankLSB = bankLSB;
		}

		public INS(IntPtr ptr, long size) {
			Load(ptr, size);
		}

		protected override void LoadInfo(IntPtr ptr, string type, string value) {
			Info[type] = value;
		}
	}
}
