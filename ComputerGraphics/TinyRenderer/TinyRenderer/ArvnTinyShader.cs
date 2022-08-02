using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer
{
    class ArvnTinyShader : ArvnShader
    {
        public ArvnTinyShader() : base()
        {
            //Uniforms
            DefineVariable("modelview", "mat4f", new float[4, 4]);
            DefineVariable("projection", "mat4f", new float[4, 4]);
            DefineVariable("viewport", "mat4f", new float[4, 4]);
            DefineVariable("lightdir", "vec3f", new float[3]);

            //Varying
            DefineVaryingVariable("intensity", "float");

            //Attributes
            DefineAttributeVariable("vertices", "vec3");
            DefineAttributeVariable("vnormals", "vec3");
        }
        public override void FragmentShader(params object[] input)
        {
            float intensity = (float)GetVaryingVariable("intensity", 0) + (float)GetVaryingVariable("intensity", 1) + (float)GetVaryingVariable("intensity", 2);
            intensity /= 3;
            if (intensity <= 0)
            {
                SetVariable("arFragColor", Vec4f(1, 0, 0, 1));
            }
            else
            {
                SetVariable("arFragColor", Vec4f(intensity, intensity, intensity, 1));
            }
            
        }

        public override void VertexShader(int index, int vindex, params object[] input)
        {
            //Accepts coord / vn
            float[,] viewport = (float[,])GetVariable("viewport");
            float[,] projection = (float[,])GetVariable("projection");
            float[,] model = (float[,])GetVariable("modelview");
            float[] lightdir = (float[])GetVariable("lightdir");

            float[] vertex = (float[])GetAttributeVariable("vertices")[index];
            float[] vertexp = new float[4];
            float[] normal = (float[])GetAttributeVariable("vnormals")[index];

            float[,] transformed = ArvnCore.MatrixMultiply(viewport, projection, model);
            ArvnCore.HomogeneousLinearTransform3DToCartesian(transformed, vertex[0], vertex[1], vertex[2], 1, out vertexp[0], out vertexp[1], out vertexp[2]);
            vertexp[3] = 1;
            SetVariable("arPosition", vertexp);

            float intensity = ArvnCore.DotProduct(normal, lightdir);
            intensity = Math.Max(0f, intensity);
            SetVaryingVariable("intensity", vindex, intensity);

        }
    }
}
