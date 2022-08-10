using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using TinyRenderer.Core;

namespace TinyRenderer.Display
{
    class ArvnImageBitmap : IArvnImage
    {
        protected int height;
        protected int width;
        Bitmap image;
        int[,] imageBuffer;
        public ArvnImageBitmap(int w, int h)
        {
            SetWidth(w);
            SetHeight(h);
            image = new Bitmap(w, h);
            imageBuffer = new int[w, h];
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
            imageBuffer = new int[image.Width, image.Height];
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    imageBuffer[i, j] = image.GetPixel(i, j).ToArgb();
                }
            }
        }
        public int Get(int x, int y)
        {
            return imageBuffer[x, height - 1 - y];
            //return image.GetPixel().ToArgb();
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
            imageBuffer[x, height - 1 - y] = hexColor;
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
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    image.SetPixel(i, j, Color.FromArgb(imageBuffer[i, j]));
                }
            }
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
    }
}
