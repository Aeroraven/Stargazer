﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinyRenderer.Core.Application;
using TinyRenderer.Core;
using System.Threading;

namespace TinyRenderer.Applications
{
    class ArvnAppL9S2 : IArvnApplication
    {
        Form form;
        int lastFrameTimestamp = 0;
        int frameTimestamp = 0;
        float fps;
        public void Run()
        {
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
                Thread.Sleep(23);
                Application.DoEvents();
            }

        }
        public void Render()
        {
            using(var g = form.CreateGraphics())
            {
                g.Clear(Color.White);
                g.DrawString("FPS: " + fps, new Font(new FontFamily("Arial"), 14), Brushes.Black, 0, 0);
            }
        }
    }
}
