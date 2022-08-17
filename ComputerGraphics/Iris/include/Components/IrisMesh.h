#pragma once

#include "../Core/IrisCore.h"

namespace Iris {
	namespace Components {
		class IrisMesh {
		private:
			vector<float> vList;
			vector<float> vtList;
			vector<float> vnList;
			vector<int> fList;
			vector<int> fvtList;
			vector<int> fvnList;
		public:
			IrisObject* ExportVertex();
			IrisObject* ExportVertexNormal();
			IrisObject* ExportVertexTextureUV();
			int* ExportFaceIndex();
			void GetFace(int idx, int& ta, int& tb, int& tc);
			void GetFaceVertexIndex(int idx, int& ta, int& tb, int& tc);
			void GetVertex(int idx, float& ta, float& tb, float& tc);
			void GetVertexNormal(int idx, float& ta, float& tb, float& tc);
			void GetVertexTextureUV(int idx, float& ta, float& tb, float& tc);
			void ParseFromWavefront(string path);
			int GetNumVertices();
		};
	}
}