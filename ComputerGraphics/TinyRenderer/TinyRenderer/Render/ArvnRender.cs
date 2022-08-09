using System;
using System.Collections.Generic;
using System.Text;
using TinyRenderer.Core;
using TinyRenderer.Display;
using TinyRenderer.Shaders;

namespace TinyRenderer.Render
{
    class ArvnRender
    {
        public bool standardZCoordLimit = false;
        static public ArvnRender Create()
        {
            return new ArvnRender();
        }
        public void DrawLine(int x0, int y0, int x1, int y1, int colorHex, ref IArvnImage target)
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
            int yi = y1 > y0 ? 1 : -1;
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
                    error -= dx * 2;
                }
            }
        }
        public void RasterizeTriangles3D(int[] faceIndex, ref ArvnCompatibleShader shader, ref IArvnImage target, ref ArvnZBuffer zbuf)
        {
            IArvnShaderCaller caller = shader;
            RasterizeTriangles3D(faceIndex, ref caller, ref target, ref zbuf);
        }
        public void RasterizeTriangles3D(int[] faceIndex, ref ArvnShader shader, ref IArvnImage target, ref ArvnZBuffer zbuf)
        {
            IArvnShaderCaller caller = shader;
            RasterizeTriangles3D(faceIndex, ref caller, ref target, ref zbuf);
        }
        public void RasterizeTriangles3D(int[] faceIndex, ref IArvnShaderCaller shader, ref IArvnImage target, ref ArvnZBuffer zbuf)
        {
            for (int i = 0; i < faceIndex.Length; i += 3)
            {
                float[][] v = new float[3][];
                for (int j = 0; j < 3; j++)
                {
                    shader.VertexShader(faceIndex[i + j], j);
                    v[j] = (float[])shader.GetVariable("arPosition");
                }
                TriangleFragProcess3D(v[0], v[1], v[2], ref shader, ref target, ref zbuf);
            }
        }
        protected void TriangleFragProcess3D(float[] t0, float[] t1, float[] t2, ref IArvnShaderCaller shader, ref IArvnImage target, ref ArvnZBuffer zbuf)
        {
            TriangleFragProcess3D(ArvnVec3f.Create(t0[0], t0[1], t0[2]), ArvnVec3f.Create(t1[0], t1[1], t1[2]), ArvnVec3f.Create(t2[0], t2[1], t2[2]), ref shader, ref target, ref zbuf);
        }
        protected void TriangleFragProcess3D(ArvnVec3f t0, ArvnVec3f t1, ArvnVec3f t2, ref IArvnShaderCaller shader, ref IArvnImage target, ref ArvnZBuffer zbuf)
        {
            ArvnVec2f bboxMax = ArvnVec2f.Create(0, 0);
            ArvnVec2f bboxMin = ArvnVec2f.Create(target.GetWidth(), target.GetHeight());
            bboxMax.x = Math.Max(bboxMax.x, t0.x);
            bboxMax.x = Math.Max(bboxMax.x, t1.x);
            bboxMax.x = Math.Max(bboxMax.x, t2.x);
            bboxMax.x = Math.Min(bboxMax.x, target.GetWidth() - 1);

            bboxMax.y = Math.Max(bboxMax.y, t0.y);
            bboxMax.y = Math.Max(bboxMax.y, t1.y);
            bboxMax.y = Math.Max(bboxMax.y, t2.y);
            bboxMax.y = Math.Min(bboxMax.y, target.GetHeight() - 1);

            bboxMin.x = Math.Min(bboxMin.x, t0.x);
            bboxMin.x = Math.Min(bboxMin.x, t1.x);
            bboxMin.x = Math.Min(bboxMin.x, t2.x);
            bboxMin.x = Math.Max(bboxMin.x, 0);

            bboxMin.y = Math.Min(bboxMin.y, t0.y);
            bboxMin.y = Math.Min(bboxMin.y, t1.y);
            bboxMin.y = Math.Min(bboxMin.y, t2.y);
            bboxMin.y = Math.Max(bboxMin.y, 0);

            for (int i = (int)bboxMin.x; i <= (int)bboxMax.x; i++)
            {
                for (int j = (int)bboxMin.y; j <= (int)bboxMax.y; j++)
                {
                    float ta, tb, tc;
                    ArvnCore.ToBarycentric(i, j, t0.x, t0.y, t1.x, t1.y, t2.x, t2.y, out ta, out tb, out tc);
                    if (ta >= 0 && tb >= 0 & tc >= 0)
                    {
                        float zv = (float)(ta * t0.z + tb * t1.z + tc * t2.z);
                        if (standardZCoordLimit && (zv < -1 || zv > 1))
                        {
                            continue;
                        }
                        shader.FragmentShader(new float[] { ta, tb, tc });
                        float[] color = (float[])shader.GetVariable("arFragColor");
                        int hexColor = ArvnCore.RGBToHex((int)(color[0] * 255), (int)(color[1] * 255), (int)(color[2] * 255));
                        if (zbuf.Get(i, j) < zv)
                        {
                            target.Set(i, j, hexColor);
                            zbuf.Set(i, j, zv);
                        }

                    }
                }
            }
        }
    }
}