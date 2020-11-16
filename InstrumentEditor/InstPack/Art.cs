using System.Collections.Generic;

namespace InstPack {
    public class Lart {
        private List<ART> List = new List<ART>();

        public Lart() { }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public ART this[int index] {
            get { return List[index]; }
        }

        public ART[] ToArray() {
            return List.ToArray();
        }

        public void Add(ART art) {
            List.Add(art);
        }

        public void Update(ART_TYPE id, float value) {
            var idx = -1;
            for (var i = 0; i < List.Count; i++) {
                if (List[i].Type == id) {
                    idx = i;
                    break;
                }
            }
            if (idx < 0) {
                List.Add(new ART {
                    Type = id,
                    Value = value
                });
            } else {
                List[idx] = new ART {
                    Type = id,
                    Value = value
                };
            }
        }

        public void Delete(ART_TYPE id) {
            int idx = -1;
            for (var i = 0; i < List.Count; i++) {
                if (List[i].Type == id) {
                    idx = i;
                    break;
                }
            }
            if (0 <= idx) {
                List.RemoveAt(idx);
            }
        }
    }
}
