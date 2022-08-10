using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using TinyRenderer.Core;
using TinyRenderer.Core.Drawing;

namespace TinyRenderer.Display
{
    class ArvnBitmap : IArvnImage
    {
        protected int height;
        protected int width;
        Bitmap image;
        public ArvnBitmap(int w, int h)
        {
            SetWidth(w);
            SetHeight(h);
            image = new Bitmap(w, h);
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
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Set(i, j, clearColor);
                }
            }
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
