#pragma once

#include "../IrisCore.h"
namespace Iris{
	namespace Core {
		namespace Application {
			class IIrisApplication {

			public:
				virtual void Run() = 0;
				virtual void DrawLoop() = 0;
				virtual void Draw() = 0;
			};
		}
	}
}
