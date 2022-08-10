using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using TinyRenderer.Core;
using TinyRenderer.Core.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TinyRenderer.Display
{
    class ArvnBitmap : IArvnImage
    {
        protected int height;
        protected int width;
        protected byte[] clearbits;
        Bitmap image;
        public ArvnBitmap(int w, int h)
        {
            SetWidth(w);
            SetHeight(h);
            image = new Bitmap(w, h);
            clearbits = new byte[w * h * 4];
        }
        public void SyncFromImage()
        {

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
        public void Load(string path)
        {
            image = new Bitmap(path);
            SetWidth(image.Width);
            SetHeight(image.Height);
        }
        public void Clear(int clearColor)
        {
            for (int i = 0; i < clearbits.Length; i+=4)
            {
                clearbits[i + 3] = (byte)(clearColor >> (8 * 3));
                clearbits[i + 2] = (byte)((clearColor >> (8 * 2)) & 0xff);
                clearbits[i + 1] = (byte)((clearColor >> (8 * 1)) & 0xff);
                clearbits[i] = (byte)((clearColor >> (8 * 0)) & 0xff);
            }
            var bits = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, image.PixelFormat);
            Marshal.Copy(clearbits, 0, bits.Scan0, clearbits.Length);
            image.UnlockBits(bits);
        }
        public int Get(int x, int y)
        {
            //return imageBuffer[];
            return image.GetPixel(x, height - 1 - y).ToArgb();
        }
        public int GetInNormalized(float x, float y)
        {
            return Get((int)(x * (width - 1)), (int)(y * (height - 1)));
        }
        public int GetHeight()
        {
            return image.Height;
        }

        public int GetWidth()
        {
            return image.Width;
        }

        public void Set(int x, int y, int hexColor)
        {
            image.SetPixel( x, height - 1 - y, Color.FromArgb(hexColor));
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

        public object GetImage()
        {
            SyncFromBuf();
            return image;
        }

        public void SyncFromBuf()
        {

        }
    }
}
