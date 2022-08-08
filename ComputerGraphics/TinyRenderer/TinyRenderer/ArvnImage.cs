using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer
{
    abstract class ArvnImage
    {
        protected int height;
        protected int width;
        abstract public void Set(int x, int y, int hexColor);
        abstract public int Get(int x, int y);

        abstract public int GetInNormalized(float x, float y);

        abstract public void GetInNormalized(float x, float y,out float r,out float g, out float b);

        abstract public float[] GetInNormalizedEx(float x, float y);
        abstract public void SetHeight(int x);
        abstract public int GetHeight();
        abstract public void SetWidth(int x);
        abstract public int GetWidth();
        abstract public void Save(string path);
        abstract public void Load(string path);

    }
}
