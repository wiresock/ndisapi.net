// ----------------------------------------------
// <copyright file="Program.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
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
    class Program
    {
        private static void Main()
        {
            var filter = NdisApi.Open();
            if (!filter.IsValid)
                throw new ApplicationException("Cannot load driver.");


            Console.WriteLine($"Loaded driver: {filter.GetVersion()}.");
            
            // Create and set event for the adapters.
            var waitHandlesCollection = new List<ManualResetEvent>();
            var tcpAdapters = new List<NetworkAdapter>();
            foreach (var networkAdapter in filter.GetNetworkAdapters())
            {
                if (networkAdapter.IsValid)
                {
                    var success = filter.SetAdapterMode(networkAdapter,
                                                            NdisApiDotNet.Native.NdisApi.MSTCP_FLAGS.MSTCP_FLAG_TUNNEL |
                                                            NdisApiDotNet.Native.NdisApi.MSTCP_FLAGS.MSTCP_FLAG_LOOPBACK_FILTER |
                                                            NdisApiDotNet.Native.NdisApi.MSTCP_FLAGS.MSTCP_FLAG_LOOPBACK_BLOCK);

                    var manualResetEvent = new ManualResetEvent(false);

                    success &= filter.SetPacketEvent(networkAdapter, manualResetEvent.SafeWaitHandle);

                    if (success)
                    {
                        Console.WriteLine($"Added {networkAdapter.FriendlyName}.");

                        waitHandlesCollection.Add(manualResetEvent);
                        tcpAdapters.Add(networkAdapter);
                    }
                }
            }

            var waitHandlesManualResetEvents = waitHandlesCollection.Cast<ManualResetEvent>().ToArray();
            var waitHandles = waitHandlesCollection.Cast<WaitHandle>().ToArray();

            var t1 = Task.Factory.StartNew(() => PassThruThread(filter, waitHandles, tcpAdapters.ToArray(), waitHandlesManualResetEvents));
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
        private static void PassThruThread(NdisApi filter, WaitHandle[] waitHandles, IReadOnlyList<NetworkAdapter> networkAdapters, IReadOnlyList<ManualResetEvent> waitHandlesManualResetEvents)
        {
            var ndisApiHelper = new NdisApiHelper();

            var ethPackets = ndisApiHelper.CreateEthMRequest();

            while (true)
            {
                var handle = WaitHandle.WaitAny(waitHandles);
                ethPackets.AdapterHandle = networkAdapters[handle].Handle;

                while (filter.ReadPackets(ref ethPackets))
                {
                    var packets = ethPackets.Packets;
                    for (int i = 0; i < ethPackets.PacketsCount; i++)
                    {
                        var ethPacket = packets[i].GetEthernetPacket(ndisApiHelper);
                        if (ethPacket.PayloadPacket is IPv4Packet iPv4Packet)
                        {
                            if (iPv4Packet.PayloadPacket is TcpPacket tcpPacket)
                            {
                                Console.WriteLine($"{iPv4Packet.SourceAddress}:{tcpPacket.SourcePort} -> {iPv4Packet.DestinationAddress}:{tcpPacket.DestinationPort}.");
                            }
                        }
                    }

                    filter.SendPackets(ref ethPackets);
                    ethPackets.PacketsCount = 0;
                }

                waitHandlesManualResetEvents[handle].Reset();
            }
        }
    }
}