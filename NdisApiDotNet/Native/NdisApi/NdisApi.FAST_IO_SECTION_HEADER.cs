// ----------------------------------------------
// <copyright file="NdisApi.FAST_IO_SECTION_HEADER.cs" company="NT Kernel">
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
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FAST_IO_SECTION_HEADER
    {
        /// <summary>
        /// The write union.
        /// </summary>
        public FAST_IO_WRITE_UNION fast_io_write_union;

        /// <summary>
        /// The flag specifying whether read is in progress.
        /// </summary>
        public volatile int read_in_progress_flag;

        /// <summary>
        /// The size of <see cref="FAST_IO_SECTION_HEADER" />.
        /// </summary>
        public static int Size = Marshal.SizeOf<FAST_IO_SECTION_HEADER>();
    }
}