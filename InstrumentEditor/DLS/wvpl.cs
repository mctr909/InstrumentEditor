using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLS {
    unsafe public class WVPL : RIFF {
        public Dictionary<int, WAVE> List = new Dictionary<int, WAVE>();

        public WVPL() { }

        public WVPL(byte* ptr, byte* endPtr) : base(ptr, endPtr) { }

        protected override void ReadList(byte* ptr, byte* endPtr) {
            switch ((LIST_TYPE)mListType) {
            case LIST_TYPE.WAVE:
                List.Add(List.Count, new WAVE(ptr, endPtr));
                break;
            default:
                throw new Exception();
            }
        }

        public new byte[] Bytes {
            get {
                var msPtbl = new MemoryStream();
                var bwPtbl = new BinaryWriter(msPtbl);
                bwPtbl.Write((uint)CHUNK_TYPE.PTBL);
                bwPtbl.Write((uint)(sizeof(CK_PTBL) + List.Count * sizeof(uint)));
                bwPtbl.Write((uint)8);
                bwPtbl.Write((uint)List.Count);

                var msWave = new MemoryStream();
                var bwWave = new BinaryWriter(msWave);
                foreach (var wav in List) {
                    bwPtbl.Write((uint)msWave.Position);
                    bwWave.Write(wav.Value.Bytes);
                }

                var ms = new MemoryStream();
                var bw = new BinaryWriter(ms);
                if (0 < msWave.Length) {
                    bw.Write(msPtbl.ToArray());
                    bw.Write((uint)CHUNK_TYPE.LIST);
                    bw.Write((uint)(msWave.Length + 4));
                    bw.Write((uint)LIST_TYPE.WVPL);
                    bw.Write(msWave.ToArray());
                }

                return ms.ToArray();
            }
        }
    }

    unsafe public class WAVE : RIFF {
        public CK_FMT Format;
        public CK_WSMP Sampler;
        public Dictionary<int, WaveLoop> Loops = new Dictionary<int, WaveLoop>();
        public byte[] Data;
        public INFO Info = new INFO();

        public WAVE(string filePath) {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            var riff = br.ReadUInt32();
            var riffSize = br.ReadUInt32();
            var riffType = br.ReadUInt32();

            while (fs.Position < fs.Length) {
                var chunkType = (CHUNK_TYPE)br.ReadUInt32();
                var chunkSize = br.ReadUInt32();
                var chunkData = br.ReadBytes((int)chunkSize);

                switch (chunkType) {
                case CHUNK_TYPE.FMT_:
                    fixed (byte* ptr = &chunkData[0]) {
                        Format = (CK_FMT)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_FMT));
                    }
                    break;
                case CHUNK_TYPE.DATA:
                    Data = chunkData;
                    break;
                case CHUNK_TYPE.WSMP:
                    fixed (byte* ptr = &chunkData[0]) {
                        Sampler = (CK_WSMP)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_WSMP));
                        var pLoop = ptr + sizeof(CK_WSMP);
                        for (var i = 0; i < Sampler.LoopCount; ++i) {
                            Loops.Add(Loops.Count, (WaveLoop)Marshal.PtrToStructure((IntPtr)pLoop, typeof(WaveLoop)));
                            pLoop += sizeof(WaveLoop);
                        }
                    }
                    break;
                case CHUNK_TYPE.LIST:
                    fixed (byte* ptr = &chunkData[0]) {
                        var listType = (LIST_TYPE)Marshal.PtrToStructure((IntPtr)ptr, typeof(LIST_TYPE));
                        switch (listType) {
                        case LIST_TYPE.INFO:
                            //Info = new INFO(ptr + sizeof(CK_LIST), ptr + chunkSize);
                            break;
                        }
                    }
                    break;
                }
            }

            if (null == Info) {
                Info = new INFO();
                Info.Name = Path.GetFileNameWithoutExtension(filePath);
            } else if (string.IsNullOrWhiteSpace(Info.Name)) {
                Info.Name = Path.GetFileNameWithoutExtension(filePath);
            }

            br.Dispose();
            fs.Close();
            fs.Dispose();
        }

        public WAVE(byte* ptr, byte* endPtr) : base(ptr, endPtr) { }

        protected override void ReadChunk(byte* ptr) {
            switch ((CHUNK_TYPE)mChunkType) {
            case CHUNK_TYPE.DLID:
            case CHUNK_TYPE.GUID:
                break;
            case CHUNK_TYPE.FMT_:
                Format = (CK_FMT)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_FMT));
                break;
            case CHUNK_TYPE.DATA:
                Data = new byte[mChunkSize];
                Marshal.Copy((IntPtr)ptr, Data, 0, Data.Length);
                break;
            case CHUNK_TYPE.WSMP:
                Sampler = (CK_WSMP)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_WSMP));
                var pLoop = ptr + sizeof(CK_WSMP);
                for (var i = 0; i < Sampler.LoopCount; ++i) {
                    Loops.Add(Loops.Count, (WaveLoop)Marshal.PtrToStructure((IntPtr)pLoop, typeof(WaveLoop)));
                    pLoop += sizeof(WaveLoop);
                }
                break;
            }
        }

        protected override void ReadList(byte* ptr, byte* endPtr) {
            switch ((LIST_TYPE)mListType) {
            case LIST_TYPE.INFO:
                Info = new INFO(ptr, endPtr);
                break;
            }
        }

        protected override void WriteChunk(BinaryWriter bw) {
            mListType = (uint)LIST_TYPE.WAVE;

            var data = Sampler.Bytes;
            bw.Write((uint)CHUNK_TYPE.WSMP);
            bw.Write((uint)(data.Length + Sampler.LoopCount * sizeof(WaveLoop)));
            bw.Write(data);
            foreach (var loop in Loops.Values) {
                bw.Write(loop.Bytes);
            }

            data = Format.Bytes;
            bw.Write((uint)CHUNK_TYPE.FMT_);
            bw.Write(data.Length);
            bw.Write(data);

            bw.Write((uint)CHUNK_TYPE.DATA);
            bw.Write(Data.Length);
            bw.Write(Data);
        }

        protected override void WriteList(BinaryWriter bw) {
            bw.Write(Info.Bytes);
        }

        public void ToFile(string filePath) {
            if (16 != Format.Bits) {
                return;
            }

            FileStream fs = new FileStream(filePath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            var msr = new MemoryStream(Data);
            var bmr = new BinaryReader(msr);
            var msw = new MemoryStream();
            var bmw = new BinaryWriter(msw);

            while (msr.Position < msr.Length) {
                bmw.Write(bmr.ReadInt16());
            }

            // RIFF
            bw.Write((uint)0x46464952);
            bw.Write((uint)0);
            bw.Write((uint)0x45564157);

            // fmt
            bw.Write((uint)CHUNK_TYPE.FMT_);
            bw.Write((uint)16);
            bw.Write(Format.Tag);
            bw.Write(Format.Channels);
            bw.Write(Format.SampleRate);
            bw.Write(Format.BytesPerSec);
            bw.Write(Format.BlockAlign);
            bw.Write(Format.Bits);

            // data
            bw.Write((uint)CHUNK_TYPE.DATA);
            bw.Write((uint)msw.Length);
            bw.Write(msw.ToArray());

            // sampler
            var data = Sampler.Bytes;
            bw.Write((uint)CHUNK_TYPE.WSMP);
            bw.Write((uint)(data.Length + Sampler.LoopCount * sizeof(WaveLoop)));
            bw.Write(data);
            foreach (var loop in Loops.Values) {
                bw.Write(loop.Bytes);
            }

            // info
            bw.Write(Info.Bytes);

            fs.Seek(4, SeekOrigin.Begin);
            bw.Write((uint)(fs.Length - 8));

            bw.Dispose();
            fs.Close();
            fs.Dispose();
        }
    }
}
