// ----------------------------------------------
// <copyright file="NdisApi.TCP_AdapterList.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// Used for requesting information about currently bound TCPIP adapters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TCP_AdapterList
    {
        /// <summary>
        /// The number of adapters that are actually set.
        /// </summary>
        public uint m_nAdapterCount;

        /// <summary>
        /// The adapter names.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ADAPTER_LIST_SIZE * ADAPTER_NAME_SIZE)]
        public byte[] m_szAdapterNameList;

        /// <summary>
        /// The adapter handle.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ADAPTER_LIST_SIZE)]
        public IntPtr[] m_nAdapterHandle;

        /// <summary>
        /// The adapter mediums.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ADAPTER_LIST_SIZE)]
        public NDIS_MEDIUM[] m_nAdapterMediumList;

        /// <summary>
        /// The currently configured ethernet address.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ADAPTER_LIST_SIZE * ETHER_ADDR_LENGTH)]
        public byte[] m_czCurrentAddress;

        /// <summary>
        /// The current adapter MTU.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ADAPTER_LIST_SIZE)]
        public ushort[] m_usMTU;
    }
}