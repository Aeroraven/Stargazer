using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinyRenderer.Core.Application;

namespace TinyRenderer.Applications
{
    class ArvnAppL9S1 : IArvnApplication
    {
        public void Run()
        {
            Form form = new Form
            {
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterScreen
            };
            form.Show();
            while (true)
            {
                Application.DoEvents();
            }
        }
    }
}
