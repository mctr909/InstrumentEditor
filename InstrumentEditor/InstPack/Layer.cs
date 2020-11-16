using System.Collections.Generic;

using Riff;

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

        public void Update(int index, LYRH header) {
            List[index].Header = header;
        }

        public List<Layer> Find(LYRH header) {
            var ret = new List<Layer>();
            foreach (var layer in List) {
                if (header.KeyLo <= layer.Header.KeyHi && layer.Header.KeyLo <= header.KeyHi &&
                    header.VelLo <= layer.Header.VelHi && layer.Header.VelLo <= header.VelHi &&
                    header.InstIndex == layer.Header.InstIndex) {
                    ret.Add(layer);
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

        public bool ContainsKey(LYRH header) {
            foreach (var layer in List) {
                if (header.KeyLo <= layer.Header.KeyHi && layer.Header.KeyLo <= header.KeyHi &&
                    header.VelLo <= layer.Header.VelHi && layer.Header.VelLo <= header.VelHi &&
                    header.InstIndex == layer.Header.InstIndex) {
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

    public class Layer : Chunk {
        public LYRH Header;
        public Lart Art = new Lart();

        public Layer() { }
    }
}
