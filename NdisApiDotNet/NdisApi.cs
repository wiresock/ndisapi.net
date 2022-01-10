﻿// ----------------------------------------------
// <copyright file="NdisApi.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using Microsoft.Win32.SafeHandles;
using NdisApiDotNet.Filters;
using NdisApiDotNet.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace NdisApiDotNet
{
    public class NdisAPI : IDisposable
    {
        private readonly byte[] _driverNameBytes;
        private readonly object _lock = new object();
        private readonly NdisAPIHelper _ndisApiHelper;
        private EthMRequest _ethPacketsToAdapter;
        private EthMRequest _ethPacketsToMstcp;

        static NdisAPI()
        {
            //Console.WriteLine(AppContext.GetData("NATIVE_DLL_SEARCH_DIRECTORIES"));
            //Console.WriteLine(AppContext.GetData("SINGLE_FILE_EXTRACTION_PATH"));
            NativeLibrary.SetDllImportResolver(typeof(Imports).Assembly, ImportResolver);
        }

        private static IntPtr ImportResolver(string libraryName, System.Reflection.Assembly assembly, DllImportSearchPath? searchPath)
        {
            IntPtr libHandle = IntPtr.Zero;
            if (libraryName == Imports.DllName) NativeLibrary.TryLoad($"{Path.GetFileNameWithoutExtension(Imports.DllName)}_{(Environment.Is64BitProcess ? "x64" : "x86")}.dll", assembly, DllImportSearchPath.SafeDirectories, out libHandle);

            return libHandle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NdisAPI" /> class.
        /// </summary>
        /// <param name="handle">The filter driver handle.</param>
        protected NdisAPI(SafeFilterDriverHandle handle)
        {
            Handle = handle;

            _ndisApiHelper = new NdisAPIHelper();
            _ethPacketsToMstcp = _ndisApiHelper.CreateEthMRequest();
            _ethPacketsToAdapter = _ndisApiHelper.CreateEthMRequest();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NdisAPI" /> class.
        /// </summary>
        /// <param name="handle">The filter driver handle.</param>
        /// <param name="driverNameBytes">The driver name bytes.</param>
        protected NdisAPI(SafeFilterDriverHandle handle, byte[] driverNameBytes) : this(handle)
        {
            _driverNameBytes = driverNameBytes;
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
        public bool IsValid { get { return !Handle.IsInvalid; } }

        /// <summary>
        /// Gets the maximum size of a packet in bytes.
        /// </summary>
        public static uint MaxPacketSize { get { return Consts.MAX_ETHER_FRAME; } }

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
        /// <returns><see cref="NdisAPI" />.</returns>
        /// <exception cref="Exception">Missing NDIS DLL</exception>
        public static NdisAPI Open(string driverName = "NDISRD")
        {
            //if (!NdisApiDllExists()) throw new Exception("Missing NDIS DLL");

            byte[] driverNameBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(driverName);
            SafeFilterDriverHandle handle = Imports.OpenFilterDriver(driverNameBytes);
            return new NdisAPI(handle, driverNameBytes);
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
            if (_driverNameBytes == null || _driverNameBytes.Length == 0) throw new Exception("Missing driver name.");

            Close();
            Handle = Imports.OpenFilterDriver(_driverNameBytes);
        }

        /// <summary>
        /// Gets the native version of the filter driver.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        public uint GetNativeVersion()
        {
            return Imports.GetDriverVersion(Handle);
        }

        /// <summary>
        /// Gets the version of the filter driver.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><see cref="Version" />.</returns>
        public static Version GetVersion(string fileName = "NDISRD")
        {
            if (!Environment.Is64BitProcess) return GetVersionX86(fileName);

            return GetVersionInternal(fileName);
        }

        private static Version GetVersionX86(string fileName)
        {
            IntPtr wow64Value = IntPtr.Zero;

            try
            {
                Kernel32.Wow64DisableWow64FsRedirection(ref wow64Value); // Disable System32 -> SysWOW64 redirection.

                return GetVersionInternal(fileName);
            }
            finally
            {
                if (wow64Value != IntPtr.Zero) Kernel32.Wow64RevertWow64FsRedirection(wow64Value); // Re-enable redirection.
            }
        }

        private static Version GetVersionInternal(string fileName)
        {
            string filePath = Path.Combine(Environment.SystemDirectory, @"drivers\" + fileName + ".sys");
            if (!File.Exists(filePath)) return new Version(0, 0, 0, 0);

            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
            return new Version(fileVersionInfo.FileVersion);
        }

        /// <summary>
        /// Determines whether the driver has been loaded.
        /// </summary>
        /// <returns><c>true</c> if the driver is loaded; otherwise, <c>false</c>.</returns>
        public bool IsDriverLoaded()
        {
            return Imports.IsDriverLoaded(Handle);
        }

        /// <summary>
        /// Determines whether the driver is installed.
        /// </summary>
        /// <param name="componentId">The component identifier.</param>
        /// <returns><c>true</c> if the driver is installed; otherwise, <c>false</c>.</returns>
        public static bool IsDriverInstalled(string componentId = "nt_ndisrd")
        {
            using NetCfg snetCfg = new NetCfg("");

            return snetCfg.IsInstalled(componentId);
        }

        /// <summary>
        /// Uninstalls the driver.
        /// </summary>
        /// <param name="afterReboot">if set to <c>true</c>, requires a reboot for the changes to take affect.</param>
        /// <param name="errorCode">The error code.</param>
        /// <returns><c>true</c> if uninstalled, <c>false</c> otherwise.</returns>
        public static bool UninstallDriver(out bool afterReboot, out uint errorCode)
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
        public static bool UninstallDriver(string componentId, out bool afterReboot, out uint errorCode)
        {
            using NetCfg snetCfg = new NetCfg("");

            return snetCfg.Uninstall(componentId, out afterReboot, out errorCode);
        }

        /// <summary>
        /// Installs the driver.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="afterReboot">if set to <c>true</c>, requires a reboot for the changes to take affect.</param>
        /// <param name="errorCode">The error code.</param>
        /// <returns><c>true</c> if installed, <c>false</c> otherwise.</returns>
        public static bool InstallDriver(string rootPath, out bool afterReboot, out uint errorCode)
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
        public static bool InstallDriver(string rootPath, string infFileName, string componentId, out bool afterReboot, out uint errorCode)
        {
            string subFolder;
            if (OperatingSystem.IsWin10()) subFolder = "win10";
            else if (OperatingSystem.IsWin8() || OperatingSystem.IsWin81()) subFolder = "win8";
            else if (OperatingSystem.IsWin7()) subFolder = "win7";
            else if (OperatingSystem.IsWinVista()) subFolder = "vista";
            else throw new Exception("System not supported"); // TODO: Windows 11?

            string architecture = Environment.Is64BitOperatingSystem ? "amd64" : "i386";
            string path = Path.Combine(Path.Combine(rootPath, subFolder), architecture);
            using NetCfg snetCfg = new NetCfg(path);

            return snetCfg.Install(infFileName, componentId, out afterReboot, out errorCode);
        }

        /// <summary>
        /// Installs the certificate for the driver.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if installed, <c>false</c> otherwise.</returns>
        public static bool InstallCertificate(string path)
        {
            try
            {
                X509Certificate2 x509Certificate = new X509Certificate2(path);

                X509Store x509Store = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);
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
        public static uint GetMtuDecrement()
        {
            return Imports.GetMTUDecrement();
        }

        /// <summary>
        /// Gets the bytes returned.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        public uint GetBytesReturned()
        {
            return Imports.GetBytesReturned(Handle);
        }

        /// <summary>
        /// Sets the MTU decrement.
        /// </summary>
        /// <param name="mtuDecrement">The mtu decrement.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public static bool SetMtuDecrement(uint mtuDecrement)
        {
            return Imports.SetMTUDecrement(mtuDecrement);
        }

        /// <summary>
        /// Gets the adapters startup mode.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        public static MSTCPFlags GetAdaptersStartupMode()
        {
            return Imports.GetAdaptersStartupMode();
        }

        /// <summary>
        /// Gets the network adapters.
        /// </summary>
        /// <returns>The <see cref="NetworkAdapter" />s.</returns>
        public IEnumerable<NetworkAdapter> GetNetworkAdapters()
        {
            TCPAdapterList adapterList = new TCPAdapterList();
            Imports.GetTcpipBoundAdaptersInfo(Handle, ref adapterList);

            for (int i = 0; i < adapterList.m_nAdapterCount; i++)
            {
                yield return new NetworkAdapter(adapterList.AdapterHandle[i],
                                                adapterList.AdapterNames.Skip(i * Consts.ADAPTER_NAME_SIZE).Take(Consts.ADAPTER_NAME_SIZE).ToArray(),
                                                adapterList.AdapterMediums[i],
                                                adapterList.CurrentAddress.Skip(i * Consts.ETHER_ADDR_LENGTH).Take(Consts.ETHER_ADDR_LENGTH).ToArray(),
                                                adapterList.MTU[i]);
            }
        }

        /// <summary>
        /// Gets the network adapters with all specified <see cref="flags" />.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns><see cref="NetworkAdapter" />s.</returns>
        public IEnumerable<NetworkAdapter> GetNetworkAdapters(MSTCPFlags flags)
        {
            foreach (NetworkAdapter networkAdapter in GetNetworkAdapters())
            {
                if (GetAdapterMode(networkAdapter).Flags.Equals(flags)) yield return networkAdapter;
            }
        }

        /// <summary>
        /// Gets the bound network adapters.
        /// </summary>
        /// <returns><see cref="NetworkAdapter" />s.</returns>
        public IEnumerable<NetworkAdapter> GetBoundNetworkAdapters()
        {
            foreach (NetworkAdapter networkAdapter in GetNetworkAdapters())
            {
                if (GetAdapterMode(networkAdapter).Flags != 0) yield return networkAdapter;
            }
        }

        /// <summary>
        /// Sets the adapter mode to the specified <see cref="flags" />.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <param name="flags">The flags.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SetAdapterMode(NetworkAdapter networkAdapter, MSTCPFlags flags)
        {
            AdapterMode adapterMode = new AdapterMode { dwFlags = flags, hAdapterHandle = networkAdapter.Handle };

            return Imports.SetAdapterMode(Handle, ref adapterMode);
        }

        /// <summary>
        /// Resets the adapter mode by clearing all set flags.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool ResetAdapterMode(NetworkAdapter networkAdapter)
        {
            AdapterMode adapterMode = new AdapterMode { dwFlags = 0, hAdapterHandle = networkAdapter.Handle };

            return Imports.SetAdapterMode(Handle, ref adapterMode);
        }

        /// <summary>
        /// Sets the packet event to the specified <see cref="networkAdapter" />.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <param name="waitHandle">The wait handle.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SetPacketEvent(NetworkAdapter networkAdapter, WaitHandle waitHandle)
        {
            bool success = Imports.SetPacketEvent(Handle, networkAdapter.Handle, waitHandle.SafeWaitHandle);
            if (success) networkAdapter.WaitHandle = waitHandle;

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
            return Imports.SetPacketEvent(Handle, networkAdapter.Handle, safeWaitHandle);
        }

        /// <summary>
        /// Sets the WAN event.
        /// </summary>
        /// <param name="safeWaitHandle">The safe wait handle.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SetWanEvent(SafeWaitHandle safeWaitHandle)
        {
            return Imports.SetWANEvent(Handle, safeWaitHandle);
        }

        /// <summary>
        /// Sets the adapter list change event.
        /// </summary>
        /// <param name="safeWaitHandle">The safe wait handle.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SetAdapterListChangeEvent(SafeWaitHandle safeWaitHandle)
        {
            return Imports.SetAdapterListChangeEvent(Handle, safeWaitHandle);
        }

        /// <summary>
        /// Reads the packet to the specified <see cref="ethRequest" />.
        /// </summary>
        /// <param name="ethRequest">The ether request.</param>
        /// <remarks>The adapter handle needs to be set.</remarks>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool ReadPacket(ref EthRequest ethRequest)
        {
            return Imports.ReadPacket(Handle, ref ethRequest);
        }

        /// <summary>
        /// Reads the packets to the specified <see cref="ethMRequest" />.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <remarks>The adapter handle needs to be set.</remarks>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool ReadPackets(ref EthMRequest ethMRequest)
        {
            return Imports.ReadPackets(Handle, ref ethMRequest);
        }

        /// <summary>
        /// Reads the packets to the specified <see cref="ethMRequest" />.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <remarks>The adapter handle needs to be set.</remarks>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public unsafe bool ReadPackets(EthMRequestUnsafe* ethMRequest)
        {
            return Imports.ReadPackets(Handle, ethMRequest);
        }

        /// <summary>
        /// Sends the packet to adapter.
        /// </summary>
        /// <param name="ethRequest">The ether request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPacketToAdapter(ref EthRequest ethRequest)
        {
            return Imports.SendPacketToAdapter(Handle, ref ethRequest);
        }

        /// <summary>
        /// Sends the packet to MSTCP.
        /// </summary>
        /// <param name="ethRequest">The ether request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPacketToMstcp(ref EthRequest ethRequest)
        {
            return Imports.SendPacketToMstcp(Handle, ref ethRequest);
        }

        /// <summary>
        /// Sends the packet to MSTCP or adapter.
        /// </summary>
        /// <param name="ethRequest">The ether request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPacket(ref EthRequest ethRequest)
        {
            unsafe
            {
                IntermediateBufferUnsafe* buffer = (IntermediateBufferUnsafe*)ethRequest._ethPacket._buffer;

                if (buffer->m_dwDeviceFlags == PacketFlag.Send)
                    return SendPacketToAdapter(ref ethRequest);
            }

            return SendPacketToMstcp(ref ethRequest);
        }

        /// <summary>
        /// Sends the packets to the adapter.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPacketsToAdapter(ref EthMRequest ethMRequest)
        {
            return Imports.SendPacketsToAdapter(Handle, ref ethMRequest);
        }

        /// <summary>
        /// Sends the packets to the adapter.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public unsafe bool SendPacketsToAdapter(EthMRequestUnsafe* ethMRequest)
        {
            return Imports.SendPacketsToAdapter(Handle, ethMRequest);
        }

        /// <summary>
        /// Sends the packets to MSTCP.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPacketsToMstcp(ref EthMRequest ethMRequest)
        {
            return Imports.SendPacketsToMstcp(Handle, ref ethMRequest);
        }

        /// <summary>
        /// Sends the packets to MSTCP.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public unsafe bool SendPacketsToMstcp(EthMRequestUnsafe* ethMRequest)
        {
            return Imports.SendPacketsToMstcp(Handle, ethMRequest);
        }

        /// <summary>
        /// Sends the packets to MSTCP or adapter.
        /// </summary>
        /// <param name="ethMRequest">The ether multiple request.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPackets(ref EthMRequest ethMRequest)
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
                        EthPacket packet = ethMRequest._ethPacket[i];
                        IntermediateBufferUnsafe* buffer = (IntermediateBufferUnsafe*)packet._buffer;

                        if (buffer->m_dwDeviceFlags == PacketFlag.Send)
                            _ethPacketsToAdapter._ethPacket[_ethPacketsToAdapter.dwPacketsNumber++]._buffer = packet._buffer;
                        else
                            _ethPacketsToMstcp._ethPacket[_ethPacketsToMstcp.dwPacketsNumber++]._buffer = packet._buffer;
                    }
                }

                bool success = true;

                if (_ethPacketsToMstcp.dwPacketsNumber > 0) success = SendPacketsToMstcp(ref _ethPacketsToMstcp);

                if (_ethPacketsToAdapter.dwPacketsNumber > 0) success = SendPacketsToAdapter(ref _ethPacketsToAdapter) && success;

                return success;
            }
        }

        /// <summary>
        /// Sends the packets to MSTCP or adapter.
        /// </summary>
        /// <param name="ethRequests">The ether requests.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPackets(IList<EthRequest> ethRequests)
        {
            lock (_lock)
            {
                if (ethRequests == null || ethRequests.Count == 0)
                    return true;


                HashSet<IntPtr> adapterHandles = new HashSet<IntPtr>(ethRequests.Select(x => x.AdapterHandle));
                bool success = true;

                unsafe
                {
                    foreach (IntPtr adapterHandle in adapterHandles)
                    {
                        _ethPacketsToAdapter.dwPacketsSuccess = 0;
                        _ethPacketsToMstcp.dwPacketsSuccess = 0;
                        _ethPacketsToAdapter.dwPacketsNumber = 0;
                        _ethPacketsToMstcp.dwPacketsNumber = 0;
                        _ethPacketsToAdapter.hAdapterHandle = adapterHandle;
                        _ethPacketsToMstcp.hAdapterHandle = adapterHandle;

                        foreach (EthRequest ethRequest in ethRequests)
                        {
                            if (!ethRequest.AdapterHandle.Equals(adapterHandle))
                                continue;


                            EthPacket packet = ethRequest._ethPacket;
                            IntermediateBufferUnsafe* buffer = (IntermediateBufferUnsafe*)packet._buffer;

                            if (buffer->m_dwDeviceFlags == PacketFlag.Send)
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
        public bool SendPackets(NetworkAdapter networkAdapter, IList<EthPacket> ethRequests)
        {
            return SendPackets(networkAdapter.Handle, ethRequests);
        }

        /// <summary>
        /// Sends the packets to MSTCP or adapter.
        /// </summary>
        /// <param name="adapterHandle">The adapter handle.</param>
        /// <param name="ethRequests">The ether requests.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SendPackets(IntPtr adapterHandle, IList<EthPacket> ethRequests)
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

                foreach (EthPacket packet in ethRequests)
                {
                    unsafe
                    {
                        IntermediateBufferUnsafe* buffer = (IntermediateBufferUnsafe*)packet._buffer;

                        if (buffer->m_dwDeviceFlags == PacketFlag.Send)
                            _ethPacketsToAdapter._ethPacket[_ethPacketsToAdapter.dwPacketsNumber++]._buffer = packet._buffer;
                        else
                            _ethPacketsToMstcp._ethPacket[_ethPacketsToMstcp.dwPacketsNumber++]._buffer = packet._buffer;
                    }
                }

                bool success = true;

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
        public unsafe bool SendPackets(EthMRequestUnsafe* ethMRequest, EthPacket[] packets = null)
        {
            lock (_lock)
            {
                _ethPacketsToAdapter.dwPacketsSuccess = 0;
                _ethPacketsToMstcp.dwPacketsSuccess = 0;
                _ethPacketsToAdapter.dwPacketsNumber = 0;
                _ethPacketsToMstcp.dwPacketsNumber = 0;
                _ethPacketsToAdapter.hAdapterHandle = ethMRequest->hAdapterHandle;
                _ethPacketsToMstcp.hAdapterHandle = ethMRequest->hAdapterHandle;

                packets ??= ethMRequest->GetPackets();

                foreach (EthPacket packet in packets)
                {
                    IntermediateBufferUnsafe* buffer = (IntermediateBufferUnsafe*)packet._buffer;

                    if (buffer->m_dwDeviceFlags == PacketFlag.Send)
                        _ethPacketsToAdapter._ethPacket[_ethPacketsToAdapter.dwPacketsNumber++]._buffer = packet._buffer;
                    else
                        _ethPacketsToMstcp._ethPacket[_ethPacketsToMstcp.dwPacketsNumber++]._buffer = packet._buffer;
                }

                bool success = true;

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
            return Imports.FlushAdapterPacketQueue(Handle, networkAdapter.Handle);
        }

        /// <summary>
        /// Gets the size of the adapter packet queue.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns>The queue size if valid, <c>-1</c> otherwise.</returns>
        public int GetAdapterPacketQueueSize(NetworkAdapter networkAdapter)
        {
            uint size = 0;
            if (Imports.GetAdapterPacketQueueSize(Handle, networkAdapter.Handle, ref size))
                return (int)size;


            return -1;
        }

        /// <summary>
        /// Performs a query or set operation on the adapter provided by <see cref="packetOidData" />,
        /// </summary>
        /// <param name="packetOidData">The packet oid data.</param>
        /// <param name="set">if set to <c>true</c> [set].</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool NdisrdRequest(ref PacketOID packetOidData, bool set)
        {
            return Imports.NdisrdRequest(Handle, ref packetOidData, set);
        }

        /// <summary>
        /// Sets the hardware packet filter.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <param name="ndisPacketType">Type of the ndis packet.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SetHwPacketFilter(NetworkAdapter networkAdapter, PacketType ndisPacketType)
        {
            return Imports.SetHwPacketFilter(Handle, networkAdapter.Handle, ndisPacketType);
        }

        /// <summary>
        /// Gets the hardware packet filter.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns><see cref="PacketType" />.</returns>
        public PacketType GetHwPacketFilter(NetworkAdapter networkAdapter)
        {
            PacketType ndisPacketType = PacketType.None;
            Imports.GetHwPacketFilter(Handle, networkAdapter.Handle, ref ndisPacketType);
            return ndisPacketType;
        }

        /// <summary>
        /// Sets the packet filter table.
        /// </summary>
        /// <param name="filterTable">The filter table.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SetPacketFilterTable(StaticFilterTable filterTable)
        {
            return Imports.SetPacketFilterTable(Handle, ref filterTable);
        }

        /// <summary>
        /// Gets the packet filter table.
        /// </summary>
        /// <returns><see cref="StaticFilterTable" />.</returns>
        public StaticFilterTable GetPacketFilterTable()
        {
            StaticFilterTable filterTable = default;
            Imports.GetPacketFilterTable(Handle, ref filterTable);
            return filterTable;
        }

        ///// <summary>
        ///// Sets the packet filter table.
        ///// </summary>
        ///// <param name="filterTable">The filter table.</param>
        ///// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        //public unsafe bool SetPacketFilterTable(StaticFilterTableUnsafe* filterTable)
        //{
        //    return Imports.SetPacketFilterTable(Handle, filterTable);
        //}

        ///// <summary>
        ///// Gets the unsafe packet filter table.
        ///// </summary>
        ///// <returns><see cref="StaticFilterTableUnsafe" />.</returns>
        //public unsafe StaticFilterTableUnsafe GetUnsafePacketFilterTable()
        //{
        //    uint tableSize = GetPacketFilterTableSize();
        //    StaticFilterTableUnsafe* filterTable = _ndisApiHelper.CreateUnsafeStaticFilterTable(tableSize);
        //    Imports.GetPacketFilterTable(Handle, filterTable);

        //    _ndisApiHelper.DisposeObject(filterTable);

        //    return *filterTable;
        //}

        /// <summary>
        /// Sets the adapters startup mode.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public static bool SetAdaptersStartupMode(MSTCPFlags flags)
        {
            return Imports.SetAdaptersStartupMode(flags);
        }

        /// <summary>
        /// Gets the adapter mode.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns>
        /// <see cref="AdapterMode" />.
        /// </returns>
        public AdapterMode GetAdapterMode(NetworkAdapter networkAdapter)
        {
            AdapterMode adapterMode = new AdapterMode { hAdapterHandle = networkAdapter.Handle };
            Imports.GetAdapterMode(Handle, ref adapterMode);
            return adapterMode;
        }

        /// <summary>
        /// Gets the size of the packet filter table.
        /// </summary>
        /// <returns>
        /// <see cref="uint" />.
        /// </returns>
        public uint GetPacketFilterTableSize()
        {
            uint tableSize = 0;
            Imports.GetPacketFilterTableSize(Handle, ref tableSize);
            return tableSize;
        }

        /// <summary>
        /// Gets the packet filter table reset stats.
        /// </summary>
        /// <returns>
        /// <see cref="StaticFilterTable" />.
        /// </returns>
        public StaticFilterTable GetPacketFilterTableResetStats()
        {
            StaticFilterTable filterList = default;
            Imports.GetPacketFilterTableResetStats(Handle, ref filterList);
            return filterList;
        }

        ///// <summary>
        ///// Gets the packet filter table reset stats.
        ///// </summary>
        ///// <returns>
        ///// <see cref="StaticFilterTableUnsafe" />.
        ///// </returns>
        //public unsafe StaticFilterTableUnsafe GetUnsafePacketFilterTableResetStats()
        //{
        //    uint tableSize = GetPacketFilterTableSize();
        //    StaticFilterTableUnsafe* filterTable = _ndisApiHelper.CreateUnsafeStaticFilterTable(tableSize);
        //    Imports.GetPacketFilterTableResetStats(Handle, filterTable);

        //    _ndisApiHelper.DisposeObject(filterTable);

        //    return *filterTable;
        //}

        /// <summary>
        /// Gets the ras links of the specified <see cref="networkAdapter" />.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns>
        ///     <see cref="RAS_LINKS" />
        /// </returns>
        public RasLinks GetRasLinks(NetworkAdapter networkAdapter)
        {
            IntPtr rasLinksPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RasLinks)));

            try
            {
                bool result = Imports.GetRasLinks(Handle, networkAdapter.Handle, rasLinksPtr);
                if (!result) return default;

                return (RasLinks)Marshal.PtrToStructure(rasLinksPtr, typeof(RasLinks));
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
            return Imports.ResetPacketFilterTable(Handle);
        }

        /// <summary>
        /// Determines whether the NdisApi DLL exists.
        /// </summary>
        /// <returns>
        /// <see cref="bool" />.
        /// </returns>
        //private static bool NdisApiDllExists()
        //{
        //    string directory = AppDomain.CurrentDomain.BaseDirectory;
        //    string path = Path.Combine(directory, "ndisapi.dll");

        //    return File.Exists(path);
        //}
    }
}