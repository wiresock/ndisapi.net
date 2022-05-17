// ----------------------------------------------
// <copyright file="NdisApi.FILTER_SELECT_FLAGS.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// The filter select flags.
    /// </summary>
    public enum FILTER_SELECT_FLAGS : uint
    {
        /// <summary>
        /// Ethernet 802.3.
        /// </summary>
        ETH_802_3 = 0x00000001,

        /// <summary>
        /// IPv4.
        /// </summary>
        IPV4 = 0x00000001,

        /// <summary>
        /// IPv6.
        /// </summary>
        IPV6 = 0x00000002,

        /// <summary>
        /// TCP/UDP.
        /// </summary>
        TCPUDP = 0x00000001
    }
}