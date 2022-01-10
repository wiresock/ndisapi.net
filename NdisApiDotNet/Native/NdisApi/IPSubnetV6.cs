// ----------------------------------------------
// <copyright file="NdisApi.IP_SUBNET_V6.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using NdisApiDotNet.Extensions;
using System.Net;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    /// <summary>
    /// The IPv6 subnet type.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IPSubnetV6 // IP_SUBNET_V6
    {
        [FieldOffset(0)]
        internal IN6_ADDR m_Ip;
        [FieldOffset(16)]
        internal IN6_ADDR m_IpMask;

        /// <summary>
        /// Initializes a new instance of the <see cref="IPSubnetV6" /> struct.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="ipAddressMask">The ip address mask.</param>
        public IPSubnetV6(IPAddress ipAddress, IPAddress ipAddressMask)
        {
            m_Ip = new IN6_ADDR(ipAddress);
            m_IpMask = new IN6_ADDR(ipAddressMask);
        }


        /// <summary>
        /// Gets or sets the IP v6 address.
        /// </summary>
        public IN6_ADDR AddressRaw
        {
            get
            {
                return m_Ip;
            }

            set
            {
                m_Ip = value;
            }
        }

        /// <summary>
        /// Gets or sets the IP v6 mask.
        /// </summary>
        public IN6_ADDR AddressMaskRaw
        {
            get
            {
                return m_IpMask;
            }

            set
            {
                m_IpMask = value;
            }
        }

        /// <summary>
        /// Gets or sets the IPv6 address.
        /// </summary>
        public IPAddress Address
        {
            get
            {
                return AddressRaw;
            }

            set
            {
                AddressRaw = value;
            }
        }

        /// <summary>
        /// Gets or sets the IPv6 address mask.
        /// </summary>
        public IPAddress AddressMask
        {
            get
            {
                return AddressMaskRaw;
            }

            set
            {
                AddressMaskRaw = value;
            }
        }
    }
}