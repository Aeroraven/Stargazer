using System;
using System.Windows.Forms;
using TinyRenderer.Core.Application;
using TinyRenderer.Core;
using TinyRenderer.Core.Render;
using TinyRenderer.Core.Drawing;
using TinyRenderer.Utility;
using System.Drawing;
using TinyRenderer.Display;
using TinyRenderer.Shaders;
using System.Threading;

namespace TinyRenderer.Applications
{
    class ArvnAppL10S1 : IArvnApplication
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
            IArvnImage pImg = new ArvnBufferedBitmap(800, 600);
            ArvnZBuffer zbuf = ArvnZBuffer.Create(800, 600);
            ArvnZBuffer zbufPost = ArvnZBuffer.Create(800, 600);
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

            object[] vertex2 = { new float[] { 0, 0, 1 }, new float[] { 800, 0, 1 }, new float[] { 800, 600, 1 }, new float[] { 0, 600, 1 } };
            int[] faceIndices2 = { 0, 1, 2, 2, 3, 0 };

            ArvnShader shader = new ArvnShaderL9S5();
            shader.SetVariable("viewport", viewport);
            shader.SetVariable("projection", projection);
            shader.SetVariable("modelview", modelview);
            shader.SetVariable("light", light);
            shader.SetAttributeVariable("a_vert", vertex);
            shader.SetAttributeVariable("a_normal", vertexNormal);

            ArvnShader postShader = new ArvnGaussianShaderL10S1();
            postShader.SetVariable("sigma", 3f);
            postShader.SetVariable("image", pImg);
            postShader.SetAttributeVariable("vertex", vertex2);

            renderer = ArvnRender.Create();

            //Render Loop
            while (true)
            {
                lastFrameTimestamp = frameTimestamp;
                frameTimestamp = ArvnTime.GetMiliSecond();
                fps = 1000.0f / (frameTimestamp - lastFrameTimestamp);

                //Render
                IArvnImage curBuf = drawBuffer.GetDrawingBuffer();
                pImg.Clear(ArvnCore.RGBToHex(0, 0, 0));
                curBuf.Clear(ArvnCore.RGBToHex(0, 0, 0));

                zbuf.Reset();
                zbufPost.Reset();
                Console.WriteLine("First Pass");
                renderer.RasterizeTriangles3D(faceIndices, ref shader, ref pImg, ref zbuf);

                Console.WriteLine("Second Pass");
                renderer.RasterizeTriangles3D(faceIndices2, ref postShader, ref curBuf, ref zbufPost);

                RenderFPS();

                //End of Render
                drawBuffer.SwapBuffer();
                Display();
                Application.DoEvents();
                if (ArvnTime.GetMiliSecond() - frameTimestamp < minLoopTime)
                {
                    Thread.Sleep((int)(minLoopTime - (ArvnTime.GetMiliSecond() - frameTimestamp)));
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
