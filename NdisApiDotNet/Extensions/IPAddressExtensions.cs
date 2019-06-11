// ----------------------------------------------
// <copyright file="IPAddressExtensions.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace NdisApiDotNet.Extensions
{
    internal static class IPAddressExtensions
    {
        /// <summary>
        /// Converts the specified <see cref="ipAddress" /> an uint32.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns><see cref="System.UInt32" />.</returns>
        internal static uint ToUInt32(this IPAddress ipAddress)
        {
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork || ipAddress.IsIPv4MappedToIPv6)
                return BitConverter.ToUInt32(ipAddress.GetAddressBytes().Reverse().ToArray(), 0);


            return 0;
        }

        /// <summary>
        /// Converts the specified <see cref="in6Addr" /> to an ip address.
        /// </summary>
        /// <param name="in6Addr">The IPv6 addr.</param>
        /// <returns><see cref="IPAddress" />.</returns>
        internal static IPAddress ToIPAddress(this Native.NdisApi.IN6_ADDR in6Addr)
        {
            return new IPAddress(new[]
            {
                    in6Addr.b0,
                    in6Addr.b1,
                    in6Addr.b2,
                    in6Addr.b3,
                    in6Addr.b4,
                    in6Addr.b5,
                    in6Addr.b6,
                    in6Addr.b7,
                    in6Addr.b8,
                    in6Addr.b9,
                    in6Addr.b10,
                    in6Addr.b11,
                    in6Addr.b12,
                    in6Addr.b13,
                    in6Addr.b14,
                    in6Addr.b15
            });
        }

        /// <summary>
        /// Converts the specified <see cref="in4Addr" /> to an ip address.
        /// </summary>
        /// <param name="in4Addr">The IPv4 address.</param>
        /// <returns><see cref="IPAddress" />.</returns>
        internal static IPAddress ToIPAddress(this uint in4Addr)
        {
            return new IPAddress(in4Addr);
        }

        /// <summary>
        /// Converts the specified <see cref="ipAddress" /> an IN6_ADDR.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns><see cref="Native.NdisApi.IN6_ADDR"/>.</returns>
        internal static Native.NdisApi.IN6_ADDR ToIN6_ADDR(this IPAddress ipAddress)
        {
            return new Native.NdisApi.IN6_ADDR(ipAddress);
        }
    }
}