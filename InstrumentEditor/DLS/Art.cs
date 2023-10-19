using System;
using System.Collections.Generic;
using System.IO;

namespace DLS {
	public class LART : Riff {
		public List<Connection> List = new List<Connection>();

		Chunk cArt;

		void set() {
			cArt = new Chunk("art1", (i) => {
				i.Write(8);
				i.Write(List.Count);
				i.Write(List);
			}, (i) => {
				i.Seek(8);
				i.Read(List);
			});
		}

		public LART() {
			set();
		}

		public LART(IntPtr ptr, long size) {
			set();
			Load(ptr, size);
		}

		public override void Write(BinaryWriter bw) {
			if (0 == List.Count) {
				return;
			}
			var msArt = new MemoryStream();
			var bwArt = new BinaryWriter(msArt);
			bwArt.Write("LIST".ToCharArray());
			bwArt.Write(0xFFFFFFFF);
			bwArt.Write("lart".ToCharArray());

			cArt.Save(bwArt);

			bwArt.Seek(4, SeekOrigin.Begin);
			bwArt.Write((uint)msArt.Length - 8);
			bw.Write(msArt.ToArray());
		}

		public void Add(Connection conn) {
			Delete(conn.Destination);
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

		protected override void LoadChunk(IntPtr ptr, string type, long size) {
			switch (type) {
			case "art1":
			case "art2":
				cArt.Load(ptr, size);
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkType [{0}]", type));
			}
		}
	}
}
