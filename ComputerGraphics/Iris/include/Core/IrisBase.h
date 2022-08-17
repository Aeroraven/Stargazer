#pragma once

//Standard & Libraries
#include <cstdio>
#include <cstring>
#include <initializer_list>
#include <cmath>
#include <chrono>
#include <string>
#include <map>
#include <vector>
#include <iostream>
#include <sstream>
#include <fstream>

//Macro
#define MatLoc(i,j,c) ((i)*(c)+(j))

//Types
typedef float* Matf;
typedef float* Vecf;
typedef float*& OutVecf;
typedef void* IrisObject;

//Std
using namespace std;
using namespace std::chrono;

//Debug
#define IRIS_DEBUG_OUT_PATH "C:\\TR\\iris.txt" 

#define IrisDebug(x) (std::cout<<(x)<<std::endl);
#define IrisDout (Iris::Core::IrisCore::GetDebugOutputStream())