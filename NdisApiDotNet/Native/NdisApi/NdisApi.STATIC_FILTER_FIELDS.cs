// ----------------------------------------------
// <copyright file="NdisApi.STATIC_FILTER_FIELDS.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// The static filter fields used.
    /// </summary>
    [Flags]
    public enum STATIC_FILTER_FIELDS : uint
    {
        /// <summary>
        /// Match packet against data link layer filter.
        /// </summary>
        DATA_LINK_LAYER_VALID = 0x00000001,

        /// <summary>
        /// Match packet against network layer filter.
        /// </summary>
        NETWORK_LAYER_VALID = 0x00000002,

        /// <summary>
        /// Match packet against transport layer filter.
        /// </summary>
        TRANSPORT_LAYER_VALID = 0x00000004
    }
}