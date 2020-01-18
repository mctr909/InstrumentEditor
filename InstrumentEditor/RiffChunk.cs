using System;
using System.IO;
using System.Runtime.InteropServices;

unsafe public class RIFF {
    protected string mChunkType;
    protected int mChunkSize;
    protected uint mListType;

    protected RIFF() { }

    protected RIFF(IntPtr ptr, IntPtr endPtr) {
        while (ptr.ToInt32() < endPtr.ToInt32()) {
            mChunkType = Marshal.PtrToStringAnsi(ptr, 4);
            ptr += sizeof(uint);
            mChunkSize = *(int*)ptr;
            ptr += sizeof(uint);
            if ("RIFF" == mChunkType) {
                mListType = *(uint*)ptr;
                ReadList(ptr + sizeof(uint), ptr + mChunkSize);
            } else {
                mListType = 0;
                ReadChunk(ptr);
            }
            ptr += mChunkSize;
        }
    }

    public byte[] Bytes {
        get {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            WriteChunk(bw);
            WriteList(bw);

            var ms2 = new MemoryStream();
            var bw2 = new BinaryWriter(ms2);
            bw2.Write((uint)0x5453494C);
            bw2.Write((uint)(ms.Length + 4));
            bw2.Write(mListType);
            bw2.Write(ms.ToArray());

            return ms2.ToArray();
        }
    }

    protected virtual void ReadChunk(IntPtr ptr) { }

    protected virtual void ReadList(IntPtr ptr, IntPtr endPtr) { }

    protected virtual void WriteChunk(BinaryWriter bw) { }

    protected virtual void WriteList(BinaryWriter bw) { }
}
