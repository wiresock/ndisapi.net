// ----------------------------------------------
// <copyright file="NdisApi.STATIC_FILTER_TABLE.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;
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
    public struct STATIC_FILTER_TABLE
    {
        /// <summary>
        /// The number of <see cref="STATIC_FILTER" /> entries.
        /// </summary>
        public uint m_TableSize;

		private readonly uint m_Padding;

		/// <summary>
		/// Gets the first <see cref="STATIC_FILTER" />s.
		/// </summary>
		public STATIC_FILTER m_StaticFilters; // This is an array of STATIC_FILTER, but this cannot be declared directly as it's a variable width.

        /// <summary>
        /// Gets the <see cref="STATIC_FILTER" />s.
        /// </summary>
        public unsafe Span<STATIC_FILTER> StaticFilters
		{
			get
			{
#if NETCOREAPP
				return MemoryMarshal.CreateSpan(ref m_StaticFilters, (int)m_TableSize);
#else
				fixed (STATIC_FILTER* staticFilter = &m_StaticFilters)
				{
					return new Span<STATIC_FILTER>(staticFilter, (int)m_TableSize);
				}
#endif
			}
		}

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
        /// The size of <see cref="STATIC_FILTER_TABLE" /> without the <see cref="StaticFilters" />.
        /// </summary>
        public static int SizeOfHeader = Marshal.SizeOf<STATIC_FILTER_TABLE>() - STATIC_FILTER.Size;
    }
}