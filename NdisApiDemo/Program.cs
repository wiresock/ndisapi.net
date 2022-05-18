// ----------------------------------------------
// <copyright file="Program.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NdisApiDotNet;
using NdisApiDotNetPacketDotNet.Extensions;
using PacketDotNet;

namespace NdisApiDemo
{
    public class Program
    {
        private static void Main()
        {
            var filter = NdisApi.Open();

            Console.WriteLine($"Version: {filter.GetVersion()}.");
            Console.WriteLine($"Loaded driver: {filter.IsDriverLoaded()}.");

            // Create and set event for the adapters.
            var waitHandlesCollection = new List<AutoResetEvent>();

            foreach (var networkAdapter in filter.GetNetworkAdapters())
            {
                if (networkAdapter.IsValid)
                {
                    var success = filter.SetAdapterMode(networkAdapter,
                                                        NdisApiDotNet.Native.NdisApi.MSTCP_FLAGS.MSTCP_FLAG_TUNNEL |
                                                        NdisApiDotNet.Native.NdisApi.MSTCP_FLAGS.MSTCP_FLAG_LOOPBACK_FILTER |
                                                        NdisApiDotNet.Native.NdisApi.MSTCP_FLAGS.MSTCP_FLAG_LOOPBACK_BLOCK);

                    var resetEvent = new AutoResetEvent(false);

                    success &= filter.SetPacketEvent(networkAdapter, resetEvent.SafeWaitHandle);

                    if (success)
                    {
                        Console.WriteLine($"Added {networkAdapter.FriendlyName} - {networkAdapter.Handle}.");

                        waitHandlesCollection.Add(resetEvent);
                    }
                }
            }

            var waitHandlesAutoResetEvents = waitHandlesCollection.Cast<AutoResetEvent>().ToArray();
            var waitHandles = waitHandlesCollection.Cast<WaitHandle>().ToArray();

            var t1 = Task.Factory.StartNew(() => PassThruUnsortedThread(filter, waitHandles, waitHandlesAutoResetEvents));
            //var t1 = Task.Factory.StartNew(() => PassThruThread(filter, waitHandles, filter.GetNetworkAdapters().ToArray(), waitHandlesAutoResetEvents));
            Task.WaitAll(t1);

            Console.Read();
        }

        /// <summary>
        /// Starts a pass thru thread.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="waitHandles">The wait handles.</param>
        /// <param name="networkAdapters">The network adapters.</param>
        /// <param name="waitHandlesManualResetEvents">The wait handles manual reset events.</param>
        private static unsafe void PassThruThread(NdisApi filter, WaitHandle[] waitHandles, IReadOnlyList<NetworkAdapter> networkAdapters, IReadOnlyList<AutoResetEvent> waitHandlesManualResetEvents)
        {
            var ethPackets = filter.CreateEthMRequest(32);

            while (true)
            {
                var handle = WaitHandle.WaitAny(waitHandles);
                ethPackets->hAdapterHandle = networkAdapters[handle].Handle;

                while (filter.ReadPackets(ethPackets))
                {
                    var packets = ethPackets->Packets;
                    for (int i = 0; i < ethPackets->dwPacketsSuccess; i++)
                    {
                        var ethPacket = packets[i].GetEthernetPacket(filter);
                        if (ethPacket.PayloadPacket is IPv4Packet iPv4Packet)
                        {
                            if (iPv4Packet.PayloadPacket is TcpPacket tcpPacket)
                            {
                                //Console.WriteLine($"{iPv4Packet.SourceAddress}:{tcpPacket.SourcePort} -> {iPv4Packet.DestinationAddress}:{tcpPacket.DestinationPort}.");
                            }
                        }
                    }

                    filter.SendPackets(ethPackets);
                    ethPackets->dwPacketsSuccess = 0;
                }
            }
        }

        /// <summary>
        /// Starts a pass thru thread.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="waitHandles">The wait handles.</param>
        /// <param name="waitHandlesManualResetEvents">The wait handles manual reset events.</param>
        private static unsafe void PassThruUnsortedThread
        (
            NdisApi filter,
            WaitHandle[] waitHandles,
            IReadOnlyList<AutoResetEvent> waitHandlesManualResetEvents)
        {
            const int bufferSize = 32;

            var buffers = new IntPtr[bufferSize];
            for (int i = 0; i < buffers.Length; i++)
            {
                buffers[i] = (IntPtr)filter.CreateIntermediateBuffer();
            }

            while (true)
            {
                WaitHandle.WaitAny(waitHandles);
                uint packetsSuccess = 0;

                while (filter.ReadPacketsUnsorted(buffers, bufferSize, ref packetsSuccess))
                {
                    for (int i = 0; i < packetsSuccess; i++)
                    {
                        var ethPacket = buffers[i].GetEthernetPacket(filter);
                        if (ethPacket.PayloadPacket is IPv4Packet iPv4Packet)
                        {
                            if (iPv4Packet.PayloadPacket is TcpPacket tcpPacket)
                            {
                                //Console.WriteLine($"{iPv4Packet.SourceAddress}:{tcpPacket.SourcePort} -> {iPv4Packet.DestinationAddress}:{tcpPacket.DestinationPort}.");
                            }
                        }
                    }

                    if (packetsSuccess > 0)
                    {
                        filter.SendPacketsUnsorted(buffers, packetsSuccess, out var numPacketsSuccess);
                    }
                }
            }
        }
    }
}