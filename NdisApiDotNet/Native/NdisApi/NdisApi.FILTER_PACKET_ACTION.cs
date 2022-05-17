// ----------------------------------------------
// <copyright file="NdisApi.FILTER_PACKET_ACTION.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// The filter packet action.
    /// </summary>
    public enum FILTER_PACKET_ACTION : uint
    {
        /// <summary>
        /// Pass packet if it matches the filter.
        /// </summary>
        FILTER_PACKET_PASS = 0x00000001,

        /// <summary>
        /// Drop packet if it matches the filter.
        /// </summary>
        FILTER_PACKET_DROP = 0x00000002,

        /// <summary>
        /// Redirect packet to the client application.
        /// </summary>
        FILTER_PACKET_REDIRECT = 0x00000003
    }
}