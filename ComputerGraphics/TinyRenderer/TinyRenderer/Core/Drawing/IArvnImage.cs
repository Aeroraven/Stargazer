using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer.Core.Drawing
{
    interface IArvnImage
    {

        void Set(int x, int y, int hexColor);
        int Get(int x, int y);

        int GetInNormalized(float x, float y);

        void GetInNormalized(float x, float y, out float r, out float g, out float b);

        float[] GetInNormalizedEx(float x, float y);
        void SetHeight(int x);
        int GetHeight();
        void SetWidth(int x);
        int GetWidth();
        void Save(string path);
        void Load(string path);
        object GetImage();
        void SyncFromBuf();

        void Clear(int clearColor);
        void SyncFromImage();

    }
}
