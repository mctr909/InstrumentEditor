using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace DLS {
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct WLNK {
		public ushort Options;
		public ushort PhaseGroup;
		public uint Channel;
		public uint TableIndex;
	}

	public class LRGN : Riff {
		public sealed class Sort : IComparer<RGN.HEADER> {
			// IComparerの実装
			public int Compare(RGN.HEADER x, RGN.HEADER y) {
				var keyH = x.KeyHi < y.KeyLo;
				var keyL = y.KeyHi < x.KeyLo;
				var velH = x.VelHi < y.VelLo;
				var velL = y.VelHi < x.VelLo;
				var key = keyH || keyL;
				var vel = velH || velL;
				if (key || vel) {
					if (keyH) {
						return 1;
					}
					if (velH) {
						return 1;
					}
					return -1;
				} else {
					return 0;
				}
			}
		}

		public SortedList<RGN.HEADER, RGN> List = new SortedList<RGN.HEADER, RGN>(new Sort());

		public LRGN() { }

		public LRGN(IntPtr ptr, long size) {
			Load(ptr, size);
		}

		public override void Write(BinaryWriter bw) {
			var msList = new MemoryStream();
			var bwList = new BinaryWriter(msList);
			foreach (var rgn in List) {
				rgn.Value.Write(bwList);
			}

			if (0 < msList.Length) {
				bw.Write("LIST".ToCharArray());
				bw.Write((uint)(msList.Length + 4));
				bw.Write("lrgn".ToCharArray());
				bw.Write(msList.ToArray());
			}
		}

		public void Clear() {
			List.Clear();
		}

		public int Count {
			get { return List.Count; }
		}

		public IList<RGN> Array {
			get { return List.Values; }
		}

		public RGN this[int index] {
			get { return List.Values[index]; }
		}

		public bool Add(RGN region) {
			if (List.ContainsKey(region.Header)) {
				return false;
			}
			List.Add(region.Header, region);
			return true;
		}

		public List<RGN> Find(RGN.HEADER header) {
			var ret = new List<RGN>();
			foreach (var rng in List.Values) {
				if (header.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= header.KeyHi &&
					header.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= header.VelHi) {
					ret.Add(rng);
				}
			}
			return ret;
		}

		public RGN Find(int noteNo, int velocity) {
			foreach (var rng in List.Values) {
				if (noteNo <= rng.Header.KeyHi && rng.Header.KeyLo <= noteNo &&
					velocity <= rng.Header.VelHi && rng.Header.VelLo <= velocity) {
					return rng;
				}
			}
			return null;
		}

		public RGN FindFirst(RGN.HEADER header) {
			foreach (var rng in List.Values) {
				if (header.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= header.KeyHi &&
					header.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= header.VelHi) {
					return rng;
				}
			}
			return null;
		}

		public bool ContainsKey(RGN.HEADER header) {
			foreach (var rng in List.Values) {
				if (header.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= header.KeyHi &&
					header.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= header.VelHi) {
					return true;
				}
			}
			return false;
		}

		public bool ContainsKey(int noteNo, int velocity) {
			foreach (var rng in List.Values) {
				if (noteNo <= rng.Header.KeyHi && rng.Header.KeyLo <= noteNo &&
					velocity <= rng.Header.VelHi && rng.Header.VelLo <= velocity) {
					return true;
				}
			}
			return false;
		}

		public void Remove(RGN.HEADER header) {
			var tmpRegion = new List<RGN>();
			foreach (var rng in List.Values) {
				if (header.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= header.KeyHi &&
					header.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= header.VelHi) {
				} else {
					tmpRegion.Add(rng);
				}
			}
			List.Clear();
			foreach (var rgn in tmpRegion) {
				List.Add(rgn.Header, rgn);
			}
		}

		protected override void LoadChunk(IntPtr ptr, string type, long size) {
			switch (type) {
			case "rgn ":
				var rgn = new RGN(ptr, size);
				if (!List.ContainsKey(rgn.Header)) {
					List.Add(rgn.Header, rgn);
				}
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkType [{0}]", type));
			}
		}
	}

	public class RGN : Riff {
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		public struct HEADER {
			public ushort KeyLo;
			public ushort KeyHi;
			public ushort VelLo;
			public ushort VelHi;
			public ushort Options;
			public ushort KeyGroup;
			public ushort Layer;
		}

		public HEADER Header;
		public CK_WSMP Sampler;
		public List<WaveLoop> Loops = new List<WaveLoop>();
		public WLNK WaveLink;
		public LART Articulations = new LART();

		Chunk cRgnh;
		Chunk cWlnk;
		Chunk cWsmp;

		void set() {
			cRgnh = new Chunk("rgnh", (i) => {
				i.Write(Header);
			}, (i) => {
				i.Read(ref Header);
			});
			cWlnk = new Chunk("wlnk", (i) => {
				i.Write(WaveLink);
			}, (i) => {
				i.Read(ref WaveLink);
			});
			cWsmp = new Chunk("wsmp", (i) => {
				i.Write(Sampler);
				i.Write(Loops);
			}, (i) => {
				i.Read(ref Sampler);
				i.Read(Loops);
			});
		}

		public RGN() {
			set();
			Header.KeyLo = 0;
			Header.KeyHi = 127;
			Header.VelLo = 0;
			Header.VelHi = 127;
		}

		public RGN(IntPtr ptr, long size) {
			set();
			Load(ptr, size);
		}

		public override void Write(BinaryWriter bw) {
			var msRgn = new MemoryStream();
			var bwRgn = new BinaryWriter(msRgn);
			bwRgn.Write("LIST".ToCharArray());
			bwRgn.Write(0xFFFFFFFF);
			bwRgn.Write("rgn ".ToCharArray());

			cRgnh.Save(bwRgn);
			cWlnk.Save(bwRgn);
			cWsmp.Save(bwRgn);
			Articulations.Write(bwRgn);

			bwRgn.Seek(4, SeekOrigin.Begin);
			bwRgn.Write((uint)msRgn.Length - 8);
			bw.Write(msRgn.ToArray());
		}

		protected override void LoadChunk(IntPtr ptr, string type, long size) {
			switch (type) {
			case "rgnh":
				cRgnh.Load(ptr, size);
				break;
			case "wsmp":
				cWsmp.Load(ptr, size);
				break;
			case "wlnk":
				cWlnk.Load(ptr, size);
				break;
			case "lart":
			case "lar2":
				Articulations = new LART(ptr, size);
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkType [{0}]", type));
			}
		}
	}
}