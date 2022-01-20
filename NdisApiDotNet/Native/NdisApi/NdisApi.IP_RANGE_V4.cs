// ----------------------------------------------
// <copyright file="NdisApi.IP_RANGE_V4.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System.Net;
using System.Runtime.InteropServices;
using NdisApiDotNet.Extensions;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        /// <summary>
        /// The IPv4 range.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IP_RANGE_V4
        {
            /// <summary>
            /// The start IP address.
            /// </summary>
            public uint m_StartIp;

            /// <summary>
            /// The end IP address.
            /// </summary>
            public uint m_EndIp;

            /// <summary>
            /// Initializes a new instance of the <see cref="IP_RANGE_V4" /> struct.
            /// </summary>
            /// <param name="ipAddress">The IP address.</param>
            public IP_RANGE_V4(IPAddress ipAddress)
            {
                m_StartIp = ipAddress.ToUInt32();
                m_EndIp = ipAddress.ToUInt32();
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="IP_RANGE_V4" /> struct.
            /// </summary>
            /// <param name="startIPAddress">The start IP address.</param>
            /// <param name="endIPAddress">The end IP address.</param>
            public IP_RANGE_V4(IPAddress startIPAddress, IPAddress endIPAddress)
            {
                m_StartIp = startIPAddress.ToUInt32();
                m_EndIp = endIPAddress.ToUInt32();
            }

            /// <summary>
            /// Gets or sets the start IPv4 address.
            /// </summary>
            public IPAddress Start
            {
                get => m_StartIp.ToIPAddress();
                set => m_StartIp = value.ToUInt32();
            }

            /// <summary>
            /// Gets or sets the end IPv4 address.
            /// </summary>
            public IPAddress End
            {
                get => m_EndIp.ToIPAddress();
                set => m_EndIp = value.ToUInt32();
            }
        }
    }
}