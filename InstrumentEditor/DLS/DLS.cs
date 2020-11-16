using InstPack;
using Riff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace DLS {
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

        public Pack ToPack() {
            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            var pack = new Pack();

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
                lyr.Header.InstIndex = pack.Inst.Count;

                pres.Layer.Add(lyr);
                pres.Info.Name = dlsInst.Value.Info.Name;
                pres.Info.Category = dlsInst.Value.Info.Category;
                pres.Info.CreationDate = now;
                pres.Info.SourceForm = Path.GetFileName(mFilePath);
                pack.Preset.Add(pres.Header, pres);

                var inst = new Inst();
                inst.Info.Name = dlsInst.Value.Info.Name;
                inst.Info.Category = dlsInst.Value.Info.Category;
                inst.Info.CreationDate = now;
                inst.Info.SourceForm = Path.GetFileName(mFilePath);

                if (null != dlsInst.Value.Articulations && null != dlsInst.Value.Articulations.ART) {
                    foreach (var instArt in dlsInst.Value.Articulations.ART.List.Values) {
                        if (instArt.Source != SRC_TYPE.NONE || instArt.Control != SRC_TYPE.NONE) {
                            continue;
                        }

                        var art = new InstPack.ART {
                            Value = instArt.Value
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
                        foreach (var regionArt in dlsRegion.Value.Articulations.ART.List.Values) {
                            if (regionArt.Source != SRC_TYPE.NONE || regionArt.Control != SRC_TYPE.NONE) {
                                continue;
                            }

                            var art = new InstPack.ART {
                                Value = regionArt.Value
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
            saveFile.WavePool.List = new Dictionary<int, WAVE>();
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
            foreach (var srcPre in pack.Preset.Values) {
                if (1 != srcPre.Layer.Count) {
                    continue;
                }

                var srcIns = pack.Inst[srcPre.Layer[0].Header.InstIndex];

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
                foreach (var srcArt in srcIns.Art.ToArray()) {
                    switch (srcArt.Type) {
                    case ART_TYPE.EG_AMP_ATTACK:
                        var ampA = new Connection();
                        ampA.Source = SRC_TYPE.NONE;
                        ampA.Control = SRC_TYPE.NONE;
                        ampA.Destination = DST_TYPE.EG1_ATTACK_TIME;
                        ampA.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, ampA);
                        break;
                    case ART_TYPE.EG_AMP_HOLD:
                        var ampH = new Connection();
                        ampH.Source = SRC_TYPE.NONE;
                        ampH.Control = SRC_TYPE.NONE;
                        ampH.Destination = DST_TYPE.EG1_HOLD_TIME;
                        ampH.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, ampH);
                        break;
                    case ART_TYPE.EG_AMP_DECAY:
                        var ampD = new Connection();
                        ampD.Source = SRC_TYPE.NONE;
                        ampD.Control = SRC_TYPE.NONE;
                        ampD.Destination = DST_TYPE.EG1_DECAY_TIME;
                        ampD.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, ampD);
                        break;
                    case ART_TYPE.EG_AMP_SUSTAIN:
                        var ampS = new Connection();
                        ampS.Source = SRC_TYPE.NONE;
                        ampS.Control = SRC_TYPE.NONE;
                        ampS.Destination = DST_TYPE.EG1_SUSTAIN_LEVEL;
                        ampS.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, ampS);
                        break;
                    case ART_TYPE.EG_AMP_RELEASE:
                        var ampR = new Connection();
                        ampR.Source = SRC_TYPE.NONE;
                        ampR.Control = SRC_TYPE.NONE;
                        ampR.Destination = DST_TYPE.EG1_RELEASE_TIME;
                        ampR.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, ampR);
                        break;
                    case ART_TYPE.EG_CUTOFF_ATTACK:
                        var fcA = new Connection();
                        fcA.Source = SRC_TYPE.NONE;
                        fcA.Control = SRC_TYPE.NONE;
                        fcA.Destination = DST_TYPE.EG1_ATTACK_TIME;
                        fcA.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, fcA);
                        break;
                    case ART_TYPE.EG_CUTOFF_HOLD:
                        var fcH = new Connection();
                        fcH.Source = SRC_TYPE.NONE;
                        fcH.Control = SRC_TYPE.NONE;
                        fcH.Destination = DST_TYPE.EG1_HOLD_TIME;
                        fcH.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, fcH);
                        break;
                    case ART_TYPE.EG_CUTOFF_DECAY:
                        var fcD = new Connection();
                        fcD.Source = SRC_TYPE.NONE;
                        fcD.Control = SRC_TYPE.NONE;
                        fcD.Destination = DST_TYPE.EG1_DECAY_TIME;
                        fcD.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, fcD);
                        break;
                    case ART_TYPE.EG_CUTOFF_SUSTAIN:
                        var fcS = new Connection();
                        fcS.Source = SRC_TYPE.NONE;
                        fcS.Control = SRC_TYPE.NONE;
                        fcS.Destination = DST_TYPE.EG1_SUSTAIN_LEVEL;
                        fcS.Value = srcArt.Value;
                        ins.Articulations.ART.List.Add(ins.Articulations.ART.List.Count, fcS);
                        break;
                    case ART_TYPE.EG_CUTOFF_RELEASE:
                        var fcR = new Connection();
                        fcR.Source = SRC_TYPE.NONE;
                        fcR.Control = SRC_TYPE.NONE;
                        fcR.Destination = DST_TYPE.EG1_RELEASE_TIME;
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
                            ampA.Source = SRC_TYPE.NONE;
                            ampA.Control = SRC_TYPE.NONE;
                            ampA.Destination = DST_TYPE.EG1_ATTACK_TIME;
                            ampA.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, ampA);
                            break;
                        case ART_TYPE.EG_AMP_HOLD:
                            var ampH = new Connection();
                            ampH.Source = SRC_TYPE.NONE;
                            ampH.Control = SRC_TYPE.NONE;
                            ampH.Destination = DST_TYPE.EG1_HOLD_TIME;
                            ampH.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, ampH);
                            break;
                        case ART_TYPE.EG_AMP_DECAY:
                            var ampD = new Connection();
                            ampD.Source = SRC_TYPE.NONE;
                            ampD.Control = SRC_TYPE.NONE;
                            ampD.Destination = DST_TYPE.EG1_DECAY_TIME;
                            ampD.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, ampD);
                            break;
                        case ART_TYPE.EG_AMP_SUSTAIN:
                            var ampS = new Connection();
                            ampS.Source = SRC_TYPE.NONE;
                            ampS.Control = SRC_TYPE.NONE;
                            ampS.Destination = DST_TYPE.EG1_SUSTAIN_LEVEL;
                            ampS.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, ampS);
                            break;
                        case ART_TYPE.EG_AMP_RELEASE:
                            var ampR = new Connection();
                            ampR.Source = SRC_TYPE.NONE;
                            ampR.Control = SRC_TYPE.NONE;
                            ampR.Destination = DST_TYPE.EG1_RELEASE_TIME;
                            ampR.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, ampR);
                            break;
                        case ART_TYPE.EG_CUTOFF_ATTACK:
                            var fcA = new Connection();
                            fcA.Source = SRC_TYPE.NONE;
                            fcA.Control = SRC_TYPE.NONE;
                            fcA.Destination = DST_TYPE.EG1_ATTACK_TIME;
                            fcA.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, fcA);
                            break;
                        case ART_TYPE.EG_CUTOFF_HOLD:
                            var fcH = new Connection();
                            fcH.Source = SRC_TYPE.NONE;
                            fcH.Control = SRC_TYPE.NONE;
                            fcH.Destination = DST_TYPE.EG1_HOLD_TIME;
                            fcH.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, fcH);
                            break;
                        case ART_TYPE.EG_CUTOFF_DECAY:
                            var fcD = new Connection();
                            fcD.Source = SRC_TYPE.NONE;
                            fcD.Control = SRC_TYPE.NONE;
                            fcD.Destination = DST_TYPE.EG1_DECAY_TIME;
                            fcD.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, fcD);
                            break;
                        case ART_TYPE.EG_CUTOFF_SUSTAIN:
                            var fcS = new Connection();
                            fcS.Source = SRC_TYPE.NONE;
                            fcS.Control = SRC_TYPE.NONE;
                            fcS.Destination = DST_TYPE.EG1_SUSTAIN_LEVEL;
                            fcS.Value = srcArt.Value;
                            rgn.Articulations.ART.List.Add(rgn.Articulations.ART.List.Count, fcS);
                            break;
                        case ART_TYPE.EG_CUTOFF_RELEASE:
                            var fcR = new Connection();
                            fcR.Source = SRC_TYPE.NONE;
                            fcR.Control = SRC_TYPE.NONE;
                            fcR.Destination = DST_TYPE.EG1_RELEASE_TIME;
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
}
