// ----------------------------------------------
// <copyright file="NdisApi.NETWORK_LAYER_FILTER.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// Represents network layer (OSI-7) filter level.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct NETWORK_LAYER_FILTER
    {
        /// <summary>
        /// The union selector, which must always be set to <see cref="FILTER_SELECT_FLAGS.IPV4" /> or <see cref="FILTER_SELECT_FLAGS.IPV6" />.
        /// </summary>
        [FieldOffset(0)]
        public FILTER_SELECT_FLAGS m_dwUnionSelector;

        /// <summary>
        /// The IPv4 filter.
        /// </summary>
        [FieldOffset(4)]
        public IP_V4_FILTER m_IPv4;

        /// <summary>
        /// The IPv6 filter.
        /// </summary>
        [FieldOffset(4)]
        public IP_V6_FILTER m_IPv6;
    }
}