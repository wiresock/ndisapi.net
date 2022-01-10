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
    /// <summary>
    /// Used for passing the blocks of packets to and from the driver.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EthMRequest // ETH_M_REQUEST
    {
        internal IntPtr hAdapterHandle;
        internal uint dwPacketsNumber;
        internal uint dwPacketsSuccess;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.EthMRequestEthPacketSize)]
        internal EthPacket[] _ethPacket;

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
        /// Gets or sets the total number of packets that can be set.
        /// </summary>
        public uint PacketsLength
        {
            get
            {
                return dwPacketsNumber;
            }
            set
            {
                dwPacketsNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of packets that are actually set.
        /// </summary>
        public uint PacketsCount
        {
            get
            {
                return dwPacketsSuccess;
            }

            set
            {
                dwPacketsSuccess = value;
            }
        }

        /// <summary>
        /// Gets or sets the ether packets.
        /// </summary>
        public EthPacket[] Packets
        {
            get
            {
                return _ethPacket;
            }

            set
            {
                _ethPacket = value;
            }
        }
    }
}