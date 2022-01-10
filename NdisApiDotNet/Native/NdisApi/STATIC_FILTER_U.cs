// ----------------------------------------------
// <copyright file="NdisApi.STATIC_FILTER_U.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    /// <summary>
    /// Defines a static filter entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StaticFilterUnsafe // STATIC_FILTER_U
    {
        internal ulong m_Adapter;
        internal PacketFlag m_dwDirectionFlags;
        internal FilterAction m_FilterAction;
        internal FilterFields m_ValidFields;
        internal uint m_LastReset;
        internal ulong m_PacketsIn;
        internal ulong m_BytesIn;
        internal ulong m_PacketsOut;
        internal ulong m_BytesOut;
        internal DataLinkFilterUnsafe m_DataLinkFilter;
        internal NetworkFilter m_NetworkFilter;
        internal TransportFilter m_TransportFilter;

        /// <summary>
        /// Gets or sets the direction of the packets.
        /// </summary>
        public PacketFlag DirectionFlags
        {
            get
            {
                return m_dwDirectionFlags;
            }

            set
            {
                m_dwDirectionFlags = value;
            }
        }

        /// <summary>
        /// Gets or sets the adapter handle.
        /// </summary>
        public IntPtr Adapter
        {
            get
            {
                return (IntPtr)m_Adapter;
            }

            set
            {
                m_Adapter = (ulong)value;
            }
        }

        /// <summary>
        /// Gets or sets the action to perform on the packets.
        /// </summary>
        public FilterAction FilterAction
        {
            get
            {
                return m_FilterAction;
            }

            set
            {
                m_FilterAction = value;
            }
        }

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
        /// Gets or sets the time of the last counters reset (in seconds passed since 1 Jan 1980).
        /// </summary>
        public uint LastReset
        {
            get
            {
                return m_LastReset;
            }

            set
            {
                m_LastReset = value;
            }
        }

        /// <summary>
        /// Gets or sets the incoming packets passed through this filter.
        /// </summary>
        public ulong PacketsIn
        {
            get
            {
                return m_PacketsIn;
            }

            set
            {
                m_PacketsIn = value;
            }
        }

        /// <summary>
        /// Gets or sets the incoming bytes passed through this filter.
        /// </summary>
        public ulong BytesIn
        {
            get
            {
                return m_BytesIn;
            }

            set
            {
                m_BytesIn = value;
            }
        }

        /// <summary>
        /// Gets or sets the outgoing packets passed through this filter.
        /// </summary>
        public ulong PacketsOut
        {
            get
            {
                return m_PacketsOut;
            }

            set
            {
                m_PacketsOut = value;
            }
        }

        /// <summary>
        /// Gets or sets the outgoing bytes passed through this filter.
        /// </summary>
        public ulong BytesOut
        {
            get
            {
                return m_BytesOut;
            }

            set
            {
                m_BytesOut = value;
            }
        }

        /// <summary>
        /// Gets or sets the data link filter.
        /// </summary>
        public DataLinkFilterUnsafe DataLinkFilter
        {
            get
            {
                return m_DataLinkFilter;
            }

            set
            {
                m_DataLinkFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets the network filter.
        /// </summary>
        public NetworkFilter NetworkFilter
        {
            get
            {
                return m_NetworkFilter;
            }

            set
            {
                m_NetworkFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets the transport filter.
        /// </summary>
        public TransportFilter TransportFilter
        {
            get
            {
                return m_TransportFilter;
            }

            set
            {
                m_TransportFilter = value;
            }
        }
    }
}