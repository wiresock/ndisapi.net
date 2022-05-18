// ----------------------------------------------
// <copyright file="NdisApi.NDISRD_ETH_Packet.cs" company="NT Kernel">
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
    /// A container for a <see cref="INTERMEDIATE_BUFFER" /> pointer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NDISRD_ETH_Packet
    {
        /// <summary>
        /// The pointer to the <see cref="INTERMEDIATE_BUFFER" /> or <see cref="INTERMEDIATE_BUFFER_VARIABLE" />.
        /// </summary>
        public IntPtr Buffer;

        /// <summary>
        /// Gets the intermediate buffer.
        /// </summary>
        /// <returns><see cref="INTERMEDIATE_BUFFER" />.</returns>
        public unsafe INTERMEDIATE_BUFFER* GetIntermediateBuffer()
        {
            return (INTERMEDIATE_BUFFER*) Buffer;
        }

        /// <summary>
        /// Gets the variable intermediate buffer.
        /// </summary>
        /// <returns><see cref="INTERMEDIATE_BUFFER_VARIABLE" />.</returns>
        public unsafe INTERMEDIATE_BUFFER_VARIABLE* GetVariableIntermediateBuffer()
        {
            return (INTERMEDIATE_BUFFER_VARIABLE*) Buffer;
        }

        /// <summary>
        /// The size of <see cref="NDISRD_ETH_Packet" />.
        /// </summary>
        public static int Size = Marshal.SizeOf<NDISRD_ETH_Packet>();
    }
}