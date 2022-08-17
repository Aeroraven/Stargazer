#pragma once

#include "../IrisCore.h"
#include "../Structure/IrisTrie.h"

namespace Iris{
	namespace Core {
		namespace Render {
			class IrisShader {
			public:
				enum IrisShaderInternalVar {
					arFragColor = 0,
					arPosition = 1
				};
				enum IrisShaderType {
					tpInt = 0,
					tpFloat = 1,
					tpVec2f = 2,
					tpVec3f = 3,
					tpVec4f = 4,
					tpMat3f = 5,
					tpMat4f = 6,
					tpMat2f = 7,
					tpSampler2d = 8
				};
			protected:
				int activeIndex = 0;
				int activeVIndex = 0;
				bool compiled = false;
				int nVaryings = 0;
				Iris::Core::Structure::IrisTrie* varyingKeyMap;
				
				//Compiled Varyings
				IrisObject** varyingList;
				int* varyingTypeList;
				IrisObject* interpolatedVaryingList;

				//Varyings
				map<int, IrisObject*> varyingListPre;
				map<int, IrisObject> interpolatedVaryingListPre;
				map<int, int> varyingTypeListPre;

				//Uniforms
				map<string, IrisObject> varList;
				map<string, int> typeList;
				bool uniformChanged = false;

				//Attributes
				map<string, IrisObject*> attributeList;
				map<string, int>attributeTypeList;

				//Internal
				IrisObject* internalList;

				//Reserve Addrs
				vector<IrisObject> constantRefList;

				//Placeholder
				int* placeholder;

			protected:
				IrisShader();

			public:
				//Dtor
				~IrisShader();

				//Functional
				IrisObject AllocateForInt(void* x);
				IrisObject AllocateForFloat(void* x);

				IrisObject GetInternalVariable(int varName);
				void SetInternalVariable(int varName, IrisObject value);
				void BeforeRunning();
				void CacheShaderVariables();
				bool FindIsUniformChanged();
				void SetUniformChangedState();

				void SetAttributeVariable(string varName, IrisObject* value);
				IrisObject GetAttributeVariable(string varName);
				void DefineAttributeVariable(string varName, int typeName);
			
				void DefineVaryingVariable(string varName, int typeName);
				IrisObject GetVaryingVariable(string varName);
				void SetVaryingVariable(string varName, IrisObject value);

				void DefineVariable(string varName, int typeName);
				void SetVariable(string varName, IrisObject value);
				IrisObject GetVariable(string varName);

				void InterpolateVaryings(int id, Vecf bc);

				void FragmentShaderCall(Vecf bc);
				void VertexShaderCall(int index, int vindex);
				
				virtual void VertexShader() = 0;
				virtual void FragmentShader() = 0;
				virtual void ComputeDerivedVariables() = 0;
			};
		}
	}
}
