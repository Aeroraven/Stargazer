#include "../../../include/Core/Drawing/IrisDoubleBuffer.h"

using namespace Iris::Core::Drawing;
using namespace Iris::Core;

IIrisImage* IrisDoubleBuffer::GetDisplayBuffer() {
	return fore;
}
IIrisImage* IrisDoubleBuffer::GetDrawingBuffer() {
	return back;
}
void IrisDoubleBuffer::SetBuffer(IIrisImage* f, IIrisImage* g) {
	fore = f;
	back = g;
}
void IrisDoubleBuffer::SwapBuffer() {
	IrisCore::Swap(fore, back);
}