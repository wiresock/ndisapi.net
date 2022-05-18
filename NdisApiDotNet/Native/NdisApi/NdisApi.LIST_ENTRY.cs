// ----------------------------------------------
// <copyright file="NdisApi.LIST_ENTRY.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// The <see cref="LIST_ENTRY"/> as defined in ntdef.h.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LIST_ENTRY
    {
        public IntPtr Flink;
        public IntPtr Blink;
    }
}