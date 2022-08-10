using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyRenderer.Core.Drawing
{
    class ArvnDoubleBuffer
    {
        private IArvnImage fore;
        private IArvnImage back;

        public IArvnImage GetDisplayBuffer()
        {
            return fore;
        }
        public IArvnImage GetDrawingBuffer()
        {
            return back;
        }
        public void SetBuffer(IArvnImage fore,IArvnImage back)
        {
            this.fore = fore;
            this.back = back;
        }
        public void SwapBuffer()
        {
            ArvnCore.Swap(ref fore, ref back);
        }
    }
}
