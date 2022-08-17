#pragma once
#pragma once

#include "../Core/IrisCore.h"
#include "../Core/Application/IIrisApplication.h"
#include "../Display/IrisBitmap.h"
#include "../Utility/IrisWin32Helper.h"
#include "../Shaders/IrisShaderL10S2.h"
#include "../Core/Render/IrisRender.h"
#include "../Components/IrisMesh.h"

//Application Ch:10 Section:2
//Drawing Mesh & Back Face Culling

namespace Iris {
	namespace Applications {
		class IrisAppC10S2 : public Iris::Core::Application::IIrisApplication {
		private:
			Iris::Core::Drawing::IIrisImage* bitmap;
			Iris::Core::Render::IrisShader* shader;
			Iris::Core::Render::IrisRender* renderer;
			Iris::Core::Render::IrisZBuffer* zbuf;
			Iris::Components::IrisMesh* mesh;
			Matf transform;
			Matf invModel;
			Vecf light;
			void** aVert;
			void** aNorm;
			int* fidx;
			int nf;

		public:
			void Run() override;
			void DrawLoop() override;
			void Draw() override;
		};
	}
}