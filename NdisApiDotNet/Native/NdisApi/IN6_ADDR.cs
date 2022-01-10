// ----------------------------------------------
// <copyright file="NdisApi.IN6_ADDR.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    /// <summary>
    /// The IPv6 address.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IN6_ADDR // IN6_ADDR
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IN6_ADDR" /> struct.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        public IN6_ADDR(IPAddress ipAddress) : this(ipAddress.GetAddressBytes()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IN6_ADDR" /> struct.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <exception cref="ArgumentOutOfRangeException">byteArray</exception>
        public IN6_ADDR(IReadOnlyList<byte> byteArray) : this()
        {
            if (byteArray.Count < 16 || byteArray.Count > 16) throw new ArgumentOutOfRangeException(nameof(byteArray));

            b0 = byteArray[0];
            b1 = byteArray[1];
            b2 = byteArray[2];
            b3 = byteArray[3];
            b4 = byteArray[4];
            b5 = byteArray[5];
            b6 = byteArray[6];
            b7 = byteArray[7];
            b8 = byteArray[8];
            b9 = byteArray[9];
            b10 = byteArray[10];
            b11 = byteArray[11];
            b12 = byteArray[12];
            b13 = byteArray[13];
            b14 = byteArray[14];
            b15 = byteArray[15];
        }

        public byte[] GetBytes()
        {
            return new[] { b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15 };
        }

        public static implicit operator IPAddress(IN6_ADDR address)
        {
            return new IPAddress(address.GetBytes());
        }

        public static implicit operator IN6_ADDR(IPAddress ipAddress)
        {
            return new IN6_ADDR(ipAddress);
        }

        public byte b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15;
    }
}