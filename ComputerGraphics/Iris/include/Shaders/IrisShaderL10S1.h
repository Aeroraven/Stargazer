#pragma once

#include "../Core/Render/IrisShader.h"

namespace Iris {
	namespace Shaders {
		class IrisShaderL10S1 : public Iris::Core::Render::IrisShader {
		public:
			IrisShaderL10S1();
			void VertexShader() override;
			void FragmentShader() override;
			void ComputeDerivedVariables() override;
		};
	}
}