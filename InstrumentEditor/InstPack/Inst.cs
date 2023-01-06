using System.Collections.Generic;

using DLS;

namespace InstPack {
    public class LInst {
        List<INS> mList = new List<INS>();

        public LInst() { }

        public void Clear() {
            mList.Clear();
        }

        public int Count {
            get { return mList.Count; }
        }

        public INS this[int index] {
            get { return mList[index]; }
        }

        public INS[] ToArray() {
            return mList.ToArray();
        }

        public void Add(INS inst) {
            mList.Add(inst);
        }

        public void AddRange(List<INS> instList) {
            foreach (var inst in instList) {
                mList.Add(inst);
            }
        }

        public void Remove(int index) {
            mList.RemoveAt(index);
        }
    }
}
