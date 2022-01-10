// ----------------------------------------------
// <copyright file="NdisApi.STATIC_FILTER_TABLE.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    /// <summary>
    /// The static filters table to be passed to WinPkFilter driver.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StaticFilterTable : IList<StaticFilter> // STATIC_FILTER_TABLE
    {
        public const int MaxSize = 256;

        internal uint m_TableSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxSize)]
        internal StaticFilter[] m_StaticFilters;

        /// <summary>
        /// Gets or sets the number of STATIC_FILTER entries.
        /// </summary>
        public uint TableSize
        {
            get
            {
                return m_TableSize;
            }

            set
            {
                m_TableSize = Math.Max(0, value);
            }
        }

        /// <summary>
        /// Gets or sets the static filters.
        /// </summary>
        /// <remarks>
        /// For convenience the size of the array is fixed to 256 entries, feel free to change this value if you need more filter
        /// entries.
        /// </remarks>
        public StaticFilter[] StaticFilters
        {
            get
            {
                return m_StaticFilters;
            }

            set
            {
                m_StaticFilters = value;
            }
        }

        public int Count { get { return (int)TableSize; } }
        public bool IsReadOnly { get { return false; } }
        public StaticFilter this[int index] { get { return StaticFilters[index]; } set { StaticFilters[index] = value; } }

        public StaticFilterTable(uint size = 0)
        {
            m_TableSize = size;
            m_StaticFilters = new StaticFilter[MaxSize];
        }

        public void Add(StaticFilter filter)
        {
            StaticFilters[TableSize] = filter;

            TableSize++;
        }

        public StaticFilter Add(IntPtr adapter, PacketFlag direction, FilterAction action, params object[] filters)
        {
            StaticFilter filter = new StaticFilter(adapter, direction, action, filters);

            Add(filter);

            return filter;
        }

        public StaticFilter Add(PacketFlag direction, FilterAction action, params object[] filters)
        {
            return Add(IntPtr.Zero, direction, action, filters);
        }

        public StaticFilter Add(FilterAction action, params object[] filters)
        {
            return Add(PacketFlag.Both, action, filters);
        }

        public int IndexOf(StaticFilter item)
        {
            return Array.IndexOf(StaticFilters, item);
        }

        public void Insert(int index, StaticFilter item)
        {
            IEnumerable<StaticFilter> skip = StaticFilters.Skip(index);

            StaticFilters[index] = item;

            foreach (StaticFilter filter in skip)
            {
                if (++index >= StaticFilters.Length) break;

                StaticFilters[index] = filter;
            }
        }

        public void RemoveAt(int index)
        {
            if (index > Count || index < 0) return;

            IEnumerable<StaticFilter> skip = StaticFilters.Skip(index + 1);

            foreach (StaticFilter filter in skip)
            {
                StaticFilters[index] = filter;

                index++;
            }

            while (index < StaticFilters.Length)
            {
                StaticFilters[index] = default;

                index++;
            }

            TableSize--;
        }

        public void Clear()
        {
            StaticFilters = new StaticFilter[MaxSize];
        }

        public bool Contains(StaticFilter item)
        {
            return StaticFilters.Contains(item);
        }

        public void CopyTo(StaticFilter[] array, int arrayIndex)
        {
            StaticFilters.CopyTo(array, arrayIndex);
        }

        public bool Remove(StaticFilter item)
        {
            int index = IndexOf(item);

            if (index < 0) return false;

            RemoveAt(index);

            return true;
        }

        public IEnumerator<StaticFilter> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return StaticFilters.GetEnumerator();
        }
    }
}