using System.Collections.Generic;

namespace InstPack {
    public class Pack {
        public LWave Wave = new LWave();
        public LInst Inst = new LInst();
        public LPreset Preset = new LPreset();

        public bool DeleteWave(List<int> indices) {
            // find deletable wave
            var deleteList = new Dictionary<int, bool>();
            foreach (int selectedIndex in indices) {
                var deletable = true;
                foreach (var inst in Inst.ToArray()) {
                    foreach (var region in inst.Region.Array) {
                        foreach (var art in region.Art.ToArray()) {
                            if (art.Type != ART_TYPE.WAVE_INDEX) {
                                continue;
                            }
                            if (selectedIndex == (int)art.Value) {
                                deletable = false;
                                break;
                            }
                        }
                        if (!deletable) {
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
            var newIndex = 0;
            var renumberingList = new Dictionary<int, int>();
            for (var iWave = 0; iWave < Wave.Count; iWave++) {
                if (deleteList.ContainsKey(iWave) && deleteList[iWave]) {
                    continue;
                }
                renumberingList.Add(iWave, newIndex);
                ++newIndex;
            }

            // delete wave
            var waveList = new List<Wave>();
            for (var iWave = 0; iWave < Wave.Count; iWave++) {
                if (deleteList.ContainsKey(iWave) && deleteList[iWave]) {
                    continue;
                }
                waveList.Add(Wave[iWave]);
            }
            Wave.Clear();
            Wave.AddRange(waveList);

            // update inst's region art
            for (var iInst = 0; iInst < Inst.Count; iInst++) {
                var inst = Inst[iInst];
                for (var iRgn = 0; iRgn < inst.Region.Count; iRgn++) {
                    var rgn = inst.Region[iRgn];
                    for (var iArt = 0; iArt < rgn.Art.Count; iArt++) {
                        if (rgn.Art[iArt].Type != ART_TYPE.WAVE_INDEX) {
                            continue;
                        }
                        var iWave = (int)rgn.Art[iArt].Value;
                        if (renumberingList.ContainsKey(iWave)) {
                            rgn.Art.Update(ART_TYPE.WAVE_INDEX, renumberingList[iWave]);
                        }
                    }
                }
            }

            return true;
        }

        public bool DeleteInst(List<int> indices) {
            // find deletable inst
            var deleteList = new Dictionary<int, bool>();
            foreach (int selectedIndex in indices) {
                var deletable = true;
                foreach (var preset in Preset.Values) {
                    foreach (var layer in preset.Layer.ToArray()) {
                        if (selectedIndex == layer.Header.InstIndex) {
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

            if(0 == deleteList.Count) {
                return false;
            }

            // renumbering
            var newIndex = 0;
            var renumberingList = new Dictionary<int, int>();
            for (var iInst = 0; iInst < Inst.Count; iInst++) {
                if (deleteList.ContainsKey(iInst) && deleteList[iInst]) {
                    continue;
                }
                renumberingList.Add(iInst, newIndex);
                ++newIndex;
            }

            // delete inst
            var newInstList = new List<Inst>();
            for (var iInst = 0; iInst < Inst.Count; iInst++) {
                if (deleteList.ContainsKey(iInst) && deleteList[iInst]) {
                    continue;
                }
                newInstList.Add(Inst[iInst]);
            }
            Inst.Clear();
            Inst.AddRange(newInstList);

            // update preset's layer art
            foreach (var preset in Preset.Values) {
                for (var iLayer = 0; iLayer < preset.Layer.Count; iLayer++) {
                    var layer = preset.Layer[iLayer];
                    var iInst = layer.Header.InstIndex;
                    if (renumberingList.ContainsKey(iInst)) {
                        layer.Header.InstIndex = renumberingList[iInst];
                    }
                }
            }

            return true;
        }
    }
}
