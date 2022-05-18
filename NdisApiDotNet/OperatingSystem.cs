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
    /// Determines whether this OS is Windows 7.
    /// </summary>
    /// <returns><c>true</c> if this OS is Windows 7; otherwise, <c>false</c>.</returns>
    public static bool IsWindows7()
    {
        var osVersionInfoEx = GetVersion();
        return osVersionInfoEx.dwMajorVersion == 6 && osVersionInfoEx.dwMinorVersion == 1;
    }

    /// <summary>
    /// Determines whether this OS is Windows 8.
    /// </summary>
    /// <returns><c>true</c> if this OS is Windows 8; otherwise, <c>false</c>.</returns>
    public static bool IsWindows8()
    {
        // Windows 8 + 8.1.
        var osVersionInfoEx = GetVersion();
        return osVersionInfoEx.dwMajorVersion == 6 && (osVersionInfoEx.dwMinorVersion == 2 || osVersionInfoEx.dwMinorVersion == 3);
    }

    /// <summary>
    /// Determines whether this OS is Windows 10 or greater.
    /// </summary>
    /// <returns><c>true</c> if is Windows 10 or greater; otherwise, <c>false</c>.</returns>
    public static bool IsWindows10OrGreater()
    {
        var osVersionInfoEx = GetVersion();
        return osVersionInfoEx.dwMajorVersion >= 10;
    }

    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <returns><see cref="NtDll.OSVERSIONINFOEX" />.</returns>
    private static NtDll.OSVERSIONINFOEX GetVersion()
    {
        var osVersionInfoEx = new NtDll.OSVERSIONINFOEX();
        osVersionInfoEx.dwOSVersionInfoSize = (uint) Marshal.SizeOf(osVersionInfoEx);
        NtDll.RtlGetVersion(ref osVersionInfoEx);

        return osVersionInfoEx;
    }
}