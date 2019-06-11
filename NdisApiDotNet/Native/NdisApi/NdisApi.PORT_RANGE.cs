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
    public static partial class NdisApi
    {
        /// <summary>
        /// The port range.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PORT_RANGE
        {
            internal ushort m_StartRange;
            internal ushort m_EndRange;

            /// <summary>
            /// Initializes a new instance of the <see cref="PORT_RANGE"/> struct.
            /// </summary>
            /// <param name="start">The start port.</param>
            /// <param name="end">The end port.</param>
            public PORT_RANGE(ushort start, ushort end)
            {
                m_StartRange = start;
                m_EndRange = end;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="PORT_RANGE"/> struct.
            /// </summary>
            /// <param name="port">The port.</param>
            public PORT_RANGE(ushort port)
            {
                m_StartRange = port;
                m_EndRange = port;
            }

            /// <summary>
            /// Gets or sets the end of the range.
            /// </summary>
            public ushort End
            {
                get => m_EndRange;
                set => m_EndRange = value;
            }

            /// <summary>
            /// Gets or sets the start of the range.
            /// </summary>
            public ushort Start
            {
                get => m_StartRange;
                set => m_StartRange = value;
            }
        }
    }
}