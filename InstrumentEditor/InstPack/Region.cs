using System.Collections.Generic;

namespace InstPack {
    public class LRegion {
        List<Region> mList = new List<Region>();

        public LRegion() { }

        public void Clear() {
            mList.Clear();
        }

        public int Count {
            get { return mList.Count; }
        }

        public Region this[int index] {
            get { return mList[index]; }
        }

        public Region[] ToArray() {
            return mList.ToArray();
        }

        public void Add(Region layer) {
            mList.Add(layer);
        }

        public void Update(int index, Region layer) {
            mList[index].Header = layer.Header;
            mList[index].InstIndex = layer.InstIndex;
        }

        public List<Region> Find(Region layer) {
            var ret = new List<Region>();
            foreach (var lyr in mList) {
                if (layer.Header.KeyLo <= lyr.Header.KeyHi && lyr.Header.KeyLo <= layer.Header.KeyHi &&
                    layer.Header.VelLo <= lyr.Header.VelHi && lyr.Header.VelLo <= layer.Header.VelHi &&
                    layer.InstIndex == lyr.InstIndex) {
                    ret.Add(lyr);
                }
            }
            return ret;
        }

        public List<Region> Find(int noteNo, int velocity) {
            var ret = new List<Region>();
            foreach (var layer in mList) {
                if (noteNo <= layer.Header.KeyHi && layer.Header.KeyLo <= noteNo &&
                    velocity <= layer.Header.VelHi && layer.Header.VelLo <= velocity) {
                    ret.Add(layer);
                }
            }
            return ret;
        }

        public bool ContainsKey(Region layer) {
            foreach (var lyr in mList) {
                if (layer.Header.KeyLo <= lyr.Header.KeyHi && lyr.Header.KeyLo <= layer.Header.KeyHi &&
                    layer.Header.VelLo <= lyr.Header.VelHi && lyr.Header.VelLo <= layer.Header.VelHi &&
                    layer.InstIndex == lyr.InstIndex) {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(int noteNo, int velocity) {
            foreach (var layer in mList) {
                if (noteNo <= layer.Header.KeyHi && layer.Header.KeyLo <= noteNo &&
                    velocity <= layer.Header.VelHi && layer.Header.VelLo <= velocity) {
                    return true;
                }
            }
            return false;
        }

        public void Remove(int index) {
            var tmp = new List<Region>();
            for (var iLayer = 0; iLayer < mList.Count; iLayer++) {
                if (iLayer != index) {
                    tmp.Add(mList[iLayer]);
                }
            }
            mList.Clear();
            mList.AddRange(tmp);
        }
    }

    public class Region : Riff {
        public DLS.RGN.HEADER Header = new DLS.RGN.HEADER();
        public int InstIndex;
        public DLS.LART Articulations = new DLS.LART();

        protected override void Init(out string id, List<Chunk> chunks, List<LIST> riffs) {
            id = "";
        }

        public Region() { }
    }
}
