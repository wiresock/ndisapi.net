// ----------------------------------------------
// <copyright file="NdisApi.STATIC_FILTER_TABLE_U.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using NdisApiDotNet.Native;
using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Filters
{
    /// <summary>
    /// The static filters table to be passed to WinPkFilter driver.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct StaticFilterTableUnsafe // STATIC_FILTER_TABLE_U
    {
        internal uint m_TableSize;
        internal StaticFilterUnsafe* m_StaticFilters;

        /// <summary>
        /// Gets or sets the number of STATIC_FILTER entries.
        /// </summary>
        public uint TableSize
        {
            get
            {
                return m_TableSize;
            }

            set
            {
                m_TableSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the static filters.
        /// </summary>
        /// <para>Use <see cref="GetStaticFilters" /> and <see cref="SetStaticFilters" /> instead. This field should not be relied upon.</para>
        public StaticFilterUnsafe* StaticFilters
        {
            get
            {
                return m_StaticFilters;
            }

            set
            {
                m_StaticFilters = value;
            }
        }

        /// <summary>
        /// Gets the static filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns><see cref="StaticFilter" />s.</returns>
        public StaticFilterUnsafe[] GetStaticFilters(uint filters = 0)
        {
            uint filterSize = filters == 0 ? TableSize : filters;

            StaticFilterUnsafe[] staticFilters = new StaticFilterUnsafe[filterSize];
            GCHandle pinnedStaticFilters = GCHandle.Alloc(staticFilters, GCHandleType.Pinned);

            long sizeToCopy = Consts.StaticFilterUSize * filterSize;
            fixed (StaticFilterTableUnsafe* a = &this)
            {
                Imports.MemoryCopy((void*)((IntPtr)a + Consts.StaticFilterTableUStaticFiltersOffset), (void*)pinnedStaticFilters.AddrOfPinnedObject(), sizeToCopy);
            }

            pinnedStaticFilters.Free();

            return staticFilters;
        }

        /// <summary>
        /// Sets the static filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public void SetStaticFilters(StaticFilterUnsafe[] filters)
        {
            GCHandle pinnedStaticFilters = GCHandle.Alloc(filters, GCHandleType.Pinned);

            int sizeToCopy = Consts.StaticFilterUSize * filters.Length;
            fixed (StaticFilterTableUnsafe* a = &this)
            {
                Imports.MemoryCopy((void*)pinnedStaticFilters.AddrOfPinnedObject(), (void*)((IntPtr)a + Consts.StaticFilterTableUStaticFiltersOffset), sizeToCopy);
            }

            pinnedStaticFilters.Free();
        }
    }
}