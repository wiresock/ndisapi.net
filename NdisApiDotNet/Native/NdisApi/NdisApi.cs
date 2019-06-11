// ----------------------------------------------
// <copyright file="NdisApi.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        /// <summary>
        /// The file name of the Ndis Api DLL.
        /// </summary>
        public const string DllName = "ndisapi.dll";

        static unsafe NdisApi()
        {
            NdisRdEthPacketSize = Marshal.SizeOf(typeof(NDISRD_ETH_Packet));

            EthMRequestUSize = Marshal.SizeOf(typeof(ETH_M_REQUEST_U));
            EthMRequestUEthPacketOffset = (int)Marshal.OffsetOf(typeof(ETH_M_REQUEST_U), nameof(ETH_M_REQUEST_U._ethPacket));

            StaticFilterUSize = Marshal.SizeOf(typeof(STATIC_FILTER_U));
            StaticFilterTableUSize = Marshal.SizeOf(typeof(STATIC_FILTER_TABLE_U));
            StaticFilterTableUStaticFiltersOffset = (int)Marshal.OffsetOf(typeof(STATIC_FILTER_TABLE_U), nameof(STATIC_FILTER_TABLE_U.m_StaticFilters));

            PacketOidDataSize = Marshal.SizeOf(typeof(PACKET_OID_DATA));
            PacketOidDataDataOffset = (int)Marshal.OffsetOf(typeof(PACKET_OID_DATA), nameof(PACKET_OID_DATA._data));

            IntermediateBufferSize = Marshal.SizeOf(typeof(INTERMEDIATE_BUFFER));
            IntermediateBufferBufferOffset = (int)Marshal.OffsetOf(typeof(INTERMEDIATE_BUFFER), nameof(INTERMEDIATE_BUFFER.m_IBuffer));
        }
        
        #region Methods

        [DllImport(DllName)]
        public static extern SafeFilterDriverHandle OpenFilterDriver(byte[] pszDriverName);

        [DllImport(DllName)]
        public static extern void CloseFilterDriver(IntPtr hOpen);

        [DllImport(DllName)]
        public static extern uint GetDriverVersion(SafeFilterDriverHandle hOpen);

        [DllImport(DllName)]
        public static extern bool GetTcpipBoundAdaptersInfo(SafeFilterDriverHandle hOpen, ref TCP_AdapterList adapters);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool SendPacketToMstcp(SafeFilterDriverHandle hOpen, ref ETH_REQUEST packet);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool SendPacketToAdapter(SafeFilterDriverHandle hOpen, ref ETH_REQUEST packet);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool ReadPacket(SafeFilterDriverHandle hOpen, ref ETH_REQUEST packet);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool SendPacketsToMstcp(SafeFilterDriverHandle hOpen, ref ETH_M_REQUEST packets);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern unsafe bool SendPacketsToMstcp(SafeFilterDriverHandle hOpen, ETH_M_REQUEST_U* packets);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool SendPacketsToAdapter(SafeFilterDriverHandle hOpen, ref ETH_M_REQUEST packets);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern unsafe bool SendPacketsToAdapter(SafeFilterDriverHandle hOpen, ETH_M_REQUEST_U* packets);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool ReadPackets(SafeFilterDriverHandle hOpen, ref ETH_M_REQUEST packets);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern unsafe bool ReadPackets(SafeFilterDriverHandle hOpen, ETH_M_REQUEST_U* packets);

        [DllImport(DllName)]
        public static extern bool SetAdapterMode(SafeFilterDriverHandle hOpen, ref ADAPTER_MODE mode);

        [DllImport(DllName)]
        public static extern bool GetAdapterMode(SafeFilterDriverHandle hOpen, ref ADAPTER_MODE mode);

        [DllImport(DllName)]
        public static extern bool FlushAdapterPacketQueue(SafeFilterDriverHandle hOpen, IntPtr hAdapter);

        [DllImport(DllName)]
        public static extern bool GetAdapterPacketQueueSize(SafeFilterDriverHandle hOpen, IntPtr hAdapter, ref uint dwSize);

        [DllImport(DllName)]
        public static extern bool SetPacketEvent(SafeFilterDriverHandle hOpen, IntPtr hAdapter, SafeWaitHandle hWin32Event);

        [DllImport(DllName)]
        public static extern bool SetWANEvent(SafeFilterDriverHandle hOpen, SafeWaitHandle hWin32Event);

        [DllImport(DllName)]
        public static extern bool SetAdapterListChangeEvent(SafeFilterDriverHandle hOpen, SafeWaitHandle hWin32Event);

        [DllImport(DllName)]
        public static extern bool NdisrdRequest(SafeFilterDriverHandle hOpen, ref PACKET_OID_DATA oidData, bool set);

        [DllImport(DllName)]
        public static extern bool GetRasLinks(SafeFilterDriverHandle hOpen, IntPtr hAdapter, IntPtr pLinks);

        [DllImport(DllName)]
        public static extern bool SetHwPacketFilter(SafeFilterDriverHandle hOpen, IntPtr hAdapter, NDIS_PACKET_TYPE filter);

        [DllImport(DllName)]
        public static extern bool GetHwPacketFilter(SafeFilterDriverHandle hOpen, IntPtr hAdapter, ref NDIS_PACKET_TYPE pFilter);

        [DllImport(DllName)]
        public static extern bool SetPacketFilterTable(SafeFilterDriverHandle hOpen, ref STATIC_FILTER_TABLE pFilterList);

        [DllImport(DllName)]
        public static extern unsafe bool SetPacketFilterTable(SafeFilterDriverHandle hOpen, STATIC_FILTER_TABLE_U* pFilterList);

        [DllImport(DllName)]
        public static extern bool ResetPacketFilterTable(SafeFilterDriverHandle hOpen);

        [DllImport(DllName)]
        public static extern bool GetPacketFilterTableSize(SafeFilterDriverHandle hOpen, ref uint pdwTableSize);

        [DllImport(DllName)]
        public static extern bool GetPacketFilterTable(SafeFilterDriverHandle hOpen, ref STATIC_FILTER_TABLE pFilterList);

        [DllImport(DllName)]
        public static extern unsafe bool GetPacketFilterTable(SafeFilterDriverHandle hOpen, STATIC_FILTER_TABLE_U* pFilterList);

        [DllImport(DllName)]
        public static extern bool GetPacketFilterTableResetStats(SafeFilterDriverHandle hOpen, ref STATIC_FILTER_TABLE pFilterList);

        [DllImport(DllName)]
        public static extern unsafe bool GetPacketFilterTableResetStats(SafeFilterDriverHandle hOpen, STATIC_FILTER_TABLE_U* pFilterList);

        [DllImport(DllName)]
        public static extern uint GetMTUDecrement();

        [DllImport(DllName)]
        public static extern bool SetMTUDecrement(uint dwMtuDecrement);

        [DllImport(DllName)]
        public static extern MSTCP_FLAGS GetAdaptersStartupMode();

        [DllImport(DllName)]
        public static extern bool SetAdaptersStartupMode(MSTCP_FLAGS dwStartupMode);

        [DllImport(DllName)]
        public static extern bool IsDriverLoaded(SafeFilterDriverHandle hOpen);

        [DllImport(DllName)]
        public static extern uint GetBytesReturned(SafeFilterDriverHandle hOpen);

        [DllImport(DllName)]
        public static extern unsafe bool ConvertWindows2000AdapterName(byte* szAdapterName, byte* szUserFriendlyName, uint dwUserFriendlyNameLength);

        [DllImport(DllName)]
        public static extern unsafe bool ConvertWindows9xAdapterName(byte* szAdapterName, byte* szUserFriendlyName, uint dwUserFriendlyNameLength);

        [DllImport(DllName)]
        public static extern unsafe bool ConvertWindowsNTAdapterName(byte* szAdapterName, byte* szUserFriendlyName, uint dwUserFriendlyNameLength);

        /// <summary>
        /// Copies the specified memory.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="size">The size.</param>
        private static unsafe void MemoryCopy(void* src, void* dest, long size)
        {
#if NETSTANDARD2_0
            Buffer.MemoryCopy(src, dest, size, size);
#else
            Kernel32.CopyMemory((IntPtr)dest, (IntPtr)src, (uint)size);
#endif
        }

        #endregion


        #region Sizes & Offsets

        /// <summary>
        /// The ether multiple request eth packet size
        /// </summary>
        public const int EthMRequestEthPacketSize = 256;

        /// <summary>
        /// The ether multiple request unsafe size.
        /// </summary>
        public static readonly int EthMRequestUSize;

        /// <summary>
        /// The ether multiple request unsafe eth packet offset.
        /// </summary>
        public static readonly int EthMRequestUEthPacketOffset;

        /// <summary>
        /// The ndis rd eth packet size.
        /// </summary>
        public static readonly int NdisRdEthPacketSize;

        /// <summary>
        /// The packet oid data data offset.
        /// </summary>
        public static readonly int PacketOidDataDataOffset;

        /// <summary>
        /// The packet oid data size.
        /// </summary>
        public static readonly int PacketOidDataSize;

        /// <summary>
        /// The static filter table u static filters offset.
        /// </summary>
        public static readonly int StaticFilterTableUStaticFiltersOffset;

        /// <summary>
        /// The static filter size.
        /// </summary>
        public static readonly int StaticFilterUSize;

        /// <summary>
        /// The size of the static filter table u.
        /// </summary>
        public static readonly int StaticFilterTableUSize;

        /// <summary>
        /// The intermediate buffer size.
        /// </summary>
        public static readonly int IntermediateBufferSize;

        /// <summary>
        /// The intermediate buffer buffer offset.
        /// </summary>
        public static readonly int IntermediateBufferBufferOffset;

        #endregion


        #region Constants

        /// <summary>
        /// The WAN link buffer length.
        /// </summary>
        internal const int RAS_LINK_BUFFER_LENGTH = 2048;

        /// <summary>
        /// The WAN links maximum length.
        /// </summary>
        public const int RAS_LINKS_MAX = 256;

        /// <summary>
        /// The maximum adapter name length.
        /// </summary>
        internal const int ADAPTER_NAME_SIZE = 256;

        /// <summary>
        /// The adapter list size.
        /// </summary>
        internal const int ADAPTER_LIST_SIZE = 32;

        /// <summary>
        /// The maximum ether frame size.
        /// </summary>
        public const int MAX_ETHER_FRAME = 1514;

        /// <summary>
        /// The ether address length.
        /// </summary>
        public const int ETHER_ADDR_LENGTH = 6;

        #endregion
    }
}