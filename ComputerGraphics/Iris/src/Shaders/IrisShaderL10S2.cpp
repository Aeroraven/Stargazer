#include "../../include/Shaders/IrisShaderL10S2.h"

using namespace Iris::Shaders;
using namespace Iris::Core;

IrisShaderL10S2::IrisShaderL10S2() {
	DefineAttributeVariable("a_vert", tpVec3f);
	DefineAttributeVariable("a_norm", tpVec3f);
	DefineVaryingVariable("v_norm", tpVec3f);
	DefineVariable("a_trans", tpMat4f);
	DefineVariable("u_light", tpVec4f);
	DefineVariable("u_modelinv", tpVec3f);

	ac = IrisCore::CreateVector(4);
}

void IrisShaderL10S2::VertexShader() {
	Vecf av = (Vecf)GetAttributeVariable("a_vert");
	Vecf an = (Vecf)GetAttributeVariable("a_norm");
	Matf at = (Matf)GetVariable("a_trans");
	Matf uMinv = (Matf)GetVariable("u_modelinv");

	//Position
	Vecf ret4 = IrisCore::CreateVector({ av[0],av[1],av[2],1 });
	Vecf ret4t = IrisCore::CreateVector(4);
	IrisCore::LinearTransform(4, at, ret4, ret4t);
	IrisCore::HomoNormalize(4, ret4t);
	SetInternalVariable(arPosition, ret4t);
	delete[] ret4;
	delete[] ret4t;

	//Normal
	ret4 = IrisCore::CreateVector({ an[0],an[1],an[2],0 });
	ret4t = IrisCore::CreateVector(4);
	IrisCore::LinearTransform(4, uMinv, ret4, ret4t);
	IrisCore::NormalizeSelf(3, ret4t);
	Vecf ret4t3 = IrisCore::CreateVector({ ret4t[0],ret4t[1],ret4t[2]});
	SetVaryingVariable("v_norm", ret4t3);
	delete[] ret4;
	delete[] ret4t3;
	delete[] ret4t;
	
}

void IrisShaderL10S2::FragmentShader() {
	//Light
	Vecf vn = (Vecf)GetVaryingVariable("v_norm");
	IrisCore::NormalizeSelf(3, vn);
	float diff = IrisCore::DotProduct(3, vn, light);
	diff = IrisCore::Max(diff, 0.0f);

	//Coloring
	IrisCore::SetVector(ac, { diff,diff,diff,1 });
	SetInternalVariable(arFragColor, ac);
}

void IrisShaderL10S2::ComputeDerivedVariables() {
	light = (Vecf)GetVariable("u_light");
}