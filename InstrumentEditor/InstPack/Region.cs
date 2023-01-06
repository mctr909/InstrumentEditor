using System.Collections.Generic;

using DLS;

namespace InstPack {
    public class LRegion {
        public sealed class Sort : IComparer<CK_RGNH> {
            // IComparerの実装
            public int Compare(CK_RGNH x, CK_RGNH y) {
                var keyH = x.Key.Hi < y.Key.Lo;
                var keyL = y.Key.Hi < x.Key.Lo;
                var velH = x.Vel.Hi < y.Vel.Lo;
                var velL = y.Vel.Hi < x.Vel.Lo;
                var key = keyH || keyL;
                var vel = velH || velL;
                if (key || vel) {
                    if (keyH) {
                        return -1;
                    }
                    if (velH) {
                        return -1;
                    }
                    return 1;
                } else {
                    return 0;
                }
            }
        }

        private SortedList<CK_RGNH, RGN> List = new SortedList<CK_RGNH, RGN>(new Sort());

        public LRegion() { }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public IList<RGN> Array {
            get { return List.Values; }
        }

        public RGN this[int index] {
            get { return List.Values[index]; }
        }

        public bool Add(RGN region) {
            if (List.ContainsKey(region.Header)) {
                return false;
            }
            List.Add(region.Header, region);
            return true;
        }

        public List<RGN> Find(CK_RGNH header) {
            var ret = new List<RGN>();
            foreach (var rng in List.Values) {
                if (header.Key.Lo <= rng.Header.Key.Hi && rng.Header.Key.Lo <= header.Key.Hi &&
                    header.Vel.Lo <= rng.Header.Vel.Hi && rng.Header.Vel.Lo <= header.Vel.Hi) {
                    ret.Add(rng);
                }
            }
            return ret;
        }

        public RGN Find(int noteNo, int velocity) {
            foreach (var rng in List.Values) {
                if (noteNo <= rng.Header.Key.Hi && rng.Header.Key.Lo <= noteNo &&
                    velocity <= rng.Header.Vel.Hi && rng.Header.Vel.Lo <= velocity) {
                    return rng;
                }
            }
            return null;
        }

        public RGN FindFirst(CK_RGNH header) {
            foreach (var rng in List.Values) {
                if (header.Key.Lo <= rng.Header.Key.Hi && rng.Header.Key.Lo <= header.Key.Hi &&
                    header.Vel.Lo <= rng.Header.Vel.Hi && rng.Header.Vel.Lo <= header.Vel.Hi) {
                    return rng;
                }
            }
            return null;
        }

        public bool ContainsKey(CK_RGNH header) {
            foreach (var rng in List.Values) {
                if (header.Key.Lo <= rng.Header.Key.Hi && rng.Header.Key.Lo <= header.Key.Hi &&
                    header.Vel.Lo <= rng.Header.Vel.Hi && rng.Header.Vel.Lo <= header.Vel.Hi) {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(int noteNo, int velocity) {
            foreach (var rng in List.Values) {
                if (noteNo <= rng.Header.Key.Hi && rng.Header.Key.Lo <= noteNo &&
                    velocity <= rng.Header.Vel.Hi && rng.Header.Vel.Lo <= velocity) {
                    return true;
                }
            }
            return false;
        }

        public void Remove(CK_RGNH header) {
            var tmpRegion = new List<RGN>();
            foreach (var rng in List.Values) {
                if (header.Key.Lo <= rng.Header.Key.Hi && rng.Header.Key.Lo <= header.Key.Hi &&
                    header.Vel.Lo <= rng.Header.Vel.Hi && rng.Header.Vel.Lo <= header.Vel.Hi) {
                } else {
                    tmpRegion.Add(rng);
                }
            }
            List.Clear();
            foreach (var rgn in tmpRegion) {
                List.Add(rgn.Header, rgn);
            }
        }
    }
}
