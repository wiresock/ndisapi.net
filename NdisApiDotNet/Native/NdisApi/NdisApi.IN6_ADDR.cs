// ----------------------------------------------
// <copyright file="NdisApi.IN6_ADDR.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
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

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// The IPv6 address.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IN6_ADDR
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IN6_ADDR" /> struct.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        public IN6_ADDR(IPAddress ipAddress) : this(ipAddress.GetAddressBytes())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IN6_ADDR" /> struct.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <exception cref="ArgumentOutOfRangeException">byteArray</exception>
        public IN6_ADDR(IReadOnlyList<byte> byteArray)
            : this()
        {
            if (byteArray.Count is < 16 or > 16)
            {
                throw new ArgumentOutOfRangeException(nameof(byteArray));
            }

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

        public byte b0;
        public byte b1;
        public byte b2;
        public byte b3;
        public byte b4;
        public byte b5;
        public byte b6;
        public byte b7;
        public byte b8;
        public byte b9;
        public byte b10;
        public byte b11;
        public byte b12;
        public byte b13;
        public byte b14;
        public byte b15;
    }
}