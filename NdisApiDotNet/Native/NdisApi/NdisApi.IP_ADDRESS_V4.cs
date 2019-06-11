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
    public static partial class NdisApi
    {
        /// <summary>
        /// IPv4 address.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct IP_ADDRESS_V4
        {
            [FieldOffset(0)]
            internal IP_ADDRESS_V4_TYPE m_AddressType;
            [FieldOffset(4)]
            internal IP_SUBNET_V4 m_IpSubnet;
            [FieldOffset(4)]
            internal IP_RANGE_V4 m_IpRange;

            /// <summary>
            /// Gets or sets which of the IP v4 address types is used below.
            /// </summary>
            public IP_ADDRESS_V4_TYPE AddressType
            {
                get => m_AddressType;
                set => m_AddressType = value;
            }

            /// <summary>
            /// Gets or sets the IP v4 subnet.
            /// </summary>
            public IP_SUBNET_V4 Subnet
            {
                get => m_IpSubnet;
                set => m_IpSubnet = value;
            }

            /// <summary>
            /// Gets or sets the IP v4 range.
            /// </summary>
            public IP_RANGE_V4 Range
            {
                get => m_IpRange;
                set => m_IpRange = value;
            }
        }
    }
}