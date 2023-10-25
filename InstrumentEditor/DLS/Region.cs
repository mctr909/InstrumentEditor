using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLS {
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

		protected override void Initialize(out string id, List<Chunk> chunks, List<LIST> riffs) {
			id = "lrgn";
			riffs.Add(new LIST("rgn ", (i) => {
				foreach (var rgn in List.Values) {
					rgn.Write(i);
				}
			}, (ptr, size) => {
				var rgn = new RGN(ptr, size);
				List.Add(rgn.Header, rgn);
			}));
		}

		public LRGN() { }

		public LRGN(IntPtr ptr, long size) : base(ptr, size) { }

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
		}
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct WLNK {
			public ushort Options;
			public ushort PhaseGroup;
			public uint Channel;
			public uint TableIndex;
		}

		public HEADER Header;
		public ushort Layer;
		public WLNK WaveLink;
		public WSMP Sampler;
		public List<WaveLoop> Loops = new List<WaveLoop>();
		public LART Articulations = new LART();

		protected override void Initialize(out string id, List<Chunk> chunks, List<LIST> riffs) {
			id = "rgn ";
			chunks.Add(new Chunk("rgnh", (i) => {
				i.Write(Header);
				if (0 != Layer) {
					i.Write(Layer);
				}
			}, (i) => {
				i.Read(ref Header);
				i.Read(ref Layer);
			}));
			chunks.Add(new Chunk("wlnk", (i) => {
				i.Write(WaveLink);
			}, (i) => {
				i.Read(ref WaveLink);
			}));
			chunks.Add(new Chunk("wsmp", (i) => {
				i.Write(Sampler);
				i.Write(Loops.Count);
				i.Write(Loops);
			}, (i) => {
				i.Read(ref Sampler);
				i.Seek(4);
				i.Read(Loops);
			}));
			riffs.Add(new LIST("lart", (i) => {
				Articulations.Write(i);
			}, (ptr, size) => {
				Articulations = new LART(ptr, size);
			}));
		}

		public RGN() {
			Header.KeyLo = 0;
			Header.KeyHi = 127;
			Header.VelLo = 0;
			Header.VelHi = 127;
		}

		public RGN(IntPtr ptr, long size) : base(ptr, size) { }
	}
}
