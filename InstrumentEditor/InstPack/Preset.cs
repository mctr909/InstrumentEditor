using System.Collections.Generic;

namespace InstPack {
    public class LPreset {
        private Dictionary<PREH, Preset> List = new Dictionary<PREH, Preset>();

        public LPreset() { }

        public int Count {
            get { return List.Count; }
        }

        public Preset this[PREH id] {
            get { return List[id]; }
        }

        public Dictionary<PREH, Preset>.KeyCollection Keys {
            get { return List.Keys; }
        }

        public Dictionary<PREH, Preset>.ValueCollection Values {
            get { return List.Values; }
        }

        public bool ContainsKey(PREH id) {
            return List.ContainsKey(id);
        }

        public void Add(PREH id, Preset preset) {
            List.Add(id, preset);
        }

        public void Remove(PREH id) {
            List.Remove(id);
        }
    }

    public class Preset {
        public PREH Header;
        public Lart Art = new Lart();
        public LLayer Layer = new LLayer();
        public Info Info = new Info();

        public Preset() { }
    }
}
