using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace INST {
    #region struct
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct WPTR {
        public UInt32 ofsHeader;
        public UInt32 ofsData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct WAVH {
        public UInt32 sampleRate;
        public UInt32 loopBegin;
        public UInt32 loopLength;
        public byte   loopEnable;
        public byte   unityNote;
        public UInt16 reserved;
        public double gain;
        public double pitch;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct INSH {
        public byte flag;
        public byte bankMSB;
        public byte bankLSB;
        public byte progNum;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct LYRH {
        public byte   keyHi;
        public byte   keyLo;
        public byte   velHi;
        public byte   velLo;
        public UInt32 instIdx;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct RGNH {
        public byte   keyHi;
        public byte   keyLo;
        public byte   velHi;
        public byte   velLo;
        public UInt32 waveIdx;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ART {
        public UInt32 type;
        public float  value;
    }
    #endregion

    public class File {
        public List<WaveInfo> waves = new List<WaveInfo>();
        public List<InstInfo> instList = new List<InstInfo>();

        public void Write(string path) {
            var fs = new FileStream(path, FileMode.Create);
            var bw = new BinaryWriter(fs);

            bw.Write(new char[] { 'R', 'I', 'F', 'F' });
            bw.Write(0xFFFFFFFF);
            bw.Write(new char[] { 'I', 'N', 'S', 'T' });

            {
                bw.Write(new char[] { 'w', 'p', 't', 'r' });
                bw.Write(Marshal.SizeOf<WPTR>() * waves.Count);
                var pos = -8;
                foreach (var wave in waves) {
                    var data = wave.Write();
                    bw.Write((uint)(pos + data.Key.ofsHeader));
                    bw.Write((uint)(pos + data.Key.ofsData));
                    pos += data.Value.Length;
                }
            }

            {
                bw.Write(new char[] { 'L', 'I', 'S', 'T' });
                var ofs = fs.Position;
                bw.Write(0xFFFFFFFF);
                bw.Write(new char[] { 'l', 'w', 'a', 'v' });
                foreach (var wave in waves) {
                    var data = wave.Write();
                    bw.Write(data.Value, 0, data.Value.Length);
                }
                var pos = fs.Position;
                fs.Seek(ofs, SeekOrigin.Begin);
                bw.Write((uint)(pos - ofs) - 4);
                fs.Seek(pos, SeekOrigin.Begin);
            }

            {
                bw.Write(new char[] { 'L', 'I', 'S', 'T' });
                var ofs = fs.Position;
                bw.Write(0xFFFFFFFF);
                bw.Write(new char[] { 'l', 'i', 'n', 's' });
                foreach (var inst in instList) {
                    var data = inst.Write();
                    bw.Write(data.Value, 0, data.Value.Length);
                }
                var pos = fs.Position;
                fs.Seek(ofs, SeekOrigin.Begin);
                bw.Write((uint)(pos - ofs) - 4);
                fs.Seek(pos, SeekOrigin.Begin);
            }

            fs.Seek(4, SeekOrigin.Begin);
            bw.Write((uint)fs.Length - 8);

            fs.Close();
            fs.Dispose();
        }
    }

    public class WaveInfo {
        public WAVH header;
        public Int16[] data = null;
        public Dictionary<string, string> infoList = new Dictionary<string, string>();

        public KeyValuePair<WPTR, byte[]> Write() {
            var msWave = new MemoryStream();
            var bwWave = new BinaryWriter(msWave);
            WPTR wptr;

            bwWave.Write(new char[] { 'L', 'I', 'S', 'T' });
            bwWave.Write(0xFFFFFFFF);
            bwWave.Write(new char[] { 'w', 'a', 'v', ' ' });

            {
                var size = Marshal.SizeOf<WAVH>();
                bwWave.Write(new char[] { 'w', 'a', 'v', 'h' });
                bwWave.Write(size);

                wptr.ofsHeader = (uint)msWave.Position;
                var ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(header, ptr, true);
                var arr = new byte[size];
                Marshal.Copy(ptr, arr, 0, size);
                bwWave.Write(arr);
                Marshal.FreeHGlobal(ptr);
            }

            {
                bwWave.Write(new char[] { 'd', 'a', 't', 'a' });
                bwWave.Write(data.Length * 2);

                wptr.ofsData = (uint)msWave.Position;
                var ptr = Marshal.AllocHGlobal(data.Length * 2);
                Marshal.Copy(data, 0, ptr, data.Length);
                var arr = new byte[data.Length * 2];
                Marshal.Copy(ptr, arr, 0, data.Length * 2);
                bwWave.Write(arr);
                Marshal.FreeHGlobal(ptr);
            }

            if (0 < infoList.Count) {
                var ms = new MemoryStream();
                var bw = new BinaryWriter(ms);
                bw.Write(new char[] { 'L', 'I', 'S', 'T' });
                bw.Write(0xFFFFFFFF);
                bw.Write(new char[] { 'I', 'N', 'F', 'O' });
                foreach (var info in infoList) {
                    var arrId = new byte[4];
                    Encoding.ASCII.GetBytes(info.Key, 0, 4, arrId, 0);
                    var arrText = Encoding.ASCII.GetBytes(info.Value);
                    bw.Write(arrId);
                    if (0 == arrText.Length % 2) {
                        bw.Write(arrText.Length);
                        bw.Write(arrText);
                    } else {
                        bw.Write(arrText.Length + 1);
                        bw.Write(arrText);
                        bw.Write((byte)0);
                    }
                }
                bw.Seek(4, SeekOrigin.Begin);
                bw.Write((uint)ms.Length - 8);

                bwWave.Write(ms.ToArray());
            }

            msWave.Seek(4, SeekOrigin.Begin);
            bwWave.Write((uint)msWave.Length - 8);

            return new KeyValuePair<WPTR, byte[]>(wptr, msWave.ToArray());
        }
    }

    public class InstInfo {
        public INSH header;
        public List<Layer> layers = new List<Layer>();
        public List<ART> arts = new List<ART>();
        public Dictionary<string, string> infoList = new Dictionary<string, string>();

        public KeyValuePair<uint, byte[]> Write() {
            var msInst = new MemoryStream();
            var bwInst = new BinaryWriter(msInst);

            bwInst.Write(new char[] { 'L', 'I', 'S', 'T' });
            bwInst.Write(0xFFFFFFFF);
            bwInst.Write(new char[] { 'i', 'n', 's', ' ' });

            {
                var size = Marshal.SizeOf<INSH>();
                bwInst.Write(new char[] { 'i', 'n', 's', 'h' });
                bwInst.Write(size);
                var ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(header, ptr, true);
                var arr = new byte[size];
                Marshal.Copy(ptr, arr, 0, size);
                bwInst.Write(arr);
            }

            if (0 < arts.Count) {
                var ms = new MemoryStream();
                var bw = new BinaryWriter(ms);
                bw.Write(new char[] { 'a', 'r', 't', ' ' });
                bw.Write(Marshal.SizeOf<ART>() * arts.Count);
                foreach (var art in arts) {
                    bw.Write(art.type);
                    bw.Write(art.value);
                }

                bwInst.Write(ms.ToArray());
            }

            if (0 < layers.Count) {
                var ms = new MemoryStream();
                var bw = new BinaryWriter(ms);
                var size = 0;
                bw.Write(new char[] { 'L', 'I', 'S', 'T' });
                bw.Write(0xFFFFFFFF);
                bw.Write(new char[] { 'l', 'l', 'y', 'r' });
                foreach (var layer in layers) {
                    var arr = layer.Write();
                    size += arr.Length;
                    ms.Write(arr, 0, arr.Length);
                }
                bw.Seek(4, SeekOrigin.Begin);
                bw.Write(size + 4);

                bwInst.Write(ms.ToArray());
            }

            if (0 < infoList.Count) {
                var ms = new MemoryStream();
                var bw = new BinaryWriter(ms);
                bw.Write(new char[] { 'L', 'I', 'S', 'T' });
                bw.Write(0xFFFFFFFF);
                bw.Write(new char[] { 'I', 'N', 'F', 'O' });
                foreach (var info in infoList) {
                    var arrId = new byte[4];
                    Encoding.ASCII.GetBytes(info.Key, 0, 4, arrId, 0);
                    var arrText = Encoding.ASCII.GetBytes(info.Value);
                    bw.Write(arrId);
                    if (0 == arrText.Length % 2) {
                        bw.Write(arrText.Length);
                        bw.Write(arrText);
                    } else {
                        bw.Write(arrText.Length + 1);
                        bw.Write(arrText);
                        bw.Write((byte)0);
                    }
                }
                bw.Seek(4, SeekOrigin.Begin);
                bw.Write((uint)ms.Length - 8);

                bwInst.Write(ms.ToArray());
            }

            msInst.Seek(4, SeekOrigin.Begin);
            bwInst.Write((uint)msInst.Length - 8);
            return new KeyValuePair<uint, byte[]>(0, msInst.ToArray());
        }
    }

    public class Layer {
        public LYRH header;
        public List<Region> regions = new List<Region>();
        public List<ART> arts = new List<ART>();

        public byte[] Write() {
            var msLayer = new MemoryStream();
            var bwLayer = new BinaryWriter(msLayer);

            bwLayer.Write(new char[] { 'L', 'I', 'S', 'T' });
            bwLayer.Write(0xFFFFFFFF);
            bwLayer.Write(new char[] { 'l', 'y', 'r', ' ' });

            {
                var size = Marshal.SizeOf<LYRH>();
                bwLayer.Write(new char[] { 'l', 'y', 'r', 'h' });
                bwLayer.Write(size);
                var ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(header, ptr, true);
                var arr = new byte[size];
                Marshal.Copy(ptr, arr, 0, size);
                bwLayer.Write(arr);
            }

            if (0 < arts.Count) {
                bwLayer.Write(new char[] { 'a', 'r', 't', ' ' });
                bwLayer.Write(Marshal.SizeOf<ART>() * arts.Count);
                foreach (var art in arts) {
                    bwLayer.Write(art.type);
                    bwLayer.Write(art.value);
                }
            }

            if (0 < regions.Count) {
                var ms = new MemoryStream();
                var bw = new BinaryWriter(ms);
                var size = 0;
                bw.Write(new char[] { 'L', 'I', 'S', 'T' });
                bw.Write(0xFFFFFFFF);
                bw.Write(new char[] { 'l', 'r', 'g', 'n' });
                foreach (var region in regions) {
                    var arr = region.Write();
                    size += arr.Length;
                    ms.Write(arr, 0, arr.Length);
                }
                bw.Seek(4, SeekOrigin.Begin);
                bw.Write(size + 4);

                bwLayer.Write(ms.ToArray());
            }

            msLayer.Seek(4, SeekOrigin.Begin);
            bwLayer.Write((uint)msLayer.Length - 8);
            return msLayer.ToArray();
        }
    }

    public class Region {
        public RGNH header;
        public List<ART> arts = new List<ART>();

        public byte[] Write() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var rgnhSize = Marshal.SizeOf<RGNH>();
            var artSize = Marshal.SizeOf<ART>() * arts.Count;

            if (0 < artSize) {
                bw.Write(new char[] { 'L', 'I', 'S', 'T' });
                bw.Write(rgnhSize + artSize + 20);
                bw.Write(new char[] { 'r', 'g', 'n', ' ' });

                bw.Write(new char[] { 'r', 'g', 'n', 'h' });
                bw.Write(rgnhSize);
                var ptr = Marshal.AllocHGlobal(rgnhSize);
                Marshal.StructureToPtr(header, ptr, true);
                var arr = new byte[rgnhSize];
                Marshal.Copy(ptr, arr, 0, rgnhSize);
                bw.Write(arr);

                bw.Write(new char[] { 'a', 'r', 't', ' ' });
                bw.Write(artSize);
                foreach(var art in arts) {
                    bw.Write(art.type);
                    bw.Write(art.value);
                }
            } else {
                bw.Write(new char[] { 'L', 'I', 'S', 'T' });
                bw.Write(rgnhSize + 12);
                bw.Write(new char[] { 'r', 'g', 'n', ' ' });

                bw.Write(new char[] { 'r', 'g', 'n', 'h' });
                bw.Write(rgnhSize);
                var ptr = Marshal.AllocHGlobal(rgnhSize);
                Marshal.StructureToPtr(header, ptr, true);
                var arr = new byte[rgnhSize];
                Marshal.Copy(ptr, arr, 0, rgnhSize);
                bw.Write(arr);
            }

            return ms.ToArray();
        }
    }
}
