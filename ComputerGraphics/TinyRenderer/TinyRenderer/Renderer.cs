using System;
using System.Collections.Generic;
using System.Text;


namespace TinyRenderer
{
    class Renderer
    {
        static public int RGBToHex(int r, int g,int b)
        {
            return (0xff << 24) |(r << 16) | (g << 8) | b;
        }
        static public int RandomColorHex()
        {
            Random rd = new Random();
            return RGBToHex(rd.Next(0, 255), rd.Next(0, 255), rd.Next(0, 255));
        }
        static public void DrawLineV1(int x0, int y0, int x1, int y1, int colorHex, ref ArvnImage target)
        {
            for(float t = 0.0f; t < 1.0f; t += 0.1f)
            {
                int x = (int)(x0 + (x1 - x0) * t);
                int y = (int)(y0 + (y1 - y0) * t);
                target.Set(x, y, colorHex);
            }
        }
        static public void DrawLineV2(int x0, int y0, int x1, int y1, int colorHex, ref ArvnImage target)
        {
            for(int i = x0; i <= x1; i++)
            {
                float t = (i - x0) / (float)(x1 - x0);
                int y = (int)(y0 * (1.0f - t) + y1 * t);
                target.Set(i, y, colorHex);
            }
        }
        static public void DrawLineV3(int x0, int y0, int x1, int y1, int colorHex, ref ArvnImage target)
        {
            bool steep = false;
            if (Math.Abs(y1 - y0) > Math.Abs(x1 - x0))
            {
                steep = true;
                int t = x1;
                x1 = y1;
                y1 = t;
                t = x0;
                x0 = y0;
                y0 = t;
            }
            if (x0 > x1)
            {
                int t = x0;
                x0 = x1;
                x1 = t;
                t = y0;
                y0 = y1;
                y1 = t;
            }

            for (int i = x0; i <= x1; i++)
            {
                float t = (i - x0) / (float)(x1 - x0);
                int y = (int)(y0 * (1.0f - t) + y1 * t);
                if (!steep)
                {
                    target.Set(i, y, colorHex);
                }
                else
                {
                    target.Set(y, i, colorHex);
                }
                
            }
        }
        static public void DrawLineV4(int x0, int y0, int x1, int y1, int colorHex, ref ArvnImage target)
        {
            bool steep = false;
            if (Math.Abs(y1 - y0) > Math.Abs(x1 - x0))
            {
                steep = true;
                ArvnCore.Swap(ref x0, ref y0);
                ArvnCore.Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                ArvnCore.Swap(ref x0, ref x1);
                ArvnCore.Swap(ref y0, ref y1);
            }
            float slope = Math.Abs((y1 - y0) / (float)(x1 - x0));
            float error = 0;
            int y = y0;
            for (int i = x0; i <= x1; i++)
            {
                if (!steep)
                {
                    target.Set(i, y, colorHex);
                }
                else
                {
                    target.Set(y, i, colorHex);
                }
                error += slope;
                if (error > 0.5)
                {
                    y += (y1 > y0) ? 1 : -1;
                    error -= 1;
                }
            }
        }
        static public void DrawLineV5(int x0, int y0, int x1, int y1, int colorHex, ref ArvnImage target)
        {
            bool steep = false;
            if (Math.Abs(y1 - y0) > Math.Abs(x1 - x0))
            {
                steep = true;
                ArvnCore.Swap(ref x0, ref y0);
                ArvnCore.Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                ArvnCore.Swap(ref x0, ref x1);
                ArvnCore.Swap(ref y0, ref y1);
            }
            int dy = y1 - y0;
            int dx = x1 - x0;
            int dy2a = Math.Abs(dy) * 2;
            int error = 0;
            int y = y0;
            int yi = (y1 > y0) ? 1 : -1;
            for (int i = x0; i <= x1; i++)
            {
                if (!steep)
                {
                    target.Set(i, y, colorHex);
                }
                else
                {
                    target.Set(y, i, colorHex);
                }
                error += dy2a;
                if (error > dx)
                {
                    y += yi;
                    error -= dx*2 ;
                }
            }
        }
        static public void DrawTriangleHollowV1(ArvnVec2i t0, ArvnVec2i t1, ArvnVec2i t2,int colorHex, ref ArvnImage target)
        {
            DrawLineV5(t0.x, t0.y, t1.x, t1.y, colorHex, ref target);
            DrawLineV5(t1.x, t1.y, t2.x, t2.y, colorHex, ref target);
            DrawLineV5(t2.x, t2.y, t0.x, t0.y, colorHex, ref target);
        }
        static public void DrawTriangleHollowV2(ArvnVec2i t0, ArvnVec2i t1, ArvnVec2i t2, int colorHex, ref ArvnImage target)
        {
            if (t0.y > t1.y) { ArvnCore.Swap(ref t0, ref t1); }
            if (t0.y > t2.y) { ArvnCore.Swap(ref t0, ref t2); }
            if (t1.y > t2.y) { ArvnCore.Swap(ref t1, ref t2); }
            DrawLineV5(t0.x, t0.y, t1.x, t1.y, RGBToHex(0, 255, 0), ref target);
            DrawLineV5(t1.x, t1.y, t2.x, t2.y, RGBToHex(0, 255, 0), ref target);
            DrawLineV5(t2.x, t2.y, t0.x, t0.y, RGBToHex(255, 0, 0), ref target);
        }
        static public void DrawTriangleHollowV3(ArvnVec2i t0, ArvnVec2i t1, ArvnVec2i t2, int colorHex, ref ArvnImage target)
        {
            if (t0.y > t1.y) { ArvnCore.Swap(ref t0, ref t1); }
            if (t0.y > t2.y) { ArvnCore.Swap(ref t0, ref t2); }
            if (t1.y > t2.y) { ArvnCore.Swap(ref t1, ref t2); }
            //Lower Part
            int totalHeight = t2.y - t0.y;
            int segHeight = t1.y - t0.y;
            for(int i = t0.y; i <= t1.y; i++)
            {
                float ah = (i - t0.y) / (float)totalHeight; //T0-T2
                float bh = (i - t0.y) / (float)segHeight; //T0-T1
                int ax = (int)(t0.x + (t2.x - t0.x) * ah);
                int bx = (int)(t0.x + (t1.x - t0.x) * bh);
                //Fill [ax,bx]
                if (ax > bx)
                {
                    ArvnCore.Swap(ref ax,ref bx);
                }
                for(int j = ax; j <= bx; j++)
                {
                    target.Set(j, i, colorHex);
                }
            }
        }
        static public void DrawTriangleHollowV4(ArvnVec2i t0, ArvnVec2i t1, ArvnVec2i t2, int colorHex, ref ArvnImage target)
        {
            if (t0.y > t1.y) { ArvnCore.Swap(ref t0, ref t1); }
            if (t0.y > t2.y) { ArvnCore.Swap(ref t0, ref t2); }
            if (t1.y > t2.y) { ArvnCore.Swap(ref t1, ref t2); }

            int totalHeight = t2.y - t0.y;
            int segHeight = t1.y - t0.y;
            int segUpHeight = t2.y - t1.y;
            for (int i = t0.y; i <= t2.y; i++)
            {
                bool upper = (i > t1.y);
                float ah = (i - t0.y) / (float)totalHeight; 
                float bh = upper?((i - t1.y) / (float)segUpHeight) :((i - t0.y) / (float)segHeight);
                int ax = (int)(t0.x + (t2.x - t0.x) * ah);
                int bx = upper?((int)(t1.x + (t2.x - t1.x) * bh)):((int)(t0.x + (t1.x - t0.x) * bh));
                if (ax > bx)
                {
                    ArvnCore.Swap(ref ax, ref bx);
                }
                for (int j = ax; j <= bx; j++)
                {
                    target.Set(j, i, colorHex);
                }
            }
        }
        static public void DrawTriangle(ArvnVec2i t0, ArvnVec2i t1, ArvnVec2i t2, int colorHex, ref ArvnImage target)
        {
            ArvnVec2i bboxMax = ArvnVec2i.Create(0, 0);
            ArvnVec2i bboxMin = ArvnVec2i.Create(target.GetWidth(), target.GetHeight());
            bboxMax.x = Math.Max(bboxMax.x, t0.x);
            bboxMax.x = Math.Max(bboxMax.x, t1.x);
            bboxMax.x = Math.Max(bboxMax.x, t2.x);
            bboxMax.x = Math.Min(bboxMax.x, target.GetWidth()-1);

            bboxMax.y = Math.Max(bboxMax.y, t0.y);
            bboxMax.y = Math.Max(bboxMax.y, t1.y);
            bboxMax.y = Math.Max(bboxMax.y, t2.y);
            bboxMax.y = Math.Min(bboxMax.y, target.GetHeight()-1);

            bboxMin.x = Math.Min(bboxMin.x, t0.x);
            bboxMin.x = Math.Min(bboxMin.x, t1.x);
            bboxMin.x = Math.Min(bboxMin.x, t2.x);
            bboxMin.x = Math.Max(bboxMin.x,0);

            bboxMin.y = Math.Min(bboxMin.y, t0.y);
            bboxMin.y = Math.Min(bboxMin.y, t1.y);
            bboxMin.y = Math.Min(bboxMin.y, t2.y);
            bboxMin.y = Math.Max(bboxMin.y, 0);

            for(int i = bboxMin.x; i <= bboxMax.x; i++)
            {
                for (int j = bboxMin.y; j <= bboxMax.y; j++)
                {
                    float ta, tb, tc;
                    ArvnCore.ToBarycentric(i, j, t0.x, t0.y, t1.x, t1.y, t2.x, t2.y, out ta, out tb,out tc);
                    if (ta >= 0 && tb >= 0 & tc >= 0)
                    {
                        target.Set(i, j, colorHex);
                    }
                }
            }
        }

        static public void DrawWireMesh(ArvnMesh model,int colorHex,ref ArvnImage target)
        {
            for(int i = 0; i < model.GetFaceNums(); i++)
            {
                int[] fidx = { 0, 0, 0 };
                float t;
                float[] dx = { 0, 0, 0 };
                float[] dy = { 0, 0, 0 };
                ArvnVec2i[] vx = { ArvnVec2i.Create(0, 0), ArvnVec2i.Create(0, 0), ArvnVec2i.Create(0, 0) };
                model.GetFace(i, out fidx[0], out fidx[1], out fidx[2]);
                for(int j = 0; j < 3; j++)
                {
                    model.GetVertex(fidx[j], out dx[j], out dy[j], out t);
                    vx[j].x = (int)((dx[j] + 1) / 2 * (target.GetWidth()-1));
                    vx[j].y = (int)((dy[j] + 1) / 2 * (target.GetWidth()-1));
                }
                DrawTriangleHollowV1(vx[0], vx[1], vx[2], colorHex, ref target);
            }
        }

        static public void DrawFlatShadingV1(ArvnMesh model, ref ArvnImage target)
        {
            for (int i = 0; i < model.GetFaceNums(); i++)
            {
                int[] fidx = { 0, 0, 0 };
                float t;
                float[] dx = { 0, 0, 0 };
                float[] dy = { 0, 0, 0 };
                ArvnVec2i[] vx = { ArvnVec2i.Create(0, 0), ArvnVec2i.Create(0, 0), ArvnVec2i.Create(0, 0) };
                model.GetFace(i, out fidx[0], out fidx[1], out fidx[2]);
                for (int j = 0; j < 3; j++)
                {
                    model.GetVertex(fidx[j], out dx[j], out dy[j], out t);
                    vx[j].x = (int)((dx[j] + 1) / 2 * (target.GetWidth() - 1));
                    vx[j].y = (int)((dy[j] + 1) / 2 * (target.GetWidth() - 1));
                }
                DrawTriangle(vx[0], vx[1], vx[2], RandomColorHex(), ref target);
            }
        }
    }
}
