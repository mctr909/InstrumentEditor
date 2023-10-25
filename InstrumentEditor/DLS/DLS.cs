using System.Collections.Generic;

namespace DLS {
	public class File : Riff {
		public uint VersionMSB;
		public uint VersionLSB;
		public LINS Inst = new LINS();
		public WVPL Wave = new WVPL();

		protected override void Initialize(out string id, List<Chunk> chunks, List<LIST> riffs) {
			id = "DLS ";
			chunks.Add(new Chunk("colh", (i) => {
				i.Write((uint)Inst.List.Count);
			}, (i) => {
			}));
			chunks.Add(new Chunk("vers", (i) => {
				i.Write(VersionMSB);
				i.Write(VersionLSB);
			}, (i) => {
				i.Read(ref VersionMSB);
				i.Read(ref VersionLSB);
			}));
			chunks.Add(new Chunk("msyn", (i) => {
				i.Write((uint)1);
			}, (i) => {
			}));
			chunks.Add(new Chunk("ptbl", (i) => {
				var cues = Wave.GetCues();
				i.Write((uint)8);
				i.Write(cues.Count);
				i.Write(cues);
			}, (i) => {
			}));
			riffs.Add(new LIST("lins", (i) => {
				Inst.Write(i);
			}, (ptr, size) => {
				Inst = new LINS(ptr, size);
			}));
			riffs.Add(new LIST("wvpl", (i) => {
				Wave.Write(i);
			}, (ptr, size) => {
				Wave = new WVPL(ptr, size);
			}));
		}

		public File() { }

		public File(string filePath) : base(filePath) { }

		public bool RemoveWaves(List<uint> indices) {
			// 削除対象波形がどこからも参照されていないことを確認
			var deleteList = new List<uint>();
			foreach (var index in indices) {
				var deletable = true;
				foreach (var inst in Inst.List.Values) {
					foreach (var region in inst.Regions.Array) {
						if (index == region.WaveLink.TableIndex) {
							deletable = false;
							break;
						}
					}
					if (!deletable) {
						break;
					}
				}
				if (deletable) {
					deleteList.Add(index);
				}
			}
			if (0 == deleteList.Count) {
				return false;
			}
			// 波形インデックス振り直し
			uint newIndex = 0;
			var renumberingList = new Dictionary<uint, uint>();
			for (uint oldIndex = 0; oldIndex < Wave.Count; oldIndex++) {
				if (deleteList.Contains(oldIndex)) {
					continue;
				}
				renumberingList.Add(oldIndex, newIndex);
				newIndex++;
			}
			foreach (var inst in Inst.List.Values) {
				for (var iRgn = 0; iRgn < inst.Regions.List.Count; iRgn++) {
					var rgn = inst.Regions[iRgn];
					inst.Regions[iRgn].WaveLink.TableIndex
						= renumberingList[rgn.WaveLink.TableIndex];
				}
			}
			// 波形削除
			var newWaveList = new List<WAVE>();
			for (uint iWave = 0; iWave < Wave.Count; iWave++) {
				if (deleteList.Contains(iWave)) {
					continue;
				}
				newWaveList.Add(Wave[(int)iWave]);
			}
			Wave.Clear();
			Wave.AddRange(newWaveList);
			return true;
		}
	}
}
