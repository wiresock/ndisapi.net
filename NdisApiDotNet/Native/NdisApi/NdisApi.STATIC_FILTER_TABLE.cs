// ----------------------------------------------
// <copyright file="NdisApi.STATIC_FILTER_TABLE.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// The static filters table to be passed to WinPkFilter driver.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct STATIC_FILTER_TABLE
    {
        /// <summary>
        /// The number of <see cref="STATIC_FILTER" /> entries.
        /// </summary>
        public uint m_TableSize;

        /// <summary>
        /// The <see cref="STATIC_FILTER" />s.
        /// </summary>
        public STATIC_FILTER m_StaticFilters; // This is an array of STATIC_FILTER, but this cannot be declared directly as it's a variable width.

        /// <summary>
        /// Gets the <see cref="STATIC_FILTER" />s.
        /// </summary>
        public STATIC_FILTER* StaticFilters => (STATIC_FILTER*) Unsafe.AsPointer(ref m_StaticFilters);

        /// <summary>
        /// Gets or sets the <see cref="STATIC_FILTER" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns><see cref="STATIC_FILTER" />.</returns>
        public STATIC_FILTER this[int index]
        {
            get => StaticFilters[index];
            set => StaticFilters[index] = value;
        }

        /// <summary>
        /// Gets the static filters.
        /// </summary>
        /// <returns><see cref="STATIC_FILTER" />s.</returns>
        public STATIC_FILTER[] GetStaticFilters()
        {
            var staticFiltersSize = m_TableSize;

            var staticFilters = new STATIC_FILTER[staticFiltersSize];
            var staticFiltersPtr = StaticFilters;

            for (int i = 0; i < staticFiltersSize; i++)
            {
                staticFilters[i] = staticFiltersPtr[i];
            }

            return staticFilters;
        }

        /// <summary>
        /// Sets the static filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public void SetStaticFilters(STATIC_FILTER[] filters)
        {
            var staticFiltersPtr = StaticFilters;

            for (int i = 0; i < filters.Length; i++)
            {
                staticFiltersPtr[i] = filters[i];
            }

            m_TableSize = (uint) filters.Length;
        }

        /// <summary>
        /// The size of <see cref="STATIC_FILTER_TABLE" /> without the <see cref="StaticFilters" />.
        /// </summary>
        public static int SizeOfHeader = Marshal.SizeOf<STATIC_FILTER_TABLE>() - STATIC_FILTER.Size;
    }
}