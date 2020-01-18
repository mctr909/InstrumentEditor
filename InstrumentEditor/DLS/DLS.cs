using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace DLS {
    public class DLS : RIFF {
        private CK_VERS mVersion;
        private uint mMSYN = 1;

        public LINS Instruments = new LINS();
        public WVPL WavePool = new WVPL();
        public INFO Text = new INFO();

        public DLS() { }

        public DLS(IntPtr ptr, IntPtr endPtr) : base(ptr, endPtr) { }

        protected override void ReadChunk(IntPtr ptr) {
            switch (mChunkType) {
            case "colh":
                break;
            case "vers":
                mVersion = (CK_VERS)Marshal.PtrToStructure(ptr, typeof(CK_VERS));
                break;
            case "msyn":
                break;
            case "ptbl":
                break;
            case "dlid":
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", mChunkType));
            }
        }

        protected override void ReadList(IntPtr ptr, IntPtr endPtr) {
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
            var instFile = new INST.File();
            foreach(var wave in WavePool.List.Values) {
                var waveInfo = new INST.WaveInfo();
                waveInfo.header.sampleRate = wave.Format.SampleRate;
                if (0 < wave.Sampler.LoopCount) {
                    waveInfo.header.loopEnable = 1;
                    waveInfo.header.loopBegin = wave.Loops[0].Start;
                    waveInfo.header.loopLength = wave.Loops[0].Length;
                } else {
                    waveInfo.header.loopEnable = 0;
                    waveInfo.header.loopBegin = 0;
                    waveInfo.header.loopLength = (uint)(wave.Data.Length / wave.Format.BlockAlign);
                }
                waveInfo.header.unityNote = (byte)wave.Sampler.UnityNote;
                waveInfo.header.gain = wave.Sampler.Gain;
                waveInfo.header.pitch = Math.Pow(2.0, wave.Sampler.FineTune / 1200.0);

                var ptr = Marshal.AllocHGlobal(wave.Data.Length);
                Marshal.Copy(wave.Data, 0, ptr, wave.Data.Length);
                waveInfo.data = new short[wave.Data.Length / 2];
                Marshal.Copy(ptr, waveInfo.data, 0, waveInfo.data.Length);
                Marshal.FreeHGlobal(ptr);

                waveInfo.infoList.Add("INAM", wave.Info.Name);
                waveInfo.infoList.Add("ICAT", wave.Info.Keywords);
                instFile.waves.Add(waveInfo);
            }

            foreach (var inst in Instruments.List) {
                var ins = new INST.InstInfo();
                ins.header.flag = inst.Key.BankFlags;
                ins.header.bankMSB = inst.Key.BankMSB;
                ins.header.bankLSB = inst.Key.BankLSB;
                ins.header.progNum = inst.Key.ProgramNo;

                if (null != inst.Value.Articulations && null != inst.Value.Articulations.ART) {
                    foreach (var a in inst.Value.Articulations.ART.List.Values) {
                        if (a.Source != Connection.SRC_TYPE.NONE || a.Control != Connection.SRC_TYPE.NONE) {
                            continue;
                        }
                        var art = new INST.ART();
                        art.type = (uint)a.Destination;
                        art.value = (float)a.Value;
                        switch (a.Destination) {
                        case Connection.DST_TYPE.EG1_ATTACK_TIME:
                        case Connection.DST_TYPE.EG1_HOLD_TIME:
                        case Connection.DST_TYPE.EG1_DECAY_TIME:
                        case Connection.DST_TYPE.EG1_SUSTAIN_LEVEL:
                        case Connection.DST_TYPE.EG1_RELEASE_TIME:
                        case Connection.DST_TYPE.EG2_ATTACK_TIME:
                        case Connection.DST_TYPE.EG2_HOLD_TIME:
                        case Connection.DST_TYPE.EG2_DECAY_TIME:
                        case Connection.DST_TYPE.EG2_SUSTAIN_LEVEL:
                        case Connection.DST_TYPE.EG2_RELEASE_TIME:
                            ins.arts.Add(art);
                            break;
                        case Connection.DST_TYPE.PAN:
                        case Connection.DST_TYPE.GAIN:
                        case Connection.DST_TYPE.PITCH:
                        case Connection.DST_TYPE.FILTER_Q:
                        case Connection.DST_TYPE.FILTER_CUTOFF:
                            ins.arts.Add(art);
                            break;
                        }
                    }
                }

                var lyr = new INST.Layer();
                lyr.header.keyLo = 0;
                lyr.header.keyHi = 127;
                lyr.header.velLo = 0;
                lyr.header.velHi = 127;
                lyr.header.instIdx = 0xFFFFFFFF;

                foreach(var r in inst.Value.Regions.List) {
                    var rgn = new INST.Region();
                    rgn.header.keyLo = (byte)r.Key.Key.Low;
                    rgn.header.keyHi = (byte)r.Key.Key.High;
                    rgn.header.velLo = (byte)r.Key.Velocity.Low;
                    rgn.header.velHi = (byte)r.Key.Velocity.High;
                    rgn.header.waveIdx = r.Value.WaveLink.TableIndex;

                    if (null != r.Value.Articulations && null != r.Value.Articulations.ART) {
                        foreach (var a in r.Value.Articulations.ART.List.Values) {
                            if (a.Source != Connection.SRC_TYPE.NONE || a.Control != Connection.SRC_TYPE.NONE) {
                                continue;
                            }
                            var art = new INST.ART();
                            art.type = (uint)a.Destination;
                            art.value = (float)a.Value;
                            switch (a.Destination) {
                            case Connection.DST_TYPE.EG1_ATTACK_TIME:
                            case Connection.DST_TYPE.EG1_HOLD_TIME:
                            case Connection.DST_TYPE.EG1_DECAY_TIME:
                            case Connection.DST_TYPE.EG1_SUSTAIN_LEVEL:
                            case Connection.DST_TYPE.EG1_RELEASE_TIME:
                            case Connection.DST_TYPE.EG2_ATTACK_TIME:
                            case Connection.DST_TYPE.EG2_HOLD_TIME:
                            case Connection.DST_TYPE.EG2_DECAY_TIME:
                            case Connection.DST_TYPE.EG2_SUSTAIN_LEVEL:
                            case Connection.DST_TYPE.EG2_RELEASE_TIME:
                                rgn.arts.Add(art);
                                break;
                            case Connection.DST_TYPE.PAN:
                            case Connection.DST_TYPE.GAIN:
                            case Connection.DST_TYPE.PITCH:
                            case Connection.DST_TYPE.FILTER_Q:
                            case Connection.DST_TYPE.FILTER_CUTOFF:
                                ins.arts.Add(art);
                                break;
                            }
                        }
                    }

                    lyr.regions.Add(rgn);
                }

                ins.layers.Add(lyr);
                ins.infoList.Add("INAM", inst.Value.Info.Name);
                ins.infoList.Add("ICAT", inst.Value.Info.Keywords);
                instFile.instList.Add(ins);
            }

            instFile.Write(filePath + ".bin");

            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(new char[] { 'D', 'L', 'S', ' ' });

            bw.Write(new char[] { 'c', 'o', 'l', 'h' });
            bw.Write((uint)4);
            bw.Write((uint)Instruments.List.Count);

            bw.Write(new char[] { 'v', 'e', 'r', 's' });
            bw.Write((uint)Marshal.SizeOf<CK_VERS>());
            bw.Write(mVersion.Bytes);

            bw.Write(new char[] { 'm', 's', 'y', 'n' });
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

    public class LINS : RIFF {
        public sealed class Sort : IComparer<MidiLocale> {
            // IComparerの実装
            public int Compare(MidiLocale x, MidiLocale y) {
                var xKey = ((x.BankFlags & 0x80) << 17) | (x.ProgramNo << 16) | (x.BankMSB << 8) | x.BankLSB;
                var yKey = ((y.BankFlags & 0x80) << 17) | (y.ProgramNo << 16) | (y.BankMSB << 8) | y.BankLSB;
                return xKey - yKey;
            }
        }

        public SortedDictionary<MidiLocale, INS> List = new SortedDictionary<MidiLocale, INS>(new Sort());

        public LINS() { }

        public LINS(IntPtr ptr, IntPtr endPtr) : base(ptr, endPtr) { }

        protected override void ReadList(IntPtr ptr, IntPtr endPtr) {
            switch ((LIST_TYPE)mListType) {
            case LIST_TYPE.INS_:
                var inst = new INS(ptr, endPtr);
                if (List.ContainsKey(inst.Header.Locale)) {
                    return;
                }
                List.Add(inst.Header.Locale, inst);
                break;
            default:
                throw new Exception(string.Format("Unknown ListId [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes(mListType))));
            }
        }

        public new byte[] Bytes {
            get {
                var ms = new MemoryStream();
                var bw = new BinaryWriter(ms);
                foreach (var ins in List) {
                    bw.Write(ins.Value.Bytes);
                }

                var ms2 = new MemoryStream();
                var bw2 = new BinaryWriter(ms2);
                if (0 < ms.Length) {
                    bw2.Write(new char[] { 'L', 'I', 'S', 'T' });
                    bw2.Write((uint)(ms.Length + 4));
                    bw2.Write((uint)LIST_TYPE.LINS);
                    bw2.Write(ms.ToArray());
                }

                return ms2.ToArray();
            }
        }
    }

    public class INS : RIFF {
        public CK_INSH Header;
        public LRGN Regions = new LRGN();
        public LART Articulations = new LART();
        public INFO Info = new INFO();

        public INS() { }

        public INS(byte programNo, byte bankMSB = 0, byte bankLSB = 0, bool isDrum = false) {
            Header.Locale.BankFlags = (byte)(isDrum ? 0x80 : 0x00);
            Header.Locale.ProgramNo = programNo;
            Header.Locale.BankMSB = bankMSB;
            Header.Locale.BankLSB = bankLSB;
        }

        public INS(IntPtr ptr, IntPtr endPtr) : base(ptr, endPtr) { }

        protected override void ReadChunk(IntPtr ptr) {
            switch (mChunkType) {
            case "insh":
                Header = (CK_INSH)Marshal.PtrToStructure(ptr, typeof(CK_INSH));
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", mChunkType));
            }
        }

        protected override void ReadList(IntPtr ptr, IntPtr endPtr) {
            switch ((LIST_TYPE)mListType) {
            case LIST_TYPE.LRGN:
                Regions = new LRGN(ptr, endPtr);
                break;
            case LIST_TYPE.LART:
            case LIST_TYPE.LAR2:
                Articulations = new LART(ptr, endPtr);
                break;
            case LIST_TYPE.INFO:
                Info = new INFO(ptr, endPtr);
                break;
            default:
                throw new Exception(string.Format("Unknown ListType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes(mListType))));
            }
        }

        protected override void WriteChunk(BinaryWriter bw) {
            mListType = (uint)LIST_TYPE.INS_;
            var data = Header.Bytes;
            bw.Write(new char[] { 'i', 'n', 's', 'h' });
            bw.Write(data.Length);
            bw.Write(data);
        }

        protected override void WriteList(BinaryWriter bw) {
            bw.Write(Regions.Bytes);
            bw.Write(Articulations.Bytes);
            bw.Write(Info.Bytes);
        }
    }

    public class LRGN : RIFF {
        public sealed class Sort : IComparer<CK_RGNH> {
            // IComparerの実装
            public int Compare(CK_RGNH x, CK_RGNH y) {
                var xKey = (x.Key.Low << 24) | (x.Key.High << 16) | (x.Velocity.Low << 8) | x.Velocity.High;
                var yKey = (y.Key.Low << 24) | (y.Key.High << 16) | (y.Velocity.Low << 8) | y.Velocity.High;
                return xKey - yKey;
            }
        }

        public SortedDictionary<CK_RGNH, RGN> List = new SortedDictionary<CK_RGNH, RGN>(new Sort());

        public LRGN() { }

        public LRGN(IntPtr ptr, IntPtr endPtr) : base(ptr, endPtr) { }

        protected override void ReadList(IntPtr ptr, IntPtr endPtr) {
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
                    bw2.Write(new char[] { 'i', 'n', 's', 'h' });
                    bw2.Write((uint)(ms.Length + 4));
                    bw2.Write((uint)LIST_TYPE.LRGN);
                    bw2.Write(ms.ToArray());
                }

                return ms2.ToArray();
            }
        }
    }

    public class RGN : RIFF {
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

        public RGN(IntPtr ptr, IntPtr endPtr) : base(ptr, endPtr) { }

        protected override void ReadChunk(IntPtr ptr) {
            switch (mChunkType) {
            case "rgnh":
                Header = (CK_RGNH)Marshal.PtrToStructure(ptr, typeof(CK_RGNH));
                if (mChunkSize < Marshal.SizeOf<CK_RGNH>()) {
                    Header.Layer = 0;
                }
                break;
            case "wsmp":
                Sampler = (CK_WSMP)Marshal.PtrToStructure(ptr, typeof(CK_WSMP));
                var pLoop = ptr + Marshal.SizeOf<CK_WSMP>();
                for (var i = 0; i < Sampler.LoopCount; ++i) {
                    Loops.Add(Loops.Count, (WaveLoop)Marshal.PtrToStructure(pLoop, typeof(WaveLoop)));
                    pLoop += Marshal.SizeOf<WaveLoop>();
                }
                break;
            case "wlnk":
                WaveLink = (CK_WLNK)Marshal.PtrToStructure(ptr, typeof(CK_WLNK));
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", mChunkType));
            }
        }

        protected override void ReadList(IntPtr ptr, IntPtr endPtr) {
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
            bw.Write(new char[] { 'r', 'g', 'n', 'h' });
            bw.Write(data.Length);
            bw.Write(data);

            data = Sampler.Bytes;
            bw.Write(new char[] { 'w', 's', 'm', 'p' });
            bw.Write((uint)(data.Length + Sampler.LoopCount * Marshal.SizeOf<WaveLoop>()));
            bw.Write(data);
            for (var i = 0; i < Sampler.LoopCount && i < Loops.Count; ++i) {
                bw.Write(Loops[i].Bytes);
            }

            data = WaveLink.Bytes;
            bw.Write(new char[] { 'w', 'l', 'n', 'k' });
            bw.Write(data.Length);
            bw.Write(data);
        }

        protected override void WriteList(BinaryWriter bw) {
            bw.Write(Articulations.Bytes);
        }
    }

    public class LART : RIFF {
        public ART ART;

        public LART() { }

        public LART(IntPtr ptr, IntPtr endPtr) : base(ptr, endPtr) { }

        protected override void ReadChunk(IntPtr ptr) {
            switch (mChunkType) {
            case "art1":
            case "art2":
                ART = new ART(ptr);
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", mChunkType));
            }
        }

        public new byte[] Bytes {
            get {
                if (null == ART) {
                    return new byte[0];
                }

                var ms = new MemoryStream();
                var bw = new BinaryWriter(ms);

                bw.Write(new char[] { 'a', 'r', 't', '1' });
                bw.Write((uint)(Marshal.SizeOf<CK_ART1>() + ART.List.Count * Marshal.SizeOf<Connection>()));
                bw.Write((uint)8);
                bw.Write((uint)ART.List.Count);
                foreach (var art in ART.List) {
                    bw.Write(art.Value.Bytes);
                }

                var ms2 = new MemoryStream();
                var bw2 = new BinaryWriter(ms2);
                if (0 < ms.Length) {
                    bw2.Write(new char[] { 'L', 'I', 'S', 'T' });
                    bw2.Write((uint)(ms.Length + 4));
                    bw2.Write((uint)LIST_TYPE.LART);
                    bw2.Write(ms.ToArray());
                }

                return ms2.ToArray();
            }
        }
    }

    public class ART {
        public Dictionary<int, Connection> List = new Dictionary<int, Connection>();

        public ART() { }

        public ART(IntPtr ptr) {
            var info = (CK_ART1)Marshal.PtrToStructure(ptr, typeof(CK_ART1));
            ptr += Marshal.SizeOf<CK_ART1>();

            for (var i = 0; i < info.Count; ++i) {
                List.Add(i, (Connection)Marshal.PtrToStructure(ptr, typeof(Connection)));
                ptr += Marshal.SizeOf<Connection>();
            }
        }
    }

    public class WVPL : RIFF {
        public Dictionary<int, WAVE> List = new Dictionary<int, WAVE>();

        public WVPL() { }

        public WVPL(IntPtr ptr, IntPtr endPtr) : base(ptr, endPtr) { }

        protected override void ReadList(IntPtr ptr, IntPtr endPtr) {
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
                bwPtbl.Write(new char[] { 'p', 't', 'b', 'l' });
                bwPtbl.Write((uint)(Marshal.SizeOf<CK_PTBL>() + List.Count * sizeof(uint)));
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
                    bw.Write(new char[] { 'L', 'I', 'S', 'T' });
                    bw.Write((uint)(msWave.Length + 4));
                    bw.Write((uint)LIST_TYPE.WVPL);
                    bw.Write(msWave.ToArray());
                }

                return ms.ToArray();
            }
        }
    }

    public class WAVE : RIFF {
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
                var chunkType = Encoding.ASCII.GetString(br.ReadBytes(4));
                var chunkSize = br.ReadUInt32();
                var pChunkData = Marshal.AllocHGlobal((int)chunkSize);
                Marshal.StructureToPtr(br.ReadBytes((int)chunkSize), pChunkData, true);

                switch (chunkType) {
                case "fmt ":
                    Format = (CK_FMT)Marshal.PtrToStructure(pChunkData, typeof(CK_FMT));
                    break;
                case "data":
                    Marshal.Copy(pChunkData, Data, 0, Data.Length);
                    break;
                case "wsmp":
                    Sampler = (CK_WSMP)Marshal.PtrToStructure(pChunkData, typeof(CK_WSMP));
                    var pLoop = pChunkData + Marshal.SizeOf<CK_WSMP>();
                    for (var i = 0; i < Sampler.LoopCount; ++i) {
                        Loops.Add(Loops.Count, (WaveLoop)Marshal.PtrToStructure(pLoop, typeof(WaveLoop)));
                        pLoop += Marshal.SizeOf<WaveLoop>();
                    }
                    break;
                case "LIST":
                    var listType = (LIST_TYPE)Marshal.PtrToStructure(pChunkData, typeof(LIST_TYPE));
                    switch (listType) {
                    case LIST_TYPE.INFO:
                        //Info = new INFO(pChunkData + 4, pChunkData + chunkSize);
                        break;
                    }
                    break;
                }
                Marshal.FreeHGlobal(pChunkData);
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

        public WAVE(IntPtr ptr, IntPtr endPtr) : base(ptr, endPtr) { }

        protected override void ReadChunk(IntPtr ptr) {
            switch (mChunkType) {
            case "dlid":
            case "guid":
                break;
            case "fmt ":
                Format = (CK_FMT)Marshal.PtrToStructure(ptr, typeof(CK_FMT));
                break;
            case "data":
                Data = new byte[mChunkSize];
                Marshal.Copy(ptr, Data, 0, Data.Length);
                break;
            case "wsmp":
                Sampler = (CK_WSMP)Marshal.PtrToStructure(ptr, typeof(CK_WSMP));
                var pLoop = ptr + Marshal.SizeOf<CK_WSMP>();
                for (var i = 0; i < Sampler.LoopCount; ++i) {
                    Loops.Add(Loops.Count, (WaveLoop)Marshal.PtrToStructure(pLoop, typeof(WaveLoop)));
                    pLoop += Marshal.SizeOf<WaveLoop>();
                }
                break;
            }
        }

        protected override void ReadList(IntPtr ptr, IntPtr endPtr) {
            switch ((LIST_TYPE)mListType) {
            case LIST_TYPE.INFO:
                Info = new INFO(ptr, endPtr);
                break;
            }
        }

        protected override void WriteChunk(BinaryWriter bw) {
            mListType = (uint)LIST_TYPE.WAVE;

            var data = Sampler.Bytes;
            bw.Write(new char[] { 'w', 's', 'm', 'p' });
            bw.Write((uint)(data.Length + Sampler.LoopCount * Marshal.SizeOf<WaveLoop>()));
            bw.Write(data);
            foreach (var loop in Loops.Values) {
                bw.Write(loop.Bytes);
            }

            data = Format.Bytes;
            bw.Write(new char[] { 'f', 'm', 't', ' ' });
            bw.Write(data.Length);
            bw.Write(data);

            bw.Write(new char[] { 'd', 'a', 't', 'a' });
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

            bw.Write(new char[] { 'R', 'I', 'F', 'F' });
            bw.Write((uint)0);
            bw.Write(new char[] { 'W', 'A', 'V', 'E' });

            bw.Write(new char[] { 'f', 'm', 't', ' ' });
            bw.Write((uint)16);
            bw.Write(Format.Tag);
            bw.Write(Format.Channels);
            bw.Write(Format.SampleRate);
            bw.Write(Format.BytesPerSec);
            bw.Write(Format.BlockAlign);
            bw.Write(Format.Bits);

            bw.Write(new char[] { 'd', 'a', 't', 'a' });
            bw.Write((uint)msw.Length);
            bw.Write(msw.ToArray());

            var data = Sampler.Bytes;
            bw.Write(new char[] { 'w', 's', 'm', 'p' });
            bw.Write((uint)(data.Length + Sampler.LoopCount * Marshal.SizeOf<WaveLoop>()));
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
