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
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct PREH {
        public byte BankFlg;
        public byte BankMSB;
        public byte BankLSB;
        public byte ProgNum;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct RANGE {
        public byte KeyLo;
        public byte KeyHi;
        public byte VelLo;
        public byte VelHi;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ART {
        public ART_TYPE Type;
        public ushort   Reserved;
        public float    Value;
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
        public Lwav Wave = new Lwav();
        public Lins Inst = new Lins();
        public Lpre Preset = new Lpre();
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
                Wave = new Lwav(ptr, ptrTerm);
                break;
            case "lins":
                Inst = new Lins(ptr, ptrTerm);
                break;
            case "lpre":
                Preset = new Lpre(ptr, ptrTerm);
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
            Inst.Write(bw);
            Preset.Write(bw);
            Info.Write(bw);

            fs.Seek(4, SeekOrigin.Begin);
            bw.Write((uint)fs.Length - 8);

            fs.Close();
            fs.Dispose();
        }

        public void DeleteInst(int[] indices) {
            //
            var useList = new Dictionary<int, bool>();
            foreach (int selectedIndex in indices) {
                var useFlag = false;
                foreach (var preset in Preset.Values) {
                    foreach (var layer in preset.Layer.Array) {
                        foreach (var art in layer.Art.Values) {
                            if (art.Type != ART_TYPE.INST_INDEX) {
                                continue;
                            }
                            if (selectedIndex == (int)art.Value) {
                                useFlag = true;
                                break;
                            }
                        }
                        if (useFlag) {
                            break;
                        }
                    }
                    if (useFlag) {
                        break;
                    }
                }
                useList.Add(selectedIndex, useFlag);
            }
            // renumbering
            var count = 0;
            var renumberingList = new Dictionary<int, int>();
            for (var idx = 0; idx < Inst.Count; idx++) {
                if (useList.ContainsKey(idx) && useList[idx] && idx != count) {
                    renumberingList.Add(idx, count);
                    ++count;
                }
            }
            // delete inst
            var instList = new List<Inst>();
            for (var idx = 0; idx < Wave.Count; idx++) {
                if (useList[idx]) {
                    instList.Add(Inst[idx]);
                }
            }
            Inst.Clear();
            Inst.AddRange(instList);
            //
            foreach(var preset in Preset.Values) { 
                for (var iLayer = 0; iLayer < preset.Layer.Count; iLayer++) {
                    var layer = preset.Layer[iLayer];
                    foreach (var art in layer.Art.Values) {
                        if (art.Type != ART_TYPE.INST_INDEX) {
                            continue;
                        }
                        var index = (int)art.Value;
                        if (renumberingList.ContainsKey(index)) {
                            layer.Art.Update(ART_TYPE.INST_INDEX, renumberingList[index]);
                        }
                    }
                }
            }
        }

        public void DeleteWave(int[] indices) {
            //
            var useList = new Dictionary<int, bool>();
            foreach (int selectedIndex in indices) {
                var useFlag = false;
                foreach (var inst in Inst.Array) {
                    foreach (var region in inst.Region.Array) {
                        foreach (var art in region.Art.Values) {
                            if (art.Type != ART_TYPE.WAVE_INDEX) {
                                continue;
                            }
                            if (selectedIndex == (int)art.Value) {
                                useFlag = true;
                                break;
                            }
                        }
                        if (useFlag) {
                            break;
                        }
                    }
                    if (useFlag) {
                        break;
                    }
                }
                useList.Add(selectedIndex, useFlag);
            }
            // renumbering
            var count = 0;
            var renumberingList = new Dictionary<int, int>();
            for (var idx = 0; idx < Wave.Count; idx++) {
                if (useList.ContainsKey(idx) && useList[idx] && idx != count) {
                    renumberingList.Add(idx, count);
                    ++count;
                }
            }
            // delete wave
            var waveList = new List<Wave>();
            for (var idx = 0; idx < Wave.Count; idx++) {
                if (useList[idx]) {
                    waveList.Add(Wave[idx]);
                }
            }
            Wave.Clear();
            Wave.AddRange(waveList);
            //
            for (var iInst = 0; iInst < Inst.Count; iInst++) {
                var inst = Inst[iInst];
                for (var iRgn = 0; iRgn < inst.Region.Count; iRgn++) {
                    var rgn = inst.Region[iRgn];
                    foreach (var art in rgn.Art.Values) {
                        if (art.Type != ART_TYPE.WAVE_INDEX) {
                            continue;
                        }
                        var index = (int)art.Value;
                        if (renumberingList.ContainsKey(index)) {
                            rgn.Art.Update(ART_TYPE.WAVE_INDEX, renumberingList[index]);
                        }
                    }
                }
            }
        }
    }

    public class Lwav : Chunk {
        private List<Wave> List = new List<Wave>();

        public Lwav() { }

        public Lwav(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

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

        public void Write(FileStream fs) {
            if (0 == List.Count) {
                return;
            }

            var bw = new BinaryWriter(fs);

            // wptr chunk
            bw.Write("wptr".ToCharArray());
            bw.Write(Marshal.SizeOf<WPTR>() * List.Count);
            var pos = -8;
            foreach (var wave in List) {
                var data = wave.Write();
                bw.Write((uint)(pos + data.Key.ofsHeader));
                bw.Write((uint)(pos + data.Key.ofsData));
                pos += data.Value.Length;
            }

            // lwav list
            bw.Write("LIST".ToCharArray());
            var wavePos = fs.Position;
            bw.Write(0xFFFFFFFF);
            bw.Write("lwav".ToCharArray());
            foreach (var wave in List) {
                var data = wave.Write();
                bw.Write(data.Value, 0, data.Value.Length);
            }
            var waveTerm = fs.Position;
            fs.Seek(wavePos, SeekOrigin.Begin);
            bw.Write((uint)(waveTerm - wavePos - 4));
            fs.Seek(waveTerm, SeekOrigin.Begin);
        }
    }

    public class Wave : Chunk {
        public WAVH Header;
        public FMT Format;
        public short[] Data = null;
        public Info Info = new Info();

        public Wave() { }

        public Wave(string filePath) {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            var riff = br.ReadUInt32();
            var riffSize = br.ReadUInt32();
            var riffType = br.ReadUInt32();

            while (fs.Position < fs.Length) {
                var chunkType = Encoding.ASCII.GetString(br.ReadBytes(4));
                var chunkSize = br.ReadUInt32();
                var pChunkData = Marshal.AllocHGlobal((int)chunkSize);
                Marshal.Copy(br.ReadBytes((int)chunkSize), 0, pChunkData, (int)chunkSize);

                switch (chunkType) {
                case "fmt ":
                    Format = Marshal.PtrToStructure<FMT>(pChunkData);
                    break;
                case "data":
                    Data = new short[chunkSize / 2];
                    Marshal.Copy(pChunkData, Data, 0, Data.Length);
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

            if (0 == Header.SampleRate) {
                Header.SampleRate = Format.SampleRate;
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

        public KeyValuePair<WPTR, byte[]> Write() {
            var msWave = new MemoryStream();
            var bwWave = new BinaryWriter(msWave);
            WPTR wptr;

            bwWave.Write("LIST".ToCharArray());
            bwWave.Write(0xFFFFFFFF);
            bwWave.Write("wave".ToCharArray());

            {
                // wavh chunk
                var size = Marshal.SizeOf<WAVH>();
                bwWave.Write("wavh".ToCharArray());
                bwWave.Write(size);

                wptr.ofsHeader = (uint)msWave.Position;
                var ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(Header, ptr, true);
                var arr = new byte[size];
                Marshal.Copy(ptr, arr, 0, size);
                bwWave.Write(arr);
                Marshal.FreeHGlobal(ptr);
            }

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

            msWave.Seek(4, SeekOrigin.Begin);
            bwWave.Write((uint)msWave.Length - 8);

            return new KeyValuePair<WPTR, byte[]>(wptr, msWave.ToArray());
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

            {
                // wavh chunk
                var size = Marshal.SizeOf<WAVH>();
                bw.Write("wavh".ToCharArray());
                bw.Write(size);
                var ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(Header, ptr, true);
                var arr = new byte[size];
                Marshal.Copy(ptr, arr, 0, size);
                bw.Write(arr);
                Marshal.FreeHGlobal(ptr);
            }

            Info.Write(bw);

            fs.Seek(4, SeekOrigin.Begin);
            bw.Write((uint)(fs.Length - 8));

            bw.Dispose();
            fs.Close();
            fs.Dispose();
        }
    }

    public class Lins : Chunk {
        private List<Inst> List = new List<Inst>();

        public Lins() { }

        public Lins(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

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

        public void Write(BinaryWriter bw) {
            if (0 == List.Count) {
                return;
            }

            var msInst = new MemoryStream();
            var bwInst = new BinaryWriter(msInst);
            bwInst.Write("LIST".ToCharArray());
            bwInst.Write(0xFFFFFFFF);
            bwInst.Write("lins".ToCharArray());
            foreach (var inst in List) {
                var data = inst.Write();
                bwInst.Write(data.Value, 0, data.Value.Length);
            }
            msInst.Seek(4, SeekOrigin.Begin);
            bwInst.Write((int)msInst.Length - 8);
            bw.Write(msInst.ToArray());
        }
    }

    public class Inst : Chunk {
        public Lart Art = new Lart();
        public Lrgn Region = new Lrgn();
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
                Region = new Lrgn(ptr, ptrTerm);
                break;
            case "INFO":
                Info = new Info(ptr, ptrTerm);
                break;
            default:
                break;
            }
        }

        public KeyValuePair<uint, byte[]> Write() {
            var msInst = new MemoryStream();
            var bwInst = new BinaryWriter(msInst);

            bwInst.Write("LIST".ToCharArray());
            bwInst.Write(0xFFFFFFFF);
            bwInst.Write("inst".ToCharArray());

            Art.Write(bwInst);
            Region.Write(bwInst);
            Info.Write(bwInst);

            msInst.Seek(4, SeekOrigin.Begin);
            bwInst.Write((uint)msInst.Length - 8);
            return new KeyValuePair<uint, byte[]>(0, msInst.ToArray());
        }
    }

    public class Lpre : Chunk {
        private Dictionary<PREH, Preset> List = new Dictionary<PREH, Preset>();

        public Lpre() { }

        public Lpre(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

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

        public void Write(BinaryWriter bw) {
            var msPres = new MemoryStream();
            var bwPres = new BinaryWriter(msPres);
            bwPres.Write("LIST".ToCharArray());
            bwPres.Write(0xFFFFFFFF);
            bwPres.Write("lpre".ToCharArray());
            foreach (var pres in List.Values) {
                pres.Write(bwPres);
            }
            bwPres.Seek(4, SeekOrigin.Begin);
            bwPres.Write((int)msPres.Length - 8);
            bw.Write(msPres.ToArray());
        }
    }

    public class Preset : Chunk {
        public PREH Header;
        public Lart Art = new Lart();
        public Llyr Layer = new Llyr();
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
                Layer = new Llyr(ptr, ptrTerm);
                break;
            case "INFO":
                Info = new Info(ptr, ptrTerm);
                break;
            default:
                break;
            }
        }

        public void Write(BinaryWriter bw) {
            var msInst = new MemoryStream();
            var bwInst = new BinaryWriter(msInst);
            bwInst.Write("LIST".ToCharArray());
            bwInst.Write(0xFFFFFFFF);
            bwInst.Write("pres".ToCharArray());

            // preh chunk
            var size = Marshal.SizeOf<PREH>();
            bwInst.Write("preh".ToCharArray());
            bwInst.Write(size);
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(Header, ptr, true);
            var arr = new byte[size];
            Marshal.Copy(ptr, arr, 0, size);
            bwInst.Write(arr);

            Art.Write(bwInst);
            Layer.Write(bwInst);
            Info.Write(bwInst);

            msInst.Seek(4, SeekOrigin.Begin);
            bwInst.Write((uint)msInst.Length - 8);
            bw.Write(msInst.ToArray());
        }
    }

    public class Llyr : Chunk {
        private List<Layer> List = new List<Layer>();

        public Llyr() { }

        public Llyr(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

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

        protected override void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) {
            switch (listType) {
            case "lyr ":
                break;
            default:
                break;
            }
        }

        public void Write(BinaryWriter bw) {
            var msLyr = new MemoryStream();
            var bwLyr = new BinaryWriter(msLyr);
            bwLyr.Write("LIST".ToCharArray());
            bwLyr.Write(0xFFFFFFFF);
            bwLyr.Write("llyr".ToCharArray());
            foreach (var layer in List) {
                layer.Write(bwLyr);
            }
            bwLyr.Seek(4, SeekOrigin.Begin);
            bwLyr.Write((int)msLyr.Length - 8);
            bw.Write(msLyr.ToArray());
        }
    }

    public class Layer : Chunk {
        public RANGE Header;
        public Lart Art = new Lart();

        public Layer() { }

        public Layer(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "lyrh":
                Header = Marshal.PtrToStructure<RANGE>(ptr);
                break;
            case "artc":
                Art = new Lart(ptr, chunkSize);
                break;
            default:
                break;
            }
        }

        public void Write(BinaryWriter bw) {
            var msLayer = new MemoryStream();
            var bwLayer = new BinaryWriter(msLayer);

            bwLayer.Write("LIST".ToCharArray());
            bwLayer.Write(0xFFFFFFFF);
            bwLayer.Write("lyr ".ToCharArray());

            // lyrh chunk
            var size = Marshal.SizeOf<RANGE>();
            bwLayer.Write("lyrh".ToCharArray());
            bwLayer.Write(size);
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(Header, ptr, true);
            var arr = new byte[size];
            Marshal.Copy(ptr, arr, 0, size);
            bwLayer.Write(arr);

            Art.Write(bwLayer);

            bwLayer.Seek(4, SeekOrigin.Begin);
            bwLayer.Write((uint)msLayer.Length - 8);
            bw.Write(msLayer.ToArray());
        }
    }

    public class Lrgn : Chunk {
        private List<Region> List = new List<Region>();

        public Lrgn() { }

        public Lrgn(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

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

        public Region FindFirst(RANGE range) {
            foreach (var rng in List) {
                if (range.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= range.KeyHi &&
                    range.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= range.VelHi) {
                    return rng;
                }
            }
            return null;
        }

        public bool ContainsKey(RANGE range) {
            foreach(var rng in List) {
                if (range.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= range.KeyHi &&
                    range.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= range.VelHi) {
                    return true;
                }
            }
            return false;
        }

        public void Remove(RANGE range) {
            var tmpRegion = new List<Region>();
            foreach (var rng in List) {
                if (range.KeyLo <= rng.Header.KeyHi && rng.Header.KeyLo <= range.KeyHi &&
                    range.VelLo <= rng.Header.VelHi && rng.Header.VelLo <= range.VelHi) {
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

        public void Write(BinaryWriter bw) {
            if (0 == List.Count) {
                return;
            }

            var msRgn = new MemoryStream();
            var bwRgn = new BinaryWriter(msRgn);
            bwRgn.Write("LIST".ToCharArray());
            bwRgn.Write(0xFFFFFFFF);
            bwRgn.Write("lrgn".ToCharArray());
            foreach (var region in List) {
                region.Write(bwRgn);
            }
            bwRgn.Seek(4, SeekOrigin.Begin);
            bwRgn.Write((int)msRgn.Length - 8);
            bw.Write(msRgn.ToArray());
        }
    }

    public class Lart {
        private Dictionary<ART_TYPE, ART> List = new Dictionary<ART_TYPE, ART>();

        public Lart() { }

        public Lart(IntPtr ptr, int size) {
            for (int ofs = 0; ofs < size; ofs += Marshal.SizeOf<ART>()) {
                var tmp = Marshal.PtrToStructure<ART>(ptr + ofs);
                List.Add(tmp.Type, tmp);
            }
        }

        public void Clear() {
            List.Clear();
        }

        public int Count {
            get { return List.Count; }
        }

        public Dictionary<ART_TYPE, ART>.KeyCollection Keys {
            get { return List.Keys; }
        }

        public Dictionary<ART_TYPE, ART>.ValueCollection Values {
            get { return List.Values; }
        }

        public ART this[ART_TYPE id] {
            get { return List[id]; }
        }

        public void Add(ART art) {
            List.Add(art.Type, art);
        }

        public void Update(ART_TYPE id, float value) {
            List.Remove(id);
            List.Add(id, new ART {
                Type = id,
                Value = value
            });
        }

        public void Write(BinaryWriter bw) {
            var msArt = new MemoryStream();
            var bwArt = new BinaryWriter(msArt);
            bwArt.Write("artc".ToCharArray());
            bwArt.Write(0xFFFFFFFF);
            foreach (var art in List.Values) {
                bwArt.Write((ushort)art.Type);
                bwArt.Write((ushort)0);
                bwArt.Write(art.Value);
            }
            bwArt.Seek(4, SeekOrigin.Begin);
            bwArt.Write((int)msArt.Length - 8);
            bw.Write(msArt.ToArray());
        }
    }

    public class Region : Chunk {
        public RANGE Header;
        public Lart Art = new Lart();

        public Region() { }

        public Region(IntPtr ptr, IntPtr ptrTerm) : base(ptr, ptrTerm) { }

        protected override void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) {
            switch (chunkType) {
            case "rgnh":
                Header = Marshal.PtrToStructure<RANGE>(ptr);
                break;
            case "artc":
                Art = new Lart(ptr, chunkSize);
                break;
            default:
                break;
            }
        }

        public void Write(BinaryWriter bw) {
            var msRgn = new MemoryStream();
            var bwRgn = new BinaryWriter(msRgn);
            bwRgn.Write("LIST".ToCharArray());
            bwRgn.Write(0xFFFFFFFF);
            bwRgn.Write("rgn ".ToCharArray());

            // rgnh chunk
            var rgnhSize = Marshal.SizeOf<RANGE>();
            bwRgn.Write("rgnh".ToCharArray());
            bwRgn.Write(rgnhSize);
            var ptr = Marshal.AllocHGlobal(rgnhSize);
            Marshal.StructureToPtr(Header, ptr, true);
            var arr = new byte[rgnhSize];
            Marshal.Copy(ptr, arr, 0, rgnhSize);
            bwRgn.Write(arr);

            Art.Write(bwRgn);

            bwRgn.Seek(4, SeekOrigin.Begin);
            bwRgn.Write((int)msRgn.Length - 8);
            bw.Write(msRgn.ToArray());
        }
    }
}
