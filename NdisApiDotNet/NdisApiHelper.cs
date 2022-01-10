// ----------------------------------------------
// <copyright file="NdisApiHelper.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using NdisApiDotNet.Native;
using System;
using System.Runtime.InteropServices;

namespace NdisApiDotNet
{
    public class NdisAPIHelper : IDisposable
    {
        private readonly PinnedManagedArrayAllocator<byte> _pinnedManagedArrayAllocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NdisAPIHelper"/> class.
        /// </summary>
        public NdisAPIHelper()
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
        public static void CloneEthRequest(ref EthRequest source, ref EthRequest destination)
        {
            destination.hAdapterHandle = source.hAdapterHandle;

            Kernel32.CopyMemory(destination._ethPacket._buffer, source._ethPacket._buffer, (uint)NdisApi.IntermediateBufferSize);
        }

        /// <summary>
        /// Clones the specified request.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CloneEthMRequest(ref EthMRequest source, ref EthMRequest destination)
        {
            destination.hAdapterHandle = source.hAdapterHandle;
            destination.dwPacketsSuccess = source.dwPacketsSuccess;
            destination.dwPacketsNumber = source.dwPacketsNumber;

            for (int i = 0; i < source.dwPacketsSuccess; i++) Kernel32.CopyMemory(destination._ethPacket[i]._buffer, source._ethPacket[i]._buffer, (uint)NdisApi.IntermediateBufferSize);
        }

        /// <summary>
        /// Clones the specified request.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public unsafe void CloneUnsafeEthMRequest(ref EthMRequestUnsafe* source, ref EthMRequestUnsafe* destination)
        {
            destination->hAdapterHandle = source->hAdapterHandle;
            destination->dwPacketsSuccess = source->dwPacketsSuccess;
            destination->dwPacketsNumber = source->dwPacketsNumber;

            EthPacket[] requestPackets = source->GetPackets();
            EthPacket[] nextPackets = destination->GetPackets();

            for (int i = 0; i < source->dwPacketsSuccess; i++) Kernel32.CopyMemory(nextPackets[i]._buffer, requestPackets[i]._buffer, (uint)NdisApi.IntermediateBufferSize);
        }

        /// <summary>
        /// Clones the specified <see cref="source" />.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns><see cref="EthRequest" />.</returns>
        public EthRequest CloneEthRequest(ref EthRequest source)
        {
            EthRequest destination = CreateEthRequest();
            CloneEthRequest(ref source, ref destination);

            return destination;
        }

        /// <summary>
        /// Clones the specified <see cref="source" />.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns><see cref="EthMRequest" />.</returns>
        public EthMRequest CloneEthMRequest(ref EthMRequest source)
        {
            EthMRequest destination = CreateEthMRequest();
            CloneEthMRequest(ref source, ref destination);

            return destination;
        }

        /// <summary>
        /// Clones the specified <see cref="source" />.
        /// </summary>
        /// <param name="source">The request.</param>
        /// <returns><see cref="EthMRequestUnsafe" />.</returns>
        public unsafe EthMRequestUnsafe* CloneUnsafeEthMRequest(ref EthMRequestUnsafe* source)
        {
            EthMRequestUnsafe* destination = CreateUnsafeEthMRequest(source->dwPacketsNumber);
            CloneUnsafeEthMRequest(ref source, ref destination);

            return destination;
        }

        /// <summary>
        /// Zeroes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public static void ZeroEthRequest(ref EthRequest request)
        {
            request.hAdapterHandle = IntPtr.Zero;
            Kernel32.ZeroMemory(request._ethPacket._buffer, NdisApi.IntermediateBufferSize);
        }

        /// <summary>
        /// Zeroes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public static void ZeroEthMRequest(ref EthMRequest request)
        {
            request.hAdapterHandle = IntPtr.Zero;

            for (int i = 0; i < request.dwPacketsNumber; i++) Kernel32.ZeroMemory(request._ethPacket[i]._buffer, NdisApi.IntermediateBufferSize);
        }

        /// <summary>
        /// Zeroes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public unsafe void ZeroUnsafeEthMRequest(ref EthMRequestUnsafe* request)
        {
            request->hAdapterHandle = IntPtr.Zero;
            EthPacket[] packets = request->GetPackets();

            for (int i = 0; i < request->dwPacketsNumber; i++) Kernel32.ZeroMemory(packets[i]._buffer, NdisApi.IntermediateBufferSize);
        }

        /// <summary>
        /// Creates a new <see cref="EthRequest" />.
        /// </summary>
        /// <remarks>The adapter handle still needs to be set.</remarks>
        /// <returns>NativeMethods.ETH_REQUEST.</returns>
        public EthRequest CreateEthRequest()
        {
            return new EthRequest
            {
                _ethPacket =
                {
                    _buffer = _pinnedManagedArrayAllocator.AllocateArray(NdisApi.IntermediateBufferSize)
                }
            };
        }

        /// <summary>
        /// Creates a new <see cref="EthMRequest" />.
        /// </summary>
        /// <remarks>The adapter handle still needs to be set.</remarks>
        /// <returns>NativeMethods.ETH_M_REQUEST_256.</returns>
        public EthMRequest CreateEthMRequest()
        {
            EthMRequest ethPackets = new EthMRequest
            {
                _ethPacket = new EthPacket[NdisApi.EthMRequestEthPacketSize],
                dwPacketsNumber = NdisApi.EthMRequestEthPacketSize
            };

            for (int i = 0; i < NdisApi.EthMRequestEthPacketSize; i++) ethPackets._ethPacket[i]._buffer = _pinnedManagedArrayAllocator.AllocateArray(NdisApi.IntermediateBufferSize);

            return ethPackets;
        }

        /// <summary>
        /// Creates a new <see cref="EthMRequestUnsafe" />.
        /// </summary>
        /// <param name="packetSize">Size of the packet.</param>
        /// <returns>NativeMethods.ETH_M_REQUEST_U.</returns>
        /// <remarks>The adapter handle still needs to be set.</remarks>
        public unsafe EthMRequestUnsafe* CreateUnsafeEthMRequest(uint packetSize)
        {
            long totalNdisPacketSize = packetSize * NdisApi.NdisRdEthPacketSize;
            long requestSize = NdisApi.EthMRequestUSize + totalNdisPacketSize;

            IntPtr pinnedRequestPtr = _pinnedManagedArrayAllocator.AllocateArray((int)requestSize);
            EthMRequestUnsafe* ethMRequestUnsafe = (EthMRequestUnsafe*)pinnedRequestPtr;
            ethMRequestUnsafe->dwPacketsNumber = packetSize;

            EthPacket[] ndisrdEthPackets = new EthPacket[packetSize];

            for (int i = 0; i < packetSize; i++)
            {
                ndisrdEthPackets[i] = new EthPacket
                {
                    _buffer = _pinnedManagedArrayAllocator.AllocateArray(NdisApi.IntermediateBufferSize)
                };
            }

            ethMRequestUnsafe->SetPackets(ndisrdEthPackets);

            return ethMRequestUnsafe;
        }

        /// <summary>
        /// Creates a new <see cref="PACKET_OID_DATA" />.
        /// </summary>
        /// <remarks>The adapter handle still needs to be set.</remarks>
        /// <returns>NativeMethods.PACKET_OID_DATA.</returns>
        public unsafe PacketOID CreatePacketOidData(uint dataSize)
        {
            long totalSize = NdisApi.PacketOidDataSize + dataSize;
            byte[] data = new byte[totalSize];
            GCHandle pinnedData = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr pinnedDataPtr = pinnedData.AddrOfPinnedObject();

            PacketOID* packetOidData = (PacketOID*)pinnedDataPtr;
            packetOidData->SetData(new byte[dataSize]);
            packetOidData->_length = dataSize;

            pinnedData.Free();

            return *packetOidData;
        }

        /// <summary>
        /// Creates a new <see cref="StaticFilterTableUnsafe" />.
        /// </summary>
        /// <param name="filterSize">Size of the filter.</param>
        /// <returns><see cref="StaticFilterTableUnsafe" />.</returns>
        public unsafe StaticFilterTableUnsafe* CreateUnsafeStaticFilterTable(uint filterSize)
        {
            long totalFilterSize = filterSize * NdisApi.StaticFilterUSize;
            long requestSize = NdisApi.StaticFilterTableUStaticFiltersOffset + totalFilterSize;

            IntPtr pinnedRequestPtr = _pinnedManagedArrayAllocator.AllocateArray((int)requestSize);
            StaticFilterTableUnsafe* staticFilterTableU = (StaticFilterTableUnsafe*)pinnedRequestPtr;
            staticFilterTableU->m_TableSize = filterSize;

            StaticFilterUnsafe[] staticFilterUs = new StaticFilterUnsafe[filterSize];
            for (int i = 0; i < filterSize; i++)
                staticFilterUs[i] = new StaticFilterUnsafe();

            staticFilterTableU->SetStaticFilters(staticFilterUs);

            return staticFilterTableU;
        }

        /// <summary>
        /// Gets the underlying pinned array for the specified <see cref="ptr" />.
        /// </summary>
        /// <param name="ptr">The pointer.</param>
        /// <returns><see cref="byte" />s.</returns>
        public byte[] GetPinnedArray(IntPtr ptr)
        {
            return _pinnedManagedArrayAllocator.GetArray(ptr);
        }

        /// <summary>
        /// Disposes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public unsafe void DisposeObject(StaticFilterTableUnsafe* obj)
        {
            _pinnedManagedArrayAllocator.FreeArray((IntPtr)obj);
        }

        /// <summary>
        /// Disposes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public unsafe void DisposeObject(EthMRequestUnsafe* obj)
        {
            _pinnedManagedArrayAllocator.FreeArray((IntPtr)obj);
        }

        /// <summary>
        /// Disposes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void DisposeObject(EthRequest obj)
        {
            _pinnedManagedArrayAllocator.FreeArray(obj._ethPacket._buffer);
        }

        /// <summary>
        /// Disposes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void DisposeObject(EthMRequest obj)
        {
            foreach (EthPacket packet in obj._ethPacket) _pinnedManagedArrayAllocator.FreeArray(packet._buffer);
        }
    }
}