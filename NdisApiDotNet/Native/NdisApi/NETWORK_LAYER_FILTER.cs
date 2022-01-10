// ----------------------------------------------
// <copyright file="NdisApi.NETWORK_LAYER_FILTER.cs" company="NT Kernel">
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
    /// <summary>
    /// Represents network layer (OSI-7) filter level.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct NetworkFilter // NETWORK_LAYER_FILTER
    {
        [FieldOffset(0)]
        internal uint m_dwUnionSelector;
        [FieldOffset(4)]
        internal IPv4Filter m_IPv4;
        [FieldOffset(4)]
        internal IPv6Filter m_IPv6;

        /// <summary>
        /// Gets or sets the IP v6 filter.
        /// </summary>
        public IPv6Filter IPv6
        {
            get
            {
                return m_IPv6;
            }

            set
            {
                m_IPv6 = value;
            }
        }

        /// <summary>
        /// Gets or sets the IP v4 filter.
        /// </summary>
        public IPv4Filter IPv4
        {
            get
            {
                return m_IPv4;
            }

            set
            {
                m_IPv4 = value;
            }
        }

        /// <summary>
        /// Gets or sets the union selector.
        /// Must always be set to <see cref="FilterSelectFlags.IPv4" /> or <see cref="FilterSelectFlags.IPv6" />.
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
    }
}