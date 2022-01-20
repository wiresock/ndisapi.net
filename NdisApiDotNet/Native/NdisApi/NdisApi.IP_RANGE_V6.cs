// ----------------------------------------------
// <copyright file="NdisApi.IP_RANGE_V6.cs" company="NT Kernel">
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
        /// The IPv6 range.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IP_RANGE_V6
        {
            /// <summary>
            /// The start IP address.
            /// </summary>
            public IN6_ADDR m_StartIp;

            /// <summary>
            /// The end IP address.
            /// </summary>
            public IN6_ADDR m_EndIp;

            /// <summary>
            /// Initializes a new instance of the <see cref="IP_RANGE_V6" /> struct.
            /// </summary>
            /// <param name="addressStart">The IP address start.</param>
            /// <param name="addressEnd">The IP address end.</param>
            public IP_RANGE_V6(IPAddress addressStart, IPAddress addressEnd)
            {
                m_StartIp = addressStart.ToIN6_ADDR();
                m_EndIp = addressEnd.ToIN6_ADDR();
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="IP_RANGE_V6" /> struct.
            /// </summary>
            /// <param name="addressStartEnd">The IP address start end.</param>
            public IP_RANGE_V6(IPAddress addressStartEnd)
            {
                m_StartIp = addressStartEnd.ToIN6_ADDR();
                m_EndIp = addressStartEnd.ToIN6_ADDR();
            }

            /// <summary>
            /// Gets or sets the start IP address.
            /// </summary>
            public IPAddress Start
            {
                get => m_StartIp.ToIPAddress();
                set => m_StartIp = value.ToIN6_ADDR();
            }

            /// <summary>
            /// Gets or sets the end IP address.
            /// </summary>
            public IPAddress End
            {
                get => m_EndIp.ToIPAddress();
                set => m_EndIp = value.ToIN6_ADDR();
            }
        }
    }
}