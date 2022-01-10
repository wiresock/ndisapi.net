// ----------------------------------------------
// <copyright file="NdisApi.FILTER_SELECT_FLAGS.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Filters
{
    /// <summary>
    /// The filter select flags.
    /// </summary>
    public enum FilterSelectFlags : uint // FILTER_SELECT_FLAGS
    {
        /// <summary>
        /// The eth 802 3.
        /// </summary>
        Eth802_3 = 0x00000001, // ETH_802_3

        /// <summary>
        /// The IPv4.
        /// </summary>
        IPv4 = 0x00000001, // IPV4

        /// <summary>
        /// The ip v6.
        /// </summary>
        IPv6 = 0x00000002, // IPV6

        /// <summary>
        /// The TCP/UDP.
        /// </summary>
        TCPUDP = 0x00000001
    }
}