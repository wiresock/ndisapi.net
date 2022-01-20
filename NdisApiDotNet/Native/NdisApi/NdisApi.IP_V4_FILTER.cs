// ----------------------------------------------
// <copyright file="NdisApi.IP_V4_FILTER.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
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
        /// The IPv4 filter type.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IP_V4_FILTER
        {
            /// <summary>
            /// Which of the fields below contain valid values and should be matched against the packet.
            /// </summary>
            public IP_V4_FILTER_FIELDS m_ValidFields;

            /// <summary>
            /// The IPv4 source address.
            /// </summary>
            public IP_ADDRESS_V4 m_SrcAddress;

            /// <summary>
            /// The IPv4 destination address.
            /// </summary>
            public IP_ADDRESS_V4 m_DestAddress;

            /// <summary>
            /// The next protocol.
            /// </summary>
            public byte m_Protocol;

            internal byte Padding1;
            internal byte Padding2;
            internal byte Padding3;
        }
    }
}