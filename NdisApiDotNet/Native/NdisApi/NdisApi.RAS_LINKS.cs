// ----------------------------------------------
// <copyright file="NdisApi.RAS_LINKS.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
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
        /// RAS links.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RAS_LINKS
        {
            /// <summary>
            /// The number of RAS links.
            /// </summary>
            public uint nNumberOfLinks;

            /// <summary>
            /// The RAS links.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = RAS_LINKS_MAX)]
            public RAS_LINK_INFO[] RasLinks;

            /// <summary>
            /// The size of <see cref="RAS_LINKS" />.
            /// </summary>
            public static int Size = Marshal.SizeOf<RAS_LINKS>();
        }
    }
}