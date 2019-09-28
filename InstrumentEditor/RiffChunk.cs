using System.IO;

unsafe public class RIFF {
    protected uint mChunkType;
    protected uint mChunkSize;
    protected uint mListType;

    protected RIFF() { }

    protected RIFF(byte* ptr, byte* endPtr) {
        while (ptr < endPtr) {
            mChunkType = *(uint*)ptr;
            ptr += sizeof(uint);
            mChunkSize = *(uint*)ptr;
            ptr += sizeof(uint);
            if (0x5453494C == mChunkType) {
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

    protected virtual void ReadChunk(byte* ptr) { }

    protected virtual void ReadList(byte* ptr, byte* endPtr) { }

    protected virtual void WriteChunk(BinaryWriter bw) { }

    protected virtual void WriteList(BinaryWriter bw) { }
}
