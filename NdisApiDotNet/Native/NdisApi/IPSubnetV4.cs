// ----------------------------------------------
// <copyright file="NdisApi.IP_SUBNET_V4.cs" company="NT Kernel">
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
    /// The IPv4 subnet type.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IPSubnetV4 // IP_SUBNET_V4
    {
        internal uint m_Ip;
        internal uint m_IpMask;

        /// <summary>
        /// Initializes a new instance of the <see cref="IPSubnetV4" /> struct.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="ipMaskAddress">The ip mask address.</param>
        public IPSubnetV4(IPAddress ipAddress, IPAddress ipMaskAddress)
        {
            m_Ip = ipAddress.ToUInt32();
            m_IpMask = ipMaskAddress.ToUInt32();
        }

        /// <summary>
        /// Gets or sets the IPv4 mask expressed as uint.
        /// </summary>
        public uint AddressMaskRaw
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
        /// Gets or sets the IPv4 address expressed as uint.
        /// </summary>
        public uint AddressRaw
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
        /// Gets or sets the IPv4 address.
        /// </summary>
        public IPAddress Address
        {
            get
            {
                return AddressRaw.ToIPAddress();
            }
            set
            {
                AddressRaw = value.ToUInt32();
            }
        }

        /// <summary>
        /// Gets or sets the IPv4 address mask.
        /// </summary>
        public IPAddress AddressMask
        {
            get
            {
                return AddressMaskRaw.ToIPAddress();
            }
            set
            {
                AddressMaskRaw = value.ToUInt32();
            }
        }
    }
}