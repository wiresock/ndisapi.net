// ----------------------------------------------
// <copyright file="NdisApi.ETH_M_REQUEST.cs" company="NT Kernel">
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
	/// Used for passing the blocks of packets to and from the driver.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ETH_M_REQUEST
	{
		/// <summary>
		/// The adapter handle.
		/// </summary>
		public IntPtr hAdapterHandle;

		/// <summary>
		/// The total number of packets that can be set.
		/// </summary>
		public uint dwPacketsNumber;

		/// <summary>
		/// The number of packets that were successfully returned.
		/// </summary>
		public uint dwPacketsSuccess;

		/// <summary>
		/// The first <see cref="NDISRD_ETH_Packet" />.
		/// </summary>
		public NDISRD_ETH_Packet EthPacket; // This is an array of NDISRD_ETH_Packet, but this cannot be declared directly as it's a variable width.

		/// <summary>
		/// Gets the <see cref="NDISRD_ETH_Packet" />s.
		/// </summary>
		public unsafe Span<NDISRD_ETH_Packet> EthPackets
		{
			get
			{
#if NETCOREAPP
				return MemoryMarshal.CreateSpan(ref EthPacket, (int)dwPacketsNumber);
#else
				fixed (NDISRD_ETH_Packet* packets = &EthPacket)
				{
					return new Span<NDISRD_ETH_Packet>(packets, (int)dwPacketsNumber);
				}
#endif
			}
		}

		/// <summary>
		/// The size of <see cref="ETH_M_REQUEST" /> without the <see cref="EthPacket" />.
		/// </summary>
		public static int SizeOfHeader = Marshal.SizeOf<ETH_M_REQUEST>() - NDISRD_ETH_Packet.Size;

		/// <summary>
		/// The offset of <see cref="EthPacket" />.
		/// </summary>
		public static IntPtr EthPacketOffset = Marshal.OffsetOf(typeof(ETH_M_REQUEST), nameof(EthPacket));
	}
}