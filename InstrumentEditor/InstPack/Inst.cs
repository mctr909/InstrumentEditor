using System.Collections.Generic;

namespace InstPack {
    public class LInst {
        private List<Inst> List = new List<Inst>();

        public LInst() { }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public Inst this[int index] {
            get { return List[index]; }
        }

        public Inst[] ToArray() {
            return List.ToArray();
        }

        public void Add(Inst inst) {
            List.Add(inst);
        }

        public void AddRange(List<Inst> instList) {
            foreach (var inst in instList) {
                List.Add(inst);
            }
        }

        public void Remove(int index) {
            List.RemoveAt(index);
        }
    }

    public class Inst {
        public Lart Art = new Lart();
        public LRegion Region = new LRegion();
        public Info Info = new Info();

        public Inst() { }
    }
}
