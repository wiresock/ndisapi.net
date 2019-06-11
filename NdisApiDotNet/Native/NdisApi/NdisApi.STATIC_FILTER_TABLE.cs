// ----------------------------------------------
// <copyright file="NdisApi.STATIC_FILTER_TABLE.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        /// <summary>
        /// The static filters table to be passed to WinPkFilter driver.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
        public struct STATIC_FILTER_TABLE
        {
            internal uint m_TableSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            internal STATIC_FILTER[] m_StaticFilters;

            /// <summary>
            /// Gets or sets the number of STATIC_FILTER entries.
            /// </summary>
            public uint TableSize
            {
                get => m_TableSize;
                set => m_TableSize = value;
            }

            /// <summary>
            /// Gets or sets the static filters.
            /// </summary>
            /// <remarks>
            /// For convenience the size of the array is fixed to 256 entries, feel free to change this value if you need more filter
            /// entries.
            /// </remarks>
            public STATIC_FILTER[] StaticFilters
            {
                get => m_StaticFilters;
                set => m_StaticFilters = value;
            }
        }
    }
}