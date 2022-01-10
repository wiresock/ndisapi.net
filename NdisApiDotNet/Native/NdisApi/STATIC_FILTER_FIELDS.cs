// ----------------------------------------------
// <copyright file="NdisApi.STATIC_FILTER_FIELDS.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    /// <summary>
    /// The static filter fields used.
    /// </summary>
    [Flags]
    public enum FilterFields : uint // STATIC_FILTER_FIELDS
    {
        None = 0,

        /// <summary>
        /// Match packet against data link layer filter.
        /// </summary>
        DataLink = 0x00000001, // DATA_LINK_LAYER_VALID

        /// <summary>
        /// Match packet against network layer filter.
        /// </summary>
        Network = 0x00000002, // NETWORK_LAYER_VALID

        /// <summary>
        /// Match packet against transport layer filter.
        /// </summary>
        Transport = 0x00000004 // TRANSPORT_LAYER_VALID
    }
}