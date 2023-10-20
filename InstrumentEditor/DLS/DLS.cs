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

        protected override void Init(out string id, List<Chunk> chunks, List<LIST> riffs) {
            id = "";
        }

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

                waveInfo.Data = new byte[wave.Data.Length];
                Array.Copy(wave.Data, 0, waveInfo.Data, 0, wave.Data.Length);
                waveInfo.To16bit();

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

                var rgn = new Region();
                rgn.Header.KeyLo = 0;
                rgn.Header.KeyHi = 127;
                rgn.Header.VelLo = 0;
                rgn.Header.VelHi = 127;
                rgn.InstIndex = pack.Inst.Count;

                pres.Regions.Add(rgn);
                pres.Info.CopyFrom(dlsInst.Value.Info);
                pres.Info[Info.TYPE.ICRD] = now;
                pack.Preset.Add(pres.Header, pres);

                var inst = new INS();
                inst.Info.CopyFrom(dlsInst.Value.Info);
                inst.Info[Info.TYPE.ICRD] = now;

                foreach (var instArt in dlsInst.Value.Articulations.List) {
                    var art = new Connection {
                        Source = instArt.Source,
                        Control = instArt.Control,
                        Destination = instArt.Destination,
                        Value = instArt.Value
                    };
                    inst.Articulations.Add(art);
                }

                foreach (var dlsRegion in dlsInst.Value.Regions.List) {
                    var tmpRgn = new RGN();
                    tmpRgn.Header = dlsRegion.Key;
                    tmpRgn.WaveLink = dlsRegion.Value.WaveLink;
                    tmpRgn.Sampler = dlsRegion.Value.Sampler;
                    foreach (var src in dlsRegion.Value.Loops) {
                        tmpRgn.Loops.Add(new WaveLoop() {
                            Start = src.Start,
                            Length = src.Length
                        });
                    }
                    foreach (var regionArt in dlsRegion.Value.Articulations.List) {
                        var art = new Connection {
                            Source = regionArt.Source,
                            Control = regionArt.Control,
                            Destination = regionArt.Destination,
                            Value = regionArt.Value
                        };
                        tmpRgn.Articulations.Add(art);
                    }
                    inst.Regions.Add(tmpRgn);
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
                wavh.Format = new WAVE.FMT();
                wavh.Format.Tag = 1;
                wavh.Format.Bits = 16;
                wavh.Format.Channels = 1;
                wavh.Format.SampleRate = wav.Format.SampleRate;
                wavh.Format.BlockAlign = (ushort)(wavh.Format.Bits * wavh.Format.Channels / 8);
                wavh.Format.BytesPerSec = wavh.Format.SampleRate * wavh.Format.BlockAlign;

                wavh.Sampler = wav.Sampler;
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
                if (1 != srcPre.Regions.Count) {
                    continue;
                }

                var srcIns = pack.Inst[srcPre.Regions[0].InstIndex];

                var ins = new INS();
                ins.Locale = new MidiLocale();
                ins.Locale.BankFlg = (byte)(srcPre.Header.IsDrum ? 0x80 : 0x00);
                ins.Locale.BankMSB = srcPre.Header.BankMSB;
                ins.Locale.BankLSB = srcPre.Header.BankLSB;
                ins.Locale.ProgNum = srcPre.Header.ProgNum;

                ins.Info.CopyFrom(srcPre.Info);

                ins.Articulations = new LART();
                foreach (var srcArt in srcIns.Articulations.List) {
                    ins.Articulations.Add(srcArt);
                }

                ins.Regions = new LRGN();
                ins.Regions.List = new SortedList<RGN.HEADER, RGN>(new LRGN.Sort());
                foreach(var srcRgn in srcIns.Regions.Array) {
                    var rgn = new RGN();
                    rgn.Header = srcRgn.Header;
                    rgn.WaveLink = srcRgn.WaveLink;
                    rgn.Sampler = srcRgn.Sampler;

                    rgn.Articulations = new LART();
                    foreach (var srcArt in srcRgn.Articulations.List) {
                        ins.Articulations.Add(srcArt);
                    }

                    ins.Regions.List.Add(rgn.Header, rgn);
                }

                saveFile.Instruments.List.Add(ins.Locale, ins);
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
