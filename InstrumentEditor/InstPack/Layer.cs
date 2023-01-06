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
                if (layer.Header.Key.Lo <= lyr.Header.Key.Hi && lyr.Header.Key.Lo <= layer.Header.Key.Hi &&
                    layer.Header.Vel.Lo <= lyr.Header.Vel.Hi && lyr.Header.Vel.Lo <= layer.Header.Vel.Hi &&
                    layer.InstIndex == lyr.InstIndex) {
                    ret.Add(lyr);
                }
            }
            return ret;
        }

        public List<Layer> Find(int noteNo, int velocity) {
            var ret = new List<Layer>();
            foreach (var layer in List) {
                if (noteNo <= layer.Header.Key.Hi && layer.Header.Key.Lo <= noteNo &&
                    velocity <= layer.Header.Vel.Hi && layer.Header.Vel.Lo <= velocity) {
                    ret.Add(layer);
                }
            }
            return ret;
        }

        public bool ContainsKey(Layer layer) {
            foreach (var lyr in List) {
                if (layer.Header.Key.Lo <= lyr.Header.Key.Hi && lyr.Header.Key.Lo <= layer.Header.Key.Hi &&
                    layer.Header.Vel.Lo <= lyr.Header.Vel.Hi && lyr.Header.Vel.Lo <= layer.Header.Vel.Hi &&
                    layer.InstIndex == lyr.InstIndex) {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(int noteNo, int velocity) {
            foreach (var layer in List) {
                if (noteNo <= layer.Header.Key.Hi && layer.Header.Key.Lo <= noteNo &&
                    velocity <= layer.Header.Vel.Hi && layer.Header.Vel.Lo <= velocity) {
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
        public DLS.CK_RGNH Header;
        public int InstIndex;
        public DLS.LART Articulations = new DLS.LART();

        public Layer() { }
    }
}
