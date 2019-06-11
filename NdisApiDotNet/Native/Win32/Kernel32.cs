// ----------------------------------------------
// <copyright file="Kernel32.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Runtime.InteropServices;
using System.Security;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace NdisApiDotNet.Native
{
    internal static class Kernel32
    {
        [DllImport("kernel32.dll", EntryPoint = "RtlZeroMemory")]
        [SuppressUnmanagedCodeSecurity]
        internal static extern void ZeroMemory(IntPtr destination, int length);

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory")]
        [SuppressUnmanagedCodeSecurity]
        internal static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
    }
}