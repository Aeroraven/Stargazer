using System;
using System.Collections.Generic;
using System.Text;
using TinyRenderer.Core;
using TinyRenderer.Core.Drawing;
using TinyRenderer.Core.Render;
using TinyRenderer.Display;

namespace TinyRenderer.Shaders
{
    class ArvnPlainTexturedShader : ArvnShader
    {
        private float[,] modelview;
        private float[,] projection;
        private float[,] viewport;
        private float[,] pm;
        private IArvnImage diffuseTexture;
        public ArvnPlainTexturedShader() : base()
        {
            //Uniforms
            DefineVariable("modelview", tpMat4f, new float[4, 4]);
            DefineVariable("projection", tpMat4f, new float[4, 4]);
            DefineVariable("viewport", tpMat4f, new float[4, 4]);
            DefineVariable("diffuse_texture", tpSampler2d, new ArvnBufferedBitmap(1, 1));

            //Varyings
            DefineVaryingVariable("v_uv", tpVec2f);


            //Attributes
            DefineAttributeVariable("vertices", tpVec3f);
            DefineAttributeVariable("vtexture", tpVec2f);
        }
        public override void ComputeDerivedVariables()
        {
            if (FindIsUniformChanged())
            {
                viewport = (float[,])GetVariable("viewport");
                modelview = (float[,])GetVariable("modelview");
                projection = (float[,])GetVariable("projection");
                pm = ArvnCore.MatrixMultiply(viewport, projection, modelview);
                diffuseTexture = (IArvnImage)GetVariable("diffuse_texture");
                SetUniformChangedState();
            }
        }

        public override void FragmentShader()
        {
            ComputeDerivedVariables();
            float[] vUv = (float[])GetVaryingVariable("v_uv");
            float[] c = diffuseTexture.GetInNormalizedEx(vUv[0], vUv[1]);
            SetInternalVariable(arFragColor, new float[] { c[0], c[1], c[2], 1 });
        }

        public override void VertexShader()
        {
            ComputeDerivedVariables();
            float[] v = (float[])GetAttributeVariable("vertices");
            float[] vx = new float[4];
            ArvnCore.HomogeneousLinearTransform3DToCartesian(pm, v[0], v[1], v[2], 1, out vx[0], out vx[1], out vx[2]);
            float[] vt = (float[])GetAttributeVariable("vtexture");
            SetVaryingVariable("v_uv", new float[] { vt[0], vt[1] });
            SetInternalVariable(arPosition, vx);

        }
    }
}
