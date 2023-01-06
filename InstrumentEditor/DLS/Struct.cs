using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace DLS {
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct MidiLocale {
        public byte BankLSB;
        public byte BankMSB;
        private byte Reserve1;
        public byte BankFlg;
        public byte ProgNum;
        private byte Reserve2;
        private byte Reserve3;
        private byte Reserve4;

        public void Write(BinaryWriter bw) {
            bw.Write(BankLSB);
            bw.Write(BankMSB);
            bw.Write(Reserve1);
            bw.Write(BankFlg);
            bw.Write(ProgNum);
            bw.Write(Reserve2);
            bw.Write(Reserve3);
            bw.Write(Reserve4);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Range {
        public ushort Lo;
        public ushort Hi;

        public void Write(BinaryWriter bw) {
            bw.Write(Lo);
            bw.Write(Hi);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Connection {
        [MarshalAs(UnmanagedType.U2)]
        public SRC_TYPE Source;
        [MarshalAs(UnmanagedType.U2)]
        public SRC_TYPE Control;
        [MarshalAs(UnmanagedType.U2)]
        public DST_TYPE Destination;
        public ushort Transform;
        public int Scale;

        public TRN_TYPE OutTransform {
            get { return (TRN_TYPE)(Transform & 0x0F); }
        }

        public TRN_TYPE CtrlTransform {
            get { return (TRN_TYPE)((Transform >> 4) & 0x0F); }
        }

        public TRN_TYPE SrcTransform {
            get { return (TRN_TYPE)((Transform >> 10) & 0x0F); }
        }

        public TRN_CTRL CtrlType {
            get { return (TRN_CTRL)(Transform & (ushort)TRN_CTRL.MASK); }
        }

        public TRN_SRC SrcType {
            get { return (TRN_SRC)(Transform & (ushort)TRN_SRC.MASK); }
        }

        public double Value {
            get {
                switch (Destination) {
                case DST_TYPE.GAIN:
                case DST_TYPE.FILTER_Q:
                    return Math.Pow(10.0, Scale / (200 * 65536.0));
                case DST_TYPE.PAN:
                    return (Scale / 655360.0) - 0.5;
                case DST_TYPE.LFO_START_DELAY:
                case DST_TYPE.VIB_START_DELAY:
                case DST_TYPE.EG1_ATTACK_TIME:
                case DST_TYPE.EG1_DECAY_TIME:
                case DST_TYPE.EG1_RELEASE_TIME:
                case DST_TYPE.EG1_DELAY_TIME:
                case DST_TYPE.EG1_HOLD_TIME:
                case DST_TYPE.EG1_SHUTDOWN_TIME:
                case DST_TYPE.EG2_ATTACK_TIME:
                case DST_TYPE.EG2_DECAY_TIME:
                case DST_TYPE.EG2_RELEASE_TIME:
                case DST_TYPE.EG2_DELAY_TIME:
                case DST_TYPE.EG2_HOLD_TIME:
                    return Math.Pow(2.0, Scale / (1200 * 65536.0));
                case DST_TYPE.EG1_SUSTAIN_LEVEL:
                case DST_TYPE.EG2_SUSTAIN_LEVEL:
                    return Scale / 655360.0;
                case DST_TYPE.PITCH:
                case DST_TYPE.LFO_FREQUENCY:
                case DST_TYPE.VIB_FREQUENCY:
                case DST_TYPE.FILTER_CUTOFF:
                    return Math.Pow(2.0, (Scale / 65536.0 - 6900) / 1200.0) * 440;
                default:
                    return 0.0;
                }
            }

            set {
                switch (Destination) {
                case DST_TYPE.GAIN:
                case DST_TYPE.FILTER_Q:
                    Scale = (int)(Math.Log10(value) * 200 * 65536);
                    break;
                case DST_TYPE.PAN:
                    Scale = (int)((value + 0.5) * 655360);
                    break;
                case DST_TYPE.LFO_START_DELAY:
                case DST_TYPE.VIB_START_DELAY:
                case DST_TYPE.EG1_ATTACK_TIME:
                case DST_TYPE.EG1_DECAY_TIME:
                case DST_TYPE.EG1_RELEASE_TIME:
                case DST_TYPE.EG1_DELAY_TIME:
                case DST_TYPE.EG1_HOLD_TIME:
                case DST_TYPE.EG1_SHUTDOWN_TIME:
                case DST_TYPE.EG2_ATTACK_TIME:
                case DST_TYPE.EG2_DECAY_TIME:
                case DST_TYPE.EG2_RELEASE_TIME:
                case DST_TYPE.EG2_DELAY_TIME:
                case DST_TYPE.EG2_HOLD_TIME:
                    Scale = (int)(Math.Log(value, 2.0) * 1200 * 65536);
                    break;
                case DST_TYPE.EG1_SUSTAIN_LEVEL:
                case DST_TYPE.EG2_SUSTAIN_LEVEL:
                    Scale = (int)(value * 655360);
                    break;
                case DST_TYPE.PITCH:
                case DST_TYPE.LFO_FREQUENCY:
                case DST_TYPE.VIB_FREQUENCY:
                case DST_TYPE.FILTER_CUTOFF:
                    Scale = (int)(((Math.Log(value / 440, 2.0) * 1200) + 6900) * 65536);
                    break;
                default:
                    Scale = 0;
                    break;
                }
            }
        }

        public void Write(BinaryWriter bw) {
            bw.Write((ushort)Source);
            bw.Write((ushort)Control);
            bw.Write((ushort)Destination);
            bw.Write(Transform);
            bw.Write(Scale);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct WaveLoop {
        private uint Size;
        public uint Type;
        public uint Start;
        public uint Length;

        public void Write(BinaryWriter bw) {
            bw.Write(16);
            bw.Write(0);
            bw.Write(Start);
            bw.Write(Length);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CK_VERS {
        public uint MSB;
        public uint LSB;

        public void Write(BinaryWriter bw) {
            bw.Write("vers".ToCharArray());
            bw.Write(Marshal.SizeOf<CK_VERS>());
            bw.Write(MSB);
            bw.Write(LSB);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct CK_INSH {
        public uint Regions;
        public MidiLocale Locale;

        public void Write(BinaryWriter bw) {
            bw.Write("insh".ToCharArray());
            bw.Write(Marshal.SizeOf<CK_INSH>());
            bw.Write(Regions);
            Locale.Write(bw);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct CK_RGNH {
        public Range Key;
        public Range Vel;
        public ushort Options;
        public ushort KeyGroup;
        public ushort Layer;

        public void Write(BinaryWriter bw) {
            bw.Write("rgnh".ToCharArray());
            if (0 == Layer) {
                bw.Write(Marshal.SizeOf<CK_RGNH>() - 2);
            } else {
                bw.Write(Marshal.SizeOf<CK_RGNH>());
            }
            Key.Write(bw);
            Vel.Write(bw);
            bw.Write(Options);
            bw.Write(KeyGroup);
            if (0 != Layer) {
                bw.Write(Layer);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CK_ART1 {
        public uint Size;
        public uint Count;

        public void Write(BinaryWriter bw) {
            bw.Write(Size);
            bw.Write(Count);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct CK_WLNK {
        public ushort Options;
        public ushort PhaseGroup;
        public uint Channel;
        public uint TableIndex;

        public void Write(BinaryWriter bw) {
            bw.Write("wlnk".ToCharArray());
            bw.Write(Marshal.SizeOf<CK_WLNK>());
            bw.Write(Options);
            bw.Write(PhaseGroup);
            bw.Write(Channel);
            bw.Write(TableIndex);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct CK_WSMP {
        private uint Size;
        public ushort UnityNote;
        public short FineTune;
        public int GainInt;
        public uint Options;
        public uint LoopCount;

        public double Gain {
            get {
                return Math.Pow(10.0, GainInt / (200 * 65536.0));
            }
            set {
                GainInt = (int)(Math.Log10(value) * 200 * 65536);
            }
        }

        public void Write(BinaryWriter bw) {
            bw.Write(20 + 16 * LoopCount);
            bw.Write(UnityNote);
            bw.Write(FineTune);
            bw.Write(GainInt);
            bw.Write(Options);
            bw.Write(LoopCount);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CK_PTBL {
        public uint Size;
        public uint Count;

        public void Write(BinaryWriter bw) {
            bw.Write(Size);
            bw.Write(Count);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CK_FMT {
        public ushort Tag;
        public ushort Channels;
        public uint SampleRate;
        public uint BytesPerSec;
        public ushort BlockAlign;
        public ushort Bits;

        public void Write(BinaryWriter bw) {
            bw.Write("fmt ".ToCharArray());
            bw.Write(Marshal.SizeOf<CK_FMT>());
            bw.Write(Tag);
            bw.Write(Channels);
            bw.Write(SampleRate);
            bw.Write(BytesPerSec);
            bw.Write(BlockAlign);
            bw.Write(Bits);
        }
    }

    public class LINS : Riff {
        public sealed class Sort : IComparer<MidiLocale> {
            // IComparerの実装
            public int Compare(MidiLocale x, MidiLocale y) {
                var xKey = ((x.BankFlg & 0x80) << 17) | (x.ProgNum << 16) | (x.BankMSB << 8) | x.BankLSB;
                var yKey = ((y.BankFlg & 0x80) << 17) | (y.ProgNum << 16) | (y.BankMSB << 8) | y.BankLSB;
                return xKey - yKey;
            }
        }

        public SortedDictionary<MidiLocale, INS> List = new SortedDictionary<MidiLocale, INS>(new Sort());

        public LINS() { }

        public LINS(IntPtr ptr, long size) {
            Load(ptr, size);
        }

        public void Write(BinaryWriter bw) {
            var msLins = new MemoryStream();
            var bwLins = new BinaryWriter(msLins);
            foreach (var ins in List) {
                ins.Value.Write(bwLins);
            }

            if (0 < msLins.Length) {
                bw.Write("LIST".ToCharArray());
                bw.Write((uint)(msLins.Length + 4));
                bw.Write("lins".ToCharArray());
                bw.Write(msLins.ToArray());
            }
        }

        protected override void LoadChunk(IntPtr ptr, string type, long size) {
            switch (type) {
            case "ins ":
                var inst = new INS(ptr, size);
                if (List.ContainsKey(inst.Header.Locale)) {
                    return;
                }
                List.Add(inst.Header.Locale, inst);
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", type));
            }
        }
    }

    public class INS : Riff {
        public CK_INSH Header;
        public LRGN Regions = new LRGN();
        public LART Articulations = new LART();
        public Info Info = new Info();

        public INS(byte programNo = 0, byte bankMSB = 0, byte bankLSB = 0, bool isDrum = false) {
            Header.Locale.BankFlg = (byte)(isDrum ? 0x80 : 0x00);
            Header.Locale.ProgNum = programNo;
            Header.Locale.BankMSB = bankMSB;
            Header.Locale.BankLSB = bankLSB;
        }

        public INS(IntPtr ptr, long size) {
            Load(ptr, size);
        }

        public void Write(BinaryWriter bw) {
            var msIns = new MemoryStream();
            var bwIns = new BinaryWriter(msIns);
            bwIns.Write("LIST".ToCharArray());
            bwIns.Write(0xFFFFFFFF);
            bwIns.Write("ins ".ToCharArray());

            Header.Write(bwIns);

            Regions.Write(bwIns);
            Articulations.Write(bwIns);

            Info.Write(bwIns);

            bwIns.Seek(4, SeekOrigin.Begin);
            bwIns.Write((uint)msIns.Length - 8);
            bw.Write(msIns.ToArray());
        }

        protected override void LoadChunk(IntPtr ptr, string type, long size) {
            switch (type) {
            case "insh":
                Header = Marshal.PtrToStructure<CK_INSH>(ptr);
                break;
            case "lrgn":
                Regions = new LRGN(ptr, size);
                break;
            case "lart":
            case "lar2":
                Articulations = new LART(ptr, size);
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", type));
            }
        }

        protected override void LoadInfo(IntPtr ptr, string type, string value) {
            Info[type] = value;
        }
    }

    public class LRGN : Riff {
        public sealed class Sort : IComparer<CK_RGNH> {
            // IComparerの実装
            public int Compare(CK_RGNH x, CK_RGNH y) {
                var keyH = x.Key.Hi < y.Key.Lo;
                var keyL = y.Key.Hi < x.Key.Lo;
                var velH = x.Vel.Hi < y.Vel.Lo;
                var velL = y.Vel.Hi < x.Vel.Lo;
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

        public SortedList<CK_RGNH, RGN> List = new SortedList<CK_RGNH, RGN>(new Sort());

        public LRGN() { }

        public LRGN(IntPtr ptr, long size) {
            Load(ptr, size);
        }

        public void Write(BinaryWriter bw) {
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

        public List<RGN> Find(CK_RGNH header) {
            var ret = new List<RGN>();
            foreach (var rng in List.Values) {
                if (header.Key.Lo <= rng.Header.Key.Hi && rng.Header.Key.Lo <= header.Key.Hi &&
                    header.Vel.Lo <= rng.Header.Vel.Hi && rng.Header.Vel.Lo <= header.Vel.Hi) {
                    ret.Add(rng);
                }
            }
            return ret;
        }

        public RGN Find(int noteNo, int velocity) {
            foreach (var rng in List.Values) {
                if (noteNo <= rng.Header.Key.Hi && rng.Header.Key.Lo <= noteNo &&
                    velocity <= rng.Header.Vel.Hi && rng.Header.Vel.Lo <= velocity) {
                    return rng;
                }
            }
            return null;
        }

        public RGN FindFirst(CK_RGNH header) {
            foreach (var rng in List.Values) {
                if (header.Key.Lo <= rng.Header.Key.Hi && rng.Header.Key.Lo <= header.Key.Hi &&
                    header.Vel.Lo <= rng.Header.Vel.Hi && rng.Header.Vel.Lo <= header.Vel.Hi) {
                    return rng;
                }
            }
            return null;
        }

        public bool ContainsKey(CK_RGNH header) {
            foreach (var rng in List.Values) {
                if (header.Key.Lo <= rng.Header.Key.Hi && rng.Header.Key.Lo <= header.Key.Hi &&
                    header.Vel.Lo <= rng.Header.Vel.Hi && rng.Header.Vel.Lo <= header.Vel.Hi) {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(int noteNo, int velocity) {
            foreach (var rng in List.Values) {
                if (noteNo <= rng.Header.Key.Hi && rng.Header.Key.Lo <= noteNo &&
                    velocity <= rng.Header.Vel.Hi && rng.Header.Vel.Lo <= velocity) {
                    return true;
                }
            }
            return false;
        }

        public void Remove(CK_RGNH header) {
            var tmpRegion = new List<RGN>();
            foreach (var rng in List.Values) {
                if (header.Key.Lo <= rng.Header.Key.Hi && rng.Header.Key.Lo <= header.Key.Hi &&
                    header.Vel.Lo <= rng.Header.Vel.Hi && rng.Header.Vel.Lo <= header.Vel.Hi) {
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
                List.Add(rgn.Header, rgn);
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", type));
            }
        }
    }

    public class RGN : Riff {
        public CK_RGNH Header;
        public CK_WSMP Sampler;
        public List<WaveLoop> Loops = new List<WaveLoop>();
        public CK_WLNK WaveLink;
        public LART Articulations = new LART();

        public RGN() {
            Header.Key.Lo = 0;
            Header.Key.Hi = 127;
            Header.Vel.Lo = 0;
            Header.Vel.Hi = 127;
        }

        public RGN(IntPtr ptr, long size) {
            Load(ptr, size);
        }

        public void Write(BinaryWriter bw) {
            var msRgn = new MemoryStream();
            var bwRgn = new BinaryWriter(msRgn);
            bwRgn.Write("LIST".ToCharArray());
            bwRgn.Write(0xFFFFFFFF);
            bwRgn.Write("rgn ".ToCharArray());

            Header.Write(bwRgn);

            Sampler.LoopCount = (uint)Loops.Count;
            bwRgn.Write("wsmp".ToCharArray());
            bwRgn.Write((uint)(Marshal.SizeOf<CK_WSMP>() + Sampler.LoopCount * Marshal.SizeOf<WaveLoop>()));
            Sampler.Write(bwRgn);
            foreach (var loop in Loops) {
                loop.Write(bwRgn);
            }

            WaveLink.Write(bwRgn);
            Articulations.Write(bwRgn);

            bwRgn.Seek(4, SeekOrigin.Begin);
            bwRgn.Write((uint)msRgn.Length - 8);
            bw.Write(msRgn.ToArray());
        }

        protected override void LoadChunk(IntPtr ptr, string type, long size) {
            switch (type) {
            case "rgnh":
                Header = Marshal.PtrToStructure<CK_RGNH>(ptr);
                if (size < Marshal.SizeOf<CK_RGNH>()) {
                    Header.Layer = 0;
                }
                break;
            case "wsmp":
                Sampler = Marshal.PtrToStructure<CK_WSMP>(ptr);
                var pLoop = ptr + Marshal.SizeOf<CK_WSMP>();
                for (var i = 0; i < Sampler.LoopCount; ++i) {
                    Loops.Add(Marshal.PtrToStructure<WaveLoop>(pLoop));
                    pLoop += Marshal.SizeOf<WaveLoop>();
                }
                break;
            case "wlnk":
                WaveLink = Marshal.PtrToStructure<CK_WLNK>(ptr);
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

    public class LART : Riff {
        ART mArt = new ART();

        public List<Connection> List {
            get { return mArt.List; }
        }

        public LART() { }

        public LART(IntPtr ptr, long size) {
            Load(ptr, size);
        }

        public void Write(BinaryWriter bw) {
            if (0 == mArt.List.Count) {
                return;
            }

            var msArt = new MemoryStream();
            var bwArt = new BinaryWriter(msArt);
            bwArt.Write("art1".ToCharArray());
            bwArt.Write((uint)(Marshal.SizeOf<CK_ART1>() + mArt.List.Count * Marshal.SizeOf<Connection>()));
            bwArt.Write((uint)8);
            bwArt.Write((uint)mArt.List.Count);
            foreach (var art in mArt.List) {
                art.Write(bwArt);
            }

            if (0 < msArt.Length) {
                bw.Write("LIST".ToCharArray());
                bw.Write((uint)(msArt.Length + 4));
                bw.Write("lart".ToCharArray());
                bw.Write(msArt.ToArray());
            }
        }

        public void Add(Connection conn) {
            Delete(conn.Destination);
            mArt.List.Add(conn);
        }

        public void AddRange(IEnumerable<Connection> list) {
            mArt.List.AddRange(list);
        }

        public void Clear() {
            mArt.List.Clear();
        }

        public void Delete(DST_TYPE type) {
            var tmp = new List<Connection>();
            for (int i = 0; i < mArt.List.Count; i++) {
                var art = mArt.List[i];
                if (type == art.Destination) {
                    continue;
                }
                tmp.Add(art);
            }
            mArt.List = tmp;
        }

        public void Update(DST_TYPE type, double value) {
            for (int i = 0; i < mArt.List.Count; i++) {
                var art = mArt.List[i];
                if (type == art.Destination) {
                    art.Value = value;
                    mArt.List[i] = art;
                    return;
                }
            }
            mArt.List.Add(new Connection {
                Destination = type,
                Value = value
            });
        }

        protected override void LoadChunk(IntPtr ptr, string type, long size) {
            switch (type) {
            case "art1":
            case "art2":
                mArt = new ART(ptr);
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", type));
            }
        }
    }

    public class ART {
        public List<Connection> List = new List<Connection>();

        public ART() { }

        public ART(IntPtr ptr) {
            var info = Marshal.PtrToStructure<CK_ART1>(ptr);
            ptr += Marshal.SizeOf<CK_ART1>();

            for (var i = 0; i < info.Count; ++i) {
                List.Add(Marshal.PtrToStructure<Connection>(ptr));
                ptr += Marshal.SizeOf<Connection>();
            }
        }
    }

    public class WVPL : Riff {
        public List<WAVE> List = new List<WAVE>();

        public WVPL() { }

        public WVPL(IntPtr ptr, long size) {
            Load(ptr, size);
        }

        public void Write(BinaryWriter bw) {
            var msPtbl = new MemoryStream();
            var bwPtbl = new BinaryWriter(msPtbl);
            bwPtbl.Write("ptbl".ToCharArray());
            bwPtbl.Write((uint)(Marshal.SizeOf<CK_PTBL>() + List.Count * sizeof(uint)));
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
        public CK_FMT Format;
        public CK_WSMP Sampler;
        public List<WaveLoop> Loops = new List<WaveLoop>();
        public byte[] Data;
        public Info Info = new Info();

        public WAVE() { }

        public WAVE(IntPtr ptr, long size) {
            Load(ptr, size);
        }

        public WAVE(string filePath) {
            MainLoop(filePath);
        }

        public void Write(BinaryWriter bw) {
            var msSmp = new MemoryStream();
            var bwSmp = new BinaryWriter(msSmp);
            bwSmp.Write("LIST".ToCharArray());
            bwSmp.Write(0xFFFFFFFF);
            bwSmp.Write("wave".ToCharArray());

            Sampler.LoopCount = (uint)Loops.Count;
            bwSmp.Write("wsmp".ToCharArray());
            bwSmp.Write((uint)(Marshal.SizeOf<CK_WSMP>() + Sampler.LoopCount * Marshal.SizeOf<WaveLoop>()));
            Sampler.Write(bwSmp);
            foreach (var loop in Loops) {
                loop.Write(bwSmp);
            }

            Format.Write(bwSmp);

            bwSmp.Write("data".ToCharArray());
            bwSmp.Write(Data.Length);
            bwSmp.Write(Data);

            Info.Write(bwSmp);

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

            {
                // fmt chunk
                bw.Write("fmt ".ToCharArray());
                bw.Write((uint)16);
                bw.Write((ushort)1);
                bw.Write((ushort)1);
                bw.Write(Format.SampleRate);
                bw.Write(Format.SampleRate * 2);
                bw.Write((ushort)2);
                bw.Write((ushort)16);
            }

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

            {
                Sampler.LoopCount = (uint)Loops.Count;
                bw.Write("wsmp".ToCharArray());
                bw.Write((uint)(Marshal.SizeOf<CK_WSMP>() + Sampler.LoopCount * Marshal.SizeOf<WaveLoop>()));
                Sampler.Write(bw);
                foreach (var loop in Loops) {
                    loop.Write(bw);
                }
            }

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

        protected override void LoadChunk(IntPtr ptr, string type, long size) {
            switch (type) {
            case "dlid":
            case "guid":
                break;
            case "fmt ":
                Format = Marshal.PtrToStructure<CK_FMT>(ptr);
                break;
            case "data":
                Data = new byte[size];
                Marshal.Copy(ptr, Data, 0, Data.Length);
                break;
            case "wsmp":
                Sampler = Marshal.PtrToStructure<CK_WSMP>(ptr);
                var pLoop = ptr + Marshal.SizeOf<CK_WSMP>();
                for (var i = 0; i < Sampler.LoopCount; ++i) {
                    Loops.Add(Marshal.PtrToStructure<WaveLoop>(pLoop));
                    pLoop += Marshal.SizeOf<WaveLoop>();
                }
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
