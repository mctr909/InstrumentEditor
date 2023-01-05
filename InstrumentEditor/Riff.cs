﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public class Riff {
    public static readonly Encoding Enc = Encoding.GetEncoding("shift-jis");

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
                    loopInfo(ptr, chunkSize);
                } else {
                    LoadChunk(ptr, chunkId, chunkSize);
                }
            } else {
                LoadChunk(ptr, chunkId, chunkSize);
            }
            ptr += chunkSize;
        }
    }

    protected virtual void LoadChunk(IntPtr ptr, string type, long size) { }

    protected virtual void LoadInfo(IntPtr ptr, string type, string value) { }

    private void loopInfo(IntPtr ptr, long size) {
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

public static class INFO_TYPE {
    public const string IARL = "IARL"; // ArchivalLocation
    public const string IART = "IART"; // Artists
    public const string ICAT = "ICAT"; // Category
    public const string ICMS = "ICMS"; // Commissioned
    public const string ICMT = "ICMT"; // Comments
    public const string ICOP = "ICOP"; // Copyright
    public const string ICRD = "ICRD"; // CreationDate
    public const string IENG = "IENG"; // Engineer
    public const string IGNR = "IGNR"; // Genre
    public const string IKEY = "IKEY"; // Keywords
    public const string IMED = "IMED"; // Medium
    public const string INAM = "INAM"; // Name
    public const string IPRD = "IPRD"; // Product
    public const string ISFT = "ISFT"; // Software
    public const string ISRC = "ISRC"; // Source
    public const string ISRF = "ISRF"; // SourceForm
    public const string ISBJ = "ISBJ"; // Subject
    public const string ITCH = "ITCH"; // Technician
}

public class Info {
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

    public void Write(BinaryWriter bw) {
        var msInfo = new MemoryStream();
        var bwInfo = new BinaryWriter(msInfo);
        foreach (var v in mList) {
            writeText(bwInfo, v.Key, v.Value);
        }
        if (0 < msInfo.Length) {
            bw.Write("LIST".ToCharArray());
            bw.Write((uint)(msInfo.Length + 4));
            bw.Write("INFO".ToCharArray());
            bw.Write(msInfo.ToArray());
        }
    }

    private void writeText(BinaryWriter bw, string type, string text) {
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
