#pragma once

#include "../IrisCore.h"
namespace Iris {
    namespace Core {
        namespace Drawing {
            class IIrisImage {
            public:
                virtual void Set(int x, int y, int hexColor) = 0;
                virtual int Get(int x, int y) = 0;
                virtual void Get(int x, int y, float& r, float& g, float& b) = 0;
                virtual int GetInNormalized(float x, float y) = 0;
                virtual void GetInNormalized(float x, float y, float& r, float& g, float& b) = 0;
                virtual void SetHeight(int x) = 0;
                virtual int GetHeight() = 0;
                virtual void SetWidth(int x) = 0;
                virtual int GetWidth() = 0;
                virtual void Save(string path) = 0;
                virtual void Load(string path) = 0;
                virtual void* GetImage() = 0;
                virtual void SyncFromBuf() = 0;
                virtual void Clear(int clearColor) = 0;
                virtual void SyncFromImage() = 0;
            };
        }
    }
}
