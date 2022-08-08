using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer.Shaders
{
    class ArvnPlainTexturedShader : ArvnShader
    {
        private float[,] modelview;
        private float[,] projection;
        private float[,] viewport;
        private float[,] pm;
        private ArvnImage diffuseTexture;
        public ArvnPlainTexturedShader() : base()
        {
            //Uniforms
            DefineVariable("modelview", "mat4f", new float[4, 4]);
            DefineVariable("projection", "mat4f", new float[4, 4]);
            DefineVariable("viewport", "mat4f", new float[4, 4]);
            DefineVariable("diffuse_texture", "sampler2d", new ArvnImageBitmap(1, 1));

            //Varyings
            DefineVaryingVariable("v_uv", "vec2f");


            //Attributes
            DefineAttributeVariable("vertices", "vec3f");
            DefineAttributeVariable("vtexture", "vec2f");
        }
        public override void ComputeDerivedVariables()
        {
            if (FindIsUniformChanged())
            {
                viewport = (float[,])GetVariable("viewport");
                modelview = (float[,])GetVariable("modelview");
                projection = (float[,])GetVariable("projection");
                pm = ArvnCore.MatrixMultiply(viewport, projection, modelview);
                diffuseTexture = (ArvnImage)GetVariable("diffuse_texture");
                SetUniformChangedState();
            }
        }

        public override void FragmentShader()
        {
            ComputeDerivedVariables();
            float[] vUv = (float[])GetVaryingVariable("v_uv");
            float[] c = diffuseTexture.GetInNormalizedEx(vUv[0], vUv[1]);
            SetVariable(arFragColor, new float[] { c[0], c[1], c[2], 1 });
        }

        public override void VertexShader()
        {
            ComputeDerivedVariables();
            float[] v = (float[])GetAttributeVariable("vertices");
            float[] vx = new float[4];
            ArvnCore.HomogeneousLinearTransform3DToCartesian(pm, v[0], v[1], v[2], 1, out vx[0], out vx[1], out vx[2]);
            float[] vt = (float[])GetAttributeVariable("vtexture");
            SetVaryingVariable("v_uv", new float[] { vt[0], vt[1] });
            SetVariable(arPosition, vx);

        }
    }
}
