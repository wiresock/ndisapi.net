// ----------------------------------------------
// <copyright file="NdisApi.ETH_802_3_FILTER_U.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using NdisApiDotNet.Native;
using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Filters
{
    /// <summary>
    /// Ethernet 802.3 filter type.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct Eth802_3FilterUnsafe // ETH_802_3_FILTER_U
    {
        internal FilterFields m_ValidFields;
        internal fixed byte m_SrcAddress[Consts.ETHER_ADDR_LENGTH];
        internal fixed byte m_DestAddress[Consts.ETHER_ADDR_LENGTH];
        internal ushort m_Protocol;
        internal ushort _padding;

        /// <summary>
        /// Gets or sets which of the fields below contain valid values and should be matched against the packet.
        /// </summary>
        public FilterFields ValidFields
        {
            get
            {
                return m_ValidFields;
            }

            set
            {
                m_ValidFields = value;
            }
        }

        /// <summary>
        /// Gets or sets the ether type.
        /// </summary>
        public ushort Protocol
        {
            get
            {
                return m_Protocol;
            }

            set
            {
                m_Protocol = value;
            }
        }

        /// <summary>
        /// Gets or sets the padding.
        /// </summary>
        /// <remarks>This is currently unused.</remarks>
        public ushort Padding
        {
            get
            {
                return _padding;
            }

            set
            {
                _padding = value;
            }
        }

        /// <summary>
        /// Gets the source address.
        /// </summary>
        /// <returns><see cref="byte" />s.</returns>
        public byte[] GetSourceAddress()
        {
            byte[] bytes = new byte[Consts.ETHER_ADDR_LENGTH];

            fixed (byte* b = m_SrcAddress)
            {
                Marshal.Copy((IntPtr)b, bytes, 0, Consts.ETHER_ADDR_LENGTH);
            }

            return bytes;
        }

        /// <summary>
        /// Gets the destination address.
        /// </summary>
        /// <returns><see cref="byte" />s.</returns>
        public byte[] GetDestinationAddress()
        {
            byte[] bytes = new byte[Consts.ETHER_ADDR_LENGTH];

            fixed (byte* b = m_DestAddress)
            {
                Marshal.Copy((IntPtr)b, bytes, 0, Consts.ETHER_ADDR_LENGTH);
            }

            return bytes;
        }

        /// <summary>
        /// Sets the source address.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public void SetSourceAddress(byte[] bytes)
        {
            for (int i = 0; i < Consts.ETHER_ADDR_LENGTH; i++) m_SrcAddress[i] = bytes[i];
        }

        /// <summary>
        /// Sets the destination address.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public void SetDestinationAddress(byte[] bytes)
        {
            for (int i = 0; i < Consts.ETHER_ADDR_LENGTH; i++) m_DestAddress[i] = bytes[i];
        }
    }
}