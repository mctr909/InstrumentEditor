using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

using DLS;
using System.Drawing;

namespace SF2 {
    #region enum
    public enum E_OPER : ushort {
        ADDRS_OFFSET_START_LSB = 0,
        ADDRS_OFFSET_END_LSB = 1,
        ADDRS_OFFSET_LOOP_START_LSB = 2,
        ADDRS_OFFSET_LOOP_END_LSB = 3,
        ADDRS_OFFSET_START_MSB = 4,
        MOD_LFO_TO_PITCH = 5,
        VIB_LFO_TO_PITCH = 6,
        MOD_ENV_TO_PITCH = 7,
        INITIAL_FILTER_FC = 8,
        INITIAL_FILTER_Q = 9,
        MOD_LFO_TO_FILTER_FC = 10,
        MOD_ENV_TO_FILTER_FC = 11,
        ADDRS_OFFSET_END_MSB = 12,
        MOD_LFO_TO_VOLUME = 13,
        UNUSED1 = 14,
        CHORUS_EFFECTS_SEND = 15,
        REVERB_EFFECTS_SEND = 16,
        PAN = 17,
        UNUSED2 = 18,
        UNUSED3 = 19,
        UNUSED4 = 20,
        DELAY_MOD_LFO = 21,
        FREQ_MOD_LFO = 22,
        DELAY_VIB_LFO = 23,
        FREQ_VIB_LFO = 24,
        DELAY_MOD_ENV = 25,
        ATTACK_MOD_ENV = 26,
        HOLD_MOD_ENV = 27,
        DECAY_MOD_ENV = 28,
        SUSTAIN_MOD_ENV = 29,
        RELEASE_MOD_ENV = 30,
        KEY_NUM_TO_MOD_ENV_HOLD = 31,
        KEY_NUM_TO_MOD_ENV_DECAY = 32,
        ENV_VOL_DELAY = 33,
        ENV_VOL_ATTACK = 34,
        ENV_VOL_HOLD = 35,
        ENV_VOL_DECAY = 36,
        ENV_VOL_SUSTAIN = 37,
        ENV_VOL_RELEASE = 38,
        KEY_NUM_TO_VOL_ENV_HOLD = 39,
        KEY_NUM_TO_VOL_ENV_DECAY = 40,
        INSTRUMENT = 41,
        RESERVED1 = 42,
        KEY_RANGE = 43,
        VEL_RANGE = 44,
        ADDRS_OFFSET_LOOP_START_MSB = 45,
        KEYNUM = 46,
        VELOCITY = 47,
        INITIAL_ATTENUATION = 48,
        RESERVED2 = 49,
        ADDRS_OFFSET_LOOP_END_MSB = 50,
        COARSE_TUNE = 51,
        FINETUNE = 52,
        SAMPLE_ID = 53,
        SAMPLE_MODES = 54,
        RESERVED3 = 55,
        SCALE_TUNING = 56,
        EXCLUSIVE_CLASS = 57,
        OVERRIDING_ROOTKEY = 58,
        UNUSED5 = 59,
        END_OPER = 60,
    };
    #endregion

    #region struct
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct PHDR {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] name;
        public ushort presetno;
        public ushort bank;
        public ushort bagIndex;
        public uint library;
        public uint genre;
        public uint morph;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct INST {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] name;
        public ushort bagIndex;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct SHDR {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] name;
        public uint start;
        public uint end;
        public uint loopstart;
        public uint loopend;
        public uint sampleRate;
        public byte originalKey;
        public sbyte correction;
        public ushort sampleLink;
        public ushort type;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct BAG {
        public ushort genIndex;
        public ushort modIndex;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct MOD {
        public ushort srcOper;
        public E_OPER destOper;
        public short modAmount;
        public ushort amtSrcOper;
        public ushort modTransOper;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct GEN {
        public E_OPER genOper;
        public short genAmount;
    };

    public struct INST_ID {
        public byte bankFlg;
        public byte bankMSB;
        public byte bankLSB;
        public byte progNum;
    }

    public struct ENVELOPE {
        public double attack;
        public double hold;
        public double decay;
        public double sustain;
        public double release;
    }
    #endregion

    public class InstRange {
        public byte keyLo;
        public byte keyHi;
        public byte velLo;
        public byte velHi;
        public int  sampleId;
        public uint waveBegin;
        public uint waveEnd;
        public Dictionary<E_OPER, double> Art = new Dictionary<E_OPER, double>();

        public InstRange() {
            keyHi = 127;
            velHi = 127;
            sampleId = -1;
        }
    }

    public class PresetRange {
        public byte keyLo;
        public byte keyHi;
        public byte velLo;
        public byte velHi;
        public Dictionary<E_OPER, double> Art = new Dictionary<E_OPER, double>();

        public PresetRange() {
            keyHi = 127;
            velHi = 127;
        }
    }

    public class Inst {
        public string Name;
        public Dictionary<E_OPER, double> GlobalArt = new Dictionary<E_OPER, double>();
        public List<InstRange> Range = new List<InstRange>();
    }

    public class Preset {
        public string Name;
        public Dictionary<E_OPER, double> GlobalArt = new Dictionary<E_OPER, double>();
        public List<PresetRange> Range = new List<PresetRange>();
    }

    public class File : Riff {
        string mPath;
        PDTA mPdta;
        SDTA mSdta;

        protected override void Initialize(out string id, List<Chunk> chunks, List<LIST> riffs) {
            id = "";
            riffs.Add(new LIST("pdta", (i) => {
            }, (ptr, size) => {
                mPdta = new PDTA(ptr, size);
            }));
            riffs.Add(new LIST("sdta", (i) => {
            }, (ptr, size) => {
                mSdta = new SDTA(ptr, size);
            }));
        }

        public File(string filePath) : base(filePath) {
            mPath = filePath;
            //OutputPresetList();
            //OutputInstList();
            //OutputSampleList();
        }

        void OutputPresetList() {
            var sw = new StreamWriter(Path.GetDirectoryName(mPath) + "\\" + Path.GetFileNameWithoutExtension(mPath) + "_preset.csv");
            foreach (var preset in mPdta.PresetList) {
                sw.Write("{0},{1},\"{2}\n{3}\"",
                    preset.Key.bankMSB,
                    preset.Key.progNum,
                    1 == preset.Key.bankFlg ? "Drum" : "Note",
                    preset.Value.Name
                );
                sw.Write(",\"Key\nLow\",\"Key\nHigh\",\"Vel\nLow\",\"Vel\nHigh\"");
                sw.WriteLine(",Gain,Pan,Half tone,Fine tune,Inst");

                sw.Write(",,,--,--,--,--");
                sw.Write(",{0},{1},{2},{3},{4}:{5}",
                    preset.Value.GlobalArt.ContainsKey(E_OPER.INITIAL_ATTENUATION)
                        ? preset.Value.GlobalArt[E_OPER.INITIAL_ATTENUATION].ToString("0.000") : "--",
                    preset.Value.GlobalArt.ContainsKey(E_OPER.PAN)
                        ? preset.Value.GlobalArt[E_OPER.PAN].ToString("0.000") : "--",
                    preset.Value.GlobalArt.ContainsKey(E_OPER.COARSE_TUNE)
                        ? preset.Value.GlobalArt[E_OPER.COARSE_TUNE].ToString("0.000") : "--",
                    preset.Value.GlobalArt.ContainsKey(E_OPER.FINETUNE)
                        ? preset.Value.GlobalArt[E_OPER.FINETUNE].ToString("0.000") : "--",
                    preset.Value.GlobalArt.ContainsKey(E_OPER.INSTRUMENT)
                        ? preset.Value.GlobalArt[E_OPER.INSTRUMENT].ToString() : "--",
                    preset.Value.GlobalArt.ContainsKey(E_OPER.INSTRUMENT)
                        ? mPdta.InstList[(int)preset.Value.GlobalArt[E_OPER.INSTRUMENT]].Name : "--"
                );
                sw.WriteLine();

                foreach (var prgn in preset.Value.Range) {
                    sw.Write(",,,{0},{1},{2},{3}",
                        prgn.keyLo, prgn.keyHi,
                        prgn.velLo, prgn.velHi
                    );
                    sw.Write(",{0},{1},{2},{3},{4}:{5}",
                        prgn.Art.ContainsKey(E_OPER.INITIAL_ATTENUATION) ? prgn.Art[E_OPER.INITIAL_ATTENUATION].ToString("0.000") : "--",
                        prgn.Art.ContainsKey(E_OPER.PAN) ? prgn.Art[E_OPER.PAN].ToString("0.000") : "--",
                        prgn.Art.ContainsKey(E_OPER.COARSE_TUNE) ? prgn.Art[E_OPER.COARSE_TUNE].ToString("0.000") : "--",
                        prgn.Art.ContainsKey(E_OPER.FINETUNE) ? prgn.Art[E_OPER.FINETUNE].ToString("0.000") : "--",
                        prgn.Art.ContainsKey(E_OPER.INSTRUMENT) ? prgn.Art[E_OPER.INSTRUMENT].ToString() : "--",
                        prgn.Art.ContainsKey(E_OPER.INSTRUMENT) ? mPdta.InstList[(int)prgn.Art[E_OPER.INSTRUMENT]].Name : "--"
                    );
                    sw.WriteLine();
                }
            }
            sw.Close();
            sw.Dispose();
        }

        void OutputInstList() {
            var sw = new StreamWriter(Path.GetDirectoryName(mPath)
                + "\\" + Path.GetFileNameWithoutExtension(mPath) + "_inst.csv");
            int instNo = 0;
            foreach (var inst in mPdta.InstList) {
                sw.Write("{0}:{1}", instNo, inst.Name);
                sw.Write(",\"Key\nLow\",\"Key\nHigh\",\"Vel\nLow\",\"Vel\nHigh\"");
                sw.Write(",Gain,Pan");
                sw.Write(",RootKey,\"Tune\nHalf tone\",\"Tune\nCent\"");
                sw.Write(",A,H,D,S,R");
                sw.WriteLine(",Sample,Offset,Length,\"Loop\nBegin\",\"Loop\nLength\"");

                sw.Write(",--,--,--,--");

                if (inst.GlobalArt.ContainsKey(E_OPER.INITIAL_ATTENUATION)) {
                    sw.Write(",{0}", inst.GlobalArt[E_OPER.INITIAL_ATTENUATION].ToString("0.000"));
                } else {
                    sw.Write(",--");
                }
                if (inst.GlobalArt.ContainsKey(E_OPER.PAN)) {
                    sw.Write(",{0}", inst.GlobalArt[E_OPER.PAN].ToString("0.000"));
                } else {
                    sw.Write(",--");
                }

                if (inst.GlobalArt.ContainsKey(E_OPER.OVERRIDING_ROOTKEY)) {
                    sw.Write(",{0}", inst.GlobalArt[E_OPER.OVERRIDING_ROOTKEY]);
                } else {
                    sw.Write(",--");
                }
                if (inst.GlobalArt.ContainsKey(E_OPER.COARSE_TUNE)) {
                    if (1.0 == inst.GlobalArt[E_OPER.COARSE_TUNE]) {
                        sw.Write(",0");
                    } else {
                        sw.Write(",{0}", (12.0 / Math.Log(2.0, inst.GlobalArt[E_OPER.COARSE_TUNE])).ToString("0.000"));
                    }
                } else {
                    sw.Write(",--");
                }
                if (inst.GlobalArt.ContainsKey(E_OPER.FINETUNE)) {
                    if (1.0 == inst.GlobalArt[E_OPER.FINETUNE]) {
                        sw.Write(",0");
                    } else {
                        sw.Write(",{0}", (1200.0 / Math.Log(2.0, inst.GlobalArt[E_OPER.FINETUNE])).ToString("0.000"));
                    }
                } else {
                    sw.Write(",--");
                }

                if (inst.GlobalArt.ContainsKey(E_OPER.ENV_VOL_ATTACK)) {
                    sw.Write(",{0}", inst.GlobalArt[E_OPER.ENV_VOL_ATTACK].ToString("0.000"));
                } else {
                    sw.Write(",--");
                }
                if (inst.GlobalArt.ContainsKey(E_OPER.ENV_VOL_HOLD)) {
                    sw.Write(",{0}", inst.GlobalArt[E_OPER.ENV_VOL_HOLD].ToString("0.000"));
                } else {
                    sw.Write(",--");
                }
                if (inst.GlobalArt.ContainsKey(E_OPER.ENV_VOL_DECAY)) {
                    sw.Write(",{0}", inst.GlobalArt[E_OPER.ENV_VOL_DECAY].ToString("0.000"));
                } else {
                    sw.Write(",--");
                }
                if (inst.GlobalArt.ContainsKey(E_OPER.ENV_VOL_SUSTAIN)) {
                    sw.Write(",{0}", inst.GlobalArt[E_OPER.ENV_VOL_SUSTAIN].ToString("0.000"));
                } else {
                    sw.Write(",--");
                }
                if (inst.GlobalArt.ContainsKey(E_OPER.ENV_VOL_RELEASE)) {
                    sw.Write(",{0}", inst.GlobalArt[E_OPER.ENV_VOL_RELEASE].ToString("0.000"));
                } else {
                    sw.Write(",--");
                }

                sw.WriteLine();

                instNo++;
                foreach (var irgn in inst.Range) {
                    var smpl = mPdta.SampleList[irgn.sampleId];
                    var smplName = Encoding.ASCII.GetString(smpl.name).Replace("\0", "").TrimEnd();
                    sw.Write(",{0},{1},{2},{3}",
                        irgn.keyLo, irgn.keyHi,
                        irgn.velLo, irgn.velHi
                    );

                    if (irgn.Art.ContainsKey(E_OPER.INITIAL_ATTENUATION)) {
                        sw.Write(",{0}", irgn.Art[E_OPER.INITIAL_ATTENUATION].ToString("0.000"));
                    } else {
                        sw.Write(",--");
                    }
                    if (irgn.Art.ContainsKey(E_OPER.PAN)) {
                        sw.Write(",{0}", irgn.Art[E_OPER.PAN].ToString("0.000"));
                    } else {
                        sw.Write(",--");
                    }

                    if (irgn.Art.ContainsKey(E_OPER.OVERRIDING_ROOTKEY)) {
                        sw.Write(",{0}", irgn.Art[E_OPER.OVERRIDING_ROOTKEY]);
                    } else {
                        sw.Write(",--");
                    }
                    if (irgn.Art.ContainsKey(E_OPER.COARSE_TUNE)) {
                        if (1.0 == irgn.Art[E_OPER.COARSE_TUNE]) {
                            sw.Write(",0");
                        } else {
                            sw.Write(",{0}", (12.0 / Math.Log(2.0, irgn.Art[E_OPER.COARSE_TUNE])).ToString("0.000"));
                        }
                    } else {
                        sw.Write(",--");
                    }
                    if (irgn.Art.ContainsKey(E_OPER.FINETUNE)) {
                        if (1.0 == irgn.Art[E_OPER.FINETUNE]) {
                            sw.Write(",0");
                        } else {
                            sw.Write(",{0}", (1200.0 / Math.Log(2.0, irgn.Art[E_OPER.FINETUNE])).ToString("0.000"));
                        }
                    } else {
                        sw.Write(",--");
                    }

                    if (irgn.Art.ContainsKey(E_OPER.ENV_VOL_ATTACK)) {
                        sw.Write(",{0}", irgn.Art[E_OPER.ENV_VOL_ATTACK].ToString("0.000"));
                    } else {
                        sw.Write(",--");
                    }
                    if (irgn.Art.ContainsKey(E_OPER.ENV_VOL_HOLD)) {
                        sw.Write(",{0}", irgn.Art[E_OPER.ENV_VOL_HOLD].ToString("0.000"));
                    } else {
                        sw.Write(",--");
                    }
                    if (irgn.Art.ContainsKey(E_OPER.ENV_VOL_DECAY)) {
                        sw.Write(",{0}", irgn.Art[E_OPER.ENV_VOL_DECAY].ToString("0.000"));
                    } else {
                        sw.Write(",--");
                    }
                    if (irgn.Art.ContainsKey(E_OPER.ENV_VOL_SUSTAIN)) {
                        sw.Write(",{0}", irgn.Art[E_OPER.ENV_VOL_SUSTAIN].ToString("0.000"));
                    } else {
                        sw.Write(",--");
                    }
                    if (irgn.Art.ContainsKey(E_OPER.ENV_VOL_RELEASE)) {
                        sw.Write(",{0}", irgn.Art[E_OPER.ENV_VOL_RELEASE].ToString("0.000"));
                    } else {
                        sw.Write(",--");
                    }

                    var waveBegin = smpl.start + irgn.waveBegin;
                    var waveEnd = smpl.end + irgn.waveEnd;
                    var waveLen = waveEnd - waveBegin + 1;

                    if (irgn.Art.ContainsKey(E_OPER.SAMPLE_MODES)) {
                        var loopBegin = smpl.loopstart - waveBegin;
                        var loopLen = smpl.loopend - smpl.loopstart + 1;
                        sw.WriteLine(",{0}:{1},0x{2},{3},{4},{5}",
                            irgn.sampleId,
                            smplName,
                            (waveBegin * 2).ToString("X8"),
                            waveEnd - waveBegin + 1,
                            loopBegin,
                            loopLen
                        );
                    } else {
                        sw.WriteLine(",{0}:{1},0x{2},{3},-,-",
                            irgn.sampleId,
                            smplName,
                            (waveBegin * 2).ToString("X8"),
                            waveLen
                        );
                    }
                }
            }
            sw.Close();
            sw.Dispose();
        }

        void OutputSampleList() {
            var sw = new StreamWriter(Path.GetDirectoryName(mPath)
                + "\\" + Path.GetFileNameWithoutExtension(mPath) + "_sample.csv");
            int no = 0;
            sw.WriteLine("Name,UnityKey,Tune,SampleRate,Type,Addr,Length");
            foreach (var sample in mPdta.SampleList) {
                sw.Write("{0}:{1}", no, Encoding.ASCII.GetString(sample.name).Replace("\0", "").TrimEnd());
                sw.Write(",{0}", sample.originalKey);
                sw.Write(",{0}", sample.correction);
                sw.Write(",{0}", sample.sampleRate);
                sw.Write(",{0}", sample.type);
                sw.Write(",0x{0}", (sample.start * 2).ToString("X8"));
                sw.Write(",{0}", sample.end - sample.start + 1);
                sw.WriteLine();
                no++;
            }
            sw.Close();
            sw.Dispose();
        }

        public DLS.File ToPack() {
            var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            var instFile = new DLS.File();

            for (var idx = 0; idx < mPdta.SampleList.Count - 1; idx++) {
                var smpl = mPdta.SampleList[idx];
                var wi = new WAVE();
                wi.Format.Tag = 1;
                wi.Format.Bits = 16;
                wi.Format.Channels = 1;
                wi.Format.SampleRate = smpl.sampleRate;
                wi.Format.BlockAlign = (ushort)(wi.Format.Bits * wi.Format.Channels >> 3);
                wi.Format.BytesPerSec = wi.Format.BlockAlign * wi.Format.SampleRate;

                if (1 == (byte)(smpl.type & 1)) {
                    var loop = new WaveLoop();
                    loop.Start = smpl.loopstart - smpl.start;
                    loop.Length = smpl.loopend - smpl.loopstart;
                    wi.Loops.Add(loop);
                }

                wi.Sampler.UnityNote = smpl.originalKey;
                wi.Sampler.Gain = 1.0;
                wi.Sampler.FineTune = 0;

                var wavePos = (int)(smpl.start * 2);
                var waveLen = (int)(smpl.end - smpl.start + 1) * 2;
                var wavePtr = Marshal.AllocHGlobal(waveLen);
                wi.Data = new byte[waveLen];
                Marshal.Copy(mSdta.Data, wavePos, wavePtr, waveLen);
                Marshal.Copy(wavePtr, wi.Data, 0, waveLen);
                Marshal.FreeHGlobal(wavePtr);

                wi.Info[Info.TYPE.INAM] = Encoding.ASCII.GetString(smpl.name).Replace("\0", "");
                wi.Info[Info.TYPE.ICRD] = now;

                instFile.Wave.Add(wi);
            }

            foreach (var sf2Inst in mPdta.InstList) {
                var inst = new INS();

                inst.Info[Info.TYPE.INAM] = sf2Inst.Name.Replace("\0", "");
                inst.Info[Info.TYPE.ICRD] = now;

                foreach (var art in sf2Inst.GlobalArt) {
                    switch (art.Key) {
                    case E_OPER.INITIAL_ATTENUATION:
                        inst.Articulations.Add(new Connection() {
                            Destination = DST_TYPE.GAIN,
                            Value = art.Value
                        });
                        break;
                    case E_OPER.PAN:
                        inst.Articulations.Add(new Connection() {
                            Destination = DST_TYPE.PAN,
                            Value = art.Value
                        });
                        break;
                    ///TODO:ART
                    //case E_OPER.COARSE_TUNE:
                    //    globalArt.Destination = ;
                    //    inst.Articulations.Add(globalArt);
                    //    break;
                    //case E_OPER.OVERRIDING_ROOTKEY:
                    //    globalArt.Type = DST_TYPE.UNITY_KEY;
                    //    inst.Articulations.Add(globalArt);
                    //    break;
                    case E_OPER.FINETUNE:
                        inst.Articulations.Add(new Connection() {
                            Destination = DST_TYPE.PITCH,
                            Value = art.Value
                        });
                        break;


                    case E_OPER.ENV_VOL_ATTACK:
                        inst.Articulations.Add(new Connection() {
                            Destination = DST_TYPE.EG1_ATTACK_TIME,
                            Value = art.Value
                        });
                        break;
                    case E_OPER.ENV_VOL_HOLD:
                        inst.Articulations.Add(new Connection() {
                            Destination = DST_TYPE.EG1_HOLD_TIME,
                            Value = art.Value
                        });
                        break;
                    case E_OPER.ENV_VOL_DECAY:
                        inst.Articulations.Add(new Connection() {
                            Destination = DST_TYPE.EG1_DECAY_TIME,
                            Value = art.Value
                        });
                        break;
                    case E_OPER.ENV_VOL_SUSTAIN:
                        inst.Articulations.Add(new Connection() {
                            Destination = DST_TYPE.EG1_SUSTAIN_LEVEL,
                            Value = art.Value
                        });
                        break;
                    case E_OPER.ENV_VOL_RELEASE:
                        inst.Articulations.Add(new Connection() {
                            Destination = DST_TYPE.EG1_RELEASE_TIME,
                            Value = art.Value
                        });
                        break;
                    }
                }

                foreach (var sf2InstRng in sf2Inst.Range) {
                    var rgn = new RGN();
                    rgn.Header.KeyLo = sf2InstRng.keyLo;
                    rgn.Header.KeyHi = sf2InstRng.keyHi;
                    rgn.Header.VelLo = sf2InstRng.velLo;
                    rgn.Header.VelHi = sf2InstRng.velHi;

                    rgn.WaveLink.TableIndex = (uint)sf2InstRng.sampleId;

                    
                    foreach (var art in sf2InstRng.Art) {
                        switch (art.Key) {
                        case E_OPER.INITIAL_ATTENUATION:
                            rgn.Articulations.Add(new Connection() {
                                Destination = DST_TYPE.GAIN,
                                Value = art.Value
                            });
                            break;
                        case E_OPER.PAN:
                            rgn.Articulations.Add(new Connection() {
                                Destination = DST_TYPE.PAN,
                                Value = art.Value
                            });
                            break;
                        ///TODO:ART
                        //    case E_OPER.COARSE_TUNE:
                        //        layerArt.Type = ART_TYPE.COASE_TUNE;
                        //        rgn.Articulations.Add(layerArt);
                        //        break;
                        //    case E_OPER.FINETUNE:
                        //        rgn.Articulations.Add(new Connection() {
                        //            Destination = DST_TYPE.PITCH,
                        //            Value = art.Value
                        //        });
                        //        break;
                        case E_OPER.OVERRIDING_ROOTKEY:
                            rgn.Sampler.UnityNote = (ushort)art.Value;
                            break;

                        case E_OPER.ENV_VOL_ATTACK:
                            rgn.Articulations.Add(new Connection() {
                                Destination = DST_TYPE.EG1_ATTACK_TIME,
                                Value = art.Value
                            });
                            break;
                        case E_OPER.ENV_VOL_HOLD:
                            rgn.Articulations.Add(new Connection() {
                                Destination = DST_TYPE.EG1_HOLD_TIME,
                                Value = art.Value
                            });
                            break;
                        case E_OPER.ENV_VOL_DECAY:
                            rgn.Articulations.Add(new Connection() {
                                Destination = DST_TYPE.EG1_DECAY_TIME,
                                Value = art.Value
                            });
                            break;
                        case E_OPER.ENV_VOL_SUSTAIN:
                            rgn.Articulations.Add(new Connection() {
                                Destination = DST_TYPE.EG1_SUSTAIN_LEVEL,
                                Value = art.Value
                            });
                            break;
                        case E_OPER.ENV_VOL_RELEASE:
                            rgn.Articulations.Add(new Connection() {
                                Destination = DST_TYPE.EG1_RELEASE_TIME,
                                Value = art.Value
                            });
                            break;
                        }
                    }

                    inst.Regions.Add(rgn);
                }

                instFile.Inst.Add(inst);
            }

            foreach (var sf2Pres in mPdta.PresetList) {
                var preset = new INS();
                preset.Locale.BankFlg = (byte)(0 < sf2Pres.Key.bankFlg ? 0x80 : 0x00);
                preset.Locale.BankMSB = sf2Pres.Key.bankMSB;
                preset.Locale.BankLSB = sf2Pres.Key.bankLSB;
                preset.Locale.ProgNum = sf2Pres.Key.progNum;

                preset.Info[Info.TYPE.INAM] = sf2Pres.Value.Name.Replace("\0", "");
                preset.Info[Info.TYPE.ICRD] = now;

                ///TODO:ART
                //foreach (var art in sf2Pres.Value.GlobalArt) {
                //    switch (art.Key) {
                //    case E_OPER.INITIAL_ATTENUATION:
                //        preset.Articulations.Add(new Connection {
                //            Destination = DST_TYPE.GAIN,
                //            Value = art.Value
                //        });
                //        break;
                //    case E_OPER.PAN:
                //        preset.Articulations.Add(new Connection {
                //            Destination = DST_TYPE.PAN,
                //            Value = art.Value
                //        });
                //        break;
                //    case E_OPER.COARSE_TUNE:
                //        globalArt.Type = ART_TYPE.COASE_TUNE;
                //        preset.Art.Add(globalArt);
                //        break;
                //    case E_OPER.FINETUNE:
                //        preset.Articulations.Add(new Connection {
                //            Destination = DST_TYPE.PITCH,
                //            Value = art.Value
                //        });
                //        break;
                //    case E_OPER.INSTRUMENT:
                //        break;
                //    }
                //}

                foreach (var sf2PresRng in sf2Pres.Value.Range) {
                    var rgn = new RGN();
                    rgn.Header.KeyLo = sf2PresRng.keyLo;
                    rgn.Header.KeyHi = sf2PresRng.keyHi;
                    rgn.Header.VelLo = sf2PresRng.velLo;
                    rgn.Header.VelHi = sf2PresRng.velHi;

                    foreach(var art in sf2PresRng.Art) {
                        switch (art.Key) {
                        case E_OPER.INITIAL_ATTENUATION:
                            rgn.Articulations.Add(new Connection {
                                Destination = DST_TYPE.GAIN,
                                Value = art.Value
                            });
                            break;
                        case E_OPER.PAN:
                            rgn.Articulations.Add(new Connection {
                                Destination = DST_TYPE.PAN,
                                Value = art.Value
                            });
                            break;
                        case E_OPER.COARSE_TUNE:
                            ///TODO:ART
                            //layerArt.Type = ART_TYPE.COASE_TUNE;
                            //layer.Art.Add(layerArt);
                            break;
                        case E_OPER.FINETUNE:
                            rgn.Articulations.Add(new Connection {
                                Destination = DST_TYPE.PITCH,
                                Value = art.Value
                            });
                            break;
                        case E_OPER.INSTRUMENT:
                            ///TODO:RGN
                            //rgn.InstIndex = (int)art.Value;
                            break;
                        }
                    }
                    preset.Regions.Add(rgn);
                }
                instFile.Inst.Add(preset);
            }

            return instFile;
        }
    }

    public class PDTA : Riff {
        public Dictionary<INST_ID, Preset> PresetList = new Dictionary<INST_ID, Preset>();
        public List<Inst> InstList = new List<Inst>();
        public List<SHDR> SampleList = new List<SHDR>();

        List<PHDR> mPHDR = new List<PHDR>();
        List<BAG> mPBAG = new List<BAG>();
        List<MOD> mPMOD = new List<MOD>();
        List<GEN> mPGEN = new List<GEN>();
        List<INST> mINST = new List<INST>();
        List<BAG> mIBAG = new List<BAG>();
        List<MOD> mIMOD = new List<MOD>();
        List<GEN> mIGEN = new List<GEN>();

        protected override void Initialize(out string id, List<Chunk> chunks, List<LIST> riffs) {
            id = "pdta";
            chunks.Add(new Chunk("phdr", (i) => {
            }, (i) => {
                i.Read(mPHDR);
            }));
            chunks.Add(new Chunk("pbag", (i) => {
            }, (i) => {
                i.Read(mPBAG);
            }));
            chunks.Add(new Chunk("pmod", (i) => {
            }, (i) => {
                i.Read(mPMOD);
            }));
            chunks.Add(new Chunk("pgen", (i) => {
            }, (i) => {
                i.Read(mPGEN);
            }));
            chunks.Add(new Chunk("inst", (i) => {
            }, (i) => {
                i.Read(mINST);
            }));
            chunks.Add(new Chunk("ibag", (i) => {
            }, (i) => {
                i.Read(mIBAG);
            }));
            chunks.Add(new Chunk("imod", (i) => {
            }, (i) => {
                i.Read(mIMOD);
            }));
            chunks.Add(new Chunk("igen", (i) => {
            }, (i) => {
                i.Read(mIGEN);
            }));
            chunks.Add(new Chunk("shdr", (i) => {
            }, (i) => {
                i.Read(SampleList);
            }));
        }

        public PDTA(IntPtr ptr, long size) : base(ptr, size) {
            SetPresetList();
            SetInstList();
        }

        void SetPresetList() {
            for (int i = 0; i < mPHDR.Count; i++) {
                var phdr = mPHDR[i];
                int bagCount;
                if (i < mPHDR.Count - 1) {
                    bagCount = mPHDR[i + 1].bagIndex - phdr.bagIndex;
                } else {
                    bagCount = mPBAG.Count - phdr.bagIndex;
                }

                var preset = new Preset {
                    Name = Encoding.ASCII.GetString(phdr.name).Replace("\0", "").TrimEnd()
                };

                for (int ib = 0, bagIdx = phdr.bagIndex; ib < bagCount; ib++, bagIdx++) {
                    var bag = mPBAG[bagIdx];
                    int genCount;
                    if (bagIdx < mPBAG.Count - 1) {
                        genCount = mPBAG[bagIdx + 1].genIndex - bag.genIndex;
                    } else {
                        genCount = mPGEN.Count - bag.genIndex;
                    }

                    var range = new PresetRange();
                    for (int j = 0, genIdx = bag.genIndex; j < genCount; j++, genIdx++) {
                        var gen = mPGEN[genIdx];
                        switch (gen.genOper) {
                        case E_OPER.KEY_RANGE:
                            range.keyLo = (byte)(gen.genAmount & 0x7F);
                            range.keyHi = (byte)((gen.genAmount >> 8) & 0x7F);
                            if (range.keyLo == 0 && range.keyHi == 0) {
                                range.keyHi = 127;
                            }
                            break;
                        case E_OPER.VEL_RANGE:
                            range.velLo = (byte)(gen.genAmount & 0x7F);
                            range.velHi = (byte)((gen.genAmount >> 8) & 0x7F);
                            if (range.velLo == 0 && range.velHi == 0) {
                                range.velHi = 127;
                            }
                            break;
                        case E_OPER.INSTRUMENT:
                            range.Art.Add(gen.genOper, gen.genAmount);
                            break;

                        case E_OPER.INITIAL_ATTENUATION:
                            range.Art.Add(gen.genOper, Math.Pow(10.0, -gen.genAmount / 200.0));
                            break;
                        case E_OPER.PAN:
                            range.Art.Add(gen.genOper, gen.genAmount / 500.0);
                            break;
                        case E_OPER.COARSE_TUNE:
                            range.Art.Add(gen.genOper, Math.Pow(2.0, gen.genAmount / 120.0));
                            break;
                        case E_OPER.FINETUNE:
                            range.Art.Add(gen.genOper, Math.Pow(2.0, gen.genAmount / 1200.0));
                            break;
                        default:
                            break;
                        }
                    }

                    if (range.Art.ContainsKey(E_OPER.INSTRUMENT)) {
                        preset.Range.Add(range);
                    } else {
                        preset.GlobalArt = range.Art;
                    }
                }

                var id = new INST_ID {
                    bankFlg = (byte)(0 < (phdr.bank & 0x80) ? 1 : 0),
                    bankMSB = (byte)(phdr.bank & 0x7F),
                    progNum = (byte)phdr.presetno
                };
                if (!PresetList.ContainsKey(id)) {
                    PresetList.Add(id, preset);
                }
            }
            mPHDR.Clear();
            mPBAG.Clear();
            mPGEN.Clear();
            mPMOD.Clear();
        }

        void SetInstList() {
            for (int i = 0; i < mINST.Count; i++) {
                var inst = mINST[i];
                int bagCount;
                if (i < mINST.Count - 1) {
                    bagCount = mINST[i + 1].bagIndex - inst.bagIndex;
                } else {
                    bagCount = mIBAG.Count - inst.bagIndex;
                }

                var instrument = new Inst {
                    Name = Encoding.ASCII.GetString(inst.name).Replace("\0", "").TrimEnd()
                };

                for (int ib = 0, bagIdx = inst.bagIndex; ib < bagCount; ib++, bagIdx++) {
                    var bag = mIBAG[bagIdx];
                    int genCount;
                    if (bagIdx < mIBAG.Count - 1) {
                        genCount = mIBAG[bagIdx + 1].genIndex - bag.genIndex;
                    } else {
                        genCount = mIGEN.Count - bag.genIndex;
                    }

                    var range = new InstRange();
                    for (int j = 0, genIdx = bag.genIndex; j < genCount; j++, genIdx++) {
                        var gen = mIGEN[genIdx];
                        switch (gen.genOper) {
                        case E_OPER.KEY_RANGE:
                            range.keyLo = (byte)(gen.genAmount & 0x7F);
                            range.keyHi = (byte)((gen.genAmount >> 8) & 0x7F);
                            if (range.keyLo == 0 && range.keyHi == 0) {
                                range.keyHi = 127;
                            }
                            break;
                        case E_OPER.VEL_RANGE:
                            range.velLo = (byte)(gen.genAmount & 0x7F);
                            range.velHi = (byte)((gen.genAmount >> 8) & 0x7F);
                            if (range.velLo == 0 && range.velHi == 0) {
                                range.velHi = 127;
                            }
                            break;

                        case E_OPER.SAMPLE_ID:
                            range.sampleId = gen.genAmount;
                            break;
                        case E_OPER.ADDRS_OFFSET_START_LSB:
                            range.waveBegin |= (ushort)gen.genAmount;
                            break;
                        case E_OPER.ADDRS_OFFSET_START_MSB:
                            range.waveBegin |= (uint)((ushort)gen.genAmount << 16);
                            break;
                        case E_OPER.ADDRS_OFFSET_END_LSB:
                            range.waveEnd |= (ushort)gen.genAmount;
                            break;
                        case E_OPER.ADDRS_OFFSET_END_MSB:
                            range.waveEnd |= (uint)((ushort)gen.genAmount << 16);
                            break;

                        case E_OPER.INITIAL_ATTENUATION:
                            range.Art.Add(gen.genOper, Math.Pow(10.0, -gen.genAmount / 200.0));
                            break;
                        case E_OPER.PAN:
                            range.Art.Add(gen.genOper, gen.genAmount / 500.0);
                            break;
                        case E_OPER.OVERRIDING_ROOTKEY:
                            range.Art.Add(gen.genOper, gen.genAmount);
                            break;
                        case E_OPER.COARSE_TUNE:
                            range.Art.Add(gen.genOper, Math.Pow(2.0, gen.genAmount / 120.0));
                            break;
                        case E_OPER.FINETUNE:
                            range.Art.Add(gen.genOper, Math.Pow(2.0, gen.genAmount / 1200.0));
                            break;
                        case E_OPER.SAMPLE_MODES:
                            range.Art.Add(gen.genOper, gen.genAmount & 1);
                            break;

                        case E_OPER.ENV_VOL_ATTACK:
                            range.Art.Add(gen.genOper, Math.Pow(2.0, gen.genAmount / 1200.0));
                            break;
                        case E_OPER.ENV_VOL_HOLD:
                            range.Art.Add(gen.genOper, Math.Pow(2.0, gen.genAmount / 1200.0));
                            break;
                        case E_OPER.ENV_VOL_DECAY:
                            range.Art.Add(gen.genOper, Math.Pow(2.0, gen.genAmount / 1200.0));
                            break;
                        case E_OPER.ENV_VOL_SUSTAIN:
                            range.Art.Add(gen.genOper, Math.Pow(10.0, -gen.genAmount / 200.0));
                            break;
                        case E_OPER.ENV_VOL_RELEASE:
                            range.Art.Add(gen.genOper, Math.Pow(2.0, gen.genAmount / 1200.0));
                            break;
                        default:
                            break;
                        }
                    }

                    if (range.sampleId < 0) {
                        instrument.GlobalArt = range.Art;
                    } else {
                        instrument.Range.Add(range);
                    }
                }
                InstList.Add(instrument);
            }
            mINST.Clear();
            mIBAG.Clear();
            mIGEN.Clear();
            mIMOD.Clear();
        }
    }

    public class SDTA : Riff {
        public byte[] Data;

        protected override void Initialize(out string id, List<Chunk> chunks, List<LIST> riffs) {
            id = "sdta";
            chunks.Add(new Chunk("smpl", (i) => {
            }, (i) => {
                i.Read(out Data);
            }));
        }

        public SDTA(IntPtr ptr, long size) : base(ptr, size) { }
	}
}
