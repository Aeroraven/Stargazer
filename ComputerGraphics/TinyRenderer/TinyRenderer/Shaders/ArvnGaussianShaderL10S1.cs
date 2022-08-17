using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyRenderer.Core.Drawing;
using TinyRenderer.Core.Render;

namespace TinyRenderer.Shaders
{
    class ArvnGaussianShaderL10S1 : ArvnShader
    {
        IArvnImage image;
        float sigma;
        float deno;
        int kernelSize;
        float[] kernelWeight;
        float coef;
        int iw, ih;
        public ArvnGaussianShaderL10S1() : base()
        {
            //Uniforms
            DefineVariable("image", tpSampler2d, null);
            DefineVariable("sigma", tpFloat, 0);

            //Varyings
            DefineVaryingVariable("v_uv", tpVec3f);

            //Attributes
            DefineAttributeVariable("vertex", tpVec3f);
        }
        public override void ComputeDerivedVariables()
        {
            if (FindIsUniformChanged())
            {
                image = (IArvnImage)GetVariable("image");
                iw = image.GetWidth();
                ih = image.GetHeight();
                sigma = (float)GetVariable("sigma");
                kernelSize = (int)(2f * sigma);
                int k = 2 * kernelSize + 1;
                kernelWeight = new float[k * k];
                deno = 1f / (2.0f * sigma * sigma + 1e-5f);
                coef = 0;
                for (int i = -kernelSize; i <= kernelSize; i++)
                {
                    for (int j = -kernelSize; j <= kernelSize; j++)
                    {
                        kernelWeight[(i + kernelSize) * k + (j + kernelSize)] = (float)Math.Exp((i * i + j * j) * deno);
                        coef += kernelWeight[(i + kernelSize) * k + (j + kernelSize)];
                    }
                }
                coef = 1 / coef;
                SetUniformChangedState();
            }
        }

        public override void FragmentShader()
        {
            float[] uvf = (float[])GetVaryingVariable("v_uv");
            int[] uv = new int[] { (int)uvf[0], (int)uvf[1] };
            int k = 2 * kernelSize + 1;
            float r = 0, g = 0, b = 0;
            unsafe
            {
                fixed(float* kernelWeightX = kernelWeight){
                    for (int i = -kernelSize; i <= kernelSize; i++)
                    {
                        for (int j = -kernelSize; j <= kernelSize; j++)
                        {
                            float cr, cg, cb;
                            int dx = i + uv[0];
                            int dy = j + uv[1];
                            if (dx >= 0 && dx < iw && dy >= 0 && dy < ih)
                            {
                                image.Get(dx, dy, out cr, out cg, out cb);
                            }
                            else
                            {
                                image.Get(uv[0], uv[1], out cr, out cg, out cb);
                            }
                            r += cr * kernelWeightX[(i + kernelSize) * k + (j + kernelSize)];
                            g += cg * kernelWeightX[(i + kernelSize) * k + (j + kernelSize)];
                            b += cb * kernelWeightX[(i + kernelSize) * k + (j + kernelSize)];
                        }
                    }
                    r *= coef;
                    g *= coef;
                    b *= coef;
                    SetInternalVariable(arFragColor, Vec4f(r, g, b, 1));
                }
            } 
            
        }

        public override void VertexShader()
        {
            float[] v = (float[])GetAttributeVariable("vertex");
            SetVaryingVariable("v_uv", v);
            SetInternalVariable(arPosition, Vec4f(v[0], v[1], v[2], 1));
        }
    }
}
