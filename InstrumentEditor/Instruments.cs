using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Riff;

namespace Instruments {
    #region enum
    public enum ART_TYPE : ushort {
        GAIN_CONST     = 0x0000,
        GAIN_VALIABLE  = 0x0001,
        PAN_CONST      = 0x0002,
        PAN_VALIABLE   = 0x0003,
        PITCH_CONST    = 0x0004,
        PITCH_VALIABLE = 0x0005,

        COASE_TUNE   = 0x0006,
        OVERRIDE_KEY = 0x0007,

        LPF_RESONANCE       = 0x0008,
        LPF_CUTOFF_CONST    = 0x0009,
        LPF_CUTOFF_VALIABLE = 0x000A,

        COMP_THRESHOLD = 0x0010,
        COMP_RATIO     = 0x0011,
        COMP_ATTACK    = 0x0012,
        COMP_RELEASE   = 0x0013,

        EG_AMP_ATTACK  = 0x0100,
        EG_AMP_HOLD    = 0x0101,
        EG_AMP_DECAY   = 0x0102,
        EG_AMP_RELEASE = 0x0103,
        EG_AMP_SUSTAIN = 0x0106,

        EG_CUTOFF_ATTACK  = 0x0110,
        EG_CUTOFF_HOLD    = 0x0111,
        EG_CUTOFF_DECAY   = 0x0112,
        EG_CUTOFF_RELEASE = 0x0113,
        EG_CUTOFF_RISE    = 0x0114,
        EG_CUTOFF_TOP     = 0x0115,
        EG_CUTOFF_SUSTAIN = 0x0116,
        EG_CUTOFF_FALL    = 0x0117,

        EG1_ATTACK  = 0x0120,
        EG1_HOLD    = 0x0121,
        EG1_DECAY   = 0x0122,
        EG1_RELEASE = 0x0123,
        EG1_RISE    = 0x0124,
        EG1_TOP     = 0x0125,
        EG1_SUSTAIN = 0x0126,
        EG1_FALL    = 0x0127,

        EG2_ATTACK  = 0x0130,
        EG2_HOLD    = 0x0131,
        EG2_DECAY   = 0x0132,
        EG2_RELEASE = 0x0133,
        EG2_RISE    = 0x0134,
        EG2_TOP     = 0x0135,
        EG2_SUSTAIN = 0x0136,
        EG2_FALL    = 0x0137,

        LFO1_WAVE_FORM    = 0x0140,
        LFO1_PHASE        = 0x0141,
        LFO1_AMP_CONST    = 0x0142,
        LFO1_AMP_INPUT    = 0x0143,
        LFO1_BEAT_MODE    = 0x0144,
        LFO1_FREQ_CONST   = 0x0145,
        LFO1_FREQ_INPUT   = 0x0146,
        LFO1_OFFSET_CONST = 0x0147,
        LFO1_OFFSET_INPUT = 0x0148,
        LFO1_DUTY_CONST   = 0x0149,
        LFO1_DUTY_INPUT   = 0x014A,

        LFO2_WAVE_FORM    = 0x0150,
        LFO2_PHASE        = 0x0151,
        LFO2_AMP_CONST    = 0x0152,
        LFO2_AMP_INPUT    = 0x0153,
        LFO2_BEAT_MODE    = 0x0154,
        LFO2_FREQ_CONST   = 0x0155,
        LFO2_FREQ_INPUT   = 0x0156,
        LFO2_OFFSET_CONST = 0x0157,
        LFO2_OFFSET_INPUT = 0x0158,
        LFO2_DUTY_CONST   = 0x0159,
        LFO2_DUTY_INPUT   = 0x015A,

        CH_LFO1_WAVE_FORM    = 0x0180,
        CH_LFO1_PHASE        = 0x0181,
        CH_LFO1_AMP_CONST    = 0x0182,
        CH_LFO1_AMP_INPUT    = 0x0183,
        CH_LFO1_BEAT_MODE    = 0x0184,
        CH_LFO1_FREQ_CONST   = 0x0185,
        CH_LFO1_FREQ_INPUT   = 0x0186,
        CH_LFO1_OFFSET_CONST = 0x0187,
        CH_LFO1_OFFSET_INPUT = 0x0188,
        CH_LFO1_DUTY_CONST   = 0x0189,
        CH_LFO1_DUTY_INPUT   = 0x018A,

        CH_LFO2_WAVE_FORM    = 0x0190,
        CH_LFO2_PHASE        = 0x0191,
        CH_LFO2_AMP_CONST    = 0x0192,
        CH_LFO2_AMP_INPUT    = 0x0193,
        CH_LFO2_BEAT_MODE    = 0x0194,
        CH_LFO2_FREQ_CONST   = 0x0195,
        CH_LFO2_FREQ_INPUT   = 0x0196,
        CH_LFO2_OFFSET_CONST = 0x0197,
        CH_LFO2_OFFSET_INPUT = 0x0198,
        CH_LFO2_DUTY_CONST   = 0x0199,
        CH_LFO2_DUTY_INPUT   = 0x019A,

        INST_INDEX = 0xFFFE,
        WAVE_INDEX = 0xFFFF
    }
    #endregion

    #region struct
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct WPTR {
        public uint ofsHeader;
        public uint ofsData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct WAVH {
        public uint   SampleRate;
        public uint   LoopBegin;
        public uint   LoopLength;
        public byte   LoopEnable;
        public byte   UnityNote;
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

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct PREH {
        public byte BankFlg;
        public byte BankMSB;
        public byte BankLSB;
        public byte ProgNum;

        public void Write(BinaryWriter bw) {
            bw.Write("preh".ToCharArray());
            bw.Write(Marshal.SizeOf<PREH>());
            bw.Write(BankFlg);
            bw.Write(BankMSB);
            bw.Write(BankLSB);
            bw.Write(ProgNum);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct LYRH {
        public byte KeyLo;
        public byte KeyHi;
        public byte VelLo;
        public byte VelHi;

        public void Write(BinaryWriter bw) {
            bw.Write("lyrh".ToCharArray());
            bw.Write(Marshal.SizeOf<LYRH>());
            bw.Write(KeyLo);
            bw.Write(KeyHi);
            bw.Write(VelLo);
            bw.Write(VelHi);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct RGNH {
        public byte KeyLo;
        public byte KeyHi;
        public byte VelLo;
        public byte VelHi;

        public void Write(BinaryWriter bw) {
            bw.Write("rgnh".ToCharArray());
            bw.Write(Marshal.SizeOf<RGNH>());
            bw.Write(KeyLo);
            bw.Write(KeyHi);
            bw.Write(VelLo);
            bw.Write(VelHi);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ART {
        public ART_TYPE Type;
        public ushort   Reserved;
        public float    Value;

        public void Write(BinaryWriter bw) {
            bw.Write((uint)Type);
            bw.Write(Reserved);
            bw.Write(Value);
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

        public void Write(BinaryWriter bw) {
            bw.Write("fmt ".ToCharArray());
            bw.Write(Marshal.SizeOf<FMT>());
            bw.Write(Tag);
            bw.Write(Channels);
            bw.Write(SampleRate);
            bw.Write(BytesPerSec);
            bw.Write(BlockAlign);
            bw.Write(Bits);
        }
    }
    #endregion

    public class File : Chunk {
        public LWave Wave = new LWave();
        public LInst Inst = new LInst();
        public LPreset Preset = new LPreset();
        public Info Info = new Info();

        public File() { }

        public File(string filePath) : base(filePath) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "wptr":
                break;
            default:
                break;
            }
        }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "lwav":
                Wave = new LWave(ptr, ptrTerm);
                break;
            case "lins":
                Inst = new LInst(ptr, ptrTerm);
                break;
            case "lpre":
                Preset = new LPreset(ptr, ptrTerm);
                break;
            case "INFO":
                Info = new Info(ptr, ptrTerm);
                break;
            default:
                break;
            }
        }

        public void Save(string path) {
            var fs = new FileStream(path, FileMode.Create);
            var bw = new BinaryWriter(fs);

            bw.Write("RIFF".ToCharArray());
            bw.Write(0xFFFFFFFF);
            bw.Write("INST".ToCharArray());

            Wave.Write(fs);
            Inst.Write(fs);
            Preset.Write(fs);
            Info.Write(bw);

            fs.Seek(4, SeekOrigin.Begin);
            bw.Write((uint)fs.Length - 8);

            fs.Close();
            fs.Dispose();
        }

        public void DeleteWave(List<int> indices) {
            // find deletable wave
            var deleteList = new Dictionary<int, bool>();
            foreach (int selectedIndex in indices) {
                var deletable = true;
                foreach (var inst in Inst.Array) {
                    foreach (var region in inst.Region.Array) {
                        foreach (var art in region.Art.Array) {
                            if (art.Type != ART_TYPE.WAVE_INDEX) {
                                continue;
                            }
                            if (selectedIndex == (int)art.Value) {
                                deletable = false;
                                break;
                            }
                        }
                        if (!deletable) {
                            break;
                        }
                    }
                    if (!deletable) {
                        break;
                    }
                }
                deleteList.Add(selectedIndex, deletable);
            }

            // renumbering
            var newIndex = 0;
            var renumberingList = new Dictionary<int, int>();
            for (var iWave = 0; iWave < Wave.Count; iWave++) {
                if (deleteList.ContainsKey(iWave) && deleteList[iWave]) {
                    continue;
                }
                renumberingList.Add(iWave, newIndex);
                ++newIndex;
            }

            // delete wave
            var waveList = new List<Wave>();
            for (var iWave = 0; iWave < Wave.Count; iWave++) {
                if (deleteList.ContainsKey(iWave) && deleteList[iWave]) {
                    continue;
                }
                waveList.Add(Wave[iWave]);
            }
            Wave.Clear();
            Wave.AddRange(waveList);

            // update inst's region art
            for (var iInst = 0; iInst < Inst.Count; iInst++) {
                var inst = Inst[iInst];
                for (var iRgn = 0; iRgn < inst.Region.Count; iRgn++) {
                    var rgn = inst.Region[iRgn];
                    for (var iArt = 0; iArt < rgn.Art.Count; iArt++) {
                        if (rgn.Art[iArt].Type != ART_TYPE.WAVE_INDEX) {
                            continue;
                        }
                        var iWave = (int)rgn.Art[iArt].Value;
                        if (renumberingList.ContainsKey(iWave)) {
                            rgn.Art.Update(ART_TYPE.WAVE_INDEX, renumberingList[iWave]);
                        }
                    }
                }
            }
        }

        public void DeleteInst(List<int> indices) {
            // find deletable inst
            var deleteList = new Dictionary<int, bool>();
            foreach (int selectedIndex in indices) {
                var deletable = true;
                foreach (var preset in Preset.Values) {
                    foreach (var layer in preset.Layer.Array) {
                        foreach (var art in layer.Art.Array) {
                            if (art.Type != ART_TYPE.INST_INDEX) {
                                continue;
                            }
                            if (selectedIndex == (int)art.Value) {
                                deletable = false;
                                break;
                            }
                        }
                        if (!deletable) {
                            break;
                        }
                    }
                    if (!deletable) {
                        break;
                    }
                }
                deleteList.Add(selectedIndex, deletable);
            }

            // renumbering
            var newIndex = 0;
            var renumberingList = new Dictionary<int, int>();
            for (var iInst = 0; iInst < Inst.Count; iInst++) {
                if (deleteList.ContainsKey(iInst) && deleteList[iInst]) {
                    continue;
                }
                renumberingList.Add(iInst, newIndex);
                ++newIndex;
            }

            // delete inst
            var newInstList = new List<Inst>();
            for (var iInst = 0; iInst < Inst.Count; iInst++) {
                if (deleteList.ContainsKey(iInst) && deleteList[iInst]) {
                    continue;
                }
                newInstList.Add(Inst[iInst]);
            }
            Inst.Clear();
            Inst.AddRange(newInstList);

            // update preset's layer art
            foreach (var preset in Preset.Values) {
                for (var iLayer = 0; iLayer < preset.Layer.Count; iLayer++) {
                    var layer = preset.Layer[iLayer];
                    foreach (var art in layer.Art.Array) {
                        if (art.Type != ART_TYPE.INST_INDEX) {
                            continue;
                        }
                        var iInst = (int)art.Value;
                        if (renumberingList.ContainsKey(iInst)) {
                            layer.Art.Update(ART_TYPE.INST_INDEX, renumberingList[iInst]);
                        }
                    }
                }
            }
        }
    }

    public class LWave : Chunk {
        private List<Wave> List = new List<Wave>();

        public LWave() { }

        public LWave(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public Wave[] Array {
            get { return List.ToArray(); }
        }

        public Wave this[int index] {
            get { return List[index]; }
        }

        public void Add(Wave wave) {
            List.Add(wave);
        }

        public void AddRange(List<Wave> waves) {
            foreach (var wave in waves) {
                List.Add(wave);
            }
        }

        public bool ContainsKey(int index) {
            return 0 <= index && index < List.Count;
        }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "wave":
                List.Add(new Wave(ptr, ptrTerm));
                break;
            default:
                break;
            }
        }

        public void Write(Stream ms) {
            if (0 == List.Count) {
                return;
            }

            // wptr chunk
            var msWptr = new MemoryStream();
            var bwWptr = new BinaryWriter(msWptr);
            bwWptr.Write("wptr".ToCharArray());
            bwWptr.Write(Marshal.SizeOf<WPTR>() * List.Count);

            // lwav list
            var msLwav = new MemoryStream();
            var bwLwav = new BinaryWriter(msLwav);
            bwLwav.Write("LIST".ToCharArray());
            bwLwav.Write(0xFFFFFFFF);
            bwLwav.Write("lwav".ToCharArray());

            foreach (var wave in List) {
                var pos = msLwav.Position;
                var ofs = wave.Write(msLwav);
                bwWptr.Write((uint)(pos + ofs.ofsHeader));
                bwWptr.Write((uint)(pos + ofs.ofsData));
            }

            bwLwav.Seek(4, SeekOrigin.Begin);
            bwLwav.Write((uint)(msLwav.Length - 8));

            msWptr.WriteTo(ms);
            msLwav.WriteTo(ms);
        }
    }

    public class Wave : Chunk {
        public static readonly string[] NoteName = new string[] {
            "C ", "Db", "D ", "Eb", "E ", "F ", "Gb", "G ", "Ab", "A ", "Bb", "B "
        };

        public WAVH Header;
        public short[] Data = null;
        public Info Info = new Info();

        private FMT mFormat;

        public Wave() { }

        public Wave(string filePath) {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            var riff = br.ReadUInt32();
            var riffSize = br.ReadUInt32();
            var riffType = br.ReadUInt32();

            var arrData = new byte[0];

            while (fs.Position < fs.Length) {
                var chunkType = Encoding.ASCII.GetString(br.ReadBytes(4));
                var chunkSize = br.ReadUInt32();
                var pChunkData = Marshal.AllocHGlobal((int)chunkSize);
                Marshal.Copy(br.ReadBytes((int)chunkSize), 0, pChunkData, (int)chunkSize);

                switch (chunkType) {
                case "fmt ":
                    mFormat = Marshal.PtrToStructure<FMT>(pChunkData);
                    break;
                case "data":
                    arrData = new byte[chunkSize];
                    Marshal.Copy(pChunkData, arrData, 0, (int)chunkSize);
                    break;
                case "wavh":
                    Header = Marshal.PtrToStructure<WAVH>(pChunkData);
                    break;
                case "LIST":
                    switch (Marshal.PtrToStringAnsi(pChunkData, 4)) {
                    case "INFO":
                        Info = new Info(pChunkData + 4, pChunkData + (int)chunkSize);
                        break;
                    }
                    break;
                default:
                    break;
                }
                Marshal.FreeHGlobal(pChunkData);
            }

            var msData = new MemoryStream(arrData);
            var brData = new BinaryReader(msData);

            switch (mFormat.Bits) {
            case 8:
                Data = new short[msData.Length];
                for (var i = 0; msData.Position < msData.Length; i++) {
                    var data = brData.ReadByte() - 128;
                    Data[i] = (short)(256 * data);
                }
                break;
            case 16:
                Data = new short[msData.Length / 2];
                for (var i = 0; msData.Position < msData.Length; i++) {
                    Data[i] = brData.ReadInt16();
                }
                break;
            case 24:
                Data = new short[msData.Length / 3];
                for (var i = 0; msData.Position < msData.Length; i++) {
                    brData.ReadByte();
                    Data[i] = brData.ReadInt16();
                }
                break;
            case 32:
                Data = new short[msData.Length / 4];
                if (mFormat.Tag == 3) {
                    for (var i = 0; msData.Position < msData.Length; i++) {
                        var data = brData.ReadSingle();
                        if (1.0 < data) data = 1.0f;
                        if (data < -1.0) data = -1.0f;
                        Data[i] = (short)(32767 * data);
                    }
                } else {
                    for (var i = 0; msData.Position < msData.Length; i++) {
                        brData.ReadInt16();
                        Data[i] = brData.ReadInt16();
                    }
                }
                break;
            }

            if (0 == Header.SampleRate) {
                Header.SampleRate = mFormat.SampleRate;
                Header.Gain = 1.0;
                Header.Pitch = 1.0;
            }

            if (null == Info) {
                Info = new Info {
                    Name = Path.GetFileNameWithoutExtension(filePath)
                };
            } else if (string.IsNullOrWhiteSpace(Info.Name)) {
                Info.Name = Path.GetFileNameWithoutExtension(filePath);
            }

            br.Dispose();
            fs.Close();
            fs.Dispose();
        }

        public Wave(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "wavh":
                Header = Marshal.PtrToStructure<WAVH>(ptr);
                break;
            case "data":
                Data = new short[chunkSize / 2];
                Marshal.Copy(ptr, Data, 0, Data.Length);
                break;
            default:
                break;
            }
        }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "INFO":
                Info = new Info(ptr, ptrTerm);
                break;
            default:
                break;
            }
        }

        public WPTR Write(MemoryStream ms) {
            WPTR wptr;
            var msWave = new MemoryStream();
            var bwWave = new BinaryWriter(msWave);
            bwWave.Write("LIST".ToCharArray());
            bwWave.Write(0xFFFFFFFF);
            bwWave.Write("wave".ToCharArray());

            wptr.ofsHeader = (uint)msWave.Position + 8;

            Header.Write(bwWave);

            {
                // data chunk
                bwWave.Write("data".ToCharArray());
                bwWave.Write(Data.Length * 2);

                wptr.ofsData = (uint)msWave.Position;
                var ptr = Marshal.AllocHGlobal(Data.Length * 2);
                Marshal.Copy(Data, 0, ptr, Data.Length);
                var arr = new byte[Data.Length * 2];
                Marshal.Copy(ptr, arr, 0, Data.Length * 2);
                bwWave.Write(arr);
                Marshal.FreeHGlobal(ptr);
            }

            Info.Write(bwWave);

            bwWave.Seek(4, SeekOrigin.Begin);
            bwWave.Write((uint)msWave.Length - 8);
            msWave.WriteTo(ms);

            return wptr;
        }

        public void ToFile(string filePath) {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write("RIFF".ToCharArray());
            bw.Write((uint)0);
            bw.Write("WAVE".ToCharArray());

            {
                // fmt chunk
                bw.Write("fmt ".ToCharArray());
                bw.Write((uint)16);
                bw.Write((ushort)1);
                bw.Write((ushort)1);
                bw.Write(Header.SampleRate);
                bw.Write(Header.SampleRate * 2);
                bw.Write((ushort)2);
                bw.Write((ushort)16);
            }

            {
                // data chunk
                var ptr = Marshal.AllocHGlobal(Data.Length * 2);
                Marshal.Copy(Data, 0, ptr, Data.Length);
                var arr = new byte[Data.Length * 2];
                Marshal.Copy(ptr, arr, 0, Data.Length * 2);
                Marshal.FreeHGlobal(ptr);
                bw.Write("data".ToCharArray());
                bw.Write(arr.Length);
                bw.Write(arr);
            }

            Header.Write(bw);
            Info.Write(bw);

            fs.Seek(4, SeekOrigin.Begin);
            bw.Write((uint)(fs.Length - 8));

            bw.Dispose();
            fs.Close();
            fs.Dispose();
        }
    }

    public class LInst : Chunk {
        private List<Inst> List = new List<Inst>();

        public LInst() { }

        public LInst(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public Inst[] Array {
            get { return List.ToArray(); }
        }

        public Inst this[int index] {
            get { return List[index]; }
        }

        public void Add(Inst inst) {
            List.Add(inst);
        }

        public void AddRange(List<Inst> instList) {
            foreach (var inst in instList) {
                List.Add(inst);
            }
        }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "inst":
                List.Add(new Inst(ptr, ptrTerm));
                break;
            default:
                break;
            }
        }

        public void Write(Stream ms) {
            if (0 == List.Count) {
                return;
            }

            var msInst = new MemoryStream();
            var bwInst = new BinaryWriter(msInst);
            bwInst.Write("LIST".ToCharArray());
            bwInst.Write(0xFFFFFFFF);
            bwInst.Write("lins".ToCharArray());
            foreach (var inst in List) {
                inst.Write(msInst);
            }
            bwInst.Seek(4, SeekOrigin.Begin);
            bwInst.Write((int)msInst.Length - 8);
            msInst.WriteTo(ms);
        }
    }

    public class Inst : Chunk {
        public Lart Art = new Lart();
        public LRegion Region = new LRegion();
        public Info Info = new Info();

        public Inst() { }

        public Inst(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "artc":
                Art = new Lart(ptr, chunkSize);
                break;
            default:
                break;
            }
        }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "lrgn":
                Region = new LRegion(ptr, ptrTerm);
                break;
            case "INFO":
                Info = new Info(ptr, ptrTerm);
                break;
            default:
                break;
            }
        }

        public void Write(MemoryStream ms) {
            var msInst = new MemoryStream();
            var bwInst = new BinaryWriter(msInst);
            bwInst.Write("LIST".ToCharArray());
            bwInst.Write(0xFFFFFFFF);
            bwInst.Write("inst".ToCharArray());

            Art.Write(msInst);
            Region.Write(msInst);
            Info.Write(bwInst);

            bwInst.Seek(4, SeekOrigin.Begin);
            bwInst.Write((uint)msInst.Length - 8);
            msInst.WriteTo(ms);
        }
    }

    public class LPreset : Chunk {
        private Dictionary<PREH, Preset> List = new Dictionary<PREH, Preset>();

        public LPreset() { }

        public LPreset(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        public int Count {
            get { return List.Count; }
        }

        public Preset this[PREH id] {
            get { return List[id]; }
        }

        public Dictionary<PREH, Preset>.KeyCollection Keys {
            get { return List.Keys; }
        }

        public Dictionary<PREH, Preset>.ValueCollection Values {
            get { return List.Values; }
        }

        public bool ContainsKey(PREH id) {
            return List.ContainsKey(id);
        }

        public void Add(PREH id, Preset preset) {
            List.Add(id, preset);
        }

        public void Remove(PREH id) {
            List.Remove(id);
        }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "pres":
                var tmp = new Preset(ptr, ptrTerm);
                List.Add(tmp.Header, tmp);
                break;
            default:
                break;
            }
        }

        public void Write(Stream ms) {
            var msPres = new MemoryStream();
            var bwPres = new BinaryWriter(msPres);
            bwPres.Write("LIST".ToCharArray());
            bwPres.Write(0xFFFFFFFF);
            bwPres.Write("lpre".ToCharArray());
            foreach (var pres in List.Values) {
                pres.Write(msPres);
            }
            bwPres.Seek(4, SeekOrigin.Begin);
            bwPres.Write((int)msPres.Length - 8);
            msPres.WriteTo(ms);
        }
    }

    public class Preset : Chunk {
        public PREH Header;
        public Lart Art = new Lart();
        public LLayer Layer = new LLayer();
        public Info Info = new Info();

        public Preset() { }

        public Preset(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "preh":
                Header = Marshal.PtrToStructure<PREH>(ptr);
                break;
            case "artc":
                Art = new Lart(ptr, chunkSize);
                break;
            default:
                break;
            }
        }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "llyr":
                Layer = new LLayer(ptr, ptrTerm);
                break;
            case "INFO":
                Info = new Info(ptr, ptrTerm);
                break;
            default:
                break;
            }
        }

        public void Write(MemoryStream ms) {
            var msInst = new MemoryStream();
            var bwInst = new BinaryWriter(msInst);
            bwInst.Write("LIST".ToCharArray());
            bwInst.Write(0xFFFFFFFF);
            bwInst.Write("pres".ToCharArray());

            Header.Write(bwInst);
            Art.Write(msInst);
            Layer.Write(msInst);
            Info.Write(bwInst);

            bwInst.Seek(4, SeekOrigin.Begin);
            bwInst.Write((uint)msInst.Length - 8);
            msInst.WriteTo(ms);
        }
    }

    public class LLayer : Chunk {
        private List<Layer> List = new List<Layer>();

        public LLayer() { }

        public LLayer(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public Layer[] Array {
            get { return List.ToArray(); }
        }

        public Layer this[int index] {
            get { return List[index]; }
        }

        public void Add(Layer layer) {
            List.Add(layer);
        }

        public void Update(int index, LYRH header) {
            List[index].Header = header;
        }

        public List<Layer> Find(LYRH header) {
            var ret = new List<Layer>();
            foreach (var layer in List) {
                if (header.KeyLo <= layer.Header.KeyHi && layer.Header.KeyLo <= header.KeyHi &&
                    header.VelLo <= layer.Header.VelHi && layer.Header.VelLo <= header.VelHi) {
                    ret.Add(layer);
                }
            }
            return ret;
        }

        public List<Layer> Find(int noteNo, int velocity) {
            var ret = new List<Layer>();
            foreach (var layer in List) {
                if (noteNo <= layer.Header.KeyHi && layer.Header.KeyLo <= noteNo &&
                    velocity <= layer.Header.VelHi && layer.Header.VelLo <= velocity) {
                    ret.Add(layer);
                }
            }
            return ret;
        }

        public bool ContainsKey(LYRH header) {
            foreach (var layer in List) {
                if (header.KeyLo <= layer.Header.KeyHi && layer.Header.KeyLo <= header.KeyHi &&
                    header.VelLo <= layer.Header.VelHi && layer.Header.VelLo <= header.VelHi) {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(int noteNo, int velocity) {
            foreach (var layer in List) {
                if (noteNo <= layer.Header.KeyHi && layer.Header.KeyLo <= noteNo &&
                    velocity <= layer.Header.VelHi && layer.Header.VelLo <= velocity) {
                    return true;
                }
            }
            return false;
        }

        public void Remove(int index) {
            var tmp = new List<Layer>();
            for (var iLayer = 0; iLayer < List.Count; iLayer++) {
                if (iLayer != index) {
                    tmp.Add(List[iLayer]);
                }
            }
            List.Clear();
            List.AddRange(tmp);
        }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "lyr ":
                List.Add(new Layer(ptr, ptrTerm));
                break;
            default:
                break;
            }
        }

        public void Write(MemoryStream ms) {
            var msLlyr = new MemoryStream();
            var bwLlyr = new BinaryWriter(msLlyr);
            bwLlyr.Write("LIST".ToCharArray());
            bwLlyr.Write(0xFFFFFFFF);
            bwLlyr.Write("llyr".ToCharArray());
            foreach (var layer in List) {
                layer.Write(msLlyr);
            }
            bwLlyr.Seek(4, SeekOrigin.Begin);
            bwLlyr.Write((int)msLlyr.Length - 8);
            msLlyr.WriteTo(ms);
        }
    }

    public class Layer : Chunk {
        public LYRH Header;
        public Lart Art = new Lart();

        public Layer() { }

        public Layer(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "lyrh":
                Header = Marshal.PtrToStructure<LYRH>(ptr);
                break;
            case "artc":
                Art = new Lart(ptr, chunkSize);
                break;
            default:
                break;
            }
        }

        public void Write(MemoryStream ms) {
            var msLayer = new MemoryStream();
            var bwLayer = new BinaryWriter(msLayer);
            bwLayer.Write("LIST".ToCharArray());
            bwLayer.Write(0xFFFFFFFF);
            bwLayer.Write("lyr ".ToCharArray());

            Header.Write(bwLayer);
            Art.Write(msLayer);

            bwLayer.Seek(4, SeekOrigin.Begin);
            bwLayer.Write((uint)msLayer.Length - 8);
            msLayer.WriteTo(ms);
        }
    }

    public class LRegion : Chunk {
        private List<Region> List = new List<Region>();

        public LRegion() { }

        public LRegion(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public Region[] Array {
            get { return List.ToArray(); }
        }

        public Region this[int index] {
            get { return List[index]; }
        }

        public void Add(Region region) {
            List.Add(region);
        }

        public List<Region> Find(RGNH header) {
            var ret = new List<Region>();
            foreach (var rng in List) {
                if (header.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= header.KeyHi &&
                    header.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= header.VelHi) {
                    ret.Add(rng);
                }
            }
            return ret;
        }

        public Region Find(int noteNo, int velocity) {
            foreach (var rng in List) {
                if (noteNo <= rng.Header.KeyHi && rng.Header.KeyLo <= noteNo &&
                    velocity <= rng.Header.VelHi && rng.Header.VelLo <= velocity) {
                    return rng;
                }
            }
            return null;
        }

        public Region FindFirst(RGNH header) {
            foreach (var rng in List) {
                if (header.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= header.KeyHi &&
                    header.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= header.VelHi) {
                    return rng;
                }
            }
            return null;
        }

        public bool ContainsKey(RGNH header) {
            foreach (var rng in List) {
                if (header.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= header.KeyHi &&
                    header.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= header.VelHi) {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(int noteNo, int velocity) {
            foreach (var rng in List) {
                if (noteNo <= rng.Header.KeyHi && rng.Header.KeyLo <= noteNo &&
                    velocity <= rng.Header.VelHi && rng.Header.VelLo <= velocity) {
                    return true;
                }
            }
            return false;
        }

        public void Remove(RGNH header) {
            var tmpRegion = new List<Region>();
            foreach (var rng in List) {
                if (header.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= header.KeyHi &&
                    header.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= header.VelHi) {
                } else {
                    tmpRegion.Add(rng);
                }
            }
            List.Clear();
            List.AddRange(tmpRegion);
        }

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "rgn ":
                List.Add(new Region(ptr, ptrTerm));
                break;
            default:
                break;
            }
        }

        public void Write(MemoryStream ms) {
            if (0 == List.Count) {
                return;
            }

            var msLrgn = new MemoryStream();
            var bwLrgn = new BinaryWriter(msLrgn);
            bwLrgn.Write("LIST".ToCharArray());
            bwLrgn.Write(0xFFFFFFFF);
            bwLrgn.Write("lrgn".ToCharArray());
            foreach (var region in List) {
                region.Write(msLrgn);
            }
            bwLrgn.Seek(4, SeekOrigin.Begin);
            bwLrgn.Write((int)msLrgn.Length - 8);
            msLrgn.WriteTo(ms);
        }
    }

    public class Region : Chunk {
        public RGNH Header;
        public Lart Art = new Lart();

        public Region() { }

        public Region(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "rgnh":
                Header = Marshal.PtrToStructure<RGNH>(ptr);
                break;
            case "artc":
                Art = new Lart(ptr, chunkSize);
                break;
            default:
                break;
            }
        }

        public void Write(MemoryStream ms) {
            var msRgn = new MemoryStream();
            var bwRgn = new BinaryWriter(msRgn);
            bwRgn.Write("LIST".ToCharArray());
            bwRgn.Write(0xFFFFFFFF);
            bwRgn.Write("rgn ".ToCharArray());

            Header.Write(bwRgn);
            Art.Write(msRgn);

            bwRgn.Seek(4, SeekOrigin.Begin);
            bwRgn.Write((int)msRgn.Length - 8);
            msRgn.WriteTo(ms);
        }
    }

    public class Lart {
        private List<ART> List = new List<ART>();

        public Lart() { }

        public Lart(IntPtr ptr, int size) {
            for (int ofs = 0; ofs < size; ofs += Marshal.SizeOf<ART>()) {
                List.Add(Marshal.PtrToStructure<ART>(ptr + ofs));
            }
        }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public ART[] Array {
            get { return List.ToArray(); }
        }

        public ART this[int index] {
            get { return List[index]; }
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

        public void Write(MemoryStream ms) {
            var msArt = new MemoryStream();
            var bwArt = new BinaryWriter(msArt);
            bwArt.Write("artc".ToCharArray());
            bwArt.Write(0xFFFFFFFF);
            foreach (var art in List) {
                art.Write(bwArt);
            }
            bwArt.Seek(4, SeekOrigin.Begin);
            bwArt.Write((int)msArt.Length - 8);
            msArt.WriteTo(ms);
        }
    }
}
