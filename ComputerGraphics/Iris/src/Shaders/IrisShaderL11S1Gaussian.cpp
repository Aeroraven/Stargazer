#include "../../include/Shaders/IrisShaderL11S1Gaussian.h"

using namespace Iris::Shaders;
using namespace Iris::Core;
using namespace Iris::Core::Drawing;

IrisShaderL11S1Gaussian::IrisShaderL11S1Gaussian() {
    convKernel = nullptr;

    DefineVariable("u_sigma",tpFloat);
    DefineVariable("u_image",tpSampler2d);
    DefineVariable("u_viewport",tpMat4f);
    DefineVaryingVariable("v_uv",tpVec3f);
    DefineAttributeVariable("a_vert",tpVec3f);

    cl = IrisCore::CreateVector(4);
}

void IrisShaderL11S1Gaussian::VertexShader() {
	Vecf av = (Vecf)GetAttributeVariable("a_vert");
    

    Matf viewport = (Matf)GetVariable("u_viewport");
    Vecf av4 = IrisCore::CreateVector({av[0],av[1],av[2],1});
    Vecf av4t = IrisCore::CreateVector(4);
    
    IrisCore::LinearTransform(4,viewport,av4,av4t);
    SetInternalVariable(arPosition,av4t);
    Vecf av4t3 = IrisCore::CreateVector({av4t[0],av4t[1],av4t[2]});
    SetVaryingVariable("v_uv",av4t3);
    delete[] av4;
    delete[] av4t;
    delete[] av4t3;
}

void IrisShaderL11S1Gaussian::FragmentShader() {
    Vecf uv = (Vecf)GetVaryingVariable("v_uv");
    float tr=0,tg=0,tb=0;
    int s = 2*kernelSize+1;
    for(int i=-kernelSize;i<=kernelSize;i++){
        for(int j=-kernelSize;j<=kernelSize;j++){
            int ix = IrisCore::Min(IrisCore::Max(0.0f,uv[0]+i),799.0f);
            int iy = IrisCore::Min(IrisCore::Max(0.0f,uv[1]+j),599.0f);
            int dx = i+kernelSize;
            int dy = j+kernelSize;
            float r,g,b,a;
            int v;
            image->Get(ix,iy,r,g,b);
            tr+=r*convKernel[MatLoc(dx,dy,s)];
            tg+=g*convKernel[MatLoc(dx,dy,s)];
            tb+=b*convKernel[MatLoc(dx,dy,s)];
        }
    }
    tr/=coef;
    tg/=coef;
    tb/=coef;
    IrisCore::SetVector(cl,{tr,tg,tb,1});
    SetInternalVariable(arFragColor,cl);
}

void IrisShaderL11S1Gaussian::ComputeDerivedVariables() {
    image = (IIrisImage*) GetVariable("u_image");
    if(convKernel!=nullptr){
        delete[] convKernel;
    }
    coef = 0;
    sigma = *(float*)GetVariable("u_sigma");
    kernelSize = 2 * sigma;
    convKernel = IrisCore::CreateSquareMatrix(2*kernelSize+1);
    int s = 2*kernelSize+1;
    for(int i=-kernelSize;i<=kernelSize;i++){
        for(int j=-kernelSize;j<=kernelSize;j++){
            int dx = i+kernelSize;
            int dy = j+kernelSize;
            float sx = exp((i*i+j*j)/(2.0f*sigma));
            convKernel[MatLoc(dx,dy,s)] = sx;
            coef+=sx;
        }
    }
}