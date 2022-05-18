// ----------------------------------------------
// <copyright file="NdisApi.STATIC_FILTER.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// Defines a static filter entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct STATIC_FILTER
    {
        /// <summary>
        /// The adapter handle.
        /// </summary>
        public ulong m_Adapter;

        /// <summary>
        /// The direction of the packets.
        /// </summary>
        public PACKET_FLAG m_dwDirectionFlags;

        /// <summary>
        /// The action to perform on the packets.
        /// </summary>
        public FILTER_PACKET_ACTION m_FilterAction;

        /// <summary>
        /// Which of the fields below contain valid values and should be matched against the packet.
        /// </summary>
        public STATIC_FILTER_FIELDS m_ValidFields;

        /// <summary>
        /// The time of the last counters reset (in seconds passed since 1 Jan 1980).
        /// </summary>
        public uint m_LastReset;

        /// <summary>
        /// The incoming packets passed through this filter.
        /// </summary>
        public ulong m_PacketsIn;

        /// <summary>
        /// The incoming bytes passed through this filter.
        /// </summary>
        public ulong m_BytesIn;

        /// <summary>
        /// The outgoing packets passed through this filter.
        /// </summary>
        public ulong m_PacketsOut;

        /// <summary>
        /// The outgoing bytes passed through this filter.
        /// </summary>
        public ulong m_BytesOut;

        /// <summary>
        /// The data link filter.
        /// </summary>
        public DATA_LINK_LAYER_FILTER m_DataLinkFilter;

        /// <summary>
        /// The network filter.
        /// </summary>
        public NETWORK_LAYER_FILTER m_NetworkFilter;

        /// <summary>
        /// The transport filter.
        /// </summary>
        public TRANSPORT_LAYER_FILTER m_TransportFilter;

        /// <summary>
        /// Gets or sets the adapter handle.
        /// </summary>
        public IntPtr AdapterHandle
        {
            get => (IntPtr) m_Adapter;
            set => m_Adapter = (ulong) value;
        }

        /// <summary>
        /// The size of <see cref="STATIC_FILTER" />.
        /// </summary>
        public static int Size = Marshal.SizeOf<STATIC_FILTER>();
    }
}