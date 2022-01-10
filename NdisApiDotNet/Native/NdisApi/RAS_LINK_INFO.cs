// ----------------------------------------------
// <copyright file="NdisApi.RAS_LINK_INFO.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
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
    /// <summary>
    /// WAN link definitions.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RasLinkInfo // RAS_LINK_INFO
    {
        internal uint _linkSpeed;
        internal uint _maximumTotalSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NdisApi.ETHER_ADDR_LENGTH)]
        internal byte[] _remoteAddress;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NdisApi.ETHER_ADDR_LENGTH)]
        internal byte[] _localAddress;
        internal uint _protocolBufferLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NdisApi.RAS_LINK_BUFFER_LENGTH)]
        internal byte[] _protocolBuffer;

        /// <summary>
        /// Gets or sets the speed of the link, in units of 100 bps.
        /// </summary>
        public uint LinkSpeed
        {
            get
            {
                return _linkSpeed;
            }

            set
            {
                _linkSpeed = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of bytes per packet that the protocol can send over the network.
        /// </summary>
        /// <remarks>
        /// Zero indicates no change from the speed returned when the protocol called NdisRequest with OID_GEN_LINK_SPEED.
        /// </remarks>
        public uint MaximumTotalSize
        {
            get
            {
                return _maximumTotalSize;
            }

            set
            {
                _maximumTotalSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the address of the remote node on the link in Ethernet-style format. NDISWAN supplies this value.
        /// </summary>
        /// <remarks>
        /// Zero indicates no change from the value returned when the protocol called NdisRequest with OID_GEN_MAXIMUM_TOTAL_SIZE.
        /// </remarks>
        public byte[] RemoteAddressBytes
        {
            get
            {
                return _remoteAddress;
            }

            set
            {
                _remoteAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the address of the remote node on the link in Ethernet-style format. NDISWAN supplies this value.
        /// </summary>
        public PhysicalAddress RemoteAddress
        {
            get
            {
                return new PhysicalAddress(RemoteAddressBytes);
            }

            set
            {
                RemoteAddressBytes = value.GetAddressBytes();
            }
        }

        /// <summary>
        /// Gets or sets the protocol-determined context for indications on this link in Ethernet-style format.
        /// </summary>
        public byte[] LocalAddressBytes
        {
            get
            {
                return _localAddress;
            }

            set
            {
                _localAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the protocol-determined context for indications on this link in Ethernet-style format.
        /// </summary>
        public PhysicalAddress LocalAddress
        {
            get
            {
                return new PhysicalAddress(LocalAddressBytes);
            }

            set
            {
                LocalAddressBytes = value.GetAddressBytes();
            }
        }

        /// <summary>
        /// Gets or sets the number of bytes in the buffer at ProtocolBuffer.
        /// </summary>
        public uint ProtocolBufferLength
        {
            get
            {
                return _protocolBufferLength;
            }

            set
            {
                _protocolBufferLength = value;
            }
        }

        /// <summary>
        /// Gets or sets the protocol-specific information supplied by a higher-level component that makes connections through NDISWAN to the
        /// appropriate protocol(s). Maximum size is 600 bytes on Windows Vista.
        /// </summary>
        public byte[] ProtocolBuffer
        {
            get
            {
                return _protocolBuffer;
            }

            set
            {
                _protocolBuffer = value;
            }
        }
    }
}