using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinyRenderer.Core.Application;
using TinyRenderer.Core;
using TinyRenderer.Core.Render;
using TinyRenderer.Core.Drawing;
using TinyRenderer.Utility;
using System.Threading;
using System.Drawing;
using TinyRenderer.Display;
using TinyRenderer.Shaders;

namespace TinyRenderer.Applications
{
    class ArvnAppL9S5 : IArvnApplication
    {
        Form form;
        ArvnDoubleBuffer drawBuffer;
        Font fpsfont;
        ArvnRender renderer;
        double lastFrameTimestamp = 0;
        double frameTimestamp = 0;
        int minLoopTime = 1000 / 30;
        double fps;
        public void Run()
        {
            //Buffer
            IArvnImage buf1 = new ArvnBufferedBitmap(800, 600);
            IArvnImage buf2 = new ArvnBufferedBitmap(800, 600);
            drawBuffer = new ArvnDoubleBuffer();
            drawBuffer.SetBuffer(buf1, buf2);

            //Window
            fpsfont = new Font(new FontFamily("Arial"), 14);
            form = new Form
            {
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterScreen
            };
            form.Show();

            //Prepare
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 600);
            ArvnMesh model = new ArvnMesh();
            model.ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
            IArvnImage texture = new ArvnBufferedBitmap(50, 50);
            texture.Load("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\texture.jpg");

            float[] light = ArvnCore.Normalize(new float[] { 1, 1, 1 });
            float[,] projection = ArvnCore.PerspectiveMatrix(3.14159f / 3, 800 / 600f, 0.01f, 100f);
            float[,] modelview = ArvnCore.LookAt(new float[] { 1, 1, 2 }, new float[] { 0, 0, 0 }, new float[] { 0, 1, 0 });
            float[,] viewport = ArvnCore.RectViewportMatrix3D(800, 600, 1, 1);

            object[] vertex = model.ExportVertices();
            object[] vertexNormal = model.ExportVertexNormals();
            int[] faceIndices = model.ExportFaceIndexes();

            ArvnShader shader = new ArvnShaderL9S5();
            shader.SetVariable("viewport", viewport);
            shader.SetVariable("projection", projection);
            shader.SetVariable("modelview", modelview);
            shader.SetVariable("light", light);

            shader.SetAttributeVariable("a_vert", vertex);
            shader.SetAttributeVariable("a_normal", vertexNormal);

            renderer = ArvnRender.Create();

            float t = 0;
            //Render Loop
            while (true)
            {
                t += 0.01f;

                float radius = 3f;
                float dx = (float) Math.Cos(t) * radius;
                float dz = (float) Math.Sin(t) * radius;
                modelview = ArvnCore.LookAt(new float[] { dx, 1, dz }, new float[] { 0, 0, 0 }, new float[] { 0, 1, 0 });
                shader.SetVariable("modelview", modelview);

                lastFrameTimestamp = frameTimestamp;
                frameTimestamp = ArvnTime.GetMiliSecond();
                fps = 1000.0f / (frameTimestamp - lastFrameTimestamp);

                //Render
                IArvnImage curBuf = drawBuffer.GetDrawingBuffer();
                curBuf.Clear(ArvnCore.RGBToHex(0, 0, 0));
                zbuf.Reset();
                renderer.RasterizeTriangles3D(faceIndices, ref shader, ref curBuf, ref zbuf);
                RenderFPS();

                //End of Render
                drawBuffer.SwapBuffer();
                Display();
                Application.DoEvents();
                if (ArvnTime.GetMiliSecond() - frameTimestamp < minLoopTime)
                {
                   //Thread.Sleep(minLoopTime - (ArvnTime.GetMiliSecond() - frameTimestamp));
                }
            }

        }
        public void Display()
        {
            using (Graphics g = form.CreateGraphics())
            {
                Bitmap t = (Bitmap)drawBuffer.GetDisplayBuffer().GetImage();
                g.DrawImage(t, Point.Empty);
            }
        }

        public void RenderFPS()
        {
            using (var g = Graphics.FromImage((Bitmap)drawBuffer.GetDrawingBuffer().GetImage()))
            {
                g.DrawString("FPS: " + ((int)(fps * 100)) / 100.0, fpsfont, Brushes.White, 0, 0);
            }
            drawBuffer.GetDrawingBuffer().SyncFromImage();
        }

    }
}
