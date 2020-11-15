﻿using Instruments;
using Riff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace DLS {
    #region struct
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
    #endregion

    public class File : Chunk {
        private string mFilePath;
        private CK_VERS mVersion;
        private uint mMSYN = 1;

        public LINS Instruments = new LINS();
        public WVPL WavePool = new WVPL();
        public Info Info = new Info();

        public File() { }

        public File(string filePath) : base(filePath) {
            mFilePath = filePath;
        }

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

        public void Save(string filePath) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write("DLS ".ToCharArray());

            bw.Write("colh".ToCharArray());
            bw.Write((uint)4);
            bw.Write((uint)Instruments.List.Count);

            mVersion.Write(bw);

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

        public Instruments.File ToIns() {
            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            var instFile = new Instruments.File();

            foreach (var wave in WavePool.List.Values) {
                var waveInfo = new Wave();
                waveInfo.Header.SampleRate = wave.Format.SampleRate;
                if (0 < wave.Sampler.LoopCount) {
                    waveInfo.Header.LoopEnable = 1;
                    waveInfo.Header.LoopBegin = wave.Loops[0].Start;
                    waveInfo.Header.LoopLength = wave.Loops[0].Length;
                } else {
                    waveInfo.Header.LoopEnable = 0;
                    waveInfo.Header.LoopBegin = 0;
                    waveInfo.Header.LoopLength = (uint)(wave.Data.Length / wave.Format.BlockAlign);
                }
                waveInfo.Header.UnityNote = (byte)wave.Sampler.UnityNote;
                waveInfo.Header.Gain = wave.Sampler.Gain;
                waveInfo.Header.Pitch = Math.Pow(2.0, wave.Sampler.FineTune / 1200.0);

                var ptr = Marshal.AllocHGlobal(wave.Data.Length);
                Marshal.Copy(wave.Data, 0, ptr, wave.Data.Length);
                waveInfo.Data = new short[wave.Data.Length / 2];
                Marshal.Copy(ptr, waveInfo.Data, 0, waveInfo.Data.Length);
                Marshal.FreeHGlobal(ptr);

                waveInfo.Info.Name = wave.Info.Name;
                waveInfo.Info.Category = wave.Info.Category;
                waveInfo.Info.CreationDate = now;
                waveInfo.Info.SourceForm = Path.GetFileName(mFilePath);

                instFile.Wave.Add(waveInfo);
            }

            foreach (var dlsInst in Instruments.List) {
                var pres = new Preset();
                pres.Header.IsDrum = 0 < (dlsInst.Key.BankFlg & 0x80);
                pres.Header.BankMSB = dlsInst.Key.BankMSB;
                pres.Header.BankLSB = dlsInst.Key.BankLSB;
                pres.Header.ProgNum = dlsInst.Key.ProgNum;

                var lyr = new Layer();
                lyr.Header.KeyLo = 0;
                lyr.Header.KeyHi = 127;
                lyr.Header.VelLo = 0;
                lyr.Header.VelHi = 127;
                lyr.Header.InstIndex = instFile.Inst.Count;

                pres.Layer.Add(lyr);
                pres.Info.Name = dlsInst.Value.Info.Name;
                pres.Info.Category = dlsInst.Value.Info.Category;
                pres.Info.CreationDate = now;
                pres.Info.SourceForm = Path.GetFileName(mFilePath);
                instFile.Preset.Add(pres.Header, pres);

                var inst = new Inst();
                inst.Info.Name = dlsInst.Value.Info.Name;
                inst.Info.Category = dlsInst.Value.Info.Category;
                inst.Info.CreationDate = now;
                inst.Info.SourceForm = Path.GetFileName(mFilePath);

                if (null != dlsInst.Value.Articulations && null != dlsInst.Value.Articulations.ART) {
                    foreach (var instArt in dlsInst.Value.Articulations.ART.List.Values) {
                        if (instArt.Source != Connection.SRC_TYPE.NONE || instArt.Control != Connection.SRC_TYPE.NONE) {
                            continue;
                        }

                        var art = new Instruments.ART {
                            Value = (float)instArt.Value
                        };

                        switch (instArt.Destination) {
                        case Connection.DST_TYPE.EG1_ATTACK_TIME:
                            art.Type = ART_TYPE.EG_AMP_ATTACK;
                            inst.Art.Add(art);
                            break;
                        case Connection.DST_TYPE.EG1_HOLD_TIME:
                            art.Type = ART_TYPE.EG_AMP_HOLD;
                            inst.Art.Add(art);
                            break;
                        case Connection.DST_TYPE.EG1_DECAY_TIME:
                            art.Type = ART_TYPE.EG_AMP_DECAY;
                            inst.Art.Add(art);
                            break;
                        case Connection.DST_TYPE.EG1_SUSTAIN_LEVEL:
                            art.Type = ART_TYPE.EG_AMP_SUSTAIN;
                            inst.Art.Add(art);
                            break;
                        case Connection.DST_TYPE.EG1_RELEASE_TIME:
                            art.Type = ART_TYPE.EG_AMP_RELEASE;
                            inst.Art.Add(art);
                            break;

                        case Connection.DST_TYPE.EG2_ATTACK_TIME:
                            art.Type = ART_TYPE.EG_CUTOFF_ATTACK;
                            inst.Art.Add(art);
                            break;
                        case Connection.DST_TYPE.EG2_HOLD_TIME:
                            art.Type = ART_TYPE.EG_CUTOFF_HOLD;
                            inst.Art.Add(art);
                            break;
                        case Connection.DST_TYPE.EG2_DECAY_TIME:
                            art.Type = ART_TYPE.EG_CUTOFF_DECAY;
                            inst.Art.Add(art);
                            break;
                        case Connection.DST_TYPE.EG2_SUSTAIN_LEVEL:
                            art.Type = ART_TYPE.EG_CUTOFF_SUSTAIN;
                            inst.Art.Add(art);
                            break;
                        case Connection.DST_TYPE.EG2_RELEASE_TIME:
                            art.Type = ART_TYPE.EG_CUTOFF_RELEASE;
                            inst.Art.Add(art);
                            break;

                        case Connection.DST_TYPE.GAIN:
                            art.Type = ART_TYPE.GAIN;
                            inst.Art.Add(art);
                            break;
                        case Connection.DST_TYPE.PAN:
                            art.Type = ART_TYPE.PAN;
                            inst.Art.Add(art);
                            break;
                        case Connection.DST_TYPE.PITCH:
                            art.Type = ART_TYPE.FINE_TUNE;
                            inst.Art.Add(art);
                            break;
                        case Connection.DST_TYPE.FILTER_Q:
                            art.Type = ART_TYPE.LPF_RESONANCE;
                            inst.Art.Add(art);
                            break;
                        case Connection.DST_TYPE.FILTER_CUTOFF:
                            art.Type = ART_TYPE.LPF_CUTOFF;
                            inst.Art.Add(art);
                            break;
                        }
                    }
                }

                foreach (var dlsRegion in dlsInst.Value.Regions.List) {
                    var rgn = new Region();
                    rgn.Header.KeyLo = (byte)dlsRegion.Key.Key.Lo;
                    rgn.Header.KeyHi = (byte)dlsRegion.Key.Key.Hi;
                    rgn.Header.VelLo = (byte)dlsRegion.Key.Vel.Lo;
                    rgn.Header.VelHi = (byte)dlsRegion.Key.Vel.Hi;

                    rgn.Art.Add(new Instruments.ART {
                        Type = ART_TYPE.WAVE_INDEX,
                        Value = dlsRegion.Value.WaveLink.TableIndex
                    });
                    rgn.Art.Add(new Instruments.ART {
                        Type = ART_TYPE.UNITY_KEY,
                        Value = dlsRegion.Value.Sampler.UnityNote
                    });
                    rgn.Art.Add(new Instruments.ART {
                        Type = ART_TYPE.FINE_TUNE,
                        Value = (float)Math.Pow(2.0, dlsRegion.Value.Sampler.FineTune / 1200.0)
                    });
                    rgn.Art.Add(new Instruments.ART {
                        Type = ART_TYPE.GAIN,
                        Value = (float)dlsRegion.Value.Sampler.Gain
                    });

                    if (null != dlsRegion.Value.Articulations && null != dlsRegion.Value.Articulations.ART) {
                        foreach (var regionArt in dlsRegion.Value.Articulations.ART.List.Values) {
                            if (regionArt.Source != Connection.SRC_TYPE.NONE || regionArt.Control != Connection.SRC_TYPE.NONE) {
                                continue;
                            }

                            var art = new Instruments.ART {
                                Value = (float)regionArt.Value
                            };

                            switch (regionArt.Destination) {
                            case Connection.DST_TYPE.EG1_ATTACK_TIME:
                                art.Type = ART_TYPE.EG_AMP_ATTACK;
                                rgn.Art.Add(art);
                                break;
                            case Connection.DST_TYPE.EG1_HOLD_TIME:
                                art.Type = ART_TYPE.EG_AMP_HOLD;
                                rgn.Art.Add(art);
                                break;
                            case Connection.DST_TYPE.EG1_DECAY_TIME:
                                art.Type = ART_TYPE.EG_AMP_DECAY;
                                rgn.Art.Add(art);
                                break;
                            case Connection.DST_TYPE.EG1_SUSTAIN_LEVEL:
                                art.Type = ART_TYPE.EG_AMP_SUSTAIN;
                                rgn.Art.Add(art);
                                break;
                            case Connection.DST_TYPE.EG1_RELEASE_TIME:
                                art.Type = ART_TYPE.EG_AMP_RELEASE;
                                rgn.Art.Add(art);
                                break;

                            case Connection.DST_TYPE.EG2_ATTACK_TIME:
                                art.Type = ART_TYPE.EG_CUTOFF_ATTACK;
                                rgn.Art.Add(art);
                                break;
                            case Connection.DST_TYPE.EG2_HOLD_TIME:
                                art.Type = ART_TYPE.EG_CUTOFF_HOLD;
                                rgn.Art.Add(art);
                                break;
                            case Connection.DST_TYPE.EG2_DECAY_TIME:
                                art.Type = ART_TYPE.EG_CUTOFF_DECAY;
                                rgn.Art.Add(art);
                                break;
                            case Connection.DST_TYPE.EG2_SUSTAIN_LEVEL:
                                art.Type = ART_TYPE.EG_CUTOFF_SUSTAIN;
                                rgn.Art.Add(art);
                                break;
                            case Connection.DST_TYPE.EG2_RELEASE_TIME:
                                art.Type = ART_TYPE.EG_CUTOFF_RELEASE;
                                rgn.Art.Add(art);
                                break;

                            case Connection.DST_TYPE.GAIN:
                                art.Type = ART_TYPE.GAIN;
                                rgn.Art.Add(art);
                                break;
                            case Connection.DST_TYPE.PAN:
                                art.Type = ART_TYPE.PAN;
                                rgn.Art.Add(art);
                                break;
                            case Connection.DST_TYPE.PITCH:
                                art.Type = ART_TYPE.FINE_TUNE;
                                rgn.Art.Add(art);
                                break;
                            case Connection.DST_TYPE.FILTER_Q:
                                art.Type = ART_TYPE.LPF_RESONANCE;
                                rgn.Art.Add(art);
                                break;
                            case Connection.DST_TYPE.FILTER_CUTOFF:
                                art.Type = ART_TYPE.LPF_CUTOFF;
                                rgn.Art.Add(art);
                                break;
                            }
                        }
                    }
                    inst.Region.Add(rgn);
                }
                instFile.Inst.Add(inst);
            }

            return instFile;
        }

        public static void SaveFromIns(string filePath, Instruments.File insFile) {
            var saveFile = new File();

            // WAVE
            saveFile.WavePool = new WVPL();
            saveFile.WavePool.List = new Dictionary<int, WAVE>();
            foreach (var wav in insFile.Wave.Array) {
                var wavh = new WAVE();
                wavh.Format = new CK_FMT();
                wavh.Format.Tag = 1;
                wavh.Format.Bits = 16;
                wavh.Format.Channels = 1;
                wavh.Format.SampleRate = wav.Header.SampleRate;
                wavh.Format.BlockAlign = (ushort)(wavh.Format.Bits * wavh.Format.Channels / 8);
                wavh.Format.BytesPerSec = wavh.Format.SampleRate * wavh.Format.BlockAlign;

                wavh.Sampler = new CK_WSMP();
                wavh.Sampler.Gain = wav.Header.Gain;
                wavh.Sampler.FineTune = (short)(Math.Log(wav.Header.Pitch, 2.0) * 1200);
                wavh.Sampler.UnityNote = wav.Header.UnityNote;
                if (0 != wav.Header.LoopEnable) {
                    wavh.Sampler.LoopCount = 1;
                    var loop = new WaveLoop();
                    loop.Type = 1;
                    loop.Start = wav.Header.LoopBegin;
                    loop.Length = wav.Header.LoopLength;
                    wavh.Loops = new Dictionary<int, WaveLoop>();
                    wavh.Loops.Add(0, loop);
                }

                wavh.Info = new Info();
                wavh.Info.Name = wav.Info.Name;

                wavh.Data = new byte[wav.Data.Length * 2];
                var pData = Marshal.AllocHGlobal(wavh.Data.Length);
                Marshal.Copy(wav.Data, 0, pData, wav.Data.Length);
                Marshal.Copy(pData, wavh.Data, 0, wavh.Data.Length);
                Marshal.FreeHGlobal(pData);

                saveFile.WavePool.List.Add(saveFile.WavePool.List.Count, wavh);
            }

            // Inst
            saveFile.Instruments = new LINS();
            saveFile.Instruments.List = new SortedDictionary<MidiLocale, INS>(new LINS.Sort());
            foreach (var srcPre in insFile.Preset.Values) {
                if (1 != srcPre.Layer.Array.Length) {
                    continue;
                }

                var srcIns = insFile.Inst[srcPre.Layer.Array[0].Header.InstIndex];

                var ins = new INS();
                ins.Header = new CK_INSH();
                ins.Header.Locale = new MidiLocale();
                ins.Header.Locale.BankFlg = (byte)(srcPre.Header.IsDrum ? 0x80 : 0x00);
                ins.Header.Locale.BankMSB = srcPre.Header.BankMSB;
                ins.Header.Locale.BankLSB = srcPre.Header.BankLSB;
                ins.Header.Locale.ProgNum = srcPre.Header.ProgNum;
                ins.Header.Regions = (uint)srcIns.Region.Count;

                ins.Info = new Info();
                ins.Info.Name = srcPre.Info.Name;
                ins.Info.Category = srcPre.Info.Category;

                ins.Articulations = new LART();
                ins.Articulations.ART = new ART();
                ins.Articulations.ART.List = new Dictionary<int, Connection>();
                foreach (var srcArt in srcIns.Art.Array) {
                    switch (srcArt.Type) {
                    case ART_TYPE.EG_AMP_ATTACK:
                        var ampA = new Connection();
                        ampA.Source = Connection.SRC_TYPE.NONE;
                        ampA.Control = Connection.SRC_TYPE.NONE;
                        ampA.Destination = Connection.DST_TYPE.EG1_ATTACK_TIME;
                        ampA.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, ampA);
                        break;
                    case ART_TYPE.EG_AMP_HOLD:
                        var ampH = new Connection();
                        ampH.Source = Connection.SRC_TYPE.NONE;
                        ampH.Control = Connection.SRC_TYPE.NONE;
                        ampH.Destination = Connection.DST_TYPE.EG1_HOLD_TIME;
                        ampH.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, ampH);
                        break;
                    case ART_TYPE.EG_AMP_DECAY:
                        var ampD = new Connection();
                        ampD.Source = Connection.SRC_TYPE.NONE;
                        ampD.Control = Connection.SRC_TYPE.NONE;
                        ampD.Destination = Connection.DST_TYPE.EG1_DECAY_TIME;
                        ampD.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, ampD);
                        break;
                    case ART_TYPE.EG_AMP_SUSTAIN:
                        var ampS = new Connection();
                        ampS.Source = Connection.SRC_TYPE.NONE;
                        ampS.Control = Connection.SRC_TYPE.NONE;
                        ampS.Destination = Connection.DST_TYPE.EG1_SUSTAIN_LEVEL;
                        ampS.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, ampS);
                        break;
                    case ART_TYPE.EG_AMP_RELEASE:
                        var ampR = new Connection();
                        ampR.Source = Connection.SRC_TYPE.NONE;
                        ampR.Control = Connection.SRC_TYPE.NONE;
                        ampR.Destination = Connection.DST_TYPE.EG1_RELEASE_TIME;
                        ampR.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, ampR);
                        break;
                    case ART_TYPE.EG_CUTOFF_ATTACK:
                        var fcA = new Connection();
                        fcA.Source = Connection.SRC_TYPE.NONE;
                        fcA.Control = Connection.SRC_TYPE.NONE;
                        fcA.Destination = Connection.DST_TYPE.EG1_ATTACK_TIME;
                        fcA.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, fcA);
                        break;
                    case ART_TYPE.EG_CUTOFF_HOLD:
                        var fcH = new Connection();
                        fcH.Source = Connection.SRC_TYPE.NONE;
                        fcH.Control = Connection.SRC_TYPE.NONE;
                        fcH.Destination = Connection.DST_TYPE.EG1_HOLD_TIME;
                        fcH.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, fcH);
                        break;
                    case ART_TYPE.EG_CUTOFF_DECAY:
                        var fcD = new Connection();
                        fcD.Source = Connection.SRC_TYPE.NONE;
                        fcD.Control = Connection.SRC_TYPE.NONE;
                        fcD.Destination = Connection.DST_TYPE.EG1_DECAY_TIME;
                        fcD.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, fcD);
                        break;
                    case ART_TYPE.EG_CUTOFF_SUSTAIN:
                        var fcS = new Connection();
                        fcS.Source = Connection.SRC_TYPE.NONE;
                        fcS.Control = Connection.SRC_TYPE.NONE;
                        fcS.Destination = Connection.DST_TYPE.EG1_SUSTAIN_LEVEL;
                        fcS.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, fcS);
                        break;
                    case ART_TYPE.EG_CUTOFF_RELEASE:
                        var fcR = new Connection();
                        fcR.Source = Connection.SRC_TYPE.NONE;
                        fcR.Control = Connection.SRC_TYPE.NONE;
                        fcR.Destination = Connection.DST_TYPE.EG1_RELEASE_TIME;
                        fcR.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, fcR);
                        break;
                    }
                }

                ins.Regions = new LRGN();
                ins.Regions.List = new SortedDictionary<CK_RGNH, RGN>(new LRGN.Sort());
                foreach(var srcRgn in srcIns.Region.Array) {
                    var rgn = new RGN();
                    rgn.Header.Key.Hi = srcRgn.Header.KeyHi;
                    rgn.Header.Key.Lo = srcRgn.Header.KeyLo;
                    rgn.Header.Vel.Hi = srcRgn.Header.VelHi;
                    rgn.Header.Vel.Lo = srcRgn.Header.VelLo;

                    rgn.Articulations = new LART();
                    rgn.Articulations.ART = new ART();
                    rgn.Articulations.ART.List = new Dictionary<int, Connection>();
                    foreach (var srcArt in srcRgn.Art.Array) {
                        switch(srcArt.Type) {
                        case ART_TYPE.WAVE_INDEX:
                            rgn.WaveLink.Channel = 1;
                            rgn.WaveLink.TableIndex = (uint)srcArt.Value;
                            break;
                        case ART_TYPE.GAIN:
                            rgn.Sampler.Gain = srcArt.Value;
                            break;
                        case ART_TYPE.UNITY_KEY:
                            rgn.Sampler.UnityNote = (ushort)srcArt.Value;
                            break;
                        case ART_TYPE.FINE_TUNE:
                            rgn.Sampler.FineTune = (short)(Math.Log(srcArt.Value, 2.0) * 1200);
                            break;

                        case ART_TYPE.EG_AMP_ATTACK:
                            var ampA = new Connection();
                            ampA.Source = Connection.SRC_TYPE.NONE;
                            ampA.Control = Connection.SRC_TYPE.NONE;
                            ampA.Destination = Connection.DST_TYPE.EG1_ATTACK_TIME;
                            ampA.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, ampA);
                            break;
                        case ART_TYPE.EG_AMP_HOLD:
                            var ampH = new Connection();
                            ampH.Source = Connection.SRC_TYPE.NONE;
                            ampH.Control = Connection.SRC_TYPE.NONE;
                            ampH.Destination = Connection.DST_TYPE.EG1_HOLD_TIME;
                            ampH.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, ampH);
                            break;
                        case ART_TYPE.EG_AMP_DECAY:
                            var ampD = new Connection();
                            ampD.Source = Connection.SRC_TYPE.NONE;
                            ampD.Control = Connection.SRC_TYPE.NONE;
                            ampD.Destination = Connection.DST_TYPE.EG1_DECAY_TIME;
                            ampD.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, ampD);
                            break;
                        case ART_TYPE.EG_AMP_SUSTAIN:
                            var ampS = new Connection();
                            ampS.Source = Connection.SRC_TYPE.NONE;
                            ampS.Control = Connection.SRC_TYPE.NONE;
                            ampS.Destination = Connection.DST_TYPE.EG1_SUSTAIN_LEVEL;
                            ampS.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, ampS);
                            break;
                        case ART_TYPE.EG_AMP_RELEASE:
                            var ampR = new Connection();
                            ampR.Source = Connection.SRC_TYPE.NONE;
                            ampR.Control = Connection.SRC_TYPE.NONE;
                            ampR.Destination = Connection.DST_TYPE.EG1_RELEASE_TIME;
                            ampR.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, ampR);
                            break;
                        case ART_TYPE.EG_CUTOFF_ATTACK:
                            var fcA = new Connection();
                            fcA.Source = Connection.SRC_TYPE.NONE;
                            fcA.Control = Connection.SRC_TYPE.NONE;
                            fcA.Destination = Connection.DST_TYPE.EG1_ATTACK_TIME;
                            fcA.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, fcA);
                            break;
                        case ART_TYPE.EG_CUTOFF_HOLD:
                            var fcH = new Connection();
                            fcH.Source = Connection.SRC_TYPE.NONE;
                            fcH.Control = Connection.SRC_TYPE.NONE;
                            fcH.Destination = Connection.DST_TYPE.EG1_HOLD_TIME;
                            fcH.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, fcH);
                            break;
                        case ART_TYPE.EG_CUTOFF_DECAY:
                            var fcD = new Connection();
                            fcD.Source = Connection.SRC_TYPE.NONE;
                            fcD.Control = Connection.SRC_TYPE.NONE;
                            fcD.Destination = Connection.DST_TYPE.EG1_DECAY_TIME;
                            fcD.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, fcD);
                            break;
                        case ART_TYPE.EG_CUTOFF_SUSTAIN:
                            var fcS = new Connection();
                            fcS.Source = Connection.SRC_TYPE.NONE;
                            fcS.Control = Connection.SRC_TYPE.NONE;
                            fcS.Destination = Connection.DST_TYPE.EG1_SUSTAIN_LEVEL;
                            fcS.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, fcS);
                            break;
                        case ART_TYPE.EG_CUTOFF_RELEASE:
                            var fcR = new Connection();
                            fcR.Source = Connection.SRC_TYPE.NONE;
                            fcR.Control = Connection.SRC_TYPE.NONE;
                            fcR.Destination = Connection.DST_TYPE.EG1_RELEASE_TIME;
                            fcR.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, fcR);
                            break;
                        }
                    }

                    ins.Regions.List.Add(rgn.Header, rgn);
                }

                saveFile.Instruments.List.Add(ins.Header.Locale, ins);
            }

            saveFile.Save(filePath);
        }
    }

    public class LINS : Chunk {
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
    }

    public class INS : Chunk {
        public CK_INSH Header;
        public LRGN Regions = new LRGN();
        public LART Articulations = new LART();
        public Info Info = new Info();

        public INS() { }

        public INS(byte programNo, byte bankMSB = 0, byte bankLSB = 0, bool isDrum = false) {
            Header.Locale.BankFlg = (byte)(isDrum ? 0x80 : 0x00);
            Header.Locale.ProgNum = programNo;
            Header.Locale.BankMSB = bankMSB;
            Header.Locale.BankLSB = bankLSB;
        }

        public INS(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "insh":
                Header = Marshal.PtrToStructure<CK_INSH>(ptr);
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
    }

    public class LRGN : Chunk {
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
    }

    public class RGN : Chunk {
        public CK_RGNH Header;
        public CK_WSMP Sampler;
        public Dictionary<int, WaveLoop> Loops = new Dictionary<int, WaveLoop>();
        public CK_WLNK WaveLink;
        public LART Articulations = new LART();

        public RGN() { }

        public RGN(byte noteLow = 0, byte noteHigh = 127, byte velocityLow = 0, byte velocityHigh = 127) {
            Header.Key.Lo = noteLow;
            Header.Key.Hi = noteHigh;
            Header.Vel.Lo = velocityLow;
            Header.Vel.Hi = velocityHigh;
        }

        public RGN(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "rgnh":
                Header = Marshal.PtrToStructure<CK_RGNH>(ptr);
                if (chunkSize < Marshal.SizeOf<CK_RGNH>()) {
                    Header.Layer = 0;
                }
                break;
            case "wsmp":
                Sampler = Marshal.PtrToStructure<CK_WSMP>(ptr);
                var pLoop = ptr + Marshal.SizeOf<CK_WSMP>();
                for (var i = 0; i < Sampler.LoopCount; ++i) {
                    Loops.Add(Loops.Count, Marshal.PtrToStructure<WaveLoop>(pLoop));
                    pLoop += Marshal.SizeOf<WaveLoop>();
                }
                break;
            case "wlnk":
                WaveLink = Marshal.PtrToStructure<CK_WLNK>(ptr);
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

        public void Write(BinaryWriter bw) {
            var msRgn = new MemoryStream();
            var bwRgn = new BinaryWriter(msRgn);
            bwRgn.Write("LIST".ToCharArray());
            bwRgn.Write(0xFFFFFFFF);
            bwRgn.Write("rgn ".ToCharArray());

            Header.Write(bwRgn);

            bwRgn.Write("wsmp".ToCharArray());
            bwRgn.Write((uint)(Marshal.SizeOf<CK_WSMP>() + Sampler.LoopCount * Marshal.SizeOf<WaveLoop>()));
            Sampler.Write(bwRgn);
            for (var i = 0; i < Sampler.LoopCount && i < Loops.Count; ++i) {
                Loops[i].Write(bwRgn);
            }

            WaveLink.Write(bwRgn);
            Articulations.Write(bwRgn);

            bwRgn.Seek(4, SeekOrigin.Begin);
            bwRgn.Write((uint)msRgn.Length - 8);
            bw.Write(msRgn.ToArray());
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

        public void Write(BinaryWriter bw) {
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
                art.Value.Write(bwArt);
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
            var info = Marshal.PtrToStructure<CK_ART1>(ptr);
            ptr += Marshal.SizeOf<CK_ART1>();

            for (var i = 0; i < info.Count; ++i) {
                List.Add(i, Marshal.PtrToStructure<Connection>(ptr));
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
                wave.Value.Write(bwWave);
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
    }

    public class WAVE : Chunk {
        public CK_FMT Format;
        public CK_WSMP Sampler;
        public Dictionary<int, WaveLoop> Loops = new Dictionary<int, WaveLoop>();
        public byte[] Data;
        public Info Info = new Info();

        public WAVE() { }

        public WAVE(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "dlid":
            case "guid":
                break;
            case "fmt ":
                Format = Marshal.PtrToStructure<CK_FMT>(ptr);
                break;
            case "data":
                Data = new byte[chunkSize];
                Marshal.Copy(ptr, Data, 0, Data.Length);
                break;
            case "wsmp":
                Sampler = Marshal.PtrToStructure<CK_WSMP>(ptr);
                var pLoop = ptr + Marshal.SizeOf<CK_WSMP>();
                for (var i = 0; i < Sampler.LoopCount; ++i) {
                    Loops.Add(Loops.Count, Marshal.PtrToStructure<WaveLoop>(pLoop));
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

        public void Write(BinaryWriter bw) {
            var msSmp = new MemoryStream();
            var bwSmp = new BinaryWriter(msSmp);
            bwSmp.Write("LIST".ToCharArray());
            bwSmp.Write(0xFFFFFFFF);
            bwSmp.Write("wave".ToCharArray());

            bwSmp.Write("wsmp".ToCharArray());
            bwSmp.Write((uint)(Marshal.SizeOf<CK_WSMP>() + Sampler.LoopCount * Marshal.SizeOf<WaveLoop>()));
            Sampler.Write(bwSmp);
            foreach (var loop in Loops.Values) {
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
    }
}
