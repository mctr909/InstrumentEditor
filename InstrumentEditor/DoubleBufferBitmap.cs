using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

public class DoubleBufferBitmap : IDisposable {
    private BufferedGraphics mBuffer;
    private Bitmap mBmp;
    private BitmapData mData;

    public DoubleBufferBitmap(Control control) {
        Dispose();

        var currentContext = BufferedGraphicsManager.Current;
        mBuffer = currentContext.Allocate(control.CreateGraphics(), control.DisplayRectangle);
        mBmp = new Bitmap(control.Width, control.Height, PixelFormat.Format32bppRgb);
    }

    ~DoubleBufferBitmap() {
        Dispose();
    }

    public void Dispose() {
        if (null != mBuffer) {
            mBuffer.Dispose();
            mBuffer = null;
        }

        if (null != mBmp) {
            mBmp.Dispose();
            mBmp = null;
        }
    }

    public void SizeChange(Control control) {
        if (null != mBuffer) {
            mBuffer.Dispose();
            mBuffer = null;
        }

        if (null != mBmp) {
            mBmp.Dispose();
            mBmp = null;
        }

        var currentContext = BufferedGraphicsManager.Current;
        mBuffer = currentContext.Allocate(control.CreateGraphics(), control.DisplayRectangle);
        mBmp = new Bitmap(control.Width, control.Height, PixelFormat.Format32bppRgb);
    }

    public void Render() {
        if (null != mBuffer && null != mData) {
            try {
                mBmp.UnlockBits(mData);
                mBuffer.Graphics.DrawImage(mBmp, 0, 0);
                mBuffer.Render();
            }
            catch { }
        }
    }

    public BitmapData BitmapData {
        get {
            Graphics.FromImage(mBmp).Clear(Color.Transparent);
            mData = mBmp.LockBits(
                new Rectangle(0, 0, mBmp.Width, mBmp.Height),
                ImageLockMode.WriteOnly,
                mBmp.PixelFormat
            );
            return mData;
        }
    }
}