// ----------------------------------------------
// <copyright file="IntermediateBufferExtensions.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;
using NdisApiDotNet;
using PacketDotNet;
using PacketDotNet.Utils;

namespace NdisApiDotNetPacketDotNet.Extensions;

public static class IntermediateBufferExtensions
{
    /// <summary>
    /// Gets the ethernet packet.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="ndisApi">The NDIS API that created the packet.</param>
    /// <returns><see cref="EthernetPacket" /> if possible; <c>null</c> otherwise.</returns>
    public static EthernetPacket GetEthernetPacket(this IntPtr buffer, NdisApi ndisApi)
    {
        try
        {
            byte[] pinnedArray = ndisApi.GetPinnedArray(buffer);
            return new EthernetPacket(new ByteArraySegment(pinnedArray, (int)NdisApiDotNet.Native.NdisApi.INTERMEDIATE_BUFFER.BufferOffset, ndisApi.MaxPacketSize));
        }
        catch
        {
            // This can occur when you've unpinned the array.
            return null;
        }
    }
}