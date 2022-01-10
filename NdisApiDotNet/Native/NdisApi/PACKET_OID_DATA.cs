// ----------------------------------------------
// <copyright file="NdisApi.PACKET_OID_DATA.cs" company="NT Kernel">
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
    /// Used for passing a NDIS_REQUEST to the driver.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PacketOID // PACKET_OID_DATA
    {
        internal IntPtr hAdapterHandle;
        internal uint _oid;
        internal uint _length;
        internal byte* _data;

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public uint Length
        {
            get
            {
                return _length;
            }

            set
            {
                _length = value;
            }
        }

        /// <summary>
        /// Gets or sets the oid.
        /// </summary>
        public uint Oid
        {
            get
            {
                return _oid;
            }

            set
            {
                _oid = value;
            }
        }

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
        /// Gets the data.
        /// </summary>
        /// <param name="length">The number of bytes to retrieve, defaults to <see cref="Length" />.</param>
        /// <returns>byte[].</returns>
        public byte[] GetData(uint length = 0)
        {
            uint size = length == 0 ? Length : length;

            byte[] data = new byte[size];
            GCHandle pinnedData = GCHandle.Alloc(data, GCHandleType.Pinned);

            long sizeToCopy = NdisApi.NdisRdEthPacketSize * size;
            fixed (PacketOID* a = &this)
            {
                NdisApi.MemoryCopy((void*)((IntPtr)a + NdisApi.PacketOidDataDataOffset), (void*)pinnedData.AddrOfPinnedObject(), sizeToCopy);
            }

            pinnedData.Free();

            return data;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetData(byte[] data)
        {
            GCHandle pinnedData = GCHandle.Alloc(data, GCHandleType.Pinned);

            int sizeToCopy = data.Length;
            fixed (PacketOID* a = &this)
            {
                NdisApi.MemoryCopy((void*)pinnedData.AddrOfPinnedObject(), (void*)((IntPtr)a + NdisApi.PacketOidDataDataOffset), sizeToCopy);
            }

            pinnedData.Free();
        }
    }
}