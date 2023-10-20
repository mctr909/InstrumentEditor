﻿using System;
using System.Collections.Generic;

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

		protected override void Init(out string id, List<Chunk> chunks, List<LIST> riffs) {
			id = "lins";
			riffs.Add(new LIST("ins ", (i) => {
				foreach (var ins in List.Values) {
					ins.Write(i);
				}
			}, (ptr, size) => {
				var ins = new INS(ptr, size);
				List.Add(ins.Locale, ins);
			}));
		}

		public LINS() { }

		public LINS(IntPtr ptr, long size) {
			Load(ptr, size);
		}
	}

	public class INS : Riff {
		public MidiLocale Locale;
		public LRGN Regions = new LRGN();
		public LART Articulations = new LART();

		protected override void Init(out string id, List<Chunk> chunks, List<LIST> riffs) {
			id = "ins ";
			chunks.Add(new Chunk("insh", (i) => {
				i.Write(Regions.List.Count);
				i.Write(Locale);
			}, (i) => {
				i.Seek(4);
				i.Read(ref Locale);
			}));
			riffs.Add(new LIST("lrgn", (i) => {
				Regions.Write(i);
			}, (ptr, size) => {
				Regions = new LRGN(ptr, size);
			}));
			riffs.Add(new LIST("lart", (i) => {
				Articulations.Write(i);
			}, (ptr, size) => {
				Articulations = new LART(ptr, size);
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
