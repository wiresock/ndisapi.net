// ----------------------------------------------
// <copyright file="NdisApi.ADAPTER_MODE.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
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
            internal IntPtr hAdapterHandle;
            internal MSTCP_FLAGS dwFlags;

            /// <summary>
            /// Gets or sets the adapter handle.
            /// </summary>
            public IntPtr AdapterHandle
            {
                get => hAdapterHandle;
                set => hAdapterHandle = value;
            }

            /// <summary>
            /// Gets or sets the adapter flags.
            /// </summary>
            public MSTCP_FLAGS Flags
            {
                get => dwFlags;
                set => dwFlags = value;
            }
        }
    }
}