using System.Collections.Generic;

using DLS;

namespace InstPack {
    public class Pack {
        public WVPL Wave = new WVPL();
        public LINS Inst = new LINS();
        public LINS Preset = new LINS();

        public bool DeleteWave(List<uint> indices) {
            // find deletable wave
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
            for (uint iWave = 0; iWave < Wave.List.Count; iWave++) {
                if (deleteList.ContainsKey(iWave) && deleteList[iWave]) {
                    continue;
                }
                renumberingList.Add(iWave, newIndex);
                ++newIndex;
            }

            // delete wave
            var waveList = new List<WAVE>();
            for (uint iWave = 0; iWave < Wave.List.Count; iWave++) {
                if (deleteList.ContainsKey(iWave) && deleteList[iWave]) {
                    continue;
                }
                waveList.Add(Wave.List[(int)iWave]);
            }
            Wave.List.Clear();
            Wave.List.AddRange(waveList);

            // update inst's region art
            foreach(var inst in Inst.List.Values) {
                for (var iRgn = 0; iRgn < inst.Regions.List.Count; iRgn++) {
                    var rgn = inst.Regions[iRgn];
                    inst.Regions[iRgn].WaveLink.TableIndex
                        = renumberingList[rgn.WaveLink.TableIndex];
                }
            }

            return true;
        }

        public bool DeleteInst(List<int> indices) {
            // find deletable inst
            var deleteList = new Dictionary<int, bool>();
            foreach (int selectedIndex in indices) {
                var deletable = true;
                deleteList.Add(selectedIndex, deletable);
            }

            if(0 == deleteList.Count) {
                return false;
            }

            // renumbering
            var newIndex = 0;
            var renumberingList = new Dictionary<int, int>();
            for (var iInst = 0; iInst < Inst.List.Count; iInst++) {
                if (deleteList.ContainsKey(iInst) && deleteList[iInst]) {
                    continue;
                }
                renumberingList.Add(iInst, newIndex);
                ++newIndex;
            }
            ///TODO:RGN
            //// delete inst
            //var newInstList = new Dictionary<MidiLocale, INS>();
            //for (var iInst = 0; iInst < Inst.List.Count; iInst++) {
            //    if (deleteList.ContainsKey(iInst) && deleteList[iInst]) {
            //        continue;
            //    }
            //    newInstList.Add(Inst.List[iInst]);
            //}
            //Inst.List.Clear();
            //Inst.List.AddRange(newInstList);
            //// update preset's layer art
            //foreach (var preset in Preset.Values) {
            //    for (var iLayer = 0; iLayer < preset.Regions.Count; iLayer++) {
            //        var layer = preset.Regions[iLayer];
            //        var iInst = layer.InstIndex;
            //        if (renumberingList.ContainsKey(iInst)) {
            //            layer.InstIndex = renumberingList[iInst];
            //        }
            //    }
            //}

            return true;
        }
    }
}
