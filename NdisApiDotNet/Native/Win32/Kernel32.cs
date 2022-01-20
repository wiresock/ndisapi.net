// ----------------------------------------------
// <copyright file="Kernel32.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Security;

// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    [SuppressUnmanagedCodeSecurity]
    internal static class Kernel32
    {
        private const string DllName = "kernel32.dll";

        [DllImport(DllName, EntryPoint = "RtlZeroMemory")]
        public static extern void ZeroMemory(IntPtr destination, int length);

        [DllImport(DllName, EntryPoint = "RtlCopyMemory")]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
    }
}