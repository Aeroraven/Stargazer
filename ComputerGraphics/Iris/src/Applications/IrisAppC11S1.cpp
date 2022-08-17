#include "../../include/Applications/IrisAppC11S1.h"

using namespace Iris::Applications;
using namespace Iris::Display;
using namespace Iris::Utility;
using namespace Iris::Core::Render;
using namespace Iris::Core;
using namespace Iris::Components;

void IrisAppC11S1::Run() {
	bitmap = new IrisBitmap(800, 600);
	rawbmp = new IrisBitmap(800, 600);
	shader = new Iris::Shaders::IrisShaderL10S2();
	postShader = new Iris::Shaders::IrisShaderL11S1Gaussian();

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

	//Normal Transform
	invModel = IrisCore::CreateSquareMatrix(4);
	IrisCore::InverseMatrix(4, modelview, invModel);

	//Light
	Vecf lightOrg = IrisCore::CreateVector({ 1,1,1,0 });
	light = IrisCore::CreateVector(4);
	IrisCore::LinearTransform(4, modelview, lightOrg, light);
	IrisCore::NormalizeSelf(3, light);

	//Set Vars
	shader->SetAttributeVariable("a_vert", aVert);
	shader->SetAttributeVariable("a_norm", aNorm);
	shader->SetVariable("a_trans", transform);
	shader->SetVariable("u_modelinv", invModel);
	shader->SetVariable("u_light", light);

	//Set Post Vars
	float temp = 1.0;
	viewpost = IrisCore::CreateSquareMatrix(4);
	IrisCore::IdentityMatrix(4,viewpost);
	Vecf ab1 = IrisCore::CreateVector({0,0,1});
	Vecf ab2 = IrisCore::CreateVector({800,0,1});
	Vecf ab3 = IrisCore::CreateVector({800,600,1});
	Vecf ab4 = IrisCore::CreateVector({0,600,1});
	aVertb = IrisCore::CreateObjectArray({ab1,ab2,ab3,ab4});
	fidxp = new int[6]{0,1,2,2,3,0};

	postShader->SetVariable("u_sigma",&temp);
	postShader->SetVariable("u_image",rawbmp);
	postShader->SetVariable("u_viewport",viewpost);
	postShader->SetAttributeVariable("a_vert",aVertb);
	//Ready?
	renderer = new IrisRender();
	renderer->SetBackFaceCulling(true);
	zbuf = IrisZBuffer::Create(800, 600);
	zbufpost = IrisZBuffer::Create(800, 600);
}

void IrisAppC11S1::DrawLoop() {
	zbuf->Reset();
	zbufpost->Reset();

	//First Pass
	renderer->SetDebugMode(false);
	renderer->SetBackFaceCulling(true);
	rawbmp->Clear(0xff000000);
	renderer->RasterizeTriangles3D(nf, fidx, *shader, *rawbmp, *zbuf);

	//Second Pass
	renderer->SetDebugMode(true);
	renderer->SetBackFaceCulling(false);
	bitmap->Clear(0xff000000);
	renderer->RasterizeTriangles3D(6,fidxp,*postShader,*bitmap,*zbufpost);

}

void IrisAppC11S1::Draw() {
	IrisWin32Helper::DrawBitmap((HBITMAP)(((CBitmap*)(bitmap->GetImage()))->GetSafeHandle()));
}