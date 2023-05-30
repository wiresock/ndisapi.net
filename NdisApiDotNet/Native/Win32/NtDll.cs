// ----------------------------------------------
// <copyright file="NtDll.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System.Runtime.InteropServices;
using System.Security;

// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

[SuppressUnmanagedCodeSecurity]
internal static partial class NtDll
{
	private const string DllName = "ntdll.dll";

	[DllImport(DllName)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
	public static extern int RtlGetVersion(ref OSVERSIONINFOEX lpVersionInformation);
}