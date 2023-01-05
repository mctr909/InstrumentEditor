using System.Collections.Generic;

namespace InstPack {
    public class LLayer {
        private List<Layer> List = new List<Layer>();

        public LLayer() { }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public Layer this[int index] {
            get { return List[index]; }
        }

        public Layer[] ToArray() {
            return List.ToArray();
        }

        public void Add(Layer layer) {
            List.Add(layer);
        }

        public void Update(int index, Layer layer) {
            List[index].Header = layer.Header;
            List[index].InstIndex = layer.InstIndex;
        }

        public List<Layer> Find(Layer layer) {
            var ret = new List<Layer>();
            foreach (var lyr in List) {
                if (layer.Header.KeyLo <= lyr.Header.KeyHi && lyr.Header.KeyLo <= layer.Header.KeyHi &&
                    layer.Header.VelLo <= lyr.Header.VelHi && lyr.Header.VelLo <= layer.Header.VelHi &&
                    layer.InstIndex == lyr.InstIndex) {
                    ret.Add(lyr);
                }
            }
            return ret;
        }

        public List<Layer> Find(int noteNo, int velocity) {
            var ret = new List<Layer>();
            foreach (var layer in List) {
                if (noteNo <= layer.Header.KeyHi && layer.Header.KeyLo <= noteNo &&
                    velocity <= layer.Header.VelHi && layer.Header.VelLo <= velocity) {
                    ret.Add(layer);
                }
            }
            return ret;
        }

        public bool ContainsKey(Layer layer) {
            foreach (var lyr in List) {
                if (layer.Header.KeyLo <= lyr.Header.KeyHi && lyr.Header.KeyLo <= layer.Header.KeyHi &&
                    layer.Header.VelLo <= lyr.Header.VelHi && lyr.Header.VelLo <= layer.Header.VelHi &&
                    layer.InstIndex == lyr.InstIndex) {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(int noteNo, int velocity) {
            foreach (var layer in List) {
                if (noteNo <= layer.Header.KeyHi && layer.Header.KeyLo <= noteNo &&
                    velocity <= layer.Header.VelHi && layer.Header.VelLo <= velocity) {
                    return true;
                }
            }
            return false;
        }

        public void Remove(int index) {
            var tmp = new List<Layer>();
            for (var iLayer = 0; iLayer < List.Count; iLayer++) {
                if (iLayer != index) {
                    tmp.Add(List[iLayer]);
                }
            }
            List.Clear();
            List.AddRange(tmp);
        }
    }

    public class Layer : Riff {
        public RGNH Header;
        public int InstIndex;
        public List<DLS.Connection> Art = new List<DLS.Connection>();

        public Layer() { }
    }
}
