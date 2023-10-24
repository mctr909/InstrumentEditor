using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace DLS {
	public class WVPL : Riff {
		List<WAVE> mList = new List<WAVE>();

		public int Count { get { return mList.Count; } }

		protected override void Init(out string id, List<Chunk> chunks, List<LIST> riffs) {
			id = "wvpl";
			riffs.Add(new LIST("wave", (i) => {
				foreach (var wave in mList) {
					wave.Write(i);
				}
			}, (ptr, size) => {
				var ins = new WAVE(ptr, size);
				Add(ins);
			}));
		}

		public WAVE this[int index] {
			get { return mList[index]; }
		}

		public WVPL() { }

		public WVPL(IntPtr ptr, long size) {
			Load(ptr, size);
		}

		public void Add(WAVE wave) {
			mList.Add(wave);
		}

		public void AddRange(List<WAVE> waves) {
			mList.AddRange(waves);
		}

		public void Clear() {
			mList.Clear();
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
		public byte[] Data;
		public CK_WSMP Sampler;
		public List<WaveLoop> Loops = new List<WaveLoop>();

		protected override void Init(out string id, List<Chunk> chunks, List<LIST> riffs) {
			id = "wave";
			chunks.Add(new Chunk("fmt ", (i) => {
				i.Write(Format);
			}, (i) => {
				i.Read(ref Format);
			}));
			chunks.Add(new Chunk("data", (i) => {
				i.Write(Data);
			}, (i) => {
				i.Read(out Data);
			}));
			chunks.Add(new Chunk("wsmp", (i) => {
				i.Write(Sampler);
				i.Write(Loops);
			}, (i) => {
				i.Read(ref Sampler);
				i.Read(Loops);
			}));
		}

		public WAVE() { }

		public WAVE(IntPtr ptr, long size) {
			Load(ptr, size);
		}

		public WAVE(string filePath) {
			MainLoop(filePath);
		}

		public void ToFile(string filePath) {
			var fs = new FileStream(filePath, FileMode.Create);
			var bw = new BinaryWriter(fs);
			bw.Write("RIFF".ToCharArray());
			bw.Write((uint)0);
			bw.Write("WAVE".ToCharArray());
			Chunk.Save("fmt ", (i) => {
				i.Write(Format);
			}, bw);
			Chunk.Save("data", (i) => {
				i.Write(Data);
			}, bw);
			Chunk.Save("wsmp", (i) => {
				i.Write(Sampler);
				i.Write(Loops);
			}, bw);
			Info.Write(bw);
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

		protected override void LoadInfo(IntPtr ptr, string type, string value) {
			Info[type] = value;
		}
	}
}
