using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer.Shaders
{
    class ArvnDepthShader : ArvnShader
    {
        private float[,] pm;
        private float[,] pmi;
        private float[,] modelview;
        private float[,] projection;
        private float[,] viewport;
        private float[,] transformed;
        private float[,] transformed_ndc;
        public ArvnDepthShader() : base()
        {
            //Uniforms
            DefineVariable("modelview", "mat4f", new float[4, 4]);
            DefineVariable("projection", "mat4f", new float[4, 4]);
            DefineVariable("viewport", "mat4f", new float[4, 4]);

            //Varying
            DefineVaryingVariable("svertex", "vec3f");

            //Attributes
            DefineAttributeVariable("vertices", "vec3f");
        }
        public override void ComputeDerivedUniforms()
        {
            if (FindIsUniformChanged())
            {
                viewport = (float[,])GetVariable("viewport");
                modelview = (float[,])GetVariable("modelview");
                projection = (float[,])GetVariable("projection");

                transformed = ArvnCore.MatrixMultiply(viewport, projection, modelview);
                transformed_ndc = ArvnCore.MatrixMultiply(projection, modelview);
            }
        }
        public override void FragmentShader(float[] barycenterCoord, params object[] input)
        {
            ComputeDerivedUniforms();
            float[] ndca = (float[])GetVaryingVariable("svertex", 0);
            float[] ndcb = (float[])GetVaryingVariable("svertex", 1);
            float[] ndcc = (float[])GetVaryingVariable("svertex", 2);
            float[] ip = new float[3];
            for(int i = 0; i < 3; i++)
            {
                ip[i] = ndca[i] * barycenterCoord[0] + ndcb[i] * barycenterCoord[1] + ndcc[i] * barycenterCoord[2];
            }
            float depth = (ip[2] + 1) / 2f;
            if (depth < 0 || depth > 1)
            {
                depth = 0;
            }
            float[] color = Vec4f(depth, depth, depth, 1);

            SetVariable("arFragColor", color);
        }

        public override void VertexShader(int index, int vindex, params object[] input)
        {
            ComputeDerivedUniforms();

            float[] vertex = (float[])GetAttributeVariable("vertices")[index];
            float[] vertexp = new float[4];

            ArvnCore.HomogeneousLinearTransform3DToCartesian(transformed, vertex[0], vertex[1], vertex[2], 1, out vertexp[0], out vertexp[1], out vertexp[2]);
            vertexp[3] = 1;
            SetVariable("arPosition", vertexp);
            SetVaryingVariable("svertex", vindex, new float[] { vertexp[0], vertexp[1], vertexp[2] });
        }
    }
}
