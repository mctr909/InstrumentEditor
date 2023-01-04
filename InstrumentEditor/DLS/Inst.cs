using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace DLS {
    public class LINS : Riff {
        public sealed class Sort : IComparer<MidiLocale> {
            // IComparerの実装
            public int Compare(MidiLocale x, MidiLocale y) {
                var xKey = ((x.BankFlg & 0x80) << 17) | (x.ProgNum << 16) | (x.BankMSB << 8) | x.BankLSB;
                var yKey = ((y.BankFlg & 0x80) << 17) | (y.ProgNum << 16) | (y.BankMSB << 8) | y.BankLSB;
                return xKey - yKey;
            }
        }

        public SortedDictionary<MidiLocale, INS> List = new SortedDictionary<MidiLocale, INS>(new Sort());

        public LINS() { }

        public LINS(IntPtr ptr, long size) : base() {
            Load(ptr, size);
        }

        protected override void LoadChunk(IntPtr ptr, long size, string listType) {
            switch (listType) {
            case "ins ":
                var inst = new INS(ptr, size);
                if (List.ContainsKey(inst.Header.Locale)) {
                    return;
                }
                List.Add(inst.Header.Locale, inst);
                break;
            default:
                throw new Exception(string.Format("Unknown ListId [{0}]", listType));
            }
        }

        public void Write(BinaryWriter bw) {
            var msLins = new MemoryStream();
            var bwLins = new BinaryWriter(msLins);
            foreach (var ins in List) {
                ins.Value.Write(bwLins);
            }

            if (0 < msLins.Length) {
                bw.Write("LIST".ToCharArray());
                bw.Write((uint)(msLins.Length + 4));
                bw.Write("lins".ToCharArray());
                bw.Write(msLins.ToArray());
            }
        }
    }

    public class INS : Riff {
        public CK_INSH Header;
        public LRGN Regions = new LRGN();
        public LART Articulations = new LART();
        public Info Info = new Info();

        public INS() { }

        public INS(byte programNo, byte bankMSB = 0, byte bankLSB = 0, bool isDrum = false) {
            Header.Locale.BankFlg = (byte)(isDrum ? 0x80 : 0x00);
            Header.Locale.ProgNum = programNo;
            Header.Locale.BankMSB = bankMSB;
            Header.Locale.BankLSB = bankLSB;
        }

        public INS(IntPtr ptr, long size) : base() {
            Load(ptr, size);
        }

        protected override void LoadChunk(IntPtr ptr, long size, string type) {
            switch (type) {
            case "insh":
                Header = Marshal.PtrToStructure<CK_INSH>(ptr);
                break;
            case "lrgn":
                Regions = new LRGN(ptr, size);
                break;
            case "lart":
            case "lar2":
                Articulations = new LART(ptr, size);
                break;
            default:
                throw new Exception(string.Format("Unknown ChunkType [{0}]", type));
            }
        }

        protected override void LoadInfo(IntPtr ptr, string value, string type) {
            Info.Add(type, value);
        }

        public void Write(BinaryWriter bw) {
            var msIns = new MemoryStream();
            var bwIns = new BinaryWriter(msIns);
            bwIns.Write("LIST".ToCharArray());
            bwIns.Write(0xFFFFFFFF);
            bwIns.Write("ins ".ToCharArray());

            Header.Write(bwIns);

            Regions.Write(bwIns);
            Articulations.Write(bwIns);

            Info.Write(bwIns);

            bwIns.Seek(4, SeekOrigin.Begin);
            bwIns.Write((uint)msIns.Length - 8);
            bw.Write(msIns.ToArray());
        }
    }
}
