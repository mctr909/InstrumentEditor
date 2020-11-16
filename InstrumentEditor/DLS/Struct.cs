using System;
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
}
