#pragma once

#include "../IrisCore.h"
#include "./IIrisImage.h"

namespace Iris {
	namespace Core {
		namespace Drawing {
			class IrisDoubleBuffer {
			private:
				IIrisImage* fore;
				IIrisImage* back;
			public:
				IIrisImage* GetDisplayBuffer();
				IIrisImage* GetDrawingBuffer();
				void SetBuffer(IIrisImage* f, IIrisImage* g);
				void SwapBuffer();
			};
		}
	}
}