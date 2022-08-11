using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer.Core.Render
{
    interface IArvnShaderCaller
    {
        void VertexShader(int index, int vindex, params object[] input);
        object GetInternalVariable(int varName);
        void FragmentShader(float[] barycenterCoord, params object[] input);

        void BeforeRunning();
    }
}
