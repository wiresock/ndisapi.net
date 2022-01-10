// ----------------------------------------------
// <copyright file="OperatingSystem.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using NdisApiDotNet.Native;
using System.Runtime.InteropServices;

namespace NdisApiDotNet
{
    internal static class OperatingSystem
    {
        /// <summary>
        /// Determines whether this OS is Windows Vista.
        /// </summary>
        /// <returns><c>true</c> if this OS is Windows Vista; otherwise, <c>false</c>.</returns>
        internal static bool IsWinVista()
        {
            NtDll.OSVERSIONINFOEX osversioninfoex = GetVersion();

            return osversioninfoex.dwMajorVersion == 6 && osversioninfoex.dwMinorVersion == 0;
        }

        /// <summary>
        /// Determines whether this OS is Windows 7.
        /// </summary>
        /// <returns><c>true</c> if this OS is Windows 7; otherwise, <c>false</c>.</returns>
        internal static bool IsWin7()
        {
            NtDll.OSVERSIONINFOEX osversioninfoex = GetVersion();

            return osversioninfoex.dwMajorVersion == 6 && osversioninfoex.dwMinorVersion == 1;
        }

        /// <summary>
        /// Determines whether this OS is Windows 8.
        /// </summary>
        /// <returns><c>true</c> if this OS is Windows 8; otherwise, <c>false</c>.</returns>
        internal static bool IsWin8()
        {
            NtDll.OSVERSIONINFOEX osversioninfoex = GetVersion();

            return osversioninfoex.dwMajorVersion == 6 && osversioninfoex.dwMinorVersion == 2;
        }

        /// <summary>
        /// Determines whether this OS is Windows 8.1.
        /// </summary>
        /// <returns><c>true</c> if this OS is Windows 8.1; otherwise, <c>false</c>.</returns>
        internal static bool IsWin81()
        {
            NtDll.OSVERSIONINFOEX osversioninfoex = GetVersion();

            return osversioninfoex.dwMajorVersion == 6 && osversioninfoex.dwMinorVersion == 3;
        }

        /// <summary>
        /// Determines whether this OS is Windows 10.
        /// </summary>
        /// <returns><c>true</c> if this OS is Windows 10; otherwise, <c>false</c>.</returns>
        internal static bool IsWin10()
        {
            NtDll.OSVERSIONINFOEX osversioninfoex = GetVersion();

            return osversioninfoex.dwMajorVersion == 10 && osversioninfoex.dwMinorVersion == 0;
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <returns><see cref="NtDll.OSVERSIONINFOEX" />.</returns>
        private static NtDll.OSVERSIONINFOEX GetVersion()
        {
            NtDll.OSVERSIONINFOEX osversioninfoex = new NtDll.OSVERSIONINFOEX();
            osversioninfoex.dwOSVersionInfoSize = (uint)Marshal.SizeOf(osversioninfoex);
            NtDll.RtlGetVersion(ref osversioninfoex);

            return osversioninfoex;
        }
    }
}