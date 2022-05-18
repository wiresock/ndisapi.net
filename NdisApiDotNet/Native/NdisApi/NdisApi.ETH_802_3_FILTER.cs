// ----------------------------------------------
// <copyright file="NdisApi.ETH_802_3_FILTER.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// Ethernet 802.3 filter type.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ETH_802_3_FILTER
    {
        /// <summary>
        /// Which fields below contain valid values and should be matched against the packet.
        /// </summary>
        public ETH_802_3_FLAGS m_ValidFields;

        /// <summary>
        /// The source address.
        /// </summary>
        public fixed byte m_SrcAddress[ETHER_ADDR_LENGTH];

        /// <summary>
        /// The destination address.
        /// </summary>
        public fixed byte m_DestAddress[ETHER_ADDR_LENGTH];

        /// <summary>
        /// The ether type.
        /// </summary>
        public ushort m_Protocol;

        /// <summary>
        /// The padding, this is currently unused.
        /// </summary>
        public ushort Padding;

        /// <summary>
        /// Gets the source address.
        /// </summary>
        /// <returns><see cref="byte" />s.</returns>
        public byte[] GetSourceAddress()
        {
            var bytes = new byte[ETHER_ADDR_LENGTH];

            for (int i = 0; i < ETHER_ADDR_LENGTH; i++)
            {
                bytes[i] = m_SrcAddress[i];
            }

            return bytes;
        }

        /// <summary>
        /// Gets the destination address.
        /// </summary>
        /// <returns><see cref="byte" />s.</returns>
        public byte[] GetDestinationAddress()
        {
            var bytes = new byte[ETHER_ADDR_LENGTH];

            for (int i = 0; i < ETHER_ADDR_LENGTH; i++)
            {
                bytes[i] = m_DestAddress[i];
            }

            return bytes;
        }

        /// <summary>
        /// Sets the source address.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public void SetSourceAddress(byte[] bytes)
        {
            for (int i = 0; i < ETHER_ADDR_LENGTH; i++)
            {
                m_SrcAddress[i] = bytes[i];
            }
        }

        /// <summary>
        /// Sets the destination address.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public void SetDestinationAddress(byte[] bytes)
        {
            for (int i = 0; i < ETHER_ADDR_LENGTH; i++)
            {
                m_DestAddress[i] = bytes[i];
            }
        }
    }
}