// ----------------------------------------------
// <copyright file="NdisApi.PACKET_FLAG.cs" company="NT Kernel">
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
    /// The packet flag.
    /// </summary>
    [Flags]
    public enum PacketFlag : uint // PACKET_FLAG
    {
        /// <summary>
        /// The packet was intercepted from MSTCP.
        /// </summary>
        Send = 0x00000001, // PACKET_FLAG_ON_SEND

        /// <summary>
        /// The packet was intercepted from the network interface.
        /// </summary>
        Receive = 0x00000002, // PACKET_FLAG_ON_RECEIVE

        Both = Send | Receive
    }
}