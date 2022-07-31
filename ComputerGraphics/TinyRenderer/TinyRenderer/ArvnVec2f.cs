using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer
{
    class ArvnVec2f
    {
        public float x;
        public float y;
        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        static public ArvnVec2f Create(float x, float y)
        {
            ArvnVec2f t = new ArvnVec2f();
            t.x = x;
            t.y = y;
            return t;
        }
    }
}
