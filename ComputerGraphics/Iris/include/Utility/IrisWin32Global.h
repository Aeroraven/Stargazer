#pragma once

#include "../Core/Application/IIrisApplication.h"
#include <afxwin.h>
#include <Windows.h>

namespace Iris {
	namespace Utility {
		class IrisWin32Global {
		public:
			static HINSTANCE hInstance;
			static HINSTANCE hPrevInstance;
			static LPWSTR lpCmdLine;
			static int nCmdShow;
			static HWND hwndMain;
			static HDC mdc;
			static HDC hdc;
			static Iris::Core::Application::IIrisApplication* app;
			static double lastTimestamp;

			static LRESULT WINAPI WndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);
		};
	}
}
