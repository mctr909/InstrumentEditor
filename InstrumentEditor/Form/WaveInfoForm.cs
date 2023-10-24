using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

using DLS;

namespace InstrumentEditor {
    unsafe public partial class WaveInfoForm : Form {
        private bool onWaveDisp;
        private bool onDragWave;
        private bool onDragLoopBegin;
        private bool onDragLoopEnd;

        private WavePlayback mWaveOut;

        private File mFile;
        private WSMP mSampler;
        private WaveLoop mLoop = new WaveLoop {
            Start = 0,
            Length = 16
        };
        private int mWaveIndex;

        private DoubleBufferBitmap mSpecBmp;
        private DoubleBufferGraphic mWaveGraph;
        private DoubleBufferGraphic mLoopGraph;

        private uint[] mColors;
        private byte[][] mSpecData;
        private float[] mWaveData;

        private float mSpecTimeDiv;
        private float mWaveTimeScale;
        private float mWaveAmp;
        private float mLoopTimeScale;
        private float mLoopAmp;

        private int mCursolPos;
        private int mDetectNote;
        private int mDetectTune;

        public WaveInfoForm(File file, int index) {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            mFile = file;
            mWaveIndex = index;
            mSampler = mFile.Wave[index].Sampler;
            if (0 < mFile.Wave[index].Loops.Count) {
                mLoop = mFile.Wave[index].Loops[0];
            }
        }

        private void WaveInfoForm_Load(object sender, EventArgs e) {
            mWaveOut = new WavePlayback();
            mWaveOut.SetValue(mFile.Wave[mWaveIndex]);

            mSpecBmp = new DoubleBufferBitmap(picSpectrum);
            mWaveGraph = new DoubleBufferGraphic(picWave, null);
            mLoopGraph = new DoubleBufferGraphic(picLoop, null);

            mDetectNote = -1;
            mDetectTune = 0;

            mWaveTimeScale = (float)Math.Pow(2.0, ((double)numWaveScale.Value - 32.0) / 4.0);
            mWaveAmp = (float)Math.Pow(2.0, ((double)(int)numWaveAmp.Value - 2) / 2.0);
            mLoopTimeScale = (float)Math.Pow(2.0, ((double)numLoopScale.Value - 32.0) / 4.0);
            mLoopAmp = (float)Math.Pow(2.0, ((double)(int)numLoopAmp.Value - 2) / 2.0);

            SetColor();
            SetPosition();
            SetData();

            SizeChange();

            lblPitch.Text = "";
            lblPitchCent.Text = "";

            timer1.Interval = 50;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void WaveInfoForm_FormClosing(object sender, FormClosingEventArgs e) {
            timer1.Stop();

            if (null != mWaveOut) {
                mWaveOut.Dispose();
            }
            if (null != mSpecBmp) {
                mSpecBmp.Dispose();
            }
            if (null != mWaveGraph) {
                mWaveGraph.Dispose();
            }
            if (null != mLoopGraph) {
                mLoopGraph.Dispose();
            }
        }

        private void WaveInfoForm_SizeChanged(object sender, EventArgs e) {
            SizeChange();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (0.0 < mWaveOut.mPitch) {
                var x = 12.0 * Math.Log(mWaveOut.mPitch / 8.1757989, 2.0);
                mDetectNote = (int)(x + 0.5);
                mDetectTune = (int)((mDetectNote - x) * 100);

                var oct = mDetectNote / 12;
                var note = mDetectNote % 12;
                if (note < 0) {
                    note += -(note / 12 - 1) * 12;
                }
                lblPitch.Text = string.Format("{0}{1}", Const.NoteName[note], oct - 2);
                lblPitchCent.Text = string.Format("{0}cent", mDetectTune);
            }

            DrawSpec();
            DrawWave();
            DrawLoop();
        }

        #region クリックイベント
        private void btnPlay_Click(object sender, EventArgs e) {
            if ("再生" == btnPlay.Text) {
                var fileLoops = mFile.Wave[mWaveIndex].Loops;
                if (0 < fileLoops.Count) {
                    mWaveOut.mLoopBegin = (int)mLoop.Start;
                    mWaveOut.mLoopEnd = (int)mLoop.Start + (int)mLoop.Length;
                } else {
                    mWaveOut.mLoopBegin = 0;
                    mWaveOut.mLoopEnd = mWaveData.Length;
                }
                mWaveOut.Play();
                btnPlay.Text = "停止";
            } else {
                mWaveOut.Stop();
                btnPlay.Text = "再生";
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e) {
            if (0 < mFile.Wave[mWaveIndex].Loops.Count) {
                mFile.Wave[mWaveIndex].Loops[0] = mLoop;
            } else {
                mFile.Wave[mWaveIndex].Loops.Add(mLoop);
            }
            btnUpdate.Enabled = false;
        }

        private void btnUpdateAutoTune_Click(object sender, EventArgs e) {
            if (0 <= mDetectNote) {
                numUnityNote.Value = mDetectNote;
                numFineTune.Value = mDetectTune;
                mFile.Wave[mWaveIndex].Sampler.UnityNote = (byte)mDetectNote;
                mFile.Wave[mWaveIndex].Sampler.FineTune = (short)mDetectTune;
            }
        }

        private void btnLoopCreate_Click(object sender, EventArgs e) {
            var fileLoops = mFile.Wave[mWaveIndex].Loops;
            if (0 < fileLoops.Count) {
                mLoop.Start = 0;
                mLoop.Length = (uint)mWaveData.Length;
                mWaveOut.mLoopBegin = 0;
                mWaveOut.mLoopEnd = mWaveData.Length;
                mFile.Wave[mWaveIndex].Loops.Clear();
                btnLoopCreate.Text = "ループ作成";
            } else {
                mLoop.Start = (uint)hsbTime.Value;
                mLoop.Length = (uint)(mWaveData.Length < 256 ? 16 : 256);
                mWaveOut.mLoopBegin = (int)mLoop.Start;
                mWaveOut.mLoopEnd = (int)mLoop.Start + (int)mLoop.Length;
                mFile.Wave[mWaveIndex].Loops.Add(mLoop);
                btnLoopCreate.Text = "ループ削除";
            }
        }
        #endregion

        #region チェンジイベント
        private void txtName_TextChanged(object sender, EventArgs e) {
            mFile.Wave[mWaveIndex].Info[Info.TYPE.INAM] = txtName.Text;
        }

        private void numWaveScale_ValueChanged(object sender, EventArgs e) {
            mWaveTimeScale = (float)Math.Pow(2.0, ((double)numWaveScale.Value - 32.0) / 4.0);
        }

        private void numWaveAmp_ValueChanged(object sender, EventArgs e) {
            mWaveAmp = (float)Math.Pow(2.0, ((double)(int)numWaveAmp.Value - 2) / 2.0);
        }

        private void numLoopScale_ValueChanged(object sender, EventArgs e) {
            mLoopTimeScale = (float)Math.Pow(2.0, ((double)numLoopScale.Value - 32.0) / 4.0);
        }

        private void numLoopAmp_ValueChanged(object sender, EventArgs e) {
            mLoopAmp = (float)Math.Pow(2.0, ((double)(int)numLoopAmp.Value - 2) / 2.0);
        }

        private void numVolume_ValueChanged(object sender, EventArgs e) {
            var gain = (int)(20 * numVolume.Value) / 400.0;
            mFile.Wave[mWaveIndex].Sampler.Gain = Math.Pow(10.0, gain);
            mWaveOut.mVolume = gain;
        }

        private void numUnityNote_ValueChanged(object sender, EventArgs e) {
            var oct = (int)numUnityNote.Value / 12 - 2;
            var note = (int)numUnityNote.Value % 12;
            lblUnityNote.Text = string.Format("{0}{1}", Const.NoteName[note], oct);
            mFile.Wave[mWaveIndex].Sampler.UnityNote = (byte)numUnityNote.Value;
        }

        private void numFineTune_ValueChanged(object sender, EventArgs e) {
            mFile.Wave[mWaveIndex].Sampler.FineTune = (short)numFineTune.Value;
        }
        #endregion

        #region ループ範囲選択
        private void picWave_MouseDown(object sender, MouseEventArgs e) {
            onDragWave = true;
        }

        private void picWave_MouseUp(object sender, MouseEventArgs e) {
            onDragWave = false;
            onDragLoopBegin = false;
            onDragLoopEnd = false;
            var fileLoops = mFile.Wave[mWaveIndex].Loops;
            if (0 < fileLoops.Count) {
                btnUpdate.Enabled
                    = fileLoops[0].Start != mLoop.Start
                    || fileLoops[0].Length != mLoop.Length;
            } else {
                btnUpdate.Enabled = true;
            }
        }

        private void picWave_MouseMove(object sender, MouseEventArgs e) {
            mCursolPos = picWave.PointToClient(Cursor.Position).X;
            var pos = hsbTime.Value + mCursolPos / mWaveTimeScale;
            if (pos < 0) {
                pos = 0;
            }

            if (onDragLoopBegin) {
                mLoop.Start = (uint)pos;
            }
            if (onDragLoopEnd) {
                if ((pos - 16) < mLoop.Start) {
                    mLoop.Length = 16;
                } else {
                    mLoop.Length = (uint)pos - mLoop.Start;
                }
            }

            mWaveOut.mLoopBegin = (int)mLoop.Start;
            mWaveOut.mLoopEnd = (int)mLoop.Start + (int)mLoop.Length;
        }

        private void picWave_MouseEnter(object sender, EventArgs e) {
            onWaveDisp = true;
        }

        private void picWave_MouseLeave(object sender, EventArgs e) {
            onWaveDisp = false;
        }
        #endregion

        private void SetColor() {
            mColors = new uint[256];
            var dColor = 1280.0 / mColors.Length;
            var vColor = 0.0;
            for (int i = 0; i < mColors.Length; ++i) {
                var r = 0;
                var g = 0;
                var b = 0;

                if (vColor < 256.0) {
                    b = (int)vColor;
                }
                else if (vColor < 512.0) {
                    b = 255;
                    g = (int)vColor - 256;
                }
                else if (vColor < 768.0) {
                    b = 255 - (int)(vColor - 512);
                    g = 255;
                }
                else if (vColor < 1024.0) {
                    g = 255;
                    r = (int)vColor - 768;
                }
                else {
                    g = 255 - (int)(vColor - 1024);
                    r = 255;
                }
                mColors[i] = (uint)((r << 16) | (g << 8) | b);
                vColor += dColor;
            }
        }

        private void SetPosition() {
            //
            picSpectrum.Height = 96;
            //
            hsbTime.Height = 19;

            //
            picWave.Height = 96;
            numWaveScale.Top = 0;
            picSpectrum.Top = numWaveScale.Top + numWaveScale.Height + 4;
            picWave.Top = picSpectrum.Top + picSpectrum.Height + 4;
            hsbTime.Top = picWave.Top + picWave.Height + 4;

            // 
            grbMain.Top = btnPlay.Top + btnPlay.Height + 6;
            grbMain.Height
                = numWaveScale.Height + 4
                + picSpectrum.Height + 4
                + picWave.Height + 4
                + hsbTime.Height + 6
            ;

            //
            picLoop.Height = 256;
            numLoopScale.Top = 0;
            picLoop.Top = numLoopScale.Top + numLoopScale.Height + 4;

            //
            grbLoop.Top = grbMain.Top + grbMain.Height + 6;
            grbLoop.Height
                = numLoopScale.Height + 4
                + picLoop.Height + 6
            ;

            Height
                = btnPlay.Height + 6
                + grbMain.Height + 6
                + grbLoop.Height + 48;
            Width = btnUpdateAutoTune.Right + 22;
        }

        private void SetData() {
            var packSize = 24;
            var wave = mFile.Wave[mWaveIndex];
            mWaveData = wave.GetFloat(packSize);
            var samples = mWaveData.Length;
            
            hsbTime.Value = 0;
            hsbTime.Maximum = samples;

            var delta = wave.Format.SampleRate / 44100.0;
            mSpecTimeDiv = 1.0f / (float)delta / packSize;
            mSpecData = new byte[(int)(samples * mSpecTimeDiv)][];

            var sp = new Spectrum(wave.Format.SampleRate, 27.5, 12, (uint)picSpectrum.Height);
            var time = 0.0;
            for (var s = 0; s < mSpecData.Length; ++s) {
                for (var i = 0; i < packSize && time < samples; ++i) {
                    sp.Filtering(mWaveData[(int)time]);
                    time += delta;
                }

                sp.SetLevel();
                var level = sp.Level;
                var amp = mColors.Length - 1;
                mSpecData[s] = new byte[sp.Banks];
                for (var b = 0; b < sp.Banks; ++b) {
                    var lv = level[b] / sp.Max;
                    mSpecData[s][b] = (byte)(1.0 < lv ? amp : (amp * lv));
                }
            }

            if (0 < wave.Loops.Count) {
                btnLoopCreate.Text = "ループ削除";
            } else {
                btnLoopCreate.Text = "ループ作成";
            }

            mWaveOut.mVolume = wave.Sampler.Gain;
            numVolume.Value = (decimal)(20 * Math.Log10(wave.Sampler.Gain));
            numUnityNote.Value = wave.Sampler.UnityNote;
            if (0 == wave.Sampler.FineTune) {
                numFineTune.Value = 0;
            } else {
                numFineTune.Value = wave.Sampler.FineTune;
            }
            txtName.Text = wave.Info[Info.TYPE.INAM];
        }

        private void SizeChange() {
            grbMain.Width = Width - grbMain.Left - 22;
            picSpectrum.Width = Width - (picSpectrum.Left + grbMain.Left) * 2 - 16;
            picWave.Width = Width - (picWave.Left + grbMain.Left) * 2 - 16;
            hsbTime.Width = Width - (hsbTime.Left + grbMain.Left) * 2 - 16;

            grbLoop.Width = Width - grbLoop.Left - 22;
            picLoop.Width = Width - (picLoop.Left + grbLoop.Left) * 2 - 16;

            if (null != mSpecBmp) {
                mSpecBmp.SizeChange(picSpectrum);
            }
            if (null != mWaveGraph) {
                mWaveGraph.SizeChange(picWave);
            }
            if (null != mLoopGraph) {
                mLoopGraph.SizeChange(picLoop);
            }
        }

        private void DrawSpec() {
            var bmpData = mSpecBmp.BitmapData;

            var height = picSpectrum.Height;
            var width = picSpectrum.Width;
            var pixO = (uint*)bmpData.Scan0.ToPointer();
            var begin = hsbTime.Value * mSpecTimeDiv;
            var delta = mSpecTimeDiv / mWaveTimeScale;
            Parallel.For(0, height - 1, y => {
                var pix = pixO + (height - y - 1) * width;
                var time = begin;
                for (var x = 0; x < width; ++x) {
                    var t1 = (int)time;
                    var t2 = t1 + 1;
                    var dt = time - t1;
                    time += delta;
                    if (mSpecData.Length <= t2) {
                        *pix = 0;
                        ++pix;
                        break;
                    }
                    if (y < mSpecData[t2].Length) {
                        var v = (int)(mSpecData[t1][y] * (1.0 - dt) + mSpecData[t2][y] * dt);
                        *pix = mColors[v];
                        ++pix;
                    }
                }
            });

            mSpecBmp.Render();
        }

        private void DrawWave() {
            var graph = mWaveGraph.Graphics;

            var green = new Pen(Color.FromArgb(0, 168, 0), 1.0f);

            var amp = picWave.Height - 1;
            var begin = hsbTime.Value;

            //
            var wave = mFile.Wave[mWaveIndex];
            if (0 < wave.Loops.Count) {
                var loopBegin = (mLoop.Start - begin) * mWaveTimeScale;
                var loopLength = mLoop.Length * mWaveTimeScale;
                var loopEnd = loopBegin + loopLength;
                graph.FillRectangle(Brushes.WhiteSmoke, loopBegin, 0, loopLength, picWave.Height);

                if (onWaveDisp && Math.Abs(mCursolPos - loopBegin) <= 8) {
                    Cursor = Cursors.SizeWE;
                    if (onDragWave && !onDragLoopEnd) {
                        onDragLoopBegin = true;
                    }
                } else if (onWaveDisp && Math.Abs(mCursolPos - loopEnd) <= 8) {
                    Cursor = Cursors.SizeWE;
                    if (onDragWave && !onDragLoopBegin) {
                        onDragLoopEnd = true;
                    }
                } else {
                    Cursor = Cursors.Default;
                }
            }

            //
            graph.DrawLine(Pens.Red, 0, picWave.Height / 2.0f - 1, picWave.Width - 1, picWave.Height / 2.0f - 1);

            //
            var x1 = 0.0f;
            var x2 = mWaveTimeScale;
            for (int t1 = begin, t2 = begin + 1; x2 < picWave.Width && t2 < mWaveData.Length; ++t1, ++t2) {
                if (t1 < 0) {
                    continue;
                }

                var w1 = mWaveAmp * mWaveData[t1];
                var w2 = mWaveAmp * mWaveData[t2];
                if (w1 < -1.0) w1 = -1.0f;
                if (w2 < -1.0) w2 = -1.0f;
                if (1.0 < w1) w1 = 1.0f;
                if (1.0 < w2) w2 = 1.0f;
                var y1 = (0.5f - 0.5f * w1) * amp;
                var y2 = (0.5f - 0.5f * w2) * amp;

                graph.DrawLine(green, x1, y1, x2, y2);
                x1 += mWaveTimeScale;
                x2 += mWaveTimeScale;
            }

            //
            mWaveGraph.Render();
        }

        private void DrawLoop() {
            var graph = mLoopGraph.Graphics;
            var green = new Pen(Color.FromArgb(0, 168, 0), 1.0f);
            var cyan = new Pen(Color.FromArgb(127, 0, 255, 255), 1.0f).Brush;

            var amp = picLoop.Height - 1;
            var halfWidth = (int)(picLoop.Width / 2.0f - 1);

            //
            var loopBegin = (int)mLoop.Start;
            var loopEnd = (int)mLoop.Start + (int)mLoop.Length;
            if (mWaveData.Length <= loopEnd) {
                loopEnd = mWaveData.Length - 1;
            }

            //
            graph.DrawLine(Pens.Red, 0, picLoop.Height / 2.0f - 1, picLoop.Width - 1, picLoop.Height / 2.0f - 1);
            graph.DrawLine(Pens.Red, halfWidth, 0, halfWidth, picLoop.Height);

            //
            float x1 = halfWidth;
            float x2 = halfWidth + mLoopTimeScale;
            for (int t1 = loopBegin, t2 = loopBegin + 1; t2 <= loopEnd; ++t1, ++t2) {
                var w1 = mLoopAmp * mWaveData[t1];
                var w2 = mLoopAmp * mWaveData[t2];
                if (w1 < -1.0) w1 = -1.0f;
                if (w2 < -1.0) w2 = -1.0f;
                if (1.0 < w1) w1 = 1.0f;
                if (1.0 < w2) w2 = 1.0f;
                var y1 = (0.5f - 0.5f * w1) * amp;
                var y2 = (0.5f - 0.5f * w2) * amp;
                graph.DrawLine(green, x1, y1, x2, y2);
                if (4 <= mLoopTimeScale) {
                    graph.FillEllipse(cyan, x1 - 2, y1 - 2, 4, 4);
                    graph.FillEllipse(cyan, x2 - 2, y2 - 2, 4, 4);
                }
                x1 += mLoopTimeScale;
                x2 += mLoopTimeScale;
            }
            //
            x1 = halfWidth;
            x2 = halfWidth - mLoopTimeScale;
            for (int t1 = loopEnd, t2 = loopEnd - 1; loopBegin <= t2; --t1, --t2) {
                var w1 = mLoopAmp * mWaveData[t1];
                var w2 = mLoopAmp * mWaveData[t2];
                if (w1 < -1.0) w1 = -1.0f;
                if (w2 < -1.0) w2 = -1.0f;
                if (1.0 < w1) w1 = 1.0f;
                if (1.0 < w2) w2 = 1.0f;
                var y1 = (0.5f - 0.5f * w1) * amp;
                var y2 = (0.5f - 0.5f * w2) * amp;
                graph.DrawLine(green, x1, y1, x2, y2);
                if (4 <= mLoopTimeScale) {
                    graph.FillEllipse(cyan, x1 - 2, y1 - 2, 4, 4);
                    graph.FillEllipse(cyan, x2 - 2, y2 - 2, 4, 4);
                }
                x1 -= mLoopTimeScale;
                x2 -= mLoopTimeScale;
            }

            //
            mLoopGraph.Render();
        }
    }
}
