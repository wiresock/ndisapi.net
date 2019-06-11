// ----------------------------------------------
// <copyright file="NdisApi.IP_RANGE_V6.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
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
            internal IN6_ADDR m_StartIp;
            internal IN6_ADDR m_EndIp;

            /// <summary>
            /// Initializes a new instance of the <see cref="IP_RANGE_V6" /> struct.
            /// </summary>
            /// <param name="addressStart">The ip address start.</param>
            /// <param name="addressEnd">The ip address end.</param>
            public IP_RANGE_V6(IPAddress addressStart, IPAddress addressEnd)
            {
                m_StartIp = addressStart.ToIN6_ADDR();
                m_EndIp = addressEnd.ToIN6_ADDR();
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="IP_RANGE_V6" /> struct.
            /// </summary>
            /// <param name="addressStartEnd">The ip address start end.</param>
            public IP_RANGE_V6(IPAddress addressStartEnd)
            {
                m_StartIp = addressStartEnd.ToIN6_ADDR();
                m_EndIp = addressStartEnd.ToIN6_ADDR();
            }

            /// <summary>
            /// Gets or sets the start IP v6 address.
            /// </summary>
            public IN6_ADDR StartRaw
            {
                get => m_StartIp;
                set => m_StartIp = value;
            }

            /// <summary>
            /// Gets or sets the end IP v6 address.
            /// </summary>
            public IN6_ADDR EndRaw
            {
                get => m_EndIp;
                set => m_EndIp = value;
            }

            /// <summary>
            /// Gets or sets the start ip address.
            /// </summary>
            public IPAddress Start
            {
                get => StartRaw.ToIPAddress();
                set => StartRaw = value.ToIN6_ADDR();
            }

            /// <summary>
            /// Gets or sets the end ip address.
            /// </summary>
            public IPAddress End
            {
                get => EndRaw.ToIPAddress();
                set => EndRaw = value.ToIN6_ADDR();
            }
        }
    }
}