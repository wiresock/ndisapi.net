// ----------------------------------------------
// <copyright file="NdisApi.PORT_RANGE.cs" company="NT Kernel">
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
    /// The port range.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PortRange // PORT_RANGE
    {
        internal ushort m_StartRange;
        internal ushort m_EndRange;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortRange"/> struct.
        /// </summary>
        /// <param name="start">The start port.</param>
        /// <param name="end">The end port.</param>
        public PortRange(ushort start, ushort end)
        {
            m_StartRange = start;
            m_EndRange = end;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortRange"/> struct.
        /// </summary>
        /// <param name="port">The port.</param>
        public PortRange(ushort port)
        {
            m_StartRange = port;
            m_EndRange = port;
        }

        public static implicit operator PortRange(ushort port)
        {
            return new PortRange(port);
        }

        /// <summary>
        /// Gets or sets the end of the range.
        /// </summary>
        public ushort End
        {
            get
            {
                return m_EndRange;
            }

            set
            {
                m_EndRange = value;
            }
        }

        /// <summary>
        /// Gets or sets the start of the range.
        /// </summary>
        public ushort Start
        {
            get
            {
                return m_StartRange;
            }

            set
            {
                m_StartRange = value;
            }
        }
    }
}