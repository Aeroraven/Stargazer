using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TinyRenderer
{
    class ArvnImageBitmap : ArvnImage
    {
        Bitmap image;
        public ArvnImageBitmap(int w,int h)
        {
            SetWidth(w);
            SetHeight(h);
            image = new Bitmap(w, h);
        }
        public override void GetInNormalized(float x, float y, out float r, out float g, out float b)
        {
            int hex = GetInNormalized(x, y);
            int ir, ig, ib;
            ArvnCore.HexToRGB(hex, out ir, out ig, out ib);
            r = ir / 255f;
            g = ig / 255f;
            b = ib / 255f;
        }
        public override void Load(string path)
        {
            image = new Bitmap(path);
            SetWidth(image.Width);
            SetHeight(image.Height);
        }
        public override int Get(int x, int y)
        {
            return image.GetPixel(x, (height - 1) - y).ToArgb();
        }
        public override int GetInNormalized(float x, float y)
        {
            return Get((int)(x * (width - 1)), (int)(y * (height - 1)));
        }
        public override int GetHeight()
        {
            return image.Height;
        }

        public override int GetWidth()
        {
            return image.Width;
        }

        public override void Set(int x, int y, int hexColor)
        {
            image.SetPixel(x, (height-1)-y, Color.FromArgb(hexColor));
        }

        public override void SetHeight(int x)
        {
            height = x;
        }

        public override void SetWidth(int x)
        {
            width = x;
        }

        public override void Save(string path)
        {
            image.Save(path);
        }

        public override float[] GetInNormalizedEx(float x, float y)
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
    }
}
