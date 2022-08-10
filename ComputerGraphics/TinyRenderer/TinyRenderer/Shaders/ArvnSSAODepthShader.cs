using System;
using System.Collections.Generic;
using System.Text;
using TinyRenderer.Core;
using TinyRenderer.Core.Render;

namespace TinyRenderer.Shaders
{
    class ArvnSSAODepthShader : ArvnShader
    {
        float[,] modelview;
        float[,] projection;
        float[,] viewport;
        float[,] p_ndc;
        float[,] p_screen;
        public ArvnSSAODepthShader() : base()
        {
            //Uniforms
            DefineVariable("modelview", "mat4f", new float[4, 4]);
            DefineVariable("projection", "mat4f", new float[4, 4]);
            DefineVariable("viewport", "mat4f", new float[4, 4]);

            //Varying
            DefineVaryingVariable("v_ndc", "vec3f");
            DefineVaryingVariable("v_uv", "vec2f");

            //Attributes
            DefineAttributeVariable("vertices", "vec3f");
            DefineAttributeVariable("vtexture", "vec2f");
        }
        public override void ComputeDerivedVariables()
        {
            if (FindIsUniformChanged())
            {
                modelview = (float[,])GetVariable("modelview");
                projection = (float[,])GetVariable("projection");
                viewport = (float[,])GetVariable("viewport");
                p_ndc = ArvnCore.MatrixMultiply(projection, modelview);
                p_screen = ArvnCore.MatrixMultiply(viewport, p_ndc);

                SetUniformChangedState();
            }
        }

        public override void FragmentShader()
        {
            float[] vNdc = (float[])GetVaryingVariable("v_ndc");
            float[] vUv = (float[])GetVaryingVariable("v_uv");
            float depth = (vNdc[2] + 1f)/2;
            float[] cl = Vec4f(depth, vUv[0], vUv[1], depth);
            SetVariable(arFragColor, cl);
        }

        public override void VertexShader()
        {
            float[] v = (float[])GetAttributeVariable("vertices");
            float[] vt = (float[])GetAttributeVariable("vtexture");
            float[] vNdc = new float[3];
            float[] vScr = new float[4];
            ArvnCore.HomogeneousLinearTransform3DToCartesian(p_ndc, v[0], v[1], v[2], 1, out vNdc[0], out vNdc[1], out vNdc[2]);
            ArvnCore.HomogeneousLinearTransform3DToCartesian(p_screen, v[0], v[1], v[2], 1, out vScr[0], out vScr[1], out vScr[2]);
            SetVariable(arPosition, vScr);
            SetVaryingVariable("v_ndc", vNdc);
            SetVaryingVariable("v_uv", Vec2f(vt[0], vt[1]));
        }
    }
}
