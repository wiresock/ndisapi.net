// ----------------------------------------------
// <copyright file="NdisApi.TCPUDP_FILTER.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System.Runtime.InteropServices;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        /// <summary>
        /// The TCP/UDP filter.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TCPUDP_FILTER
        {
            /// <summary>
            /// Which of the fields below contain valid values and should be matched against the packet.
            /// </summary>
            public TCPUDP_FILTER_FIELDS m_ValidFields;

            /// <summary>
            /// The source port range.
            /// </summary>
            public PORT_RANGE m_SourcePort;

            /// <summary>
            /// The destination port range.
            /// </summary>
            public PORT_RANGE m_DestPort;

            /// <summary>
            /// The TCP flags combination.
            /// </summary>
            public byte m_TCPFlags;
        }
    }
}