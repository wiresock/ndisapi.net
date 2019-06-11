// ----------------------------------------------
// <copyright file="NdisApi.MSTCP_FLAGS.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        /// <summary>
        /// Used to set the adapter flags.
        /// </summary>
        [Flags]
        public enum MSTCP_FLAGS : uint
        {
            /// <summary>
            /// No flags.
            /// </summary>
            MSTCP_FLAG_NONE = 0x00000000,

            /// <summary>
            /// Receive packets sent by MSTCP to network interface.
            /// The original packet is dropped.
            /// </summary>
            MSTCP_FLAG_SENT_TUNNEL = 0x00000001,

            /// <summary>
            /// Receive packets sent from network interface to MSTCP.
            /// The original packet is dropped.
            /// </summary>
            MSTCP_FLAG_RECV_TUNNEL = 0x00000002,

            /// <summary>
            /// Receive packets sent from and to MSTCP and network interface.
            /// The original packet is dropped.
            /// </summary>
            MSTCP_FLAG_TUNNEL = MSTCP_FLAG_SENT_TUNNEL | MSTCP_FLAG_RECV_TUNNEL,

            /// <summary>
            /// Receive packets sent by MSTCP to network interface.
            /// The original packet is still delivered to the network.
            /// </summary>
            MSTCP_FLAG_SENT_LISTEN = 0x00000004,

            /// <summary>
            /// Receive packets sent from network interface to MSTCP
            /// The original packet is still delivered to the network.
            /// </summary>
            MSTCP_FLAG_RECV_LISTEN = 0x00000008,

            /// <summary>
            /// Receive packets sent from and to MSTCP and network interface.
            /// The original packet is dropped.
            /// </summary>
            MSTCP_FLAG_LISTEN = MSTCP_FLAG_SENT_LISTEN | MSTCP_FLAG_RECV_LISTEN,

            /// <summary>
            /// In promiscuous mode TCP/IP stack receives all.
            /// </summary>
            MSTCP_FLAG_FILTER_DIRECT = 0x00000010,

            /// <summary>
            /// Passes loopback packet for processing.
            /// </summary>
            MSTCP_FLAG_LOOPBACK_FILTER = 0x00000020,

            /// <summary>
            /// Silently drop loopback packets.
            /// </summary>
            MSTCP_FLAG_LOOPBACK_BLOCK = 0x00000040
        }
    }
}