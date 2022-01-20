// ----------------------------------------------
// <copyright file="NdisApi.ADAPTER_MODE.cs" company="NT Kernel">
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
        /// <summary>
        /// Used for setting adapter mode.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ADAPTER_MODE
        {
            /// <summary>
            /// The adapter handle.
            /// </summary>
            public IntPtr hAdapterHandle;

            /// <summary>
            /// The adapter flags.
            /// </summary>
            public MSTCP_FLAGS dwFlags;
        }
    }
}