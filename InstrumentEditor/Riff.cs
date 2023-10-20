using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public abstract class Riff {
    public static readonly Encoding Enc = Encoding.GetEncoding("shift-jis");

    public string Id { get; private set; }
    public Info Info = new Info();

    Dictionary<string, Chunk> mChunks = new Dictionary<string, Chunk>();
    Dictionary<string, RList> mLists = new Dictionary<string, RList>();

    protected class Chunk {
        public class Reader {
            IntPtr mPtr = IntPtr.Zero;
            public int Pos { get; private set; } = 0;
            public int Size { get; private set; } = 0;
            public Reader(IntPtr ptr, int size) {
                mPtr = ptr;
                Size = size;
            }
            public void Seek(int seek) {
                if (Pos + seek <= Size && mPtr != IntPtr.Zero) {
                    mPtr += seek;
                    Pos += seek;
                }
            }
            public void Read(out byte[] data) {
                data = new byte[Size];
                if (Pos < Size && mPtr != IntPtr.Zero) {
                    Marshal.Copy(mPtr, data, 0, Size);
                    mPtr += Size;
                    Pos += Size;
                }
            }
            public void Read<T>(ref T value) {
                var len = Marshal.SizeOf<T>();
                if (Pos < Size && mPtr != IntPtr.Zero) {
                    value = Marshal.PtrToStructure<T>(mPtr);
                    mPtr += len;
                    Pos += len;
                } else {
                    value = default;
                }
            }
            public void Read<T>(List<T> list) {
                var len = Marshal.SizeOf<T>();
                while (Pos < Size && mPtr != IntPtr.Zero) {
                    list.Add(Marshal.PtrToStructure<T>(mPtr));
                    mPtr += len;
                    Pos += len;
                }
            }
        }

        public class Writer {
            IntPtr mPtr = IntPtr.Zero;
            public int Size { get; private set; } = 0;
            public int Pos { get; private set; } = 0;
            public Writer() { }
            public Writer(IntPtr ptr, int size) {
                mPtr = ptr;
                Size = size;
            }
            public void Write(byte[] data) {
                var len = data.Length;
                if (Pos + len <= Size && mPtr != IntPtr.Zero) {
                    Marshal.Copy(data, 0, mPtr, len);
                    mPtr += len;
                }
                Pos += len;
            }
            public void Write<T>(T value) {
                var len = Marshal.SizeOf<T>();
                if (Pos + len <= Size && mPtr != IntPtr.Zero) {
                    Marshal.StructureToPtr(value, mPtr, false);
                    mPtr += len;
                }
                Pos += len;
            }
            public void Write<T>(List<T> list) {
                var len = Marshal.SizeOf<T>();
                foreach (var item in list) {
                    if (Pos + len <= Size && mPtr != IntPtr.Zero) {
                        Marshal.StructureToPtr(item, mPtr, false);
                        mPtr += len;
                    }
                    Pos += len;
                }
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
            var pBuff = Marshal.AllocHGlobal(size);
            save = new Writer(pBuff, size);
            putFunc(save);
            var buff = new byte[size];
            Marshal.Copy(pBuff, buff, 0, size);
            Marshal.FreeHGlobal(pBuff);
            bw.Write(id.ToCharArray());
            bw.Write(size);
            bw.Write(buff);
        }
        public void Save(BinaryWriter bw) {
            Save(Id, mPutFunc, bw);
        }
        public void Load(IntPtr ptr, long size) {
            var load = new Reader(ptr, (int)size);
            mSetFunc(load);
        }
    }

    protected class RList {
        public class Reader {
            IntPtr mPtr = IntPtr.Zero;
            public int Size { get; private set; } = 0;
            public Reader(IntPtr ptr, int size) {
                mPtr = ptr;
                Size = size;
            }
            public void Read(Riff riff) {
                riff.Load(mPtr, Size);
            }
        }

        public class Writer {
            BinaryWriter mBw = null;
            public Writer(BinaryWriter bw) {
                mBw = bw;
            }
            public void Write(Riff riff) {
                riff.Write(mBw);
            }
        }

        public delegate void DSetFunc(Reader instance);
        public delegate void DPutFunc(Writer instance);

        public string Id { get; private set; }

        DSetFunc mSetFunc;
        DPutFunc mPutFunc;

        RList() { }
        public RList(string id, DPutFunc putFunc, DSetFunc setFunc) {
            Id = id;
            mSetFunc = setFunc;
            mPutFunc = putFunc;
        }
        public void Save(BinaryWriter bw) {
            var save = new Writer(bw);
            mPutFunc(save);
        }
        public void Load(IntPtr ptr, long size) {
            var load = new Reader(ptr, (int)size);
            mSetFunc(load);
        }
    }

    public Riff() {
        string id;
        var chunks = new List<Chunk>();
        var lists = new List<RList>();
        Init(out id, chunks, lists);
        Id = id;
        foreach (var chunk in chunks) {
            mChunks.Add(chunk.Id, chunk);
        }
        foreach (var list in lists) {
            mLists.Add(list.Id, list);
        }
    }

    public virtual void Write(BinaryWriter bw) {
        var msCh = new MemoryStream();
        var bwCh = new BinaryWriter(msCh);
        bwCh.Write("LIST".ToCharArray());
        bwCh.Write(0xFFFFFFFF);
        bwCh.Write(Id.ToCharArray());
        Info.Write(bwCh);
        foreach (var chunk in mChunks.Values) {
            chunk.Save(bwCh);
        }
        foreach (var list in mLists.Values) {
            list.Save(bwCh);
        }
        bwCh.Seek(4, SeekOrigin.Begin);
        bwCh.Write((uint)msCh.Length - 8);
        bw.Write(msCh.ToArray());
    }

    protected abstract void Init(out string id, List<Chunk> chunks, List<RList> riffs);

    protected void MainLoop(string filePath) {
        using (var fs = new FileStream(filePath, FileMode.Open))
        using (var br = new BinaryReader(fs)) {
            var size = (int)fs.Length;
            var pBuff = Marshal.AllocHGlobal(size);
            Marshal.Copy(br.ReadBytes(size), 0, pBuff, size);

            var riffId = Marshal.PtrToStringAnsi(pBuff, 4);
            if ("RIFF" != riffId) {
                Marshal.FreeHGlobal(pBuff);
                fs.Close();
                return;
            }

            var fileSize = Marshal.PtrToStructure<int>(pBuff + 4);
            var fileId = Marshal.PtrToStringAnsi(pBuff + 8, 4);
            Load(pBuff + 12, fileSize - 4);

            Marshal.FreeHGlobal(pBuff);
            fs.Close();
        }
    }

    protected void Load(IntPtr ptr, long size) {
        long pos = 0;
        while (pos < size) {
            var chunkId = Marshal.PtrToStringAnsi(ptr, 4);
            var chunkSize = Marshal.PtrToStructure<int>(ptr + 4);
            ptr += 8;
            pos += chunkSize + 8;
            if ("LIST" == chunkId) {
                chunkId = Marshal.PtrToStringAnsi(ptr, 4);
                ptr += 4;
                chunkSize -= 4;
                if ("INFO" == chunkId) {
                    _LoopInfo(ptr, chunkSize);
                } else {
                    _LoadChunk(ptr, chunkId, chunkSize);
                }
            } else {
                _LoadChunk(ptr, chunkId, chunkSize);
            }
            ptr += chunkSize;
        }
    }

    protected virtual void LoadChunk(IntPtr ptr, string type, long size) { }

    protected virtual void LoadInfo(IntPtr ptr, string type, string value) { }

    void _LoadChunk(IntPtr ptr, string type, long size) {
        if (mChunks.ContainsKey(type)) {
            mChunks[type].Load(ptr, size);
        } else if (mLists.ContainsKey(type)) {
            mLists[type].Load(ptr, size);
        } else {
            LoadChunk(ptr, type, size);
        }
    }

    void _LoopInfo(IntPtr ptr, long size) {
        long pos = 0;
        while (pos < size) {
            var infoType = Marshal.PtrToStringAnsi(ptr, 4);
            ptr += 4;
            var infoSize = Marshal.PtrToStructure<int>(ptr);
            ptr += 4;

            infoSize += (0 == infoSize % 2) ? 0 : 1;
            pos += infoSize + 8;

            var arr = new byte[infoSize];
            Marshal.Copy(ptr, arr, 0, infoSize);
            var text = Enc.GetString(arr).Replace("\0", "");

            LoadInfo(ptr, infoType, text);
            ptr += infoSize;
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
