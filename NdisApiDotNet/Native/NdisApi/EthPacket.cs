// ----------------------------------------------
// <copyright file="NdisApi.NDISRD_ETH_Packet.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    /// <summary>
    /// A container for <see cref="IntermediateBuffer"/> pointer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EthPacket // NDISRD_ETH_Packet
    {
        internal IntPtr _buffer;

        /// <summary>
        /// Gets or sets the pointer to the <see cref="IntermediateBuffer"/>.
        /// </summary>
        public IntPtr Buffer
        {
            get
            {
                return _buffer;
            }

            set
            {
                _buffer = value;
            }
        }

        /// <summary>
        /// Gets the intermediate buffer.
        /// </summary>
        /// <returns><see cref="IntermediateBuffer"/>.</returns>
        public IntermediateBuffer GetIntermediateBuffer()
        {
            return Marshal.PtrToStructure<IntermediateBuffer>(_buffer);
        }

        /// <summary>
        /// Sets the intermediate buffer.
        /// </summary>
        /// <param name="intermediateBuffer">The intermediate buffer.</param>
        public void SetIntermediateBuffer(IntermediateBuffer intermediateBuffer)
        {
            Marshal.StructureToPtr(intermediateBuffer, _buffer, false);
        }

        /// <summary>
        /// Gets the unsafe intermediate buffer.
        /// </summary>
        /// <returns><see cref="IntermediateBufferUnsafe"/>.</returns>
        public unsafe IntermediateBufferUnsafe* GetUnsafeIntermediateBuffer()
        {
            return (IntermediateBufferUnsafe*)_buffer;
        }
    }
}