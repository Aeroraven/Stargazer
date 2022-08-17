#include "../../../include/Core/Render/IrisShader.h"

using namespace Iris::Core;
using namespace Iris::Core::Render;
using namespace Iris::Core::Structure;

IrisShader::IrisShader() {
	varyingList = nullptr;
	varyingTypeList = nullptr;
	interpolatedVaryingList = nullptr;
	placeholder = new int(5);
	varyingKeyMap = new IrisTrie();
	internalList = new IrisObject [2];
	internalList[0] = IrisCore::CreateVector(4);
	internalList[1] = IrisCore::CreateVector(4);
}

IrisShader::~IrisShader() {
	delete varyingKeyMap;
	delete placeholder;
	delete[] internalList[0];
	delete[] internalList[1];
	delete[] internalList;
	for (int i = 0; i < constantRefList.size(); i++) {
		delete constantRefList[i];
	}
	if (interpolatedVaryingList != nullptr) {
		//Memory Leaks
		delete[] interpolatedVaryingList;
		delete[] varyingTypeList;
		delete[] varyingList;
	}
}
IrisObject IrisShader::AllocateForInt(void* x) {
	constantRefList.push_back(new int(*(int*)x));
	return constantRefList[constantRefList.size() - 1];
}
IrisObject IrisShader::AllocateForFloat(void* x) {
	constantRefList.push_back(new float(*(float*)x));
	return constantRefList[constantRefList.size() - 1];
}
IrisObject IrisShader::GetInternalVariable(int varName) {
	return internalList[varName];
}
void IrisShader::SetInternalVariable(int varName, IrisObject value) {
	if (varName == arPosition || varName == arFragColor) {
		float* fv = (float*)value;
		Vecf dest = (float*)(internalList[varName]);
		for (int i = 0; i < 4; i++) {
			dest[i] = fv[i];
		}
	}
}
void IrisShader::BeforeRunning() {
	if (!compiled)CacheShaderVariables();
	compiled = true;
}

void IrisShader::CacheShaderVariables() {
	varyingList = new IrisObject*[nVaryings];
	varyingTypeList = new int[nVaryings];
	interpolatedVaryingList = new IrisObject[nVaryings];
	for (int i = 0; i < nVaryings; i++) {
		varyingList[i] = varyingListPre[i];
		varyingTypeList[i] = varyingTypeListPre[i];
		int x = varyingTypeList[i];
		if (varyingTypeList[i] == tpVec3f)
		{
			interpolatedVaryingList[i] = IrisCore::CreateVector(3);
		}else if(varyingTypeList[i] == tpVec2f)
		{
			interpolatedVaryingList[i] = IrisCore::CreateVector(2);
		}else if (varyingTypeList[i] == tpVec4f)
		{
			interpolatedVaryingList[i] = IrisCore::CreateVector(4);
		}
	}
}
bool IrisShader::FindIsUniformChanged() {
	return uniformChanged;
}
void IrisShader::SetUniformChangedState() {
	uniformChanged = false;
}
void IrisShader::SetAttributeVariable(string varName, IrisObject* value) {
	attributeList[varName] = value;
}
IrisObject IrisShader::GetAttributeVariable(string varName) {
	using namespace  Iris::Core;
	//IrisCore::DebugVector(3, (float*) attributeList[varName][activeIndex]);
	return attributeList[varName][activeIndex];
}
void  IrisShader::DefineAttributeVariable(string varName, int typeName) {
	attributeList[varName] = nullptr;
	attributeTypeList[varName] = typeName;
}

void IrisShader::DefineVaryingVariable(string varName, int typeName) {
	varyingKeyMap->Insert(varName, nVaryings);

	varyingTypeListPre[nVaryings] = typeName;
	varyingListPre[nVaryings] = new IrisObject[3];
	if (typeName == tpVec3f) {
		varyingListPre[nVaryings][0] = IrisCore::CreateVector(3);
		varyingListPre[nVaryings][1] = IrisCore::CreateVector(3);
		varyingListPre[nVaryings][2] = IrisCore::CreateVector(3);
	}
	else if (typeName == tpVec4f) {
		varyingListPre[nVaryings][0] = IrisCore::CreateVector(4);
		varyingListPre[nVaryings][1] = IrisCore::CreateVector(4);
		varyingListPre[nVaryings][2] = IrisCore::CreateVector(4);
	}
	else {
		IrisCore::DebugOutput("Invalid Varying Type");
	}
	nVaryings++;
}

IrisObject IrisShader::GetVaryingVariable(string varName) {
	int idx = varyingKeyMap->Find(varName);
	return interpolatedVaryingList[idx];
}

void IrisShader::SetVaryingVariable(string varName, IrisObject value) {
	int idx = activeVIndex;
	int hash = varyingKeyMap->Find(varName);
	int vType = varyingTypeList[hash];
	if (vType == tpVec3f) {
		float* dest = (float*)varyingList[hash][idx];
		float* src = (float*)value;
		dest[0] = src[0];
		dest[1] = src[1];
		dest[2] = src[2];
	}else if(vType == tpVec4f) {
		float* dest = (float*)varyingList[hash][idx];
		float* src = (float*)value;
		dest[0] = src[0];
		dest[1] = src[1];
		dest[2] = src[2];
		dest[3] = src[3];
	}
	else {
		IrisCore::DebugOutput("Invalid Varying Type");
	}
	 
}

void IrisShader::DefineVariable(string varName, int typeName) {
	varList[varName] = nullptr;
	typeList[varName] = typeName;
	if (typeName == tpInt) {
		int t = 0;
		varList[varName] = AllocateForInt(&t);
	}
	else if (typeName == tpFloat) {
		float t = 0;
		varList[varName] = AllocateForFloat(&t);
	}
}

void IrisShader::SetVariable(string varName, IrisObject value) {
	uniformChanged = true;
	if (typeList[varName] == tpInt) {
		*(int*)varList[varName] = *(int*)value;
	}
	else if (typeList[varName] == tpFloat) {
		*(float*)varList[varName] = *(float*)value;
	}
	else {
		varList[varName] = value;
	}
}

IrisObject IrisShader::GetVariable(string varName) {
	return varList[varName];
}

void IrisShader::InterpolateVaryings(int id, Vecf bc) {
	int tp = varyingTypeList[id];
	auto tx = varyingList[id];
	auto a = tx[0];
	auto b = tx[1];
	auto c = tx[2];
	float ta = bc[0];
	float tb = bc[1];
	float tc = bc[2];
	float dx, dy, dz, dw;

	if (tp == tpVec3f) {

		Vecf dap = (Vecf)a;
		Vecf dbp = (Vecf)b;
		Vecf dcp = (Vecf)c;
		Vecf dst = (Vecf)interpolatedVaryingList[id];
		dst[0] = ta * dap[0] + tb * dbp[0] + tc * dcp[0];
		dst[1] = ta * dap[1] + tb * dbp[1] + tc * dcp[1];
		dst[2] = ta * dap[2] + tb * dbp[2] + tc * dcp[2];

	}else if (tp == tpVec4f) {
		Vecf dap = (Vecf)a;
		Vecf dbp = (Vecf)b;
		Vecf dcp = (Vecf)c;
		Vecf dst = (Vecf)interpolatedVaryingList[id];
		dst[0] = ta * dap[0] + tb * dbp[0] + tc * dcp[0];
		dst[1] = ta * dap[1] + tb * dbp[1] + tc * dcp[1];
		dst[2] = ta * dap[2] + tb * dbp[2] + tc * dcp[2];
		dst[3] = ta * dap[3] + tb * dbp[3] + tc * dcp[3];
	}
	else{
		IrisDebug("IrisShader: Error Unsupported Type");
	}
}

void IrisShader::FragmentShaderCall(Vecf bc){
	for (int i = 0; i < nVaryings; i++){
		InterpolateVaryings(i, bc);
	}
	FragmentShader();
}

void IrisShader::VertexShaderCall(int index, int vindex)
{
	activeIndex = index;
	activeVIndex = vindex;
	if (FindIsUniformChanged()) {
		ComputeDerivedVariables();
		SetUniformChangedState();
	}
	VertexShader();
}