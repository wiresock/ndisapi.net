// ----------------------------------------------
// <copyright file="NdisApi.ETH_802_3_FLAGS.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
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
        /// ETH_802_3_FLAGS filter flags.
        /// </summary>
        [Flags]
        public enum ETH_802_3_FLAGS : uint
        {
            /// <summary>
            /// The eth 802 3 source address.
            /// </summary>
            ETH_802_3_SRC_ADDRESS = 0x00000001,

            /// <summary>
            /// The eth 802 3 destination address.
            /// </summary>
            ETH_802_3_DEST_ADDRESS = 0x00000002,

            /// <summary>
            /// The eth 802 3 protocol.
            /// </summary>
            ETH_802_3_PROTOCOL = 0x00000004
        }
    }
}