// ----------------------------------------------
// <copyright file="NdisApi.IP_RANGE_V4.cs" company="NT Kernel">
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
    /// The IPv4 range.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IPRangeV4 // IP_RANGE_V4
    {
        internal uint m_StartIp;
        internal uint m_EndIp;

        /// <summary>
        /// Initializes a new instance of the <see cref="IPRangeV4" /> struct.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        public IPRangeV4(IPAddress ipAddress) : this(ipAddress, ipAddress) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IPRangeV4" /> struct.
        /// </summary>
        /// <param name="startIPAddress">The start ip address.</param>
        /// <param name="endIPAddress">The end ip address.</param>
        public IPRangeV4(IPAddress startIPAddress, IPAddress endIPAddress)
        {
            m_StartIp = startIPAddress.ToUInt32();
            m_EndIp = endIPAddress.ToUInt32();
        }

        /// <summary>
        /// Gets or sets the IPv4 address expressed as uint.
        /// </summary>
        public uint StartRaw
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
        /// Gets or sets the IPv4 address expressed as uint.
        /// </summary>
        public uint EndRaw
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
        /// Gets or sets the start IPv4 address.
        /// </summary>
        public IPAddress Start
        {
            get
            {
                return StartRaw.ToIPAddress();
            }

            set
            {
                StartRaw = value.ToUInt32();
            }
        }

        /// <summary>
        /// Gets or sets the end IPv4 address.
        /// </summary>
        public IPAddress End
        {
            get
            {
                return EndRaw.ToIPAddress();
            }

            set
            {
                EndRaw = value.ToUInt32();
            }
        }
    }
}