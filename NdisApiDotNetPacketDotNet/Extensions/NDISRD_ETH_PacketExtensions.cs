// ----------------------------------------------
// <copyright file="NDISRD_ETH_PacketExtensions.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using NdisApiDotNet;
using PacketDotNet;
using PacketDotNet.Utils;

// ReSharper disable InconsistentNaming

namespace NdisApiDotNetPacketDotNet.Extensions;

public static class NDISRD_ETH_PacketExtensions
{
    /// <summary>
    /// Gets the ethernet packet.
    /// </summary>
    /// <param name="packet">The packet.</param>
    /// <param name="ndisApi">The NDIS API that created the packet.</param>
    /// <returns><see cref="EthernetPacket" /> if accessible; <c>null</c> otherwise.</returns>
    public static EthernetPacket GetEthernetPacket(this NdisApiDotNet.Native.NdisApi.NDISRD_ETH_Packet packet, NdisApi ndisApi)
    {
        try
        {
            var pinnedArray = ndisApi.GetPinnedArray(packet.Buffer);
            return new EthernetPacket(new ByteArraySegment(pinnedArray, (int) NdisApiDotNet.Native.NdisApi.INTERMEDIATE_BUFFER.BufferOffset, ndisApi.MaxPacketSize));
        }
        catch
        {
            // This can occur when you've unpinned the array.
            return null;
        }
    }
}