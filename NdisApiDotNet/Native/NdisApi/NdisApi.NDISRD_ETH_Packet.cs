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
    public static partial class NdisApi
    {
        /// <summary>
        /// A container for INTERMEDIATE_BUFFER pointer.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NDISRD_ETH_Packet
        {
            internal IntPtr _buffer;

            /// <summary>
            /// Gets or sets the pointer to the INTERMEDIATE_BUFFER.
            /// </summary>
            public IntPtr Buffer
            {
                get => _buffer;
                set => _buffer = value;
            }

            /// <summary>
            /// Gets the intermediate buffer.
            /// </summary>
            /// <returns><see cref="INTERMEDIATE_BUFFER" />.</returns>
            public INTERMEDIATE_BUFFER GetIntermediateBuffer()
            {
                return Marshal.PtrToStructure<INTERMEDIATE_BUFFER>(_buffer);
            }

            /// <summary>
            /// Sets the intermediate buffer.
            /// </summary>
            /// <param name="intermediateBuffer">The intermediate buffer.</param>
            public void SetIntermediateBuffer(INTERMEDIATE_BUFFER intermediateBuffer)
            {
                Marshal.StructureToPtr(intermediateBuffer, _buffer, false);
            }

            /// <summary>
            /// Gets the unsafe intermediate buffer.
            /// </summary>
            /// <returns><see cref="INTERMEDIATE_BUFFER_U" />.</returns>
            public unsafe INTERMEDIATE_BUFFER_U* GetUnsafeIntermediateBuffer()
            {
                return (INTERMEDIATE_BUFFER_U*)_buffer;
            }
        }
    }
}