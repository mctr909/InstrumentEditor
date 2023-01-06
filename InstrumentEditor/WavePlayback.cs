namespace InstrumentEditor {
    unsafe public class WavePlayback : WaveOutLib {
        public int mLoopBegin;
        public int mLoopEnd;
        public double mVolume;
        public double mPitch;

        private float[] mWave;
        private int mSampleRate;
        private double mDelta;
        private double mTime;
        private int mFftIndex;
        private FFT mFft;

        public WavePlayback() : base(44100, 1, 4096, 4) {
            mWave = new float[1];
            mFft = new FFT(16384, SampleRate);
            Stop();
        }

        public void SetValue(DLS.WAVE wave) {
            mWave = wave.GetFloat();
            mSampleRate = (int)wave.Format.SampleRate;
        }

        public void Play() {
            mDelta = (double)mSampleRate / SampleRate;
            mTime = 0.0;
            WaveOutOpen();
        }

        public void Stop() {
            WaveOutClose();
        }

        protected override void SetData() {
            for (var i = 0; i < BufferSize; i++) {
                var wave = ((int)mTime < mWave.Length) ? (mWave[(int)mTime] * mVolume) : 0.0;
                WaveBuffer[i] = (short)(wave * 32767);

                mFft.Re[mFftIndex] = wave;
                mFft.Im[mFftIndex] = 0.0;
                ++mFftIndex;
                if (mFft.Re.Length <= mFftIndex) {
                    mFftIndex = 0;
                    mPitch = mFft.Pitch();
                }

                mTime += mDelta;
                if (mLoopEnd <= mTime) {
                    mTime = mLoopBegin + mTime - mLoopEnd;
                }
            }
        }
    }
}
