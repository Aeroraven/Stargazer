#pragma once

#include "../IrisCore.h"
#include "./IrisShader.h"
#include "./IrisZBuffer.h"
#include "../Drawing/IIrisImage.h"

namespace Iris {
	namespace Core {
		namespace Render {
			class IrisRender {
			private:
				bool doBackFaceCulling = false;
				bool doDebug = false;
			public:
				void RasterizeTriangles3D(int len, int* faceIndex, IrisShader& shader, Iris::Core::Drawing::IIrisImage& image, IrisZBuffer& zbuf);
				void TriangleFragProcess3D(Vecf t0, Vecf t1, Vecf t2, IrisShader& shader, Iris::Core::Drawing::IIrisImage& image, IrisZBuffer& zbuf);
				void SetBackFaceCulling(bool enable);
				void SetDebugMode(bool enable);
			};
		}
	}
}