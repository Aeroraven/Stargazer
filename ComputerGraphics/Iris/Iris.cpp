#include <iostream>
#include "include/Utility/IrisWin32Helper.h"
#include "include/Applications/IrisAppC10S1.h"
#include "include/Applications/IrisAppC10S2.h"
#include "include/Applications/IrisAppC11S1.h"
#include <gdiplus.h>

#pragma comment(linker, "/subsystem:windows")

void Ch10S1() {
	//Ch10 S1: Establish Window & Draw Triangle
	using namespace Iris::Utility;
	using namespace Iris::Applications;
	using namespace Iris::Core::Application;
	IIrisApplication* app = new IrisAppC10S1();
	IrisWin32Helper::SetApp(app);
}

void Ch10S2() {
	//Ch10 S2: Draw Mesh & Back Face Culling
	using namespace Iris::Utility;
	using namespace Iris::Applications;
	using namespace Iris::Core::Application;
	IIrisApplication* app = new IrisAppC10S2();
	IrisWin32Helper::SetApp(app);
}


void Ch11S1() {
	//Ch10 S2: Draw Mesh & Back Face Culling
	using namespace Iris::Utility;
	using namespace Iris::Applications;
	using namespace Iris::Core::Application;
	IIrisApplication* app = new IrisAppC11S1();
	IrisWin32Helper::SetApp(app);
}

int APIENTRY wWinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPWSTR lpCmdLine, _In_ int nCmdShow){
	using namespace Iris::Utility;
	Ch11S1();
	IrisWin32Helper::AppLaunch(hInstance, hPrevInstance, lpCmdLine, nCmdShow);
	IrisWin32Helper::CreateWindowAssist();
	
	return 1;
}
