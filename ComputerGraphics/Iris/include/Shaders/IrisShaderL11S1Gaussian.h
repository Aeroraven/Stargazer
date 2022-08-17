#pragma once

#include "../Core/Render/IrisShader.h"
#include "../Core/Drawing/IIrisImage.h"

namespace Iris {
	namespace Shaders {
		class IrisShaderL11S1Gaussian : public Iris::Core::Render::IrisShader {
		private:
			Vecf light;
			Vecf ac;
			Vecf cl;
            int kernelSize;
            float sigma;
            float coef;
            Matf convKernel;
            Iris::Core::Drawing::IIrisImage* image;

		public:
			IrisShaderL11S1Gaussian();
			void VertexShader() override;
			void FragmentShader() override;
			void ComputeDerivedVariables() override;
		};
	}
}