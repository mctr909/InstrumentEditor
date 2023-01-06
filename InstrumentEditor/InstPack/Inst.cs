using System.Collections.Generic;

using DLS;

namespace InstPack {
    public class LInst {
        private List<INS> List = new List<INS>();

        public LInst() { }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public INS this[int index] {
            get { return List[index]; }
        }

        public INS[] ToArray() {
            return List.ToArray();
        }

        public void Add(INS inst) {
            List.Add(inst);
        }

        public void AddRange(List<INS> instList) {
            foreach (var inst in instList) {
                List.Add(inst);
            }
        }

        public void Remove(int index) {
            List.RemoveAt(index);
        }
    }
}
