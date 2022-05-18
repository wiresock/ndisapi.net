// ----------------------------------------------
// <copyright file="NdisApi.NDIS_FLAGS.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// The NDIS flags as defined in ndis.h.
    /// </summary>
    [Flags]
    public enum NDIS_FLAGS : uint
    {
        NDIS_FLAGS_PROTOCOL_ID_MASK = 0x0000000F,
        NDIS_FLAGS_MULTICAST_PACKET = 0x00000010,
        NDIS_FLAGS_RESERVED2 = 0x00000020,
        NDIS_FLAGS_RESERVED3 = 0x00000040,
        NDIS_FLAGS_DONT_LOOPBACK = 0x00000080,
        NDIS_FLAGS_IS_LOOPBACK_PACKET = 0x00000100,
        NDIS_FLAGS_LOOPBACK_ONLY = 0x00000200,
        NDIS_FLAGS_RESERVED4 = 0x00000400,
        NDIS_FLAGS_DOUBLE_BUFFERED = 0x00000800,
        NDIS_FLAGS_SENT_AT_DPC = 0x00001000,
        NDIS_FLAGS_USES_SG_BUFFER_LIST = 0x00002000,
        NDIS_FLAGS_USES_ORIGINAL_PACKET = 0x00004000,
        NDIS_FLAGS_PADDED = 0x00010000,
        NDIS_FLAGS_XLATE_AT_TOP = 0x00020000
    }
}