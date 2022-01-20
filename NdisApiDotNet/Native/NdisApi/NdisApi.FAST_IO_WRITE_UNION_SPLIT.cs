// ----------------------------------------------
// <copyright file="NdisApi.FAST_IO_WRITE_UNION_SPLIT.cs" company="NT Kernel">
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
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FAST_IO_WRITE_UNION_SPLIT
        {
            /// <summary>
            /// The number of packets.
            /// </summary>
            public volatile ushort number_of_packets;

            /// <summary>
            /// The flag specifying whether write is in progress.
            /// </summary>
            public volatile ushort write_in_progress_flag;
        }
    }
}