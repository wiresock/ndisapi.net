// ----------------------------------------------
// <copyright file="NdisApi.LIST_ENTRY_OR_ADAPTER_HANDLE.cs" company="NT Kernel">
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

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct LIST_ENTRY_OR_ADAPTER_HANDLE
        {
            /// <summary>
            /// The qlink entry.
            /// </summary>
            [FieldOffset(0)]
            public LIST_ENTRY m_qLink;

            /// <summary>
            /// The adapter handle.
            /// </summary>
            [FieldOffset(0)]
            public IntPtr m_hAdapter;
        }
    }
}