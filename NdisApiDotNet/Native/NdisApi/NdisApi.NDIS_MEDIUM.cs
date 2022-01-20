// ----------------------------------------------
// <copyright file="NdisApi.NDIS_MEDIUM.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        /// <summary>
        /// The NDIS medium type as defined in ntddndis.h.
        /// </summary>
        public enum NDIS_MEDIUM : uint
        {
            // Added to make it possible to specify a default.
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