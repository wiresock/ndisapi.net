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
    /// <summary>
    /// Used for passing the read packet request to the driver.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EthRequest // ETH_REQUEST
    {
        internal IntPtr hAdapterHandle;
        internal EthPacket _ethPacket;

        /// <summary>
        /// Gets or sets the ether packet.
        /// </summary>
        public EthPacket Packet
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
    }
}