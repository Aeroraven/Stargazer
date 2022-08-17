#pragma once

#include "../IrisCore.h"

namespace Iris {
	namespace Core {
		namespace Render {
			class IrisZBuffer {
			private:
				Matf buf = nullptr;
				int width = 0;
				int height = 0;
			
			private:
				IrisZBuffer();

			public:
				static IrisZBuffer* Create(int w, int h);
				void Set(int x, int y, float v);
				float Get(int x, int y);
				void Reset();
				~IrisZBuffer();
			};
		}
	}
}
