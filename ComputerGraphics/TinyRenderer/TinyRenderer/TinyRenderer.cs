using System;
using TinyRenderer.Core;
using TinyRenderer.Core.Drawing;
using TinyRenderer.Core.Render;
using TinyRenderer.Applications;
using TinyRenderer.Core.Application;
using TinyRenderer.Display;
using TinyRenderer.Legacy;
using TinyRenderer.Shaders;
using TinyRenderer.Utility;

namespace TinyRenderer
{
    class TinyRenderer
    {
        static void Lesson0()
        {
            //Lesson 0: Draw a dot on an image

            IArvnImage bitmap = new ArvnBufferedBitmap(100, 100);
            bitmap.Set(52, 41, Renderer.RGBToHex(255, 0, 0));
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l0.bmp");
        }
        static void Lesson1S1()
        {
            //Lesson 1 Section 1: Draw a line segment

            IArvnImage bitmap = new ArvnBufferedBitmap(100, 100);
            Renderer.DrawLineV1(10, 10, 20, 30, Renderer.RGBToHex(255, 0, 0), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l1s1.bmp");
        }

        static void Lesson1S2()
        {
            //Lesson 1 Section 2: Refine the algorithm which requires constant increment.

            IArvnImage bitmap = new ArvnBufferedBitmap(100, 100);
            Renderer.DrawLineV2(13, 20, 80, 40, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV2(20, 13, 40, 80, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV2(80, 40, 13, 20, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l1s2.bmp");
        }

        static void Lesson1S3()
        {
            //Lesson 1 Section 3: Fix the problem that the algorithm outputs holes

            IArvnImage bitmap = new ArvnBufferedBitmap(100, 100);
            Renderer.DrawLineV3(13, 20, 80, 40, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV3(20, 13, 40, 80, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV3(80, 40, 13, 20, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l1s3.bmp");
        }
        static void Lesson1S4()
        {
            //Lesson 1 Section 4: Reduce redudant expressions.

            IArvnImage bitmap = new ArvnBufferedBitmap(100, 100);
            Renderer.DrawLineV4(13, 20, 80, 40, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV4(20, 13, 40, 80, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV4(80, 40, 13, 20, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l1s4.bmp");
        }
        static void Lesson1S5()
        {
            //Lesson 1 Section 5: [Brehensam Line Drawing Algorithm] Avoid float-point numbers.

            IArvnImage bitmap = new ArvnBufferedBitmap(100, 100);
            Renderer.DrawLineV5(13, 20, 80, 40, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV5(20, 13, 40, 80, Renderer.RGBToHex(255, 0, 0), ref bitmap);
            Renderer.DrawLineV5(80, 40, 13, 20, Renderer.RGBToHex(255, 0, 0), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l1s5.bmp");
        }
        static void Lesson2S1P1()
        {
            //Lesson 2 Section 1 Part 1: Drawing a hollow triangle

            IArvnImage bitmap = new ArvnBufferedBitmap(200, 200);
            ArvnVec2i[] t0 = { ArvnVec2i.Create(10, 70), ArvnVec2i.Create(50, 160), ArvnVec2i.Create(70, 80) };
            ArvnVec2i[] t1 = { ArvnVec2i.Create(180, 50), ArvnVec2i.Create(150, 1), ArvnVec2i.Create(70, 180) };
            ArvnVec2i[] t2 = { ArvnVec2i.Create(180, 150), ArvnVec2i.Create(120, 160), ArvnVec2i.Create(130, 180) };
            Renderer.DrawTriangleHollowV1(t0[0], t0[1], t0[2], Renderer.RGBToHex(255, 0, 0), ref bitmap);
            Renderer.DrawTriangleHollowV1(t1[0], t1[1], t1[2], Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawTriangleHollowV1(t2[0], t2[1], t2[2], Renderer.RGBToHex(0, 255, 0), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l2s1p1.bmp");
        }

        static void Lesson2S1P2()
        {
            //Lesson 2 Section 1 Part 2: Sorting vertices according to y-coordinate

            IArvnImage bitmap = new ArvnBufferedBitmap(200, 200);
            ArvnVec2i[] t0 = { ArvnVec2i.Create(10, 70), ArvnVec2i.Create(50, 160), ArvnVec2i.Create(70, 80) };
            ArvnVec2i[] t1 = { ArvnVec2i.Create(180, 50), ArvnVec2i.Create(150, 1), ArvnVec2i.Create(70, 180) };
            ArvnVec2i[] t2 = { ArvnVec2i.Create(180, 150), ArvnVec2i.Create(120, 160), ArvnVec2i.Create(130, 180) };
            Renderer.DrawTriangleHollowV2(t0[0], t0[1], t0[2], Renderer.RGBToHex(255, 0, 0), ref bitmap);
            Renderer.DrawTriangleHollowV2(t1[0], t1[1], t1[2], Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawTriangleHollowV2(t2[0], t2[1], t2[2], Renderer.RGBToHex(0, 255, 0), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l2s1p2.bmp");
        }

        static void Lesson2S1P3()
        {
            //Lesson 2 Section 1 Part 3: Fill the lower area

            IArvnImage bitmap = new ArvnBufferedBitmap(200, 200);
            ArvnVec2i[] t0 = { ArvnVec2i.Create(10, 70), ArvnVec2i.Create(50, 160), ArvnVec2i.Create(70, 80) };
            ArvnVec2i[] t1 = { ArvnVec2i.Create(180, 50), ArvnVec2i.Create(150, 1), ArvnVec2i.Create(70, 180) };
            ArvnVec2i[] t2 = { ArvnVec2i.Create(180, 150), ArvnVec2i.Create(120, 160), ArvnVec2i.Create(130, 180) };
            Renderer.DrawTriangleHollowV3(t0[0], t0[1], t0[2], Renderer.RGBToHex(255, 0, 0), ref bitmap);
            Renderer.DrawTriangleHollowV3(t1[0], t1[1], t1[2], Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawTriangleHollowV3(t2[0], t2[1], t2[2], Renderer.RGBToHex(0, 255, 0), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l2s1p3.bmp");

        }

        static void Lesson2S1P4()
        {
            //Lesson 2 Section 1 Part 4: [Line Sweeping Algorithm] Fill the whole triangle

            IArvnImage bitmap = new ArvnBufferedBitmap(200, 200);
            ArvnVec2i[] t0 = { ArvnVec2i.Create(10, 70), ArvnVec2i.Create(50, 160), ArvnVec2i.Create(70, 80) };
            ArvnVec2i[] t1 = { ArvnVec2i.Create(180, 50), ArvnVec2i.Create(150, 1), ArvnVec2i.Create(70, 180) };
            ArvnVec2i[] t2 = { ArvnVec2i.Create(180, 150), ArvnVec2i.Create(120, 160), ArvnVec2i.Create(130, 180) };
            Renderer.DrawTriangleHollowV4(t0[0], t0[1], t0[2], Renderer.RGBToHex(255, 0, 0), ref bitmap);
            Renderer.DrawTriangleHollowV4(t1[0], t1[1], t1[2], Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawTriangleHollowV4(t2[0], t2[1], t2[2], Renderer.RGBToHex(0, 255, 0), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l2s1p4.bmp");
        }
        static void Lesson2S2()
        {
            //Lesson 2 Section 2: [Triangle Filling] Fill using bounding box

            IArvnImage bitmap = new ArvnBufferedBitmap(200, 200);
            ArvnVec2i[] t0 = { ArvnVec2i.Create(10, 70), ArvnVec2i.Create(50, 160), ArvnVec2i.Create(70, 80) };
            ArvnVec2i[] t1 = { ArvnVec2i.Create(180, 50), ArvnVec2i.Create(150, 1), ArvnVec2i.Create(70, 180) };
            ArvnVec2i[] t2 = { ArvnVec2i.Create(180, 150), ArvnVec2i.Create(120, 160), ArvnVec2i.Create(130, 180) };
            Renderer.DrawTriangle(t0[0], t0[1], t0[2], Renderer.RGBToHex(255, 0, 0), ref bitmap);
            Renderer.DrawTriangle(t1[0], t1[1], t1[2], Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawTriangle(t2[0], t2[1], t2[2], Renderer.RGBToHex(0, 255, 0), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l2s2.bmp");

        }
        static void Lesson2S3P1()
        {
            //Lesson 2 Section 3 Part 1: Wireframe Rendering

            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            Renderer.DrawWireMesh(model, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l2s3p1.bmp");
        }

        static void Lesson2S3P2()
        {
            //Lesson 2 Section 3 Part 2: Flat Shading Rendering

            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            Renderer.DrawFlatShadingV1(model, ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l2s3p2.bmp");
        }

        static void Lesson2S3P3()
        {
            //Lesson 2 Section 3 Part 3: Back Face Culling

            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnVec3f light = ArvnVec3f.Create(0, 0, -1);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            Renderer.DrawFlatShadingV2(model, light, ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l2s3p3.bmp");
        }

        static void Lesson3S1P1()
        {
            //Lesson 3 Section 1 Part 1: 2D scene at sideway viewpoint

            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            Renderer.DrawLineV5(20, 34, 744, 400, Renderer.RGBToHex(255, 0, 0), ref bitmap);
            Renderer.DrawLineV5(120, 434, 444, 400, Renderer.RGBToHex(0, 255, 0), ref bitmap);
            Renderer.DrawLineV5(330, 463, 594, 200, Renderer.RGBToHex(0, 0, 255), ref bitmap);
            Renderer.DrawLineV5(10, 10, 790, 10, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l3s1p1.bmp");

        }
        static void Lesson3S1P2()
        {
            //Lesson 3 Section 1 Part 2: Rasterize on 1D Screen

            IArvnImage bitmap = new ArvnBufferedBitmap(800, 1);
            int[] ybuffer;
            Renderer.GenerateYBuffer(800, out ybuffer);
            Renderer.Rasterize2D(20, 34, 744, 400, Renderer.RGBToHex(255, 0, 0), ref bitmap, ref ybuffer);
            Renderer.Rasterize2D(120, 434, 444, 400, Renderer.RGBToHex(0, 255, 0), ref bitmap, ref ybuffer);
            Renderer.Rasterize2D(330, 463, 594, 200, Renderer.RGBToHex(0, 0, 255), ref bitmap, ref ybuffer);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l3s1p2.bmp");

        }
        static void Lesson3S2()
        {
            //Lesson 3 Section 2: Rasterize Mesh

            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 800);
            ArvnVec3f light = ArvnVec3f.Create(0, 0, -1);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            Renderer.RasterizeFlatShading3D(model, light, ref bitmap, ref zbuf);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l3s2.bmp");
        }

        static void Lesson3S3()
        {
            //Lesson 3 Section 3: Attach diffuse texture

            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 800);
            ArvnVec3f light = ArvnVec3f.Create(0, 0, -1);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            IArvnImage texture = new ArvnBufferedBitmap(50, 50);
            texture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\texture.jpg");
            Renderer.RasterizeFlatShadingTextured3D(model, light, texture, ref bitmap, ref zbuf, false, null, null);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l3s3.bmp");
        }

        static void Lesson4S1P1()
        {
            //Lesson 4 Section 1 Part 1: Perform scale transformation

            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnVec2f[] p = { ArvnVec2f.Create(-1, -1), ArvnVec2f.Create(1, -1), ArvnVec2f.Create(1, 0), ArvnVec2f.Create(0, 1), ArvnVec2f.Create(-1, 1) };
            ArvnVec2f[] q = { ArvnVec2f.Create(-1, -1), ArvnVec2f.Create(1, -1), ArvnVec2f.Create(1, 0), ArvnVec2f.Create(0, 1), ArvnVec2f.Create(-1, 1) };

            Renderer.DrawLineV5(0, 0, 0, 1, Renderer.RGBToHex(0, 255, 0), ref bitmap);
            Renderer.DrawLineV5(0, 0, 1, 0, Renderer.RGBToHex(255, 0, 0), ref bitmap);
            for (int i = 0; i < p.Length; i++)
            {
                Renderer.DrawLineV5((int)p[i].x * 200 + 400, (int)p[i].y * 200 + 400, (int)p[(i + 1) % p.Length].x * 200 + 400, (int)p[(i + 1) % p.Length].y * 200 + 400, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            }
            for (int i = 0; i < p.Length; i++)
            {
                ArvnCore.CartesianLinearTransform2D(ArvnCore.ScaleTransformCartesian2D(0.5f), p[i].x, p[i].y, out q[i].x, out q[i].y);
            }
            for (int i = 0; i < p.Length; i++)
            {
                Renderer.DrawLineV5((int)(q[i].x * 200 + 400), (int)(q[i].y * 200 + 400), (int)(q[(i + 1) % q.Length].x * 200 + 400), (int)(q[(i + 1) % q.Length].y * 200 + 400), Renderer.RGBToHex(255, 255, 0), ref bitmap);
            }
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l4s1p1.bmp");
        }

        static void Lesson4S1P2()
        {
            //Lesson 4 Section 1 Part 2: Perform Composite transform
            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnVec2f[] p = { ArvnVec2f.Create(-1, -1), ArvnVec2f.Create(1, -1), ArvnVec2f.Create(1, 0), ArvnVec2f.Create(0, 1), ArvnVec2f.Create(-1, 1) };
            ArvnVec2f[] q = { ArvnVec2f.Create(-1, -1), ArvnVec2f.Create(1, -1), ArvnVec2f.Create(1, 0), ArvnVec2f.Create(0, 1), ArvnVec2f.Create(-1, 1) };

            Renderer.DrawLineV5(0, 0, 0, 1, Renderer.RGBToHex(0, 255, 0), ref bitmap);
            Renderer.DrawLineV5(0, 0, 1, 0, Renderer.RGBToHex(255, 0, 0), ref bitmap);
            for (int i = 0; i < p.Length; i++)
            {
                Renderer.DrawLineV5((int)p[i].x * 200 + 400, (int)p[i].y * 200 + 400, (int)p[(i + 1) % p.Length].x * 200 + 400, (int)p[(i + 1) % p.Length].y * 200 + 400, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            }
            for (int i = 0; i < p.Length; i++)
            {
                float[,] compose = ArvnCore.MatrixMultiply(ArvnCore.ScaleTransformCartesian2D(0.5f), ArvnCore.RotationTransformCartesian2D((float)Math.PI / 4));
                ArvnCore.CartesianLinearTransform2D(compose, p[i].x, p[i].y, out q[i].x, out q[i].y);
            }
            for (int i = 0; i < p.Length; i++)
            {
                Renderer.DrawLineV5((int)(q[i].x * 200 + 400), (int)(q[i].y * 200 + 400), (int)(q[(i + 1) % q.Length].x * 200 + 400), (int)(q[(i + 1) % q.Length].y * 200 + 400), Renderer.RGBToHex(255, 255, 0), ref bitmap);
            }
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l4s1p2.bmp");
        }

        static void Lesson4S2P1()
        {
            //Lesson 4 Section 2 Part 1: Perform Composite transform in Homogeneous coordinate

            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnVec2f[] p = { ArvnVec2f.Create(-1, -1), ArvnVec2f.Create(1, -1), ArvnVec2f.Create(1, 0), ArvnVec2f.Create(0, 1), ArvnVec2f.Create(-1, 1) };
            ArvnVec2f[] q = { ArvnVec2f.Create(-1, -1), ArvnVec2f.Create(1, -1), ArvnVec2f.Create(1, 0), ArvnVec2f.Create(0, 1), ArvnVec2f.Create(-1, 1) };

            Renderer.DrawLineV5(0, 0, 0, 1, Renderer.RGBToHex(0, 255, 0), ref bitmap);
            Renderer.DrawLineV5(0, 0, 1, 0, Renderer.RGBToHex(255, 0, 0), ref bitmap);
            for (int i = 0; i < p.Length; i++)
            {
                Renderer.DrawLineV5((int)p[i].x * 200 + 400, (int)p[i].y * 200 + 400, (int)p[(i + 1) % p.Length].x * 200 + 400, (int)p[(i + 1) % p.Length].y * 200 + 400, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            }
            for (int i = 0; i < p.Length; i++)
            {
                float temp;
                float[,] compose = ArvnCore.MatrixMultiply(ArvnCore.ScaleTransformHomogeneous2D(0.5f), ArvnCore.RotationTransformHomogeneous2D((float)Math.PI / 4));
                ArvnCore.HomogeneousLinearTransform2D(compose, p[i].x, p[i].y, 1, out q[i].x, out q[i].y, out temp);
            }
            for (int i = 0; i < p.Length; i++)
            {
                Renderer.DrawLineV5((int)(q[i].x * 200 + 400), (int)(q[i].y * 200 + 400), (int)(q[(i + 1) % q.Length].x * 200 + 400), (int)(q[(i + 1) % q.Length].y * 200 + 400), Renderer.RGBToHex(255, 255, 0), ref bitmap);
            }
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l4s2p1.bmp");
        }
        static void Lesson4S2P2()
        {
            //Lesson 4 Section 2 Part 2: 2D Viewport
            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnVec2f[] p = { ArvnVec2f.Create(-1, -1), ArvnVec2f.Create(1, -1), ArvnVec2f.Create(1, 0), ArvnVec2f.Create(0, 1), ArvnVec2f.Create(-1, 1) };
            ArvnVec2f[] q = { ArvnVec2f.Create(-1, -1), ArvnVec2f.Create(1, -1), ArvnVec2f.Create(1, 0), ArvnVec2f.Create(0, 1), ArvnVec2f.Create(-1, 1) };
            float[,] vp = ArvnCore.RectViewportMatrix2D(800, 800, 2, 2);
            for (int i = 0; i < p.Length; i++)
            {
                ArvnCore.HomogeneousLinearTransform2DToCartesian(vp, p[i].x, p[i].y, 1, out q[i].x, out q[i].y);
            }
            for (int i = 0; i < p.Length; i++)
            {
                Renderer.DrawLineV5((int)q[i].x, (int)q[i].y, (int)q[(i + 1) % p.Length].x, (int)q[(i + 1) % p.Length].y, Renderer.RGBToHex(255, 0, 0), ref bitmap);
            }
            float[,] composed = ArvnCore.MatrixMultiply(
                vp,
                ArvnCore.ScaleTransformHomogeneous2D(0.5f),
                ArvnCore.RotationTransformHomogeneous2D((float)Math.PI / 4));

            for (int i = 0; i < p.Length; i++)
            {
                ArvnCore.HomogeneousLinearTransform2DToCartesian(composed, p[i].x, p[i].y, 1, out q[i].x, out q[i].y);
            }
            for (int i = 0; i < p.Length; i++)
            {
                Renderer.DrawLineV5((int)q[i].x, (int)q[i].y, (int)q[(i + 1) % p.Length].x, (int)q[(i + 1) % p.Length].y, Renderer.RGBToHex(255, 255, 0), ref bitmap);
            }
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l4s2p2.bmp");
        }

        static public void Lesson4S3()
        {
            //Lesson 4 Section 3: Project in 3D

            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 800);
            ArvnVec3f light = ArvnVec3f.Create(0, 0, -1);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            IArvnImage texture = new ArvnBufferedBitmap(50, 50);
            texture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\texture.jpg");
            float[,] projection = ArvnCore.IdentityMatrix(4);
            projection[3, 2] = -1;
            float[,] viewport = ArvnCore.RectViewportMatrix3D(700, 700, 1, 1);

            Renderer.RasterizeFlatShadingTextured3D(model, light, texture, ref bitmap, ref zbuf, true, projection, viewport);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l4s3.bmp");
        }

        static public void Lesson6S1()
        {
            //Lesson 5~6 Section 1: Shader & Refactoring & Vertex Normal

            //Environment
            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            IArvnImage texture = new ArvnBufferedBitmap(50, 50);
            texture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\texture.jpg");


            //Shader
            ArvnCompatibleShader shader = new ArvnTinyShader();
            float[] light = { 0, 0, 1 };
            float[,] projection = ArvnCore.IdentityMatrix(4);
            float[,] modelview = ArvnCore.IdentityMatrix(4);
            float[,] viewport = ArvnCore.RectViewportMatrix3D(700, 700, 1, 1);
            shader.SetVariable("projection", projection);
            shader.SetVariable("modelview", modelview);
            shader.SetVariable("viewport", viewport);
            shader.SetVariable("lightdir", light);

            //Attributes
            object[] vertex = model.ExportVertices();
            object[] vertexNormal = model.ExportVertexNormals();
            int[] faceIndices = model.ExportFaceIndexes();
            shader.SetAttributeVariable("vertices", vertex);
            shader.SetAttributeVariable("vnormals", vertexNormal);

            //Render
            ArvnRender renderer = ArvnRender.Create();
            renderer.RasterizeTriangles3D(faceIndices, ref shader, ref bitmap, ref zbuf);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l6s1.bmp");
        }

        static public void Lesson6S2()
        {
            //Lesson 5~6 Section 2: Look At & Projection

            //Environment
            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            IArvnImage texture = new ArvnBufferedBitmap(50, 50);
            texture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\texture.jpg");

            //Shader
            ArvnCompatibleShader shader = new ArvnTinyShader();
            float[] light = ArvnCore.Normalize(new float[] { 1, 1, 1 });
            float[,] projection = ArvnCore.PerspectiveMatrix(3.14159f / 3, 1, 0.01f, 100f);
            //float[,] projection = ArvnCore.IdentityMatrix(4);
            float[,] modelview = ArvnCore.LookAt(new float[] { 1, 1, 2 }, new float[] { 0, 0, 0 }, new float[] { 0, 1, 0 });
            float[,] viewport = ArvnCore.RectViewportMatrix3D(700, 700, 1, 1);
            shader.SetVariable("projection", projection);
            shader.SetVariable("modelview", modelview);
            shader.SetVariable("viewport", viewport);
            shader.SetVariable("lightdir", light);

            //Attributes
            object[] vertex = model.ExportVertices();
            object[] vertexNormal = model.ExportVertexNormals();
            int[] faceIndices = model.ExportFaceIndexes();
            shader.SetAttributeVariable("vertices", vertex);
            shader.SetAttributeVariable("vnormals", vertexNormal);

            //Render
            ArvnRender renderer = ArvnRender.Create();
            renderer.RasterizeTriangles3D(faceIndices, ref shader, ref bitmap, ref zbuf);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l6s2.bmp");
        }

        static public void Lesson6S3()
        {
            //Lesson 6 Section 3: Attach texture using shader

            //Environment
            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            IArvnImage texture = new ArvnBufferedBitmap(50, 50);
            texture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\texture.jpg");

            //Shader
            ArvnCompatibleShader shader = new ArvnTinyShader();
            float[] light = ArvnCore.Normalize(new float[] { 1, 1, 1 });
            float[,] projection = ArvnCore.PerspectiveMatrix(3.14159f / 3, 1, 0.01f, 100f);
            //float[,] projection = ArvnCore.IdentityMatrix(4);
            float[,] modelview = ArvnCore.LookAt(new float[] { 1, 1, 2 }, new float[] { 0, 0, 0 }, new float[] { 0, 1, 0 });
            float[,] viewport = ArvnCore.RectViewportMatrix3D(700, 700, 1, 1);
            shader.SetVariable("projection", projection);
            shader.SetVariable("modelview", modelview);
            shader.SetVariable("viewport", viewport);
            shader.SetVariable("lightdir", light);
            shader.SetVariable("version", 2);
            shader.SetVariable("diffuse_texture", texture);


            //Attributes
            object[] vertex = model.ExportVertices();
            object[] vertexNormal = model.ExportVertexNormals();
            object[] vertexTexture = model.ExportVertexTexture();
            int[] faceIndices = model.ExportFaceIndexes();
            shader.SetAttributeVariable("vertices", vertex);
            shader.SetAttributeVariable("vnormals", vertexNormal);
            shader.SetAttributeVariable("vtexture", vertexTexture);

            //Render
            ArvnRender renderer = ArvnRender.Create();
            renderer.RasterizeTriangles3D(faceIndices, ref shader, ref bitmap, ref zbuf);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l6s3.bmp");
        }

        static public void Lesson6S4()
        {
            //Lesson 6 Section 4: Transform of normals & Specular mapping

            //Environment
            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            IArvnImage texture = new ArvnBufferedBitmap(50, 50);
            texture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\texture.jpg");
            IArvnImage specularTexture = new ArvnBufferedBitmap(50, 50);
            specularTexture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\specular.jpg");

            //Shader
            ArvnCompatibleShader shader = new ArvnTinyShaderV2();
            float[] light = ArvnCore.Normalize(new float[] { 1, 1, 1 });
            float[,] projection = ArvnCore.PerspectiveMatrix(3.14159f / 3, 1, 0.01f, 100f);
            //float[,] projection = ArvnCore.IdentityMatrix(4);
            float[,] modelview = ArvnCore.LookAt(new float[] { 1, 1, 2 }, new float[] { 0, 0, 0 }, new float[] { 0, 1, 0 });
            float[,] viewport = ArvnCore.RectViewportMatrix3D(700, 700, 1, 1);
            shader.SetVariable("projection", projection);
            shader.SetVariable("modelview", modelview);
            shader.SetVariable("viewport", viewport);
            shader.SetVariable("lightdir", light);
            shader.SetVariable("diffuse_texture", texture);
            shader.SetVariable("spec_texture", specularTexture);

            //Attributes
            object[] vertex = model.ExportVertices();
            object[] vertexNormal = model.ExportVertexNormals();
            object[] vertexTexture = model.ExportVertexTexture();
            int[] faceIndices = model.ExportFaceIndexes();
            shader.SetAttributeVariable("vertices", vertex);
            shader.SetAttributeVariable("vnormals", vertexNormal);
            shader.SetAttributeVariable("vtexture", vertexTexture);

            //Render
            ArvnRender renderer = ArvnRender.Create();
            renderer.RasterizeTriangles3D(faceIndices, ref shader, ref bitmap, ref zbuf);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l6s4.bmp");
        }

        static public void Lesson6S5()
        {
            //Lesson 6 Section 5 : Tangent space & Normal mapping

            //Environment
            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            IArvnImage texture = new ArvnBufferedBitmap(50, 50);
            texture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\texture.jpg");
            IArvnImage specularTexture = new ArvnBufferedBitmap(50, 50);
            specularTexture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\specular.jpg");
            IArvnImage normalTexture = new ArvnBufferedBitmap(50, 50);
            normalTexture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\normal.jpg");

            //Shader
            ArvnCompatibleShader shader = new ArvnTinyShaderV3();
            float[] light = ArvnCore.Normalize(new float[] { 1, 1, 1 });
            float[,] projection = ArvnCore.PerspectiveMatrix(3.14159f / 3, 1, 0.01f, 100f);
            //float[,] projection = ArvnCore.IdentityMatrix(4);
            float[,] modelview = ArvnCore.LookAt(new float[] { 1, 1, 2 }, new float[] { 0, 0, 0 }, new float[] { 0, 1, 0 });
            float[,] viewport = ArvnCore.RectViewportMatrix3D(700, 700, 1, 1);
            shader.SetVariable("projection", projection);
            shader.SetVariable("modelview", modelview);
            shader.SetVariable("viewport", viewport);
            shader.SetVariable("lightdir", light);
            shader.SetVariable("diffuse_texture", texture);
            shader.SetVariable("spec_texture", specularTexture);
            shader.SetVariable("normal_texture", normalTexture);

            //Attributes
            object[] vertex = model.ExportVertices();
            object[] vertexNormal = model.ExportVertexNormals();
            object[] vertexTexture = model.ExportVertexTexture();
            int[] faceIndices = model.ExportFaceIndexes();
            shader.SetAttributeVariable("vertices", vertex);
            shader.SetAttributeVariable("vnormals", vertexNormal);
            shader.SetAttributeVariable("vtexture", vertexTexture);

            //Render
            ArvnRender renderer = ArvnRender.Create();
            renderer.RasterizeTriangles3D(faceIndices, ref shader, ref bitmap, ref zbuf);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l6s5.bmp");
        }

        public static void Lesson7S1()
        {
            //Lesson 7 Section 1: Depth Shader

            //Environment
            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");

            //Shader
            ArvnCompatibleShader shader = new ArvnCompatibleDepthShader();
            float[] light = ArvnCore.Normalize(new float[] { 1, 1, 1 });
            //float[,] projection = ArvnCore.PerspectiveMatrix(3.14159f / 3, 1, 0.01f, 100f);
            float[,] projection = ArvnCore.ZOrthoProjectionMatrix(0.01f, 3);
            float[,] modelview = ArvnCore.LookAt(new float[] { 1, 1, 1 }, new float[] { 0, 0, 0 }, new float[] { 0, 1, 0 });
            float[,] viewport = ArvnCore.RectViewportMatrix3D(700, 700, 1, 1);
            shader.SetVariable("projection", projection);
            shader.SetVariable("modelview", modelview);
            shader.SetVariable("viewport", viewport);

            //Attribute
            object[] vertex = model.ExportVertices();
            int[] faceIndices = model.ExportFaceIndexes();
            shader.SetAttributeVariable("vertices", vertex);

            //Render
            ArvnRender renderer = ArvnRender.Create();
            renderer.RasterizeTriangles3D(faceIndices, ref shader, ref bitmap, ref zbuf);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l7s1.bmp");
        }

        public static void Lesson7S2()
        {
            //Lesson 7 Section 2: Shadow Mapping

            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            IArvnImage shadowMap = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zDepthBuf = ArvnZBuffer.Create(800, 800);
            ArvnZBuffer zBuf = ArvnZBuffer.Create(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            IArvnImage texture = new ArvnBufferedBitmap(50, 50);
            texture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\texture.jpg");
            IArvnImage specularTexture = new ArvnBufferedBitmap(50, 50);
            specularTexture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\specular.jpg");
            IArvnImage normalTexture = new ArvnBufferedBitmap(50, 50);
            normalTexture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\normal.jpg");
            ArvnCompatibleShader depthShader = new ArvnCompatibleDepthShader();
            ArvnCompatibleShader shadowShader = new ArvnCompatibleShadowShader();

            //Parameters
            float[] light = ArvnCore.Normalize(new float[] { 1, 1, 1 });
            float[,] projection = ArvnCore.PerspectiveMatrix(3.14159f / 3, 1, 0.01f, 100f);
            float[,] depthProjection = ArvnCore.ZOrthoProjectionMatrix(0.01f, 3);
            float[,] modelview = ArvnCore.LookAt(new float[] { 0, 0, 2 }, new float[] { 0, 0, 0 }, new float[] { 0, 1, 0 });
            float[,] depthModelview = ArvnCore.LookAt(new float[] { 1, 1, 1 }, new float[] { 0, 0, 0 }, new float[] { 0, 1, 0 });
            float[,] viewport = ArvnCore.RectViewportMatrix3D(700, 700, 1, 1);

            depthShader.SetVariable("projection", depthProjection);
            depthShader.SetVariable("modelview", depthModelview);
            depthShader.SetVariable("viewport", viewport);

            //Attribute
            object[] vertex = model.ExportVertices();
            object[] vertexNormal = model.ExportVertexNormals();
            object[] vertexTexture = model.ExportVertexTexture();
            int[] faceIndices = model.ExportFaceIndexes();
            depthShader.SetAttributeVariable("vertices", vertex);

            //First-pass Render
            ArvnRender renderer = ArvnRender.Create();
            renderer.RasterizeTriangles3D(faceIndices, ref depthShader, ref shadowMap, ref zDepthBuf);

            //Second-pass Preparation
            float[,] firstPassTransform = ArvnCore.MatrixMultiply(viewport, depthProjection, depthModelview);
            float[,] secondPassTransformNdc = ArvnCore.MatrixMultiply(projection, modelview);
            float[,] secondPassTransformNdcI = ArvnCore.InverseMatrix(secondPassTransformNdc);
            float[,] depthRemap = ArvnCore.MatrixMultiply(firstPassTransform, secondPassTransformNdcI);
            shadowShader.SetVariable("projection", projection);
            shadowShader.SetVariable("modelview", modelview);
            shadowShader.SetVariable("viewport", viewport);
            shadowShader.SetVariable("lightdir", light);
            shadowShader.SetVariable("diffuse_texture", texture);
            shadowShader.SetVariable("spec_texture", specularTexture);
            shadowShader.SetVariable("normal_texture", normalTexture);
            shadowShader.SetVariable("depth_map", shadowMap);
            shadowShader.SetVariable("depth_remap", depthRemap);


            shadowShader.SetAttributeVariable("vertices", vertex);
            shadowShader.SetAttributeVariable("vnormals", vertexNormal);
            shadowShader.SetAttributeVariable("vtexture", vertexTexture);

            //Second-pass Render
            renderer.RasterizeTriangles3D(faceIndices, ref shadowShader, ref bitmap, ref zBuf);

            //Save
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l7s2b.bmp");

        }
        public static void Lesson8S1P1Pre()
        {
            //Lesson 8 Section 1 Part 1 Pre: Random unit vector

            for (int i = 0; i < 30; i++)
            {
                float[] eye = ArvnCore.RandomUnitVector();
                Console.WriteLine(eye[0] + "," + eye[1] + "," + eye[2]);
            }
        }
        public static void Lesson8S1P1()
        {
            //Lesson 8 Section 1 Part 1: Ambient occlusion in BF Algorithm / Visibility information calculation & Exporting AO texture

            //Environment
            IArvnImage depthMap = new ArvnBufferedBitmap(800, 800);
            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 800);
            ArvnZBuffer zbufb = ArvnZBuffer.Create(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            ArvnCompatibleShader depthShader = new ArvnCompatibleDepthShader();
            ArvnCompatibleShader aoShader = new ArvnCompatibleBFAOTextureShader();
            ArvnRender renderer = ArvnRender.Create();

            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine("Iteration" + i);
                zbuf.Reset();
                zbufb.Reset();
                Console.WriteLine("Depth Rendering" + i);
                //Attribute
                object[] vertex = model.ExportVertices();
                int[] faceIndices = model.ExportFaceIndexes();
                object[] vertexTexture = model.ExportVertexTexture();

                //First Pass Shader
                float[] eye = ArvnCore.RandomUnitVector();
                float[] up = new float[] { ArvnCore.RandomUniform(), ArvnCore.RandomUniform(), ArvnCore.RandomUniform() };
                eye[1] = Math.Abs(eye[1]);
                float[,] projection = ArvnCore.ZOrthoProjectionMatrix(0.001f, 3);
                float[,] modelview = ArvnCore.LookAt(eye, new float[] { 0, 0, 0 }, up);
                float[,] viewport = ArvnCore.RectViewportMatrix3D(799, 799, 1, 1);
                float[,] viewportuv = ArvnCore.RectViewportMatrix3DEx(799, 799, 1, 1);
                depthShader.SetVariable("projection", projection);
                depthShader.SetVariable("modelview", modelview);
                depthShader.SetVariable("viewport", viewport);
                depthShader.SetAttributeVariable("vertices", vertex);

                //First Pass Render
                renderer.standardZCoordLimit = false;
                renderer.RasterizeTriangles3D(faceIndices, ref depthShader, ref depthMap, ref zbuf);
                if (i == 0)
                {
                    depthMap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l8s1p1a.bmp");
                }


                //Second Pass Shader
                Console.WriteLine("AO Rendering" + i);
                float[,] firstPassTransform = ArvnCore.MatrixMultiply(viewport, projection, modelview);
                aoShader.SetVariable("viewport", viewportuv);
                aoShader.SetVariable("depth_map", depthMap);
                aoShader.SetVariable("ao_map", bitmap);
                aoShader.SetVariable("depth_remap", firstPassTransform);
                aoShader.SetVariable("iteration", i);

                aoShader.SetAttributeVariable("vertices", vertex);
                aoShader.SetAttributeVariable("vtexture", vertexTexture);

                //Second Pass Render
                renderer.standardZCoordLimit = true;
                renderer.RasterizeTriangles3D(faceIndices, ref aoShader, ref bitmap, ref zbufb);
                if (i == 0)
                {
                    bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l8s1p1b.bmp");
                }

            }
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l8s1p1c.bmp");
        }

        public static void Lesson8S1P2()
        {
            //Lesson 8 Section 1 Part 2: AO Texture & Shader refactoring

            //Environment
            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            IArvnImage texture = new ArvnBufferedBitmap(50, 50);
            texture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l8s1p1c.bmp");

            //Shader
            ArvnShader shader = new ArvnPlainTexturedShader();
            float[,] projection = ArvnCore.PerspectiveMatrix(3.14159f / 3, 1, 0.01f, 100f);
            float[,] modelview = ArvnCore.LookAt(new float[] { 0, 0, 2 }, new float[] { 0, 0, 0 }, new float[] { 0, 1, 0 });
            float[,] viewport = ArvnCore.RectViewportMatrix3D(700, 700, 1, 1);
            shader.SetVariable("projection", projection);
            shader.SetVariable("modelview", modelview);
            shader.SetVariable("viewport", viewport);
            shader.SetVariable("diffuse_texture", texture);

            //Attribute
            object[] vertex = model.ExportVertices();
            object[] vertexTexture = model.ExportVertexTexture();
            int[] faceIndices = model.ExportFaceIndexes();
            shader.SetAttributeVariable("vertices", vertex);
            shader.SetAttributeVariable("vtexture", vertexTexture);

            //Render
            ArvnRender renderer = ArvnRender.Create();
            renderer.RasterizeTriangles3D(faceIndices, ref shader, ref bitmap, ref zbuf);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l8s1p2.bmp");
        }
        public static void Lesson8S2()
        {
            //Lesson 8 Section 2: Plain Screen Space Ambient Occlusion （SSAO)

            //Objects
            IArvnImage bitmap = new ArvnBufferedBitmap(800, 800);
            IArvnImage bitmapb = new ArvnBufferedBitmap(800, 800);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 800);
            ArvnZBuffer zbufb = ArvnZBuffer.Create(800, 800);
            ArvnMesh model = new ArvnMesh();
            ArvnRender renderer = ArvnRender.Create();
            renderer.standardZCoordLimit = true;
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");

            //Attributes
            float[,] projection = ArvnCore.ZOrthoProjectionMatrix(0.001f, 3);
            float[,] modelview = ArvnCore.LookAt(new float[] { 0, 0, 2 }, new float[] { 0, 0, 0 }, new float[] { 0, 1, 0 });
            float[,] viewport = ArvnCore.RectViewportMatrix3D(799, 799, 1, 1);
            object[] vertex = model.ExportVertices();
            object[] vtexture = model.ExportVertexTexture();
            int[] faceIndices = model.ExportFaceIndexes();

            //Depth Render
            ArvnShader shader = new ArvnSSAODepthShader();
            shader.SetVariable("projection", projection);
            shader.SetVariable("modelview", modelview);
            shader.SetVariable("viewport", viewport);

            shader.SetAttributeVariable("vertices", vertex);
            shader.SetAttributeVariable("vtexture", vtexture);

            renderer.RasterizeTriangles3D(faceIndices, ref shader, ref bitmap, ref zbuf);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l8s2a.bmp");

            //AO Render
            ArvnShader shaderb = new ArvnSSAORenderShader();
            shaderb.SetVariable("viewport", viewport);
            shaderb.SetVariable("depth_map", bitmap);
            object[] vertexf = { new float[] { -1, -1, 0 }, new float[] { -1, 1, 0 }, new float[] { 1, 1, 0 }, new float[] { 1, -1, 0 } };
            int[] faceIdxf = { 0, 1, 2, 2, 3, 0 };
            shaderb.SetAttributeVariable("vertex", vertexf);
            renderer.RasterizeTriangles3D(faceIdxf, ref shaderb, ref bitmapb, ref zbufb);
            bitmapb.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l8s2b.bmp");
        }

        public static void Lesson9S1()
        {
            //Lesson 9 Section 1: Establish a window & Loop

            IArvnApplication app = new ArvnAppL9S1();
            app.Run();
        }

        public static void Lesson9S2()
        {
            //Lesson 9 Section 2: Frame rate & Drawing on the window

            IArvnApplication app = new ArvnAppL9S2();
            app.Run();
        }

        public static void Lesson9S3()
        {
            //Lesson 9 Section 3: Double buffering

            IArvnApplication app = new ArvnAppL9S3();
            app.Run();
        }
        static void Main(string[] args)
        {
            Lesson9S3();
        }
    }
}
