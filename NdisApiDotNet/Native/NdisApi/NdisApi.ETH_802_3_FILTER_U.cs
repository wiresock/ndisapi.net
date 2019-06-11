// ----------------------------------------------
// <copyright file="NdisApi.ETH_802_3_FILTER_U.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        /// <summary>
        /// Ethernet 802.3 filter type.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ETH_802_3_FILTER_U
        {
            internal ETH_802_3_FLAGS m_ValidFields;
            internal fixed byte m_SrcAddress[ETHER_ADDR_LENGTH];
            internal fixed byte m_DestAddress[ETHER_ADDR_LENGTH];
            internal ushort m_Protocol;
            internal ushort _padding;

            /// <summary>
            /// Gets or sets which of the fields below contain valid values and should be matched against the packet.
            /// </summary>
            public ETH_802_3_FLAGS ValidFields
            {
                get => m_ValidFields;
                set => m_ValidFields = value;
            }

            /// <summary>
            /// Gets or sets the ether type.
            /// </summary>
            public ushort Protocol
            {
                get => m_Protocol;
                set => m_Protocol = value;
            }

            /// <summary>
            /// Gets or sets the padding.
            /// </summary>
            /// <remarks>This is currently unused.</remarks>
            public ushort Padding
            {
                get => _padding;
                set => _padding = value;
            }

            /// <summary>
            /// Gets the source address.
            /// </summary>
            /// <returns><see cref="System.Byte" />s.</returns>
            public byte[] GetSourceAddress()
            {
                var bytes = new byte[ETHER_ADDR_LENGTH];

                fixed (byte* b = m_SrcAddress)
                {
                    Marshal.Copy((IntPtr)b, bytes, 0, ETHER_ADDR_LENGTH);
                }

                return bytes;
            }

            /// <summary>
            /// Gets the destination address.
            /// </summary>
            /// <returns><see cref="System.Byte" />s.</returns>
            public byte[] GetDestinationAddress()
            {
                var bytes = new byte[ETHER_ADDR_LENGTH];

                fixed (byte* b = m_DestAddress)
                {
                    Marshal.Copy((IntPtr)b, bytes, 0, ETHER_ADDR_LENGTH);
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
                    m_SrcAddress[i] = bytes[i];
            }

            /// <summary>
            /// Sets the destination address.
            /// </summary>
            /// <param name="bytes">The bytes.</param>
            public void SetDestinationAddress(byte[] bytes)
            {
                for (int i = 0; i < ETHER_ADDR_LENGTH; i++)
                    m_DestAddress[i] = bytes[i];
            }
        }
    }
}