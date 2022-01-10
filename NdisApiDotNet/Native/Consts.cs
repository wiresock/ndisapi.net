using NdisApiDotNet.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NdisApiDotNet.Native
{
    public static class Consts
    {
        /// <summary>
        /// The file name of the Ndis Api DLL.
        /// </summary>
        public const string DllName = Imports.DllName;

        /// <summary>
        /// The ether multiple request eth packet size
        /// </summary>
        public const int EthMRequestEthPacketSize = 256;

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

        // Sizes & Offsets
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

        static unsafe Consts()
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
    }
}
