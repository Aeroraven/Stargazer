using System;
using System.Collections.Generic;
using System.Text;
using TinyRenderer.Core;
using TinyRenderer.Core.Drawing;
using TinyRenderer.Core.Render;
using TinyRenderer.Display;

namespace TinyRenderer.Shaders
{
    //Postprocessing shader for AO
    class ArvnSSAORenderShader : ArvnShader
    {
        private static int trials = 64;
        private static float radius = 0.1f;

        private float[,] viewport;
        private IArvnImage depthMap;

        public ArvnSSAORenderShader() : base()
        {
            //Uniforms
            DefineVariable("viewport", tpMat4f, new float[4,4]);
            DefineVariable("depth_map", tpSampler2d, new ArvnBufferedBitmap(1, 1));

            //Varyings
            DefineVaryingVariable("v_uv", tpVec2f);

            //Attributes
            DefineAttributeVariable("vertex", tpVec3f);
        }
        public override void ComputeDerivedVariables()
        {
            if (FindIsUniformChanged())
            {
                viewport = (float[,])GetVariable("viewport");
                depthMap = (IArvnImage)GetVariable("depth_map");
                SetUniformChangedState();
            }
        }

        public override void FragmentShader()
        {
            //Get buffered depth for (u,v)
            float[] uv = (float[])GetVaryingVariable("v_uv");
            uv[0] = (uv[0] + 1) / 2;
            uv[1] = (uv[1] + 1) / 2;
            float[] drgb = depthMap.GetInNormalizedEx(uv[0], uv[1]);
            float bufDepth = drgb[0];

            if (bufDepth <= 0.01f)
            {
                SetInternalVariable(arFragColor, Vec4f(0, 0, 0, 0));
                return;
            }
            //Generate random samples in the unit sphere
            int occls = 0;
            int skips = 0;
            for (int i=0;i< trials; i++)
            {
                float[] rand = new float[3];
                for(int j = 0; j < 3; j++)
                {
                    rand[j] = ArvnCore.RandomUniform() * 2 - 1;
                }
                float rx = rand[0] * radius + uv[0];
                float ry = rand[1] * radius + uv[1];
                float rz = rand[2] * radius + bufDepth;
                if (rx > 1 || rx < 0 || ry > 1 || ry < 0 || rz > 1 || rz < -1)
                {
                    skips++;
                    continue;
                }

                //Determine sample's contribution
                float[] rdrgb = depthMap.GetInNormalizedEx(rx, ry);
                float rBufDepth = rdrgb[0];
                if (rz <= rBufDepth)
                {
                    occls++;
                }
            }
            if (skips == trials)
            {
                float c = 1.0f - (1.0f *  occls / trials);
                SetInternalVariable(arFragColor, Vec4f(1, 0, 0, 0));
            }
            else
            {
                float c = 1.0f - (1.0f * occls / (trials-skips));
                SetInternalVariable(arFragColor, Vec4f(c, c, c, c));
            }
            
        }

        public override void VertexShader()
        {
            float[] v = (float[])GetAttributeVariable("vertex");
            float[] vt = new float[4];
            ArvnCore.HomogeneousLinearTransform3DToCartesian(viewport, v[0], v[1], v[2], 1, out vt[0], out vt[1], out vt[2]);
            SetInternalVariable(arPosition, vt);
            SetVaryingVariable("v_uv", new float[] { v[0], v[1] });
        }
    }
}
