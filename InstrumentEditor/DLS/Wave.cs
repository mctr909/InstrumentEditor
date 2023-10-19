using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace DLS {
	public class WVPL : Riff {
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct PTBL {
			public uint Size;
			public uint Count;

			public void Write(BinaryWriter bw) {
				bw.Write(Size);
				bw.Write(Count);
			}
		}

		public List<WAVE> List = new List<WAVE>();

		public WVPL() { }

		public WVPL(IntPtr ptr, long size) {
			Load(ptr, size);
		}

		public override void Write(BinaryWriter bw) {
			var msPtbl = new MemoryStream();
			var bwPtbl = new BinaryWriter(msPtbl);
			bwPtbl.Write("ptbl".ToCharArray());
			bwPtbl.Write((uint)(Marshal.SizeOf<PTBL>() + List.Count * sizeof(uint)));
			bwPtbl.Write((uint)8);
			bwPtbl.Write((uint)List.Count);

			var msWave = new MemoryStream();
			var bwWave = new BinaryWriter(msWave);
			foreach (var wave in List) {
				bwPtbl.Write((uint)msWave.Position);
				wave.Write(bwWave);
			}

			if (0 < msPtbl.Length) {
				bw.Write(msPtbl.ToArray());
			}

			if (0 < msWave.Length) {
				bw.Write("LIST".ToCharArray());
				bw.Write((uint)(msWave.Length + 4));
				bw.Write("wvpl".ToCharArray());
				bw.Write(msWave.ToArray());
			}
		}

		protected override void LoadChunk(IntPtr ptr, string type, long size) {
			switch (type) {
			case "wave":
				List.Add(new WAVE(ptr, size));
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkType [{0}]", type));
			}
		}
	}

	public class WAVE : Riff {
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct FMT {
			public ushort Tag;
			public ushort Channels;
			public uint SampleRate;
			public uint BytesPerSec;
			public ushort BlockAlign;
			public ushort Bits;
		}

		public FMT Format;
		public CK_WSMP Sampler;
		public List<WaveLoop> Loops = new List<WaveLoop>();
		public byte[] Data;
		public Info Info = new Info();

		Chunk cFmt;
		Chunk cWsmp;

		void set() {
			cFmt = new Chunk("fmt ", (i) => {
				i.Write(Format);
			}, (i) => {
				i.Read(ref Format);
			});
			cWsmp = new Chunk("wsmp", (i) => {
				i.Write(Sampler);
				i.Write(Loops);
			}, (i) => {
				i.Read(ref Sampler);
				i.Read(Loops);
			});
		}

		public WAVE() {
			set();
		}

		public WAVE(IntPtr ptr, long size) {
			set();
			Load(ptr, size);
		}

		public WAVE(string filePath) {
			set();
			MainLoop(filePath);
		}

		public override void Write(BinaryWriter bw) {
			var msSmp = new MemoryStream();
			var bwSmp = new BinaryWriter(msSmp);
			bwSmp.Write("LIST".ToCharArray());
			bwSmp.Write(0xFFFFFFFF);
			bwSmp.Write("wave".ToCharArray());

			cFmt.Save(bwSmp);

			bwSmp.Write("data".ToCharArray());
			bwSmp.Write(Data.Length);
			bwSmp.Write(Data);

			Info.Write(bwSmp);
			cWsmp.Save(bwSmp);

			bwSmp.Seek(4, SeekOrigin.Begin);
			bwSmp.Write((uint)msSmp.Length - 8);
			bw.Write(msSmp.ToArray());
		}

		public void ToFile(string filePath) {
			var fs = new FileStream(filePath, FileMode.Create);
			var bw = new BinaryWriter(fs);
			bw.Write("RIFF".ToCharArray());
			bw.Write((uint)0);
			bw.Write("WAVE".ToCharArray());

			cFmt.Save(bw);
			{
				// data chunk
				var ptr = Marshal.AllocHGlobal(Data.Length);
				Marshal.Copy(Data, 0, ptr, Data.Length);
				var arr = new byte[Data.Length];
				Marshal.Copy(ptr, arr, 0, Data.Length);
				Marshal.FreeHGlobal(ptr);
				bw.Write("data".ToCharArray());
				bw.Write(arr.Length);
				bw.Write(arr);
			}
			Info.Write(bw);
			cWsmp.Save(bw);

			fs.Seek(4, SeekOrigin.Begin);
			bw.Write((uint)(fs.Length - 8));

			bw.Dispose();
			fs.Close();
			fs.Dispose();
		}

		public float[] GetFloat(int packSize = 0) {
			var samples = Data.Length * 8 / Format.Bits;
			var len = samples;
			if (0 < packSize) {
				len += packSize * 2 - (samples % (packSize * 2));
			}
			var ret = new float[len];
			switch (Format.Bits) {
			case 8:
				for (int s = 0; s < samples; s++) {
					ret[s] = (Data[s] - 128) / 128.0f;
				}
				return ret;
			case 16:
				for (int s = 0, i2 = 0; s < samples; s++, i2 += 2) {
					ret[s] = (short)(Data[i2] | Data[i2 + 1] << 8) / 32768.0f;
				}
				return ret;
			case 24:
				for (int s = 0, i3 = 0; s < samples; s++, i3 += 3) {
					ret[s] = (short)(Data[i3 + 1] | Data[i3 + 2] << 8) / 32768.0f;
				}
				return ret;
			case 32:
				if (Format.Tag == 3) {
					for (int s = 0, i4 = 0; s < samples; s++, i4 += 4) {
						ret[s] = BitConverter.ToSingle(Data, i4);
					}
				} else {
					for (int s = 0, i4 = 0; s < samples; s++, i4 += 4) {
						ret[s] = (short)(Data[i4 + 2] | Data[i4 + 3] << 8) / 32768.0f;
					}
				}
				return ret;
			default:
				return ret;
			}
		}

		public void To16bit() {
			if (16 == Format.Bits) {
				return;
			}
			var samples = Data.Length * 8 / Format.Bits;
			var tmpArr = new byte[samples * 2];
			switch (Format.Bits) {
			case 8:
				for (int s = 0, i2 = 0; s < samples; s++, i2 += 2) {
					var val = (short)((Data[s] - 128) * 32767 / 128);
					tmpArr[i2] = (byte)(val & 0xFF);
					tmpArr[i2 + 1] = (byte)((val & 0xFF00) >> 8);
				}
				Data = tmpArr;
				break;
			case 24:
				for (int s = 0, i2 = 0, i3 = 0; s < samples; s++, i2 += 2, i3 += 3) {
					var val = (short)(Data[i3 + 1] | Data[i3 + 2] << 8);
					tmpArr[i2] = (byte)(val & 0xFF);
					tmpArr[i2 + 1] = (byte)((val & 0xFF00) >> 8);
				}
				Data = tmpArr;
				break;
			case 32:
				if (Format.Tag == 3) {
					for (int s = 0, i2 = 0, i4 = 0; s < samples; s++, i2 += 2, i4 += 4) {
						var vf = BitConverter.ToSingle(Data, i4);
						if (vf < -1.0f) {
							vf = -1.0f;
						}
						if (1.0f < vf) {
							vf = 1.0f;
						}
						var val = (short)(vf * 32767);
						tmpArr[i2] = (byte)(val & 0xFF);
						tmpArr[i2 + 1] = (byte)((val & 0xFF00) >> 8);
					}
				} else {
					for (int s = 0, i2 = 0, i4 = 0; s < samples; s++, i2 += 2, i4 += 4) {
						var val = (short)(Data[i4 + 2] | Data[i4 + 3] << 8);
						tmpArr[i2] = (byte)(val & 0xFF);
						tmpArr[i2 + 1] = (byte)((val & 0xFF00) >> 8);
					}
				}
				Data = tmpArr;
				break;
			default:
				break;
			}
			Format.Tag = 1;
			Format.Bits = 16;
			Format.BlockAlign = (ushort)(Format.Bits * Format.Channels >> 3);
			Format.BytesPerSec = Format.BlockAlign * Format.SampleRate;
		}

		protected override void LoadChunk(IntPtr ptr, string type, long size) {
			switch (type) {
			case "dlid":
			case "guid":
				break;
			case "fmt ":
				cFmt.Load(ptr, size);
				break;
			case "data":
				Data = new byte[size];
				Marshal.Copy(ptr, Data, 0, Data.Length);
				break;
			case "wsmp":
				cWsmp.Load(ptr, size);
				break;
			default:
				break;
			}
		}

		protected override void LoadInfo(IntPtr ptr, string type, string value) {
			Info[type] = value;
		}
	}
}
