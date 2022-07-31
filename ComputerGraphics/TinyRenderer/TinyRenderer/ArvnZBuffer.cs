using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer
{
    class ArvnZBuffer
    {
        private int[] buf;
        private int width;
        private int height;

        static public ArvnZBuffer Create(int w,int h)
        {
            ArvnZBuffer t = new ArvnZBuffer();
            t.buf = new int[w * h];
            for(int i = 0; i < w * h; i++)
            {
                t.buf[i] = ~0;
            }
            t.width = w;
            t.height = h;
            return t;
        }
        private ArvnZBuffer() { }

        public void Set(int x,int y,int v)
        {
            buf[y * width + x] = v;
        }
        public int Get(int x,int y)
        {
            return buf[y * width + x];
        }
    }
}
