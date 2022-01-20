// ----------------------------------------------
// <copyright file="NdisApi.IP_SUBNET_V4.cs" company="NT Kernel">
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

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        /// <summary>
        /// The IPv4 subnet type.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IP_SUBNET_V4
        {
            /// <summary>
            /// The IPv4 address.
            /// </summary>
            public uint m_Ip;

            /// <summary>
            /// The IPv4 address mask.
            /// </summary>
            public uint m_IpMask;

            /// <summary>
            /// Initializes a new instance of the <see cref="IP_SUBNET_V4" /> struct.
            /// </summary>
            /// <param name="ipAddress">The IP address.</param>
            /// <param name="ipMaskAddress">The IP address mask.</param>
            public IP_SUBNET_V4(IPAddress ipAddress, IPAddress ipMaskAddress)
            {
                m_Ip = ipAddress.ToUInt32();
                m_IpMask = ipMaskAddress.ToUInt32();
            }

            /// <summary>
            /// Gets or sets the IPv4 address.
            /// </summary>
            public IPAddress Address
            {
                get => m_Ip.ToIPAddress();
                set => m_Ip = value.ToUInt32();
            }

            /// <summary>
            /// Gets or sets the IPv4 address mask.
            /// </summary>
            public IPAddress AddressMask
            {
                get => m_IpMask.ToIPAddress();
                set => m_IpMask = value.ToUInt32();
            }
        }
    }
}