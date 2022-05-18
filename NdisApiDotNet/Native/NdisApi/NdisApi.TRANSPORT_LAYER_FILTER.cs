// ----------------------------------------------
// <copyright file="NdisApi.TRANSPORT_LAYER_FILTER.cs" company="NT Kernel">
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
    /// Represents transport layer (OSI-7) filter level.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANSPORT_LAYER_FILTER
    {
        /// <summary>
        /// The union selector, which must always be set to <see cref="FILTER_SELECT_FLAGS.TCPUDP" />.
        /// </summary>
        public FILTER_SELECT_FLAGS m_dwUnionSelector;

        /// <summary>
        /// The TCP/UDP filter.
        /// </summary>
        public TCPUDP_FILTER m_TcpUdp;
    }
}