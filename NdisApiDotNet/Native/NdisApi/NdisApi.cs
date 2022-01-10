// ----------------------------------------------
// <copyright file="NdisApi.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Security;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace NdisApiDotNet.Native
{
    internal static class NdisApi
    {
        /// <summary>
        /// The file name of the Ndis Api DLL.
        /// </summary>
        public const string DllName = "ndisapi.dll";

        static unsafe NdisApi()
        {
            NdisRdEthPacketSize = Marshal.SizeOf(typeof(EthPacket));

            EthMRequestUSize = Marshal.SizeOf(typeof(EthMRequestUnsafe));
            EthMRequestUEthPacketOffset = (int)Marshal.OffsetOf(typeof(EthMRequestUnsafe), nameof(EthMRequestUnsafe._ethPacket));

            StaticFilterUSize = Marshal.SizeOf(typeof(StaticFilterUnsafe));
            StaticFilterTableUSize = Marshal.SizeOf(typeof(StaticFilterTableUnsafe));
            StaticFilterTableUStaticFiltersOffset = (int)Marshal.OffsetOf(typeof(StaticFilterTableUnsafe), nameof(StaticFilterTableUnsafe.m_StaticFilters));

            PacketOidDataSize = Marshal.SizeOf(typeof(PacketOID));
            PacketOidDataDataOffset = (int)Marshal.OffsetOf(typeof(PacketOID), nameof(PacketOID._data));

            IntermediateBufferSize = Marshal.SizeOf(typeof(IntermediateBuffer));
            IntermediateBufferBufferOffset = (int)Marshal.OffsetOf(typeof(IntermediateBuffer), nameof(IntermediateBuffer.m_IBuffer));
        }

        #region Methods

        [DllImport(DllName)]
        public static extern SafeFilterDriverHandle OpenFilterDriver(byte[] pszDriverName);

        [DllImport(DllName)]
        public static extern void CloseFilterDriver(IntPtr hOpen);

        [DllImport(DllName)]
        public static extern uint GetDriverVersion(SafeFilterDriverHandle hOpen);

        [DllImport(DllName)]
        public static extern bool GetTcpipBoundAdaptersInfo(SafeFilterDriverHandle hOpen, ref TCPAdapterList adapters);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool SendPacketToMstcp(SafeFilterDriverHandle hOpen, ref EthRequest packet);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool SendPacketToAdapter(SafeFilterDriverHandle hOpen, ref EthRequest packet);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool ReadPacket(SafeFilterDriverHandle hOpen, ref EthRequest packet);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool SendPacketsToMstcp(SafeFilterDriverHandle hOpen, ref EthMRequest packets);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern unsafe bool SendPacketsToMstcp(SafeFilterDriverHandle hOpen, EthMRequestUnsafe* packets);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool SendPacketsToAdapter(SafeFilterDriverHandle hOpen, ref EthMRequest packets);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern unsafe bool SendPacketsToAdapter(SafeFilterDriverHandle hOpen, EthMRequestUnsafe* packets);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool ReadPackets(SafeFilterDriverHandle hOpen, ref EthMRequest packets);

        [DllImport(DllName)]
        [SuppressUnmanagedCodeSecurity]
        public static extern unsafe bool ReadPackets(SafeFilterDriverHandle hOpen, EthMRequestUnsafe* packets);

        [DllImport(DllName)]
        public static extern bool SetAdapterMode(SafeFilterDriverHandle hOpen, ref AdapterMode mode);

        [DllImport(DllName)]
        public static extern bool GetAdapterMode(SafeFilterDriverHandle hOpen, ref AdapterMode mode);

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
        public static extern bool NdisrdRequest(SafeFilterDriverHandle hOpen, ref PacketOID oidData, bool set);

        [DllImport(DllName)]
        public static extern bool GetRasLinks(SafeFilterDriverHandle hOpen, IntPtr hAdapter, IntPtr pLinks);

        [DllImport(DllName)]
        public static extern bool SetHwPacketFilter(SafeFilterDriverHandle hOpen, IntPtr hAdapter, PacketType filter);

        [DllImport(DllName)]
        public static extern bool GetHwPacketFilter(SafeFilterDriverHandle hOpen, IntPtr hAdapter, ref PacketType pFilter);

        [DllImport(DllName)]
        public static extern bool SetPacketFilterTable(SafeFilterDriverHandle hOpen, ref StaticFilterTable pFilterList);

        [DllImport(DllName)]
        public static extern unsafe bool SetPacketFilterTable(SafeFilterDriverHandle hOpen, StaticFilterTableUnsafe* pFilterList);

        [DllImport(DllName)]
        public static extern bool ResetPacketFilterTable(SafeFilterDriverHandle hOpen);

        [DllImport(DllName)]
        public static extern bool GetPacketFilterTableSize(SafeFilterDriverHandle hOpen, ref uint pdwTableSize);

        [DllImport(DllName)]
        public static extern bool GetPacketFilterTable(SafeFilterDriverHandle hOpen, ref StaticFilterTable pFilterList);

        [DllImport(DllName)]
        public static extern unsafe bool GetPacketFilterTable(SafeFilterDriverHandle hOpen, StaticFilterTableUnsafe* pFilterList);

        [DllImport(DllName)]
        public static extern bool GetPacketFilterTableResetStats(SafeFilterDriverHandle hOpen, ref StaticFilterTable pFilterList);

        [DllImport(DllName)]
        public static extern unsafe bool GetPacketFilterTableResetStats(SafeFilterDriverHandle hOpen, StaticFilterTableUnsafe* pFilterList);

        [DllImport(DllName)]
        public static extern uint GetMTUDecrement();

        [DllImport(DllName)]
        public static extern bool SetMTUDecrement(uint dwMtuDecrement);

        [DllImport(DllName)]
        public static extern MSTCPFlags GetAdaptersStartupMode();

        [DllImport(DllName)]
        public static extern bool SetAdaptersStartupMode(MSTCPFlags dwStartupMode);

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
        internal static unsafe void MemoryCopy(void* src, void* dest, long size)
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