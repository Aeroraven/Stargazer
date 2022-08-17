#include "../../../include/Core/Render/IrisRender.h"

using namespace Iris::Core::Drawing;
using namespace Iris::Core::Render;
using namespace Iris::Core;
void IrisRender::SetBackFaceCulling(bool enable) {
    doBackFaceCulling = enable;
}
void IrisRender::SetDebugMode(bool enable) {
    doDebug = enable;
}
void IrisRender::RasterizeTriangles3D(int len, int* faceIndex, IrisShader& shader, Iris::Core::Drawing::IIrisImage& image, IrisZBuffer& zbuf) {
	shader.BeforeRunning();
	Vecf v[3];
    Vecf da, db, dc;
    da = IrisCore::CreateVector(3);
    db = IrisCore::CreateVector(3);
    dc = IrisCore::CreateVector(3);

	for (int i = 0; i < 3; i++) {
		v[i] = IrisCore::CreateVector(3);
	}
	for (int i = 0; i < len; i+=3) {
		for (int j = 0; j < 3; j++) {
			shader.VertexShaderCall(faceIndex[i + j], j);
			for (int k = 0; k < 3; k++) {
				v[j][k] = ((Vecf)shader.GetInternalVariable(shader.arPosition))[k];
			}
		}
        if(false){
            IrisCore::DebugVector(3,v[0]);
            IrisCore::DebugVector(3,v[1]);
            IrisCore::DebugVector(4,v[2]);
        }
        if (doBackFaceCulling) {
            //Back face culling
            da[0] = v[0][0] - v[1][0];
            da[1] = v[0][1] - v[1][1];
            da[2] = v[0][2] - v[1][2];
            db[0] = v[1][0] - v[2][0];
            db[1] = v[1][1] - v[2][1];
            db[2] = v[1][2] - v[2][2];
            IrisCore::CrossProduct(da, db, dc);
            if (dc[2] > 0) {
                TriangleFragProcess3D(v[0], v[1], v[2], shader, image, zbuf);
            }
        }else{
            TriangleFragProcess3D(v[0], v[1], v[2], shader, image, zbuf);
        }
    }
	for (int i = 0; i < 3; i++) {
		delete[] v[i];
	}
}
void IrisRender::TriangleFragProcess3D(Vecf t0, Vecf t1, Vecf t2, IrisShader& shader, Iris::Core::Drawing::IIrisImage& image, IrisZBuffer& zbuf) {
    float maxZ = IrisCore::Max(t0[2], t1[2]);
    maxZ = IrisCore::Max(maxZ, t2[2]);

    float bboxMaxx = 0, bboxMaxy = 0;
    float bboxMinx = 1e10f, bboxMiny = 1e10f;

    bboxMaxx = IrisCore::Max(bboxMaxx, t0[0]);
    bboxMaxx = IrisCore::Max(bboxMaxx, t1[0]);
    bboxMaxx = IrisCore::Max(bboxMaxx, t2[0]);
    bboxMaxx = IrisCore::Min(bboxMaxx, image.GetWidth() - 1.0f);

    bboxMaxy = IrisCore::Max(bboxMaxy, t0[1]);
    bboxMaxy = IrisCore::Max(bboxMaxy, t1[1]);
    bboxMaxy = IrisCore::Max(bboxMaxy, t2[1]);
    bboxMaxy = IrisCore::Min(bboxMaxy, image.GetHeight() - 1.0f);

    bboxMinx = IrisCore::Min(bboxMinx, t0[0]);
    bboxMinx = IrisCore::Min(bboxMinx, t1[0]);
    bboxMinx = IrisCore::Min(bboxMinx, t2[0]);
    bboxMinx = IrisCore::Max(bboxMinx, 0.0f);

    bboxMiny = IrisCore::Min(bboxMiny, t0[1]);
    bboxMiny = IrisCore::Min(bboxMiny, t1[1]);
    bboxMiny = IrisCore::Min(bboxMiny, t2[1]);
    bboxMiny = IrisCore::Max(bboxMiny, 0.0f);

    int xm = (int)bboxMaxx;
    int ym = (int)bboxMaxy;
    int ys = (int)bboxMiny;
    float ta, tb, tc, tw, x0, y0, z0, x1, y1, z1, zbufVal, zv;
    Vecf color = IrisCore::CreateVector(4);
    Vecf bcx = IrisCore::CreateVector(3);
    int hexColor;
    for (int i = (int)bboxMinx; i <= xm; i++)
    {
        for (int j = ys; j <= ym; j++)
        {
            //Pre Z Check
            zbufVal = zbuf.Get(i, j);
            if (zbufVal > maxZ)
            {
                continue;
            }

            //BaryCenter
            x0 = t1[0] - t0[0];
            y0 = t2[0] - t0[0];
            z0 = t0[0] - i;
            x1 = t1[1] - t0[1];
            y1 = t2[1] - t0[1];
            z1 = t0[1] - j;
            tw = 1 / (x0 * y1 - y0 * x1);
            tb = (y0 * z1 - z0 * y1) * tw;
            tc = (z0 * x1 - x0 * z1) * tw;
            if (tb < 0 || tc < 0)
            {
                continue;
            }
            ta = 1 - tb - tc;

            //Shading
            if (ta >= 0)
            {
                zv = ta * t0[2] + tb * t1[2] + tc * t2[2];
                if (zbufVal < zv)
                {
                    bcx[0] = ta;
                    bcx[1] = tb;
                    bcx[2] = tc;
                    shader.FragmentShaderCall(bcx);
                    for (int k = 0; k < 4; k++) {
                        color[k] = ((Vecf)(shader.GetInternalVariable(shader.arFragColor)))[k];
                    }
                    hexColor = IrisCore::RGBToHex((int)(color[0] * 255), (int)(color[1] * 255), (int)(color[2] * 255));
                    if(false){
                        IrisCore::DebugOutput("Set Color");
                        IrisCore::DebugVector(4,color);
                        IrisCore::GetDebugOutputStream()<<hexColor<<endl;
                    }
                    image.Set(i, j, hexColor);
                    zbuf.Set(i, j, zv);
                }
            }
        }
    }
    delete[] color;
    delete[] bcx;
}
