// ----------------------------------------------
// <copyright file="NdisApi.TCPUDP_FILTER_FIELDS.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// The used TCP/UDP filter fields.
    /// </summary>
    [Flags]
    public enum TCPUDP_FILTER_FIELDS
    {
        /// <summary>
        /// The TCP/UDP source port.
        /// </summary>
        TCPUDP_SRC_PORT = 0x00000001,

        /// <summary>
        /// The TCP/UDP destination port.
        /// </summary>
        TCPUDP_DEST_PORT = 0x00000002,

        /// <summary>
        /// The TCP flags.
        /// </summary>
        TCPUDP_TCP_FLAGS = 0x00000004
    }
}