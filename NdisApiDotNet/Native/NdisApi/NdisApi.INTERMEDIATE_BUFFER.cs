// ----------------------------------------------
// <copyright file="NdisApi.INTERMEDIATE_BUFFER.cs" company="NT Kernel">
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
    public static partial class NdisApi
    {
        /// <summary>
        /// Contains packet buffer, packet NDIS flags, WinPkFilter specific flags.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct INTERMEDIATE_BUFFER
        {
            internal LIST_ENTRY m_qLink;
            internal PACKET_FLAG m_dwDeviceFlags;
            internal uint m_Length;
            internal NDIS_FLAGS m_Flags;
            internal uint m_8021q;
            internal uint m_FilterID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            internal uint[] m_Reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ETHER_FRAME)]
            internal byte[] m_IBuffer;

            /// <summary>
            /// Gets or sets the filter identifier.
            /// </summary>
            public uint FilterId
            {
                get => m_FilterID;
                set => m_FilterID = value;
            }

            /// <summary>
            /// Gets or sets the buffer.
            /// </summary>
            public byte[] Buffer
            {
                get => m_IBuffer;
                set => m_IBuffer = value;
            }

            /// <summary>
            /// Gets or sets the 802.1q info.
            /// </summary>
            public uint Dot1q
            {
                get => m_8021q;
                set => m_8021q = value;
            }

            /// <summary>
            /// Gets or sets the reserved values.
            /// </summary>
            public uint[] Reserved
            {
                get => m_Reserved;
                set => m_Reserved = value;
            }

            /// <summary>
            /// Gets or sets the ndis packet flags.
            /// </summary>
            public NDIS_FLAGS PacketFlags
            {
                get => m_Flags;
                set => m_Flags = value;
            }

            /// <summary>
            /// Gets or sets the device flags.
            /// </summary>
            public PACKET_FLAG DeviceFlags
            {
                get => m_dwDeviceFlags;
                set => m_dwDeviceFlags = value;
            }

            /// <summary>
            /// Gets or sets the q link.
            /// </summary>
            public LIST_ENTRY QLink
            {
                get => m_qLink;
                set => m_qLink = value;
            }

            /// <summary>
            /// Gets or sets the length of the <see cref="Buffer"/>.
            /// </summary>
            public uint Length
            {
                get => m_Length;
                set => m_Length = value;
            }
        }
    }
}