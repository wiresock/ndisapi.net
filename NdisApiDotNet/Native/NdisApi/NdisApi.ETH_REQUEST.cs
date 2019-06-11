// ----------------------------------------------
// <copyright file="NdisApi.ETH_REQUEST.cs" company="NT Kernel">
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
        /// Used for passing the read packet request to the driver.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ETH_REQUEST
        {
            internal IntPtr hAdapterHandle;
            internal NDISRD_ETH_Packet _ethPacket;

            /// <summary>
            /// Gets or sets the ether packet.
            /// </summary>
            public NDISRD_ETH_Packet Packet
            {
                get => _ethPacket;
                set => _ethPacket = value;
            }

            /// <summary>
            /// Gets or sets the adapter handle.
            /// </summary>
            public IntPtr AdapterHandle
            {
                get => hAdapterHandle;
                set => hAdapterHandle = value;
            }
        }
    }
}