using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace DLS {
    unsafe public class DLS : RIFF {
        private CK_VERS mVersion;
        private uint mMSYN = 1;

        public LINS Instruments = new LINS();
        public WVPL WavePool = new WVPL();
        public INFO Text = new INFO();

        public DLS() { }

        public DLS(byte* ptr, byte* endPtr) : base(ptr, endPtr) { }

        protected override void ReadChunk(byte* ptr) {
            switch ((CHUNK_TYPE)mChunkType) {
            case CHUNK_TYPE.COLH:
                break;
            case CHUNK_TYPE.VERS:
                mVersion = (CK_VERS)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_VERS));
                break;
            case CHUNK_TYPE.MSYN:
                break;
            case CHUNK_TYPE.PTBL:
                break;
            case CHUNK_TYPE.DLID:
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes(mChunkType))));
            }
        }

        protected override void ReadList(byte* ptr, byte* endPtr) {
            switch ((LIST_TYPE)mListType) {
            case LIST_TYPE.LINS:
                Instruments = new LINS(ptr, endPtr);
                break;
            case LIST_TYPE.WVPL:
                WavePool = new WVPL(ptr, endPtr);
                break;
            case LIST_TYPE.INFO:
                Text = new INFO(ptr, endPtr);
                break;
            default:
                throw new Exception(string.Format("Unknown ListType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes(mListType))));
            }
        }

        public void Save(string filePath) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);

            // "DLS "
            bw.Write((uint)0x20534C44);

            bw.Write((uint)CHUNK_TYPE.COLH);
            bw.Write((uint)4);
            bw.Write((uint)Instruments.List.Count);

            bw.Write((uint)CHUNK_TYPE.VERS);
            bw.Write((uint)sizeof(CK_VERS));
            bw.Write(mVersion.Bytes);

            bw.Write((uint)CHUNK_TYPE.MSYN);
            bw.Write((uint)4);
            bw.Write(mMSYN);

            bw.Write(Instruments.Bytes);
            bw.Write(WavePool.Bytes);
            bw.Write(Text.Bytes);

            var fs = new FileStream(filePath, FileMode.Create);
            var bw2 = new BinaryWriter(fs);

            bw2.Write((uint)0x46464952);
            bw2.Write((uint)ms.Length);
            bw2.Write(ms.ToArray());

            fs.Close();
            fs.Dispose();
        }
    }
}
