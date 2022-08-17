#include "../../include/Utility/IrisWin32Helper.h"

using namespace Iris::Utility;
void IrisWin32Helper::DrawBitmap(HBITMAP bmp) {
	SelectObject(IrisWin32Global::mdc, bmp);
	BitBlt(IrisWin32Global::hdc, 0, 0, 800 , 600, IrisWin32Global::mdc, 0, 0, SRCCOPY);
}

void IrisWin32Helper::SetApp(Iris::Core::Application::IIrisApplication* app) {
	IrisWin32Global::app = app;
}

void IrisWin32Helper::CreateWindowAssist() {
	const WNDCLASSEXW wc = {
		sizeof(WNDCLASSEXW),
		CS_VREDRAW | CS_HREDRAW,
		IrisWin32Global::WndProc,
		0, 0,
		IrisWin32Global::hInstance,
		0, 0,
		(HBRUSH)GetStockObject(WHITE_BRUSH),
		0,  L"Iris",
		0
	};
	RegisterClassExW(&wc);
	IrisWin32Global::hwndMain = CreateWindowExW(0, wc.lpszClassName, L"Iris", WS_OVERLAPPEDWINDOW | SW_SHOWDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, 800, 600, (HWND)NULL, (HMENU)NULL, wc.hInstance, NULL);
	if (IrisWin32Global::hwndMain != NULL)
	{
		ShowWindow(IrisWin32Global::hwndMain, SW_SHOWDEFAULT);
		UpdateWindow(IrisWin32Global::hwndMain);

		IrisWin32Global::hdc = GetDC(IrisWin32Global::hwndMain);
		IrisWin32Global::mdc = CreateCompatibleDC(IrisWin32Global::hdc);
		MSG msg;

		bool done = true;
		while (done){
			if (::PeekMessageW(&msg, NULL, 0U, 0U, PM_REMOVE)){
				TranslateMessage(&msg);
				DispatchMessageW(&msg);
				if (msg.message == WM_QUIT) {
					done = false;
				}
			}
			else {
				IrisWin32Global::app->DrawLoop();
				IrisWin32Global::app->Draw();
			}
		}
	}
	UnregisterClassW(wc.lpszClassName, wc.hInstance);
}
void IrisWin32Helper::AppLaunch(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPWSTR lpCmdLine, int nCmdShow) {
    IrisWin32Global::hInstance = hInstance;
    IrisWin32Global::hPrevInstance = hPrevInstance;
    IrisWin32Global::lpCmdLine = lpCmdLine;
    IrisWin32Global::nCmdShow = nCmdShow;
	IrisWin32Global::app->Run();
}