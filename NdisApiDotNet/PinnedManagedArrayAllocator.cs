// ----------------------------------------------
// <copyright file="PinnedManagedArrayAllocator.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NdisApiDotNet;

#if NETCOREAPP
[SkipLocalsInit]
#endif
internal sealed class PinnedManagedArrayAllocator<T> : IDisposable where T : struct
{
    private readonly object _lock = new();

    private bool _isDisposed;
    private Dictionary<IntPtr, T[]> _ptrToArrays;
    private Dictionary<IntPtr, GCHandle> _ptrToGcHandles;

    /// <summary>
    /// Initializes a new instance of the <see cref="PinnedManagedArrayAllocator{T}" /> class.
    /// </summary>
    public PinnedManagedArrayAllocator()
    {
        _ptrToArrays = new Dictionary<IntPtr, T[]>();
        _ptrToGcHandles = new Dictionary<IntPtr, GCHandle>();
    }

    /// <summary>
    /// Allocates the array.
    /// </summary>
    /// <param name="length">The length of the array.</param>
    /// <returns><see cref="IntPtr" />.</returns>
    public IntPtr AllocateArray(int length)
    {
#if NETCOREAPP
        T[] array = GC.AllocateUninitializedArray<T>(length);
#else
        var array = new T[length];
#endif

        var gcHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
        IntPtr ptr = gcHandle.AddrOfPinnedObject();

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
    public void FreeArray(IntPtr arrayPointer)
    {
        lock (_lock)
        {
#if NETCOREAPP
            if (_ptrToGcHandles.Remove(arrayPointer, out GCHandle handle))
                handle.Free();
#else
            if (_ptrToGcHandles.TryGetValue(arrayPointer, out GCHandle handle))
                handle.Free();

            _ptrToGcHandles.Remove(arrayPointer);
#endif

            _ptrToArrays.Remove(arrayPointer);
        }
    }

    /// <summary>
    /// Gets the array of the array pointer.
    /// </summary>
    /// <param name="arrayPointer">The array pointer.</param>
    /// <returns><see cref="T" />s.</returns>
    public T[] GetArray(IntPtr arrayPointer)
    {
        lock (_lock)
            return _ptrToArrays[arrayPointer];
    }

    /// <inheritdoc />
    public void Dispose()
    {
        lock (_lock)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (_ptrToGcHandles != null)
            {
                foreach (KeyValuePair<IntPtr, GCHandle> ptrToGcHandle in _ptrToGcHandles)
                    ptrToGcHandle.Value.Free();

                _ptrToGcHandles = null;
                _ptrToArrays = null;
            }
        }
    }
}