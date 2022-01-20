﻿// ----------------------------------------------
// <copyright file="NdisApi.INTERMEDIATE_BUFFER.cs" company="NT Kernel">
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

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        /// <summary>
        /// Contains packet buffer, packet NDIS flags, WinPkFilter specific flags.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct INTERMEDIATE_BUFFER
        {
            /// <summary>
            /// The adapter handle and qlink union.
            /// </summary>
            public LIST_ENTRY_OR_ADAPTER_HANDLE m_qLink_hAdapter;

            /// <summary>
            /// The device flags.
            /// </summary>
            public PACKET_FLAG m_dwDeviceFlags;

            /// <summary>
            /// The length of the <see cref="Buffer" />.
            /// </summary>
            public uint m_Length;

            /// <summary>
            /// The NDIS packet flags.
            /// </summary>
            public NDIS_FLAGS m_Flags;

            /// <summary>
            /// The 802.1q info.
            /// </summary>
            public uint m_8021q;

            /// <summary>
            /// The filter identifier.
            /// </summary>
            public uint m_FilterID;

            /// <summary>
            /// The reserved values, currently unused.
            /// </summary>
            public fixed uint m_Reserved[4];

            /// <summary>
            /// The data buffer.
            /// </summary>
            public fixed byte m_IBuffer[MAX_ETHER_FRAME];

            /// <summary>
            /// Gets or sets the adapter handle.
            /// </summary>
            public IntPtr AdapterHandle
            {
                get => m_qLink_hAdapter.m_hAdapter;
                set => m_qLink_hAdapter.m_hAdapter = value;
            }

            /// <summary>
            /// Gets the data buffer.
            /// </summary>
            public byte* Buffer => (byte*) Unsafe.AsPointer(ref m_IBuffer[0]);

            /// <summary>
            /// Gets the reserved values.
            /// </summary>
            public uint* Reserved => (uint*) Unsafe.AsPointer(ref m_Reserved[0]);

            /// <summary>
            /// The offset of <see cref="Buffer" />.
            /// </summary>
            public static IntPtr BufferOffset = Marshal.OffsetOf(typeof(INTERMEDIATE_BUFFER), nameof(m_IBuffer));

            /// <summary>
            /// The offset of <see cref="Reserved" />.
            /// </summary>
            public static IntPtr ReservedOffset = Marshal.OffsetOf(typeof(INTERMEDIATE_BUFFER), nameof(m_Reserved));

            /// <summary>
            /// The size of <see cref="INTERMEDIATE_BUFFER" />.
            /// </summary>
            public static int Size = Marshal.SizeOf<INTERMEDIATE_BUFFER>();
        }
    }
}