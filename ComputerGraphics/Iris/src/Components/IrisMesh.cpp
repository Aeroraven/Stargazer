#include "../../include/Components/IrisMesh.h"

using namespace Iris::Components;

IrisObject* IrisMesh::ExportVertex() {
	using namespace Iris::Core;
	IrisObject* ret = new IrisObject[fList.size()];
	for (int i = 0; i < fList.size() / 3; i++) {
		int a, b, c;
		GetFace(i, a, b, c);
		ret[i * 3] = IrisCore::CreateVector({ vList[3 * a],vList[3 * a + 1],vList[3 * a + 2] });
		ret[i * 3 + 1] = IrisCore::CreateVector({ vList[3 * b],vList[3 * b + 1],vList[3 * b + 2] });
		ret[i * 3 + 2] = IrisCore::CreateVector({ vList[3 * c],vList[3 * c + 1],vList[3 * c + 2] });
	}
	return ret;
}

IrisObject* IrisMesh::ExportVertexNormal() {
	using namespace Iris::Core;
	IrisObject* ret = new IrisObject[fList.size()];
	for (int i = 0; i < fList.size() / 3; i++) {
		int a, b, c;
		GetFace(i, a, b, c);
		ret[i * 3] = IrisCore::CreateVector({ vnList[3 * a],vnList[3 * a + 1],vnList[3 * a + 2] });
		ret[i * 3 + 1] = IrisCore::CreateVector({ vnList[3 * b],vnList[3 * b + 1],vnList[3 * b + 2] });
		ret[i * 3 + 2] = IrisCore::CreateVector({ vnList[3 * c],vnList[3 * c + 1],vnList[3 * c + 2] });
	}
	return ret;
}

IrisObject* IrisMesh::ExportVertexTextureUV() {
	using namespace Iris::Core;
	IrisObject* ret = new IrisObject[fList.size()];
	for (int i = 0; i < fList.size() / 3; i++) {
		int a, b, c;
		GetFace(i, a, b, c);
		ret[i * 3] = IrisCore::CreateVector({ vtList[3 * a],vtList[3 * a + 1],vtList[3 * a + 2] });
		ret[i * 3 + 1] = IrisCore::CreateVector({ vtList[3 * b],vtList[3 * b + 1],vtList[3 * b + 2] });
		ret[i * 3 + 2] = IrisCore::CreateVector({ vtList[3 * c],vtList[3 * c + 1],vtList[3 * c + 2] });
	}
	return ret;
}

int* IrisMesh::ExportFaceIndex() {
	using namespace Iris::Core;
	int* ret = new int[fList.size()];
	for (int i = 0; i < fList.size(); i++){
		ret[i] = i;
	}
	return ret;
}
void IrisMesh::GetFace(int idx, int& ta, int& tb, int& tc) {
	ta = fList[3 * idx];
	tb = fList[3 * idx + 1];
	tc = fList[3 * idx + 2];
}
void IrisMesh::GetFaceVertexIndex(int idx, int& ta, int& tb, int& tc) {
	ta = fvtList[3 * idx];
	tb = fvtList[3 * idx + 1];
	tc = fvtList[3 * idx + 2];
}
void IrisMesh::GetVertex(int idx, float& ta, float& tb, float& tc) {
	ta = vList[3 * idx];
	tb = vList[3 * idx + 1];
	tc = vList[3 * idx + 2];
}
void IrisMesh::GetVertexNormal(int idx, float& ta, float& tb, float& tc) {
	ta = vnList[3 * idx];
	tb = vnList[3 * idx + 1];
	tc = vnList[3 * idx + 2];
}
void IrisMesh::GetVertexTextureUV(int idx, float& ta, float& tb, float& tc) {
	ta = vtList[3 * idx];
	tb = vtList[3 * idx + 1];
	tc = vtList[3 * idx + 2];
}
void IrisMesh::ParseFromWavefront(string path) {
	ifstream fs(path);
	string st;
	while (fs >> st) {
		if (st == "v") {
			float a, b, c;
			fs >> a >> b >> c;
			vList.push_back(a);
			vList.push_back(b);
			vList.push_back(c);
		}
		else if (st == "vn") {
			float a, b, c;
			fs >> a >> b >> c;
			vnList.push_back(a);
			vnList.push_back(b);
			vnList.push_back(c);
		}
		else if (st == "vt") {
			float a, b, c;
			fs >> a >> b >> c;
			vtList.push_back(a);
			vtList.push_back(b);
			vtList.push_back(c);
		}
		else if (st == "f") {
			string a[3];
			for (int i = 0; i < 3; i++) {
				fs >> a[i];
				int d = 0;
				int s[3] = { 0,0,0 };
				for (int j = 0; j < a[i].size(); j++) {
					if (a[i][j] != '/') {
						s[d] = s[d] * 10 + (a[i][j] - '0');
					}
					else {
						d++;
					}
				}
				fList.push_back(s[0]-1);
				fvtList.push_back(s[1]-1);
				fvnList.push_back(s[2]-1);
			}
		}
	}
}
int IrisMesh::GetNumVertices() {
	return fList.size();
}