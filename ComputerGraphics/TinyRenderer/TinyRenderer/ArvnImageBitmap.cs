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
    }
}
