// ----------------------------------------------
// <copyright file="NdisApi.NDIS_PACKET_TYPE.cs" company="NT Kernel">
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
    /// The NDIS packet type as defined in ndis.h.
    /// </summary>
    [Flags]
    public enum NDIS_PACKET_TYPE : uint
    {
        NDIS_PACKET_TYPE_NONE = 0x0000,
        NDIS_PACKET_TYPE_DIRECTED = 0x0001,
        NDIS_PACKET_TYPE_MULTICAST = 0x0002,
        NDIS_PACKET_TYPE_ALL_MULTICAST = 0x0004,
        NDIS_PACKET_TYPE_BROADCAST = 0x0008,
        NDIS_PACKET_TYPE_SOURCE_ROUTING = 0x0010,
        NDIS_PACKET_TYPE_PROMISCUOUS = 0x0020,
        NDIS_PACKET_TYPE_SMT = 0x0040,
        NDIS_PACKET_TYPE_ALL_LOCAL = 0x0080,
        NDIS_PACKET_TYPE_GROUP = 0x1000,
        NDIS_PACKET_TYPE_ALL_FUNCTIONAL = 0x2000,
        NDIS_PACKET_TYPE_FUNCTIONAL = 0x4000,
        NDIS_PACKET_TYPE_MAC_FRAME = 0x8000,
        NDIS_PACKET_TYPE_802_11_RAW_DATA = 0x00010000,
        NDIS_PACKET_TYPE_802_11_DIRECTED_MGMT = 0x00020000,
        NDIS_PACKET_TYPE_802_11_BROADCAST_MGMT = 0x00040000,
        NDIS_PACKET_TYPE_802_11_MULTICAST_MGMT = 0x00080000,
        NDIS_PACKET_TYPE_802_11_ALL_MULTICAST_MGMT = 0x00100000,
        NDIS_PACKET_TYPE_802_11_PROMISCUOUS_MGMT = 0x00200000,
        NDIS_PACKET_TYPE_802_11_RAW_MGMT = 0x00400000,
        NDIS_PACKET_TYPE_802_11_DIRECTED_CTRL = 0x00800000,
        NDIS_PACKET_TYPE_802_11_BROADCAST_CTRL = 0x01000000,
        NDIS_PACKET_TYPE_802_11_PROMISCUOUS_CTRL = 0x02000000,

        NDIS_PACKET_TYPE_ALL_802_11_FILTERS = NDIS_PACKET_TYPE_DIRECTED |
                                              NDIS_PACKET_TYPE_MULTICAST |
                                              NDIS_PACKET_TYPE_ALL_MULTICAST |
                                              NDIS_PACKET_TYPE_BROADCAST |
                                              NDIS_PACKET_TYPE_PROMISCUOUS |
                                              NDIS_PACKET_TYPE_802_11_RAW_DATA |
                                              NDIS_PACKET_TYPE_802_11_DIRECTED_MGMT |
                                              NDIS_PACKET_TYPE_802_11_BROADCAST_MGMT |
                                              NDIS_PACKET_TYPE_802_11_MULTICAST_MGMT |
                                              NDIS_PACKET_TYPE_802_11_ALL_MULTICAST_MGMT |
                                              NDIS_PACKET_TYPE_802_11_PROMISCUOUS_MGMT |
                                              NDIS_PACKET_TYPE_802_11_RAW_MGMT |
                                              NDIS_PACKET_TYPE_802_11_DIRECTED_CTRL |
                                              NDIS_PACKET_TYPE_802_11_BROADCAST_CTRL |
                                              NDIS_PACKET_TYPE_802_11_PROMISCUOUS_CTRL
    }
}