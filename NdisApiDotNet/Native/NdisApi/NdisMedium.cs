// ----------------------------------------------
// <copyright file="NdisApi.NDIS_MEDIUM.cs" company="NT Kernel">
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
    /// The NDIS medium type.
    /// </summary>
    public enum NdisMedium : uint
    {
        // Custom added default.
        Default = 0, // NdisMediumDefault

        Eth802_3 = 0, // NdisMedium802_3
        Eth802_5, // NdisMedium802_5
        Fddi, // NdisMediumFddi
        Wan, // NdisMediumWan
        LocalTalk, // NdisMediumLocalTalk
        Dix, // NdisMediumDix
        ArcnetRaw, // NdisMediumArcnetRaw
        Arcnet878_2, // NdisMediumArcnet878_2
        Atm, // NdisMediumAtm
        WirelessWan, // NdisMediumWirelessWan
        Irda, // NdisMediumIrda
        Bpc, // NdisMediumBpc
        CoWan, // NdisMediumCoWan
        NdisMedium1394, // NdisMedium1394
        InfiniBand, // NdisMediumInfiniBand
        Tunnel, // NdisMediumTunnel
        Native802_11, // NdisMediumNative802_11
        Loopback, // NdisMediumLoopback
        WiMAX, // NdisMediumWiMAX
        IP, // NdisMediumIP
        Max // NdisMediumMax
    }
}