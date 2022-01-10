// ----------------------------------------------
// <copyright file="NdisApi.FILTER_PACKET_ACTION.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    /// <summary>
    /// The filter packet action.
    /// </summary>
    public enum FilterAction : uint // FILTER_PACKET_ACTION
    {
        /// <summary>
        /// Pass (skip usermode) packet if it matches the filter.
        /// </summary>
        Pass = 0x00000001, // FILTER_PACKET_PASS

        /// <summary>
        /// Drop packet if it matches the filter.
        /// </summary>
        Drop = 0x00000002, // FILTER_PACKET_DROP

        /// <summary>
        /// Redirect packet to WinpkFilter client application
        /// </summary>
        Redirect = 0x00000003 // FILTER_PACKET_REDIRECT
    }
}