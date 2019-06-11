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
    public static partial class NdisApi
    {
        /// <summary>
        /// The NDIS medium type.
        /// </summary>
        public enum NDIS_MEDIUM : uint
        {
            // Custom added default.
            NdisMediumDefault = 0,

            NdisMedium802_3 = 0,
            NdisMedium802_5,
            NdisMediumFddi,
            NdisMediumWan,
            NdisMediumLocalTalk,
            NdisMediumDix,
            NdisMediumArcnetRaw,
            NdisMediumArcnet878_2,
            NdisMediumAtm,
            NdisMediumWirelessWan,
            NdisMediumIrda,
            NdisMediumBpc,
            NdisMediumCoWan,
            NdisMedium1394,
            NdisMediumInfiniBand,
            NdisMediumTunnel,
            NdisMediumNative802_11,
            NdisMediumLoopback,
            NdisMediumWiMAX,
            NdisMediumIP,
            NdisMediumMax
        }
    }
}