// ----------------------------------------------
// <copyright file="NdisApi.IP_V4_FILTER.cs" company="NT Kernel">
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
        /// The IPv4 filter type.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IP_V4_FILTER
        {
            internal IP_V4_FILTER_FIELDS m_ValidFields;
            internal IP_ADDRESS_V4 m_SrcAddress;
            internal IP_ADDRESS_V4 m_DestAddress;
            internal byte m_Protocol;
            internal byte Padding1;
            internal byte Padding2;
            internal byte Padding3;

            /// <summary>
            /// Gets or sets which of the fields below contain valid values and should be matched against the packet.
            /// </summary>
            public IP_V4_FILTER_FIELDS ValidFields
            {
                get => m_ValidFields;
                set => m_ValidFields = value;
            }

            /// <summary>
            /// Gets or sets the IP v4 source address.
            /// </summary>
            public IP_ADDRESS_V4 Source
            {
                get => m_SrcAddress;
                set => m_SrcAddress = value;
            }

            /// <summary>
            /// Gets or sets the IP v4 destination address.
            /// </summary>
            public IP_ADDRESS_V4 Destination
            {
                get => m_DestAddress;
                set => m_DestAddress = value;
            }

            /// <summary>
            /// Gets or sets the next protocol.
            /// </summary>
            public byte NextProtocol
            {
                get => m_Protocol;
                set => m_Protocol = value;
            }
        }
    }
}