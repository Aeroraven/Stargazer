using System;
using System.Collections.Generic;
using System.Text;
using TinyRenderer.Core;
using TinyRenderer.Display;

namespace TinyRenderer.Shaders
{
    //Shader Using Phong Shading
    class ArvnTinyShaderV2 : ArvnCompatibleShader
    {
        private float[,] pm;
        public ArvnTinyShaderV2() : base()
        {
            //Uniforms
            DefineVariable("modelview", "mat4f", new float[4, 4]);
            DefineVariable("projection", "mat4f", new float[4, 4]);
            DefineVariable("viewport", "mat4f", new float[4, 4]);
            DefineVariable("lightdir", "vec3f", new float[3]);
            DefineVariable("diffuse_texture", "sampler2d", new ArvnImageBitmap(1, 1));
            DefineVariable("spec_texture", "sampler2d", new ArvnImageBitmap(1, 1));

            DefineVariable("version", "int", 1);

            DefineVariable("projection_model", "mat4f", new float[4, 4]);
            DefineVariable("projection_model_inverse", "mat4f", new float[4, 4]);

            //Varying
            DefineVaryingVariable("diffuse_uv", "vec2f");
            DefineVaryingVariable("diffuse_uv_normal", "vec3f");

            //Attributes
            DefineAttributeVariable("vertices", "vec3f");
            DefineAttributeVariable("vnormals", "vec3f");
            DefineAttributeVariable("vtexture", "vec2f");
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

        public override void FragmentShader(float[] barycenterCoord, params object[] input)
        {
            ComputeDerivedUniforms();

            float[,] pm = (float[,])GetVariable("projection_model");
            float[,] pmi = (float[,])GetVariable("projection_model_inverse");
            IArvnImage diffuseTexture = (IArvnImage)GetVariable("diffuse_texture");
            IArvnImage specTexture = (IArvnImage)GetVariable("spec_texture");

            float[] lightdir_t = new float[3];
            float[] relightdir_t = new float[3];
            float temp;

            //Light
            float[] lightdir = (float[])GetVariable("lightdir");
            ArvnCore.HomogeneousLinearTransform3D(pm, lightdir[0], lightdir[1], lightdir[2], 0, out lightdir_t[0], out lightdir_t[1], out lightdir_t[2], out temp);
            lightdir_t = ArvnCore.Normalize(lightdir_t);

            //Texture Interpolation
            float uvx = 0, uvy = 0;
            uvx += ((float[])GetVaryingVariable("diffuse_uv", 0))[0] * barycenterCoord[0];
            uvx += ((float[])GetVaryingVariable("diffuse_uv", 1))[0] * barycenterCoord[1];
            uvx += ((float[])GetVaryingVariable("diffuse_uv", 2))[0] * barycenterCoord[2];
            uvy += ((float[])GetVaryingVariable("diffuse_uv", 0))[1] * barycenterCoord[0];
            uvy += ((float[])GetVaryingVariable("diffuse_uv", 1))[1] * barycenterCoord[1];
            uvy += ((float[])GetVaryingVariable("diffuse_uv", 2))[1] * barycenterCoord[2];
            int texColor = diffuseTexture.GetInNormalized(uvx, uvy);

            //Normal & Diffuse
            float[] oNormal = new float[3];
            float intensity = 0;
            for (int i = 0; i < 3; i++)
            {
                float[] normal = (float[])GetVaryingVariable("diffuse_uv_normal", i);
                float[] normal_t = new float[3];
                ArvnCore.HomogeneousLinearTransform3D(pmi, normal[0], normal[1], normal[2], 0, out normal_t[0], out normal_t[1], out normal_t[2], out temp);
                normal_t = ArvnCore.Normalize(normal_t);
                

                oNormal[0] += barycenterCoord[i] * normal_t[0];
                oNormal[1] += barycenterCoord[i] * normal_t[1];
                oNormal[2] += barycenterCoord[i] * normal_t[2];

                intensity += barycenterCoord[i] * Math.Max(0f, ArvnCore.DotProduct(normal_t, lightdir_t));
            }

            //Specular
            oNormal = ArvnCore.Normalize(oNormal);
            float[] specLight = ArvnCore.SpecularReflection(lightdir_t, oNormal);
            int specHex = specTexture.GetInNormalized(uvx, uvy);
            int specCoef, specTemp;
            ArvnCore.HexToRGB(specHex, out specCoef, out specTemp, out specTemp);
            float specPower = (float)Math.Pow(Math.Max(0, specLight[2]), 1.0f * specCoef);

            //Texture Coloring
            float[] color = Vec4f(1, 1, 1, 1);
            int tr, tg, tb;
            ArvnCore.HexToRGB(texColor, out tr, out tg, out tb);
            float trf = 1.0f * tr / 255, tgf = 1.0f * tg / 255, tbf = 1.0f * tb / 255;
            color[0] *= trf;
            color[1] *= tgf;
            color[2] *= tbf;

            //Light Model
            for(int i = 0; i < 3; i++)
            {
                color[i] = Math.Min(1.0f, 0.02f + color[i] * (0.78f*intensity + 0.2f * specPower));
            }

            SetVariable("arFragColor", color);
        }

        public override void VertexShader(int index, int vindex, params object[] input)
        {
            ComputeDerivedUniforms();
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

            float[] vtexture = (float[])GetAttributeVariable("vtexture")[index];
            SetVaryingVariable("diffuse_uv", vindex, new float[] { vtexture[0], vtexture[1] });

            float[] vnormal = (float[])GetAttributeVariable("vnormals")[index];
            SetVaryingVariable("diffuse_uv_normal", vindex, new float[] { vnormal[0], vnormal[1], vnormal[2] });
        }
    }
}
