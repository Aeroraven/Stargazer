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
        ArvnRender renderer;
        int lastFrameTimestamp = 0;
        int frameTimestamp = 0;
        int minLoopTime = 1000 / 30;
        float fps;
        public void Run()
        {
            //Buffer
            IArvnImage buf1 = new ArvnBitmap(800, 600);
            IArvnImage buf2 = new ArvnBitmap(800, 600);
            drawBuffer = new ArvnDoubleBuffer();
            drawBuffer.SetBuffer(buf1, buf2);

            //Window
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

            //Render Loop
            while (true)
            {


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
                    Thread.Sleep(minLoopTime - (ArvnTime.GetMiliSecond() - frameTimestamp));
                }
            }

        }
        public void Display()
        {
            using (var g = form.CreateGraphics())
            {
                g.DrawImage((Bitmap)drawBuffer.GetDisplayBuffer().GetImage(), Point.Empty);
            }
        }

        public void RenderFPS()
        {
            using (var g = Graphics.FromImage((Bitmap)drawBuffer.GetDrawingBuffer().GetImage()))
            {
                g.DrawString("FPS: " + fps, new Font(new FontFamily("Arial"), 14), Brushes.White, 0, 0);
            }
        }

    }
}
