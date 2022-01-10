// ----------------------------------------------
// <copyright file="NdisApi.NDIS_FLAGS.cs" company="NT Kernel">
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
    /// The NDIS flags.
    /// </summary>
    [Flags]
    public enum NdisFlags : uint // NDIS_FLAGS
    {
        ProtocolIDMask = 0x0000000F, // NDIS_FLAGS_PROTOCOL_ID_MASK
        MulticastPacket = 0x00000010, // NDIS_FLAGS_MULTICAST_PACKET
        Reserved2 = 0x00000020, // NDIS_FLAGS_RESERVED2
        Reserved3 = 0x00000040, // NDIS_FLAGS_RESERVED3
        DontLoopback = 0x00000080, // NDIS_FLAGS_DONT_LOOPBACK
        IsLoopbackPacket = 0x00000100, // NDIS_FLAGS_IS_LOOPBACK_PACKET
        LoopbackOnly = 0x00000200, // NDIS_FLAGS_LOOPBACK_ONLY
        Reserved4 = 0x00000400, // NDIS_FLAGS_RESERVED4
        DoubleBuffered = 0x00000800, // NDIS_FLAGS_DOUBLE_BUFFERED
        NDIS_FLAGS_SENT_AT_DPC = 0x00001000, // NDIS_FLAGS_SENT_AT_DPC
        NDIS_FLAGS_USES_SG_BUFFER_LIST = 0x00002000, // NDIS_FLAGS_USES_SG_BUFFER_LIST
        NDIS_FLAGS_USES_ORIGINAL_PACKET = 0x00004000, // NDIS_FLAGS_USES_ORIGINAL_PACKET
        Padded = 0x00010000, // NDIS_FLAGS_PADDED
        NDIS_FLAGS_XLATE_AT_TOP = 0x00020000 // NDIS_FLAGS_XLATE_AT_TOP
    }
}