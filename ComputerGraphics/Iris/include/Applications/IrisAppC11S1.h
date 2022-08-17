#pragma once
#pragma once

#include "../Core/IrisCore.h"
#include "../Core/Application/IIrisApplication.h"
#include "../Display/IrisBitmap.h"
#include "../Utility/IrisWin32Helper.h"
#include "../Shaders/IrisShaderL10S2.h"
#include "../Shaders/IrisShaderL11S1Gaussian.h"
#include "../Core/Render/IrisRender.h"
#include "../Components/IrisMesh.h"

//Application Ch:11 Section:1
//Gaussian Blur

namespace Iris {
	namespace Applications {
		class IrisAppC11S1 : public Iris::Core::Application::IIrisApplication {
		private:
			Iris::Core::Drawing::IIrisImage* bitmap;
			Iris::Core::Drawing::IIrisImage* rawbmp;
			Iris::Core::Render::IrisShader* shader;
			Iris::Core::Render::IrisShader* postShader;
			Iris::Core::Render::IrisRender* renderer;
			Iris::Core::Render::IrisZBuffer* zbuf;
			Iris::Core::Render::IrisZBuffer* zbufpost;
			Iris::Components::IrisMesh* mesh;
			Matf transform;
			Matf invModel;
			Matf viewpost;
			Vecf light;
			void** aVert;
			void** aNorm;
			void** aVertb;
			int* fidx;
			int* fidxp;
			int nf;

		public:
			void Run() override;
			void DrawLoop() override;
			void Draw() override;
		};
	}
}