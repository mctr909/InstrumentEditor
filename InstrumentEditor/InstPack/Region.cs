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
                if (layer.Header.Key.Lo <= lyr.Header.Key.Hi && lyr.Header.Key.Lo <= layer.Header.Key.Hi &&
                    layer.Header.Vel.Lo <= lyr.Header.Vel.Hi && lyr.Header.Vel.Lo <= layer.Header.Vel.Hi &&
                    layer.InstIndex == lyr.InstIndex) {
                    ret.Add(lyr);
                }
            }
            return ret;
        }

        public List<Region> Find(int noteNo, int velocity) {
            var ret = new List<Region>();
            foreach (var layer in mList) {
                if (noteNo <= layer.Header.Key.Hi && layer.Header.Key.Lo <= noteNo &&
                    velocity <= layer.Header.Vel.Hi && layer.Header.Vel.Lo <= velocity) {
                    ret.Add(layer);
                }
            }
            return ret;
        }

        public bool ContainsKey(Region layer) {
            foreach (var lyr in mList) {
                if (layer.Header.Key.Lo <= lyr.Header.Key.Hi && lyr.Header.Key.Lo <= layer.Header.Key.Hi &&
                    layer.Header.Vel.Lo <= lyr.Header.Vel.Hi && lyr.Header.Vel.Lo <= layer.Header.Vel.Hi &&
                    layer.InstIndex == lyr.InstIndex) {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(int noteNo, int velocity) {
            foreach (var layer in mList) {
                if (noteNo <= layer.Header.Key.Hi && layer.Header.Key.Lo <= noteNo &&
                    velocity <= layer.Header.Vel.Hi && layer.Header.Vel.Lo <= velocity) {
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
        public DLS.CK_RGNH Header;
        public int InstIndex;
        public DLS.LART Articulations = new DLS.LART();

        public Region() { }
    }
}
