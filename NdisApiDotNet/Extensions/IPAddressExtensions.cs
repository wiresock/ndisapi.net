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
        /// <returns><see cref="uint" />.</returns>
        internal static uint ToUInt32(this IPAddress ipAddress)
        {
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork || ipAddress.IsIPv4MappedToIPv6)
                return BitConverter.ToUInt32(ipAddress.GetAddressBytes().Reverse().ToArray(), 0);


            return 0;
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
    }
}