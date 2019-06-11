// ----------------------------------------------
// <copyright file="NdisApi.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace NdisApiDotNet
{
    public class NdisApi : IDisposable
    {
        private readonly byte[] _driverNameBytes;
        private readonly object _lock = new object();
        private readonly NdisApiHelper _ndisApiHelper;
        private Native.NdisApi.ETH_M_REQUEST _ethPacketsToAdapter;
        private Native.NdisApi.ETH_M_REQUEST _ethPacketsToMstcp;

        /// <summary>
        /// Initializes a new instance of the <see cref="NdisApi" /> class.
        /// </summary>
        /// <param name="handle">The filter driver handle.</param>
        protected NdisApi(SafeFilterDriverHandle handle)
        {
            Handle = handle;

            _ndisApiHelper = new NdisApiHelper();
            _ethPacketsToMstcp = _ndisApiHelper.CreateEthMRequest();
            _ethPacketsToAdapter = _ndisApiHelper.CreateEthMRequest();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NdisApi" /> class.
        /// </summary>
        /// <param name="handle">The filter driver handle.</param>
        /// <param name="driverNameBytes">The driver name bytes.</param>
        protected NdisApi(SafeFilterDriverHandle handle, byte[] driverNameBytes)
        {
            _driverNameBytes = driverNameBytes;
            Handle = handle;

            _ndisApiHelper = new NdisApiHelper();
            _ethPacketsToMstcp = _ndisApiHelper.CreateEthMRequest();
            _ethPacketsToAdapter = _ndisApiHelper.CreateEthMRequest();
        }


        /// <summary>
        /// Gets the handle to the filter driver.
        /// </summary>
        public SafeFilterDriverHandle Handle { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the instance can be used.
        /// </summary>
        /// <remarks>
        /// This only checks whether the DLL has been opened, you should probably also check if the driver has been loaded.
        /// </remarks>
        public bool IsValid => !Handle.IsInvalid;

        /// <summary>
        /// Gets the maximum size of a packet in bytes.
        /// </summary>
        public uint MaxPacketSize => Native.NdisApi.MAX_ETHER_FRAME;

        /// <inheritdoc />
        public virtual void Dispose()
        {
            Handle?.Dispose();
            _ndisApiHelper.Dispose();
        }

        /// <summary>
        /// Opens the filter driver.
        /// </summary>
        /// <param name="driverName">The name of the driver.</param>
        /// <returns><see cref="NdisApi" />.</returns>
        /// <exception cref="Exception">Missing NDIS DLL</exception>
        public static NdisApi Open(string driverName = "NDISRD")
        {
            if (!NdisApiDllExists())
                throw new Exception("Missing NDIS DLL");


            var driverNameBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(driverName);
            var handle = Native.NdisApi.OpenFilterDriver(driverNameBytes);
            return new NdisApi(handle, driverNameBytes);
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
        /// <returns>System.UInt32.</returns>
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
            using (var snetCfg = new NetCfg(""))
            {
                return snetCfg.IsInstalled(componentId);
            }
        }

        /// <summary>
        /// Uninstalls the driver.
        /// </summary>
        /// <param name="afterReboot">if set to <c>true</c>, requires a reboot for the changes to take affect.</param>
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
        /// <param name="afterReboot">if set to <c>true</c>, requires a reboot for the changes to take affect.</param>
        /// <param name="errorCode">The error code.</param>
        /// <returns><c>true</c> if uninstalled, <c>false</c> otherwise.</returns>
        public bool UninstallDriver(string componentId, out bool afterReboot, out uint errorCode)
        {
            using (var snetCfg = new NetCfg(""))
            {
                return snetCfg.Uninstall(componentId, out afterReboot, out errorCode);
            }
        }

        /// <summary>
        /// Installs the driver.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="afterReboot">if set to <c>true</c>, requires a reboot for the changes to take affect.</param>
        /// <param name="errorCode">The error code.</param>
        /// <returns><c>true</c> if installed, <c>false</c> otherwise.</returns>
        public bool InstallDriver(string rootPath, out bool afterReboot, out uint errorCode)
        {
            return InstallDriver(rootPath, "ndisrd_lwf.inf", "nt_ndisrd", out afterReboot, out errorCode);
        }

        /// <summary>
        /// Installs the driver.
        /// </summary>
        /// <param name="rootPath">The root path, this should contain the original folder structure.</param>
        /// <param name="infFileName">Name of the inf file.</param>
        /// <param name="componentId">The component identifier.</param>
        /// <param name="afterReboot">if set to <c>true</c>, requires a reboot for the changes to take affect.</param>
        /// <param name="errorCode">The error code.</param>
        /// <returns><c>true</c> if installed, <c>false</c> otherwise.</returns>
        public bool InstallDriver(string rootPath, string infFileName, string componentId, out bool afterReboot, out uint errorCode)
        {
            var subFolder = "";
            if (OperatingSystem.IsWin10())
                subFolder = "win10";
            else if (OperatingSystem.IsWin8() || OperatingSystem.IsWin81())
                subFolder = "win8";
            else if (OperatingSystem.IsWin7())
                subFolder = "win7";
            else if (OperatingSystem.IsWinVista())
                subFolder = "vista";

            var architecture = Environment.Is64BitOperatingSystem ? "amd64" : "i386";
            var path = Path.Combine(Path.Combine(rootPath, subFolder), architecture);
            using (var snetCfg = new NetCfg(path))
            {
                return snetCfg.Install(infFileName, componentId, out afterReboot, out errorCode);
            }
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
                var x509Certificate = new X509Certificate2(path);

                var x509Store = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);
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
        /// Gets the MTU decrement.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        public uint GetMtuDecrement()
        {
            return Native.NdisApi.GetMTUDecrement();
        }

        /// <summary>
        /// Gets the bytes returned.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        public uint GetBytesReturned()
        {
            return Native.NdisApi.GetBytesReturned(Handle);
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
        /// <returns>System.UInt32.</returns>
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
            Native.NdisApi.GetTcpipBoundAdaptersInfo(Handle, ref adapterList);

            for (var i = 0; i < adapterList.m_nAdapterCount; i++)
            {
                yield return new NetworkAdapter(adapterList.AdapterHandle[i],
                                                adapterList.AdapterNames.Skip(i * Native.NdisApi.ADAPTER_NAME_SIZE).Take(Native.NdisApi.ADAPTER_NAME_SIZE).ToArray(),
                                                adapterList.AdapterMediums[i],
                                                adapterList.CurrentAddress.Skip(i * Native.NdisApi.ETHER_ADDR_LENGTH).Take(Native.NdisApi.ETHER_ADDR_LENGTH).ToArray(),
                                                adapterList.MTU[i]);
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
                if (GetAdapterMode(networkAdapter).Flags.Equals(flags))
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
                if (GetAdapterMode(networkAdapter).Flags != 0)
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
            var adapterMode = new Native.NdisApi.ADAPTER_MODE { dwFlags = 0, hAdapterHandle = networkAdapter.Handle };
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
            var success = Native.NdisApi.SetPacketEvent(Handle, networkAdapter.Handle, waitHandle.SafeWaitHandle);
            if (success)
                networkAdapter.WaitHandle = waitHandle;

            return success;
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
        /// Reads the packets to the specified <see cref="ethMRequest" />.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <remarks>The adapter handle needs to be set.</remarks>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool ReadPackets(ref Native.NdisApi.ETH_M_REQUEST ethMRequest)
        {
            return Native.NdisApi.ReadPackets(Handle, ref ethMRequest);
        }

        /// <summary>
        /// Reads the packets to the specified <see cref="ethMRequest" />.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <remarks>The adapter handle needs to be set.</remarks>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public unsafe bool ReadPackets(Native.NdisApi.ETH_M_REQUEST_U* ethMRequest)
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
            unsafe
            {
                var buffer = (Native.NdisApi.INTERMEDIATE_BUFFER_U*)ethRequest._ethPacket._buffer;

                if (buffer->m_dwDeviceFlags == Native.NdisApi.PACKET_FLAG.PACKET_FLAG_ON_SEND)
                    return SendPacketToAdapter(ref ethRequest);
            }

            return SendPacketToMstcp(ref ethRequest);
        }

        /// <summary>
        /// Sends the packets to the adapter.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPacketsToAdapter(ref Native.NdisApi.ETH_M_REQUEST ethMRequest)
        {
            return Native.NdisApi.SendPacketsToAdapter(Handle, ref ethMRequest);
        }

        /// <summary>
        /// Sends the packets to the adapter.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public unsafe bool SendPacketsToAdapter(Native.NdisApi.ETH_M_REQUEST_U* ethMRequest)
        {
            return Native.NdisApi.SendPacketsToAdapter(Handle, ethMRequest);
        }

        /// <summary>
        /// Sends the packets to MSTCP.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPacketsToMstcp(ref Native.NdisApi.ETH_M_REQUEST ethMRequest)
        {
            return Native.NdisApi.SendPacketsToMstcp(Handle, ref ethMRequest);
        }

        /// <summary>
        /// Sends the packets to MSTCP.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public unsafe bool SendPacketsToMstcp(Native.NdisApi.ETH_M_REQUEST_U* ethMRequest)
        {
            return Native.NdisApi.SendPacketsToMstcp(Handle, ethMRequest);
        }

        /// <summary>
        /// Sends the packets to MSTCP or adapter.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPackets(ref Native.NdisApi.ETH_M_REQUEST ethMRequest)
        {
            lock (_lock)
            {
                if (ethMRequest.dwPacketsSuccess == 0)
                    return true;


                _ethPacketsToAdapter.dwPacketsSuccess = 0;
                _ethPacketsToMstcp.dwPacketsSuccess = 0;
                _ethPacketsToAdapter.dwPacketsNumber = 0;
                _ethPacketsToMstcp.dwPacketsNumber = 0;
                _ethPacketsToAdapter.hAdapterHandle = ethMRequest.hAdapterHandle;
                _ethPacketsToMstcp.hAdapterHandle = ethMRequest.hAdapterHandle;

                unsafe
                {
                    for (int i = 0; i < ethMRequest.dwPacketsSuccess; i++)
                    {
                        var packet = ethMRequest._ethPacket[i];
                        var buffer = (Native.NdisApi.INTERMEDIATE_BUFFER_U*)packet._buffer;

                        if (buffer->m_dwDeviceFlags == Native.NdisApi.PACKET_FLAG.PACKET_FLAG_ON_SEND)
                            _ethPacketsToAdapter._ethPacket[_ethPacketsToAdapter.dwPacketsNumber++]._buffer = packet._buffer;
                        else
                            _ethPacketsToMstcp._ethPacket[_ethPacketsToMstcp.dwPacketsNumber++]._buffer = packet._buffer;
                    }
                }

                var success = true;

                if (_ethPacketsToMstcp.dwPacketsNumber > 0)
                    success = SendPacketsToMstcp(ref _ethPacketsToMstcp);

                if (_ethPacketsToAdapter.dwPacketsNumber > 0)
                    success = SendPacketsToAdapter(ref _ethPacketsToAdapter) && success;

                return success;
            }
        }

        /// <summary>
        /// Sends the packets to MSTCP or adapter.
        /// </summary>
        /// <param name="ethRequests">The ether requests.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPackets(IList<Native.NdisApi.ETH_REQUEST> ethRequests)
        {
            lock (_lock)
            {
                if (ethRequests == null || ethRequests.Count == 0)
                    return true;


                var adapterHandles = new HashSet<IntPtr>(ethRequests.Select(x => x.AdapterHandle));
                var success = true;

                unsafe
                {
                    foreach (var adapterHandle in adapterHandles)
                    {
                        _ethPacketsToAdapter.dwPacketsSuccess = 0;
                        _ethPacketsToMstcp.dwPacketsSuccess = 0;
                        _ethPacketsToAdapter.dwPacketsNumber = 0;
                        _ethPacketsToMstcp.dwPacketsNumber = 0;
                        _ethPacketsToAdapter.hAdapterHandle = adapterHandle;
                        _ethPacketsToMstcp.hAdapterHandle = adapterHandle;

                        foreach (var ethRequest in ethRequests)
                        {
                            if (!ethRequest.AdapterHandle.Equals(adapterHandle))
                                continue;


                            var packet = ethRequest._ethPacket;
                            var buffer = (Native.NdisApi.INTERMEDIATE_BUFFER_U*)packet._buffer;

                            if (buffer->m_dwDeviceFlags == Native.NdisApi.PACKET_FLAG.PACKET_FLAG_ON_SEND)
                                _ethPacketsToAdapter._ethPacket[_ethPacketsToAdapter.dwPacketsNumber++]._buffer = packet._buffer;
                            else
                                _ethPacketsToMstcp._ethPacket[_ethPacketsToMstcp.dwPacketsNumber++]._buffer = packet._buffer;
                        }

                        if (_ethPacketsToMstcp.dwPacketsNumber > 0)
                            success = SendPacketsToMstcp(ref _ethPacketsToMstcp) && success;

                        if (_ethPacketsToAdapter.dwPacketsNumber > 0)
                            success = SendPacketsToAdapter(ref _ethPacketsToAdapter) && success;
                    }
                }

                return success;
            }
        }

        /// <summary>
        /// Sends the packets to MSTCP or adapter.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <param name="ethRequests">The ether requests.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPackets(NetworkAdapter networkAdapter, IList<Native.NdisApi.NDISRD_ETH_Packet> ethRequests)
        {
            return SendPackets(networkAdapter.Handle, ethRequests);
        }

        /// <summary>
        /// Sends the packets to MSTCP or adapter.
        /// </summary>
        /// <param name="adapterHandle">The adapter handle.</param>
        /// <param name="ethRequests">The ether requests.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPackets(IntPtr adapterHandle, IList<Native.NdisApi.NDISRD_ETH_Packet> ethRequests)
        {
            lock (_lock)
            {
                if (ethRequests == null || ethRequests.Count == 0)
                    return true;


                _ethPacketsToAdapter.dwPacketsSuccess = 0;
                _ethPacketsToMstcp.dwPacketsSuccess = 0;
                _ethPacketsToAdapter.dwPacketsNumber = 0;
                _ethPacketsToMstcp.dwPacketsNumber = 0;
                _ethPacketsToAdapter.hAdapterHandle = adapterHandle;
                _ethPacketsToMstcp.hAdapterHandle = adapterHandle;

                foreach (var packet in ethRequests)
                {
                    unsafe
                    {
                        var buffer = (Native.NdisApi.INTERMEDIATE_BUFFER_U*)packet._buffer;

                        if (buffer->m_dwDeviceFlags == Native.NdisApi.PACKET_FLAG.PACKET_FLAG_ON_SEND)
                            _ethPacketsToAdapter._ethPacket[_ethPacketsToAdapter.dwPacketsNumber++]._buffer = packet._buffer;
                        else
                            _ethPacketsToMstcp._ethPacket[_ethPacketsToMstcp.dwPacketsNumber++]._buffer = packet._buffer;
                    }
                }

                var success = true;

                if (_ethPacketsToMstcp.dwPacketsNumber > 0)
                    success = SendPacketsToMstcp(ref _ethPacketsToMstcp);

                if (_ethPacketsToAdapter.dwPacketsNumber > 0)
                    success = SendPacketsToAdapter(ref _ethPacketsToAdapter) && success;

                return success;
            }
        }

        /// <summary>
        /// Sends the packets to MSTCP or adapter.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <param name="packets">The optional packets.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public unsafe bool SendPackets(Native.NdisApi.ETH_M_REQUEST_U* ethMRequest, Native.NdisApi.NDISRD_ETH_Packet[] packets = null)
        {
            lock (_lock)
            {
                _ethPacketsToAdapter.dwPacketsSuccess = 0;
                _ethPacketsToMstcp.dwPacketsSuccess = 0;
                _ethPacketsToAdapter.dwPacketsNumber = 0;
                _ethPacketsToMstcp.dwPacketsNumber = 0;
                _ethPacketsToAdapter.hAdapterHandle = ethMRequest->hAdapterHandle;
                _ethPacketsToMstcp.hAdapterHandle = ethMRequest->hAdapterHandle;

                packets = packets ?? ethMRequest->GetPackets();

                foreach (var packet in packets)
                {
                    var buffer = (Native.NdisApi.INTERMEDIATE_BUFFER_U*)packet._buffer;

                    if (buffer->m_dwDeviceFlags == Native.NdisApi.PACKET_FLAG.PACKET_FLAG_ON_SEND)
                        _ethPacketsToAdapter._ethPacket[_ethPacketsToAdapter.dwPacketsNumber++]._buffer = packet._buffer;
                    else
                        _ethPacketsToMstcp._ethPacket[_ethPacketsToMstcp.dwPacketsNumber++]._buffer = packet._buffer;
                }

                var success = true;

                if (_ethPacketsToMstcp.dwPacketsNumber > 0)
                    success = SendPacketsToMstcp(ref _ethPacketsToMstcp);

                if (_ethPacketsToAdapter.dwPacketsNumber > 0)
                    success = SendPacketsToAdapter(ref _ethPacketsToAdapter) && success;

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
        /// <param name="set">if set to <c>true</c> [set].</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool NdisrdRequest(ref Native.NdisApi.PACKET_OID_DATA packetOidData, bool set)
        {
            return Native.NdisApi.NdisrdRequest(Handle, ref packetOidData, set);
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
        /// Gets the hardware packet filter.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns><see cref="Native.NdisApi.NDIS_PACKET_TYPE" />.</returns>
        public Native.NdisApi.NDIS_PACKET_TYPE GetHwPacketFilter(NetworkAdapter networkAdapter)
        {
            var ndisPacketType = Native.NdisApi.NDIS_PACKET_TYPE.NDIS_PACKET_TYPE_NONE;
            Native.NdisApi.GetHwPacketFilter(Handle, networkAdapter.Handle, ref ndisPacketType);
            return ndisPacketType;
        }

        /// <summary>
        /// Sets the packet filter table.
        /// </summary>
        /// <param name="filterTable">The filter table.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SetPacketFilterTable(Native.NdisApi.STATIC_FILTER_TABLE filterTable)
        {
            return Native.NdisApi.SetPacketFilterTable(Handle, ref filterTable);
        }

        /// <summary>
        /// Gets the packet filter table.
        /// </summary>
        /// <returns><see cref="Native.NdisApi.STATIC_FILTER_TABLE" />.</returns>
        public Native.NdisApi.STATIC_FILTER_TABLE GetPacketFilterTable()
        {
            Native.NdisApi.STATIC_FILTER_TABLE filterTable = default;
            Native.NdisApi.GetPacketFilterTable(Handle, ref filterTable);
            return filterTable;
        }

        /// <summary>
        /// Sets the packet filter table.
        /// </summary>
        /// <param name="filterTable">The filter table.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public unsafe bool SetPacketFilterTable(Native.NdisApi.STATIC_FILTER_TABLE_U* filterTable)
        {
            return Native.NdisApi.SetPacketFilterTable(Handle, filterTable);
        }

        /// <summary>
        /// Gets the unsafe packet filter table.
        /// </summary>
        /// <returns><see cref="Native.NdisApi.STATIC_FILTER_TABLE_U" />.</returns>
        public unsafe Native.NdisApi.STATIC_FILTER_TABLE_U GetUnsafePacketFilterTable()
        {
            var tableSize = GetPacketFilterTableSize();
            var filterTable = _ndisApiHelper.CreateUnsafeStaticFilterTable(tableSize);
            Native.NdisApi.GetPacketFilterTable(Handle, filterTable);

            _ndisApiHelper.DisposeObject(filterTable);

            return *filterTable;
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
        /// <returns>
        /// <see cref="Native.NdisApi.ADAPTER_MODE" />.
        /// </returns>
        public Native.NdisApi.ADAPTER_MODE GetAdapterMode(NetworkAdapter networkAdapter)
        {
            var adapterMode = new Native.NdisApi.ADAPTER_MODE { hAdapterHandle = networkAdapter.Handle };
            Native.NdisApi.GetAdapterMode(Handle, ref adapterMode);
            return adapterMode;
        }

        /// <summary>
        /// Gets the size of the packet filter table.
        /// </summary>
        /// <returns>
        /// <see cref="System.UInt32" />.
        /// </returns>
        public uint GetPacketFilterTableSize()
        {
            uint tableSize = 0;
            Native.NdisApi.GetPacketFilterTableSize(Handle, ref tableSize);
            return tableSize;
        }

        /// <summary>
        /// Gets the packet filter table reset stats.
        /// </summary>
        /// <returns>
        /// <see cref="Native.NdisApi.STATIC_FILTER_TABLE" />.
        /// </returns>
        public Native.NdisApi.STATIC_FILTER_TABLE GetPacketFilterTableResetStats()
        {
            Native.NdisApi.STATIC_FILTER_TABLE filterList = default;
            Native.NdisApi.GetPacketFilterTableResetStats(Handle, ref filterList);
            return filterList;
        }

        /// <summary>
        /// Gets the packet filter table reset stats.
        /// </summary>
        /// <returns>
        /// <see cref="Native.NdisApi.STATIC_FILTER_TABLE_U" />.
        /// </returns>
        public unsafe Native.NdisApi.STATIC_FILTER_TABLE_U GetUnsafePacketFilterTableResetStats()
        {
            var tableSize = GetPacketFilterTableSize();
            var filterTable = _ndisApiHelper.CreateUnsafeStaticFilterTable(tableSize);
            Native.NdisApi.GetPacketFilterTableResetStats(Handle, filterTable);

            _ndisApiHelper.DisposeObject(filterTable);

            return *filterTable;
        }

        /// <summary>
        /// Gets the ras links of the specified <see cref="networkAdapter" />.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns>
        ///     <see cref="Native.NdisApi.RAS_LINKS" />
        /// </returns>
        public Native.NdisApi.RAS_LINKS GetRasLinks(NetworkAdapter networkAdapter)
        {
            var rasLinksPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Native.NdisApi.RAS_LINKS)));

            try
            {
                var result = Native.NdisApi.GetRasLinks(Handle, networkAdapter.Handle, rasLinksPtr);
                if (!result)
                    return default;


                return (Native.NdisApi.RAS_LINKS) Marshal.PtrToStructure(rasLinksPtr, typeof(Native.NdisApi.RAS_LINKS));
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
        /// Determines whether the NdisApi DLL exists.
        /// </summary>
        /// <returns>
        /// <see cref="System.Boolean" />.
        /// </returns>
        private static bool NdisApiDllExists()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(directory, "ndisapi.dll");

            return File.Exists(path);
        }
    }
}