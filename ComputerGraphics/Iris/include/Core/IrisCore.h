#pragma once

#include "IrisBase.h"

namespace Iris {
    namespace Core {
        class IrisCore {
        private:

        public:
            //Ops
            template<class T> static void Swap(T& a, T& b) {
                T x = a;
                a = b;
                b = x;
            }
            template<class T> static T Max(T a, T b) {
                return (a > b) ? a : b;
            }
            template<class T> static T Min(T a, T b) {
                return (a < b) ? a : b;
            }
            //Create Template
            template<class T> static void* CreateArray(initializer_list<T> x) {
                T* ret = new T[(size_t)x.size()];
                int t = 0;
                for (auto i : x) {
                    ret[t++] = i;
                }
                return ret;
            }
            template<class T> static IrisObject* CreateObjectArray(initializer_list<T> x) {
                IrisObject* ret = new IrisObject[(size_t)x.size()];
                int t = 0;
                for (auto i : x) {
                    ret[t++] = i;
                }
                return ret;
            }

            //Create
            static Matf CreateSquareMatrix(int d, bool clcr = true);
            static Matf CreateMatrix(int r, int c, bool clcr = true);
            static Vecf CreateVector(int d, bool clcr = true);
            static Vecf CreateVector(initializer_list<float> x);
            static void SetVector(Vecf& r,initializer_list<float> x);
            //Math
            static void CrossProduct(Vecf x, Vecf y, Vecf& ret);
            static float DotProduct(int d, Vecf x, Vecf y);
            static void LinearTransform(int d, Matf t, Vecf x, Vecf& ret);
            static void MatrixMultiplyI(int d,Matf a,Matf b,Matf& c);
            static void MatrixMultiply(int d,initializer_list<Matf> t, Matf& ret);
            static void RectViewportMatrix3D(float w, float h, float vw, float vh, Matf& ret);
            static void IdentityMatrix(int d, Matf& ret);
            static void HomoNormalize(int d, Vecf& ret);
            static void NormalizeSelf(int d, Vecf& v);
            static void VectorMinus(int d, Vecf& a, Vecf& b, Vecf& r);
            static void LookAt(Vecf eye, Vecf center, Vecf up,Matf& ret);
            static void PerspectiveMatrix(float fovy, float aspect, float zNear, float zFar, Matf& ret);

            static void InverseMatrix(int d,Matf x, Matf& ret);
            static void InverseTransposedMatrix(int d,Matf x, Matf& ret);

            //Color
            static void HexToRGB(int hex, int& r, int& g, int& b);
            static int RGBToHex(int r, int g, int b);

            //Time
            static double GetMiliSecond();

            //Debug
            static ofstream& GetDebugOutputStream();

            //Template Debug
            template<class T> static void DebugVector(int d, T* x) {
                GetDebugOutputStream() << "[Iris-DebugVector]" << (void*)x <<": ";
                for (int i = 0; i < d; i++) {
                    GetDebugOutputStream() << x[i] << " ";
                }
                GetDebugOutputStream() << endl;
            }

            static void DebugOutput(string x) {
                GetDebugOutputStream() << "[Iris-DebugOutput]" << x <<endl;
            }
        };
    }
}
