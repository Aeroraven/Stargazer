﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer
{
    class ArvnCoreException : System.ApplicationException
    {
        public ArvnCoreException(string message) : base(message) { }
    }
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
        static public void ToBarycentric(int xs, int ys, int x0, int y0, int x1, int y1, int x2, int y2, out float a, out float b, out float c)
        {
            int u, v, w;
            CrossProduct(x1 - x0, x2 - x0, x0 - xs, y1 - y0, y2 - y0, y0 - ys, out u, out v, out w);
            b = u / (float)w;
            c = v / (float)w;
            a = 1 - b - c;
        }
        static public void ToBarycentric(float xs, float ys, float x0, float y0, float x1, float y1, float x2, float y2, out float a, out float b, out float c)
        {
            float u, v, w;
            CrossProduct(x1 - x0, x2 - x0, x0 - xs, y1 - y0, y2 - y0, y0 - ys, out u, out v, out w);
            b = u / (float)w;
            c = v / (float)w;
            a = 1 - b - c;
        }
        static public void GetTriangleNormal(ArvnVec3f ta, ArvnVec3f tb, ArvnVec3f tc, out float dx, out float dy,out float dz)
        {
            float vx = tc.x - ta.x;
            float vy = tc.y - ta.y;
            float vz = tc.z - ta.z;
            float ux = tb.x - ta.x;
            float uy = tb.y - ta.y;
            float uz = tb.z - ta.z;
            CrossProduct(vx, vy, vz, ux, uy, uz, out dx, out dy, out dz);
        }
        static public void NormalizeVec3f(ref float x,ref float y,ref float z)
        {
            float sf = x * x + y * y + z * z;
            sf = (float)Math.Sqrt(sf);
            x /= sf;
            y /= sf;
            z /= sf;
        }
        static public float DotProduct(float x0,float y0,float z0,float x1,float y1,float z1)
        {
            return x0 * x1 + y0 * y1 + z0 * z1;
        }
        static public void CartesianLinearTransform2D(float[,] t,float x,float y,out float ox, out float oy)
        {
            //Order: F=TX
            ox = t[0, 0] * x + t[0, 1] * y;
            oy = t[1, 0] * x + t[1, 1] * y;
        }
        static public void CartesianLinearTransform3D(float[,] t, float x, float y, float z, out float ox, out float oy,out float oz)
        {
            //Order: F=TX
            ox = t[0, 0] * x + t[0, 1] * y + t[0, 2] * z;
            oy = t[1, 0] * x + t[1, 1] * y + t[1, 2] * z;
            oz = t[2, 0] * x + t[2, 1] * y + t[2, 2] * z;
        }
        static public void HomogeneousLinearTransform2DToCartesian(float[,] t, float x, float y, float z, out float ox, out float oy)
        {
            //Order: F=TX
            ox = t[0, 0] * x + t[0, 1] * y + t[0, 2] * z;
            oy = t[1, 0] * x + t[1, 1] * y + t[1, 2] * z;
            float oz = t[2, 0] * x + t[2, 1] * y + t[2, 2] * z;
            ox /= oz;
            oy /= oz;
        }
        static public void HomogeneousLinearTransform2D(float[,] t, float x, float y, float z, out float ox, out float oy, out float oz)
        {
            //Order: F=TX
            ox = t[0, 0] * x + t[0, 1] * y + t[0, 2] * z;
            oy = t[1, 0] * x + t[1, 1] * y + t[1, 2] * z;
            oz = t[2, 0] * x + t[2, 1] * y + t[2, 2] * z;
        }
        static public void HomogeneousLinearTransform3D(float[,] t, float x, float y, float z, float w, out float ox, out float oy, out float oz, float ow)
        {
            //Order: F=TX
            ox = t[0, 0] * x + t[0, 1] * y + t[0, 2] * z + t[0, 3] * w;
            oy = t[1, 0] * x + t[1, 1] * y + t[1, 2] * z + t[1, 3] * w;
            oz = t[2, 0] * x + t[2, 1] * y + t[2, 2] * z + t[2, 3] * w;
            ow = t[3, 0] * x + t[3, 1] * y + t[3, 2] * z + t[3, 3] * w;
        }
        static public void MatrixMultiplyI(float[,] a, float[,] b, out float[,] c)
        {
            int ar = a.GetLength(0);
            int ac = a.GetLength(1);
            int br = b.GetLength(0);
            if (ac != br)
            {
                throw new ArvnCoreException("Invalid matrice pair");
            }
            int bc = b.GetLength(1);
            c = new float[ar, bc];
            for(int i = 0; i < ar; i++)
            {
                for(int j = 0; j < bc; j++)
                {
                    float v = 0;
                    for(int k = 0; k < ac; k++)
                    {
                        v += a[i, k] * b[k, j];
                    }
                    c[i, j] = v;
                }
                
            }
        }
        static public float[,] MatrixMultiplyI(float[,] a,float[,] b)
        {
            float[,] c;
            MatrixMultiplyI(a, b, out c);
            return c;
        }

        static public float[,] MatrixMultiply(params float[][,] m)
        {
            if (m.Length == 0)
            {
                throw new ArvnCoreException("No matrix is given");
            }
            float[,] v = m[0];
            for(int i = 1; i < m.Length; i++)
            {
                v = MatrixMultiplyI(v, m[i]);
            }
            return v;
        }
        
        static public float[,] ScaleTransformCartesian2D(float scale)
        {
            float[,] m = new float[2, 2];
            m[0, 0] = scale;
            m[1, 1] = scale;
            m[0, 1] = 0;
            m[1, 0] = 0;
            return m;
        }
        static public float[,] ScaleTransformHomogeneous2D(float scale)
        {
            float[,] m = new float[3, 3];
            for(int i = 0; i < 9; i++)
            {
                m[i / 3, i % 3] = 0;
            }
            m[0, 0] = scale;
            m[1, 1] = scale;
            m[2, 2] = 1;
            return m;
        }
        static public float[,] ScaleTransformCartesian3D(float scale)
        {
            float[,] m = new float[3, 3];
            for (int i = 0; i < 9; i++)
            {
                m[i / 3, i % 3] = 0;
            }
            m[0, 0] = scale;
            m[1, 1] = scale;
            m[2, 2] = scale;
            return m;
        }
        static public float[,] ScaleTransformHomogeneous3D(float scale)
        {
            float[,] m = new float[4, 4];
            for (int i = 0; i < 16; i++)
            {
                m[i / 4, i % 4] = 0;
            }
            m[0, 0] = scale;
            m[1, 1] = scale;
            m[2, 2] = scale;
            m[3, 3] = 1;
            return m;
        }
        static public float[,] RotationTransformCartesian2D(float rad)
        {
            float[,] m = new float[2, 2];
            m[0, 0] = (float)Math.Cos(rad);
            m[1, 1] = (float)Math.Cos(rad);
            m[0, 1] = (float)-Math.Sin(rad);
            m[1, 0] = (float)Math.Sin(rad);
            return m;
        }
        static public float[,] RotationTransformHomogeneous2D(float rad)
        {
            float[,] m = new float[3, 3];
            for (int i = 0; i < 9; i++)
            {
                m[i / 3, i % 3] = 0;
            }
            m[0, 0] = (float)Math.Cos(rad);
            m[1, 1] = (float)Math.Cos(rad);
            m[0, 1] = (float)-Math.Sin(rad);
            m[1, 0] = (float)Math.Sin(rad);
            m[2, 2] = 1;
            return m;
        }
        
        static public float[,] TranslationTransformHomogeneous2D(float dx,float dy)
        {
            float[,] m = new float[3, 3];
            for (int i = 0; i < 9; i++)
            {
                m[i / 3, i % 3] = 0;
            }
            m[0, 0] = 1;
            m[1, 1] = 1;
            m[0, 2] = dx;
            m[1, 2] = dy; 
            m[2, 2] = 1;
            return m;
        }
        static public float[,] RectViewportMatrix2D(float w,float h,float vw,float vh)
        {
            float[,] m = new float[3, 3];
            for (int i = 0; i < 9; i++)
            {
                m[i / 3, i % 3] = 0;
            }
            float wscale = w / 2 / vw;
            float hscale = h / 2 / vh;
            m[0, 0] = wscale;
            m[1, 1] = hscale;
            m[2, 2] = 1;
            m[0, 2] = w / 2;
            m[1, 2] = h / 2;
            return m;
        }
        static public float[,] IdentityMatrix(int order)
        {
            float[,] m = new float[order, order];
            for(int i = 0; i < order; i++)
            {
                for (int j = 0; j < order; j++)
                {
                    m[i, j] = 0;
                }
                m[i, i] = 1;
            }
            return m;
        }

    }
}
