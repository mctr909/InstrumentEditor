namespace InstPack {
    public enum ART_TYPE : ushort {
        GAIN = 0x0000,
        PAN = 0x0001,
        COASE_TUNE = 0x0002,
        FINE_TUNE = 0x0003,
        UNITY_KEY = 0x0004,
        LPF_RESONANCE = 0x0005,
        LPF_CUTOFF = 0x0006,

        EG_AMP_ATTACK = 0x0100,
        EG_AMP_HOLD = 0x0101,
        EG_AMP_DECAY = 0x0102,
        EG_AMP_RELEASE = 0x0103,
        EG_AMP_SUSTAIN = 0x0104,

        EG_CUTOFF_ATTACK = 0x0110,
        EG_CUTOFF_HOLD = 0x0111,
        EG_CUTOFF_DECAY = 0x0112,
        EG_CUTOFF_RELEASE = 0x0113,
        EG_CUTOFF_SUSTAIN = 0x0114,
        EG_CUTOFF_RISE = 0x0115,
        EG_CUTOFF_LEVEL = 0x0116,
        EG_CUTOFF_FALL = 0x0117
    }
}
