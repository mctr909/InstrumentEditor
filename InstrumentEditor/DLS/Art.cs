using System;
using System.Collections.Generic;

namespace DLS {
	public class LART : Riff {
		public List<Connection> List = new List<Connection>();

		protected override void Initialize(out string id, List<Chunk> chunks, List<LIST> riffs) {
			id = "lart";
			chunks.Add(new Chunk("art1", (i) => {
				if (0 == List.Count) {
					return;
				}
				i.Write(8);
				i.Write(List.Count);
				i.Write(List);
			}, (i) => {
				i.Seek(8);
				i.Read(List);
			}));
		}

		public LART() { }

		public LART(IntPtr ptr, long size) : base(ptr, size) { }

		public void Add(Connection conn) {
			if (List.Contains(conn)) {
				List.Remove(conn);
			}
			List.Add(conn);
		}

		public void AddRange(IEnumerable<Connection> list) {
			List.AddRange(list);
		}

		public void Clear() {
			List.Clear();
		}

		public void Delete(DST_TYPE type) {
			var tmp = new List<Connection>();
			for (int i = 0; i < List.Count; i++) {
				var art = List[i];
				if (type == art.Destination) {
					continue;
				}
				tmp.Add(art);
			}
			List = tmp;
		}

		public void Update(DST_TYPE type, double value) {
			for (int i = 0; i < List.Count; i++) {
				var art = List[i];
				if (type == art.Destination) {
					art.Value = value;
					List[i] = art;
					return;
				}
			}
			List.Add(new Connection {
				Destination = type,
				Value = value
			});
		}
	}
}
