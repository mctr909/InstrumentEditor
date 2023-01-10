using System.Collections.Generic;

namespace InstPack {
    public struct PREH {
        public bool IsDrum;
        public byte BankMSB;
        public byte BankLSB;
        public byte ProgNum;
    }

    public class LPreset {
        Dictionary<PREH, Preset> mList = new Dictionary<PREH, Preset>();

        public LPreset() { }

        public int Count {
            get { return mList.Count; }
        }

        public Preset this[PREH id] {
            get { return mList[id]; }
        }

        public Dictionary<PREH, Preset>.KeyCollection Keys {
            get { return mList.Keys; }
        }

        public Dictionary<PREH, Preset>.ValueCollection Values {
            get { return mList.Values; }
        }

        public bool ContainsKey(PREH id) {
            return mList.ContainsKey(id);
        }

        public void Add(PREH id, Preset preset) {
            mList.Add(id, preset);
        }

        public void Remove(PREH id) {
            mList.Remove(id);
        }
    }

    public class Preset {
        public PREH Header;
        public DLS.LART Articulations = new DLS.LART();
        public LRegion Regions = new LRegion();
        public Info Info = new Info();

        public Preset() { }
    }
}
