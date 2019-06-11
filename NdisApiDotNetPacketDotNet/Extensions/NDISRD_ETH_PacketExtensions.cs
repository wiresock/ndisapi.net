// ----------------------------------------------
// <copyright file="NDISRD_ETH_PacketExtensions.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using NdisApiDotNet;
using PacketDotNet;
using PacketDotNet.Utils;
using NdisApi = NdisApiDotNet.Native.NdisApi;

namespace NdisApiDotNetPacketDotNet.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class NDISRD_ETH_PacketExtensions
    {
        /// <summary>
        /// Gets the ethernet packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="ndisApiHelper">The optional Ndis API helper. Should be passed for best performance.</param>
        /// <returns><see cref="EthernetPacket" /> if possible; <c>null</c> otherwise.</returns>
        public static EthernetPacket GetEthernetPacket(this NdisApi.NDISRD_ETH_Packet packet, NdisApiHelper ndisApiHelper = null)
        {
            if (ndisApiHelper != null)
            {
                try
                {
                    var pinnedArray = ndisApiHelper.GetPinnedArray(packet.Buffer);
                    return new EthernetPacket(new ByteArraySegment(pinnedArray, NdisApi.IntermediateBufferBufferOffset, NdisApi.MAX_ETHER_FRAME));
                }
                catch
                {
                    // This can occur when you've unpinned the array.
                    // In this case, return null, as the code below will be invalid as well.
                    return null;
                }
            }

            return new EthernetPacket(new ByteArraySegment(packet.GetIntermediateBuffer().Buffer));
        }
    }
}