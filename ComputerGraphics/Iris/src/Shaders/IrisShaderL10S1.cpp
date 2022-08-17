#include "../../include/Shaders/IrisShaderL10S1.h"

using namespace Iris::Shaders;
using namespace Iris::Core;

IrisShaderL10S1::IrisShaderL10S1() {
	DefineAttributeVariable("a_vert", tpVec3f);
	DefineAttributeVariable("a_color", tpVec4f);
	DefineVariable("a_trans", tpMat4f);
	DefineVaryingVariable("v_color", tpVec4f);
}

void IrisShaderL10S1::VertexShader() {
	Vecf av = (Vecf)GetAttributeVariable("a_vert");
	Vecf ac = (Vecf)GetAttributeVariable("a_color");
	Matf at = (Matf)GetVariable("a_trans");
	Vecf ret4 = IrisCore::CreateVector({ av[0],av[1],av[2],1 });
	Vecf ret4t = IrisCore::CreateVector(4);
	IrisCore::LinearTransform(4, at, ret4, ret4t);
	SetInternalVariable(arPosition, ret4t);
	SetVaryingVariable("v_color", ac);
	delete[] ret4;
	delete[] ret4t;
}

void IrisShaderL10S1::FragmentShader() {
	Vecf ac = (Vecf)GetVaryingVariable("v_color");
	SetInternalVariable(arFragColor, ac);
}

void IrisShaderL10S1::ComputeDerivedVariables() {
	
}