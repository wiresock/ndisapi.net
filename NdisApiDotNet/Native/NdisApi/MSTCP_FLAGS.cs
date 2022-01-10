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
    /// <summary>
    /// Used to set the adapter flags.
    /// </summary>
    [Flags]
    public enum MSTCPFlags : uint // MSTCP_FLAGS
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0x00000000, // MSTCP_FLAG_NONE

        /// <summary>
        /// Receive packets sent by MSTCP to network interface.
        /// The original packet is dropped.
        /// </summary>
        TunnelSent = 0x00000001, // MSTCP_FLAG_SENT_TUNNEL

        /// <summary>
        /// Receive packets sent from network interface to MSTCP.
        /// The original packet is dropped.
        /// </summary>
        TunnelReceived = 0x00000002, // MSTCP_FLAG_RECV_TUNNEL

        /// <summary>
        /// Receive packets sent from and to MSTCP and network interface.
        /// The original packet is dropped.
        /// </summary>
        Tunnel = TunnelSent | TunnelReceived, // MSTCP_FLAG_TUNNEL

        /// <summary>
        /// Receive packets sent by MSTCP to network interface.
        /// The original packet is still delivered to the network.
        /// </summary>
        ListenSent = 0x00000004, // MSTCP_FLAG_SENT_LISTEN

        /// <summary>
        /// Receive packets sent from network interface to MSTCP
        /// The original packet is still delivered to the network.
        /// </summary>
        ListenReceived = 0x00000008, // MSTCP_FLAG_RECV_LISTEN

        /// <summary>
        /// Receive packets sent from and to MSTCP and network interface.
        /// The original packet is dropped.
        /// </summary>
        Listen = ListenSent | ListenReceived, // MSTCP_FLAG_LISTEN

        /// <summary>
        /// In promiscuous mode TCP/IP stack receives all.
        /// </summary>
        FilterDirect = 0x00000010, // MSTCP_FLAG_FILTER_DIRECT

        /// <summary>
        /// Passes loopback packet for processing.
        /// </summary>
        FilterLoopback = 0x00000020, // MSTCP_FLAG_LOOPBACK_FILTER

        /// <summary>
        /// Silently drop loopback packets.
        /// </summary>
        BlockLoopback = 0x00000040 // MSTCP_FLAG_LOOPBACK_BLOCK
    }
}