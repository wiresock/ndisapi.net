// ----------------------------------------------
// <copyright file="NdisApi.IP_ADDRESS_V6_TYPE.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

public static partial class NdisApi
{
    /// <summary>
    /// The type of IPv6 address.
    /// </summary>
    public enum IP_ADDRESS_V6_TYPE : uint
    {
        /// <summary>
        /// The IPv6 subnet type.
        /// </summary>
        IP_SUBNET_V6_TYPE = 0x00000001,

        /// <summary>
        /// The IPv6 range type.
        /// </summary>
        IP_RANGE_V6_TYPE = 0x00000002
    }
}