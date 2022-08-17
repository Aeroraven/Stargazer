#include "../../../include/Core/Render/IrisZBuffer.h"

using namespace Iris::Core::Render;
using namespace Iris::Core;

IrisZBuffer::IrisZBuffer() {
	buf = nullptr;
	width = 0;
	height = 0;
}

IrisZBuffer* IrisZBuffer::Create(int w, int h) {
	IrisZBuffer* t = new IrisZBuffer();
	t->buf = IrisCore::CreateMatrix(h, w);
	t->Reset();
	t->width = w;
	t->height = h;
	return t;
}

void IrisZBuffer::Reset() {
	for (int i = 0; i < width * height; i++) {
		buf[i] = -1e30f;
	}
}

void IrisZBuffer::Set(int x, int y, float v) {
	buf[y * width + x] = v;
}

float IrisZBuffer::Get(int x, int y) {
	return buf[y * width + x];
}

IrisZBuffer::~IrisZBuffer() {
	delete[] buf;
}