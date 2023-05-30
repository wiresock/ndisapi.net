// ----------------------------------------------
// <copyright file="NdisApi.ETH_M_REQUEST.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// Used for passing the blocks of packets to and from the driver.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ETH_M_REQUEST
    {
        /// <summary>
        /// The adapter handle.
        /// </summary>
        public IntPtr hAdapterHandle;

        /// <summary>
        /// The total number of packets that can be set.
        /// </summary>
        public uint dwPacketsNumber;

        /// <summary>
        /// The number of packets that were successfully returned.
        /// </summary>
        public uint dwPacketsSuccess;

        /// <summary>
        /// The first <see cref="NDISRD_ETH_Packet" />.
        /// </summary>
        public NDISRD_ETH_Packet EthPacket; // This is an array of NDISRD_ETH_Packet, but this cannot be declared directly as it's a variable width.

        /// <summary>
        /// Gets the <see cref="NDISRD_ETH_Packet" />s.
        /// </summary>
        public NDISRD_ETH_Packet* Packets => (NDISRD_ETH_Packet*) Unsafe.AsPointer(ref EthPacket);

        /// <summary>
        /// Gets or sets the <see cref="NDISRD_ETH_Packet" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns><see cref="NDISRD_ETH_Packet" />.</returns>
        public NDISRD_ETH_Packet this[int index]
        {
            get => Packets[index];
            set => Packets[index] = value;
        }

        /// <summary>
        /// Gets the <see cref="NDISRD_ETH_Packet" />s.
        /// </summary>
        /// <param name="packets">The number of packets to retrieve, defaults to <see cref="dwPacketsSuccess" />.</param>
        /// <returns><see cref="NDISRD_ETH_Packet" />s.</returns>
        public NDISRD_ETH_Packet[] GetPackets(uint packets = 0)
        {
            uint packetSize = packets == 0 ? dwPacketsSuccess : packets;

            var ndisrdEthPackets = new NDISRD_ETH_Packet[packetSize];
            NDISRD_ETH_Packet* packetsPtr = Packets;

            for (int i = 0; i < packetSize; i++)
            {
                ndisrdEthPackets[i] = packetsPtr[i];
            }

            return ndisrdEthPackets;
        }

        /// <summary>
        /// Sets the <see cref="NDISRD_ETH_Packet" />s.
        /// </summary>
        /// <remarks>This should never exceed the allocated <see cref="GCHandle" />'s packet size.</remarks>
        /// <param name="packets">The packets.</param>
        public void SetPackets(NDISRD_ETH_Packet[] packets)
        {
            NDISRD_ETH_Packet* packetsPtr = Packets;

            for (int i = 0; i < packets.Length; i++)
            {
                packetsPtr[i] = packets[i];
            }
        }

        /// <summary>
        /// The size of <see cref="ETH_M_REQUEST" /> without the <see cref="Packets" />.
        /// </summary>
        public static int SizeOfHeader = Marshal.SizeOf<ETH_M_REQUEST>() - NDISRD_ETH_Packet.Size;

        /// <summary>
        /// The offset of <see cref="Packets" />.
        /// </summary>
        public static IntPtr PacketsOffset = Marshal.OffsetOf(typeof(ETH_M_REQUEST), nameof(EthPacket));
    }
}