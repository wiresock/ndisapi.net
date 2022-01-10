// ----------------------------------------------
// <copyright file="NdisApi.IP_V4_FILTER.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using NdisApiDotNet.Native;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace NdisApiDotNet.Filters
{
    /// <summary>
    /// The IPv4 filter type.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IPv4Filter // IP_V4_FILTER
    {
        internal FilterFields m_ValidFields;
        internal IPAddressV4 m_SrcAddress;
        internal IPAddressV4 m_DestAddress;
        internal byte m_Protocol;
        internal byte Padding1;
        internal byte Padding2;
        internal byte Padding3;

        /// <summary>
        /// Gets or sets which of the fields below contain valid values and should be matched against the packet.
        /// </summary>
        public FilterFields ValidFields
        {
            get
            {
                return m_ValidFields;
            }

            set
            {
                m_ValidFields = value;
            }
        }

        /// <summary>
        /// Gets or sets the IP v4 source address.
        /// </summary>
        public IPAddressV4 Source
        {
            get
            {
                return m_SrcAddress;
            }

            set
            {
                m_SrcAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the IP v4 destination address.
        /// </summary>
        public IPAddressV4 Destination
        {
            get
            {
                return m_DestAddress;
            }

            set
            {
                m_DestAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the next protocol.
        /// </summary>
        public byte NextProtocol
        {
            get
            {
                return m_Protocol;
            }

            set
            {
                m_Protocol = value;
            }
        }
    }
}