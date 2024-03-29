﻿using System;

public class FFT {
    public double[] Re;
    public double[] Im;
    private double[] Window;
    private double[][] WR;
    private double[][] WI;
    private int SampleRate;

    public FFT(int length, int sampleRate) {
        Re = new double[length];
        Im = new double[length];
        SampleRate = sampleRate;
        Window = new double[length];
        for (int i = 0; i < length; ++i) {
            Window[i] = 0.6 - 0.4 * Math.Cos(2.0 * Math.PI * i / length);
        }

        {
            WR = new double[(int)Math.Log(length, 2)][];
            WI = new double[(int)Math.Log(length, 2)][];
            int i, k;
            int m, mh;
            double theta = -2.0 * Math.PI / length;
            for (m = length, mh = m >> 1, k = 0; 1 <= (mh = m >> 1); m = mh, k++) {
                WI[k] = new double[mh];
                WR[k] = new double[mh];
                for (i = 0; i < mh; ++i) {
                    WR[k][i] = Math.Cos(theta * i);
                    WI[k][i] = Math.Sin(theta * i);
                }
                theta *= 2;
            }
        }
    }

    public void Execute() {
        int N = Re.Length;
        int m, mh, i, j, k, w;
        double wr, wi, xr, xi;

        for (m = N, w=0; 1 <= (mh = m >> 1); m = mh, w++) {
            for (i = 0; i < mh; ++i) {
                wr = WR[w][i];
                wi = WI[w][i];
                for (j = i; j < N; j += m) {
                    k = j + mh;
                    xr = Re[j] - Re[k];
                    xi = Im[j] - Im[k];
                    Re[j] += Re[k];
                    Im[j] += Im[k];
                    Re[k] = wr * xr - wi * xi;
                    Im[k] = wr * xi + wi * xr;
                }
            }
        }

        i = 0;
        for (j = 1; j < N - 1; ++j) {
            for (k = N >> 1; k > (i ^= k); k >>= 1) ;
            if (j < i) {
                xr = Re[j];
                xi = Im[j];
                Re[j] = Re[i];
                Im[j] = Im[i];
                Re[i] = xr;
                Im[i] = xi;
            }
            Re[j] /= N;
            Im[j] /= N;
        }
        Re[0] /= N;
        Im[0] /= N;
        Re[j] /= N;
        Im[j] /= N;
    }

    public void IExecute() {
        int N = Re.Length;
        int m, mh, i, j, k, w;
        double wr, wi, xr, xi;

        for (m = N, w = 0; 1 <= (mh = m >> 1); m = mh, w++) {
            for (i = 0; i < mh; ++i) {
                wr = WR[w][i];
                wi = -WI[w][i];
                for (j = i; j < N; j += m) {
                    k = j + mh;
                    xr = Re[j] - Re[k];
                    xi = Im[j] - Im[k];
                    Re[j] += Re[k];
                    Im[j] += Im[k];
                    Re[k] = wr * xr - wi * xi;
                    Im[k] = wr * xi + wi * xr;
                }
            }
        }

        i = 0;
        for (j = 1; j < N - 1; ++j) {
            for (k = N >> 1; k > (i ^= k); k >>= 1) ;
            if (j < i) {
                xr = Re[j];
                xi = Im[j];
                Re[j] = Re[i];
                Im[j] = Im[i];
                Re[i] = xr;
                Im[i] = xi;
            }
        }
    }

    public void Power() {
        int N = Re.Length;
        int m, mh, i, j, k, w;
        double wr, wi, xr, xi;

        for (m = N, w=0; 1 <= (mh = m >> 1); m = mh, w++) {
            for (i = 0; i < mh; ++i) {
                wr = WR[w][i];
                wi = WI[w][i];
                for (j = i; j < N; j += m) {
                    k = j + mh;
                    xr = Re[j] - Re[k];
                    xi = Im[j] - Im[k];
                    Re[j] += Re[k];
                    Im[j] += Im[k];
                    Re[k] = wr * xr - wi * xi;
                    Im[k] = wr * xi + wi * xr;
                }
            }
        }

        i = 0;
        for (j = 1; j < N - 1; ++j) {
            for (k = N >> 1; k > (i ^= k); k >>= 1) ;
            if (j < i) {
                xr = Re[j];
                xi = Im[j];
                Re[j] = Re[i];
                Im[j] = Im[i];
                Re[i] = xr;
                Im[i] = xi;
            }
            Re[j] = (Re[j] * Re[j] + Im[j] * Im[j]) / N;
            Im[j] = 0.0;
        }
        Re[0] = (Re[0] * Re[0] + Im[0] * Im[0]) / N;
        Re[j] = (Re[j] * Re[j] + Im[j] * Im[j]) / N;
        Im[0] = 0.0;
        Im[j] = 0.0;
    }

    public void Nsdf() {
        int N = Re.Length;
        int m, mh, i, j, k, w;
        double wr, wi, xr, xi;

        //
        for (m = N, w = 0; 1 <= (mh = m >> 1); m = mh, w++) {
            for (i = 0; i < mh; ++i) {
                wr = WR[w][i];
                wi = WI[w][i];
                for (j = i; j < N; j += m) {
                    k = j + mh;
                    xr = Re[j] - Re[k];
                    xi = Im[j] - Im[k];
                    Re[j] += Re[k];
                    Im[j] += Im[k];
                    Re[k] = wr * xr - wi * xi;
                    Im[k] = wr * xi + wi * xr;
                }
            }
        }

        i = 0;
        for (j = 1; j < N - 1; ++j) {
            for (k = N >> 1; k > (i ^= k); k >>= 1) ;
            if (j < i) {
                xr = Re[j];
                xi = Im[j];
                Re[j] = Re[i];
                Im[j] = Im[i];
                Re[i] = xr;
                Im[i] = xi;
            }
            Re[j] = (Re[j] * Re[j] + Im[j] * Im[j]) / N;
            Im[j] = 0.0;
        }
        Re[0] = (Re[0] * Re[0] + Im[0] * Im[0]) / N;
        Re[j] = (Re[j] * Re[j] + Im[j] * Im[j]) / N;
        Im[0] = 0.0;
        Im[j] = 0.0;

        //
        for (m = N, w = 0; 1 <= (mh = m >> 1); m = mh, w++) {
            for (i = 0; i < mh; ++i) {
                wr = WR[w][i];
                wi = -WI[w][i];
                for (j = i; j < N; j += m) {
                    k = j + mh;
                    xr = Re[j] - Re[k];
                    xi = Im[j] - Im[k];
                    Re[j] += Re[k];
                    Im[j] += Im[k];
                    Re[k] = wr * xr - wi * xi;
                    Im[k] = wr * xi + wi * xr;
                }
            }
        }

        if (Re[0] < 0.0001) {
            Re[0] = 0.0001;
        }

        i = 0;
        for (j = 1; j < N / 2 - 1; ++j) {
            for (k = N >> 1; k > (i ^= k); k >>= 1) ;
            if (j < i) {
                xr = Re[j];
                xi = Im[j];
                Re[j] = Re[i];
                Im[j] = Im[i];
                Re[i] = xr;
                Im[i] = xi;
            }
            Re[j] /= Re[0];
        }
        Re[0] = 1.0;
    }

    public double Pitch() {
        var N = Re.Length;

        for (int i = 0; i < N; ++i) {
            Re[i] *= Window[i];
        }

        Nsdf();

        const double threshold = 0.9;
        var clipLength = 0;
        var clipCount = 0;
        var clipIndexSum = 0;
        var clipIndexBegin = 1.0;
        var clipIndexEnd = 1.0;
        for (var i = (N >> 2) - 1; 0 <= i; --i) {
            if (threshold < Re[i] * (1.0 + (double)i / N)) {
                ++clipLength;
                clipIndexSum += i;
            }
            else {
                if (0 < clipLength) {
                    clipIndexBegin = clipIndexSum / clipLength;
                    if (0 == clipCount) {
                        clipIndexEnd = clipIndexBegin;
                    }
                    ++clipCount;
                    clipLength = 0;
                    clipIndexSum = 0;
                }
            }
        }

        if (0.0 == clipCount) {
            return 0.0;
        }

        var clipIndexBase = clipIndexEnd / clipIndexBegin;
        if (clipCount < (int)clipIndexBase) {
            return SampleRate * clipIndexBase / clipIndexEnd;
        }
        else {
            return SampleRate * clipCount / clipIndexEnd;
        }
    }
}
