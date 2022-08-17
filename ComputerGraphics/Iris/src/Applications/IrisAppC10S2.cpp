#include "../../include/Applications/IrisAppC10S2.h"

using namespace Iris::Applications;
using namespace Iris::Display;
using namespace Iris::Utility;
using namespace Iris::Core::Render;
using namespace Iris::Core;
using namespace Iris::Components;

void IrisAppC10S2::Run() {
	bitmap = new IrisBitmap(800, 600);
	shader = new Iris::Shaders::IrisShaderL10S2();
	mesh = new IrisMesh();
	mesh->ParseFromWavefront("D:\\WR\\Stargazer\\ComputerGraphics\\TinyRenderer\\src.obj");
	aVert = mesh->ExportVertex();
	aNorm = mesh->ExportVertexNormal();
	fidx = mesh -> ExportFaceIndex();
	nf = mesh->GetNumVertices();

	//Projection
	Matf modelview = IrisCore::CreateSquareMatrix(4);
	Matf projection = IrisCore::CreateSquareMatrix(4);
	Matf viewport = IrisCore::CreateSquareMatrix(4);
	transform = IrisCore::CreateSquareMatrix(4);
	IrisCore::RectViewportMatrix3D(800, 600, 1, 1, viewport);
	IrisCore::PerspectiveMatrix(3.14159f / 3, 800.0f/600, 0.01f, 100.0f, projection);
	IrisCore::LookAt(IrisCore::CreateVector({ 0,1,2 }), IrisCore::CreateVector({ 0,0,0 }), IrisCore::CreateVector({ 0,1,0 }), modelview);
	
	IrisCore::MatrixMultiply(4, { viewport,projection,modelview }, transform);
	IrisCore::DebugVector(16, transform);
	IrisCore::DebugVector(16, projection);
	IrisCore::DebugVector(16, modelview);
	IrisCore::DebugVector(16, viewport);

	//Normal Transform
	invModel = IrisCore::CreateSquareMatrix(4);
	IrisCore::InverseMatrix(4, modelview, invModel);

	//Light
	Vecf lightOrg = IrisCore::CreateVector({ 1,1,1,0 });
	light = IrisCore::CreateVector(4);
	IrisCore::LinearTransform(4, modelview, lightOrg, light);
	IrisCore::NormalizeSelf(3, light);

	shader->SetAttributeVariable("a_vert", aVert);
	shader->SetAttributeVariable("a_norm", aNorm);
	shader->SetVariable("a_trans", transform);
	shader->SetVariable("u_modelinv", invModel);
	shader->SetVariable("u_light", light);

	renderer = new IrisRender();
	renderer->SetBackFaceCulling(true);
	zbuf = IrisZBuffer::Create(800, 600);
}

void IrisAppC10S2::DrawLoop() {
	zbuf->Reset();
	bitmap->Clear(0xff000000);
	renderer->RasterizeTriangles3D(nf, fidx, *shader, *bitmap, *zbuf);
}

void IrisAppC10S2::Draw() {
	IrisWin32Helper::DrawBitmap((HBITMAP)(((CBitmap*)(bitmap->GetImage()))->GetSafeHandle()));
}