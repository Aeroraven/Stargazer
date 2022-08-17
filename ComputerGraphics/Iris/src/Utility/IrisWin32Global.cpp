#include "../../include/Utility/IrisWin32Global.h"

using namespace Iris::Utility;

HINSTANCE IrisWin32Global::hInstance = 0;
HINSTANCE IrisWin32Global::hPrevInstance = 0;
LPWSTR IrisWin32Global::lpCmdLine = 0;
int IrisWin32Global::nCmdShow = 0;
HWND IrisWin32Global::hwndMain = 0;
HDC IrisWin32Global::mdc = 0;
HDC IrisWin32Global::hdc = 0;
double IrisWin32Global::lastTimestamp = 0;
Iris::Core::Application::IIrisApplication* IrisWin32Global::app = nullptr;

LRESULT WINAPI IrisWin32Global::WndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam) {
	switch (msg)
	{
	case WM_PAINT:
	{
		PAINTSTRUCT ps;
		HDC hdc = BeginPaint(hWnd, &ps);
		if (app != nullptr) {
			wstringstream wss;
			wss << "Iris App (FPS: " << int(1000 / (Iris::Core::IrisCore::GetMiliSecond() - IrisWin32Global::lastTimestamp))<<")";
			IrisWin32Global::lastTimestamp = Iris::Core::IrisCore::GetMiliSecond();
			app->DrawLoop();
			SetWindowTextW(hwndMain, wss.str().c_str());
			app->Draw();
		}
		else {
			TextOutW(hdc, 0, 0, L"IrisWin32: Application is not defined", 37);
		}
		
		EndPaint(hWnd, &ps);
		RedrawWindow(hWnd, NULL, NULL, RDW_INVALIDATE | RDW_UPDATENOW);
		return 0L;
	}

	case WM_DESTROY:
		::PostQuitMessage(0);
		return 0;
	}
	return ::DefWindowProcW(hWnd, msg, wParam, lParam);
}