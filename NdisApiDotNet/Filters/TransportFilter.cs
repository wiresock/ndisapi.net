// ----------------------------------------------
// <copyright file="NdisApi.TRANSPORT_LAYER_FILTER.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Filters
{
    /// <summary>
    /// Represents transport layer (OSI-7) filter level.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TransportFilter // TRANSPORT_LAYER_FILTER
    {
        internal uint m_dwUnionSelector;
        internal TCPUDPFilter m_TcpUdp;

        /// <summary>
        /// Gets or sets the TCP/UDP filter.
        /// </summary>
        public TCPUDPFilter Filter
        {
            get
            {
                return m_TcpUdp;
            }

            set
            {
                m_TcpUdp = value;
            }
        }

        /// <summary>
        /// Gets or sets the union selector.
        /// Must always be set to <see cref="FilterSelectFlags.TCPUDP" />.
        /// </summary>
        public FilterSelectFlags Selector
        {
            get
            {
                return (FilterSelectFlags)m_dwUnionSelector;
            }

            set
            {
                m_dwUnionSelector = (uint)value;
            }
        }

        public TransportFilter(TCPUDPFilter filter)
        {
            m_dwUnionSelector = (uint)FilterSelectFlags.TCPUDP;
            m_TcpUdp = filter;
        }

        public static implicit operator TransportFilter(TCPUDPFilter filter)
        {
            return new TransportFilter(filter);
        }
    }
}