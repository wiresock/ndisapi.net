// ----------------------------------------------
// <copyright file="NdisApiHelper.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Runtime.InteropServices;
using NdisApiDotNet.Native;

namespace NdisApiDotNet
{
    public class NdisApiHelper : IDisposable
    {
        private readonly PinnedManagedArrayAllocator<byte> _pinnedManagedArrayAllocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NdisApiHelper"/> class.
        /// </summary>
        public NdisApiHelper()
        {
            _pinnedManagedArrayAllocator = new PinnedManagedArrayAllocator<byte>();
            GC.KeepAlive(_pinnedManagedArrayAllocator);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _pinnedManagedArrayAllocator?.Dispose();
        }

        /// <summary>
        /// Clones the specified request.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void CloneEthRequest(ref Native.NdisApi.ETH_REQUEST source, ref Native.NdisApi.ETH_REQUEST destination)
        {
            destination.hAdapterHandle = source.hAdapterHandle;

            Kernel32.CopyMemory(destination._ethPacket._buffer, source._ethPacket._buffer, (uint) Native.NdisApi.IntermediateBufferSize);
        }

        /// <summary>
        /// Clones the specified request.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void CloneEthMRequest(ref Native.NdisApi.ETH_M_REQUEST source, ref Native.NdisApi.ETH_M_REQUEST destination)
        {
            destination.hAdapterHandle = source.hAdapterHandle;
            destination.dwPacketsSuccess = source.dwPacketsSuccess;
            destination.dwPacketsNumber = source.dwPacketsNumber;

            for (int i = 0; i < source.dwPacketsSuccess; i++)
                Kernel32.CopyMemory(destination._ethPacket[i]._buffer, source._ethPacket[i]._buffer, (uint) Native.NdisApi.IntermediateBufferSize);
        }

        /// <summary>
        /// Clones the specified request.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public unsafe void CloneUnsafeEthMRequest(ref Native.NdisApi.ETH_M_REQUEST_U* source, ref Native.NdisApi.ETH_M_REQUEST_U* destination)
        {
            destination->hAdapterHandle = source->hAdapterHandle;
            destination->dwPacketsSuccess = source->dwPacketsSuccess;
            destination->dwPacketsNumber = source->dwPacketsNumber;

            var requestPackets = source->GetPackets();
            var nextPackets = destination->GetPackets();

            for (int i = 0; i < source->dwPacketsSuccess; i++)
                Kernel32.CopyMemory(nextPackets[i]._buffer, requestPackets[i]._buffer, (uint) Native.NdisApi.IntermediateBufferSize);
        }

        /// <summary>
        /// Clones the specified <see cref="source" />.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns><see cref="Native.NdisApi.ETH_REQUEST" />.</returns>
        public Native.NdisApi.ETH_REQUEST CloneEthRequest(ref Native.NdisApi.ETH_REQUEST source)
        {
            var destination = CreateEthRequest();
            CloneEthRequest(ref source, ref destination);
            return destination;
        }

        /// <summary>
        /// Clones the specified <see cref="source" />.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns><see cref="Native.NdisApi.ETH_M_REQUEST" />.</returns>
        public Native.NdisApi.ETH_M_REQUEST CloneEthMRequest(ref Native.NdisApi.ETH_M_REQUEST source)
        {
            var destination = CreateEthMRequest();
            CloneEthMRequest(ref source, ref destination);
            return destination;
        }

        /// <summary>
        /// Clones the specified <see cref="source" />.
        /// </summary>
        /// <param name="source">The request.</param>
        /// <returns><see cref="Native.NdisApi.ETH_M_REQUEST_U" />.</returns>
        public unsafe Native.NdisApi.ETH_M_REQUEST_U* CloneUnsafeEthMRequest(ref Native.NdisApi.ETH_M_REQUEST_U* source)
        {
            var destination = CreateUnsafeEthMRequest(source->dwPacketsNumber);
            CloneUnsafeEthMRequest(ref source, ref destination);
            return destination;
        }

        /// <summary>
        /// Zeroes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void ZeroEthRequest(ref Native.NdisApi.ETH_REQUEST request)
        {
            request.hAdapterHandle = IntPtr.Zero;
            Kernel32.ZeroMemory(request._ethPacket._buffer, Native.NdisApi.IntermediateBufferSize);
        }

        /// <summary>
        /// Zeroes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void ZeroEthMRequest(ref Native.NdisApi.ETH_M_REQUEST request)
        {
            request.hAdapterHandle = IntPtr.Zero;

            for (int i = 0; i < request.dwPacketsNumber; i++)
                Kernel32.ZeroMemory(request._ethPacket[i]._buffer, Native.NdisApi.IntermediateBufferSize);
        }

        /// <summary>
        /// Zeroes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public unsafe void ZeroUnsafeEthMRequest(ref Native.NdisApi.ETH_M_REQUEST_U* request)
        {
            request->hAdapterHandle = IntPtr.Zero;
            var packets = request->GetPackets();

            for (int i = 0; i < request->dwPacketsNumber; i++)
                Kernel32.ZeroMemory(packets[i]._buffer, Native.NdisApi.IntermediateBufferSize);
        }

        /// <summary>
        /// Creates a new <see cref="Native.NdisApi.ETH_REQUEST" />.
        /// </summary>
        /// <remarks>The adapter handle still needs to be set.</remarks>
        /// <returns>NativeMethods.ETH_REQUEST.</returns>
        public Native.NdisApi.ETH_REQUEST CreateEthRequest()
        {
            return new Native.NdisApi.ETH_REQUEST
            {
                _ethPacket = 
                {
                    _buffer = _pinnedManagedArrayAllocator.AllocateArray(Native.NdisApi.IntermediateBufferSize)
                }
            };
        }

        /// <summary>
        /// Creates a new <see cref="Native.NdisApi.ETH_M_REQUEST" />.
        /// </summary>
        /// <remarks>The adapter handle still needs to be set.</remarks>
        /// <returns>NativeMethods.ETH_M_REQUEST_256.</returns>
        public Native.NdisApi.ETH_M_REQUEST CreateEthMRequest()
        {
            var ethPackets = new Native.NdisApi.ETH_M_REQUEST
            {
                _ethPacket = new Native.NdisApi.NDISRD_ETH_Packet[Native.NdisApi.EthMRequestEthPacketSize],
                dwPacketsNumber = Native.NdisApi.EthMRequestEthPacketSize
            };

            for (var i = 0; i < Native.NdisApi.EthMRequestEthPacketSize; i++)
                ethPackets._ethPacket[i]._buffer = _pinnedManagedArrayAllocator.AllocateArray(Native.NdisApi.IntermediateBufferSize);

            return ethPackets;
        }       
        
        /// <summary>
        /// Creates a new <see cref="Native.NdisApi.ETH_M_REQUEST_U" />.
        /// </summary>
        /// <param name="packetSize">Size of the packet.</param>
        /// <returns>NativeMethods.ETH_M_REQUEST_U.</returns>
        /// <remarks>The adapter handle still needs to be set.</remarks>
        public unsafe Native.NdisApi.ETH_M_REQUEST_U* CreateUnsafeEthMRequest(uint packetSize)
        {
            var totalNdisPacketSize = packetSize * Native.NdisApi.NdisRdEthPacketSize;
            var requestSize = Native.NdisApi.EthMRequestUSize + totalNdisPacketSize;

            var pinnedRequestPtr = _pinnedManagedArrayAllocator.AllocateArray((int)requestSize);
            var ethMRequestUnsafe = (Native.NdisApi.ETH_M_REQUEST_U*)pinnedRequestPtr;
            ethMRequestUnsafe->dwPacketsNumber = packetSize;

            var ndisrdEthPackets = new Native.NdisApi.NDISRD_ETH_Packet[packetSize];

            for (int i = 0; i < packetSize; i++)
            {
                ndisrdEthPackets[i] = new Native.NdisApi.NDISRD_ETH_Packet
                {
                    _buffer = _pinnedManagedArrayAllocator.AllocateArray(Native.NdisApi.IntermediateBufferSize)
                };
            }

            ethMRequestUnsafe->SetPackets(ndisrdEthPackets);

            return ethMRequestUnsafe;
        }

        /// <summary>
        /// Creates a new <see cref="Native.NdisApi.PACKET_OID_DATA" />.
        /// </summary>
        /// <remarks>The adapter handle still needs to be set.</remarks>
        /// <returns>NativeMethods.PACKET_OID_DATA.</returns>
        public unsafe Native.NdisApi.PACKET_OID_DATA CreatePacketOidData(uint dataSize)
        {
            var totalSize = Native.NdisApi.PacketOidDataSize + dataSize;
            var data = new byte[totalSize];
            var pinnedData = GCHandle.Alloc(data, GCHandleType.Pinned);
            var pinnedDataPtr = pinnedData.AddrOfPinnedObject();

            var packetOidData = (Native.NdisApi.PACKET_OID_DATA*) pinnedDataPtr;
            packetOidData->SetData(new byte[dataSize]);
            packetOidData->_length = dataSize;

            pinnedData.Free();

            return *packetOidData;
        }

        /// <summary>
        /// Creates a new <see cref="Native.NdisApi.STATIC_FILTER_TABLE_U" />.
        /// </summary>
        /// <param name="filterSize">Size of the filter.</param>
        /// <returns><see cref="Native.NdisApi.STATIC_FILTER_TABLE_U" />.</returns>
        public unsafe Native.NdisApi.STATIC_FILTER_TABLE_U* CreateUnsafeStaticFilterTable(uint filterSize)
        {
            var totalFilterSize = filterSize * Native.NdisApi.StaticFilterUSize;
            var requestSize = Native.NdisApi.StaticFilterTableUStaticFiltersOffset + totalFilterSize;

            var pinnedRequestPtr = _pinnedManagedArrayAllocator.AllocateArray((int) requestSize);
            var staticFilterTableU = (Native.NdisApi.STATIC_FILTER_TABLE_U*) pinnedRequestPtr;
            staticFilterTableU->m_TableSize = filterSize;

            var staticFilterUs = new Native.NdisApi.STATIC_FILTER_U[filterSize];
            for (int i = 0; i < filterSize; i++)
                staticFilterUs[i] = new Native.NdisApi.STATIC_FILTER_U();

            staticFilterTableU->SetStaticFilters(staticFilterUs);

            return staticFilterTableU;
        }

        /// <summary>
        /// Gets the underlying pinned array for the specified <see cref="ptr" />.
        /// </summary>
        /// <param name="ptr">The pointer.</param>
        /// <returns><see cref="System.Byte" />s.</returns>
        public byte[] GetPinnedArray(IntPtr ptr)
        {
            return _pinnedManagedArrayAllocator.GetArray(ptr);
        }

        /// <summary>
        /// Disposes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public unsafe void DisposeObject(Native.NdisApi.STATIC_FILTER_TABLE_U* obj)
        {
            _pinnedManagedArrayAllocator.FreeArray((IntPtr)obj);
        }

        /// <summary>
        /// Disposes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public unsafe void DisposeObject(Native.NdisApi.ETH_M_REQUEST_U* obj)
        {
            _pinnedManagedArrayAllocator.FreeArray((IntPtr)obj);
        }

        /// <summary>
        /// Disposes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void DisposeObject(Native.NdisApi.ETH_REQUEST obj)
        {
            _pinnedManagedArrayAllocator.FreeArray(obj._ethPacket._buffer);
        }

        /// <summary>
        /// Disposes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void DisposeObject(Native.NdisApi.ETH_M_REQUEST obj)
        {
            foreach (var packet in obj._ethPacket)
                _pinnedManagedArrayAllocator.FreeArray(packet._buffer);
        }
    }
}