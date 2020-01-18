using System;
using System.Runtime.InteropServices;

namespace DLS {
    public enum LIST_TYPE : uint {
        LINS = 0x736E696C,
        WVPL = 0x6C707677,
        INFO = 0x4F464E49,
        INS_ = 0x20736E69,
        WAVE = 0x65766177,
        LRGN = 0x6E67726C,
        LART = 0x7472616C,
        LAR2 = 0x3272616C,
        RGN_ = 0x206E6772,
        RGN2 = 0x326E6772
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MidiLocale {
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte BankLSB;
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte BankMSB;
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        private byte Reserve1;
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte BankFlags;
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte ProgramNo;
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        private byte Reserve2;
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        private byte Reserve3;
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        private byte Reserve4;

        public byte[] Bytes {
            get {
                return new byte[] {
                    BankLSB,
                    BankMSB,
                    Reserve1,
                    BankFlags,
                    ProgramNo,
                    Reserve2,
                    Reserve3,
                    Reserve4
                };
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Range {
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Low;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort High;

        public byte[] Bytes {
            get {
                var buff = new byte[4];
                BitConverter.GetBytes(Low).CopyTo(buff, 0);
                BitConverter.GetBytes(High).CopyTo(buff, 2);
                return buff;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Connection {
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public SRC_TYPE Source;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public SRC_TYPE Control;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public DST_TYPE Destination;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Transform;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
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

        public enum SRC_TYPE : ushort {
            // MODULATOR SOURCES
            NONE = 0x0000,
            LFO = 0x0001,
            KEY_ON_VELOCITY = 0x0002,
            KEY_NUMBER = 0x0003,
            EG1 = 0x0004,
            EG2 = 0x0005,
            PITCH_WHEEL = 0x0006,
            POLY_PRESSURE = 0x0007,
            CHANNEL_PRESSURE = 0x0008,
            VIBRATO = 0x0009,

            // MIDI CONTROLLER SOURCES
            CC1 = 0x0081,
            CC7 = 0x0087,
            CC10 = 0x008A,
            CC11 = 0x008B,
            CC91 = 0x00DB,
            CC93 = 0x00DD,

            // REGISTERED PARAMETER NUMBERS
            RPN0 = 0x0100,
            RPN1 = 0x0101,
            RPN2 = 0x0102
        }

        public enum DST_TYPE : ushort {
            // GENERIC DESTINATIONS
            NONE = 0x0000,
            GAIN = 0x0001,
            RESERVED = 0x0002,
            PITCH = 0x0003,
            PAN = 0x0004,
            KEY_NUMBER = 0x0005,

            // CHANNEL OUTPUT DESTINATIONS
            LEFT = 0x0010,
            RIGHT = 0x0011,
            CENTER = 0x0012,
            LFET_CHANNEL = 0x0013,
            LEFT_REAR = 0x0014,
            RIGHT_REAR = 0x0015,
            CHORUS = 0x0080,
            REVERB = 0x0081,

            // MODULATOR LFO DESTINATIONS
            LFO_FREQUENCY = 0x0104,
            LFO_START_DELAY = 0x0105,

            // VIBRATO LFO DESTINATIONS
            VIB_FREQUENCY = 0x0114,
            VIB_START_DELAY = 0x0115,

            // EG DESTINATIONS
            EG1_ATTACK_TIME = 0x0206,
            EG1_DECAY_TIME = 0x0207,
            EG1_RESERVED = 0x0208,
            EG1_RELEASE_TIME = 0x0209,
            EG1_SUSTAIN_LEVEL = 0x020A,
            EG1_DELAY_TIME = 0x020B,
            EG1_HOLD_TIME = 0x020C,
            EG1_SHUTDOWN_TIME = 0x020D,
            EG2_ATTACK_TIME = 0x030A,
            EG2_DECAY_TIME = 0x030B,
            EG2_RESERVED = 0x030C,
            EG2_RELEASE_TIME = 0x030D,
            EG2_SUSTAIN_LEVEL = 0x030E,
            EG2_DELAY_TIME = 0x030F,
            EG2_HOLD_TIME = 0x0310,

            // FILTER DESTINATIONS
            FILTER_CUTOFF = 0x0500,
            FILTER_Q = 0x0501
        }

        public enum TRN_TYPE : ushort {
            NONE = 0x0000,
            CONCAVE = 0x0001,
            CONVEX = 0x0002,
            SWITCH = 0x0003
        }

        public enum TRN_CTRL : ushort {
            MASK = 0x0300,
            BIPOLAR = 0x0100,
            INVERT = 0x0200
        }

        public enum TRN_SRC : ushort {
            MASK = 0xC000,
            BIPOLAR = 0x4000,
            INVERT = 0x8000
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

        public byte[] Bytes {
            get {
                var buff = new byte[12];
                BitConverter.GetBytes((ushort)Source).CopyTo(buff, 0);
                BitConverter.GetBytes((ushort)Control).CopyTo(buff, 2);
                BitConverter.GetBytes((ushort)Destination).CopyTo(buff, 4);
                BitConverter.GetBytes((ushort)Transform).CopyTo(buff, 6);
                BitConverter.GetBytes(Scale).CopyTo(buff, 8);
                return buff;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WaveLoop {
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Size;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Type;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Start;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Length;

        public byte[] Bytes {
            get {
                var buff = new byte[16];
                BitConverter.GetBytes(16).CopyTo(buff, 0);
                BitConverter.GetBytes(0).CopyTo(buff, 4);
                BitConverter.GetBytes(Start).CopyTo(buff, 8);
                BitConverter.GetBytes(Length).CopyTo(buff, 12);
                return buff;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CK_VERS {
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint MSB;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint LSB;

        public byte[] Bytes {
            get {
                var buff = new byte[8];
                BitConverter.GetBytes(MSB).CopyTo(buff, 0);
                BitConverter.GetBytes(LSB).CopyTo(buff, 4);
                return buff;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CK_COLH {
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Instruments;

        public byte[] Bytes {
            get {
                var buff = new byte[4];
                BitConverter.GetBytes(Instruments).CopyTo(buff, 0);
                return buff;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CK_INSH {
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Regions;
        public MidiLocale Locale;

        public byte[] Bytes {
            get {
                var buff = new byte[12];
                BitConverter.GetBytes(Regions).CopyTo(buff, 0);
                Locale.Bytes.CopyTo(buff, 4);
                return buff;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CK_RGNH {
        public Range Key;
        public Range Velocity;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Options;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort KeyGroup;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Layer;

        public byte[] Bytes {
            get {
                var buff = new byte[0 == Layer ? 12 : 14];
                Key.Bytes.CopyTo(buff, 0);
                Velocity.Bytes.CopyTo(buff, 4);
                BitConverter.GetBytes(Options).CopyTo(buff, 8);
                BitConverter.GetBytes(KeyGroup).CopyTo(buff, 10);
                if (0 != Layer) {
                    BitConverter.GetBytes(Layer).CopyTo(buff, 12);
                }
                return buff;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CK_ART1 {
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Size;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Count;

        public byte[] Bytes {
            get {
                var buff = new byte[8];
                BitConverter.GetBytes(Size).CopyTo(buff, 0);
                BitConverter.GetBytes(Count).CopyTo(buff, 4);
                return buff;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CK_WLNK {
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Options;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort PhaseGroup;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Channel;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint TableIndex;

        public byte[] Bytes {
            get {
                var buff = new byte[12];
                BitConverter.GetBytes(Options).CopyTo(buff, 0);
                BitConverter.GetBytes(PhaseGroup).CopyTo(buff, 2);
                BitConverter.GetBytes(Channel).CopyTo(buff, 4);
                BitConverter.GetBytes(TableIndex).CopyTo(buff, 8);
                return buff;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CK_WSMP {
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Size;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort UnityNote;
        [MarshalAs(UnmanagedType.I2, SizeConst = 2)]
        public short FineTune;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int GainInt;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Options;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint LoopCount;

        public double Gain {
            get {
                return Math.Pow(10.0, GainInt / (200 * 65536.0));
            }
            set {
                GainInt = (int)(Math.Log10(value) * 200 * 65536);
            }
        }

        public byte[] Bytes {
            get {
                var buff = new byte[20];
                BitConverter.GetBytes(Size).CopyTo(buff, 0);
                BitConverter.GetBytes(UnityNote).CopyTo(buff, 4);
                BitConverter.GetBytes(FineTune).CopyTo(buff, 6);
                BitConverter.GetBytes(GainInt).CopyTo(buff, 8);
                BitConverter.GetBytes(Options).CopyTo(buff, 12);
                BitConverter.GetBytes(LoopCount).CopyTo(buff, 16);
                return buff;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CK_PTBL {
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Size;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint Count;

        public byte[] Bytes {
            get {
                var buff = new byte[8];
                BitConverter.GetBytes(Size).CopyTo(buff, 0);
                BitConverter.GetBytes(Count).CopyTo(buff, 4);
                return buff;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CK_FMT {
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Tag;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Channels;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint SampleRate;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint BytesPerSec;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort BlockAlign;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Bits;

        public byte[] Bytes {
            get {
                var buff = new byte[16];
                BitConverter.GetBytes(Tag).CopyTo(buff, 0);
                BitConverter.GetBytes(Channels).CopyTo(buff, 2);
                BitConverter.GetBytes(SampleRate).CopyTo(buff, 4);
                BitConverter.GetBytes(BytesPerSec).CopyTo(buff, 8);
                BitConverter.GetBytes(BlockAlign).CopyTo(buff, 12);
                BitConverter.GetBytes(Bits).CopyTo(buff, 14);
                return buff;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CK_INFO {
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public TYPE Type;

        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int Size;

        public enum TYPE : uint {
            IARL = 0x4C524149, // ArchivalLocation
            IART = 0x54524149, // Artists
            ICAT = 0x54414349, // Category
            ICMS = 0x534D4349, // Commissioned
            ICMT = 0x544D4349, // Comments
            ICOP = 0x504F4349, // Copyright
            ICRD = 0x44524349, // CreationDate
            IENG = 0x474E4549, // Engineer
            IGNR = 0x524E4749, // Genre
            IKEY = 0x59454B49, // Keywords
            IMED = 0x44454D49, // Medium
            INAM = 0x4D414E49, // Name
            IPRD = 0x44525049, // Product
            ISFT = 0x54465349, // Software
            ISRC = 0x43525349, // Source
            ISRF = 0x46525349, // SourceForm
            ISBJ = 0x4A425349, // Subject
            ITCH = 0x48435449  // Technician
        }
    }
}
