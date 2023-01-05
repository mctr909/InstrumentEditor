using System.IO;
using System.Runtime.InteropServices;

namespace InstPack {
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct WAVH {
        public uint SampleRate;
        public uint LoopBegin;
        public uint LoopLength;
        public byte LoopEnable;
        public byte UnityNote;
        public ushort Reserved;
        public double Gain;
        public double Pitch;

        public void Write(BinaryWriter bw) {
            bw.Write("wavh".ToCharArray());
            bw.Write(Marshal.SizeOf<WAVH>());
            bw.Write(SampleRate);
            bw.Write(LoopBegin);
            bw.Write(LoopLength);
            bw.Write(LoopEnable);
            bw.Write(UnityNote);
            bw.Write(Reserved);
            bw.Write(Gain);
            bw.Write(Pitch);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct FMT {
        public ushort Tag;
        public ushort Channels;
        public uint SampleRate;
        public uint BytesPerSec;
        public ushort BlockAlign;
        public ushort Bits;
    }

    public struct PREH {
        public bool IsDrum;
        public byte BankMSB;
        public byte BankLSB;
        public byte ProgNum;
    }

    public struct RGNH {
        public byte KeyLo;
        public byte KeyHi;
        public byte VelLo;
        public byte VelHi;
    }

    public struct ART {
        public ART_TYPE Type;
        public uint Source;
        public uint Contorol;
        public double Value;
    }
}
