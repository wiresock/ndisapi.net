// ----------------------------------------------
// <copyright file="NdisApi.IP_SUBNET_V6.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System.Net;
using System.Runtime.InteropServices;
using NdisApiDotNet.Extensions;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// The IPv6 subnet type.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IP_SUBNET_V6
    {
        /// <summary>
        /// The IPv6 address.
        /// </summary>
        [FieldOffset(0)]
        public IN6_ADDR m_Ip;

        /// <summary>
        /// The IPv6 address mask.
        /// </summary>
        [FieldOffset(16)]
        public IN6_ADDR m_IpMask;

        /// <summary>
        /// Initializes a new instance of the <see cref="IP_SUBNET_V6" /> struct.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="ipAddressMask">The IP address mask.</param>
        public IP_SUBNET_V6(IPAddress ipAddress, IPAddress ipAddressMask)
        {
            m_Ip = new IN6_ADDR(ipAddress);
            m_IpMask = new IN6_ADDR(ipAddressMask);
        }

        /// <summary>
        /// Gets or sets the IPv6 address.
        /// </summary>
        public IPAddress Address
        {
            get => m_Ip.ToIPAddress();
            set => m_Ip = value.ToIN6_ADDR();
        }

        /// <summary>
        /// Gets or sets the IPv6 address mask.
        /// </summary>
        public IPAddress AddressMask
        {
            get => m_IpMask.ToIPAddress();
            set => m_IpMask = value.ToIN6_ADDR();
        }
    }
}