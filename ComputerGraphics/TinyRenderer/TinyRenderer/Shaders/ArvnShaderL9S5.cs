using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyRenderer.Core;
using TinyRenderer.Core.Render;
using TinyRenderer.Core.Drawing;

namespace TinyRenderer.Shaders
{
    class ArvnShaderL9S5 : ArvnShader
    {
        private float[,] viewport, projection, modelview, model_it, trans;
        private float[] light,lightT;

        public ArvnShaderL9S5() : base()
        {
            DefineVariable("viewport", tpMat4f, new float[4, 4]);
            DefineVariable("projection", tpMat4f, new float[4, 4]);
            DefineVariable("modelview", tpMat4f, new float[4, 4]);
            DefineVariable("light", tpVec3f, new float[3]);

            DefineVaryingVariable("v_normal", tpVec3f);

            DefineAttributeVariable("a_normal", tpVec3f);
            DefineAttributeVariable("a_vert", tpVec3f);
        }
        public override void ComputeDerivedVariables()
        {
            if (FindIsUniformChanged())
            {
                viewport = (float[,])GetVariable("viewport");
                projection = (float[,])GetVariable("projection");
                modelview = (float[,])GetVariable("modelview");
                light = (float[])GetVariable("light");

                lightT = new float[3];
                ArvnCore.HomogeneousLinearTransform3D(modelview, light[0], light[1], light[2], 0, out lightT[0], out lightT[1], out lightT[2], out _);
                ArvnCore.NormalizeSelfWd3(ref lightT);
                model_it = ArvnCore.InverseTransposedMatrix(modelview);
                trans = ArvnCore.MatrixMultiply(viewport, projection, modelview);
                SetUniformChangedState();
            }
        }

        public override void FragmentShader()
        {
            float[] vNormal = (float[])GetVaryingVariable("v_normal");
            ArvnCore.NormalizeSelfWd3(ref vNormal);
            float diff = ArvnCore.DotProduct(vNormal, lightT);
            if (diff < 0)
            {
                diff = 0;
            }
            SetInternalVariable(arFragColor, Vec4f(diff, diff, diff, diff));
        }

        public override void VertexShader()
        {
            float[] aNormal = (float[])GetAttributeVariable("a_normal");
            float[] aVert = (float[])GetAttributeVariable("a_vert");

            float[] aVertT = new float[4];
            ArvnCore.HomogeneousLinearTransform3DToCartesian(trans, aVert[0], aVert[1], aVert[2], 1, out aVertT[0], out aVertT[1], out aVertT[2]);

            float[] aNormalT = new float[3];
            ArvnCore.HomogeneousLinearTransform3DToCartesian(model_it, aNormal[0], aNormal[1], aNormal[2], 0, out aNormalT[0], out aNormalT[1], out aNormalT[2]);

            SetVaryingVariable("v_normal", aNormalT);
            SetInternalVariable(arPosition, aVertT);
        }
    }
}
