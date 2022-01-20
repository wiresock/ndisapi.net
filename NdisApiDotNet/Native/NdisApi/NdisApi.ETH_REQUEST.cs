// ----------------------------------------------
// <copyright file="NdisApi.ETH_REQUEST.cs" company="NT Kernel">
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
        /// Used for passing the read packet request to the driver.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ETH_REQUEST
        {
            /// <summary>
            /// The adapter handle.
            /// </summary>
            public IntPtr hAdapterHandle;

            /// <summary>
            /// The <see cref="NDISRD_ETH_Packet" />.
            /// </summary>
            public NDISRD_ETH_Packet EthPacket;

            /// <summary>
            /// The size of <see cref="ETH_REQUEST" />.
            /// </summary>
            public static int Size = Marshal.SizeOf<ETH_REQUEST>();
        }
    }
}