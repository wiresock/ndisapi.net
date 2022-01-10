using System;

namespace NdisApiDotNet.Filters
{
    /// <summary>
    /// Filter flags.
    /// </summary>
    [Flags]
    public enum FilterFields : uint // ETH_802_3_FLAGS, IP_V4_FILTER_FIELDS, IP_V6_FILTER_FIELDS
    {
        /// <summary>
        /// The eth 802 3 source address.
        /// </summary>
        Source = 0x00000001, // ETH_802_3_SRC_ADDRESS, IP_V4_FILTER_SRC_ADDRESS, IP_V6_FILTER_SRC_ADDRESS

        /// <summary>
        /// The eth 802 3 destination address.
        /// </summary>
        Destination = 0x00000002, // ETH_802_3_DEST_ADDRESS, IP_V4_FILTER_DEST_ADDRESS, IP_V6_FILTER_DEST_ADDRESS

        /// <summary>
        /// The eth 802 3/IPv4 filter/IPv6 filter protocol.
        /// </summary>
        Protocol = 0x00000004, // ETH_802_3_PROTOCOL, IP_V4_FILTER_PROTOCOL, IP_V6_FILTER_PROTOCOL, TCPUDP_TCP_FLAGS

        /// <summary>
        /// The TCP flags.
        /// </summary>
        TCPFlags = Protocol // TCPUDP_TCP_FLAGS
    }
}