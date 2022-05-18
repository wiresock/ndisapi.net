// ----------------------------------------------
// <copyright file="NdisApi.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using NdisApiDotNet.Native;

namespace NdisApiDotNet;

public unsafe class NdisApi : IDisposable
{
    private const int EthMRequestPacketSize = 256;

    private readonly ArrayPool<IntPtr> _arrayPool;
    private readonly byte[] _driverNameBytes;
    private readonly Native.NdisApi.ETH_M_REQUEST* _ethPacketsToAdapter;
    private readonly Native.NdisApi.ETH_M_REQUEST* _ethPacketsToMstcp;
    private readonly object _lock = new();
    private readonly PinnedManagedArrayAllocator<byte> _pinnedManagedArrayAllocator;
    private Native.NdisApi.FAST_IO_SECTION* _fastIOSection;
    private int _fastIOSectionNumberOfPackets;
    private Native.NdisApi.FAST_IO_SECTION*[] _secondaryFastIOSections;

    /// <summary>
    /// Initializes a new instance of the <see cref="NdisApi" /> class.
    /// </summary>
    /// <param name="handle">The filter driver handle.</param>
    /// <param name="driverNameBytes">The driver name bytes.</param>
    /// <param name="maxPacketSize">The maximum packet size in bytes.</param>
    protected NdisApi(SafeFilterDriverHandle handle, byte[] driverNameBytes, int maxPacketSize)
    {
        _driverNameBytes = driverNameBytes;

        _arrayPool = ArrayPool<IntPtr>.Create();
        _pinnedManagedArrayAllocator = new PinnedManagedArrayAllocator<byte>();

        _ethPacketsToMstcp = CreateEthMRequest(EthMRequestPacketSize);
        _ethPacketsToAdapter = CreateEthMRequest(EthMRequestPacketSize);

        Handle = handle;
        MaxPacketSize = maxPacketSize;
        IntermediateBufferSize = Native.NdisApi.INTERMEDIATE_BUFFER_VARIABLE.SizeOfHeader + maxPacketSize;
    }

    /// <summary>
    /// Gets the handle to the filter driver.
    /// </summary>
    public SafeFilterDriverHandle Handle { get; private set; }

    /// <summary>
    /// Gets the size of the intermediate buffer in bytes.
    /// </summary>
    public int IntermediateBufferSize { get; }

    /// <summary>
    /// Gets the maximum size of a packet in bytes.
    /// </summary>
    public int MaxPacketSize { get; }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        Handle?.Dispose();
        _pinnedManagedArrayAllocator.Dispose();

        if (_fastIOSection != null)
            Marshal.FreeHGlobal((IntPtr) _fastIOSection);

        if (_secondaryFastIOSections != null)
        {
            foreach (var secondaryFastIOSection in _secondaryFastIOSections)
                Marshal.FreeHGlobal((IntPtr) secondaryFastIOSection);
        }
    }

    /// <summary>
    /// Opens the filter driver.
    /// </summary>
    /// <param name="driverName">The name of the driver.</param>
    /// <param name="maxPacketSize">The maximum packet size in bytes, defaults to <see cref="Native.NdisApi.MAX_ETHER_FRAME" />.</param>
    /// <returns><see cref="NdisApi" />.</returns>
    /// <exception cref="Exception">Missing NDIS DLL</exception>
    public static NdisApi Open(string driverName = "NDISRD", int maxPacketSize = Native.NdisApi.MAX_ETHER_FRAME)
    {
        if (!NdisApiDllExists())
            throw new DllNotFoundException("Missing NDIS DLL.");

        var driverNameBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(driverName);
        var handle = Native.NdisApi.OpenFilterDriver(driverNameBytes);
        if (handle.IsInvalid)
            throw new Win32Exception(Marshal.GetLastWin32Error());

        return new NdisApi(handle, driverNameBytes, maxPacketSize);
    }

    /// <summary>
    /// Closes the filter driver.
    /// </summary>
    public void Close()
    {
        Handle.Close();
    }

    /// <summary>
    /// Reopens the filter driver.
    /// </summary>
    public void Reopen()
    {
        if (_driverNameBytes == null || _driverNameBytes.Length == 0)
            throw new Exception("Missing driver name.");

        Close();
        Handle = Native.NdisApi.OpenFilterDriver(_driverNameBytes);
    }

    /// <summary>
    /// Gets the native version of the filter driver.
    /// </summary>
    /// <returns><see cref="uint" />.</returns>
    public uint GetNativeVersion()
    {
        return Native.NdisApi.GetDriverVersion(Handle);
    }

    /// <summary>
    /// Gets the version of the filter driver.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns><see cref="Version" />.</returns>
    public Version GetVersion(string fileName = "NDISRD")
    {
        var filePath = Path.Combine(Environment.SystemDirectory, @"drivers\" + fileName + ".sys");
        if (!File.Exists(filePath))
            return new Version(0, 0, 0, 0);

        var fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
        return new Version(fileVersionInfo.FileVersion);
    }

    /// <summary>
    /// Determines whether the driver has been loaded.
    /// </summary>
    /// <returns><c>true</c> if the driver is loaded; otherwise, <c>false</c>.</returns>
    public bool IsDriverLoaded()
    {
        return Native.NdisApi.IsDriverLoaded(Handle);
    }

    /// <summary>
    /// Determines whether the driver is installed.
    /// </summary>
    /// <param name="componentId">The component identifier.</param>
    /// <returns><c>true</c> if the driver is installed; otherwise, <c>false</c>.</returns>
    public bool IsDriverInstalled(string componentId = "nt_ndisrd")
    {
        using var netCfg = new NetCfg(Path.GetTempPath());

        return netCfg.IsInstalled(componentId);
    }

    /// <summary>
    /// Uninstalls the driver.
    /// </summary>
    /// <param name="afterReboot">If set to <c>true</c>, requires a reboot for the changes to take affect.</param>
    /// <param name="errorCode">The error code.</param>
    /// <returns><c>true</c> if uninstalled, <c>false</c> otherwise.</returns>
    public bool UninstallDriver(out bool afterReboot, out uint errorCode)
    {
        return UninstallDriver("nt_ndisrd", out afterReboot, out errorCode);
    }

    /// <summary>
    /// Uninstalls the driver.
    /// </summary>
    /// <param name="componentId">The component identifier.</param>
    /// <param name="afterReboot">If set to <c>true</c>, requires a reboot for the changes to take affect.</param>
    /// <param name="errorCode">The error code.</param>
    /// <returns><c>true</c> if uninstalled, <c>false</c> otherwise.</returns>
    public bool UninstallDriver(string componentId, out bool afterReboot, out uint errorCode)
    {
        using var netCfg = new NetCfg(Path.GetTempPath());

        return netCfg.Uninstall(componentId, out afterReboot, out errorCode);
    }

    /// <summary>
    /// Installs the driver.
    /// </summary>
    /// <param name="rootPath">The path containing the driver files, this should contain the original folder structure.</param>
    /// <param name="afterReboot">If set to <c>true</c>, requires a reboot for the changes to take affect.</param>
    /// <param name="errorCode">The error code.</param>
    /// <returns><c>true</c> if installed, <c>false</c> otherwise.</returns>
    public bool InstallDriver(string rootPath, out bool afterReboot, out uint errorCode)
    {
        return InstallDriver(rootPath, "ndisrd_lwf.inf", "nt_ndisrd", out afterReboot, out errorCode);
    }

    /// <summary>
    /// Installs the driver.
    /// </summary>
    /// <param name="rootPath">The path containing the driver files, this should contain the original folder structure.</param>
    /// <param name="infFileName">Name of the inf file.</param>
    /// <param name="componentId">The component identifier.</param>
    /// <param name="afterReboot">If set to <c>true</c>, requires a reboot for the changes to take affect.</param>
    /// <param name="errorCode">The error code.</param>
    /// <returns><c>true</c> if installed, <c>false</c> otherwise.</returns>
    public bool InstallDriver(string rootPath, string infFileName, string componentId, out bool afterReboot, out uint errorCode)
    {
        var osName = string.Empty;
        if (OperatingSystem.IsWindows10OrGreater())
        {
            osName = "win10";
        }
        else if (OperatingSystem.IsWindows8())
        {
            osName = "win8";
        }
        else if (OperatingSystem.IsWindows7())
        {
            osName = "win7";
        }

        var architectures = Environment.Is64BitOperatingSystem ? new[] { "amd64", "x64" } : new[] { "i386", "x86" };
        string foundPath = null;

        foreach (var architecture in architectures)
        {
            // .\win10\amd64.
            var path = Path.Combine(rootPath, osName, architecture);
            if (Directory.Exists(path))
            {
                foundPath = path;
                break;
            }

            // .\amd64\win10.
            path = Path.Combine(rootPath, architecture, osName);
            if (Directory.Exists(path))
            {
                foundPath = path;
                break;
            }
        }

        if (foundPath != null)
        {
            using var netCfg = new NetCfg(foundPath);

            return netCfg.Install(infFileName, componentId, out afterReboot, out errorCode);
        }

        afterReboot = false;
        errorCode = uint.MaxValue;
        return false;
    }

    /// <summary>
    /// Installs the certificate for the driver.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns><c>true</c> if installed, <c>false</c> otherwise.</returns>
    public bool InstallCertificate(string path)
    {
        try
        {
            var x509Store = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);
            var x509Certificate = new X509Certificate2(path);

            x509Store.Open(OpenFlags.ReadWrite);
            x509Store.Add(x509Certificate);
            x509Store.Close();

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the bytes returned.
    /// </summary>
    /// <returns><see cref="uint" />.</returns>
    public uint GetBytesReturned()
    {
        return Native.NdisApi.GetBytesReturned(Handle);
    }

    /// <summary>
    /// Gets the MTU decrement.
    /// </summary>
    /// <returns><see cref="uint" />.</returns>
    public uint GetMtuDecrement()
    {
        return Native.NdisApi.GetMTUDecrement();
    }

    /// <summary>
    /// Sets the MTU decrement.
    /// </summary>
    /// <param name="mtuDecrement">The mtu decrement.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SetMtuDecrement(uint mtuDecrement)
    {
        return Native.NdisApi.SetMTUDecrement(mtuDecrement);
    }

    /// <summary>
    /// Gets the adapters startup mode.
    /// </summary>
    /// <returns><see cref="Native.NdisApi.MSTCP_FLAGS" />.</returns>
    public Native.NdisApi.MSTCP_FLAGS GetAdaptersStartupMode()
    {
        return Native.NdisApi.GetAdaptersStartupMode();
    }

    /// <summary>
    /// Gets the network adapters.
    /// </summary>
    /// <returns>The <see cref="NetworkAdapter" />s.</returns>
    public IEnumerable<NetworkAdapter> GetNetworkAdapters()
    {
        var adapterList = new Native.NdisApi.TCP_AdapterList();
        if (!Native.NdisApi.GetTcpipBoundAdaptersInfo(Handle, ref adapterList))
            yield break;

        for (var i = 0; i < adapterList.m_nAdapterCount; i++)
        {
            yield return new NetworkAdapter(adapterList.m_nAdapterHandle[i],
                                            adapterList.m_szAdapterNameList.Skip(i * Native.NdisApi.ADAPTER_NAME_SIZE).Take(Native.NdisApi.ADAPTER_NAME_SIZE).ToArray(),
                                            adapterList.m_nAdapterMediumList[i],
                                            adapterList.m_czCurrentAddress.Skip(i * Native.NdisApi.ETHER_ADDR_LENGTH).Take(Native.NdisApi.ETHER_ADDR_LENGTH).ToArray(),
                                            adapterList.m_usMTU[i],
                                            this);
        }
    }

    /// <summary>
    /// Gets the network adapters with all specified <see cref="flags" />.
    /// </summary>
    /// <param name="flags">The flags.</param>
    /// <returns><see cref="NetworkAdapter" />s.</returns>
    public IEnumerable<NetworkAdapter> GetNetworkAdapters(Native.NdisApi.MSTCP_FLAGS flags)
    {
        foreach (var networkAdapter in GetNetworkAdapters())
        {
            if (GetAdapterMode(networkAdapter, out var adapterMode) && adapterMode.dwFlags.Equals(flags))
                yield return networkAdapter;
        }
    }

    /// <summary>
    /// Gets the bound network adapters.
    /// </summary>
    /// <returns><see cref="NetworkAdapter" />s.</returns>
    public IEnumerable<NetworkAdapter> GetBoundNetworkAdapters()
    {
        foreach (var networkAdapter in GetNetworkAdapters())
        {
            if (GetAdapterMode(networkAdapter, out var adapterMode) && adapterMode.dwFlags != 0)
                yield return networkAdapter;
        }
    }

    /// <summary>
    /// Sets the adapter mode to the specified <see cref="flags" />.
    /// </summary>
    /// <param name="networkAdapter">The network adapter.</param>
    /// <param name="flags">The flags.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SetAdapterMode(NetworkAdapter networkAdapter, Native.NdisApi.MSTCP_FLAGS flags)
    {
        var adapterMode = new Native.NdisApi.ADAPTER_MODE { dwFlags = flags, hAdapterHandle = networkAdapter.Handle };
        return Native.NdisApi.SetAdapterMode(Handle, ref adapterMode);
    }

    /// <summary>
    /// Resets the adapter mode by clearing all set flags.
    /// </summary>
    /// <param name="networkAdapter">The network adapter.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool ResetAdapterMode(NetworkAdapter networkAdapter)
    {
        var adapterMode = new Native.NdisApi.ADAPTER_MODE { dwFlags = Native.NdisApi.MSTCP_FLAGS.MSTCP_FLAG_NONE, hAdapterHandle = networkAdapter.Handle };
        return Native.NdisApi.SetAdapterMode(Handle, ref adapterMode);
    }

    /// <summary>
    /// Sets the packet event to the specified <see cref="networkAdapter" />.
    /// </summary>
    /// <param name="networkAdapter">The network adapter.</param>
    /// <param name="waitHandle">The wait handle.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SetPacketEvent(NetworkAdapter networkAdapter, WaitHandle waitHandle)
    {
        return Native.NdisApi.SetPacketEvent(Handle, networkAdapter.Handle, waitHandle?.SafeWaitHandle);
    }

    /// <summary>
    /// Sets the packet event to the specified <see cref="networkAdapter" />.
    /// </summary>
    /// <remarks>This will not set the <see cref="networkAdapter" />'s wait handle.</remarks>
    /// <param name="networkAdapter">The network adapter.</param>
    /// <param name="safeWaitHandle">The safe wait handle.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SetPacketEvent(NetworkAdapter networkAdapter, SafeWaitHandle safeWaitHandle)
    {
        return Native.NdisApi.SetPacketEvent(Handle, networkAdapter.Handle, safeWaitHandle);
    }

    /// <summary>
    /// Sets the packet event to the specified <see cref="networkAdapter" />.
    /// </summary>
    /// <remarks>This will not set the <see cref="networkAdapter" />'s wait handle.</remarks>
    /// <param name="networkAdapter">The network adapter.</param>
    /// <param name="handle">The handle.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SetPacketEvent(NetworkAdapter networkAdapter, IntPtr handle)
    {
        return Native.NdisApi.SetPacketEvent(Handle, networkAdapter.Handle, handle);
    }

    /// <summary>
    /// Resets the currently set packet event.
    /// </summary>
    /// <param name="networkAdapter">The network adapter.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool ResetPacketEvent(NetworkAdapter networkAdapter)
    {
        return Native.NdisApi.SetPacketEvent(Handle, networkAdapter.Handle, IntPtr.Zero);
    }

    /// <summary>
    /// Sets the WAN event.
    /// </summary>
    /// <param name="safeWaitHandle">The safe wait handle.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SetWanEvent(SafeWaitHandle safeWaitHandle)
    {
        return Native.NdisApi.SetWANEvent(Handle, safeWaitHandle);
    }

    /// <summary>
    /// Sets the adapter list change event.
    /// </summary>
    /// <param name="safeWaitHandle">The safe wait handle.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SetAdapterListChangeEvent(SafeWaitHandle safeWaitHandle)
    {
        return Native.NdisApi.SetAdapterListChangeEvent(Handle, safeWaitHandle);
    }

    /// <summary>
    /// Reads the packet to the specified <see cref="ethRequest" />.
    /// </summary>
    /// <param name="ethRequest">The ether request.</param>
    /// <remarks>The adapter handle needs to be set.</remarks>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool ReadPacket(ref Native.NdisApi.ETH_REQUEST ethRequest)
    {
        return Native.NdisApi.ReadPacket(Handle, ref ethRequest);
    }

    /// <summary>
    /// Reads the packets to the specified <see cref="buffers" />.
    /// </summary>
    /// <param name="buffers">The buffers.</param>
    /// <param name="dwPacketsNum">The number of packets that can be read in the <see cref="buffers" />.</param>
    /// <param name="pdwPacketsSuccess">The number of read packets.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool ReadPacketsUnsorted(IntPtr[] buffers, uint dwPacketsNum, ref uint pdwPacketsSuccess)
    {
        return Native.NdisApi.ReadPacketsUnsorted(Handle, buffers, dwPacketsNum, ref pdwPacketsSuccess);
    }

    /// <summary>
    /// Reads the packets to the specified <see cref="ethMRequest" />.
    /// </summary>
    /// <param name="ethMRequest">The ether multiple request.</param>
    /// <remarks>The adapter handle needs to be set.</remarks>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool ReadPackets(Native.NdisApi.ETH_M_REQUEST* ethMRequest)
    {
        return Native.NdisApi.ReadPackets(Handle, ethMRequest);
    }

    /// <summary>
    /// Sends the packet to adapter.
    /// </summary>
    /// <param name="ethRequest">The ether request.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SendPacketToAdapter(ref Native.NdisApi.ETH_REQUEST ethRequest)
    {
        return Native.NdisApi.SendPacketToAdapter(Handle, ref ethRequest);
    }

    /// <summary>
    /// Sends the packet to MSTCP.
    /// </summary>
    /// <param name="ethRequest">The ether request.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SendPacketToMstcp(ref Native.NdisApi.ETH_REQUEST ethRequest)
    {
        return Native.NdisApi.SendPacketToMstcp(Handle, ref ethRequest);
    }

    /// <summary>
    /// Sends the packet to MSTCP or adapter.
    /// </summary>
    /// <param name="ethRequest">The ether request.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SendPacket(ref Native.NdisApi.ETH_REQUEST ethRequest)
    {
        var buffer = (Native.NdisApi.INTERMEDIATE_BUFFER_VARIABLE*) ethRequest.EthPacket.Buffer;
        if (buffer->m_dwDeviceFlags == Native.NdisApi.PACKET_FLAG.PACKET_FLAG_ON_SEND)
        {
            return SendPacketToAdapter(ref ethRequest);
        }

        return SendPacketToMstcp(ref ethRequest);
    }

    /// <summary>
    /// Sends the packets to the adapter.
    /// </summary>
    /// <param name="ethMRequest">The ether multiple request.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SendPacketsToAdapter(Native.NdisApi.ETH_M_REQUEST* ethMRequest)
    {
        return Native.NdisApi.SendPacketsToAdapter(Handle, ethMRequest);
    }

    /// <summary>
    /// Sends the packets to adapters.
    /// </summary>
    /// <param name="buffers">The buffers.</param>
    /// <param name="dwPacketsNum">The number of packets that should be send from the <see cref="buffers" />.</param>
    /// <param name="pdwPacketsSuccess">The number of send packets.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SendPacketsToAdaptersUnsorted
        (IntPtr[] buffers, uint dwPacketsNum, ref uint pdwPacketsSuccess)
    {
        return Native.NdisApi.SendPacketsToAdaptersUnsorted(Handle, buffers, dwPacketsNum, ref pdwPacketsSuccess);
    }

    /// <summary>
    /// Sends the packets to MSTCP.
    /// </summary>
    /// <param name="ethMRequest">The ether multiple request.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SendPacketsToMstcp(Native.NdisApi.ETH_M_REQUEST* ethMRequest)
    {
        return Native.NdisApi.SendPacketsToMstcp(Handle, ethMRequest);
    }

    /// <summary>
    /// Sends the packets to MSTCP.
    /// </summary>
    /// <param name="buffers">The buffers.</param>
    /// <param name="dwPacketsNum">The number of packets that should be send from the <see cref="buffers" />.</param>
    /// <param name="pdwPacketsSuccess">The number of sent packets.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SendPacketsToMstcpUnsorted(IntPtr[] buffers, uint dwPacketsNum, ref uint pdwPacketsSuccess)
    {
        return Native.NdisApi.SendPacketsToMstcpUnsorted(Handle, buffers, dwPacketsNum, ref pdwPacketsSuccess);
    }

    /// <summary>
    /// Sends the packets.
    /// </summary>
    /// <param name="buffers">The buffers.</param>
    /// <param name="dwPacketsNum">The number of packets that should be send from the <see cref="buffers" />.</param>
    /// <param name="pdwPacketsSuccess">The number of sent packets.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SendPacketsUnsorted(IntPtr[] buffers, uint dwPacketsNum, out uint pdwPacketsSuccess)
    {
        var arrayPool = _arrayPool;

        var mstcpBuffers = arrayPool.Rent((int) dwPacketsNum);
        var adapterBuffers = arrayPool.Rent((int) dwPacketsNum);

        var mstcpBufferCount = 0u;
        var adapterBufferCount = 0u;

        for (int i = 0; i < dwPacketsNum; i++)
        {
            var buffer = (Native.NdisApi.INTERMEDIATE_BUFFER_VARIABLE*) buffers[i];

            if (buffer->m_dwDeviceFlags == Native.NdisApi.PACKET_FLAG.PACKET_FLAG_ON_SEND)
                adapterBuffers[adapterBufferCount++] = buffers[i];
            else
                mstcpBuffers[mstcpBufferCount++] = buffers[i];
        }

        var success = true;
        var packetsSuccess = 0u;

        if (adapterBufferCount > 0)
            success = SendPacketsToAdaptersUnsorted(adapterBuffers, adapterBufferCount, ref packetsSuccess);

        if (mstcpBufferCount > 0)
        {
            var previousPacketsSuccess = packetsSuccess;
            success = SendPacketsToMstcpUnsorted(mstcpBuffers, mstcpBufferCount, ref packetsSuccess) & success;
            packetsSuccess += previousPacketsSuccess;
        }

        pdwPacketsSuccess = packetsSuccess;

        arrayPool.Return(mstcpBuffers);
        arrayPool.Return(adapterBuffers);

        return success;
    }

    /// <summary>
    /// Sends the packets to MSTCP or adapter.
    /// </summary>
    /// <param name="ethMRequest">The ether multiple request.</param>
    /// <param name="packets">The optional packets.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SendPackets(Native.NdisApi.ETH_M_REQUEST* ethMRequest, Native.NdisApi.NDISRD_ETH_Packet[] packets = null)
    {
        lock (_lock)
        {
            _ethPacketsToAdapter->dwPacketsSuccess = 0;
            _ethPacketsToMstcp->dwPacketsSuccess = 0;
            _ethPacketsToAdapter->dwPacketsNumber = 0;
            _ethPacketsToMstcp->dwPacketsNumber = 0;
            _ethPacketsToAdapter->hAdapterHandle = ethMRequest->hAdapterHandle;
            _ethPacketsToMstcp->hAdapterHandle = ethMRequest->hAdapterHandle;

            packets ??= ethMRequest->GetPackets();

            foreach (var packet in packets)
            {
                var buffer = (Native.NdisApi.INTERMEDIATE_BUFFER_VARIABLE*) packet.Buffer;

                if (buffer->m_dwDeviceFlags == Native.NdisApi.PACKET_FLAG.PACKET_FLAG_ON_SEND)
                    _ethPacketsToAdapter->Packets[_ethPacketsToAdapter->dwPacketsNumber++].Buffer = packet.Buffer;
                else
                    _ethPacketsToMstcp->Packets[_ethPacketsToMstcp->dwPacketsNumber++].Buffer = packet.Buffer;
            }

            var success = true;

            if (_ethPacketsToMstcp->dwPacketsNumber > 0)
                success = SendPacketsToMstcp(_ethPacketsToMstcp);

            if (_ethPacketsToAdapter->dwPacketsNumber > 0)
                success = SendPacketsToAdapter(_ethPacketsToAdapter) && success;

            return success;
        }
    }

    /// <summary>
    /// Flushes the adapter packet queue.
    /// </summary>
    /// <param name="networkAdapter">The network adapter.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool FlushAdapterPacketQueue(NetworkAdapter networkAdapter)
    {
        return Native.NdisApi.FlushAdapterPacketQueue(Handle, networkAdapter.Handle);
    }

    /// <summary>
    /// Gets the size of the adapter packet queue.
    /// </summary>
    /// <param name="networkAdapter">The network adapter.</param>
    /// <returns>The queue size if valid, <c>-1</c> otherwise.</returns>
    public int GetAdapterPacketQueueSize(NetworkAdapter networkAdapter)
    {
        uint size = 0;
        if (Native.NdisApi.GetAdapterPacketQueueSize(Handle, networkAdapter.Handle, ref size))
            return (int) size;

        return -1;
    }

    /// <summary>
    /// Performs a query or set operation on the adapter provided by <see cref="packetOidData" />,
    /// </summary>
    /// <param name="packetOidData">The packet oid data.</param>
    /// <param name="set">If set to <c>true</c>, sets the packet oid data.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool NdisrdRequest(ref Native.NdisApi.PACKET_OID_DATA packetOidData, bool set)
    {
        return Native.NdisApi.NdisrdRequest(Handle, ref packetOidData, set);
    }

    /// <summary>
    /// Performs a query or set operation on the adapter provided by <see cref="packetOidData" />,
    /// </summary>
    /// <param name="packetOidData">The packet oid data.</param>
    /// <param name="set">If set to <c>true</c>, sets the data of <paramref name="packetOidData" />.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool NdisrdRequest(Native.NdisApi.PACKET_OID_DATA* packetOidData, bool set)
    {
        return Native.NdisApi.NdisrdRequest(Handle, packetOidData, set);
    }

    /// <summary>
    /// Initializes the fast i/o.
    /// </summary>
    /// <param name="fastIOSection">The fast i/o section.</param>
    /// <param name="size">The size.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool InitializeFastIo(Native.NdisApi.FAST_IO_SECTION* fastIOSection, uint size)
    {
        return Native.NdisApi.InitializeFastIo(Handle, fastIOSection, size);
    }

    /// <summary>
    /// Adds the secondary fast i/o.
    /// </summary>
    /// <param name="fastIOSection">The fast i/o section.</param>
    /// <param name="size">The size.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool AddSecondaryFastIo(Native.NdisApi.FAST_IO_SECTION* fastIOSection, uint size)
    {
        return Native.NdisApi.AddSecondaryFastIo(Handle, fastIOSection, size);
    }

    /// <summary>
    /// Initializes the fast i/o. If successful, this will set <see cref="_fastIOSection" /> to the underlying <see cref="Native.NdisApi.FAST_IO_SECTION" />.
    /// </summary>
    /// <param name="numberOfPackets">The number of packets.</param>
    /// <param name="fastIOSection">The resulting fast i/o section.</param>
    /// <param name="secondaryFastIOSection">The resulting secondary fast i/o section.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentException">Cannot reinitialize fast i/o, you need to create a new instance.</exception>
    public bool InitializeFastIo(int numberOfPackets, out Native.NdisApi.FAST_IO_SECTION* fastIOSection, out Native.NdisApi.FAST_IO_SECTION* secondaryFastIOSection)
    {
        var initialized = InitializeFastIo(numberOfPackets, 1, out fastIOSection, out var secondaryFastIOSections);
        secondaryFastIOSection = initialized ? secondaryFastIOSections[0] : null;
        return initialized;
    }

    /// <summary>
    /// Initializes the fast i/o. If successful, this will set <see cref="_fastIOSection" /> to the underlying <see cref="Native.NdisApi.FAST_IO_SECTION" />.
    /// </summary>
    /// <param name="numberOfPackets">The number of packets.</param>
    /// <param name="numberOfSecondaryFastIOSections">The number of secondary fast i/o sections.</param>
    /// <param name="fastIOSection">The resulting fast i/o section.</param>
    /// <param name="secondaryFastIOSections">The secondary fast i/o sections.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentException">Cannot reinitialize fast i/o, you need to create a new instance.</exception>
    public bool InitializeFastIo
    (
        int numberOfPackets,
        int numberOfSecondaryFastIOSections,
        out Native.NdisApi.FAST_IO_SECTION* fastIOSection,
        out Native.NdisApi.FAST_IO_SECTION*[] secondaryFastIOSections)
    {
        if (numberOfSecondaryFastIOSections < 1 || numberOfSecondaryFastIOSections > 15)
            throw new ArgumentException("The number of secondary fast i/o sections must be between 1 and 15.");

        if (_fastIOSection != null)
        {
            if (_fastIOSectionNumberOfPackets != numberOfPackets || _secondaryFastIOSections.Length != numberOfSecondaryFastIOSections)
                throw new ArgumentException("Cannot reinitialize fast i/o, you need to create a new instance.");

            fastIOSection = _fastIOSection;
            secondaryFastIOSections = _secondaryFastIOSections;
            return true;
        }

        var fastIOSize = (uint) (Native.NdisApi.FAST_IO_SECTION.SizeOfHeader + (IntermediateBufferSize * numberOfPackets));

        var fastIOSectionPointer = Marshal.AllocHGlobal((int) fastIOSize);
        Kernel32.ZeroMemory(fastIOSectionPointer, (int) fastIOSize); // This must be set to zero.

        fastIOSection = (Native.NdisApi.FAST_IO_SECTION*) fastIOSectionPointer;

        if (InitializeFastIo(fastIOSection, fastIOSize))
        {
            secondaryFastIOSections = new Native.NdisApi.FAST_IO_SECTION*[numberOfSecondaryFastIOSections];

            for (int i = 0; i < numberOfSecondaryFastIOSections; i++)
            {
                var secondaryFastIOSectionPointer = Marshal.AllocHGlobal((int) fastIOSize);
                Kernel32.ZeroMemory(secondaryFastIOSectionPointer, (int) fastIOSize); // This must be set to zero.

                var secondaryFastIOSection = (Native.NdisApi.FAST_IO_SECTION*) secondaryFastIOSectionPointer;

                if (AddSecondaryFastIo(secondaryFastIOSection, fastIOSize))
                {
                    secondaryFastIOSections[i] = secondaryFastIOSection;
                }
                else
                {
                    for (int j = 0; j < i; j++)
                        Marshal.FreeHGlobal((IntPtr) secondaryFastIOSections[j]);

                    Marshal.FreeHGlobal(fastIOSectionPointer);

                    fastIOSection = null;
                    secondaryFastIOSections = null;

                    return false;
                }
            }

            _fastIOSection = fastIOSection;
            _secondaryFastIOSections = secondaryFastIOSections;
            _fastIOSectionNumberOfPackets = numberOfPackets;

            return true;
        }

        Marshal.FreeHGlobal(fastIOSectionPointer);

        fastIOSection = null;
        secondaryFastIOSections = null;

        return false;
    }

    /// <summary>
    /// Sets the hardware packet filter.
    /// </summary>
    /// <param name="networkAdapter">The network adapter.</param>
    /// <param name="ndisPacketType">Type of the ndis packet.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SetHwPacketFilter(NetworkAdapter networkAdapter, Native.NdisApi.NDIS_PACKET_TYPE ndisPacketType)
    {
        return Native.NdisApi.SetHwPacketFilter(Handle, networkAdapter.Handle, ndisPacketType);
    }

    /// <summary>
    /// Get the hardware packet filter.
    /// </summary>
    /// <param name="networkAdapter">The network adapter.</param>
    /// <param name="hwPacketType">The resulting hardware packet filter.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool GetHwPacketFilter(NetworkAdapter networkAdapter, out Native.NdisApi.NDIS_PACKET_TYPE hwPacketType)
    {
        hwPacketType = Native.NdisApi.NDIS_PACKET_TYPE.NDIS_PACKET_TYPE_NONE;
        return Native.NdisApi.GetHwPacketFilter(Handle, networkAdapter.Handle, ref hwPacketType);
    }

    /// <summary>
    /// Sets the packet filter table.
    /// </summary>
    /// <param name="filterTable">The filter table.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SetPacketFilterTable(Native.NdisApi.STATIC_FILTER_TABLE filterTable)
    {
        return Native.NdisApi.SetPacketFilterTable(Handle, &filterTable);
    }

    /// <summary>
    /// Gets the packet filter table.
    /// </summary>
    /// <param name="staticFilterTable">The resulting filter table.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool GetPacketFilterTable(out Native.NdisApi.STATIC_FILTER_TABLE staticFilterTable)
    {
        var tableSize = GetPacketFilterTableSize();
        if (tableSize == -1)
        {
            staticFilterTable = default;
            return false;
        }

        var newStaticFilterTable = CreateStaticFilterTable((uint) tableSize);
        var success = Native.NdisApi.GetPacketFilterTable(Handle, newStaticFilterTable);

        staticFilterTable = *newStaticFilterTable;
        DisposeObject(newStaticFilterTable);

        return success;
    }

    /// <summary>
    /// Sets the adapters startup mode.
    /// </summary>
    /// <param name="flags">The flags.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool SetAdaptersStartupMode(Native.NdisApi.MSTCP_FLAGS flags)
    {
        return Native.NdisApi.SetAdaptersStartupMode(flags);
    }

    /// <summary>
    /// Gets the adapter mode.
    /// </summary>
    /// <param name="networkAdapter">The network adapter.</param>
    /// <param name="adapterMode">The adapter mode.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool GetAdapterMode(NetworkAdapter networkAdapter, out Native.NdisApi.ADAPTER_MODE adapterMode)
    {
        adapterMode = new Native.NdisApi.ADAPTER_MODE { hAdapterHandle = networkAdapter.Handle };
        return Native.NdisApi.GetAdapterMode(Handle, ref adapterMode);
    }

    /// <summary>
    /// Gets the size of the packet filter table.
    /// </summary>
    /// <returns>The filter table size if valid, <c>-1</c> otherwise.</returns>
    public int GetPacketFilterTableSize()
    {
        uint tableSize = 0;
        if (Native.NdisApi.GetPacketFilterTableSize(Handle, ref tableSize))
            return (int) tableSize;

        return -1;
    }

    /// <summary>
    /// Gets the packet filter table reset stats.
    /// </summary>
    /// <param name="staticFilterTable">The resulting filter table.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool GetPacketFilterTableResetStats(out Native.NdisApi.STATIC_FILTER_TABLE staticFilterTable)
    {
        var tableSize = GetPacketFilterTableSize();
        if (tableSize == -1)
        {
            staticFilterTable = default;
            return false;
        }

        var staticFilterTableTemp = CreateStaticFilterTable((uint) tableSize);
        var success = Native.NdisApi.GetPacketFilterTableResetStats(Handle, staticFilterTableTemp);

        staticFilterTable = *staticFilterTableTemp;
        DisposeObject(staticFilterTableTemp);

        return success;
    }

    /// <summary>
    /// Gets the ras links of the specified <see cref="networkAdapter" />.
    /// </summary>
    /// <param name="networkAdapter">The network adapter.</param>
    /// <returns><see cref="Native.NdisApi.RAS_LINKS" />.</returns>
    public Native.NdisApi.RAS_LINKS GetRasLinks(NetworkAdapter networkAdapter)
    {
        var rasLinksPtr = Marshal.AllocHGlobal(Native.NdisApi.RAS_LINKS.Size);

        try
        {
            var result = Native.NdisApi.GetRasLinks(Handle, networkAdapter.Handle, rasLinksPtr);
            return result
                ? Marshal.PtrToStructure<Native.NdisApi.RAS_LINKS>(rasLinksPtr)
                : default;
        }
        finally
        {
            Marshal.FreeHGlobal(rasLinksPtr);
        }
    }

    /// <summary>
    /// Resets the packet filter table.
    /// </summary>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool ResetPacketFilterTable()
    {
        return Native.NdisApi.ResetPacketFilterTable(Handle);
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
    /// Creates a new <see cref="Native.NdisApi.INTERMEDIATE_BUFFER" />.
    /// </summary>
    /// <returns><see cref="Native.NdisApi.INTERMEDIATE_BUFFER" />.</returns>
    public Native.NdisApi.INTERMEDIATE_BUFFER* CreateIntermediateBuffer()
    {
        return (Native.NdisApi.INTERMEDIATE_BUFFER*) _pinnedManagedArrayAllocator.AllocateArray(IntermediateBufferSize);
    }

    /// <summary>
    /// Creates a new <see cref="Native.NdisApi.INTERMEDIATE_BUFFER" />.
    /// </summary>
    /// <returns><see cref="Native.NdisApi.INTERMEDIATE_BUFFER" />.</returns>
    public Native.NdisApi.INTERMEDIATE_BUFFER_VARIABLE* CreateVariableIntermediateBuffer()
    {
        return (Native.NdisApi.INTERMEDIATE_BUFFER_VARIABLE*) _pinnedManagedArrayAllocator.AllocateArray(IntermediateBufferSize);
    }

    /// <summary>
    /// Creates a new <see cref="Native.NdisApi.ETH_REQUEST" />.
    /// </summary>
    /// <remarks>The adapter handle still needs to be set.</remarks>
    /// <returns><see cref="Native.NdisApi.ETH_REQUEST" />.</returns>
    public Native.NdisApi.ETH_REQUEST* CreateEthRequest()
    {
        var ethRequest = (Native.NdisApi.ETH_REQUEST*) _pinnedManagedArrayAllocator.AllocateArray(Native.NdisApi.ETH_REQUEST.Size);
        ethRequest->EthPacket.Buffer = _pinnedManagedArrayAllocator.AllocateArray(IntermediateBufferSize);
        return ethRequest;
    }

    /// <summary>
    /// Creates a new <see cref="Native.NdisApi.ETH_M_REQUEST" />.
    /// </summary>
    /// <param name="packetCount">The number of <see cref="Native.NdisApi.NDISRD_ETH_Packet" />s.</param>
    /// <returns><see cref="Native.NdisApi.ETH_M_REQUEST" />.</returns>
    /// <remarks>The adapter handle still needs to be set.</remarks>
    public Native.NdisApi.ETH_M_REQUEST* CreateEthMRequest(uint packetCount)
    {
        var pinnedPtr = _pinnedManagedArrayAllocator.AllocateArray(Native.NdisApi.ETH_M_REQUEST.SizeOfHeader + (int) (packetCount * Native.NdisApi.NDISRD_ETH_Packet.Size));

        var ethMRequest = (Native.NdisApi.ETH_M_REQUEST*) pinnedPtr;
        ethMRequest->dwPacketsNumber = packetCount;

        var ndisrdEthPackets = new Native.NdisApi.NDISRD_ETH_Packet[packetCount];

        for (int i = 0; i < packetCount; i++)
        {
            ndisrdEthPackets[i] = new Native.NdisApi.NDISRD_ETH_Packet
            {
                Buffer = _pinnedManagedArrayAllocator.AllocateArray(IntermediateBufferSize)
            };
        }

        ethMRequest->SetPackets(ndisrdEthPackets);

        return ethMRequest;
    }

    /// <summary>
    /// Creates a new <see cref="Native.NdisApi.FAST_IO_SECTION" />.
    /// </summary>
    /// <param name="intermediateBufferCount">The number of intermediate buffers.</param>
    /// <returns><see cref="Native.NdisApi.FAST_IO_SECTION" />.</returns>
    public Native.NdisApi.FAST_IO_SECTION* CreateFastIOSection(uint intermediateBufferCount)
    {
        return (Native.NdisApi.FAST_IO_SECTION*) _pinnedManagedArrayAllocator.AllocateArray(Native.NdisApi.FAST_IO_SECTION_HEADER.Size + (int) (intermediateBufferCount * IntermediateBufferSize));
    }

    /// <summary>
    /// Creates a new <see cref="Native.NdisApi.STATIC_FILTER_TABLE" />.
    /// </summary>
    /// <param name="filterCount">The number of static filters.</param>
    /// <returns><see cref="Native.NdisApi.STATIC_FILTER_TABLE" />.</returns>
    public Native.NdisApi.STATIC_FILTER_TABLE* CreateStaticFilterTable(uint filterCount)
    {
        var pinnedPtr = _pinnedManagedArrayAllocator.AllocateArray(Native.NdisApi.STATIC_FILTER_TABLE.SizeOfHeader + (int) (filterCount * Native.NdisApi.STATIC_FILTER.Size));

        var staticFilterTable = (Native.NdisApi.STATIC_FILTER_TABLE*) pinnedPtr;
        staticFilterTable->m_TableSize = filterCount;

        var staticFilters = new Native.NdisApi.STATIC_FILTER[filterCount];
        for (int i = 0; i < filterCount; i++)
            staticFilters[i] = new Native.NdisApi.STATIC_FILTER();

        staticFilterTable->SetStaticFilters(staticFilters);

        return staticFilterTable;
    }

    /// <summary>
    /// Clones the specified object.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    public void CloneObject(Native.NdisApi.INTERMEDIATE_BUFFER* source, Native.NdisApi.INTERMEDIATE_BUFFER* destination)
    {
        Kernel32.CopyMemory((IntPtr) destination, (IntPtr) source, (uint) IntermediateBufferSize);
    }

    /// <summary>
    /// Clones the specified object.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    public void CloneObject(Native.NdisApi.INTERMEDIATE_BUFFER_VARIABLE* source, Native.NdisApi.INTERMEDIATE_BUFFER_VARIABLE* destination)
    {
        Kernel32.CopyMemory((IntPtr) destination, (IntPtr) source, (uint) IntermediateBufferSize);
    }

    /// <summary>
    /// Clones the specified <see cref="source" />.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns><see cref="Native.NdisApi.ETH_REQUEST" />.</returns>
    public Native.NdisApi.ETH_REQUEST* CloneObject(Native.NdisApi.ETH_REQUEST* source)
    {
        var destination = CreateEthRequest();
        CloneObject(source, destination);

        return destination;
    }

    /// <summary>
    /// Clones the specified object.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    public void CloneObject(Native.NdisApi.ETH_REQUEST* source, Native.NdisApi.ETH_REQUEST* destination)
    {
        destination->hAdapterHandle = source->hAdapterHandle;

        Kernel32.CopyMemory(destination->EthPacket.Buffer, source->EthPacket.Buffer, (uint) IntermediateBufferSize);
    }

    /// <summary>
    /// Clones the specified <see cref="source" />.
    /// </summary>
    /// <param name="source">The request.</param>
    /// <returns><see cref="Native.NdisApi.ETH_M_REQUEST" />.</returns>
    public Native.NdisApi.ETH_M_REQUEST* CloneObject(Native.NdisApi.ETH_M_REQUEST* source)
    {
        var destination = CreateEthMRequest(source->dwPacketsNumber);
        CloneObject(source, destination);

        return destination;
    }

    /// <summary>
    /// Clones the specified object.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    public void CloneObject(Native.NdisApi.ETH_M_REQUEST* source, Native.NdisApi.ETH_M_REQUEST* destination)
    {
        destination->hAdapterHandle = source->hAdapterHandle;
        destination->dwPacketsSuccess = source->dwPacketsSuccess;
        destination->dwPacketsNumber = source->dwPacketsNumber;

        var requestPackets = source->GetPackets();
        var nextPackets = destination->GetPackets();

        for (int i = 0; i < source->dwPacketsNumber; i++)
            Kernel32.CopyMemory(nextPackets[i].Buffer, requestPackets[i].Buffer, (uint) IntermediateBufferSize);
    }

    /// <summary>
    /// Disposes the specified <see cref="ptr" />.
    /// </summary>
    /// <param name="ptr">The pointer.</param>
    public void DisposeObject(IntPtr ptr)
    {
        _pinnedManagedArrayAllocator.FreeArray(ptr);
    }

    /// <summary>
    /// Disposes the specified <see cref="buffer" />.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    public void DisposeObject(Native.NdisApi.INTERMEDIATE_BUFFER* buffer)
    {
        _pinnedManagedArrayAllocator.FreeArray((IntPtr) buffer);
    }

    /// <summary>
    /// Disposes the specified <see cref="buffer" />.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    public void DisposeObject(Native.NdisApi.INTERMEDIATE_BUFFER_VARIABLE* buffer)
    {
        _pinnedManagedArrayAllocator.FreeArray((IntPtr) buffer);
    }

    /// <summary>
    /// Disposes the specified <see cref="request" />.
    /// </summary>
    /// <param name="request">The request.</param>
    public void DisposeObject(Native.NdisApi.ETH_REQUEST* request)
    {
        _pinnedManagedArrayAllocator.FreeArray(request->EthPacket.Buffer);
        _pinnedManagedArrayAllocator.FreeArray((IntPtr) request);
    }

    /// <summary>
    /// Disposes the specified <see cref="request" />.
    /// </summary>
    /// <param name="request">The request.</param>
    public void DisposeObject(Native.NdisApi.ETH_M_REQUEST* request)
    {
        var packets = request->GetPackets(Math.Max(request->dwPacketsNumber, request->dwPacketsSuccess));

        for (int i = 0; i < packets.Length; i++)
        {
            if (packets[i].Buffer != IntPtr.Zero)
                _pinnedManagedArrayAllocator.FreeArray(packets[i].Buffer);
        }

        _pinnedManagedArrayAllocator.FreeArray((IntPtr) request);
    }

    /// <summary>
    /// Disposes the specified <see cref="staticFilterTable" />.
    /// </summary>
    /// <param name="staticFilterTable">The object.</param>
    public void DisposeObject(Native.NdisApi.STATIC_FILTER_TABLE* staticFilterTable)
    {
        _pinnedManagedArrayAllocator.FreeArray((IntPtr) staticFilterTable);
    }

    /// <summary>
    /// Zeroes the data of the specified <see cref="buffer" />.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    public void ZeroObject(Native.NdisApi.INTERMEDIATE_BUFFER* buffer)
    {
        Kernel32.ZeroMemory((IntPtr) buffer, IntermediateBufferSize);
    }

    /// <summary>
    /// Zeroes the data of the specified <see cref="buffer" />.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    public void ZeroObject(Native.NdisApi.INTERMEDIATE_BUFFER_VARIABLE* buffer)
    {
        Kernel32.ZeroMemory((IntPtr) buffer, IntermediateBufferSize);
    }

    /// <summary>
    /// Zeroes the data of the specified <see cref="request" />.
    /// </summary>
    /// <param name="request">The request.</param>
    public void ZeroObject(Native.NdisApi.ETH_REQUEST* request)
    {
        request->hAdapterHandle = IntPtr.Zero;

        Kernel32.ZeroMemory(request->EthPacket.Buffer, IntermediateBufferSize);
    }

    /// <summary>
    /// Zeroes the data of the specified <see cref="request" />.
    /// </summary>
    /// <param name="request">The request.</param>
    public void ZeroObject(Native.NdisApi.ETH_M_REQUEST* request)
    {
        request->hAdapterHandle = IntPtr.Zero;
        request->dwPacketsSuccess = 0;
        request->dwPacketsNumber = 0;

        Kernel32.ZeroMemory((IntPtr) request + (int) Native.NdisApi.ETH_M_REQUEST.PacketsOffset, (int) (IntermediateBufferSize * request->dwPacketsNumber));
    }

    /// <summary>
    /// Determines whether the NdisApi DLL exists.
    /// </summary>
    /// <returns><c>true</c> if it exists; otherwise, <c>false</c>.</returns>
    private static bool NdisApiDllExists() => File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ndisapi.dll"));
}