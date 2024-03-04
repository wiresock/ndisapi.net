// ----------------------------------------------
// <copyright file="NdisApi.FAST_IO_SECTION.cs" company="NT Kernel">
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
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct FAST_IO_SECTION
	{
		/// <summary>
		/// The header.
		/// </summary>
		public FAST_IO_SECTION_HEADER fast_io_header;

		/// <summary>
		/// The first <see cref="INTERMEDIATE_BUFFER"/>.
		/// </summary>
		public INTERMEDIATE_BUFFER fast_io_packets; // This is an array of INTERMEDIATE_BUFFER, but this cannot be declared directly as it's a variable width.

		/// <summary>
		/// Gets the <see cref="INTERMEDIATE_BUFFER"/>s.
		/// </summary>
		public unsafe Span<INTERMEDIATE_BUFFER> IntermediateBuffers
		{
			get
			{
#if NETCOREAPP
				return MemoryMarshal.CreateSpan(ref fast_io_packets, fast_io_header.fast_io_write_union.split.number_of_packets);
#else
				fixed (INTERMEDIATE_BUFFER* packets = &fast_io_packets)
				{
					return new Span<INTERMEDIATE_BUFFER>(packets, fast_io_header.fast_io_write_union.split.number_of_packets);
				}
#endif
			}
		}

		/// <summary>
		/// The size of <see cref="FAST_IO_SECTION" /> without the <see cref="IntermediateBuffers" />.
		/// </summary>
		public static int SizeOfHeader = FAST_IO_SECTION_HEADER.Size;
	}
}