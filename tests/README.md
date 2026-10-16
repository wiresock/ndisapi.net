# Driver name regression tests

On Windows x64 with the .NET 8 SDK (or a newer SDK with the .NET 8 runtime
and targeting pack) and Visual Studio C++ build tools, run from the repository:

```powershell
powershell -NoProfile -ExecutionPolicy RemoteSigned -File tests/Run-DriverNameTests.ps1
```

This dependency-free console suite calls the public `NdisApi.Open`, `Reopen`
and `Close` methods through their real P/Invoke declarations. It builds an
isolated test `ndisapi.dll` next to the test executable, so no installed packet
filter driver, administrator rights, or network traffic is needed. Do not copy
this test DLL into an application or driver installation.

The tests use literal expected bytes for default UTF-16LE, legacy Latin-1,
explicit encodings, empty strings and names already ending in NUL. The native
API accepts only a pointer and gives the test DLL no allocation length. To
avoid reading outside an unterminated allocation, the tests first check the
managed buffer retained for `Reopen` using reflection, then have the native
stub compare those bytes during `Reopen`. Removing name termination makes
these checks fail deterministically, even if adjacent heap memory is zero.
The suite also checks null arguments and invalid native handles.
