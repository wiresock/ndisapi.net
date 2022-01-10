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
    /// <summary>
    /// Used for setting adapter mode.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AdapterMode
    {
        internal IntPtr hAdapterHandle;
        internal MSTCPFlags dwFlags;

        /// <summary>
        /// Gets or sets the adapter handle.
        /// </summary>
        public IntPtr AdapterHandle
        {
            get
            {
                return hAdapterHandle;
            }
            set
            {
                hAdapterHandle = value;
            }
        }

        /// <summary>
        /// Gets or sets the adapter flags.
        /// </summary>
        public MSTCPFlags Flags
        {
            get
            {
                return dwFlags;
            }
            set
            {
                dwFlags = value;
            }
        }
    }
}