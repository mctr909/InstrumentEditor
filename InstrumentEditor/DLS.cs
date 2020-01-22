using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

using Riff;

namespace DLS {
    #region struct
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
    #endregion

    public class DLS : Chunk {
        private CK_VERS mVersion;
        private uint mMSYN = 1;

        public LINS Instruments = new LINS();
        public WVPL WavePool = new WVPL();
        public Info Info = new Info();

        public DLS() { }

        public DLS(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
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
                throw new Exception(string.Format("Unknown ChunkType [{0}]", chunkType));
            }
        }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "lins":
                Instruments = new LINS(ptr, ptrTerm);
                break;
            case "wvpl":
                WavePool = new WVPL(ptr, ptrTerm);
                break;
            case "INFO":
                Info = new Info(ptr, ptrTerm);
                break;
            default:
                throw new Exception(string.Format("Unknown ListType [{0}]", listType));
            }
        }

        public void ToInst(string filePath) {
            var instFile = new INST.File();

            foreach (var wave in WavePool.List.Values) {
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

                foreach (var r in inst.Value.Regions.List) {
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

            instFile.Write(Path.GetDirectoryName(filePath)
                + "\\" + Path.GetFileNameWithoutExtension(filePath) + ".ins");
        }

        public void Save(string filePath) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write("DLS ".ToCharArray());

            bw.Write("colh".ToCharArray());
            bw.Write((uint)4);
            bw.Write((uint)Instruments.List.Count);

            bw.Write("vers".ToCharArray());
            bw.Write((uint)Marshal.SizeOf<CK_VERS>());
            bw.Write(mVersion.Bytes);

            bw.Write("msyn".ToCharArray());
            bw.Write((uint)4);
            bw.Write(mMSYN);

            Instruments.Write(bw);
            WavePool.Write(bw);
            Info.Write(bw);

            var fs = new FileStream(filePath, FileMode.Create);
            var bw2 = new BinaryWriter(fs);
            bw2.Write("RIFF".ToCharArray());
            bw2.Write((uint)ms.Length);
            bw2.Write(ms.ToArray());

            fs.Close();
            fs.Dispose();
        }
    }

    public class LINS : Chunk {
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

        public LINS(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "ins ":
                var inst = new INS(ptr, ptrTerm);
                if (List.ContainsKey(inst.Header.Locale)) {
                    return;
                }
                List.Add(inst.Header.Locale, inst);
                break;
            default:
                throw new Exception(string.Format("Unknown ListId [{0}]", listType));
            }
        }

        public new void Write(BinaryWriter bw) {
            var msLins = new MemoryStream();
            var bwLins = new BinaryWriter(msLins);
            foreach (var ins in List) {
                bwLins.Write(ins.Value.Bytes);
            }

            if (0 < msLins.Length) {
                bw.Write("LIST".ToCharArray());
                bw.Write((uint)(msLins.Length + 4));
                bw.Write("lins".ToCharArray());
                bw.Write(msLins.ToArray());
            }
        }
    }

    public class INS : Chunk {
        public CK_INSH Header;
        public LRGN Regions = new LRGN();
        public LART Articulations = new LART();
        public Info Info = new Info();

        public INS() { }

        public INS(byte programNo, byte bankMSB = 0, byte bankLSB = 0, bool isDrum = false) {
            Header.Locale.BankFlags = (byte)(isDrum ? 0x80 : 0x00);
            Header.Locale.ProgramNo = programNo;
            Header.Locale.BankMSB = bankMSB;
            Header.Locale.BankLSB = bankLSB;
        }

        public INS(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "insh":
                Header = (CK_INSH)Marshal.PtrToStructure(ptr, typeof(CK_INSH));
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", chunkType));
            }
        }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "lrgn":
                Regions = new LRGN(ptr, ptrTerm);
                break;
            case "lart":
            case "lar2":
                Articulations = new LART(ptr, ptrTerm);
                break;
            case "INFO":
                Info = new Info(ptr, ptrTerm);
                break;
            default:
                throw new Exception(string.Format("Unknown ListType [{0}]", listType));
            }
        }

        protected override string Write(BinaryWriter bw) {
            var data = Header.Bytes;
            bw.Write("insh".ToCharArray());
            bw.Write(data.Length);
            bw.Write(data);

            Regions.Write(bw);
            Articulations.Write(bw);
            Info.Write(bw);

            return "ins ";
        }
    }

    public class LRGN : Chunk {
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

        public LRGN(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "rgn ":
                var rgn = new RGN(ptr, ptrTerm);
                List.Add(rgn.Header, rgn);
                break;
            default:
                throw new Exception(string.Format("Unknown ListType [{0}]", listType));
            }
        }

        public new void Write(BinaryWriter bw) {
            var msList = new MemoryStream();
            var bwList = new BinaryWriter(msList);
            foreach (var rgn in List) {
                bwList.Write(rgn.Value.Bytes);
            }

            if (0 < msList.Length) {
                bw.Write("LIST".ToCharArray());
                bw.Write((uint)(msList.Length + 4));
                bw.Write("lrgn".ToCharArray());
                bw.Write(msList.ToArray());
            }
        }
    }

    public class RGN : Chunk {
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

        public RGN(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "rgnh":
                Header = (CK_RGNH)Marshal.PtrToStructure(ptr, typeof(CK_RGNH));
                if (chunkSize < Marshal.SizeOf<CK_RGNH>()) {
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
                throw new Exception(string.Format("Unknown ChunkType [{0}]", chunkType));
            }
        }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "lart":
            case "lar2":
                Articulations = new LART(ptr, ptrTerm);
                break;
            default:
                throw new Exception(string.Format("Unknown ListType [{0}]", listType));
            }
        }

        protected override string Write(BinaryWriter bw) {
            var data = Header.Bytes;
            bw.Write("rgnh".ToCharArray());
            bw.Write(data.Length);
            bw.Write(data);

            data = Sampler.Bytes;
            bw.Write("wsmp".ToCharArray());
            bw.Write((uint)(data.Length + Sampler.LoopCount * Marshal.SizeOf<WaveLoop>()));
            bw.Write(data);
            for (var i = 0; i < Sampler.LoopCount && i < Loops.Count; ++i) {
                bw.Write(Loops[i].Bytes);
            }

            data = WaveLink.Bytes;
            bw.Write("wlnk".ToCharArray());
            bw.Write(data.Length);
            bw.Write(data);

            Articulations.Write(bw);

            return "rgn ";
        }
    }

    public class LART : Chunk {
        public ART ART;

        public LART() { }

        public LART(IntPtr ptr, IntPtr endPtr) : base(ptr, endPtr) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "art1":
            case "art2":
                ART = new ART(ptr);
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", chunkType));
            }
        }

        public new void Write(BinaryWriter bw) {
            if (null == ART) {
                return;
            }

            var msArt = new MemoryStream();
            var bwArt = new BinaryWriter(msArt);
            bwArt.Write("art1".ToCharArray());
            bwArt.Write((uint)(Marshal.SizeOf<CK_ART1>() + ART.List.Count * Marshal.SizeOf<Connection>()));
            bwArt.Write((uint)8);
            bwArt.Write((uint)ART.List.Count);
            foreach (var art in ART.List) {
                bwArt.Write(art.Value.Bytes);
            }

            if (0 < msArt.Length) {
                bw.Write("LIST".ToCharArray());
                bw.Write((uint)(msArt.Length + 4));
                bw.Write("lart".ToCharArray());
                bw.Write(msArt.ToArray());
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

    public class WVPL : Chunk {
        public Dictionary<int, WAVE> List = new Dictionary<int, WAVE>();

        public WVPL() { }

        public WVPL(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "wave":
                List.Add(List.Count, new WAVE(ptr, ptrTerm));
                break;
            default:
                throw new Exception();
            }
        }

        public new void Write(BinaryWriter bw) {
            var msPtbl = new MemoryStream();
            var bwPtbl = new BinaryWriter(msPtbl);
            bwPtbl.Write("ptbl".ToCharArray());
            bwPtbl.Write((uint)(Marshal.SizeOf<CK_PTBL>() + List.Count * sizeof(uint)));
            bwPtbl.Write((uint)8);
            bwPtbl.Write((uint)List.Count);

            var msWave = new MemoryStream();
            var bwWave = new BinaryWriter(msWave);
            foreach (var wav in List) {
                bwPtbl.Write((uint)msWave.Position);
                bwWave.Write(wav.Value.Bytes);
            }

            if (0 < msWave.Length) {
                bw.Write(msPtbl.ToArray());
                bw.Write("LIST".ToCharArray());
                bw.Write((uint)(msWave.Length + 4));
                bw.Write("wvpl".ToCharArray());
                bw.Write(msWave.ToArray());
            }
        }
    }

    public class WAVE : Chunk {
        public CK_FMT Format;
        public CK_WSMP Sampler;
        public Dictionary<int, WaveLoop> Loops = new Dictionary<int, WaveLoop>();
        public byte[] Data;
        public Info Info = new Info();

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
                    switch (Marshal.PtrToStringAnsi(pChunkData, 4)) {
                    case "INFO":
                        Info = new Info(pChunkData + 4, pChunkData + (int)chunkSize);
                        break;
                    }
                    break;
                }
                Marshal.FreeHGlobal(pChunkData);
            }

            if (null == Info) {
                Info = new Info();
                Info.Name = Path.GetFileNameWithoutExtension(filePath);
            } else if (string.IsNullOrWhiteSpace(Info.Name)) {
                Info.Name = Path.GetFileNameWithoutExtension(filePath);
            }

            br.Dispose();
            fs.Close();
            fs.Dispose();
        }

        public WAVE(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "dlid":
            case "guid":
                break;
            case "fmt ":
                Format = (CK_FMT)Marshal.PtrToStructure(ptr, typeof(CK_FMT));
                break;
            case "data":
                Data = new byte[chunkSize];
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

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "INFO":
                Info = new Info(ptr, ptrTerm);
                break;
            }
        }

        protected override string Write(BinaryWriter bw) {
            var data = Sampler.Bytes;
            bw.Write("wsmp".ToCharArray());
            bw.Write((uint)(data.Length + Sampler.LoopCount * Marshal.SizeOf<WaveLoop>()));
            bw.Write(data);
            foreach (var loop in Loops.Values) {
                bw.Write(loop.Bytes);
            }

            data = Format.Bytes;
            bw.Write("fmt ".ToCharArray());
            bw.Write(data.Length);
            bw.Write(data);

            bw.Write("data".ToCharArray());
            bw.Write(Data.Length);
            bw.Write(Data);

            Info.Write(bw);

            return "wave";
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

            bw.Write("RIFF".ToCharArray());
            bw.Write((uint)0);
            bw.Write("WAVE".ToCharArray());

            bw.Write("fmt ".ToCharArray());
            bw.Write((uint)16);
            bw.Write(Format.Tag);
            bw.Write(Format.Channels);
            bw.Write(Format.SampleRate);
            bw.Write(Format.BytesPerSec);
            bw.Write(Format.BlockAlign);
            bw.Write(Format.Bits);

            bw.Write("data".ToCharArray());
            bw.Write((uint)msw.Length);
            bw.Write(msw.ToArray());

            var data = Sampler.Bytes;
            bw.Write("wsmp".ToCharArray());
            bw.Write((uint)(data.Length + Sampler.LoopCount * Marshal.SizeOf<WaveLoop>()));
            bw.Write(data);
            foreach (var loop in Loops.Values) {
                bw.Write(loop.Bytes);
            }

            Info.Write(bw);

            fs.Seek(4, SeekOrigin.Begin);
            bw.Write((uint)(fs.Length - 8));

            bw.Dispose();
            fs.Close();
            fs.Dispose();
        }
    }
}
