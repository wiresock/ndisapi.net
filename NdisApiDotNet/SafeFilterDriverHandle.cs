// ----------------------------------------------
// <copyright file="SafeFilterDriverHandle.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------


using System.Security;
using Microsoft.Win32.SafeHandles;

namespace NdisApiDotNet
{
    [SecurityCritical]
    public class SafeFilterDriverHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeFilterDriverHandle"/> class.
        /// </summary>
        [SecurityCritical]
        public SafeFilterDriverHandle() : base(true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeFilterDriverHandle"/> class.
        /// </summary>
        /// <param name="ownsHandle">true to reliably release the handle during the finalization phase; false to prevent reliable release (not recommended).</param>
        [SecurityCritical]
        public SafeFilterDriverHandle(bool ownsHandle) : base(ownsHandle)
        { }

        /// <inheritdoc />
        [SecurityCritical]
        protected override bool ReleaseHandle()
        {
            Native.NdisApi.CloseFilterDriver(handle);

            return true;
        }
    }
}