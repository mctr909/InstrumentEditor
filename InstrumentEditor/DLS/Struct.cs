using System;
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
                    return "-50% to +50%";
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
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct WaveLoop {
        private uint Size;
        public uint Type;
        public uint Start;
        public uint Length;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct WSMP {
        private uint Size;
        public ushort UnityNote;
        public short FineTune;
        public int GainInt;
        public uint Options;

        public double Gain {
            get {
                return Math.Pow(10.0, GainInt / (200 * 65536.0));
            }
            set {
                GainInt = (int)(Math.Log10(value) * 200 * 65536);
            }
        }
    }
}