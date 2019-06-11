// ----------------------------------------------
// <copyright file="NdisApi.STATIC_FILTER.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        /// <summary>
        /// Defines a static filter entry.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
        public struct STATIC_FILTER
        {
            internal ulong m_Adapter;
            internal PACKET_FLAG m_dwDirectionFlags;
            internal FILTER_PACKET_ACTION m_FilterAction;
            internal STATIC_FILTER_FIELDS m_ValidFields;
            internal uint m_LastReset;
            internal ulong m_PacketsIn;
            internal ulong m_BytesIn;
            internal ulong m_PacketsOut;
            internal ulong m_BytesOut;
            internal DATA_LINK_LAYER_FILTER m_DataLinkFilter;
            internal NETWORK_LAYER_FILTER m_NetworkFilter;
            internal TRANSPORT_LAYER_FILTER m_TransportFilter;

            /// <summary>
            /// Gets or sets the direction of the packets.
            /// </summary>
            public PACKET_FLAG Direction
            {
                get => m_dwDirectionFlags;
                set => m_dwDirectionFlags = value;
            }

            /// <summary>
            /// Gets or sets the adapter handle.
            /// </summary>
            public IntPtr AdapterHandle
            {
                get => (IntPtr)m_Adapter;
                set => m_Adapter = (ulong)value;
            }

            /// <summary>
            /// Gets or sets the action to perform on the packets.
            /// </summary>
            public FILTER_PACKET_ACTION Action
            {
                get => m_FilterAction;
                set => m_FilterAction = value;
            }

            /// <summary>
            /// Gets or sets which of the fields below contain valid values and should be matched against the packet.
            /// </summary>
            public STATIC_FILTER_FIELDS ValidFields
            {
                get => m_ValidFields;
                set => m_ValidFields = value;
            }

            /// <summary>
            /// Gets or sets the time of the last counters reset (in seconds passed since 1 Jan 1980).
            /// </summary>
            public uint LastReset
            {
                get => m_LastReset;
                set => m_LastReset = value;
            }

            /// <summary>
            /// Gets or sets the incoming packets passed through this filter.
            /// </summary>
            public ulong PacketsIn
            {
                get => m_PacketsIn;
                set => m_PacketsIn = value;
            }

            /// <summary>
            /// Gets or sets the incoming bytes passed through this filter.
            /// </summary>
            public ulong BytesIn
            {
                get => m_BytesIn;
                set => m_BytesIn = value;
            }

            /// <summary>
            /// Gets or sets the outgoing packets passed through this filter.
            /// </summary>
            public ulong PacketsOut
            {
                get => m_PacketsOut;
                set => m_PacketsOut = value;
            }

            /// <summary>
            /// Gets or sets the outgoing bytes passed through this filter.
            /// </summary>
            public ulong BytesOut
            {
                get => m_BytesOut;
                set => m_BytesOut = value;
            }

            /// <summary>
            /// Gets or sets the data link filter.
            /// </summary>
            public DATA_LINK_LAYER_FILTER DataLinkFilter
            {
                get => m_DataLinkFilter;
                set => m_DataLinkFilter = value;
            }

            /// <summary>
            /// Gets or sets the network filter.
            /// </summary>
            public NETWORK_LAYER_FILTER NetworkFilter
            {
                get => m_NetworkFilter;
                set => m_NetworkFilter = value;
            }

            /// <summary>
            /// Gets or sets the transport filter.
            /// </summary>
            public TRANSPORT_LAYER_FILTER TransportFilter
            {
                get => m_TransportFilter;
                set => m_TransportFilter = value;
            }
        }
    }
}