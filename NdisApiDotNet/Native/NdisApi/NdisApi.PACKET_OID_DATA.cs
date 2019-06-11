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
    public static partial class NdisApi
    {
        /// <summary>
        /// Used for passing a NDIS_REQUEST to the driver.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct PACKET_OID_DATA
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
                get => _length;
                set => _length = value;
            }

            /// <summary>
            /// Gets or sets the oid.
            /// </summary>
            public uint Oid
            {
                get => _oid;
                set => _oid = value;
            }

            /// <summary>
            /// Gets or sets the adapter handle.
            /// </summary>
            public IntPtr AdapterHandle
            {
                get => hAdapterHandle;
                set => hAdapterHandle = value;
            }

            /// <summary>
            /// Gets the data.
            /// </summary>
            /// <param name="length">The number of bytes to retrieve, defaults to <see cref="Length" />.</param>
            /// <returns>byte[].</returns>
            public byte[] GetData(uint length = 0)
            {
                var size = length == 0 ? Length : length;

                var data = new byte[size];
                var pinnedData = GCHandle.Alloc(data, GCHandleType.Pinned);

                var sizeToCopy = NdisRdEthPacketSize * size;
                fixed (PACKET_OID_DATA* a = &this)
                {
                    MemoryCopy((void*)((IntPtr)a + PacketOidDataDataOffset), (void*)pinnedData.AddrOfPinnedObject(), sizeToCopy);
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
                var pinnedData = GCHandle.Alloc(data, GCHandleType.Pinned);

                var sizeToCopy = data.Length;
                fixed (PACKET_OID_DATA* a = &this)
                {
                    MemoryCopy((void*)pinnedData.AddrOfPinnedObject(), (void*)((IntPtr)a + PacketOidDataDataOffset), sizeToCopy);
                }

                pinnedData.Free();
            }
        }
    }
}