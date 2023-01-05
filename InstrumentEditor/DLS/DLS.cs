using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using InstPack;

namespace DLS {
    public class File : Riff {
        private CK_VERS mVersion;
        private uint mMSYN = 1;

        public LINS Instruments = new LINS();
        public WVPL WavePool = new WVPL();
        public Info Info = new Info();

        public File() { }

        public File(string filePath) {
            MainLoop(filePath);
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

        public Pack ToPack() {
            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            var pack = new Pack();

            foreach (var wave in WavePool.List) {
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

                waveInfo.Info.CopyFrom(wave.Info);
                waveInfo.Info[Info.TYPE.ICRD] = now;

                pack.Wave.Add(waveInfo);
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
                lyr.InstIndex = pack.Inst.Count;

                pres.Layer.Add(lyr);
                pres.Info.CopyFrom(dlsInst.Value.Info);
                pres.Info[Info.TYPE.ICRD] = now;
                pack.Preset.Add(pres.Header, pres);

                var inst = new Inst();
                inst.Info.CopyFrom(dlsInst.Value.Info);
                inst.Info[Info.TYPE.ICRD] = now;

                if (null != dlsInst.Value.Articulations && null != dlsInst.Value.Articulations.ART) {
                    foreach (var instArt in dlsInst.Value.Articulations.ART.List) {
                        var art = new InstPack.ART {
                            Value = instArt.Value,
                            Source = (uint)instArt.Source,
                            Contorol = (uint)instArt.Control
                        };

                        switch (instArt.Destination) {
                        case DST_TYPE.EG1_ATTACK_TIME:
                            art.Type = ART_TYPE.EG_AMP_ATTACK;
                            inst.Art.Add(art);
                            break;
                        case DST_TYPE.EG1_HOLD_TIME:
                            art.Type = ART_TYPE.EG_AMP_HOLD;
                            inst.Art.Add(art);
                            break;
                        case DST_TYPE.EG1_DECAY_TIME:
                            art.Type = ART_TYPE.EG_AMP_DECAY;
                            inst.Art.Add(art);
                            break;
                        case DST_TYPE.EG1_SUSTAIN_LEVEL:
                            art.Type = ART_TYPE.EG_AMP_SUSTAIN;
                            inst.Art.Add(art);
                            break;
                        case DST_TYPE.EG1_RELEASE_TIME:
                            art.Type = ART_TYPE.EG_AMP_RELEASE;
                            inst.Art.Add(art);
                            break;

                        case DST_TYPE.EG2_ATTACK_TIME:
                            art.Type = ART_TYPE.EG_CUTOFF_ATTACK;
                            inst.Art.Add(art);
                            break;
                        case DST_TYPE.EG2_HOLD_TIME:
                            art.Type = ART_TYPE.EG_CUTOFF_HOLD;
                            inst.Art.Add(art);
                            break;
                        case DST_TYPE.EG2_DECAY_TIME:
                            art.Type = ART_TYPE.EG_CUTOFF_DECAY;
                            inst.Art.Add(art);
                            break;
                        case DST_TYPE.EG2_SUSTAIN_LEVEL:
                            art.Type = ART_TYPE.EG_CUTOFF_SUSTAIN;
                            inst.Art.Add(art);
                            break;
                        case DST_TYPE.EG2_RELEASE_TIME:
                            art.Type = ART_TYPE.EG_CUTOFF_RELEASE;
                            inst.Art.Add(art);
                            break;

                        case DST_TYPE.GAIN:
                            art.Type = ART_TYPE.GAIN;
                            inst.Art.Add(art);
                            break;
                        case DST_TYPE.PAN:
                            art.Type = ART_TYPE.PAN;
                            inst.Art.Add(art);
                            break;
                        case DST_TYPE.PITCH:
                            art.Type = ART_TYPE.FINE_TUNE;
                            inst.Art.Add(art);
                            break;
                        case DST_TYPE.FILTER_Q:
                            art.Type = ART_TYPE.LPF_RESONANCE;
                            inst.Art.Add(art);
                            break;
                        case DST_TYPE.FILTER_CUTOFF:
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

                    rgn.Art.Add(new InstPack.ART {
                        Type = ART_TYPE.WAVE_INDEX,
                        Value = dlsRegion.Value.WaveLink.TableIndex
                    });
                    rgn.Art.Add(new InstPack.ART {
                        Type = ART_TYPE.UNITY_KEY,
                        Value = dlsRegion.Value.Sampler.UnityNote
                    });
                    rgn.Art.Add(new InstPack.ART {
                        Type = ART_TYPE.FINE_TUNE,
                        Value = Math.Pow(2.0, dlsRegion.Value.Sampler.FineTune / 1200.0)
                    });
                    rgn.Art.Add(new InstPack.ART {
                        Type = ART_TYPE.GAIN,
                        Value = dlsRegion.Value.Sampler.Gain
                    });

                    if (null != dlsRegion.Value.Articulations && null != dlsRegion.Value.Articulations.ART) {
                        foreach (var regionArt in dlsRegion.Value.Articulations.ART.List) {
                            var art = new InstPack.ART {
                                Value = regionArt.Value,
                                Source = (uint)regionArt.Source,
                                Contorol = (uint)regionArt.Control
                            };

                            switch (regionArt.Destination) {
                            case DST_TYPE.EG1_ATTACK_TIME:
                                art.Type = ART_TYPE.EG_AMP_ATTACK;
                                rgn.Art.Add(art);
                                break;
                            case DST_TYPE.EG1_HOLD_TIME:
                                art.Type = ART_TYPE.EG_AMP_HOLD;
                                rgn.Art.Add(art);
                                break;
                            case DST_TYPE.EG1_DECAY_TIME:
                                art.Type = ART_TYPE.EG_AMP_DECAY;
                                rgn.Art.Add(art);
                                break;
                            case DST_TYPE.EG1_SUSTAIN_LEVEL:
                                art.Type = ART_TYPE.EG_AMP_SUSTAIN;
                                rgn.Art.Add(art);
                                break;
                            case DST_TYPE.EG1_RELEASE_TIME:
                                art.Type = ART_TYPE.EG_AMP_RELEASE;
                                rgn.Art.Add(art);
                                break;

                            case DST_TYPE.EG2_ATTACK_TIME:
                                art.Type = ART_TYPE.EG_CUTOFF_ATTACK;
                                rgn.Art.Add(art);
                                break;
                            case DST_TYPE.EG2_HOLD_TIME:
                                art.Type = ART_TYPE.EG_CUTOFF_HOLD;
                                rgn.Art.Add(art);
                                break;
                            case DST_TYPE.EG2_DECAY_TIME:
                                art.Type = ART_TYPE.EG_CUTOFF_DECAY;
                                rgn.Art.Add(art);
                                break;
                            case DST_TYPE.EG2_SUSTAIN_LEVEL:
                                art.Type = ART_TYPE.EG_CUTOFF_SUSTAIN;
                                rgn.Art.Add(art);
                                break;
                            case DST_TYPE.EG2_RELEASE_TIME:
                                art.Type = ART_TYPE.EG_CUTOFF_RELEASE;
                                rgn.Art.Add(art);
                                break;

                            case DST_TYPE.GAIN:
                                art.Type = ART_TYPE.GAIN;
                                rgn.Art.Add(art);
                                break;
                            case DST_TYPE.PAN:
                                art.Type = ART_TYPE.PAN;
                                rgn.Art.Add(art);
                                break;
                            case DST_TYPE.PITCH:
                                art.Type = ART_TYPE.FINE_TUNE;
                                rgn.Art.Add(art);
                                break;
                            case DST_TYPE.FILTER_Q:
                                art.Type = ART_TYPE.LPF_RESONANCE;
                                rgn.Art.Add(art);
                                break;
                            case DST_TYPE.FILTER_CUTOFF:
                                art.Type = ART_TYPE.LPF_CUTOFF;
                                rgn.Art.Add(art);
                                break;
                            }
                        }
                    }
                    inst.Region.Add(rgn);
                }
                pack.Inst.Add(inst);
            }

            return pack;
        }

        public static void SaveFromPack(string filePath, Pack pack) {
            var saveFile = new File();

            // WAVE
            saveFile.WavePool = new WVPL();
            saveFile.WavePool.List = new List<WAVE>();
            foreach (var wav in pack.Wave.ToArray()) {
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
                    wavh.Loops = new List<WaveLoop>() {
                        loop
                    };
                }

                wavh.Info.CopyFrom(wav.Info);

                wavh.Data = new byte[wav.Data.Length * 2];
                var pData = Marshal.AllocHGlobal(wavh.Data.Length);
                Marshal.Copy(wav.Data, 0, pData, wav.Data.Length);
                Marshal.Copy(pData, wavh.Data, 0, wavh.Data.Length);
                Marshal.FreeHGlobal(pData);

                saveFile.WavePool.List.Add(wavh);
            }

            // Inst
            saveFile.Instruments = new LINS();
            saveFile.Instruments.List = new SortedDictionary<MidiLocale, INS>(new LINS.Sort());
            foreach (var srcPre in pack.Preset.Values) {
                if (1 != srcPre.Layer.Count) {
                    continue;
                }

                var srcIns = pack.Inst[srcPre.Layer[0].InstIndex];

                var ins = new INS();
                ins.Header = new CK_INSH();
                ins.Header.Locale = new MidiLocale();
                ins.Header.Locale.BankFlg = (byte)(srcPre.Header.IsDrum ? 0x80 : 0x00);
                ins.Header.Locale.BankMSB = srcPre.Header.BankMSB;
                ins.Header.Locale.BankLSB = srcPre.Header.BankLSB;
                ins.Header.Locale.ProgNum = srcPre.Header.ProgNum;
                ins.Header.Regions = (uint)srcIns.Region.Count;

                ins.Info.CopyFrom(srcPre.Info);

                ins.Articulations = new LART();
                ins.Articulations.ART = new ART();
                ins.Articulations.ART.List = new List<Connection>();
                foreach (var srcArt in srcIns.Art.ToArray()) {
                    switch (srcArt.Type) {
                    case ART_TYPE.EG_AMP_ATTACK:
                        var ampA = new Connection();
                        ampA.Source = (SRC_TYPE)srcArt.Source;
                        ampA.Control = (SRC_TYPE)srcArt.Contorol;
                        ampA.Destination = DST_TYPE.EG1_ATTACK_TIME;
                        ampA.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ampA);
                        break;
                    case ART_TYPE.EG_AMP_HOLD:
                        var ampH = new Connection();
                        ampH.Source = (SRC_TYPE)srcArt.Source;
                        ampH.Control = (SRC_TYPE)srcArt.Contorol;
                        ampH.Destination = DST_TYPE.EG1_HOLD_TIME;
                        ampH.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ampH);
                        break;
                    case ART_TYPE.EG_AMP_DECAY:
                        var ampD = new Connection();
                        ampD.Source = (SRC_TYPE)srcArt.Source;
                        ampD.Control = (SRC_TYPE)srcArt.Contorol;
                        ampD.Destination = DST_TYPE.EG1_DECAY_TIME;
                        ampD.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ampD);
                        break;
                    case ART_TYPE.EG_AMP_SUSTAIN:
                        var ampS = new Connection();
                        ampS.Source = (SRC_TYPE)srcArt.Source;
                        ampS.Control = (SRC_TYPE)srcArt.Contorol;
                        ampS.Destination = DST_TYPE.EG1_SUSTAIN_LEVEL;
                        ampS.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ampS);
                        break;
                    case ART_TYPE.EG_AMP_RELEASE:
                        var ampR = new Connection();
                        ampR.Source = (SRC_TYPE)srcArt.Source;
                        ampR.Control = (SRC_TYPE)srcArt.Contorol;
                        ampR.Destination = DST_TYPE.EG1_RELEASE_TIME;
                        ampR.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ampR);
                        break;
                    case ART_TYPE.EG_CUTOFF_ATTACK:
                        var fcA = new Connection();
                        fcA.Source = (SRC_TYPE)srcArt.Source;
                        fcA.Control = (SRC_TYPE)srcArt.Contorol;
                        fcA.Destination = DST_TYPE.EG1_ATTACK_TIME;
                        fcA.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(fcA);
                        break;
                    case ART_TYPE.EG_CUTOFF_HOLD:
                        var fcH = new Connection();
                        fcH.Source = (SRC_TYPE)srcArt.Source;
                        fcH.Control = (SRC_TYPE)srcArt.Contorol;
                        fcH.Destination = DST_TYPE.EG1_HOLD_TIME;
                        fcH.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(fcH);
                        break;
                    case ART_TYPE.EG_CUTOFF_DECAY:
                        var fcD = new Connection();
                        fcD.Source = (SRC_TYPE)srcArt.Source;
                        fcD.Control = (SRC_TYPE)srcArt.Contorol;
                        fcD.Destination = DST_TYPE.EG1_DECAY_TIME;
                        fcD.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(fcD);
                        break;
                    case ART_TYPE.EG_CUTOFF_SUSTAIN:
                        var fcS = new Connection();
                        fcS.Source = (SRC_TYPE)srcArt.Source;
                        fcS.Control = (SRC_TYPE)srcArt.Contorol;
                        fcS.Destination = DST_TYPE.EG1_SUSTAIN_LEVEL;
                        fcS.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(fcS);
                        break;
                    case ART_TYPE.EG_CUTOFF_RELEASE:
                        var fcR = new Connection();
                        fcR.Source = (SRC_TYPE)srcArt.Source;
                        fcR.Control = (SRC_TYPE)srcArt.Contorol;
                        fcR.Destination = DST_TYPE.EG1_RELEASE_TIME;
                        fcR.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(fcR);
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
                    rgn.Articulations.ART.List = new List<Connection>();
                    foreach (var srcArt in srcRgn.Art.ToArray()) {
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
                            ampA.Source = (SRC_TYPE)srcArt.Source;
                            ampA.Control = (SRC_TYPE)srcArt.Contorol;
                            ampA.Destination = DST_TYPE.EG1_ATTACK_TIME;
                            ampA.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(ampA);
                            break;
                        case ART_TYPE.EG_AMP_HOLD:
                            var ampH = new Connection();
                            ampH.Source = (SRC_TYPE)srcArt.Source;
                            ampH.Control = (SRC_TYPE)srcArt.Contorol;
                            ampH.Destination = DST_TYPE.EG1_HOLD_TIME;
                            ampH.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(ampH);
                            break;
                        case ART_TYPE.EG_AMP_DECAY:
                            var ampD = new Connection();
                            ampD.Source = (SRC_TYPE)srcArt.Source;
                            ampD.Control = (SRC_TYPE)srcArt.Contorol;
                            ampD.Destination = DST_TYPE.EG1_DECAY_TIME;
                            ampD.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(ampD);
                            break;
                        case ART_TYPE.EG_AMP_SUSTAIN:
                            var ampS = new Connection();
                            ampS.Source = (SRC_TYPE)srcArt.Source;
                            ampS.Control = (SRC_TYPE)srcArt.Contorol;
                            ampS.Destination = DST_TYPE.EG1_SUSTAIN_LEVEL;
                            ampS.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(ampS);
                            break;
                        case ART_TYPE.EG_AMP_RELEASE:
                            var ampR = new Connection();
                            ampR.Source = (SRC_TYPE)srcArt.Source;
                            ampR.Control = (SRC_TYPE)srcArt.Contorol;
                            ampR.Destination = DST_TYPE.EG1_RELEASE_TIME;
                            ampR.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(ampR);
                            break;
                        case ART_TYPE.EG_CUTOFF_ATTACK:
                            var fcA = new Connection();
                            fcA.Source = (SRC_TYPE)srcArt.Source;
                            fcA.Control = (SRC_TYPE)srcArt.Contorol;
                            fcA.Destination = DST_TYPE.EG1_ATTACK_TIME;
                            fcA.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(fcA);
                            break;
                        case ART_TYPE.EG_CUTOFF_HOLD:
                            var fcH = new Connection();
                            fcH.Source = (SRC_TYPE)srcArt.Source;
                            fcH.Control = (SRC_TYPE)srcArt.Contorol;
                            fcH.Destination = DST_TYPE.EG1_HOLD_TIME;
                            fcH.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(fcH);
                            break;
                        case ART_TYPE.EG_CUTOFF_DECAY:
                            var fcD = new Connection();
                            fcD.Source = (SRC_TYPE)srcArt.Source;
                            fcD.Control = (SRC_TYPE)srcArt.Contorol;
                            fcD.Destination = DST_TYPE.EG1_DECAY_TIME;
                            fcD.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(fcD);
                            break;
                        case ART_TYPE.EG_CUTOFF_SUSTAIN:
                            var fcS = new Connection();
                            fcS.Source = (SRC_TYPE)srcArt.Source;
                            fcS.Control = (SRC_TYPE)srcArt.Contorol;
                            fcS.Destination = DST_TYPE.EG1_SUSTAIN_LEVEL;
                            fcS.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(fcS);
                            break;
                        case ART_TYPE.EG_CUTOFF_RELEASE:
                            var fcR = new Connection();
                            fcR.Source = (SRC_TYPE)srcArt.Source;
                            fcR.Control = (SRC_TYPE)srcArt.Contorol;
                            fcR.Destination = DST_TYPE.EG1_RELEASE_TIME;
                            fcR.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(fcR);
                            break;
                        }
                    }

                    ins.Regions.List.Add(rgn.Header, rgn);
                }

                saveFile.Instruments.List.Add(ins.Header.Locale, ins);
            }

            saveFile.Save(filePath);
        }

        protected override void LoadChunk(IntPtr ptr, string type, long size) {
            switch (type) {
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
            case "lins":
                Instruments = new LINS(ptr, size);
                break;
            case "wvpl":
                WavePool = new WVPL(ptr, size);
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", type));
            }
        }

        protected override void LoadInfo(IntPtr ptr, string type, string value) {
            Info[type] = value;
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

        public SortedDictionary<CK_RGNH, RGN> List = new SortedDictionary<CK_RGNH, RGN>(new Sort());

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
        public ART ART;

        public LART() { }

        public LART(IntPtr ptr, long size) {
            Load(ptr, size);
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
                art.Write(bwArt);
            }

            if (0 < msArt.Length) {
                bw.Write("LIST".ToCharArray());
                bw.Write((uint)(msArt.Length + 4));
                bw.Write("lart".ToCharArray());
                bw.Write(msArt.ToArray());
            }
        }

        protected override void LoadChunk(IntPtr ptr, string type, long size) {
            switch (type) {
            case "art1":
            case "art2":
                ART = new ART(ptr);
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

        public void Write(BinaryWriter bw) {
            var msSmp = new MemoryStream();
            var bwSmp = new BinaryWriter(msSmp);
            bwSmp.Write("LIST".ToCharArray());
            bwSmp.Write(0xFFFFFFFF);
            bwSmp.Write("wave".ToCharArray());

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
