// ----------------------------------------------
// <copyright file="NdisApi.DATA_LINK_LAYER_FILTER.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
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
        /// Represents data link layer (OSI-7) filter level.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DATA_LINK_LAYER_FILTER
        {
            /// <summary>
            /// The union selector, this must always be set to <see cref="FILTER_SELECT_FLAGS.ETH_802_3" />.
            /// </summary>
            public FILTER_SELECT_FLAGS m_dwUnionSelector;

            /// <summary>
            /// The Ethernet 802.3 filter.
            /// </summary>
            public ETH_802_3_FILTER m_Eth8023Filter;
        }
    }
}