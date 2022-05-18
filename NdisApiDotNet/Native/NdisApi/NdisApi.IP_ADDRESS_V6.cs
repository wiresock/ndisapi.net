// ----------------------------------------------
// <copyright file="NdisApi.IP_ADDRESS_V6.cs" company="NT Kernel">
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
    /// IPv6 address type.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IP_ADDRESS_V6
    {
        /// <summary>
        /// Which of the IPv6 address types is used below.
        /// </summary>
        [FieldOffset(0)]
        public IP_ADDRESS_V6_TYPE m_AddressType;

        /// <summary>
        /// The IPv6 subnet.
        /// </summary>
        [FieldOffset(4)]
        public IP_SUBNET_V6 m_IpSubnet;

        /// <summary>
        /// The IPv6 range.
        /// </summary>
        [FieldOffset(4)]
        public IP_RANGE_V6 m_IpRange;
    }
}