// ----------------------------------------------
// <copyright file="PinnedManagedArrayAllocator.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using NdisApiDotNet.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace NdisApiDotNet
{
    internal sealed class PinnedManagedArrayAllocator<T> : IDisposable where T : struct
    {
        private readonly object _lock = new object();
        private bool _isDisposed;
        private Dictionary<IntPtr, T[]> _ptrToArrays;
        private Dictionary<IntPtr, GCHandle> _ptrToGcHandles;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedManagedArrayAllocator{T}"/> class.
        /// </summary>
        internal PinnedManagedArrayAllocator()
        {
            _ptrToArrays = new Dictionary<IntPtr, T[]>();
            _ptrToGcHandles = new Dictionary<IntPtr, GCHandle>();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            lock (_lock)
            {
                if (_isDisposed) return;

                if (_ptrToGcHandles != null)
                {
                    foreach (GCHandle gcHandle in _ptrToGcHandles.Values)
                        gcHandle.Free();

                    _ptrToGcHandles = null;
                }

                _ptrToArrays = null;
                _isDisposed = true;
            }
        }

        /// <summary>
        /// Allocates the array.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns><see cref="IntPtr" />.</returns>
        internal IntPtr AllocateArray(int count)
        {
            T[] array = new T[count];
            GCHandle gcHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
            IntPtr ptr = gcHandle.AddrOfPinnedObject();

            Kernel32.ZeroMemory(ptr, count);

            lock (_lock)
            {
                _ptrToArrays.Add(ptr, array);
                _ptrToGcHandles.Add(ptr, gcHandle);
            }

            return ptr;
        }

        /// <summary>
        /// Frees the array.
        /// </summary>
        /// <param name="arrayPointer">The array pointer.</param>
        internal void FreeArray(IntPtr arrayPointer)
        {
            lock (_lock)
            {
                KeyValuePair<IntPtr, GCHandle> array = _ptrToGcHandles.FirstOrDefault(x => x.Key == arrayPointer);
                if (array.Equals(default)) return;

                array.Value.Free();
                _ptrToGcHandles.Remove(arrayPointer);
                _ptrToArrays.Remove(arrayPointer);
            }
        }

        /// <summary>
        /// Gets the array of the array pointer.
        /// </summary>
        /// <param name="arrayPointer">The array pointer.</param>
        /// <returns><see cref="T" />s.</returns>
        internal T[] GetArray(IntPtr arrayPointer)
        {
            lock (_lock)
            {
                return _ptrToArrays[arrayPointer];
            }
        }
    }
}