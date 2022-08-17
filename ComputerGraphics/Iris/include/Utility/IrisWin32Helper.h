#pragma once

#include "./IrisWin32Global.h"
#include "../Core/Application/IIrisApplication.h"

namespace Iris {
	namespace Utility {
		class IrisWin32Helper {
		public:
			static void DrawBitmap(HBITMAP bmp);
			static void SetApp(Iris::Core::Application::IIrisApplication* app);
			static void CreateWindowAssist();
			static void AppLaunch(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPWSTR lpCmdLine, int nCmdShow);
		};
	}
}