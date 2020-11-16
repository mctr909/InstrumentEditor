using System;

public class Spectrum {
    private class BANK {
        public double a1;
        public double a2;
        public double b0;
        public double b1;
        public double b2;

        public double aDelay1;
        public double aDelay2;
        public double bDelay1;
        public double bDelay2;

        public double amplitude;
        public double attenuation;
    }

    private BANK[] mBanks;
    private double mFreqToOmega;

    private readonly double mScale;
    private readonly double mAttenuation;

    public double[] Level { get; private set; }

    public int Banks {
        get {
            return Level.Length;
        }
    }

    public double Max { get; private set; }

    public Spectrum(uint sampleRate, double baseFreq, uint octDiv, uint banks) {
        mFreqToOmega = 8.0 * Math.Atan(1.0) / sampleRate;
        mBanks = new BANK[banks];
        Level = new double[banks];
        for (uint bankNo = 0; bankNo < banks; ++bankNo) {
            mBanks[bankNo] = new BANK();
            var width = 2.0 - 9.0 * bankNo / banks;
            if (width < 0.25) {
                width = 0.25;
            }

            var omega = Math.Pow(2.0, (double)bankNo / octDiv) * baseFreq * mFreqToOmega;
            var x = Math.Log(2.0) / 2.0 * width * omega / Math.Sin(omega) / 12.0;
            var alpha = Math.Sin(omega) * (Math.Exp(x) - Math.Exp(-x)) / 2.0;
            var a0 = 1.0 + alpha;
            var bank = mBanks[bankNo];
            bank.a1 = -2.0 * Math.Cos(omega) / a0;
            bank.a2 = (1.0 - alpha) / a0;
            bank.b0 = alpha / a0;
            bank.b1 = 0.0;
            bank.b2 = -alpha / a0;
            bank.attenuation = 1.0 / width;
        }
        mScale = 32768.0;
        mAttenuation = 0.75;
    }

    public void Filtering(double input) {
        for (var i = 0; i < mBanks.Length; ++i) {
            var bank = mBanks[i];

            var output
                = bank.b0 * input
                + bank.b1 * bank.bDelay1
                + bank.b2 * bank.bDelay2
                - bank.a1 * bank.aDelay1
                - bank.a2 * bank.aDelay2
            ;

            bank.aDelay2 = bank.aDelay1;
            bank.aDelay1 = output;
            bank.bDelay2 = bank.bDelay1;
            bank.bDelay1 = input;

            bank.amplitude += output * output * bank.attenuation;
        }
    }

    public void SetLevel() {
        Max *= 1.0 - 1.0 / 1024.0;

        for (uint b = 0; b < mBanks.Length; ++b) {
            var s = 1.0 - mAttenuation * b / mBanks.Length;
            if (s < 0.01) {
                s = 0.01;
            }
            mBanks[b].amplitude *= s;

            Level[b] = mScale * mBanks[b].amplitude / s;
            if (Level[b] < 1.0) {
                Level[b] = 1.0;
            }
            Level[b] = Math.Log10(Level[b]) / Math.Log10(mScale);
            if (Max < Level[b]) {
                Max = Level[b];
            }
        }
    }
}
