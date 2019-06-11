// ----------------------------------------------
// <copyright file="NdisApi.TCPUDP_FILTER.cs" company="NT Kernel">
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
        /// The TCP/UDP filter.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TCPUDP_FILTER
        {
            internal TCPUDP_FILTER_FIELDS m_ValidFields;
            internal PORT_RANGE m_SourcePort;
            internal PORT_RANGE m_DestPort;
            internal byte m_TCPFlags;

            /// <summary>
            /// Gets or sets which of the fields below contain valid values and should be matched against the packet.
            /// </summary>
            public TCPUDP_FILTER_FIELDS ValidFields
            {
                get => m_ValidFields;
                set => m_ValidFields = value;
            }

            /// <summary>
            /// Gets or sets the source port range.
            /// </summary>
            public PORT_RANGE Source
            {
                get => m_SourcePort;
                set => m_SourcePort = value;
            }

            /// <summary>
            /// Gets or sets the destination port range.
            /// </summary>
            public PORT_RANGE Destination
            {
                get => m_DestPort;
                set => m_DestPort = value;
            }

            /// <summary>
            /// Gets or sets the TCP flags combination.
            /// </summary>
            public byte TcpFlags
            {
                get => m_TCPFlags;
                set => m_TCPFlags = value;
            }
        }
    }
}