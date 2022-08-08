﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer
{
    interface IArvnShaderCaller
    {
        public void VertexShader(int index, int vindex, params object[] input);
        public object GetVariable(string varName);
        public void FragmentShader(float[] barycenterCoord, params object[] input);
    }
}
