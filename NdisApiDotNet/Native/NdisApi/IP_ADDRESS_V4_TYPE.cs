// ----------------------------------------------
// <copyright file="NdisApi.IP_ADDRESS_V4_TYPE.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native
{
    /// <summary>
    /// The type of IP address.
    /// </summary>
    public enum IPAddressType : uint // IP_ADDRESS_V4_TYPE, IP_ADDRESS_V6_TYPE
    {
        /// <summary>
        /// The ip subnet type.
        /// </summary>
        Subnet = 0x00000001, // IP_SUBNET_V4_TYPE, IP_SUBNET_V6_TYPE

        /// <summary>
        /// The ip range type.
        /// </summary>
        Range = 0x00000002 // IP_RANGE_V4_TYPE, IP_RANGE_V6_TYPE
    }
}