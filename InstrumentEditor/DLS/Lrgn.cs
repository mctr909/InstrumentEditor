using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

namespace DLS {
    public sealed class LRGNSort : IComparer<CK_RGNH> {
        // IComparerの実装
        public int Compare(CK_RGNH x, CK_RGNH y) {
            var xKey = (x.Key.Low << 24) | (x.Key.High << 16) | (x.Velocity.Low << 8) | x.Velocity.High;
            var yKey = (y.Key.Low << 24) | (y.Key.High << 16) | (y.Velocity.Low << 8) | y.Velocity.High;
            return xKey - yKey;
        }
    }

    unsafe public class LRGN : RIFF {
        public SortedDictionary<CK_RGNH, RGN> List = new SortedDictionary<CK_RGNH, RGN>(new LRGNSort());

        public LRGN() { }

        public LRGN(byte* ptr, byte* endPtr) : base(ptr, endPtr) { }

        protected override void ReadList(byte* ptr, byte* endPtr) {
            switch ((LIST_TYPE)mListType) {
                case LIST_TYPE.RGN_:
                    var rgn = new RGN(ptr, endPtr);
                    List.Add(rgn.Header, rgn);
                    break;
                default:
                    throw new Exception(string.Format("Unknown ListType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes(mListType))));
            }
        }

        public new byte[] Bytes {
            get {
                var ms = new MemoryStream();
                var bw = new BinaryWriter(ms);
                foreach (var rgn in List) {
                    bw.Write(rgn.Value.Bytes);
                }

                var ms2 = new MemoryStream();
                var bw2 = new BinaryWriter(ms2);
                if (0 < ms.Length) {
                    bw2.Write((uint)CHUNK_TYPE.LIST);
                    bw2.Write((uint)(ms.Length + 4));
                    bw2.Write((uint)LIST_TYPE.LRGN);
                    bw2.Write(ms.ToArray());
                }

                return ms2.ToArray();
            }
        }
    }

    unsafe public class RGN : RIFF {
        public CK_RGNH Header;
        public CK_WSMP Sampler;
        public Dictionary<int, WaveLoop> Loops = new Dictionary<int, WaveLoop>();
        public CK_WLNK WaveLink;
        public LART Articulations = new LART();

        public RGN() { }

        public RGN(byte noteLow = 0, byte noteHigh = 127, byte velocityLow = 0, byte velocityHigh = 127) {
            Header.Key.Low = noteLow;
            Header.Key.High = noteHigh;
            Header.Velocity.Low = velocityLow;
            Header.Velocity.High = velocityHigh;
        }

        public RGN(byte* ptr, byte* endPtr) : base(ptr, endPtr) { }

        protected override void ReadChunk(byte* ptr) {
            switch ((CHUNK_TYPE)mChunkType) {
                case CHUNK_TYPE.RGNH:
                    Header = (CK_RGNH)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_RGNH));
                    if (mChunkSize < sizeof(CK_RGNH)) {
                        Header.Layer = 0;
                    }
                    break;
                case CHUNK_TYPE.WSMP:
                    Sampler = (CK_WSMP)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_WSMP));
                    var pLoop = ptr + sizeof(CK_WSMP);
                    for (var i = 0; i < Sampler.LoopCount; ++i) {
                        Loops.Add(Loops.Count, (WaveLoop)Marshal.PtrToStructure((IntPtr)pLoop, typeof(WaveLoop)));
                        pLoop += sizeof(WaveLoop);
                    }
                    break;
                case CHUNK_TYPE.WLNK:
                    WaveLink = (CK_WLNK)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_WLNK));
                    break;
                default:
                    throw new Exception(string.Format("Unknown ChunkType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes(mChunkType))));
            }
        }

        protected override void ReadList(byte* ptr, byte* endPtr) {
            switch ((LIST_TYPE)mListType) {
                case LIST_TYPE.LART:
                case LIST_TYPE.LAR2:
                    Articulations = new LART(ptr, endPtr);
                    break;
                default:
                    throw new Exception(string.Format("Unknown ListType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes(mListType))));
            }
        }

        protected override void WriteChunk(BinaryWriter bw) {
            mListType = (uint)LIST_TYPE.RGN_;

            var data = Header.Bytes;
            bw.Write((uint)CHUNK_TYPE.RGNH);
            bw.Write(data.Length);
            bw.Write(data);

            data = Sampler.Bytes;
            bw.Write((uint)CHUNK_TYPE.WSMP);
            bw.Write((uint)(data.Length + Sampler.LoopCount * sizeof(WaveLoop)));
            bw.Write(data);
            for (var i = 0; i < Sampler.LoopCount && i < Loops.Count; ++i) {
                bw.Write(Loops[i].Bytes);
            }

            data = WaveLink.Bytes;
            bw.Write((uint)CHUNK_TYPE.WLNK);
            bw.Write(data.Length);
            bw.Write(data);
        }

        protected override void WriteList(BinaryWriter bw) {
            bw.Write(Articulations.Bytes);
        }
    }
}