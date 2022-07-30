﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer
{
    class ArvnVec3f
    {
        public float x, y, z;
        public void Set(int x, int y,int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        static public ArvnVec3f Create(int x, int y, int z)
        {
            ArvnVec3f t = new ArvnVec3f();
            t.x = x;
            t.y = y;
            t.z = z;
            return t;
        }
        static public ArvnVec3f Create()
        {
            return ArvnVec3f.Create(0, 0, 0);
        }
    }
}
