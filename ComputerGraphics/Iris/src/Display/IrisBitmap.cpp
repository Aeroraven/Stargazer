#include "../../include/Display/IrisBitmap.h"

using namespace Iris::Display;
using namespace Iris::Core;

wchar_t* IrisBitmap::Char2wchar(const char* cchar)
{
    wchar_t* m_wchar;
    int len = MultiByteToWideChar(CP_ACP, 0, cchar, strlen(cchar), NULL, 0);
    m_wchar = new wchar_t[len + 1];
    MultiByteToWideChar(CP_ACP, 0, cchar, strlen(cchar), m_wchar, len);
    m_wchar[len] = '\0';
    return m_wchar;
}
bool IrisBitmap::SaveBitmapToFile(CBitmap* bitmap, LPCWSTR lpFileName)
{
    
    
    
    
    HBITMAP hBitmap;    
    HDC hDC; 
    int iBits; 
    WORD wBitCount; 
    DWORD dwPaletteSize = 0, 
        dwBmBitsSize,  
        dwDIBSize,   
        dwWritten;  
    BITMAP Bitmap; 
    BITMAPFILEHEADER bmfHdr; 
    BITMAPINFOHEADER bi; 
    LPBITMAPINFOHEADER lpbi; 
    HANDLE fh,   
        hDib,    
        hPal,   
        hOldPal = NULL;

    
    hBitmap = (HBITMAP)*bitmap;
    hDC = CreateDC(L"DISPLAY", NULL, NULL, NULL);
    iBits = GetDeviceCaps(hDC, BITSPIXEL) * GetDeviceCaps(hDC, PLANES);
    DeleteDC(hDC);

    if (iBits <= 1)
        wBitCount = 1;
    else if (iBits <= 4)
        wBitCount = 4;
    else if (iBits <= 8)
        wBitCount = 8;
    else if (iBits <= 24)
        wBitCount = 24;
    else if (iBits <= 32)
        wBitCount = 32;

    
    if (wBitCount <= 8)
        dwPaletteSize = (1 << wBitCount) * sizeof(RGBQUAD);

    
    GetObject(hBitmap, sizeof(BITMAP), (LPSTR)&Bitmap);
    bi.biSize = sizeof(BITMAPINFOHEADER);
    bi.biWidth = Bitmap.bmWidth;
    bi.biHeight = Bitmap.bmHeight;
    bi.biPlanes = 1;
    bi.biBitCount = wBitCount;
    bi.biCompression = BI_RGB;
    bi.biSizeImage = 0;
    bi.biXPelsPerMeter = 0;
    bi.biYPelsPerMeter = 0;
    bi.biClrUsed = 0;
    bi.biClrImportant = 0;

    dwBmBitsSize = ((Bitmap.bmWidth * wBitCount + 31) / 32) * 4 * Bitmap.bmHeight;

    
    hDib = GlobalAlloc(GHND, dwBmBitsSize + dwPaletteSize + sizeof(BITMAPINFOHEADER));
    lpbi = (LPBITMAPINFOHEADER)GlobalLock(hDib);
    *lpbi = bi;

    
    hPal = GetStockObject(DEFAULT_PALETTE);
    if (hPal)
    {
        hDC = ::GetDC(NULL);
        hOldPal = ::SelectPalette(hDC, (HPALETTE)hPal, FALSE);
        RealizePalette(hDC);
    }

    
    GetDIBits(hDC, hBitmap, 0, (UINT)Bitmap.bmHeight,
        (LPSTR)lpbi + sizeof(BITMAPINFOHEADER) + dwPaletteSize,
        (LPBITMAPINFO)lpbi, DIB_RGB_COLORS);

    
    if (hOldPal)
    {
        SelectPalette(hDC, (HPALETTE)hOldPal, TRUE);
        RealizePalette(hDC);
        ::ReleaseDC(NULL, hDC);
    }

    
    fh = CreateFile(lpFileName, GENERIC_WRITE,
        0, NULL, CREATE_ALWAYS,
        FILE_ATTRIBUTE_NORMAL | FILE_FLAG_SEQUENTIAL_SCAN, NULL);

    if (fh == INVALID_HANDLE_VALUE)
        return FALSE;

    
    bmfHdr.bfType = 0x4D42;     
    dwDIBSize = sizeof(BITMAPFILEHEADER)
        + sizeof(BITMAPINFOHEADER)
        + dwPaletteSize + dwBmBitsSize;
    bmfHdr.bfSize = dwDIBSize;
    bmfHdr.bfReserved1 = 0;
    bmfHdr.bfReserved2 = 0;
    bmfHdr.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER)
        + (DWORD)sizeof(BITMAPINFOHEADER)
        + dwPaletteSize;

    
    WriteFile(fh, (LPSTR)&bmfHdr, sizeof(BITMAPFILEHEADER), &dwWritten, NULL);

    
    WriteFile(fh, (LPSTR)lpbi, dwDIBSize,
        &dwWritten, NULL);

    
    GlobalUnlock(hDib);
    GlobalFree(hDib);
    CloseHandle(fh);

    return TRUE;
}
IrisBitmap::IrisBitmap(int w, int h) {
	cbmp = new CBitmap();
	cbuf = new char[w * h * 4];
	cbmp->CreateBitmap(w, h, 1, 32, nullptr);
	width = w;
	height = h;
}

IrisBitmap::~IrisBitmap() {
	delete cbmp;
	delete[] cbuf;
}

void IrisBitmap::Set(int x, int y, int hexColor) {
	int i = (x + (height - 1 - y) * width) * 4;
	cbuf[i + 3] = (char)(hexColor >> (8 * 3));
	cbuf[i + 2] = (char)((hexColor >> (8 * 2)) & 0xff);
	cbuf[i + 1] = (char)((hexColor >> (8 * 1)) & 0xff);
	cbuf[i] = (char)((hexColor >> (8 * 0)) & 0xff);
}
int IrisBitmap::Get(int x, int y) {
	int i = (x + (height - 1 - y) * width) * 4;
	int a = (unsigned char)cbuf[i + 3];
	int r = (unsigned char)cbuf[i + 2];
	int g = (unsigned char)cbuf[i + 1];
	int b = (unsigned char)cbuf[i + 0];
	return ((a << 24) | (r << 16) | (g << 8) | (b));
}
void IrisBitmap::Get(int x, int y, float& r, float& g, float& b) {
	int i = (x + (height - 1 - y) * width) * 4;
	int ir = (unsigned char)cbuf[i + 2];
	int ig = (unsigned char)cbuf[i + 1];
	int ib = (unsigned char)cbuf[i + 0];
	r = ir / 255.0f;
	g = ig / 255.0f;
	b = ib / 255.0f;
}
int IrisBitmap::GetInNormalized(float x, float y) {
	return Get((int)(x * (width - 1)), (int)(y * (height - 1)));
}
void IrisBitmap::GetInNormalized(float x, float y, float& r, float& g, float& b) {
	int hex = GetInNormalized(x, y);
	int ir, ig, ib;
	IrisCore::HexToRGB(hex, ir, ig, ib);
	r = ir / 255.0f;
	g = ig / 255.0f;
	b = ib / 255.0f;
}
void IrisBitmap::SetHeight(int x) {
	height = x;
}
int IrisBitmap::GetHeight() {
	return height;
}
void IrisBitmap::SetWidth(int x) {
	width = x;
}
int IrisBitmap::GetWidth() {
	return width;
}
void IrisBitmap::Save(string path) {
	SyncFromBuf();
    SaveBitmapToFile(cbmp, Char2wchar(path.c_str()));
}
void IrisBitmap::Load(string path) {
	IrisDebug("IrisBitmap:Load:not supported");
}
void* IrisBitmap::GetImage() {
	SyncFromBuf();
	return cbmp;
}
void IrisBitmap::SyncFromBuf() {
	cbmp->SetBitmapBits(width * height * 4, cbuf);
}
void IrisBitmap::Clear(int clearColor) {
	memset(cbuf, 0, width * height * 4);
}
void IrisBitmap::SyncFromImage() {
	IrisDebug("IrisBitmap:SyncFromImg:not supported");
}