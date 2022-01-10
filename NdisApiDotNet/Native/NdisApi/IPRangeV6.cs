// ----------------------------------------------
// <copyright file="NdisApi.IP_RANGE_V6.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using NdisApiDotNet.Extensions;
using System.Net;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    /// <summary>
    /// The IPv6 range.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IPRangeV6 // IP_RANGE_V6
    {
        internal IN6_ADDR m_StartIp;
        internal IN6_ADDR m_EndIp;

        /// <summary>
        /// Initializes a new instance of the <see cref="IPRangeV6" /> struct.
        /// </summary>
        /// <param name="addressStart">The ip address start.</param>
        /// <param name="addressEnd">The ip address end.</param>
        public IPRangeV6(IPAddress addressStart, IPAddress addressEnd)
        {
            m_StartIp = addressStart;
            m_EndIp = addressEnd;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IPRangeV6" /> struct.
        /// </summary>
        /// <param name="addressStartEnd">The ip address start end.</param>
        public IPRangeV6(IPAddress addressStartEnd) : this(addressStartEnd, addressStartEnd) { }

        /// <summary>
        /// Gets or sets the start IP v6 address.
        /// </summary>
        public IN6_ADDR StartRaw
        {
            get
            {
                return m_StartIp;
            }

            set
            {
                m_StartIp = value;
            }
        }

        /// <summary>
        /// Gets or sets the end IP v6 address.
        /// </summary>
        public IN6_ADDR EndRaw
        {
            get
            {
                return m_EndIp;
            }

            set
            {
                m_EndIp = value;
            }
        }

        /// <summary>
        /// Gets or sets the start ip address.
        /// </summary>
        public IPAddress Start
        {
            get
            {
                return StartRaw;
            }

            set
            {
                StartRaw = value;
            }
        }

        /// <summary>
        /// Gets or sets the end ip address.
        /// </summary>
        public IPAddress End
        {
            get
            {
                return EndRaw;
            }

            set
            {
                EndRaw = value;
            }
        }
    }
}