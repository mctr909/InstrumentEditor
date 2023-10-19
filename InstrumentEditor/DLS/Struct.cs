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

        public string Unit {
            get {
                switch (Destination) {
                case DST_TYPE.GAIN:
                case DST_TYPE.FILTER_Q:
                    return "db";
                case DST_TYPE.PAN:
                    return "-0.5 to +0.5";
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
                    return "sec";
                case DST_TYPE.EG1_SUSTAIN_LEVEL:
                case DST_TYPE.EG2_SUSTAIN_LEVEL:
                    return "%";
                case DST_TYPE.PITCH:
                case DST_TYPE.LFO_FREQUENCY:
                case DST_TYPE.VIB_FREQUENCY:
                case DST_TYPE.FILTER_CUTOFF:
                    return "Hz";
                default:
                    return "";
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
}