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
                var waveInfo = new WAVE();
                waveInfo.Format = wave.Format;
                waveInfo.Sampler = wave.Sampler;
                foreach (var src in wave.Loops) {
                    waveInfo.Loops.Add(new WaveLoop() {
                        Start = src.Start,
                        Length = src.Length
                    });
                }

                var ptr = Marshal.AllocHGlobal(wave.Data.Length);
                Marshal.Copy(wave.Data, 0, ptr, wave.Data.Length);
                waveInfo.Data = new byte[wave.Data.Length];
                Marshal.Copy(ptr, waveInfo.Data, 0, waveInfo.Data.Length);
                Marshal.FreeHGlobal(ptr);

                waveInfo.Info.CopyFrom(wave.Info);
                waveInfo.Info[Info.TYPE.ICRD] = now;

                pack.Wave.List.Add(waveInfo);
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
                        var art = new Connection {
                            Source = instArt.Source,
                            Control = instArt.Control,
                            Destination = instArt.Destination,
                            Value = instArt.Value
                        };
                        inst.Art.Add(art);
                    }
                }

                foreach (var dlsRegion in dlsInst.Value.Regions.List) {
                    var rgn = new Region();
                    rgn.Header.KeyLo = (byte)dlsRegion.Key.Key.Lo;
                    rgn.Header.KeyHi = (byte)dlsRegion.Key.Key.Hi;
                    rgn.Header.VelLo = (byte)dlsRegion.Key.Vel.Lo;
                    rgn.Header.VelHi = (byte)dlsRegion.Key.Vel.Hi;

                    rgn.WaveIndex = dlsRegion.Value.WaveLink.TableIndex;
                    rgn.UnityNote = dlsRegion.Value.Sampler.UnityNote;
                    rgn.FineTune = Math.Pow(2.0, dlsRegion.Value.Sampler.FineTune / 1200.0);
                    rgn.Gain = dlsRegion.Value.Sampler.Gain;

                    if (null != dlsRegion.Value.Articulations && null != dlsRegion.Value.Articulations.ART) {
                        foreach (var regionArt in dlsRegion.Value.Articulations.ART.List) {
                            var art = new Connection {
                                Source = regionArt.Source,
                                Control = regionArt.Control,
                                Destination = regionArt.Destination,
                                Value = regionArt.Value
                            };
                            rgn.Art.Add(art);
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
            foreach (var wav in pack.Wave.List) {
                var wavh = new WAVE();
                wavh.Format = new CK_FMT();
                wavh.Format.Tag = 1;
                wavh.Format.Bits = 16;
                wavh.Format.Channels = 1;
                wavh.Format.SampleRate = wav.Format.SampleRate;
                wavh.Format.BlockAlign = (ushort)(wavh.Format.Bits * wavh.Format.Channels / 8);
                wavh.Format.BytesPerSec = wavh.Format.SampleRate * wavh.Format.BlockAlign;

                wavh.Sampler = new CK_WSMP();
                wavh.Sampler.Gain = wav.Sampler.Gain;
                wavh.Sampler.FineTune = wav.Sampler.FineTune;
                wavh.Sampler.UnityNote = wav.Sampler.UnityNote;
                if (0 < wav.Loops.Count) {
                    wavh.Sampler.LoopCount = 1;
                    var loop = new WaveLoop();
                    loop.Type = 1;
                    loop.Start = wav.Loops[0].Start;
                    loop.Length = wav.Loops[0].Length;
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
                    ins.Articulations.ART.List.Add(srcArt);
                }

                ins.Regions = new LRGN();
                ins.Regions.List = new SortedDictionary<CK_RGNH, RGN>(new LRGN.Sort());
                foreach(var srcRgn in srcIns.Region.Array) {
                    var rgn = new RGN();
                    rgn.Header.Key.Hi = srcRgn.Header.KeyHi;
                    rgn.Header.Key.Lo = srcRgn.Header.KeyLo;
                    rgn.Header.Vel.Hi = srcRgn.Header.VelHi;
                    rgn.Header.Vel.Lo = srcRgn.Header.VelLo;

                    rgn.WaveLink.TableIndex = srcRgn.WaveIndex;

                    rgn.Sampler.Gain = srcRgn.Gain;
                    rgn.Sampler.UnityNote = srcRgn.UnityNote;
                    rgn.Sampler.FineTune = (short)(Math.Log(srcRgn.FineTune, 2.0) * 1200);

                    rgn.Articulations = new LART();
                    rgn.Articulations.ART = new ART();
                    rgn.Articulations.ART.List = new List<Connection>();
                    foreach (var srcArt in srcRgn.Art.ToArray()) {
                        ins.Articulations.ART.List.Add(srcArt);
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
}
