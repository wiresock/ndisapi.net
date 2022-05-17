// ----------------------------------------------
// <copyright file="NdisApi.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
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

namespace NdisApiDotNet.Native;

[SuppressUnmanagedCodeSecurity]
public static partial class NdisApi
{
    /// <summary>
    /// The file name of the NDIS API DLL.
    /// </summary>
    public const string DllName = "ndisapi.dll";

    /// <summary>
    /// The ether address length.
    /// </summary>
    public const int ETHER_ADDR_LENGTH = 6;

    /// <summary>
    /// The length of <see cref="INTERMEDIATE_BUFFER.m_Reserved" />.
    /// </summary>
    public const int INTERMEDIATE_BUFFER_RESERVED_LENGTH = 4;

    /// <summary>
    /// The maximum ether frame size.
    /// </summary>
    public const int MAX_ETHER_FRAME = 1514;

    /// <summary>
    /// The adapter list size.
    /// </summary>
    internal const int ADAPTER_LIST_SIZE = 32;

    /// <summary>
    /// The maximum adapter name length.
    /// </summary>
    internal const int ADAPTER_NAME_SIZE = 256;

    /// <summary>
    /// The WAN link buffer length.
    /// </summary>
    internal const int RAS_LINK_BUFFER_LENGTH = 2048;

    /// <summary>
    /// The WAN links maximum length.
    /// </summary>
    internal const int RAS_LINKS_MAX = 256;

    [DllImport(DllName, SetLastError = true)]
    public static extern SafeFilterDriverHandle OpenFilterDriver(byte[] pszDriverName);

    [DllImport(DllName)]
    public static extern void CloseFilterDriver(IntPtr hOpen);

    [DllImport(DllName)]
    public static extern uint GetDriverVersion(SafeFilterDriverHandle hOpen);

    [DllImport(DllName)]
    public static extern bool GetTcpipBoundAdaptersInfo(SafeFilterDriverHandle hOpen, ref TCP_AdapterList adapters);

    [DllImport(DllName)]
    public static extern bool SendPacketToMstcp(SafeFilterDriverHandle hOpen, ref ETH_REQUEST packet);

    [DllImport(DllName)]
    public static extern bool SendPacketToAdapter(SafeFilterDriverHandle hOpen, ref ETH_REQUEST packet);

    [DllImport(DllName)]
    public static extern bool ReadPacket(SafeFilterDriverHandle hOpen, ref ETH_REQUEST packet);

    [DllImport(DllName)]
    public static extern unsafe bool SendPacketsToMstcp(SafeFilterDriverHandle hOpen, ETH_M_REQUEST* packets);

    [DllImport(DllName)]
    public static extern unsafe bool SendPacketsToAdapter(SafeFilterDriverHandle hOpen, ETH_M_REQUEST* packets);

    [DllImport(DllName)]
    public static extern bool SendPacketsToAdaptersUnsorted(SafeFilterDriverHandle hOpen, IntPtr[] packets, uint dwPacketsNum, ref uint pdwPacketsSuccess);

    [DllImport(DllName)]
    public static extern bool SendPacketsToMstcpUnsorted(SafeFilterDriverHandle hOpen, IntPtr[] packets, uint dwPacketsNum, ref uint pdwPacketsSuccess);

    [DllImport(DllName)]
    public static extern bool ReadPacketsUnsorted(SafeFilterDriverHandle hOpen, IntPtr[] packets, uint dwPacketsNum, ref uint pdwPacketsSuccess);

    [DllImport(DllName)]
    public static extern unsafe bool ReadPackets(SafeFilterDriverHandle hOpen, ETH_M_REQUEST* packets);

    [DllImport(DllName)]
    public static extern bool SetAdapterMode(SafeFilterDriverHandle hOpen, ref ADAPTER_MODE mode);

    [DllImport(DllName)]
    public static extern bool GetAdapterMode(SafeFilterDriverHandle hOpen, ref ADAPTER_MODE mode);

    [DllImport(DllName)]
    public static extern bool FlushAdapterPacketQueue(SafeFilterDriverHandle hOpen, IntPtr hAdapter);

    [DllImport(DllName)]
    public static extern bool GetAdapterPacketQueueSize(SafeFilterDriverHandle hOpen, IntPtr hAdapter, ref uint dwSize);

    [DllImport(DllName)]
    public static extern bool SetPacketEvent(SafeFilterDriverHandle hOpen, IntPtr hAdapter, SafeWaitHandle hEvent);

    [DllImport(DllName)]
    public static extern bool SetPacketEvent(SafeFilterDriverHandle hOpen, IntPtr hAdapter, IntPtr hEvent);

    [DllImport(DllName)]
    public static extern bool SetWANEvent(SafeFilterDriverHandle hOpen, SafeWaitHandle hWin32Event);

    [DllImport(DllName)]
    public static extern bool SetAdapterListChangeEvent(SafeFilterDriverHandle hOpen, SafeWaitHandle hWin32Event);

    [DllImport(DllName)]
    public static extern bool NdisrdRequest(SafeFilterDriverHandle hOpen, ref PACKET_OID_DATA oidData, bool set);

    [DllImport(DllName)]
    public static extern unsafe bool NdisrdRequest(SafeFilterDriverHandle hOpen, PACKET_OID_DATA* oidData, bool set);

    [DllImport(DllName)]
    public static extern bool GetRasLinks(SafeFilterDriverHandle hOpen, IntPtr hAdapter, IntPtr pLinks);

    [DllImport(DllName)]
    public static extern bool SetHwPacketFilter(SafeFilterDriverHandle hOpen, IntPtr hAdapter, NDIS_PACKET_TYPE filter);

    [DllImport(DllName)]
    public static extern bool GetHwPacketFilter(SafeFilterDriverHandle hOpen, IntPtr hAdapter, ref NDIS_PACKET_TYPE pFilter);

    [DllImport(DllName)]
    public static extern unsafe bool SetPacketFilterTable(SafeFilterDriverHandle hOpen, STATIC_FILTER_TABLE* pFilterList);

    [DllImport(DllName)]
    public static extern bool ResetPacketFilterTable(SafeFilterDriverHandle hOpen);

    [DllImport(DllName)]
    public static extern bool GetPacketFilterTableSize(SafeFilterDriverHandle hOpen, ref uint pdwTableSize);

    [DllImport(DllName)]
    public static extern unsafe bool GetPacketFilterTable(SafeFilterDriverHandle hOpen, STATIC_FILTER_TABLE* pFilterList);

    [DllImport(DllName)]
    public static extern unsafe bool GetPacketFilterTableResetStats(SafeFilterDriverHandle hOpen, STATIC_FILTER_TABLE* pFilterList);

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

    [DllImport(DllName)]
    public static extern unsafe bool InitializeFastIo(SafeFilterDriverHandle hOpen, FAST_IO_SECTION* pFastIo, uint dwSize);

    [DllImport(DllName)]
    public static extern unsafe bool AddSecondaryFastIo(SafeFilterDriverHandle hOpen, FAST_IO_SECTION* pFastIo, uint dwSize);
}