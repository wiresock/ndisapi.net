// ----------------------------------------------
// <copyright file="NdisApi.FAST_IO_SECTION.cs" company="NT Kernel">
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

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct FAST_IO_SECTION
        {
            /// <summary>
            /// The header.
            /// </summary>
            public FAST_IO_SECTION_HEADER fast_io_header;

            /// <summary>
            /// The intermediate buffers.
            /// </summary>
            internal INTERMEDIATE_BUFFER_VARIABLE fast_io_packets; // This is an array of INTERMEDIATE_BUFFER, but this cannot be declared directly as it's a variable width.

            /// <summary>
            /// Gets the intermediate buffers.
            /// </summary>
            public INTERMEDIATE_BUFFER* IntermediateBuffers => (INTERMEDIATE_BUFFER*) Unsafe.AsPointer(ref fast_io_packets);

            /// <summary>
            /// Gets the variable intermediate buffers.
            /// </summary>
            public INTERMEDIATE_BUFFER_VARIABLE* VariableIntermediateBuffers => (INTERMEDIATE_BUFFER_VARIABLE*) Unsafe.AsPointer(ref fast_io_packets);

            /// <summary>
            /// Gets the intermediate buffers.
            /// </summary>
            /// <param name="length">The number of packets to retrieve, defaults to the number of packets.</param>
            /// <returns><see cref="INTERMEDIATE_BUFFER" />s.</returns>
            public INTERMEDIATE_BUFFER*[] GetIntermediateBuffers(uint length = 0)
            {
                var size = length == 0 ? fast_io_header.fast_io_write_union.split.number_of_packets : length;

                var data = new INTERMEDIATE_BUFFER*[size];
                var intermediateBufferPtr = IntermediateBuffers;

                for (int i = 0; i < size; i++)
                {
                    data[i] = intermediateBufferPtr + (i * INTERMEDIATE_BUFFER.Size);
                }

                return data;
            }

            /// <summary>
            /// Gets the variable intermediate buffers.
            /// </summary>
            /// <param name="intermediateBufferSize">The size of the variable intermediate buffer.</param>
            /// <param name="length">The number of packets to retrieve, defaults to the number of packets.</param>
            /// <returns><see cref="INTERMEDIATE_BUFFER_VARIABLE" />s.</returns>
            public INTERMEDIATE_BUFFER_VARIABLE*[] GetVariableIntermediateBuffers(int intermediateBufferSize, uint length = 0)
            {
                var size = length == 0 ? fast_io_header.fast_io_write_union.split.number_of_packets : length;

                var data = new INTERMEDIATE_BUFFER_VARIABLE*[size];

                for (int i = 0; i < size; i++)
                {
                    data[i] = VariableIntermediateBuffers + (i * intermediateBufferSize);
                }

                return data;
            }

            /// <summary>
            /// The size of <see cref="FAST_IO_SECTION" /> without the <see cref="IntermediateBuffers" />.
            /// </summary>
            public static int SizeOfHeader = FAST_IO_SECTION_HEADER.Size;
        }
    }
}