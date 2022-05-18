// ----------------------------------------------
// <copyright file="NdisApi.PACKET_OID_DATA.cs" company="NT Kernel">
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
    /// Used for passing a NDIS_REQUEST to the driver.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PACKET_OID_DATA
    {
        /// <summary>
        /// The adapter handle.
        /// </summary>
        public IntPtr hAdapterHandle;

        /// <summary>
        /// The oid.
        /// </summary>
        public uint Oid;

        /// <summary>
        /// The length of the <see cref="Data" />.
        /// </summary>
        public uint Length;

        /// <summary>
        /// The data.
        /// </summary>
        public fixed byte Data[1];

        /// <summary>
        /// Gets the pointer to the data.
        /// </summary>
        public byte* DataPointer => (byte*) Unsafe.AsPointer(ref Data[0]);

        /// <summary>
        /// Gets or sets the <see cref="byte" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns><see cref="byte" />.</returns>
        public byte this[int index]
        {
            get => Data[index];
            set => Data[index] = value;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns><see cref="byte" />s.</returns>
        public byte[] GetData()
        {
            var dataSize = Length;
            var data = new byte[dataSize];

            for (int i = 0; i < dataSize; i++)
            {
                data[i] = Data[i];
            }

            return data;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetData(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Data[i] = data[i];
            }

            Length = (uint) data.Length;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetData(short data)
        {
            fixed (byte* d = Data)
            {
                var c = (short*) d;
                *c = data;
            }

            Length = 2;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetData(ushort data)
        {
            fixed (byte* d = Data)
            {
                var c = (ushort*) d;
                *c = data;
            }

            Length = 2;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetData(int data)
        {
            fixed (byte* d = Data)
            {
                var c = (int*) d;
                *c = data;
            }

            Length = 4;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetData(uint data)
        {
            fixed (byte* d = Data)
            {
                var c = (uint*) d;
                *c = data;
            }

            Length = 4;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetData(long data)
        {
            fixed (byte* d = Data)
            {
                var c = (long*) d;
                *c = data;
            }

            Length = 8;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetData(ulong data)
        {
            fixed (byte* d = Data)
            {
                var c = (ulong*) d;
                *c = data;
            }

            Length = 8;
        }

        /// <summary>
        /// The size of <see cref="PACKET_OID_DATA" /> without the <see cref="DataPointer" />
        /// </summary>
        public static int SizeOfHeader = Marshal.SizeOf<PACKET_OID_DATA>() - 1;
    }
}