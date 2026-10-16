#include <windows.h>
#include <cstdlib>
#include <cstring>

namespace {
unsigned char expected_name[256];
int expected_length = 0;
int open_count = 0;
int close_count = 0;
HANDLE returned_handle = reinterpret_cast<HANDLE>(0x1234);
}

extern "C" __declspec(dllexport) void __stdcall TestReset()
{
    expected_length = open_count = close_count = 0;
    returned_handle = reinterpret_cast<HANDLE>(0x1234);
}

extern "C" __declspec(dllexport) void __stdcall TestExpectName(const unsigned char* name, int length)
{
    if (length < 0 || length > static_cast<int>(sizeof(expected_name)))
        std::abort();
    std::memcpy(expected_name, name, length);
    expected_length = length;
}

extern "C" __declspec(dllexport) void __stdcall TestReturnHandle(HANDLE handle)
{
    returned_handle = handle;
}

extern "C" __declspec(dllexport) int __stdcall TestOpenCount() { return open_count; }
extern "C" __declspec(dllexport) int __stdcall TestCloseCount() { return close_count; }

extern "C" __declspec(dllexport) HANDLE __stdcall OpenFilterDriver(const unsigned char* name)
{
    ++open_count;
    if (expected_length && std::memcmp(name, expected_name, expected_length))
    {
        SetLastError(ERROR_INVALID_NAME);
        return nullptr;
    }
    SetLastError(ERROR_OPEN_FAILED);
    return returned_handle;
}

extern "C" __declspec(dllexport) void __stdcall CloseFilterDriver(HANDLE) { ++close_count; }
