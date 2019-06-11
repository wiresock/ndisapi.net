// ----------------------------------------------
// <copyright file="NdisApi.ETH_M_REQUEST.cs" company="NT Kernel">
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
        /// Used for passing the blocks of packets to and from the driver.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ETH_M_REQUEST
        {
            internal IntPtr hAdapterHandle;
            internal uint dwPacketsNumber;
            internal uint dwPacketsSuccess;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = EthMRequestEthPacketSize)]
            internal NDISRD_ETH_Packet[] _ethPacket;

            /// <summary>
            /// Gets or sets the adapter handle.
            /// </summary>
            public IntPtr AdapterHandle
            {
                get => hAdapterHandle;
                set => hAdapterHandle = value;
            }

            /// <summary>
            /// Gets or sets the total number of packets that can be set.
            /// </summary>
            public uint PacketsLength
            {
                get => dwPacketsNumber;
                set => dwPacketsNumber = value;
            }

            /// <summary>
            /// Gets or sets the number of packets that are actually set.
            /// </summary>
            public uint PacketsCount
            {
                get => dwPacketsSuccess;
                set => dwPacketsSuccess = value;
            }

            /// <summary>
            /// Gets or sets the ether packets.
            /// </summary>
            public NDISRD_ETH_Packet[] Packets
            {
                get => _ethPacket;
                set => _ethPacket = value;
            }
        }
    }
}