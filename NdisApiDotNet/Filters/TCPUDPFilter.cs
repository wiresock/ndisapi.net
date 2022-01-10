// ----------------------------------------------
// <copyright file="NdisApi.TCPUDP_FILTER.cs" company="NT Kernel">
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
    /// The TCP/UDP filter.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TCPUDPFilter // TCPUDP_FILTER
    {
        internal FilterFields m_ValidFields;
        internal PortRange m_SourcePort;
        internal PortRange m_DestPort;
        internal byte m_TCPFlags;

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
        /// Gets or sets the source port range.
        /// </summary>
        public PortRange Source
        {
            get
            {
                return m_SourcePort;
            }

            set
            {
                m_SourcePort = value;
            }
        }

        /// <summary>
        /// Gets or sets the destination port range.
        /// </summary>
        public PortRange Destination
        {
            get
            {
                return m_DestPort;
            }

            set
            {
                m_DestPort = value;
            }
        }

        /// <summary>
        /// Gets or sets the TCP flags combination.
        /// </summary>
        public byte TCPFlags
        {
            get
            {
                return m_TCPFlags;
            }

            set
            {
                m_TCPFlags = value;
            }
        }

        public TCPUDPFilter(FilterFields validFields, PortRange source, PortRange destination, byte tcpFlags = 0)
        {
            m_ValidFields = validFields;
            m_SourcePort = source;
            m_DestPort = destination;
            m_TCPFlags = tcpFlags;
        }

        public TCPUDPFilter(FilterFields validFields, PortRange ports) : this(validFields, ports, ports)
        {
        }
    }
}