using System;
using System.Drawing;

namespace TinyRenderer
{
    class Program
    {
        static void Lesson0()
        {
            //Lesson 0: Draw a dot on an image

            ArvnImage bitmap = new ArvnImageBitmap(100, 100);
            bitmap.Set(52, 41, Renderer.RGBToHex(255, 0, 0));
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l0.bmp");
        }
        static void Lesson1S1()
        {
            //Lesson 1 Section 1: Draw a line segment

            ArvnImage bitmap = new ArvnImageBitmap(100, 100);
            Renderer.DrawLineV1(10, 10, 20, 30, Renderer.RGBToHex(255, 0, 0), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l1s1.bmp");
        }

        static void Lesson1S2()
        {
            //Lesson 1 Section 2: Refine the algorithm which requires constant increment.

            ArvnImage bitmap = new ArvnImageBitmap(100, 100);
            Renderer.DrawLineV2(13, 20, 80, 40, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV2(20, 13, 40, 80, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV2(80, 40, 13, 20, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l1s2.bmp");
        }

        static void Lesson1S3()
        {
            //Lesson 1 Section 3: Fix the problem that the algorithm outputs holes

            ArvnImage bitmap = new ArvnImageBitmap(100, 100);
            Renderer.DrawLineV3(13, 20, 80, 40, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV3(20, 13, 40, 80, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV3(80, 40, 13, 20, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l1s3.bmp");
        }
        static void Lesson1S4()
        {
            //Lesson 1 Section 4: Reduce redudant expressions.

            ArvnImage bitmap = new ArvnImageBitmap(100, 100);
            Renderer.DrawLineV4(13, 20, 80, 40, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV4(20, 13, 40, 80, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV4(80, 40, 13, 20, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l1s4.bmp");
        }
        static void Lesson1S5()
        {
            //Lesson 1 Section 5: [Brehensam Line Drawing Algorithm] Avoid float-point numbers.

            ArvnImage bitmap = new ArvnImageBitmap(100, 100);
            Renderer.DrawLineV5(13, 20, 80, 40, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            Renderer.DrawLineV5(20, 13, 40, 80, Renderer.RGBToHex(255, 0, 0), ref bitmap);
            Renderer.DrawLineV5(80, 40, 13, 20, Renderer.RGBToHex(255, 0, 0), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l1s5.bmp");
        }
        static void Lesson2S1P1()
        {
            //Lesson 2 Section 1 Part 1: Drawing a hollow triangle

            ArvnImage bitmap = new ArvnImageBitmap(200, 200);
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

            ArvnImage bitmap = new ArvnImageBitmap(200, 200);
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

            ArvnImage bitmap = new ArvnImageBitmap(200, 200);
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

            ArvnImage bitmap = new ArvnImageBitmap(200, 200);
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

            ArvnImage bitmap = new ArvnImageBitmap(200, 200);
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

            ArvnImage bitmap = new ArvnImageBitmap(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            Renderer.DrawWireMesh(model, Renderer.RGBToHex(255, 255, 255), ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l2s3p1.bmp");
        }

        static void Lesson2S3P2()
        {
            //Lesson 2 Section 3 Part 2: Flat Shading Rendering

            ArvnImage bitmap = new ArvnImageBitmap(800, 800);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            Renderer.DrawFlatShadingV1(model, ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l2s3p2.bmp");
        }

        static void Lesson2S3P3()
        {
            //Lesson 2 Section 3 Part 3: Back Face Culling

            ArvnImage bitmap = new ArvnImageBitmap(800, 800);
            ArvnVec3f light = ArvnVec3f.Create(0, 0, -1);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            Renderer.DrawFlatShadingV2(model, light, ref bitmap);
            bitmap.Save("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\l2s3p3.bmp");
        }
        static void Main(string[] args)
        {
            Lesson2S3P3();
        }
    }
}
