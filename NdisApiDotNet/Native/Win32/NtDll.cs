// ----------------------------------------------
// <copyright file="NtDll.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System.Runtime.InteropServices;

// ReSharper disable CheckNamespace
namespace NdisApiDotNet.Native
{
    internal static partial class NtDll
    {
        [DllImport("ntdll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int RtlGetVersion(ref OSVERSIONINFOEX lpVersionInformation);
    }
}