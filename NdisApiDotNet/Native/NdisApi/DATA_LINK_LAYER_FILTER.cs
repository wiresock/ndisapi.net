// ----------------------------------------------
// <copyright file="NdisApi.DATA_LINK_LAYER_FILTER.cs" company="NT Kernel">
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
    /// Represents data link layer (OSI-7) filter level.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DataLinkFilter // DATA_LINK_LAYER_FILTER
    {
        internal FilterSelectFlags m_dwUnionSelector;
        internal Eth802_3Filter m_Eth8023Filter;

        /// <summary>
        /// Gets or sets the union selector.
        /// Must always be set to <see cref="FILTER_SELECT_FLAGS.ETH_802_3" />.
        /// </summary>
        public FilterSelectFlags Selector
        {
            get => m_dwUnionSelector;
            set => m_dwUnionSelector = value;
        }

        /// <summary>
        /// Gets or sets the eth8023 filter.
        /// </summary>
        public Eth802_3Filter Filter
        {
            get => m_Eth8023Filter;
            set => m_Eth8023Filter = value;
        }
    }
}