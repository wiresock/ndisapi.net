// ----------------------------------------------
// <copyright file="NdisApi.PORT_RANGE.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// The port range.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PORT_RANGE
    {
        /// <summary>
        /// The start of the range.
        /// </summary>
        public ushort m_StartRange;

        /// <summary>
        /// The end of the range.
        /// </summary>
        public ushort m_EndRange;

        /// <summary>
        /// Initializes a new instance of the <see cref="PORT_RANGE" /> struct.
        /// </summary>
        /// <param name="start">The start port.</param>
        /// <param name="end">The end port.</param>
        public PORT_RANGE(ushort start, ushort end)
        {
            m_StartRange = start;
            m_EndRange = end;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PORT_RANGE" /> struct.
        /// </summary>
        /// <param name="port">The port.</param>
        public PORT_RANGE(ushort port)
        {
            m_StartRange = port;
            m_EndRange = port;
        }
    }
}