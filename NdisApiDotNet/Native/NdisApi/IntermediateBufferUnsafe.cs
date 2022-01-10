// ----------------------------------------------
// <copyright file="NdisApi.INTERMEDIATE_BUFFER_U.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    /// <summary>
    /// Contains packet buffer, packet NDIS flags, WinPkFilter specific flags.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct IntermediateBufferUnsafe // INTERMEDIATE_BUFFER_U
    {
        internal ListEntry m_qLink;
        internal PacketFlag m_dwDeviceFlags;
        internal uint m_Length;
        internal NdisFlags m_Flags;
        internal uint m_8021q;
        internal uint m_FilterID;
        internal fixed uint m_Reserved[4];
        internal fixed byte m_IBuffer[Consts.MAX_ETHER_FRAME];

        /// <summary>
        /// Gets or sets the filter identifier.
        /// </summary>
        public uint FilterId
        {
            get
            {
                return m_FilterID;
            }

            set
            {
                m_FilterID = value;
            }
        }

        /// <summary>
        /// Gets or sets the 802.1q info.
        /// </summary>
        public uint Dot1q
        {
            get
            {
                return m_8021q;
            }

            set
            {
                m_8021q = value;
            }
        }

        /// <summary>
        /// Gets or sets the ndis packet flags.
        /// </summary>
        public NdisFlags PacketFlags
        {
            get
            {
                return m_Flags;
            }

            set
            {
                m_Flags = value;
            }
        }

        /// <summary>
        /// Gets or sets the device flags.
        /// </summary>
        public PacketFlag DeviceFlags
        {
            get
            {
                return m_dwDeviceFlags;
            }

            set
            {
                m_dwDeviceFlags = value;
            }
        }

        /// <summary>
        /// Gets or sets the q link.
        /// </summary>
        public ListEntry QLink
        {
            get
            {
                return m_qLink;
            }

            set
            {
                m_qLink = value;
            }
        }

        /// <summary>
        /// Gets or sets the length of the <see cref="Buffer"/>.
        /// </summary>
        public uint Length
        {
            get
            {
                return m_Length;
            }

            set
            {
                m_Length = value;
            }
        }

        /// <summary>
        /// Gets the buffer.
        /// </summary>
        public byte* Buffer
        {
            get
            {
                fixed (byte* buffer = m_IBuffer)
                {
                    return buffer;
                }
            }
        }

        /// <summary>
        /// Gets the reserved values.
        /// </summary>
        public uint* Reserved
        {
            get
            {
                fixed (uint* reserved = m_Reserved)
                {
                    return reserved;
                }
            }
        }
    }
}