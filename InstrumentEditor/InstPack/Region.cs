using System.Collections.Generic;

namespace InstPack {
    public class LRegion {
        public sealed class Sort : IComparer<RGNH> {
            // IComparerの実装
            public int Compare(RGNH x, RGNH y) {
                var keyH = x.KeyHi < y.KeyLo;
                var keyL = y.KeyHi < x.KeyLo;
                var velH = x.VelHi < y.VelLo;
                var velL = y.VelHi < x.VelLo;
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

        private SortedList<RGNH, Region> List = new SortedList<RGNH, Region>(new Sort());

        public LRegion() { }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public IList<Region> Array {
            get { return List.Values; }
        }

        public Region this[int index] {
            get { return List.Values[index]; }
        }

        public bool Add(Region region) {
            if (List.ContainsKey(region.Header)) {
                return false;
            }
            List.Add(region.Header, region);
            return true;
        }

        public List<Region> Find(RGNH header) {
            var ret = new List<Region>();
            foreach (var rng in List.Values) {
                if (header.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= header.KeyHi &&
                    header.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= header.VelHi) {
                    ret.Add(rng);
                }
            }
            return ret;
        }

        public Region Find(int noteNo, int velocity) {
            foreach (var rng in List.Values) {
                if (noteNo <= rng.Header.KeyHi && rng.Header.KeyLo <= noteNo &&
                    velocity <= rng.Header.VelHi && rng.Header.VelLo <= velocity) {
                    return rng;
                }
            }
            return null;
        }

        public Region FindFirst(RGNH header) {
            foreach (var rng in List.Values) {
                if (header.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= header.KeyHi &&
                    header.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= header.VelHi) {
                    return rng;
                }
            }
            return null;
        }

        public bool ContainsKey(RGNH header) {
            foreach (var rng in List.Values) {
                if (header.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= header.KeyHi &&
                    header.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= header.VelHi) {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(int noteNo, int velocity) {
            foreach (var rng in List.Values) {
                if (noteNo <= rng.Header.KeyHi && rng.Header.KeyLo <= noteNo &&
                    velocity <= rng.Header.VelHi && rng.Header.VelLo <= velocity) {
                    return true;
                }
            }
            return false;
        }

        public void Remove(RGNH header) {
            var tmpRegion = new List<Region>();
            foreach (var rng in List.Values) {
                if (header.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= header.KeyHi &&
                    header.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= header.VelHi) {
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

    public class Region {
        public RGNH Header;
        public uint WaveIndex;
        public Lart Art = new Lart();

        public Region() { }
    }
}
