using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer
{
    class ArvnTinyShader : ArvnShader
    {
        private float[,] pm;
        public ArvnTinyShader() : base()
        {
            //Uniforms
            DefineVariable("modelview", "mat4f", new float[4, 4]);
            DefineVariable("projection", "mat4f", new float[4, 4]);
            DefineVariable("viewport", "mat4f", new float[4, 4]);
            DefineVariable("lightdir", "vec3f", new float[3]);
            DefineVariable("diffuse_texture", "sampler2d", new ArvnImageBitmap(1, 1));

            DefineVariable("version", "int", 1);

            DefineVariable("projection_model", "mat4f", new float[4, 4]);
            DefineVariable("projection_model_inverse", "mat4f", new float[4, 4]);

            //Varying
            DefineVaryingVariable("intensity", "float");
            DefineVaryingVariable("diffuse_uv", "vec2f");

            //Attributes
            DefineAttributeVariable("vertices", "vec3f");
            DefineAttributeVariable("vnormals", "vec3f");
            DefineAttributeVariable("vtexture", "vec2f");
        }
        public override void FragmentShader(float[] barycenterCoord, params object[] input)
        { 
            ComputeDerivedUniforms();

            float intensity = 0;
            intensity += (float)GetVaryingVariable("intensity", 0) * barycenterCoord[0];
            intensity += (float)GetVaryingVariable("intensity", 1) * barycenterCoord[1];
            intensity += (float)GetVaryingVariable("intensity", 2) * barycenterCoord[2];

            int version = (int)GetVariable("version");
            float[] color = Vec4f(intensity, intensity, intensity, 1);
            if (version >= 2)
            {
                ArvnImage diffuseTexture = (ArvnImage)GetVariable("diffuse_texture");

                float uvx = 0, uvy = 0;
                
                uvx += ((float[])GetVaryingVariable("diffuse_uv", 0))[0] * barycenterCoord[0];
                uvx += ((float[])GetVaryingVariable("diffuse_uv", 1))[0] * barycenterCoord[1];
                uvx += ((float[])GetVaryingVariable("diffuse_uv", 2))[0] * barycenterCoord[2];
                uvy += ((float[])GetVaryingVariable("diffuse_uv", 0))[1] * barycenterCoord[0];
                uvy += ((float[])GetVaryingVariable("diffuse_uv", 1))[1] * barycenterCoord[1];
                uvy += ((float[])GetVaryingVariable("diffuse_uv", 2))[1] * barycenterCoord[2];
                int texColor = diffuseTexture.GetInNormalized(uvx, uvy);
                int tr, tg, tb;
                ArvnCore.HexToRGB(texColor, out tr, out tg, out tb);
                float trf = 1.0f * tr / 255, tgf = 1.0f * tg / 255, tbf = 1.0f * tb / 255;
                color[0] *= trf;
                color[1] *= tgf;
                color[2] *= tbf;
                if (float.IsNaN(color[0]) || float.IsNaN(color[1]) || float.IsNaN(color[2]))
                {
                    throw new Exception("AAA");
                }

            }
            if(float.IsNaN(color[0]) || float.IsNaN(color[1]) || float.IsNaN(color[2]))
            {
                throw new Exception("AAA");
            }
            SetVariable("arFragColor", color);
        }
        public override void ComputeDerivedUniforms()
        {
            if (FindIsUniformChanged())
            {
                float[,] projection = (float[,])GetVariable("projection");
                float[,] model = (float[,])GetVariable("modelview");
                pm = ArvnCore.MatrixMultiply(ArvnCore.IdentityMatrix(4), model);
                float[,] pmi = ArvnCore.InverseTransposedMatrix(pm);
                SetVariable("projection_model", pm);
                SetVariable("projection_model_inverse", pmi);
                SetUniformChangedState();
            }
        }

        public override void VertexShader(int index, int vindex, params object[] input)
        {
            ComputeDerivedUniforms();
            //Accepts coord / vn

            int version = (int)GetVariable("version");
            float[,] viewport = (float[,])GetVariable("viewport");
            float[,] projection = (float[,])GetVariable("projection");
            float[,] model = (float[,])GetVariable("modelview");
            float[] lightdir = (float[])GetVariable("lightdir");

            float[] vertex = (float[])GetAttributeVariable("vertices")[index];
            float[] vertexp = new float[4];
            float[] normal = (float[])GetAttributeVariable("vnormals")[index];

            float[,] pm = (float[,])GetVariable("projection_model");
            float[,] pmi = (float[,])GetVariable("projection_model_inverse");

            float[,] transformed = ArvnCore.MatrixMultiply(viewport, projection, model);

            float[] lightdir_t = new float[3];
            float[] normal_t = new float[3];
            float temp = 0;
            ArvnCore.HomogeneousLinearTransform3D(pm, lightdir[0], lightdir[1], lightdir[2], 0, out lightdir_t[0], out lightdir_t[1], out lightdir_t[2], out temp);
            ArvnCore.HomogeneousLinearTransform3D(pmi, normal[0], normal[1], normal[2], 0, out normal_t[0], out normal_t[1], out normal_t[2], out temp);
            lightdir_t = ArvnCore.Normalize(lightdir_t);
            normal_t = ArvnCore.Normalize(normal_t);

            ArvnCore.HomogeneousLinearTransform3DToCartesian(transformed, vertex[0], vertex[1], vertex[2], 1, out vertexp[0], out vertexp[1], out vertexp[2]);
            vertexp[3] = 1;
            SetVariable("arPosition", vertexp);

            float intensity;
            if(version >= 3)
            {
                intensity = ArvnCore.DotProduct(normal_t, lightdir_t);
            }
            else
            {
                intensity = ArvnCore.DotProduct(normal, lightdir);
            }
            
            intensity = Math.Max(0f, intensity);
            if(intensity > 1 || intensity < 0 || float.IsNaN(intensity)){
                throw new Exception("AAA");
            }
            SetVaryingVariable("intensity", vindex, intensity);

            if (version >= 2)
            {
                float[] vtexture = (float[])GetAttributeVariable("vtexture")[index];
                SetVaryingVariable("diffuse_uv", vindex, new float[] { vtexture[0], vtexture[1] });
            }


        }
    }
}
