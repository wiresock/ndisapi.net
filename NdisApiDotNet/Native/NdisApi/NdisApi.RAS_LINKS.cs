// ----------------------------------------------
// <copyright file="NdisApi.RAS_LINKS.cs" company="NT Kernel">
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
        /// RAS links.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RAS_LINKS
        {
            internal uint nNumberOfLinks;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = RAS_LINKS_MAX)]
            internal RAS_LINK_INFO[] _rasLinks;

            /// <summary>
            /// Gets or sets the number of RAS links.
            /// </summary>
            public uint LinksCount
            {
                get => nNumberOfLinks;
                set => nNumberOfLinks = value;
            }

            /// <summary>
            /// Gets or sets the RAS links.
            /// </summary>
            public RAS_LINK_INFO[] RasLinks
            {
                get => _rasLinks;
                set => _rasLinks = value;
            }
        }
    }
}