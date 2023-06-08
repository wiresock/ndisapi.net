# NDISAPI.NET

NDISAPI.NET is a .NET library that provides a C# interface for interacting with the [Windows Packet Filter](https://www.ntkernel.com/windows-packet-filter/) driver. This library is part of the Windows Packet Filter project, a high-performance, lightweight packet filtering framework for Windows that allows developers to efficiently manipulate network packets.

## Features

- **Enumerate Network Adapters**: Enumerate all network interfaces on your system.
- **Query and Set Network Adapter Properties**: Query and modify the properties of network adapters.
- **Capture and Analyze Packets**: Capture network packets and analyze their content.
- **Filter and Modify Packets**: Filter network packets based on various parameters and modify packets.
- **Send Raw Packets**: Send raw packets, a powerful tool for network testing and simulation.

## Demo Code

The following demo code demonstrates how to use NDISAPI.NET to interact with the Windows Packet Filter driver, enumerate network adapters, set an adapter to packet filtering mode, and read packets from the network adapter.

```csharp
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
            Console.WriteLine($"Installed driver: {filter.IsDriverInstalled()}.");

            // Create and set event for the adapters.
            var waitHandlesCollection = new List<AutoResetEvent>();

            foreach (NetworkAdapter networkAdapter in filter.GetNetworkAdapters())
            {
                if (networkAdapter.IsValid)
                {
                    bool success = filter.SetAdapterMode(networkAdapter,
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

            AutoResetEvent[] waitHandlesAutoResetEvents = waitHandlesCollection.Cast<AutoResetEvent>().ToArray();
            WaitHandle[] waitHandles = waitHandlesCollection.Cast<WaitHandle>().ToArray();

            Task t1 = Task.Factory.StartNew(() => PassThruUnsortedThread(filter, waitHandles, waitHandlesAutoResetEvents));
            Task.WaitAll(t1);

            Console.Read();
        }

        // ... rest of the code ...
    }
}
```

Please refer to the project's [GitHub repository](https://github.com/wiresock/ndisapi.net) for more information and the complete code.
