using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinyRenderer.Core.Application;
using TinyRenderer.Core;
using TinyRenderer.Core.Drawing;
using System.Threading;
using System.Drawing;
using TinyRenderer.Display;

namespace TinyRenderer.Applications
{
    class ArvnAppL9S3 : IArvnApplication
    {
        Form form;
        ArvnDoubleBuffer drawBuffer;
        double lastFrameTimestamp = 0;
        double frameTimestamp = 0;
        double fps;
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
            while (true)
            {
                lastFrameTimestamp = frameTimestamp;
                frameTimestamp = ArvnTime.GetMiliSecond();
                fps = 1000.0f / (frameTimestamp - lastFrameTimestamp);
                Render();
                drawBuffer.SwapBuffer();
                Display();
                Thread.Sleep(23);
                Application.DoEvents();
            }

        }
        public void Display()
        {
            using(var g = form.CreateGraphics())
            {
                g.DrawImage((Bitmap)drawBuffer.GetDisplayBuffer().GetImage(), Point.Empty);
            }
        }
        public void Render()
        {
            using (var g = Graphics.FromImage((Bitmap)drawBuffer.GetDrawingBuffer().GetImage()))
            {
                g.Clear(Color.White);
                g.DrawString("FPS: " + fps, new Font(new FontFamily("Arial"), 14), Brushes.Black, 0, 0);
            }
        }
    }
}
