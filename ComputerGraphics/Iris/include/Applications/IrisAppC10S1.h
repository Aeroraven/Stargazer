#pragma once

#include "../Core/IrisCore.h"
#include "../Core/Application/IIrisApplication.h"
#include "../Display/IrisBitmap.h"
#include "../Utility/IrisWin32Helper.h"
#include "../Shaders/IrisShaderL10S1.h"
#include "../Core/Render/IrisRender.h"

//Application Ch:10 Section:1
//Establish Window & Draw Triangle

namespace Iris {
	namespace Applications {
		class IrisAppC10S1 : public Iris::Core::Application::IIrisApplication {
		private:
			Iris::Core::Drawing::IIrisImage* bitmap;
			Iris::Core::Render::IrisShader* shader;
			Iris::Core::Render::IrisRender* renderer;
			Iris::Core::Render::IrisZBuffer* zbuf;
			Matf transform;
			void** aVert;
			void** aColor;
			
		public:
			void Run() override;
			void DrawLoop() override;
			void Draw() override;
		};
	}
}