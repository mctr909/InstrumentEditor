using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Riff {
    public class Chunk {
        protected Chunk() { }

        protected Chunk(IntPtr ptr, IntPtr ptrTerm) {
            main(ptr, ptrTerm);
        }

        private void main(IntPtr ptr, IntPtr ptrTerm) {
            while (ptr.ToInt32() < ptrTerm.ToInt32()) {
                var chunkType = Marshal.PtrToStringAnsi(ptr, 4);
                ptr += 4;
                var chunkSize = Marshal.PtrToStructure<int>(ptr);
                ptr += sizeof(int);

                switch (chunkType) {
                case "RIFF":
                    main(ptr + 4, ptr + chunkSize);
                    break;
                case "LIST":
                    ReadList(ptr + 4, ptr + chunkSize, Marshal.PtrToStringAnsi(ptr, 4));
                    break;
                default:
                    ReadChunk(ptr, chunkSize, chunkType);
                    break;
                }
                ptr += chunkSize;
            }
        }

        public byte[] Bytes {
            get {
                var ms = new MemoryStream();
                var bw = new BinaryWriter(ms);
                var type = Write(bw);

                var ms2 = new MemoryStream();
                var bw2 = new BinaryWriter(ms2);
                bw2.Write("LIST".ToCharArray());
                bw2.Write((uint)(ms.Length + 4));
                bw2.Write(type.ToCharArray());
                bw2.Write(ms.ToArray());

                return ms2.ToArray();
            }
        }

        protected virtual void ReadList(IntPtr ptr, IntPtr ptrTerm, string listType) { }

        protected virtual void ReadChunk(IntPtr ptr, int chunkSize, string chunkType) { }

        protected virtual string Write(BinaryWriter bw) { return ""; }
    }

    public class Info {
        [StructLayout(LayoutKind.Sequential, Pack = 8, Size = 8)]
        private struct CK_INFO {
            [MarshalAs(UnmanagedType.U4)]
            public TYPE Type;
            public int Size;

            public enum TYPE : uint {
                IARL = 0x4C524149, // ArchivalLocation
                IART = 0x54524149, // Artists
                ICAT = 0x54414349, // Category
                ICMS = 0x534D4349, // Commissioned
                ICMT = 0x544D4349, // Comments
                ICOP = 0x504F4349, // Copyright
                ICRD = 0x44524349, // CreationDate
                IENG = 0x474E4549, // Engineer
                IGNR = 0x524E4749, // Genre
                IKEY = 0x59454B49, // Keywords
                IMED = 0x44454D49, // Medium
                INAM = 0x4D414E49, // Name
                IPRD = 0x44525049, // Product
                ISFT = 0x54465349, // Software
                ISRC = 0x43525349, // Source
                ISRF = 0x46525349, // SourceForm
                ISBJ = 0x4A425349, // Subject
                ITCH = 0x48435449  // Technician
            }
        }

        private Encoding mEnc = Encoding.GetEncoding("shift-jis");
        private CK_INFO mInfo;

        public string ArchivalLocation = "";
        public string Artists = "";
        public string Category = "";
        public string Commissioned = "";
        public string Comments = "";
        public string Copyright = "";
        public string CreationDate = "";
        public string Engineer = "";
        public string Genre = "";
        public string Keywords = "";
        public string Medium = "";
        public string Name = "";
        public string Product = "";
        public string Software = "";
        public string Source = "";
        public string SourceForm = "";
        public string Subject = "";
        public string Technician = "";

        public Info() { }

        public Info(IntPtr ptr, IntPtr ptrTerm) {
            while (ptr.ToInt32() < ptrTerm.ToInt32()) {
                mInfo = (CK_INFO)Marshal.PtrToStructure(ptr, typeof(CK_INFO));
                ptr += Marshal.SizeOf<CK_INFO>();

                if (!Enum.IsDefined(typeof(CK_INFO.TYPE), mInfo.Type)) {
                    break;
                }

                var temp = new byte[mInfo.Size];
                Marshal.Copy(ptr, temp, 0, temp.Length);
                var text = mEnc.GetString(temp).Replace("\0", "");

                ptr += mInfo.Size + (2 - (mInfo.Size % 2)) % 2;

                switch (mInfo.Type) {
                case CK_INFO.TYPE.IARL:
                    ArchivalLocation = text;
                    break;
                case CK_INFO.TYPE.IART:
                    Artists = text;
                    break;
                case CK_INFO.TYPE.ICAT:
                    Category = text;
                    break;
                case CK_INFO.TYPE.ICMS:
                    Commissioned = text;
                    break;
                case CK_INFO.TYPE.ICMT:
                    Comments = text;
                    break;
                case CK_INFO.TYPE.ICOP:
                    Copyright = text;
                    break;
                case CK_INFO.TYPE.ICRD:
                    CreationDate = text;
                    break;
                case CK_INFO.TYPE.IENG:
                    Engineer = text;
                    break;
                case CK_INFO.TYPE.IGNR:
                    Genre = text;
                    break;
                case CK_INFO.TYPE.IKEY:
                    Keywords = text;
                    break;
                case CK_INFO.TYPE.IMED:
                    Medium = text;
                    break;
                case CK_INFO.TYPE.INAM:
                    Name = text;
                    break;
                case CK_INFO.TYPE.IPRD:
                    Product = text;
                    break;
                case CK_INFO.TYPE.ISFT:
                    Software = text;
                    break;
                case CK_INFO.TYPE.ISRC:
                    Source = text;
                    break;
                case CK_INFO.TYPE.ISRF:
                    SourceForm = text;
                    break;
                case CK_INFO.TYPE.ISBJ:
                    Subject = text;
                    break;
                case CK_INFO.TYPE.ITCH:
                    Technician = text;
                    break;
                }
            }
        }

        public void Write(BinaryWriter bw) {
            var msInfo = new MemoryStream();
            var bwInfo = new BinaryWriter(msInfo);
            WriteText(bwInfo, CK_INFO.TYPE.IARL, ArchivalLocation);
            WriteText(bwInfo, CK_INFO.TYPE.IART, Artists);
            WriteText(bwInfo, CK_INFO.TYPE.ICAT, Category);
            WriteText(bwInfo, CK_INFO.TYPE.ICMS, Commissioned);
            WriteText(bwInfo, CK_INFO.TYPE.ICMT, Comments);
            WriteText(bwInfo, CK_INFO.TYPE.ICOP, Copyright);
            WriteText(bwInfo, CK_INFO.TYPE.ICRD, CreationDate);
            WriteText(bwInfo, CK_INFO.TYPE.IENG, Engineer);
            WriteText(bwInfo, CK_INFO.TYPE.IGNR, Genre);
            WriteText(bwInfo, CK_INFO.TYPE.IKEY, Keywords);
            WriteText(bwInfo, CK_INFO.TYPE.IMED, Medium);
            WriteText(bwInfo, CK_INFO.TYPE.INAM, Name);
            WriteText(bwInfo, CK_INFO.TYPE.IPRD, Product);
            WriteText(bwInfo, CK_INFO.TYPE.ISFT, Software);
            WriteText(bwInfo, CK_INFO.TYPE.ISRC, Source);
            WriteText(bwInfo, CK_INFO.TYPE.ISRF, SourceForm);
            WriteText(bwInfo, CK_INFO.TYPE.ISBJ, Subject);
            WriteText(bwInfo, CK_INFO.TYPE.ITCH, Technician);

            if (0 < msInfo.Length) {
                bw.Write("LIST".ToCharArray());
                bw.Write((uint)(msInfo.Length + 4));
                bw.Write("INFO".ToCharArray());
                bw.Write(msInfo.ToArray());
            }
        }

        private void WriteText(BinaryWriter bw, CK_INFO.TYPE type, string text) {
            if (!string.IsNullOrWhiteSpace(text)) {
                var pad = 2 - (mEnc.GetBytes(text).Length % 2);
                for (int i = 0; i < pad; ++i) {
                    text += "\0";
                }

                var data = mEnc.GetBytes(text);
                bw.Write((uint)type);
                bw.Write((uint)data.Length);
                bw.Write(data);
            }
        }
    }
}
