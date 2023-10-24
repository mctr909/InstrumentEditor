using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLS {
    public class File : Riff {
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct VERS {
            public uint MSB;
            public uint LSB;
        }

        VERS mVersion;

        public LINS Inst = new LINS();
        public WVPL Wave = new WVPL();

        protected override void Init(out string id, List<Chunk> chunks, List<LIST> riffs) {
            id = "DLS ";
            chunks.Add(new Chunk("colh", (i) => {
                i.Write((uint)Inst.List.Count);
            }, (i) => {
            }));
            chunks.Add(new Chunk("vers", (i) => {
                i.Write(mVersion);
            }, (i) => {
                i.Read(ref mVersion);
            }));
            chunks.Add(new Chunk("msyn", (i) => {
                i.Write((uint)1);
            }, (i) => {
            }));
            riffs.Add(new LIST("lins", (i) => {
                Inst.Write(i);
            }, (ptr, size) => {
                Inst = new LINS(ptr, size);
            }));
            riffs.Add(new LIST("wvpl", (i) => {
                Wave.Write(i);
            }, (ptr, size) => {
                Wave = new WVPL(ptr, size);
            }));
        }

        public File() { }

        public File(string filePath) {
            MainLoop(filePath);
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

        protected override void LoadInfo(IntPtr ptr, string type, string value) {
            Info[type] = value;
        }
    }
}
