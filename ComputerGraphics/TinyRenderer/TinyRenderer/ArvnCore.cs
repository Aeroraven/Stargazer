﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer
{
    class ArvnCore
    {
        static public void Swap<T>(ref T a,ref T b)
        {
            T x = a;
            a = b;
            b = x;
        }
        static public void CrossProduct(float x0, float y0, float z0, float x1, float y1, float z1, out float xc, out float yc, out float zc)
        {
            xc = y0 * z1 - z0 * y1;
            yc = z0 * x1 - x0 * z1;
            zc = x0 * y1 - y0 * x1;
        }
        static public void CrossProduct(int x0, int y0, int z0, int x1, int y1, int z1, out int xc, out int yc, out int zc)
        {
            xc = y0 * z1 - z0 * y1;
            yc = z0 * x1 - x0 * z1;
            zc = x0 * y1 - y0 * x1;
        }
        static public void ToBarycentric(int xs,int ys,int x0,int y0,int x1 ,int y1,int x2,int y2,out float a,out float b,out float c)
        {
            int u, v, w;
            CrossProduct(x1 - x0, x2 - x0, x0 - xs, y1 - y0, y2 - y0, y0 - ys, out u, out v, out w);
            b = u / (float)w;
            c = v / (float)w;
            a = 1 - b - c;
        }
    }
}
