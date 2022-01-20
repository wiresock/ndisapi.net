// ----------------------------------------------
// <copyright file="NdisApi.RAS_LINK_INFO.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        /// <summary>
        /// WAN link definitions.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RAS_LINK_INFO
        {
            /// <summary>
            /// The speed of the link, in units of 100 bps.
            /// </summary>
            public uint LinkSpeed;

            /// <summary>
            /// The maximum number of bytes per packet that the protocol can send over the network.
            /// </summary>
            public uint MaximumTotalSize;

            /// <summary>
            /// The address of the remote node on the link in Ethernet-style format. NDISWAN supplies this value.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = ETHER_ADDR_LENGTH)]
            public byte[] RemoteAddress;

            /// <summary>
            /// The address of the local node on the link in Ethernet-style format. NDISWAN supplies this value.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = ETHER_ADDR_LENGTH)]
            public byte[] LocalAddress;

            /// <summary>
            /// The number of bytes in the buffer at <see cref="ProtocolBuffer" />.
            /// </summary>
            public uint ProtocolBufferLength;

            /// <summary>
            /// Contains protocol-specific information supplied by a higher-level component that makes connections through NDISWAN to the appropriate protocol(s).
            /// The maximum observed size is 600 bytes on Windows Vista, 1200 on Windows 10.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = RAS_LINK_BUFFER_LENGTH)]
            public byte[] ProtocolBuffer;

            /// <summary>
            /// Gets or sets the address of the remote node on the link in Ethernet-style format. NDISWAN supplies this value.
            /// </summary>
            public PhysicalAddress RemotePhysicalAddress
            {
                get => new(RemoteAddress);
                set => RemoteAddress = value.GetAddressBytes();
            }

            /// <summary>
            /// Gets or sets the protocol-determined context for indications on this link in Ethernet-style format.
            /// </summary>
            public PhysicalAddress LocalPhysicalAddress
            {
                get => new(LocalAddress);
                set => LocalAddress = value.GetAddressBytes();
            }
        }
    }
}