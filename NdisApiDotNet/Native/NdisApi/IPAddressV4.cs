// ----------------------------------------------
// <copyright file="NdisApi.IP_ADDRESS_V4.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    /// <summary>
    /// IPv4 address.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IPAddressV4 // IP_ADDRESS_V4
    {
        [FieldOffset(0)]
        internal IPAddressType m_AddressType;
        [FieldOffset(4)]
        internal IPSubnetV4 m_IpSubnet;
        [FieldOffset(4)]
        internal IPRangeV4 m_IpRange;

        /// <summary>
        /// Gets or sets which of the IP v4 address types is used below.
        /// </summary>
        public IPAddressType AddressType
        {
            get
            {
                return m_AddressType;
            }

            set
            {
                m_AddressType = value;
            }
        }

        /// <summary>
        /// Gets or sets the IP v4 subnet.
        /// </summary>
        public IPSubnetV4 Subnet
        {
            get
            {
                return m_IpSubnet;
            }

            set
            {
                m_IpSubnet = value;
            }
        }

        /// <summary>
        /// Gets or sets the IP v4 range.
        /// </summary>
        public IPRangeV4 Range
        {
            get
            {
                return m_IpRange;
            }

            set
            {
                m_IpRange = value;
            }
        }
    }
}