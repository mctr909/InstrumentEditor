using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace DLS {
    public class File : Riff {
        private CK_VERS mVersion;
        private uint mMSYN = 1;

        public LINS Inst = new LINS();
        public WVPL Wave = new WVPL();

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
            bw.Write((uint)Inst.List.Count);

            mVersion.Write(bw);

            bw.Write("msyn".ToCharArray());
            bw.Write((uint)4);
            bw.Write(mMSYN);

            Inst.Write(bw);
            Wave.Write(bw);
            Info.Write(bw);

            var fs = new FileStream(filePath, FileMode.Create);
            var bw2 = new BinaryWriter(fs);
            bw2.Write("RIFF".ToCharArray());
            bw2.Write((uint)ms.Length);
            bw2.Write(ms.ToArray());

            fs.Close();
            fs.Dispose();
        }

        public File ToPack() {
            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            var file = new File();

            foreach (var wave in Wave.Array) {
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

                file.Wave.Add(waveInfo);
            }

            foreach (var dlsInst in Inst.List) {
                var inst = new INS();
                inst.Locale = dlsInst.Key;
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
                file.Inst.Add(inst);
            }

            return file;
        }

        public bool RemoveWaves(List<uint> indices) {
            var deleteList = new Dictionary<uint, bool>();
            foreach (var selectedIndex in indices) {
                var deletable = true;
                foreach (var inst in Inst.List.Values) {
                    foreach (var region in inst.Regions.Array) {
                        if (selectedIndex == region.WaveLink.TableIndex) {
                            deletable = false;
                            break;
                        }
                    }
                    if (!deletable) {
                        break;
                    }
                }
                deleteList.Add(selectedIndex, deletable);
            }
            if (0 == deleteList.Count) {
                return false;
            }

            // renumbering
            uint newIndex = 0;
            var renumberingList = new Dictionary<uint, uint>();
            for (uint iWave = 0; iWave < Wave.Count; iWave++) {
                if (deleteList.ContainsKey(iWave) && deleteList[iWave]) {
                    continue;
                }
                renumberingList.Add(iWave, newIndex);
                ++newIndex;
            }

            // delete wave
            var waveList = new List<WAVE>();
            for (uint iWave = 0; iWave < Wave.Count; iWave++) {
                if (deleteList.ContainsKey(iWave) && deleteList[iWave]) {
                    continue;
                }
                waveList.Add(Wave[(int)iWave]);
            }
            Wave.Clear();
            Wave.AddRange(waveList);

            // update inst's region art
            foreach (var inst in Inst.List.Values) {
                for (var iRgn = 0; iRgn < inst.Regions.List.Count; iRgn++) {
                    var rgn = inst.Regions[iRgn];
                    inst.Regions[iRgn].WaveLink.TableIndex
                        = renumberingList[rgn.WaveLink.TableIndex];
                }
            }

            return true;
        }

        public static void SaveFromPack(string filePath, File file) {
            var saveFile = new File();

            // WAVE
            saveFile.Wave = new WVPL();
            foreach (var wav in file.Wave.Array) {
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

                saveFile.Wave.Add(wavh);
            }

            // Inst
            saveFile.Inst = new LINS();
            saveFile.Inst.List = new SortedDictionary<MidiLocale, INS>(new LINS.Sort());
            foreach (var srcPre in file.Inst.List.Values) {
                if (1 != srcPre.Regions.List.Count) {
                    continue;
                }

                var ins = new INS();
                ins.Locale = srcPre.Locale;
                ins.Info.CopyFrom(srcPre.Info);

                ins.Articulations = new LART();
                foreach (var srcArt in srcPre.Articulations.List) {
                    ins.Articulations.Add(srcArt);
                }

                ins.Regions = new LRGN();
                ins.Regions.List = new SortedList<RGN.HEADER, RGN>(new LRGN.Sort());
                foreach(var srcRgn in srcPre.Regions.Array) {
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

                saveFile.Inst.Add(ins);
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
                Inst = new LINS(ptr, size);
                break;
            case "wvpl":
                Wave = new WVPL(ptr, size);
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
