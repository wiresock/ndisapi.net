// ----------------------------------------------
// <copyright file="NdisApi.IP_V6_FILTER.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// The IPv6 filter type.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IP_V6_FILTER
    {
        /// <summary>
        /// Which of the fields below contain valid values and should be matched against the packet.
        /// </summary>
        public IP_V6_FILTER_FIELDS m_ValidFields;

        /// <summary>
        /// The IPv6 source address.
        /// </summary>
        public IP_ADDRESS_V6 m_SrcAddress;

        /// <summary>
        /// The IPv6 destination address.
        /// </summary>
        public IP_ADDRESS_V6 m_DestAddress;

        /// <summary>
        /// The next protocol.
        /// </summary>
        public byte m_Protocol;

        private readonly byte m_Padding1;
        private readonly byte m_Padding2;
        private readonly byte m_Padding3;
    }
}