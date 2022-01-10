// ----------------------------------------------
// <copyright file="NdisApi.ETH_M_REQUEST_U.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    /// <summary>
    /// Used for passing the blocks of packets to and from the driver.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct EthMRequestUnsafe : IDisposable // ETH_M_REQUEST_U
    {
        internal IntPtr hAdapterHandle;
        internal uint dwPacketsNumber;
        internal uint dwPacketsSuccess;
        internal EthPacket* _ethPacket;

        /// <summary>
        /// Gets or sets the adapter handle.
        /// </summary>
        public IntPtr AdapterHandle
        {
            get
            {
                return hAdapterHandle;
            }

            set
            {
                hAdapterHandle = value;
            }
        }

        /// <summary>
        /// Gets or sets the total number of packets that can be set.
        /// </summary>
        public uint PacketsLength
        {
            get
            {
                return dwPacketsNumber;
            }

            set
            {
                dwPacketsNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of packets that are actually set.
        /// </summary>
        public uint PacketsCount
        {
            get
            {
                return dwPacketsSuccess;
            }

            set
            {
                dwPacketsSuccess = value;
            }
        }

        /// <summary>
        /// Gets the packets.
        /// </summary>
        /// <param name="packets">The number of packets to retrieve, defaults to <see cref="dwPacketsSuccess" />.</param>
        /// <returns>NDISRD_ETH_Packet[].</returns>
        public EthPacket[] GetPackets(uint packets = 0)
        {
            uint packetSize = packets == 0 ? dwPacketsSuccess : packets;

            EthPacket[] ndisrdEthPackets = new EthPacket[packetSize];
            GCHandle pinnedNdisrdEthPackets = GCHandle.Alloc(ndisrdEthPackets, GCHandleType.Pinned);

            long sizeToCopy = NdisApi.NdisRdEthPacketSize * packetSize;
            fixed (EthMRequestUnsafe* a = &this)
            {
                NdisApi.MemoryCopy((void*)((IntPtr)a + NdisApi.EthMRequestUEthPacketOffset), (void*)pinnedNdisrdEthPackets.AddrOfPinnedObject(), sizeToCopy);
            }

            pinnedNdisrdEthPackets.Free();

            return ndisrdEthPackets;
        }

        /// <summary>
        /// Sets the packets.
        /// </summary>
        /// <remarks>
        /// This should never exceed the allocated GCHandle's packet size.
        /// </remarks>
        /// <param name="packets">The packets.</param>
        public void SetPackets(EthPacket[] packets)
        {
            GCHandle pinnedNdisrdEthPackets = GCHandle.Alloc(packets, GCHandleType.Pinned);

            int sizeToCopy = NdisApi.NdisRdEthPacketSize * packets.Length;
            fixed (EthMRequestUnsafe* a = &this)
            {
                NdisApi.MemoryCopy((void*)pinnedNdisrdEthPackets.AddrOfPinnedObject(), (void*)((IntPtr)a + NdisApi.EthMRequestUEthPacketOffset), sizeToCopy);
            }

            pinnedNdisrdEthPackets.Free();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            EthPacket[] packets = GetPackets(Math.Max(dwPacketsNumber, dwPacketsSuccess));
            for (int i = 0; i < packets.Length; i++)
            {
                if (packets[i].Buffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(packets[i].Buffer);
            }
        }
    }
}