using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using TinyRenderer.Core;
using TinyRenderer.Core.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;

namespace TinyRenderer.Display
{
    class ArvnBufferedBitmap : IArvnImage
    {
        protected int height;
        protected int width;
        Bitmap image;
        protected byte[] imageBuffer;

        //Clear buffer
        int lastClearColor = 0;
        protected byte[] clearImageBuffer;

        public ArvnBufferedBitmap(int w, int h)
        {
            SetWidth(w);
            SetHeight(h);
            image = new Bitmap(w, h);
            imageBuffer = new byte[w * h * 4];
            clearImageBuffer = new byte[w * h * 4];
        }
        public void GetInNormalized(float x, float y, out float r, out float g, out float b)
        {
            int hex = GetInNormalized(x, y);
            int ir, ig, ib;
            ArvnCore.HexToRGB(hex, out ir, out ig, out ib);
            r = ir / 255f;
            g = ig / 255f;
            b = ib / 255f;
        }
        public void SyncFromImage()
        {
            var bits = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, image.PixelFormat);
            Marshal.Copy(bits.Scan0, imageBuffer, 0, imageBuffer.Length);
            image.UnlockBits(bits);
        }
        public void Load(string path)
        {
            var imagew = new Bitmap(path);
            SetWidth(imagew.Width);
            SetHeight(imagew.Height);
            image = new Bitmap(width, height);
            var p = Graphics.FromImage(image);
            p.DrawImage(imagew, Point.Empty);
            imageBuffer = new byte[image.Width * image.Height * 4];
            clearImageBuffer = new byte[image.Width * image.Height * 4];
            SyncFromImage();
        }
        public int Get(int x, int y)
        {
            int i = (x + (height - 1 - y) * width) * 4;
            int a = imageBuffer[i + 3];
            int r = imageBuffer[i + 2];
            int g = imageBuffer[i + 1];
            int b = imageBuffer[i + 0];
            return ((a << 24) | (r << 16) | (g << 8) | (b));
            //return image.GetPixel().ToArgb();
        }
        public int GetInNormalized(float x, float y)
        {
            return Get((int)(x * (width - 1)), (int)(y * (height - 1)));
        }
        public int GetHeight()
        {
            return height;
        }

        public int GetWidth()
        {
            return width;
        }

        public void Set(int x, int y, int hexColor)
        {
            int i = (x + (height - 1 - y) * width) * 4;
            imageBuffer[i + 3] = (byte)(hexColor >> (8 * 3));
            imageBuffer[i + 2] = (byte)((hexColor >> (8 * 2)) & 0xff);
            imageBuffer[i + 1] = (byte)((hexColor >> (8 * 1)) & 0xff);
            imageBuffer[i] = (byte)((hexColor >> (8 * 0)) & 0xff);

            //imageBuffer[x, height - 1 - y] = hexColor;
            //image.SetPixel(x, );
        }

        public void SetHeight(int x)
        {
            height = x;
        }

        public void SetWidth(int x)
        {
            width = x;
        }

        public void Save(string path)
        {
            SyncFromBuf();
            image.Save(path);
        }

        public float[] GetInNormalizedEx(float x, float y)
        {
            float r, g, b;
            int hex = GetInNormalized(x, y);
            int ir, ig, ib;
            ArvnCore.HexToRGB(hex, out ir, out ig, out ib);
            r = ir / 255f;
            g = ig / 255f;
            b = ib / 255f;
            return new float[] { r, g, b };
        }
        public void Clear(int clearColor)
        {
            if (clearColor != lastClearColor)
            {
                lastClearColor = clearColor;
                for (int i = 0; i < clearImageBuffer.Length; i += 4)
                {
                    clearImageBuffer[i + 3] = (byte)(clearColor >> (8 * 3));
                    clearImageBuffer[i + 2] = (byte)((clearColor >> (8 * 2)) & 0xff);
                    clearImageBuffer[i + 1] = (byte)((clearColor >> (8 * 1)) & 0xff);
                    clearImageBuffer[i] = (byte)((clearColor >> (8 * 0)) & 0xff);
                }
            }
            Buffer.BlockCopy(clearImageBuffer, 0, imageBuffer, 0, clearImageBuffer.Length);
            var bits = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, image.PixelFormat);
            Marshal.Copy(imageBuffer, 0, bits.Scan0, imageBuffer.Length);
            image.UnlockBits(bits);
        }

        public object GetImage()
        {
            SyncFromBuf();
            return image;
        }

        public void SyncFromBuf()
        {
            var bits = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, image.PixelFormat);
            Marshal.Copy(imageBuffer, 0, bits.Scan0, imageBuffer.Length);
            image.UnlockBits(bits);
        }
    }
}
