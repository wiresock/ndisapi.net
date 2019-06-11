// ----------------------------------------------
// <copyright file="NetworkAdapter.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using NdisApiDotNet.Native;

namespace NdisApiDotNet
{
    public class NetworkAdapter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkAdapter" /> class.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="nameBytes">Name of the adapter in bytes.</param>
        /// <param name="medium">The medium.</param>
        /// <param name="address">The mac address.</param>
        /// <param name="mtu">The mtu.</param>
        public NetworkAdapter(IntPtr handle, byte[] nameBytes, Native.NdisApi.NDIS_MEDIUM medium, byte[] address, ushort mtu)
        {
            Handle = handle;
            Mtu = mtu;
            Medium = medium;

            PhysicalAddress = new PhysicalAddress(address);

            NameBytes = nameBytes;
            Name = GetInternalName();
            FriendlyName = GetFriendlyName();
        }

        /// <summary>
        /// Gets the friendly name of the network adapter, as shown in Windows' control panel.
        /// </summary>
        public string FriendlyName { get; }

        /// <summary>
        /// Gets the handle of the network adapter.
        /// </summary>
        public IntPtr Handle { get; }

        /// <summary>
        /// Gets the packet event wait handle.
        /// </summary>
        /// <remarks>This is only available on the instance that called <see cref="NdisApi"/>'s SetPacketEvent.</remarks>
        public WaitHandle WaitHandle { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the network adapter is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (var b in PhysicalAddress.GetAddressBytes())
                {
                    if (b != 0)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the medium of the TCP adapter.
        /// </summary>
        public Native.NdisApi.NDIS_MEDIUM Medium { get; }

        /// <summary>
        /// Gets the MTU of the network adapter.
        /// </summary>
        public ushort Mtu { get; }

        /// <summary>
        /// Gets the name of the network adapter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the physical address of the network adapter.
        /// </summary>
        public PhysicalAddress PhysicalAddress { get; }

        /// <summary>
        /// Gets the name of the network adapter in bytes.
        /// </summary>
        private byte[] NameBytes { get; }

        /// <summary>
        /// Gets the internal name.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetInternalName()
        {
            var name = Encoding.ASCII.GetString(NameBytes);
            var i = name.IndexOf((char) 0);
            return i >= 0 ? name.Substring(0, i) : name;
        }

        /// <summary>
        /// Gets the friendly name, as shown in Windows' control panel.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetFriendlyName()
        {
            var lpVersionInformation = new NtDll.OSVERSIONINFOEX();
            lpVersionInformation.dwOSVersionInfoSize = (uint) Marshal.SizeOf(lpVersionInformation);
            NtDll.RtlGetVersion(ref lpVersionInformation);

            return ConvertAdapterName(NameBytes, 0, lpVersionInformation.dwPlatformId, lpVersionInformation.dwMajorVersion);
        }

        /// <summary>
        /// Converts the name of the adapter.
        /// </summary>
        /// <param name="adapterNameBytes">Bytes of the adapter name.</param>
        /// <param name="nameStart">The start of the name.</param>
        /// <param name="platformId">The OS platform identifier.</param>
        /// <param name="majorVersion">The major OS version.</param>
        /// <returns><see cref="System.String"/>.</returns>
        private static unsafe string ConvertAdapterName(byte[] adapterNameBytes, int nameStart, uint platformId, uint majorVersion)
        {
            fixed (byte* adapterNamePtr = &adapterNameBytes[nameStart])
            {
                var friendlyNameBytes = new byte[Native.NdisApi.ADAPTER_NAME_SIZE];
                string friendlyName = null;
                var success = false;

                fixed (byte* friendlyNameFixedBytes = friendlyNameBytes)
                {
                    if (platformId == 2)
                    {
                        /*VER_PLATFORM_WIN32_NT*/

                        if (majorVersion > 4)
                        {
                            // Windows 2000 or XP.
                            success = Native.NdisApi.ConvertWindows2000AdapterName(adapterNamePtr, friendlyNameFixedBytes, (uint) friendlyNameBytes.Length);
                        }
                        else if (majorVersion == 4)
                        {
                            // Windows NT 4.0.
                            success = Native.NdisApi.ConvertWindowsNTAdapterName(adapterNamePtr, friendlyNameFixedBytes, (uint) friendlyNameBytes.Length);
                        }
                    }
                    else
                    {
                        // Windows 9x/ME.
                        success = Native.NdisApi.ConvertWindows9xAdapterName(adapterNamePtr, friendlyNameFixedBytes, (uint) friendlyNameBytes.Length);
                    }

                    if (success)
                    {
                        var indexOfZero = 0;
                        while (indexOfZero < 256 && friendlyNameBytes[indexOfZero] != 0)
                            ++indexOfZero;
                        friendlyName = Encoding.Default.GetString(friendlyNameBytes, 0, indexOfZero);
                    }
                }

                return friendlyName;
            }
        }
    }
}