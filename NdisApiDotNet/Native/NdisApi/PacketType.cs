// ----------------------------------------------
// <copyright file="NdisApi.NDIS_PACKET_TYPE.cs" company="NT Kernel">
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
    /// The NDIS packet type.
    /// </summary>
    [Flags]
    public enum PacketType : uint // NDIS_PACKET_TYPE
    {
        None = 0x0000, // NDIS_PACKET_TYPE_NONE
        Directed = 0x0001, // NDIS_PACKET_TYPE_DIRECTED
        Multicast = 0x0002, // NDIS_PACKET_TYPE_MULTICAST
        AllMulticast = 0x0004, // NDIS_PACKET_TYPE_ALL_MULTICAST
        Broadcast = 0x0008, // NDIS_PACKET_TYPE_BROADCAST
        SourceRouting = 0x0010, // NDIS_PACKET_TYPE_SOURCE_ROUTING
        Promiscuous = 0x0020, // NDIS_PACKET_TYPE_PROMISCUOUS
        SMT = 0x0040, // NDIS_PACKET_TYPE_SMT
        AllLocal = 0x0080, // NDIS_PACKET_TYPE_ALL_LOCAL
        Group = 0x1000, // NDIS_PACKET_TYPE_GROUP
        AllFunctional = 0x2000, // NDIS_PACKET_TYPE_ALL_FUNCTIONAL
        Functional = 0x4000, // NDIS_PACKET_TYPE_FUNCTIONAL
        MACFrame = 0x8000, // NDIS_PACKET_TYPE_MAC_FRAME
        Eth802_11RawData = 0x00010000, // NDIS_PACKET_TYPE_802_11_RAW_DATA
        Eth802_11DirectedMGMT = 0x00020000, // NDIS_PACKET_TYPE_802_11_DIRECTED_MGMT
        Eth802_11BroadcastMGMT = 0x00040000, // NDIS_PACKET_TYPE_802_11_BROADCAST_MGMT
        Eth802_11MulticastMGMT = 0x00080000, // NDIS_PACKET_TYPE_802_11_MULTICAST_MGMT
        Eth802_11AllMulticastMGMT = 0x00100000, // NDIS_PACKET_TYPE_802_11_ALL_MULTICAST_MGMT
        Eth802_11PromiscuousMGMT = 0x00200000, // NDIS_PACKET_TYPE_802_11_PROMISCUOUS_MGMT
        Eth802_11RawMGMT = 0x00400000, // NDIS_PACKET_TYPE_802_11_RAW_MGMT
        Eth802_11DirectedCTRL = 0x00800000, // NDIS_PACKET_TYPE_802_11_DIRECTED_CTRL
        Eth802_11BroadcastCTRL = 0x01000000, // NDIS_PACKET_TYPE_802_11_BROADCAST_CTRL
        Eth802_11PromiscuousCTRL = 0x02000000, // NDIS_PACKET_TYPE_802_11_PROMISCUOUS_CTRL
        Eth802_11All = Directed | // NDIS_PACKET_TYPE_ALL_802_11_FILTERS
                       Multicast |
                       AllMulticast |
                       Broadcast |
                       Promiscuous |
                       Eth802_11RawData |
                       Eth802_11DirectedMGMT |
                       Eth802_11BroadcastMGMT |
                       Eth802_11MulticastMGMT |
                       Eth802_11AllMulticastMGMT |
                       Eth802_11PromiscuousMGMT |
                       Eth802_11RawMGMT |
                       Eth802_11DirectedCTRL |
                       Eth802_11BroadcastCTRL |
                       Eth802_11PromiscuousCTRL
    }
}