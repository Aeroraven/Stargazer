using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TinyRenderer.Core;
using TinyRenderer.Core.Drawing;
using TinyRenderer.Legacy;
using TinyRenderer.Shaders;

namespace TinyRenderer.Core.Render
{
    class ArvnRender
    {
        public bool standardZCoordLimit = false;
        public int parallelShaders = 0;
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
            shader.BeforeRunning();
            for (int i = 0; i < faceIndex.Length; i += 3)
            {
                float[][] v = new float[3][];
                for (int j = 0; j < 3; j++)
                {
                    shader.VertexShader(faceIndex[i + j], j);
                    v[j] = (float[])shader.GetInternalVariable(1);
                }
                TriangleFragProcess3D(v[0], v[1], v[2], ref shader, ref target, ref zbuf);
            }
        }
        protected void TriangleFragProcess3D(float[] ts0, float[] ts1, float[] ts2, ref IArvnShaderCaller shader, ref IArvnImage target, ref ArvnZBuffer zbuf)
        {
            unsafe
            {
                fixed(float* t0 = ts0, t1 = ts1, t2 = ts2)
                {
                    float maxZ = Math.Max(t0[2], t1[2]);
                    maxZ = Math.Max(maxZ, t2[2]);

                    float bboxMaxx = 0, bboxMaxy = 0;
                    float bboxMinx = 1e20f, bboxMiny = 1e20f;

                    bboxMaxx = Math.Max(bboxMaxx, t0[0]);
                    bboxMaxx = Math.Max(bboxMaxx, t1[0]);
                    bboxMaxx = Math.Max(bboxMaxx, t2[0]);
                    bboxMaxx = Math.Min(bboxMaxx, target.GetWidth() - 1);

                    bboxMaxy = Math.Max(bboxMaxy, t0[1]);
                    bboxMaxy = Math.Max(bboxMaxy, t1[1]);
                    bboxMaxy = Math.Max(bboxMaxy, t2[1]);
                    bboxMaxy = Math.Min(bboxMaxy, target.GetHeight() - 1);

                    bboxMinx = Math.Min(bboxMinx, t0[0]);
                    bboxMinx = Math.Min(bboxMinx, t1[0]);
                    bboxMinx = Math.Min(bboxMinx, t2[0]);
                    bboxMinx = Math.Max(bboxMinx, 0);

                    bboxMiny = Math.Min(bboxMiny, t0[1]);
                    bboxMiny = Math.Min(bboxMiny, t1[1]);
                    bboxMiny = Math.Min(bboxMiny, t2[1]);
                    bboxMiny = Math.Max(bboxMiny, 0);

                    int xm = (int)bboxMaxx;
                    int ym = (int)bboxMaxy;
                    int ys = (int)bboxMiny;
                    float ta, tb, tc, tw, x0, y0, z0, x1, y1, z1, zbufVal, zv;
                    float[] color;
                    int hexColor;
                    for (int i = (int)bboxMinx; i <= xm; i++)
                    {
                        for (int j = ys; j <= ym; j++)
                        {
                            //Pre Z Check
                            zbufVal = zbuf.Get(i, j);
                            if (zbufVal > maxZ)
                            {
                                continue;
                            }

                            //BaryCenter
                            x0 = t1[0] - t0[0];
                            y0 = t2[0] - t0[0];
                            z0 = t0[0] - i;
                            x1 = t1[1] - t0[1];
                            y1 = t2[1] - t0[1];
                            z1 = t0[1] - j;
                            tw = 1 / (x0 * y1 - y0 * x1);
                            tb = (y0 * z1 - z0 * y1) * tw;
                            tc = (z0 * x1 - x0 * z1) * tw;
                            if (tb < 0 || tc < 0)
                            {
                                continue;
                            }
                            ta = 1 - tb - tc;

                            //Shading
                            if (ta >= 0)
                            {
                                zv = ta * t0[2] + tb * t1[2] + tc * t2[2];
                                if (standardZCoordLimit && (zv < -1 || zv > 1))
                                {
                                    continue;
                                }
                                if (zbufVal < zv)
                                {
                                    shader.FragmentShader(new float[] { ta, tb, tc });
                                    color = (float[])shader.GetInternalVariable(0);
                                    hexColor = ArvnCore.RGBToHex((int)(color[0] * 255), (int)(color[1] * 255), (int)(color[2] * 255));
                                    target.Set(i, j, hexColor);
                                    zbuf.Set(i, j, zv);
                                }

                            }
                        }
                    }
                }
                
            }
            
        }

        
    }
}