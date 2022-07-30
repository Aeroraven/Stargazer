using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer
{
    class ArvnVec2i
    {
        public int x;
        public int y;
        public void Set(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
        static public ArvnVec2i Create(int x,int y)
        {
            ArvnVec2i t = new ArvnVec2i();
            t.x = x;
            t.y = y;
            return t;
        }
    }
}
