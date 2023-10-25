using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public abstract class Riff {
	public static readonly Encoding Enc = Encoding.GetEncoding("shift-jis");

	public string Id { get; private set; }
	public Info Info = new Info();

	Dictionary<string, Chunk> mChunks = new Dictionary<string, Chunk>();
	Dictionary<string, LIST> mLists = new Dictionary<string, LIST>();

	protected class Chunk {
		public class Reader {
			BinaryReader mBR = null;
			public long Pos { get; private set; } = 0;
			public long Size { get; private set; } = 0;
			public Reader(BinaryReader br, long size) {
				mBR = br;
				Size = size;
			}
			public void Seek(long seek) {
				if (Pos + seek <= Size && mBR != null) {
					mBR.BaseStream.Position += seek;
					Pos += seek;
				}
			}
			public void Read(out byte[] data) {
				if (Pos < Size && mBR != null) {
					data = mBR.ReadBytes((int)Size);
					Pos += Size;
				} else {
					data = default;
				}
			}
			public void Read<T>(ref T value) {
				var len = Marshal.SizeOf<T>();
				if (Pos < Size && mBR != null) {
					var arr = mBR.ReadBytes(len);
					var ptr = Marshal.AllocHGlobal(len);
					Marshal.Copy(arr, 0, ptr, len);
					value = Marshal.PtrToStructure<T>(ptr);
					Marshal.FreeHGlobal(ptr);
					Pos += len;
				} else {
					value = default;
				}
			}
			public void Read<T>(List<T> list) {
				var len = Marshal.SizeOf<T>();
				var ptr = Marshal.AllocHGlobal(len);
				while (Pos < Size && mBR != null) {
					var arr = mBR.ReadBytes(len);
					Marshal.Copy(arr, 0, ptr, len);
					list.Add(Marshal.PtrToStructure<T>(ptr));
					Pos += len;
				}
				Marshal.FreeHGlobal(ptr);
			}
		}

		public class Writer {
			BinaryWriter mBW = null;
			public long Size { get; private set; } = 0;
			public long Pos { get; private set; } = 0;
			public Writer() { }
			public Writer(BinaryWriter bw, long size) {
				mBW = bw;
				Size = size;
			}
			public void Write(byte[] data) {
				var len = data.Length;
				if (Pos + len <= Size && mBW != null) {
					mBW.Write(data);
				}
				Pos += len;
			}
			public void Write<T>(T value) {
				var len = Marshal.SizeOf<T>();
				if (Pos + len <= Size && mBW != null) {
					var ptr = Marshal.AllocHGlobal(len);
					Marshal.StructureToPtr(value, ptr, false);
					var arr = new byte[len];
					Marshal.Copy(ptr, arr, 0, len);
					Marshal.FreeHGlobal(ptr);
					mBW.Write(arr);
				}
				Pos += len;
			}
			public void Write<T>(List<T> list) {
				var len = Marshal.SizeOf<T>();
				var ptr = Marshal.AllocHGlobal(len);
				var arr = new byte[len];
				foreach (var item in list) {
					if (Pos + len <= Size && mBW != null) {
						Marshal.StructureToPtr(item, ptr, false);
						Marshal.Copy(ptr, arr, 0, len);
						mBW.Write(arr);
					}
					Pos += len;
				}
				Marshal.FreeHGlobal(ptr);
			}
		}

		public delegate void DSetFunc(Reader instance);
		public delegate void DPutFunc(Writer instance);

		public string Id { get; private set; }

		DSetFunc mSetFunc;
		DPutFunc mPutFunc;

		Chunk() { }
		public Chunk(string id, DPutFunc putFunc, DSetFunc setFunc) {
			Id = id;
			mSetFunc = setFunc;
			mPutFunc = putFunc;
		}

		public static void Save(string id, DPutFunc putFunc, BinaryWriter bw) {
			var save = new Writer();
			putFunc(save);
			var size = save.Pos;
			if (0 == size) {
				return;
			}
			bw.Write(id.ToCharArray());
			var sizePos = bw.BaseStream.Position;
			bw.Write(0xFFFFFFFF);
			putFunc(new Writer(bw, size));
			bw.BaseStream.Position = sizePos;
			bw.Write((uint)size);
			bw.BaseStream.Position += size;
		}
		public void Save(BinaryWriter bw) {
			Save(Id, mPutFunc, bw);
		}
		public void Load(BinaryReader br, long size) {
			mSetFunc(new Reader(br, size));
		}
	}

	protected class LIST {
		public delegate void DSetFunc(BinaryReader br, long size);
		public delegate void DPutFunc(BinaryWriter bw);
		public string Id { get; private set; }
		DSetFunc mSetFunc;
		DPutFunc mPutFunc;
		LIST() { }
		public LIST(string id, DPutFunc putFunc, DSetFunc setFunc) {
			Id = id;
			mSetFunc = setFunc;
			mPutFunc = putFunc;
		}
		public void Save(BinaryWriter bw) {
			mPutFunc(bw);
		}
		public void Load(BinaryReader br, long size) {
			mSetFunc(br, size);
		}
	}

	public Riff() {
		Init();
	}

	public Riff(string path) {
		Init();
		using (var fs = new FileStream(path, FileMode.Open))
		using (var br = new BinaryReader(fs)) {
			var riffId = Encoding.ASCII.GetString(br.ReadBytes(4));
			if ("RIFF" != riffId) {
				fs.Close();
				return;
			}
			var fileSize = br.ReadUInt32();
			var fileId = Encoding.ASCII.GetString(br.ReadBytes(4));
			Load(br, fileSize - 4);
			fs.Close();
		}
	}

	public Riff(BinaryReader br, long size) {
		Init();
		Load(br, size);
	}

	public void Save(string path) {
		var fs = new FileStream(path, FileMode.Create);
		var bw = new BinaryWriter(fs);
		Write(bw, "RIFF");
		fs.Close();
		fs.Dispose();
	}

	public void Write(BinaryWriter bw) {
		Write(bw, "LIST");
	}

	protected abstract void Initialize(out string id, List<Chunk> chunks, List<LIST> riffs);

	void Init() {
		string id;
		var chunks = new List<Chunk>();
		var lists = new List<LIST>();
		Initialize(out id, chunks, lists);
		Id = id;
		foreach (var chunk in chunks) {
			mChunks.Add(chunk.Id, chunk);
		}
		foreach (var list in lists) {
			mLists.Add(list.Id, list);
		}
	}

	void Load(BinaryReader br, long size) {
		long pos = 0;
		while (pos < size) {
			var chunkId = Encoding.ASCII.GetString(br.ReadBytes(4));
			var chunkSize = br.ReadUInt32();
			pos += chunkSize + 8;
			if ("LIST" == chunkId) {
				chunkId = Encoding.ASCII.GetString(br.ReadBytes(4));
				chunkSize -= 4;
				if ("INFO" == chunkId) {
					LoadInfo(br, chunkSize);
				} else {
					LoadChunk(br, chunkId, chunkSize);
				}
			} else {
				LoadChunk(br, chunkId, chunkSize);
			}
		}
	}

	void LoadChunk(BinaryReader br, string type, long size) {
		var begin = br.BaseStream.Position;
		if (mChunks.ContainsKey(type)) {
			mChunks[type].Load(br, size);
		}
		if (mLists.ContainsKey(type)) {
			mLists[type].Load(br, size);
		}
		br.BaseStream.Position = begin + size;
	}

	void LoadInfo(BinaryReader br, long size) {
		var begin = br.BaseStream.Position;
		long pos = 0;
		while (pos < size) {
			var infoType = Encoding.ASCII.GetString(br.ReadBytes(4));
			var infoSize = br.ReadInt32();
			infoSize += (0 == infoSize % 2) ? 0 : 1;
			var arr = br.ReadBytes(infoSize);
			Info[infoType] = Enc.GetString(arr).Replace("\0", "");
			pos += infoSize + 8;
		}
		br.BaseStream.Position = begin + size;
	}

	void Write(BinaryWriter bw, string type) {
		bw.Write(type.ToCharArray());
		var sizePos = bw.BaseStream.Position;
		bw.Write(0xFFFFFFFF);
		bw.Write(Id.ToCharArray());
		foreach (var chunk in mChunks.Values) {
			chunk.Save(bw);
		}
		foreach (var list in mLists.Values) {
			list.Save(bw);
		}
		Info.Write(bw);
		var size = bw.BaseStream.Position - sizePos;
		if (8 == size) {
			bw.BaseStream.Position = sizePos - 4;
		} else {
			bw.BaseStream.Position = sizePos;
			bw.Write((uint)size - 4);
			bw.BaseStream.Position = sizePos + size;
		}
	}
}

public class Info {
	public static class TYPE {
		/// <summary>ArchivalLocation</summary>
		public const string IARL = "IARL";
		/// <summary>Artists</summary>
		public const string IART = "IART";
		/// <summary>Category</summary>
		public const string ICAT = "ICAT";
		/// <summary>Commissioned</summary>
		public const string ICMS = "ICMS";
		/// <summary>Comments</summary>
		public const string ICMT = "ICMT";
		/// <summary>Copyright</summary>
		public const string ICOP = "ICOP";
		/// <summary>CreationDate</summary>
		public const string ICRD = "ICRD";
		/// <summary>Engineer</summary>
		public const string IENG = "IENG";
		/// <summary>Genre</summary>
		public const string IGNR = "IGNR";
		/// <summary>Keywords</summary>
		public const string IKEY = "IKEY";
		/// <summary>Medium</summary>
		public const string IMED = "IMED";
		/// <summary>Name</summary>
		public const string INAM = "INAM";
		/// <summary>Product</summary>
		public const string IPRD = "IPRD";
		/// <summary>Software</summary>
		public const string ISFT = "ISFT";
		/// <summary>Source</summary>
		public const string ISRC = "ISRC";
		/// <summary>SourceForm</summary>
		public const string ISRF = "ISRF";
		/// <summary>Subject</summary>
		public const string ISBJ = "ISBJ";
		/// <summary>Technician</summary>
		public const string ITCH = "ITCH";
	}

	private Dictionary<string, string> mList = new Dictionary<string, string>();

	public string this[string key] {
		get {
			if (mList.ContainsKey(key)) {
				return mList[key];
			} else {
				return "";
			}
		}
		set {
			if (string.IsNullOrEmpty(value)) {
				return;
			}
			if (mList.ContainsKey(key)) {
				mList[key] = value;
			} else {
				mList.Add(key, value);
			}
		}
	}

	public void CopyFrom(Info info) {
		foreach (var v in info.mList) {
			this[v.Key] = v.Value;
		}
	}

	public void Write(BinaryWriter bw) {
		var msInfo = new MemoryStream();
		var bwInfo = new BinaryWriter(msInfo);
		foreach (var v in mList) {
			_Write(bwInfo, v.Key, v.Value);
		}
		if (0 < msInfo.Length) {
			bw.Write("LIST".ToCharArray());
			bw.Write((uint)(msInfo.Length + 4));
			bw.Write("INFO".ToCharArray());
			bw.Write(msInfo.ToArray());
		}
	}

	void _Write(BinaryWriter bw, string type, string text) {
		if (!string.IsNullOrWhiteSpace(text)) {
			var pad = 2 - (Riff.Enc.GetBytes(text).Length % 2);
			for (int i = 0; i < pad; ++i) {
				text += "\0";
			}
			var data = Riff.Enc.GetBytes(text);
			bw.Write(type.ToCharArray());
			bw.Write((uint)data.Length);
			bw.Write(data);
		}
	}
}
