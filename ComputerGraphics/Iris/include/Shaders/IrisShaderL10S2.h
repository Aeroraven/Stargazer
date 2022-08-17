#pragma once

#include "../Core/Render/IrisShader.h"

namespace Iris {
	namespace Shaders {
		class IrisShaderL10S2 : public Iris::Core::Render::IrisShader {
		private:
			Vecf light;
			Vecf ac;
		public:
			IrisShaderL10S2();
			void VertexShader() override;
			void FragmentShader() override;
			void ComputeDerivedVariables() override;
		};
	}
}