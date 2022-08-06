using System;
using System.Collections.Generic;
using System.Text;

namespace TinyRenderer
{
    //Shader Using Phong Shading & Normal Mapping
    class ArvnTinyShaderV3 : ArvnShader
    {
        private float[,] pm;
        private float[,] pmi;
        private float[,] modelview;
        private float[,] projection;
        private float[,] viewport;
        private float[] lightdir;
        private float[] lightdir_t;

        private float[,] transformed;
        private float[,] transformed_ndc;

        private ArvnImage diffuseTexture;
        private ArvnImage specTexture;
        private ArvnImage normalTexture;
        public ArvnTinyShaderV3() : base()
        {
            //Uniforms
            DefineVariable("modelview", "mat4f", new float[4, 4]);
            DefineVariable("projection", "mat4f", new float[4, 4]);
            DefineVariable("viewport", "mat4f", new float[4, 4]);
            DefineVariable("lightdir", "vec3f", new float[3]);
            DefineVariable("diffuse_texture", "sampler2d", new ArvnImageBitmap(1, 1));
            DefineVariable("spec_texture", "sampler2d", new ArvnImageBitmap(1, 1));
            DefineVariable("normal_texture", "sampler2d", new ArvnImageBitmap(1, 1));

            DefineVariable("version", "int", 1);

            DefineVariable("projection_model", "mat4f", new float[4, 4]);
            DefineVariable("projection_model_inverse", "mat4f", new float[4, 4]);

            //Varying
            DefineVaryingVariable("ndc_v", "vec3f");
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
                float temp;
                viewport = (float[,])GetVariable("viewport");
                modelview = (float[,])GetVariable("modelview");
                projection = (float[,])GetVariable("projection");
                lightdir = (float[])GetVariable("lightdir");
                transformed = ArvnCore.MatrixMultiply(viewport, projection, modelview);
                transformed_ndc = ArvnCore.MatrixMultiply(projection, modelview);

                pm = ArvnCore.MatrixMultiply(ArvnCore.IdentityMatrix(4), modelview);
                pmi = ArvnCore.InverseTransposedMatrix(pm);
                SetVariable("projection_model", pm);
                SetVariable("projection_model_inverse", pmi);

                lightdir_t = new float[3];
                ArvnCore.HomogeneousLinearTransform3D(pm, lightdir[0], lightdir[1], lightdir[2], 0, out lightdir_t[0], out lightdir_t[1], out lightdir_t[2], out temp);
                lightdir_t = ArvnCore.Normalize(lightdir_t);

                

                diffuseTexture = (ArvnImage)GetVariable("diffuse_texture");
                specTexture = (ArvnImage)GetVariable("spec_texture");
                normalTexture = (ArvnImage)GetVariable("normal_texture");

                SetUniformChangedState();
            }
        }

        public override void FragmentShader(float[] barycenterCoord, params object[] input)
        {
            ComputeDerivedUniforms();

            float temp;            

            //Texture Interpolation
            float uvx = 0, uvy = 0;
            uvx += ((float[])GetVaryingVariable("diffuse_uv", 0))[0] * barycenterCoord[0];
            uvx += ((float[])GetVaryingVariable("diffuse_uv", 1))[0] * barycenterCoord[1];
            uvx += ((float[])GetVaryingVariable("diffuse_uv", 2))[0] * barycenterCoord[2];
            uvy += ((float[])GetVaryingVariable("diffuse_uv", 0))[1] * barycenterCoord[0];
            uvy += ((float[])GetVaryingVariable("diffuse_uv", 1))[1] * barycenterCoord[1];
            uvy += ((float[])GetVaryingVariable("diffuse_uv", 2))[1] * barycenterCoord[2];
            int texColor = diffuseTexture.GetInNormalized(uvx, uvy);

            //Face Normal
            float[] oNormal = new float[3];
            for (int i = 0; i < 3; i++)
            {
                float[] normal = (float[])GetVaryingVariable("diffuse_uv_normal", i);
                float[] normal_t = new float[3];
                ArvnCore.HomogeneousLinearTransform3D(pmi, normal[0], normal[1], normal[2], 0, out normal_t[0], out normal_t[1], out normal_t[2], out temp);
                normal_t = ArvnCore.Normalize(normal_t);
                oNormal[0] += barycenterCoord[i] * normal_t[0];
                oNormal[1] += barycenterCoord[i] * normal_t[1];
                oNormal[2] += barycenterCoord[i] * normal_t[2];
            }
            oNormal = ArvnCore.Normalize(oNormal);

            //Normal Mapping Transform
            float[] ndca = (float[])GetVaryingVariable("ndc_v", 0);
            float[] ndcb = (float[])GetVaryingVariable("ndc_v", 1);
            float[] ndcc = (float[])GetVaryingVariable("ndc_v", 2);
            float[] uva = (float[])GetVaryingVariable("diffuse_uv", 0);
            float[] uvb = (float[])GetVaryingVariable("diffuse_uv", 1);
            float[] uvc = (float[])GetVaryingVariable("diffuse_uv", 2);
            float[,] tsTransform = ArvnCore.TangentSpaceBaseMatrix(ndca, ndcb, ndcc, oNormal, uva, uvb, uvc);

            float tnr, tng, tnb;
            float[] tNormal = new float[3];
            normalTexture.GetInNormalized(uvx, uvy, out tnr, out tng, out tnb);
            tnr = tnr * 2 - 1;
            tng = tng * 2 - 1;
            tnb = tnb * 2 - 1;
            ArvnCore.CartesianLinearTransform3D(tsTransform, tnr, tng, tnb, out tNormal[0], out tNormal[1], out tNormal[2]);
            ArvnCore.NormalizeSelf(ref tNormal);

            //Diffuse
            float intensity = Math.Max(0f, ArvnCore.DotProduct(tNormal, lightdir_t));


            //Specular
            float[] specLight = ArvnCore.SpecularReflection(lightdir_t, tNormal);
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
            for (int i = 0; i < 3; i++)
            {
                color[i] = Math.Min(1.0f, 0.02f + color[i] * (0.78f * intensity + 0.2f * specPower));
                //color[i] = Math.Min(1.0f, color[i] * intensity);
            }

            SetVariable("arFragColor", color);
        }

        public override void VertexShader(int index, int vindex, params object[] input)
        {
            ComputeDerivedUniforms();

            float[] vertex = (float[])GetAttributeVariable("vertices")[index];
            float[] vertexp = new float[4];
            float[] vertexpp = new float[3];

            ArvnCore.HomogeneousLinearTransform3DToCartesian(transformed, vertex[0], vertex[1], vertex[2], 1, out vertexp[0], out vertexp[1], out vertexp[2]);
            vertexp[3] = 1;
            SetVariable("arPosition", vertexp);

            float[] vtexture = (float[])GetAttributeVariable("vtexture")[index];
            SetVaryingVariable("diffuse_uv", vindex, new float[] { vtexture[0], vtexture[1] });

            float[] vnormal = (float[])GetAttributeVariable("vnormals")[index];
            SetVaryingVariable("diffuse_uv_normal", vindex, new float[] { vnormal[0], vnormal[1], vnormal[2] });

            ArvnCore.HomogeneousLinearTransform3DToCartesian(transformed_ndc, vertex[0], vertex[1], vertex[2], 1, out vertexpp[0], out vertexpp[1], out vertexpp[2]);
            SetVaryingVariable("ndc_v", vindex, new float[] { vertexpp[0], vertexpp[1], vertexpp[2] });
        }
    }
}
