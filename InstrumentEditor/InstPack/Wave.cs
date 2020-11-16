using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

using Riff;

namespace InstPack {
    public class LWave {
        private List<Wave> List = new List<Wave>();

        public LWave() { }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public Wave this[int index] {
            get { return List[index]; }
        }

        public Wave[] ToArray() {
            return List.ToArray();
        }

        public void Add(Wave wave) {
            List.Add(wave);
        }

        public void AddRange(List<Wave> waves) {
            foreach (var wave in waves) {
                List.Add(wave);
            }
        }

        public bool ContainsKey(int index) {
            return 0 <= index && index < List.Count;
        }
    }

    public class Wave {
        public WAVH Header;
        public short[] Data = null;
        public Info Info = new Info();

        private FMT mFormat;

        public Wave() { }

        public Wave(string filePath) {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            var riff = br.ReadUInt32();
            var riffSize = br.ReadUInt32();
            var riffType = br.ReadUInt32();

            var arrData = new byte[0];

            while (fs.Position < fs.Length) {
                var chunkType = Encoding.ASCII.GetString(br.ReadBytes(4));
                var chunkSize = br.ReadUInt32();
                var pChunkData = Marshal.AllocHGlobal((int)chunkSize);
                Marshal.Copy(br.ReadBytes((int)chunkSize), 0, pChunkData, (int)chunkSize);

                switch (chunkType) {
                case "fmt ":
                    mFormat = Marshal.PtrToStructure<FMT>(pChunkData);
                    break;
                case "data":
                    arrData = new byte[chunkSize];
                    Marshal.Copy(pChunkData, arrData, 0, (int)chunkSize);
                    break;
                case "wavh":
                    Header = Marshal.PtrToStructure<WAVH>(pChunkData);
                    break;
                case "LIST":
                    switch (Marshal.PtrToStringAnsi(pChunkData, 4)) {
                    case "INFO":
                        Info = new Info(pChunkData + 4, pChunkData + (int)chunkSize);
                        break;
                    }
                    break;
                default:
                    break;
                }
                Marshal.FreeHGlobal(pChunkData);
            }

            var msData = new MemoryStream(arrData);
            var brData = new BinaryReader(msData);

            switch (mFormat.Bits) {
            case 8:
                Data = new short[msData.Length];
                for (var i = 0; msData.Position < msData.Length; i++) {
                    var data = brData.ReadByte() - 128;
                    Data[i] = (short)(256 * data);
                }
                break;
            case 16:
                Data = new short[msData.Length / 2];
                for (var i = 0; msData.Position < msData.Length; i++) {
                    Data[i] = brData.ReadInt16();
                }
                break;
            case 24:
                Data = new short[msData.Length / 3];
                for (var i = 0; msData.Position < msData.Length; i++) {
                    brData.ReadByte();
                    Data[i] = brData.ReadInt16();
                }
                break;
            case 32:
                Data = new short[msData.Length / 4];
                if (mFormat.Tag == 3) {
                    for (var i = 0; msData.Position < msData.Length; i++) {
                        var data = brData.ReadSingle();
                        if (1.0 < data) data = 1.0f;
                        if (data < -1.0) data = -1.0f;
                        Data[i] = (short)(32767 * data);
                    }
                } else {
                    for (var i = 0; msData.Position < msData.Length; i++) {
                        brData.ReadInt16();
                        Data[i] = brData.ReadInt16();
                    }
                }
                break;
            }

            if (0 == Header.SampleRate) {
                Header.SampleRate = mFormat.SampleRate;
                Header.Gain = 1.0;
                Header.Pitch = 1.0;
            }

            if (null == Info) {
                Info = new Info {
                    Name = Path.GetFileNameWithoutExtension(filePath)
                };
            } else if (string.IsNullOrWhiteSpace(Info.Name)) {
                Info.Name = Path.GetFileNameWithoutExtension(filePath);
            }

            br.Dispose();
            fs.Close();
            fs.Dispose();
        }

        public void ToFile(string filePath) {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write("RIFF".ToCharArray());
            bw.Write((uint)0);
            bw.Write("WAVE".ToCharArray());

            {
                // fmt chunk
                bw.Write("fmt ".ToCharArray());
                bw.Write((uint)16);
                bw.Write((ushort)1);
                bw.Write((ushort)1);
                bw.Write(Header.SampleRate);
                bw.Write(Header.SampleRate * 2);
                bw.Write((ushort)2);
                bw.Write((ushort)16);
            }

            {
                // data chunk
                var ptr = Marshal.AllocHGlobal(Data.Length * 2);
                Marshal.Copy(Data, 0, ptr, Data.Length);
                var arr = new byte[Data.Length * 2];
                Marshal.Copy(ptr, arr, 0, Data.Length * 2);
                Marshal.FreeHGlobal(ptr);
                bw.Write("data".ToCharArray());
                bw.Write(arr.Length);
                bw.Write(arr);
            }

            Header.Write(bw);
            Info.Write(bw);

            fs.Seek(4, SeekOrigin.Begin);
            bw.Write((uint)(fs.Length - 8));

            bw.Dispose();
            fs.Close();
            fs.Dispose();
        }
    }
}
