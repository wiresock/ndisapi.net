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
    /// <summary>
    /// RAS links.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RasLinks // RAS_LINKS
    {
        internal uint nNumberOfLinks;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.RAS_LINKS_MAX)]
        internal RasLinkInfo[] _rasLinks;

        /// <summary>
        /// Gets or sets the number of RAS links.
        /// </summary>
        public uint LinksCount
        {
            get
            {
                return nNumberOfLinks;
            }

            set
            {
                nNumberOfLinks = value;
            }
        }

        /// <summary>
        /// Gets or sets the RAS links.
        /// </summary>
        public RasLinkInfo[] Links
        {
            get
            {
                return _rasLinks;
            }

            set
            {
                _rasLinks = value;
            }
        }
    }
}