// ----------------------------------------------
// <copyright file="Program.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using NdisApiDotNet;
using NdisApiDotNet.Native;
using NdisApiDotNetPacketDotNet.Extensions;
using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NdisApiDemo
{
    internal class Program
    {
        private static async Task Main()
        {
            NdisAPI filter = NdisAPI.Open();
            if (!filter.IsValid) throw new ApplicationException("Cannot load driver.");

            Console.WriteLine($"Loaded driver: {filter.GetVersion()}.");

            StaticFilterTable filterTable = new StaticFilterTable(0);
            filterTable.Add(FilterAction.Redirect, new TCPUDPFilter(FilterFlags.Source, 6672));
            filterTable.Add(FilterAction.Redirect, new TCPUDPFilter(FilterFlags.Destination, 6672));
            StaticFilter f = filterTable.Add(FilterAction.Pass);

            /*Console.WriteLine(filterTable.Contains(f));
            Console.WriteLine(filterTable.Remove(f));
            Console.WriteLine(filterTable.Contains(f));*/

            filter.SetPacketFilterTable(filterTable);

            // Create and set event for the adapters.
            List<ManualResetEvent> waitHandlesCollection = new List<ManualResetEvent>();
            List<NetworkAdapter> tcpAdapters = new List<NetworkAdapter>();
            foreach (NetworkAdapter networkAdapter in filter.GetNetworkAdapters())
            {
                if (networkAdapter.IsValid)
                {
                    bool success = filter.SetAdapterMode(networkAdapter, MSTCPFlags.Tunnel | MSTCPFlags.FilterLoopback | MSTCPFlags.BlockLoopback);

                    ManualResetEvent manualResetEvent = new ManualResetEvent(false);

                    success &= filter.SetPacketEvent(networkAdapter, manualResetEvent.SafeWaitHandle);

                    if (success)
                    {
                        Console.WriteLine($"Added {networkAdapter.FriendlyName}.");

                        waitHandlesCollection.Add(manualResetEvent);
                        tcpAdapters.Add(networkAdapter);
                    }
                }
            }

            ManualResetEvent[] waitHandlesManualResetEvents = waitHandlesCollection.Cast<ManualResetEvent>().ToArray();
            WaitHandle[] waitHandles = waitHandlesCollection.Cast<WaitHandle>().ToArray();

            await Task.Run(() => PassThruThread(filter, waitHandles, tcpAdapters.ToArray(), waitHandlesManualResetEvents));

            Console.WriteLine("Exited callback thread");
            Console.ReadLine();
        }

        /// <summary>
        /// Starts a pass thru thread.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="waitHandles">The wait handles.</param>
        /// <param name="networkAdapters">The network adapters.</param>
        /// <param name="waitHandlesManualResetEvents">The wait handles manual reset events.</param>
        private static void PassThruThread(NdisAPI filter, WaitHandle[] waitHandles, IReadOnlyList<NetworkAdapter> networkAdapters, IReadOnlyList<ManualResetEvent> waitHandlesManualResetEvents)
        {
            NdisAPIHelper ndisApiHelper = new NdisAPIHelper();

            EthMRequest ethPackets = ndisApiHelper.CreateEthMRequest();

            while (true)
            {
                int handle = WaitHandle.WaitAny(waitHandles);
                ethPackets.AdapterHandle = networkAdapters[handle].Handle;

                while (filter.ReadPackets(ref ethPackets))
                {
                    EthPacket[] packets = ethPackets.Packets;
                    for (int i = 0; i < ethPackets.PacketsCount; i++)
                    {
                        EthernetPacket ethPacket = packets[i].GetEthernetPacket(ndisApiHelper);
                        if (ethPacket.PayloadPacket is IPv4Packet iPv4Packet)
                        {
                            if (iPv4Packet.PayloadPacket is TcpPacket tcpPacket)
                            {
                                Console.WriteLine($"[TCP] {iPv4Packet.SourceAddress}:{tcpPacket.SourcePort} -> {iPv4Packet.DestinationAddress}:{tcpPacket.DestinationPort}.");
                            }
                            else if (iPv4Packet.PayloadPacket is UdpPacket udpPacket)
                            {
                                Console.WriteLine($"[UDP] {iPv4Packet.SourceAddress}:{udpPacket.SourcePort} -> {iPv4Packet.DestinationAddress}:{udpPacket.DestinationPort} ({udpPacket.PayloadData.Length}).");
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