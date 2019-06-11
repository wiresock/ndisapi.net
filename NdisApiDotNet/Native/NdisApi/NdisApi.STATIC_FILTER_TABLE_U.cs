// ----------------------------------------------
// <copyright file="NdisApi.STATIC_FILTER_TABLE_U.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
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
        public unsafe struct STATIC_FILTER_TABLE_U
        {
            internal uint m_TableSize;
            internal STATIC_FILTER_U* m_StaticFilters;

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
            /// <para>Use <see cref="GetStaticFilters" /> and <see cref="SetStaticFilters" /> instead. This field should not be relied upon.</para>
            public STATIC_FILTER_U* StaticFilters
            {
                get => m_StaticFilters;
                set => m_StaticFilters = value;
            }

            /// <summary>
            /// Gets the static filters.
            /// </summary>
            /// <param name="filters">The filters.</param>
            /// <returns><see cref="STATIC_FILTER" />s.</returns>
            public STATIC_FILTER_U[] GetStaticFilters(uint filters = 0)
            {
                var filterSize = filters == 0 ? TableSize : filters;

                var staticFilters = new STATIC_FILTER_U[filterSize];
                var pinnedStaticFilters = GCHandle.Alloc(staticFilters, GCHandleType.Pinned);

                var sizeToCopy = StaticFilterUSize * filterSize;
                fixed (STATIC_FILTER_TABLE_U* a = &this)
                {
                    MemoryCopy((void*)((IntPtr)a + StaticFilterTableUStaticFiltersOffset), (void*)pinnedStaticFilters.AddrOfPinnedObject(), sizeToCopy);
                }

                pinnedStaticFilters.Free();

                return staticFilters;
            }

            /// <summary>
            /// Sets the static filters.
            /// </summary>
            /// <param name="filters">The filters.</param>
            public void SetStaticFilters(STATIC_FILTER_U[] filters)
            {
                var pinnedStaticFilters = GCHandle.Alloc(filters, GCHandleType.Pinned);

                var sizeToCopy = StaticFilterUSize * filters.Length;
                fixed (STATIC_FILTER_TABLE_U* a = &this)
                {
                    MemoryCopy((void*)pinnedStaticFilters.AddrOfPinnedObject(), (void*)((IntPtr)a + StaticFilterTableUStaticFiltersOffset), sizeToCopy);
                }

                pinnedStaticFilters.Free();
            }
        }
    }
}