// ----------------------------------------------
// <copyright file="NdisApi.FAST_IO_WRITE_UNION.cs" company="NT Kernel">
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
        [StructLayout(LayoutKind.Explicit)]
        public struct FAST_IO_WRITE_UNION
        {
            /// <summary>
            /// The union split.
            /// </summary>
            [FieldOffset(0)]
            public FAST_IO_WRITE_UNION_SPLIT split;

            /// <summary>
            /// The join.
            /// </summary>
            [FieldOffset(0)]
            public volatile int join;
        }
    }
}