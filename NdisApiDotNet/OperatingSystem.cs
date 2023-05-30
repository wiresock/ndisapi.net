// ----------------------------------------------
// <copyright file="OperatingSystem.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System.Runtime.InteropServices;
using NdisApiDotNet.Native;

namespace NdisApiDotNet;

internal static class OperatingSystem
{
	/// <summary>
	/// Determines whether the OS is Windows 7.
	/// </summary>
	/// <returns><c>true</c> if the OS is Windows 7; otherwise, <c>false</c>.</returns>
	public static bool IsWindows7()
	{
		NtDll.OSVERSIONINFOEX osVersionInfoEx = GetVersion();
		return osVersionInfoEx is { dwMajorVersion: 6, dwMinorVersion: 1 };
	}

	/// <summary>
	/// Determines whether the OS is Windows 8.
	/// </summary>
	/// <returns><c>true</c> if the OS is Windows 8; otherwise, <c>false</c>.</returns>
	public static bool IsWindows8()
	{
		// Windows 8 + 8.1.
		NtDll.OSVERSIONINFOEX osVersionInfoEx = GetVersion();
		return osVersionInfoEx is { dwMajorVersion: 6, dwMinorVersion: 2 or 3 };
	}

	/// <summary>
	/// Determines whether the OS is Windows 10 or greater.
	/// </summary>
	/// <returns><c>true</c> if the OS is Windows 10 or greater; otherwise, <c>false</c>.</returns>
	public static bool IsWindows10OrGreater()
	{
		NtDll.OSVERSIONINFOEX osVersionInfoEx = GetVersion();
		return osVersionInfoEx.dwMajorVersion >= 10;
	}
	/// <summary>
	/// Determines whether the OS is Windows 10 RS3 or greater.
	/// </summary>
	/// <returns><c>true</c> if the OS is Windows 10 or greater; otherwise, <c>false</c>.</returns>
	public static bool IsWindows10RS3OrGreater()
	{
		NtDll.OSVERSIONINFOEX osVersionInfoEx = GetVersion();
		return osVersionInfoEx.dwMajorVersion > 10 || osVersionInfoEx is { dwMajorVersion: 10, dwBuildNumber: >= 16299 };
	}

	/// <summary>
	/// Gets the version.
	/// </summary>
	/// <returns><see cref="NtDll.OSVERSIONINFOEX" />.</returns>
	private static NtDll.OSVERSIONINFOEX GetVersion()
	{
		var osVersionInfoEx = new NtDll.OSVERSIONINFOEX();
		osVersionInfoEx.dwOSVersionInfoSize = (uint)Marshal.SizeOf(osVersionInfoEx);
		NtDll.RtlGetVersion(ref osVersionInfoEx);

		return osVersionInfoEx;
	}
}