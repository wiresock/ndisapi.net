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
    public static partial class NdisApi
    {
        /// <summary>
        /// Represents network layer (OSI-7) filter level.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct NETWORK_LAYER_FILTER
        {
            [FieldOffset(0)]
            internal uint m_dwUnionSelector;
            [FieldOffset(4)]
            internal IP_V4_FILTER m_IPv4;
            [FieldOffset(4)]
            internal IP_V6_FILTER m_IPv6;
            
            /// <summary>
            /// Gets or sets the IP v6 filter.
            /// </summary>
            public IP_V6_FILTER IPv6
            {
                get => m_IPv6;
                set => m_IPv6 = value;
            }

            /// <summary>
            /// Gets or sets the IP v4 filter.
            /// </summary>
            public IP_V4_FILTER IPv4
            {
                get => m_IPv4;
                set => m_IPv4 = value;
            }

            /// <summary>
            /// Gets or sets the union selector.
            /// Must always be set to <see cref="FILTER_SELECT_FLAGS.IPV4" /> or <see cref="FILTER_SELECT_FLAGS.IPV6" />.
            /// </summary>
            public FILTER_SELECT_FLAGS Selector
            {
                get => (FILTER_SELECT_FLAGS) m_dwUnionSelector;
                set => m_dwUnionSelector = (uint) value;
            }
        }
    }
}