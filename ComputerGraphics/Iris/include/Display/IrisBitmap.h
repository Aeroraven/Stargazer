#pragma once

#include "../Core/Drawing/IIrisImage.h"
#include <afxwin.h>

namespace Iris {
	namespace Display {
		class IrisBitmap : public Iris::Core::Drawing::IIrisImage {
		private:
			CBitmap* cbmp;
			char* cbuf;
			int height;
			int width;
		public:
			wchar_t*  Char2wchar(const char* cchar);
			bool SaveBitmapToFile(CBitmap* bitmap, LPCWSTR lpFileName);
			IrisBitmap(int w, int h);
			~IrisBitmap();
			void Set(int x, int y, int hexColor) override;
			int Get(int x, int y) override;
			void Get(int x, int y, float& r, float& g, float& b) override;
			int GetInNormalized(float x, float y) override;
			void GetInNormalized(float x, float y, float& r, float& g, float& b) override;
			void SetHeight(int x) override;
			int GetHeight() override;
			void SetWidth(int x) override;
			int GetWidth() override;
			void Save(string path) override;
			void Load(string path) override;
			void* GetImage() override;
			void SyncFromBuf() override;
			void Clear(int clearColor) override;
			void SyncFromImage() override;
		};
	}
}