// ----------------------------------------------
// <copyright file="NdisApi.IP_V4_FILTER_FIELDS.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
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
    public static partial class NdisApi
    {
        /// <summary>
        /// The IPv4 filter fields.
        /// </summary>
        [Flags]
        public enum IP_V4_FILTER_FIELDS : uint
        {
            /// <summary>
            /// The IPv4 filter source address.
            /// </summary>
            IP_V4_FILTER_SRC_ADDRESS = 0x00000001,

            /// <summary>
            /// The IPv4 filter destination address.
            /// </summary>
            IP_V4_FILTER_DEST_ADDRESS = 0x00000002,

            /// <summary>
            /// The IPv4 filter protocol.
            /// </summary>
            IP_V4_FILTER_PROTOCOL = 0x00000004
        }
    }
}