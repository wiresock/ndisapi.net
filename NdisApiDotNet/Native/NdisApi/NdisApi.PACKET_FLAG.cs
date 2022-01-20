// ----------------------------------------------
// <copyright file="NdisApi.PACKET_FLAG.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    public static partial class NdisApi
    {
        /// <summary>
        /// The packet flag.
        /// </summary>
        [Flags]
        public enum PACKET_FLAG : uint
        {
            /// <summary>
            /// The packet was intercepted from MSTCP.
            /// </summary>
            PACKET_FLAG_ON_SEND = 0x00000001,

            /// <summary>
            /// The packet was intercepted from the network interface.
            /// </summary>
            PACKET_FLAG_ON_RECEIVE = 0x00000002
        }
    }
}