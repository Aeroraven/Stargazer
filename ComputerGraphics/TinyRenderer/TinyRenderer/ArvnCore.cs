using System;
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
        static public void Swap<T>(ref T a, ref T b)
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
        static public void GetTriangleNormal(ArvnVec3f ta, ArvnVec3f tb, ArvnVec3f tc, out float dx, out float dy, out float dz)
        {
            float vx = tc.x - ta.x;
            float vy = tc.y - ta.y;
            float vz = tc.z - ta.z;
            float ux = tb.x - ta.x;
            float uy = tb.y - ta.y;
            float uz = tb.z - ta.z;
            CrossProduct(vx, vy, vz, ux, uy, uz, out dx, out dy, out dz);
        }
        static public void NormalizeVec3f(ref float x, ref float y, ref float z)
        {
            float sf = x * x + y * y + z * z;
            sf = (float)Math.Sqrt(sf);
            x /= sf;
            y /= sf;
            z /= sf;
        }
        static public float DotProduct(float x0, float y0, float z0, float x1, float y1, float z1)
        {
            return x0 * x1 + y0 * y1 + z0 * z1;
        }
        static public float DotProduct(float[] a, float[] b)
        {
            float ret = 0f;
            for (int i = 0; i < a.Length; i++)
            {
                ret += a[i] * b[i];
            }
            return ret;
        }
        static public void CartesianLinearTransform2D(float[,] t, float x, float y, out float[] o)
        {
            o = new float[2];
            CartesianLinearTransform2D(t, x, y, out o[0], out o[1]);
        }


        static public void CartesianLinearTransform2D(float[,] t, float x, float y, out float ox, out float oy)
        {
            //Order: F=TX
            ox = t[0, 0] * x + t[0, 1] * y;
            oy = t[1, 0] * x + t[1, 1] * y;
        }
        static public void CartesianLinearTransform3D(float[,] t, float x, float y, float z, out float ox, out float oy, out float oz)
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
        static public void HomogeneousLinearTransform3D(float[,] t, float x, float y, float z, float w, out float ox, out float oy, out float oz, out float ow)
        {
            //Order: F=TX
            ox = t[0, 0] * x + t[0, 1] * y + t[0, 2] * z + t[0, 3] * w;
            oy = t[1, 0] * x + t[1, 1] * y + t[1, 2] * z + t[1, 3] * w;
            oz = t[2, 0] * x + t[2, 1] * y + t[2, 2] * z + t[2, 3] * w;
            ow = t[3, 0] * x + t[3, 1] * y + t[3, 2] * z + t[3, 3] * w;
        }
        static public void HomogeneousLinearTransform3DToCartesian(float[,] t, float x, float y, float z, float w, out float ox, out float oy, out float oz)
        {
            //Order: F=TX
            ox = t[0, 0] * x + t[0, 1] * y + t[0, 2] * z + t[0, 3] * w;
            oy = t[1, 0] * x + t[1, 1] * y + t[1, 2] * z + t[1, 3] * w;
            oz = t[2, 0] * x + t[2, 1] * y + t[2, 2] * z + t[2, 3] * w;
            float ow = t[3, 0] * x + t[3, 1] * y + t[3, 2] * z + t[3, 3] * w;
            if (Math.Abs(ow) < float.Epsilon)
            {
                throw new ArvnCoreException("Divide by zero");
            }
            ox /= ow;
            oy /= ow;
            oz /= ow;
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
            for (int i = 0; i < ar; i++)
            {
                for (int j = 0; j < bc; j++)
                {
                    float v = 0;
                    for (int k = 0; k < ac; k++)
                    {
                        v += a[i, k] * b[k, j];
                    }
                    c[i, j] = v;
                }

            }
        }
        static public float[,] MatrixMultiplyI(float[,] a, float[,] b)
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
            for (int i = 1; i < m.Length; i++)
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
            for (int i = 0; i < 9; i++)
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

        static public float[,] TranslationTransformHomogeneous2D(float dx, float dy)
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
        static public float[,] RectViewportMatrix2D(float w, float h, float vw, float vh)
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
        static public float[,] RectViewportMatrix3D(float w, float h, float vw, float vh)
        {
            float[,] m = new float[4, 4];
            for (int i = 0; i < 16; i++)
            {
                m[i / 4, i % 4] = 0;
            }
            float wscale = w / 2 / vw;
            float hscale = h / 2 / vh;
            m[0, 0] = wscale;
            m[1, 1] = hscale;
            m[2, 2] = 1;
            m[3, 3] = 1;
            m[0, 3] = w / 2;
            m[1, 3] = h / 2;
            return m;
        }
        static public float[,] IdentityMatrix(int order)
        {
            float[,] m = new float[order, order];
            for (int i = 0; i < order; i++)
            {
                for (int j = 0; j < order; j++)
                {
                    m[i, j] = 0;
                }
                m[i, i] = 1;
            }
            return m;
        }
        static public void HexToRGB(int hex, out int r, out int g, out int b)
        {
            r = (int)((((0xff << 16) & hex) >> 16));
            g = (int)((((0xff << 8) & hex) >> 8));
            b = (int)((((0xff << 0) & hex) >> 0));
        }
        static public int RGBToHex(int r, int g, int b)
        {
            if (r > 255 || g > 255 || b > 255 || r < 0 || g < 0 || b < 0)
            {
                throw new ArvnCoreException("Invalid range");
            }
            return (0xff << 24) | (r << 16) | (g << 8) | b;
        }
        static public int RGBScale(int hex, float modf)
        {
            int r = (int)((((0xff << 16) & hex) >> 16) * modf);
            int g = (int)((((0xff << 8) & hex) >> 8) * modf);
            int b = (int)((((0xff << 0) & hex) >> 0) * modf);
            return RGBToHex(r, g, b);

        }
        static public int RandomColorHex()
        {
            Random rd = new Random();
            return RGBToHex(rd.Next(0, 255), rd.Next(0, 255), rd.Next(0, 255));
        }

        static public float[] Normalize(float[] vec)
        {
            float[] nvec = new float[vec.Length];
            float sq = 0;
            for (int i = 0; i < nvec.Length; i++)
            {
                sq += vec[i] * vec[i];
            }
            sq = (float)Math.Sqrt(sq);
            if (sq < float.Epsilon)
            {
                throw new ArvnCoreException("Divide by zero");
            }
            for (int i = 0; i < nvec.Length; i++)
            {
                nvec[i] = vec[i] / sq;
            }
            if (float.IsNaN(nvec[0]))
            {
                throw new Exception("Error");
            }
            return nvec;
        }
        static public void NormalizeSelf(ref float[] vec)
        {
            float sq = 0;
            for (int i = 0; i < vec.Length; i++)
            {
                sq += vec[i] * vec[i];
            }
            sq = (float)Math.Sqrt(sq);
            if (sq < float.Epsilon)
            {
                throw new ArvnCoreException("Divide by zero");
            }
            for (int i = 0; i < vec.Length; i++)
            {
                vec[i] = vec[i] / sq;
            }
            if (float.IsNaN(vec[0]))
            {
                throw new Exception("Error");
            }
        }
        static public float[] CrossProduct(float[] a, float[] b)
        {
            float[] c = new float[3];
            CrossProduct(a[0], a[1], a[2], b[0], b[1], b[2], out c[0], out c[1], out c[2]);
            return c;
        }
        static public float[] VectorMinus(float[] a, float[] b)
        {
            float[] ret = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                ret[i] = a[i] - b[i];
            }
            return ret;
        }
        static public float[,] LookAt(float[] eye, float[] center, float[] up)
        {
            float[] z = Normalize(VectorMinus(eye, center));
            float[] x = Normalize(CrossProduct(up, z));
            float[] y = Normalize(CrossProduct(z, x));
            float[,] m = IdentityMatrix(4);
            float[,] t = IdentityMatrix(4);
            for (int i = 0; i < 3; i++)
            {
                m[0, i] = x[i];
                m[1, i] = y[i];
                m[2, i] = z[i];
                t[i, 3] = -eye[i];
            }
            float[,] mv = MatrixMultiply(m, t);
            return mv;
        }
        static public float[,] PerspectiveMatrix(float fovy, float aspect, float zNear, float zFar)
        {
            float half = fovy / 2;
            float tanfov = (float)Math.Tan(half);
            float top = tanfov * zNear;
            float a = (zNear + zFar) / (zFar - zNear);
            float b = 2 * zNear * zFar / (zFar - zNear);
            float tx = 1 / (aspect * tanfov);
            float ty = 1 / (tanfov);
            float[,] pp = IdentityMatrix(4);
            pp[0, 0] = tx;
            pp[1, 1] = ty;
            pp[2, 2] = a;
            pp[2, 3] = b;
            pp[3, 2] = -1;
            pp[3, 3] = 0;
            return pp;
        }
        static public float Determinant(float[,] x)
        {
            //Pos
            int o = x.GetLength(0);
            if (o == 1)
            {
                return x[0, 0];
            }
            if (o == 2)
            {
                return x[0, 0] * x[1, 1] - x[1, 0] * x[0, 1];
            }
            if (o == 3)
            {
                float p = 1, pp = 0;
                float n = 1, nn = 0;
                for (int j = 0; j < o; j++)
                {
                    for (int i = 0; i < o; i++)
                    {
                        p *= x[i, (i + j) % o];
                    }
                    pp += p;
                    p = 1;
                }
                for (int j = 0; j < o; j++)
                {
                    for (int i = 0; i < o; i++)
                    {
                        n *= x[i, (j - i + o) % o];
                    }
                    nn += n;
                    n = 1;
                }
                return pp - nn;
            }
            //High
            float det = 0;
            for (int i = 0; i < o; i++)
            {
                float t = x[0, i] * AlgebraicCofactorElement(x, 0, i);
                det += t;
            }
            return det;

        }
        static public float AlgebraicCofactorElement(float[,] x, int c, int r)
        {
            //Pos
            int o = x.GetLength(0);
            float[,] y = new float[o - 1, o - 1];
            for (int i = 0; i < o - 1; i++)
            {
                for (int j = 0; j < o - 1; j++)
                {
                    y[i, j] = x[i + ((i >= c) ? 1 : 0), j + ((j >= r) ? 1 : 0)];
                }
            }
            float det = Determinant(y);
            if ((c + r) % 2 == 1)
            {
                return -det;
            }
            return det;
        }
        static public float[,] InverseMatrix(float[,] x)
        {
            int o = x.GetLength(0);
            float[,] ret = new float[o, o];
            float det = Determinant(x);
            for (int i = 0; i < o; i++)
            {
                for (int j = 0; j < o; j++)
                {
                    ret[j, i] = AlgebraicCofactorElement(x, i, j) / det;
                }
            }
            return ret;
        }
        static public float[,] InverseTransposedMatrix(float[,] x)
        {
            int o = x.GetLength(0);
            float[,] ret = new float[o, o];
            float det = Determinant(x);
            for (int i = 0; i < o; i++)
            {
                for (int j = 0; j < o; j++)
                {
                    ret[i, j] = AlgebraicCofactorElement(x, i, j) / det;
                }
            }
            return ret;
        }
        static public float Distance(float[] x)
        {
            float sq = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sq += x[i] * x[i];
            }
            return (float)Math.Sqrt(sq);
        }
        static public float[] SpecularReflection(float[] nip, float[] nn)
        {
            float dist1 = Math.Abs(Distance(nip) - 1);
            float dist2 = Math.Abs(Distance(nn) - 1);
            if (dist1 > 1e-6 || dist2 > 1e-6)
            {
                throw new ArvnCoreException("Vectors are not normalized");
            }
            float cosine = DotProduct(nip, nn);
            float[] ret = new float[nip.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = 2.0f * nn[i] * cosine - nip[i];
            }
            return ret;
        }
        static public float[,] PlaneLinearInterpolationTransform(float[] u, float[] v, float[] n)
        {
            float[,] ret = new float[3, 3];
            for (int i = 0; i < 3; i++)
            {
                ret[0, i] = u[i];
                ret[1, i] = v[i];
                ret[2, i] = n[i];
            }
            return InverseMatrix(ret);
        }

        static public float[,] TangentSpaceBaseMatrix(float[] a, float[] b, float[] c, float[] n, float[] ta, float[] tb, float[] tc)
        {
            if (Math.Abs(Distance(n) - 1) > 1e-6)
            {
                throw new Exception("Unnormalized normal");
            }
            float[,] intmat = new float[3, 3];
            float[,] intmatn;
            for (int i = 0; i < 3; i++)
            {
                intmat[0, i] = b[i] - a[i];
                intmat[1, i] = c[i] - a[i];
                intmat[2, i] = n[i];
            }
            intmatn = InverseMatrix(intmat);

            float[] x = new float[3], y = new float[3], z = new float[3];
            CartesianLinearTransform3D(intmatn, tb[0] - ta[0], tc[0] - ta[0], 0, out x[0], out x[1], out x[2]);
            CartesianLinearTransform3D(intmatn, tb[1] - ta[1], tc[1] - ta[1], 0, out y[0], out y[1], out y[2]);
            z[0] = n[0];
            z[1] = n[1];
            z[2] = n[2];
            NormalizeSelf(ref x);
            NormalizeSelf(ref y);
            NormalizeSelf(ref z);

            for (int i = 0; i < 3; i++)
            {
                intmat[i, 0] = x[i];
                intmat[i, 1] = y[i];
                intmat[i, 2] = z[i];
            }
            return intmat;
        }
        static public float[,] ZOrthoProjectionMatrix(float zNear,float zFar)
        {
            float[,] ret = IdentityMatrix(4);
            ret[2, 2] = -2 / (zNear - zFar);
            ret[2, 3] = -(zNear + zFar) / (zNear - zFar);
            return ret;
        }
        
    }
}