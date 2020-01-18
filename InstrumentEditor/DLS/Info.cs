using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace DLS {
    unsafe public class INFO {
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

        public INFO() { }

        public INFO(IntPtr ptr, IntPtr endPtr) {
            while (ptr.ToInt32() < endPtr.ToInt32()) {
                mInfo = (CK_INFO)Marshal.PtrToStructure(ptr, typeof(CK_INFO));
                ptr += sizeof(CK_INFO);

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

        public byte[] Bytes {
            get {
                var ms = new MemoryStream();
                var bw = new BinaryWriter(ms);

                WriteText(bw, CK_INFO.TYPE.IARL, ArchivalLocation);
                WriteText(bw, CK_INFO.TYPE.IART, Artists);
                WriteText(bw, CK_INFO.TYPE.ICAT, Category);
                WriteText(bw, CK_INFO.TYPE.ICMS, Commissioned);
                WriteText(bw, CK_INFO.TYPE.ICMT, Comments);
                WriteText(bw, CK_INFO.TYPE.ICOP, Copyright);
                WriteText(bw, CK_INFO.TYPE.ICRD, CreationDate);
                WriteText(bw, CK_INFO.TYPE.IENG, Engineer);
                WriteText(bw, CK_INFO.TYPE.IGNR, Genre);
                WriteText(bw, CK_INFO.TYPE.IKEY, Keywords);
                WriteText(bw, CK_INFO.TYPE.IMED, Medium);
                WriteText(bw, CK_INFO.TYPE.INAM, Name);
                WriteText(bw, CK_INFO.TYPE.IPRD, Product);
                WriteText(bw, CK_INFO.TYPE.ISFT, Software);
                WriteText(bw, CK_INFO.TYPE.ISRC, Source);
                WriteText(bw, CK_INFO.TYPE.ISRF, SourceForm);
                WriteText(bw, CK_INFO.TYPE.ISBJ, Subject);
                WriteText(bw, CK_INFO.TYPE.ITCH, Technician);

                var ms2 = new MemoryStream();
                var bw2 = new BinaryWriter(ms2);
                if (0 < ms.Length) {
                    bw2.Write(new char[] { 'L', 'I', 'S', 'T' });
                    bw2.Write((uint)(ms.Length + 4));
                    bw2.Write((uint)LIST_TYPE.INFO);
                    bw2.Write(ms.ToArray());
                }

                return ms2.ToArray();
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
