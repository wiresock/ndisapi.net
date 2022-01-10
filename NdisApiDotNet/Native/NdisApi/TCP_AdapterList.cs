// ----------------------------------------------
// <copyright file="NdisApi.TCP_AdapterList.cs" company="NT Kernel">
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
    /// Used for requesting information about currently bound TCPIP adapters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TCPAdapterList // TCP_AdapterList
    {
        internal uint m_nAdapterCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NdisApi.ADAPTER_LIST_SIZE * NdisApi.ADAPTER_NAME_SIZE)]
        internal byte[] m_szAdapterNameList;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NdisApi.ADAPTER_LIST_SIZE)]
        internal IntPtr[] m_nAdapterHandle;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NdisApi.ADAPTER_LIST_SIZE)]
        internal NdisMedium[] m_nAdapterMediumList;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NdisApi.ADAPTER_LIST_SIZE * NdisApi.ETHER_ADDR_LENGTH)]
        internal byte[] m_czCurrentAddress;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NdisApi.ADAPTER_LIST_SIZE)]
        internal ushort[] m_usMTU;

        /// <summary>
        /// Gets or sets the number of adapters that are actually set.
        /// </summary>
        public uint AdapterCount
        {
            get
            {
                return m_nAdapterCount;
            }

            set
            {
                m_nAdapterCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the current adapter MTU.
        /// </summary>
        public ushort[] MTU
        {
            get
            {
                return m_usMTU;
            }

            set
            {
                m_usMTU = value;
            }
        }

        /// <summary>
        /// Gets or sets the currently configured ethernet address.
        /// </summary>
        public byte[] CurrentAddress
        {
            get
            {
                return m_czCurrentAddress;
            }

            set
            {
                m_czCurrentAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the adapter mediums.
        /// </summary>
        public NdisMedium[] AdapterMediums
        {
            get
            {
                return m_nAdapterMediumList;
            }

            set
            {
                m_nAdapterMediumList = value;
            }
        }

        /// <summary>
        /// Gets or sets the adapter handles that are the handle for any adapter operation.
        /// </summary>
        public IntPtr[] AdapterHandle
        {
            get
            {
                return m_nAdapterHandle;
            }

            set
            {
                m_nAdapterHandle = value;
            }
        }

        /// <summary>
        /// Array of adapter names.
        /// </summary>
        public byte[] AdapterNames
        {
            get
            {
                return m_szAdapterNameList;
            }

            set
            {
                m_szAdapterNameList = value;
            }
        }
    }
}