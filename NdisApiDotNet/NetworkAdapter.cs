// ----------------------------------------------
// <copyright file="NetworkAdapter.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
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
    public class NetworkAdapter : IEquatable<NetworkAdapter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkAdapter" /> class.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="nameBytes">Name of the adapter in bytes.</param>
        /// <param name="medium">The medium.</param>
        /// <param name="address">The mac address.</param>
        /// <param name="mtu">The mtu.</param>
        /// <param name="ndisApi">The ndis API.</param>
        public NetworkAdapter(IntPtr handle, byte[] nameBytes, Native.NdisApi.NDIS_MEDIUM medium, byte[] address, ushort mtu, NdisApi ndisApi = null)
        {
            Handle = handle;
            Mtu = mtu;
            Medium = medium;

            PhysicalAddress = new PhysicalAddress(address);

            Name = GetName(nameBytes);
            FriendlyName = GetFriendlyName(nameBytes);

            NdisApi = ndisApi;
        }

        /// <summary>
        /// Gets the friendly name of the network adapter, as shown in Windows' control panel.
        /// </summary>
        public string FriendlyName { get; private set; }

        /// <summary>
        /// Gets the handle of the network adapter.
        /// </summary>
        public IntPtr Handle { get; }

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
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the medium of the network adapter.
        /// </summary>
        public Native.NdisApi.NDIS_MEDIUM Medium { get; private set; }

        /// <summary>
        /// Gets the MTU of the network adapter.
        /// </summary>
        public ushort Mtu { get; private set; }

        /// <summary>
        /// Gets the name of the network adapter.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the <see cref="NdisApiDotNet.NdisApi" /> that created this instance.
        /// </summary>
        public NdisApi NdisApi { get; }

        /// <summary>
        /// Gets the physical address of the network adapter.
        /// </summary>
        public PhysicalAddress PhysicalAddress { get; private set; }

        /// <summary>
        /// Gets the packet event wait handle.
        /// </summary>
        /// <remarks>This is only set if <see cref="SetPacketEvent" /> is called with a valid <see cref="WaitHandle"/>.</remarks>
        public WaitHandle WaitHandle { get; private set; }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            if (NdisApi != null)
            {
                foreach (var networkAdapter in NdisApi.GetNetworkAdapters())
                {
                    if (networkAdapter.Equals(this))
                    {
                        PhysicalAddress = networkAdapter.PhysicalAddress;
                        FriendlyName = networkAdapter.FriendlyName;
                        Name = networkAdapter.Name;
                        Medium = networkAdapter.Medium;
                        Mtu = networkAdapter.Mtu;

                        break;
                    }
                }
            }
        }

        /// <inheritdoc />
        public bool Equals(NetworkAdapter other)
        {
            if (other == null)
            {
                return false;
            }

            return Handle == other.Handle;
        }

        /// <summary>
        /// Sets the packet event.
        /// </summary>
        /// <param name="waitHandle">The wait handle.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SetPacketEvent(WaitHandle waitHandle)
        {
            if (waitHandle == null)
            {
                return false;
            }

            var success = NdisApi?.SetPacketEvent(this, waitHandle) == true;
            WaitHandle = success && waitHandle.SafeWaitHandle.DangerousGetHandle() != IntPtr.Zero ? waitHandle : null;
            return success;
        }

        /// <summary>
        /// Resets the currently set packet event.
        /// </summary>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool ResetPacketEvent()
        {
            var success = NdisApi?.ResetPacketEvent(this) == true;
            WaitHandle = null;
            return success;
        }

        /// <summary>
        /// Gets the hardware packet filter.
        /// </summary>
        /// <param name="hwPacketType">The resulting hardware packet filter.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool GetHwPacketFilter(out Native.NdisApi.NDIS_PACKET_TYPE hwPacketType)
        {
            hwPacketType = Native.NdisApi.NDIS_PACKET_TYPE.NDIS_PACKET_TYPE_NONE;
            return NdisApi?.GetHwPacketFilter(this, out hwPacketType) == true;
        }

        /// <summary>
        /// Sets the hardware packet filter.
        /// </summary>
        /// <param name="ndisPacketType">Type of the ndis packet.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool SetHwPacketFilter(Native.NdisApi.NDIS_PACKET_TYPE ndisPacketType)
        {
            return NdisApi.SetHwPacketFilter(this, ndisPacketType);
        }

        /// <summary>
        /// Gets the internal name.
        /// </summary>
        /// <param name="nameBytes">The name bytes.</param>
        /// <returns><see cref="string" />.</returns>
        private static string GetName(byte[] nameBytes)
        {
            var name = Encoding.ASCII.GetString(nameBytes);
            var i = name.IndexOf((char) 0);
            return i >= 0 ? name.Substring(0, i) : name;
        }

        /// <summary>
        /// Gets the friendly name, as shown in Windows' control panel.
        /// </summary>
        /// <param name="nameBytes">The name bytes.</param>
        /// <returns><see cref="string" />.</returns>
        private static string GetFriendlyName(byte[] nameBytes)
        {
            var lpVersionInformation = new NtDll.OSVERSIONINFOEX();
            lpVersionInformation.dwOSVersionInfoSize = (uint) Marshal.SizeOf(lpVersionInformation);
            NtDll.RtlGetVersion(ref lpVersionInformation);

            return ConvertAdapterName(nameBytes, 0, lpVersionInformation.dwPlatformId, lpVersionInformation.dwMajorVersion);
        }

        /// <summary>
        /// Converts the name of the adapter.
        /// </summary>
        /// <param name="adapterNameBytes">Bytes of the adapter name.</param>
        /// <param name="nameStart">The start of the name.</param>
        /// <param name="platformId">The OS platform identifier.</param>
        /// <param name="majorVersion">The major OS version.</param>
        /// <returns><see cref="string" />.</returns>
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
                        {
                            ++indexOfZero;
                        }

                        friendlyName = Encoding.Default.GetString(friendlyNameBytes, 0, indexOfZero);
                    }
                }

                return friendlyName;
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is NetworkAdapter networkAdapter)
            {
                return networkAdapter.Handle == Handle;
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            // Copied from .NET Runtime, as .NET Framework has an implementation that quickly collides.
            long l = (long) Handle;
            return unchecked((int) l) ^ (int) (l >> 32);
        }
    }
}