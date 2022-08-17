#include "../../include/Applications/IrisAppC10S1.h"

using namespace Iris::Applications;
using namespace Iris::Display;
using namespace Iris::Utility;
using namespace Iris::Core::Render;
using namespace Iris::Core;

void IrisAppC10S1::Run() {
	bitmap = new IrisBitmap(800, 600);
	shader = new Iris::Shaders::IrisShaderL10S1();

	//Vertex
	float* fw1 = IrisCore::CreateVector({ 0,1.73 / 3,1 });
	float* fw2 = IrisCore::CreateVector({ -0.5,-1.73 / 6,1 });
	float* fw3 = IrisCore::CreateVector({ 0.5,-1.73 / 6,1 });
	aVert = new void* [3];
	aVert[0] = fw1;
	aVert[1] = fw2;
	aVert[2] = fw3;
	//Color
	float* cl1 = IrisCore::CreateVector({ 1,0,0,1 });
	float* cl2 = IrisCore::CreateVector({ 0,1,0,1 });
	float* cl3 = IrisCore::CreateVector({ 0,0,1,1 });
	aColor = new void* [3];
	aColor[0] = cl2;
	aColor[1] = cl1;
	aColor[2] = cl3;

	//Projection
	Matf modelview = IrisCore::CreateSquareMatrix(4);
	Matf projection = IrisCore::CreateSquareMatrix(4);
	Matf viewport = IrisCore::CreateSquareMatrix(4);
	transform = IrisCore::CreateSquareMatrix(4);
	IrisCore::RectViewportMatrix3D(800, 600, 800.0f / 600, 1.0f, viewport);
	IrisCore::IdentityMatrix(4, modelview);
	IrisCore::IdentityMatrix(4, projection);
	IrisCore::MatrixMultiply(4, { viewport,projection,modelview }, transform);
	IrisCore::DebugVector(16, transform);
	shader->SetAttributeVariable("a_vert", aVert);
	shader->SetAttributeVariable("a_color", aColor);
	shader->SetVariable("a_trans", transform);
	renderer = new IrisRender();
	zbuf = IrisZBuffer::Create(800, 600);
}

void IrisAppC10S1::DrawLoop() {
	int faceIdx[3] = { 0,1,2 };
	zbuf->Reset();
	bitmap->Clear(0xff000000);
	renderer->RasterizeTriangles3D(3, faceIdx, *shader, *bitmap, *zbuf);
}

void IrisAppC10S1::Draw() {
	IrisWin32Helper::DrawBitmap((HBITMAP)(((CBitmap*)(bitmap->GetImage()))->GetSafeHandle()));
}