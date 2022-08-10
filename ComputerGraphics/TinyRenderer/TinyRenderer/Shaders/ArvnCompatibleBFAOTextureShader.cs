using System;
using System.Collections.Generic;
using System.Text;
using TinyRenderer.Core;
using TinyRenderer.Core.Drawing;
using TinyRenderer.Display;
using TinyRenderer.Legacy;

namespace TinyRenderer.Shaders
{
    class ArvnCompatibleBFAOTextureShader : ArvnCompatibleShader
    {
        private float[,] viewport;
        private float[,] depthRemap;
        private int iteration;
        private IArvnImage depthMap;
        private IArvnImage aoMap;
        public ArvnCompatibleBFAOTextureShader() : base()
        {
            //Uniforms
            DefineVariable("viewport", "mat4f", new float[4, 4]);
            DefineVariable("iteration", "int", 0);
            DefineVariable("depth_map", "sampler2d", new ArvnBufferedBitmap(1, 1));
            DefineVariable("ao_map", "sampler2d", new ArvnBufferedBitmap(1, 1));
            DefineVariable("depth_remap", "mat4f", new float[4, 4]);

            //Varying
            DefineVaryingVariable("ipc_v", "vec3f");
            DefineVaryingVariable("uv_v", "vec2f");

            //Attribute
            DefineAttributeVariable("vertices", "vec3f");
            DefineAttributeVariable("vtexture", "vec2f");
        }
        public override void ComputeDerivedUniforms()
        {
            if (FindIsUniformChanged())
            {
                viewport = (float[,])GetVariable("viewport");
                depthRemap = (float[,])GetVariable("depth_remap");
                depthMap = (IArvnImage)GetVariable("depth_map");
                aoMap = (IArvnImage)GetVariable("ao_map");
                iteration = (int)GetVariable("iteration");
                SetUniformChangedState();
            }
        }

        public override void FragmentShader(float[] barycenterCoord, params object[] input)
        {
            ComputeDerivedUniforms();

            //UV Interpolation
            float uvx = 0, uvy = 0;
            for (int i = 0; i < 3; i++)
            {
                uvx += ((float[])GetVaryingVariable("uv_v", i))[0] * barycenterCoord[i];
                uvy += ((float[])GetVaryingVariable("uv_v", i))[1] * barycenterCoord[i];
            }

            //Shadow Mapping
            float cvx = 0, cvy = 0, cvz = 0;
            float cvxt, cvyt, cvzt;
            for (int i = 0; i < 3; i++)
            {
                cvx += ((float[])GetVaryingVariable("ipc_v", i))[0] * barycenterCoord[i];
                cvy += ((float[])GetVaryingVariable("ipc_v", i))[1] * barycenterCoord[i];
                cvz += ((float[])GetVaryingVariable("ipc_v", i))[2] * barycenterCoord[i];
            }
            ArvnCore.HomogeneousLinearTransform3DToCartesian(depthRemap, cvx, cvy, cvz, 1, out cvxt, out cvyt, out cvzt);
            if(cvxt> depthMap.GetWidth() || cvxt < 0 || cvyt > depthMap.GetHeight()|| cvyt < 0){
                SetVariable("arFragColor", Vec4f(0, 0, 0, 0));
                return ;
            }
            float shadowDepth;
            int sr, sg, sb;
            int shadowMapColor = depthMap.Get((int)cvxt, (int)cvyt);
            ArvnCore.HexToRGB(shadowMapColor, out sr, out sg, out sb);
            shadowDepth = sr / 255f * 2f - 1f;

            //Reweighting
            int orgAoHex = aoMap.Get((int)uvx, (int)uvy);
            int orgAoPower;
            ArvnCore.HexToRGB(orgAoHex, out orgAoPower, out _, out _);
            
            if (shadowDepth <= cvzt)
            {
                float rp = (1.0f * orgAoPower / 255f * iteration + 1) / (iteration + 1f);
                SetVariable("arFragColor", Vec4f(rp, rp, rp, rp));
            }
            else
            {
                float rp = (1.0f * orgAoPower / 255f * iteration + 0) / (iteration + 1f);
                SetVariable("arFragColor", Vec4f(rp, rp, rp, rp));
            }
        }

        public override void VertexShader(int index, int vindex, params object[] input)
        {
            ComputeDerivedUniforms();

            float[] vertex = (float[])GetAttributeVariable("vertices")[index];
            float[] vtexture = (float[])GetAttributeVariable("vtexture")[index];

            float[] vtExpanded = new float[] { vtexture[0], vtexture[1], 1 };
            float[] vtExpandedTransformed = new float[4];
            ArvnCore.HomogeneousLinearTransform3DToCartesian(viewport, vtExpanded[0], vtExpanded[1], vtExpanded[2], 1,out vtExpandedTransformed[0], out vtExpandedTransformed[1], out vtExpandedTransformed[2]);


            SetVariable("arPosition", vtExpandedTransformed);
            SetVaryingVariable("ipc_v", vindex, new float[] { vertex[0], vertex[1], vertex[2] });
            SetVaryingVariable("uv_v", vindex, new float[] { vtExpandedTransformed[0], vtExpandedTransformed[1] });
        }
    }
}
