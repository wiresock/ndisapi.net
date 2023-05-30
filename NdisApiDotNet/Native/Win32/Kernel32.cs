// ----------------------------------------------
// <copyright file="Kernel32.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Security;

// ReSharper disable CheckNamespace

namespace NdisApiDotNet.Native;

[SuppressUnmanagedCodeSecurity]
internal static class Kernel32
{
	private const string DllName = "kernel32.dll";

	[DllImport(DllName, ExactSpelling = true)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
	public static extern void RtlZeroMemory(IntPtr destination, int length);

	[DllImport(DllName, ExactSpelling = true)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
	public static extern void RtlMoveMemory(IntPtr dest, IntPtr src, uint count);

	[DllImport(DllName, ExactSpelling = true)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
	public static extern bool IsWow64Process2(IntPtr hProcess, out ushort pProcessMachine, out ushort pNativeMachine);
}