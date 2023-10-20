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

		Chunk cInsh;

		void set() {
			cInsh = new Chunk("insh", (i) => {
				i.Write(Regions.Count);
				i.Write(Locale);
			}, (i) => {
				i.Seek(4);
				i.Read(ref Locale);
			});
		}

		public INS(byte programNo = 0, byte bankMSB = 0, byte bankLSB = 0, bool isDrum = false) {
			set();
			Locale.BankFlg = (byte)(isDrum ? 0x80 : 0x00);
			Locale.ProgNum = programNo;
			Locale.BankMSB = bankMSB;
			Locale.BankLSB = bankLSB;
		}

		public INS(IntPtr ptr, long size) {
			set();
			Load(ptr, size);
		}

		public override void Write(BinaryWriter bw) {
			var msIns = new MemoryStream();
			var bwIns = new BinaryWriter(msIns);
			bwIns.Write("LIST".ToCharArray());
			bwIns.Write(0xFFFFFFFF);
			bwIns.Write("ins ".ToCharArray());

			cInsh.Save(bwIns);

			Regions.Write(bwIns);
			Articulations.Write(bwIns);

			Info.Write(bwIns);

			bwIns.Seek(4, SeekOrigin.Begin);
			bwIns.Write((uint)msIns.Length - 8);
			bw.Write(msIns.ToArray());
		}

		protected override void LoadChunk(IntPtr ptr, string type, long size) {
			switch (type) {
			case "insh":
				cInsh.Load(ptr, size);
				break;
			case "lrgn":
				Regions = new LRGN(ptr, size);
				break;
			case "lart":
			case "lar2":
				Articulations = new LART(ptr, size);
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkType [{0}]", type));
			}
		}

		protected override void LoadInfo(IntPtr ptr, string type, string value) {
			Info[type] = value;
		}
	}
}
