#include "../../include/Core/IrisCore.h"

using namespace Iris::Core;


Matf IrisCore::CreateSquareMatrix(int d, bool clcr) {
	Matf ret = new float[(size_t)d * d];
	if (clcr) {
		for (int i = 0; i < d * d; i++) {
			ret[i] = 0;
		}
	}
	return ret;
}
Matf IrisCore::CreateMatrix(int r, int c, bool clcr) {
	Matf ret = new float[(size_t)r * c];
	if (clcr) {
		for (int i = 0; i < r * c; i++) {
			ret[i] = 0;
		}
	}
	return ret;
}
Vecf IrisCore::CreateVector(int d, bool clcr) {
	Vecf ret = new float[(size_t)d];
	if (clcr) {
		for (int i = 0; i < d; i++) {
			ret[i] = 0;
		}
	}
	return ret;
}

void IrisCore::SetVector(Vecf& r, initializer_list<float> x){
	int t = 0;
	for (auto i : x) {
		r[t++] = i;
	}
}


Vecf IrisCore::CreateVector(initializer_list<float> x) {
	Vecf ret = new float[(size_t)x.size()];
	int t = 0;
	for (auto i : x) {
		ret[t++] = i;
	}
	return ret;
}
void IrisCore::CrossProduct(Vecf x, Vecf y, Vecf& ret) {
	ret[0] = x[1] * y[2] - x[2] * y[1];
	ret[1] = x[2] * y[0] - x[0] * y[2];
	ret[2] = x[0] * y[1] - x[1] * y[0];
}
float IrisCore::DotProduct(int d, Vecf x, Vecf y) {
	float r = 0;
	for (int i = 0; i < d; i++) {
		r += x[i] * y[i];
	}
	return r;
}
void IrisCore::LinearTransform(int d, Matf t, Vecf x, Vecf& ret) {
	for (int i = 0; i < d; i++) {
		ret[i] = 0;
		for (int k = 0; k < d; k++) {
			ret[i] += t[MatLoc(i, k, d)] * x[k];
		}
	}
}

void IrisCore::HomoNormalize(int d, Vecf& ret) {
	for (int i = 0; i < d - 1; i++) {
		ret[i] /= ret[d - 1];
	}
}

void IrisCore::MatrixMultiplyI(int d, Matf a, Matf b, Matf& c) {
	for (int i = 0; i < d; i++) {
		for (int j = 0; j < d; j++) {
			float v = 0;
			for (int k = 0; k < d; k++) {
				v += a[MatLoc(i, k, d)] * b[MatLoc(k, j, d)];
			}
			c[MatLoc(i, j, d)] = v;
		}
	}
}

void IrisCore::MatrixMultiply(int d, initializer_list<Matf> t, Matf& ret) {
	IdentityMatrix(d, ret);
	Matf tp = CreateSquareMatrix(d, false);
	for (auto i : t) {
		MatrixMultiplyI(d, ret, i, tp);
		for (int i = 0; i < d * d; i++)ret[i] = tp[i];
	}
	delete[] tp;
}
void IrisCore::IdentityMatrix(int d, Matf& ret) {
	for (int i = 0; i < d; i++) {
		for (int k = 0; k < d; k++) {
			ret[MatLoc(i, k, d)] = ((i == k) ? 1 : 0);
		}
	}
}
void IrisCore::RectViewportMatrix3D(float w, float h, float vw, float vh, Matf& ret) {
	for (int i = 0; i < 16; i++) {
		ret[i] = 0;
	}
	float wscale = (w-1) / 2 / vw;
	float hscale = (h-1) / 2 / vh;
	ret[MatLoc(0, 0, 4)] = wscale;
	ret[MatLoc(1, 1, 4)] = hscale;
	ret[MatLoc(2, 2, 4)] = 1;
	ret[MatLoc(3, 3, 4)] = 1;
	ret[MatLoc(0, 3, 4)] = (w-1) / 2;
	ret[MatLoc(1, 3, 4)] = (h-1) / 2;
}
void IrisCore::NormalizeSelf(int d, Vecf& v) {
	float sq = 0;
	for (int i = 0; i < d; i++) {
		sq += v[i] * v[i];
	}
	float xhalf = 0.5f * sq;
	int q = *(int*)&sq;
	q = 0x5f3759df - (q >> 1);
	sq = *(float*)&q;
	sq = sq * (1.5f - xhalf * sq * sq);
	for (int i = 0; i < d; i++) {
		v[i] = v[i] * sq;
	}
}
void IrisCore::VectorMinus(int d, Vecf& a, Vecf& b, Vecf& r) {
	for (int i = 0; i < d; i++) {
		r[i] = a[i] - b[i];
	}
}
void IrisCore::LookAt(Vecf eye, Vecf center, Vecf up, Matf& ret) {
	Vecf z = CreateVector(3, false);
	VectorMinus(3, eye, center, z);
	NormalizeSelf(3, z);
	Vecf x = CreateVector(3, false);
	CrossProduct(up, z, x);
	Vecf y = CreateVector(3, false);
	CrossProduct(z, x, y);
	Matf m = CreateSquareMatrix(4, false);
	Matf t = CreateSquareMatrix(4, false);
	IdentityMatrix(4, m);
	IdentityMatrix(4, t);

	for (int i = 0; i < 3; i++) {
		m[MatLoc(0, i, 4)] = x[i];
		m[MatLoc(1, i, 4)] = y[i];
		m[MatLoc(2, i, 4)] = z[i];
		t[MatLoc(i, 3, 4)] = -eye[i];
	}
	MatrixMultiply(4, { m,t }, ret);
	delete[] t, m, x, y, z;
}
void IrisCore::PerspectiveMatrix(float fovy, float aspect, float zNear, float zFar, Matf& ret) {
	float half = fovy / 2;
	float tanfov = tan(half);
	float top = tanfov * zNear;
	float a = (zNear + zFar) / (zFar - zNear);
	float b = 2 * zNear * zFar / (zFar - zNear);
	float tx = 1 / (aspect * tanfov);
	float ty = 1 / tanfov;
	IdentityMatrix(4, ret);
	ret[MatLoc(0, 0,4)] = tx;
	ret[MatLoc(1, 1,4)] = ty;
	ret[MatLoc(2, 2,4)] = a;
	ret[MatLoc(2, 3,4)] = b;
	ret[MatLoc(3, 2,4)] = -1;
	ret[MatLoc(3, 3,4)] = 0;
}
void IrisCore::InverseMatrix(int d, Matf x, Matf& ret) {
	int rows = d;
	int cols = d * 2;
	Matf mat = CreateMatrix(rows, rows * 2);

	for (int i = 0; i < rows; i++) {
		for (int j = 0; j < rows * 2; j++) {
			if (j < d) {
				mat[MatLoc(i, j, cols)] = x[MatLoc(i, j, rows)];
			}
			else {
				mat[MatLoc(i, j, cols)] = (j == i + rows) ? 1 : 0;
			}
		}
	}

	for (int i = 0; i < rows; i++) {
		if (mat[MatLoc(i, i, cols)] == 0) {
			for (int j = i + 1; j < rows; j++) {
				if (mat[MatLoc(j, i, cols)] != 0) {
					for (int k = i; k < rows * 2; k++) {
						mat[MatLoc(i, k, cols)] += mat[MatLoc(j, k, cols)];
					}
					break;
				}
			}
		}
		float s = mat[MatLoc(i, i, cols)];
		for (int j = 0; j < rows * 2; j++) {
			mat[MatLoc(i, j, cols)] /= s;
		}
		for (int j = i + 1; j < rows; j++) {
			float coef = -mat[MatLoc(j, i, cols)];
			for (int k = i; k < rows * 2; k++) {
				mat[MatLoc(j, k, cols)] += coef * mat[MatLoc(i, k, cols)];
			}
		}
	}

	for (int i = rows - 1; i >= 0; i--) {
		for (int j = i - 1; j >= 0; j--) {
			float e = -1 * (mat[MatLoc(j, i, cols)] / mat[MatLoc(i, i, cols)]);
			for (int r = i; r < 2 * rows; r++) {
				mat[MatLoc(j, r, cols)] += e * mat[MatLoc(i, r, cols)];
			}
		}
	}

	for (int i = 0; i < rows; i++) {
		for (int r = rows; r < 2 * rows; r++) {
			ret[MatLoc(i, r-rows, rows)] = mat[MatLoc(i, r, cols)];
		}
	}
	delete[] mat;
}
void IrisCore::InverseTransposedMatrix(int d, Matf x, Matf& ret) {
	int rows = d;
	int cols = d * 2;
	Matf mat = CreateMatrix(rows, rows * 2);

	for (int i = 0; i < rows; i++) {
		for (int j = 0; j < rows * 2; j++) {
			if (j < d) {
				mat[MatLoc(i, j, cols)] = x[MatLoc(i, j, rows)];
			}
			else {
				mat[MatLoc(i, j, cols)] = (j == i + rows) ? 1 : 0;
			}
		}
	}

	for (int i = 0; i < rows; i++) {
		if (mat[MatLoc(i, i, cols)] == 0) {
			for (int j = i + 1; j < rows; j++) {
				if (mat[MatLoc(j, i, cols)] != 0) {
					for (int k = i; k < rows * 2; k++) {
						mat[MatLoc(i, k, cols)] += mat[MatLoc(j, k, cols)];
					}
					break;
				}
			}
		}
		float s = mat[MatLoc(i, i, cols)];
		for (int j = 0; j < rows * 2; j++) {
			mat[MatLoc(i, j, cols)] /= s;
		}
		for (int j = i + 1; j < rows; j++) {
			float coef = -mat[MatLoc(j, i, cols)];
			for (int k = i; k < rows * 2; k++) {
				mat[MatLoc(j, k, cols)] += coef * mat[MatLoc(i, k, cols)];
			}
		}
	}

	for (int i = rows - 1; i >= 0; i--) {
		for (int j = i - 1; j >= 0; j--) {
			float e = -1 * (mat[MatLoc(j, i, cols)] / mat[MatLoc(i, i, cols)]);
			for (int r = i; r < 2 * rows; r++) {
				mat[MatLoc(j, r, cols)] += e * mat[MatLoc(i, r, cols)];
			}
		}
	}

	for (int i = 0; i < rows; i++) {
		for (int r = rows; r < 2 * rows; r++) {
			ret[MatLoc( r - rows, i, rows)] = mat[MatLoc(i, r, cols)];
		}
	}
	delete[] mat;
}

void IrisCore::HexToRGB(int hex, int& r, int& g, int& b) {
	r = ((0xff << 16) & hex) >> 16;
	g = ((0xff << 8) & hex) >> 8;
	b = ((0xff << 0) & hex) >> 0;
}

int IrisCore::RGBToHex(int r, int g, int b) {
	return (0x00 << 24) | (r << 16) | (g << 8) | (b);
}

double IrisCore::GetMiliSecond() {
	auto now = system_clock::now();
	double ms = duration<double,milli>(now.time_since_epoch()).count();
	return ms;
}
ofstream& IrisCore::GetDebugOutputStream() {
	static ofstream ofs(IRIS_DEBUG_OUT_PATH);
	return ofs;
}