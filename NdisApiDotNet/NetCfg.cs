// ----------------------------------------------
// <copyright file="NetCfg.cs" company="NT Kernel">
//    Copyright (c) NT Kernel Resources / Contributors
//                      All Rights Reserved.
//                    http://www.ntkernel.com
//                      ndisrd@ntkernel.com
// </copyright>
// ----------------------------------------------

using System;
using System.Diagnostics;
using System.IO;

namespace NdisApiDotNet;

internal static class NetCfg
{
	/// <summary>
	/// Installs the specified INF file.
	/// </summary>
	/// <param name="infFilePath">The path to the INF file.</param>
	/// <param name="componentId">The component identifier.</param>
	/// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
	public static bool Install(string infFilePath, string componentId)
	{
		return ExecuteCommand($"-v -l \"{infFilePath}\" -c s -i {componentId}");
	}

	/// <summary>
	/// Uninstalls the specified component identifier.
	/// </summary>
	/// <param name="componentId">The component identifier.</param>
	/// <returns><c>true</c> if uninstalled, <c>false</c> otherwise.</returns>
	public static bool Uninstall(string componentId)
	{
		return ExecuteCommand($"-v -u {componentId}");
	}

	/// <summary>
	/// Executes the specified <paramref name="command" />.
	/// </summary>
	/// <param name="command">The command.</param>
	/// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
	private static bool ExecuteCommand(string command)
	{
		try
		{
			string workingDirectory = Environment.SystemDirectory;
			string path = Path.Combine(workingDirectory, "netcfg.exe");

			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = path,
					Arguments = command,
					UseShellExecute = false,
					CreateNoWindow = true,
					WorkingDirectory = workingDirectory
				}
			};

			process.Start();
			process.WaitForExit();

			return process.ExitCode == 0;
		}
		catch
		{
			return false;
		}
	}
}