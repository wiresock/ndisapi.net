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
using System.Globalization;
using System.IO;
using System.Reflection;

namespace NdisApiDotNet;

internal class NetCfg : IDisposable
{
    private readonly string _path;
    private readonly string _workingDirectory;
    private bool _copied;

    /// <summary>
    /// Initializes a new instance of the <see cref="NetCfg" /> class.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <exception cref="ArgumentException">Should not contain the executable name. - path</exception>
    public NetCfg(string path)
    {
        if (path.Contains("netcfg.exe"))
            throw new ArgumentException("Should not contain the executable name.", nameof(path));

        _workingDirectory = path;
        _path = Path.Combine(path, "netcfg.exe");
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_copied)
            DeleteNetCfg();
    }

    /// <summary>
    /// Determines whether the specified component identifier is installed.
    /// </summary>
    /// <param name="componentId">The component identifier.</param>
    /// <returns><c>true</c> if the specified component identifier is installed; otherwise, <c>false</c>.</returns>
    public bool IsInstalled(string componentId)
    {
        return ExecuteCommand($"-v -q {componentId}", out var output) && output.IndexOf("not installed", StringComparison.OrdinalIgnoreCase) == -1;
    }

    /// <summary>
    /// Installs the specified inf file.
    /// </summary>
    /// <param name="infFileName">Name of the inf file.</param>
    /// <param name="componentId">The component identifier.</param>
    /// <param name="afterReboot">if set to <c>true</c>, requires a reboot for the changes to take affect.</param>
    /// <param name="errorCode">The error code.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public bool Install(string infFileName, string componentId, out bool afterReboot, out uint errorCode)
    {
        if (infFileName.Contains("\\") || infFileName.Contains("//"))
            throw new ArgumentException("Should not contain a path.", nameof(infFileName));

        if (ExecuteCommand($"-v -l {infFileName} -c s -i {componentId}", out var output))
            return ParseCommandOutput(output, out afterReboot, out errorCode);

        afterReboot = false;
        errorCode = uint.MaxValue;
        return false;
    }

    /// <summary>
    /// Uninstalls the specified component identifier.
    /// </summary>
    /// <param name="componentId">The component identifier.</param>
    /// <param name="afterReboot">if set to <c>true</c>, requires a reboot for the changes to take affect.</param>
    /// <param name="errorCode">The error code.</param>
    /// <returns><c>true</c> if uninstalled, <c>false</c> otherwise.</returns>
    public bool Uninstall(string componentId, out bool afterReboot, out uint errorCode)
    {
        if (ExecuteCommand($"-v -u {componentId}", out var output))
            return ParseCommandOutput(output, out afterReboot, out errorCode);

        afterReboot = false;
        errorCode = uint.MaxValue;
        return false;
    }

    /// <summary>
    /// Parses the output of <see cref="ExecuteCommand" />.
    /// </summary>
    /// <param name="output">The output.</param>
    /// <param name="afterReboot">If set to <c>true</c>, requires a reboot for the changes to take affect.</param>
    /// <param name="errorCode">The error code.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    private static bool ParseCommandOutput(string output, out bool afterReboot, out uint errorCode)
    {
        // For example:
        //
        // Trying to install nt_ndisrd ...
        // ... failed. Error code: 0x80070002.

        var errorCodeIndex = output.IndexOf("error code:", StringComparison.OrdinalIgnoreCase);
        if (errorCodeIndex > -1 || output.IndexOf("failed", StringComparison.OrdinalIgnoreCase) > -1)
        {
            if (errorCodeIndex > -1)
            {
                var hex = output.Substring(errorCodeIndex + 11).Trim().Replace("0x", string.Empty);
                errorCode = uint.Parse(hex, NumberStyles.AllowHexSpecifier);
            }
            else
            {
                errorCode = 0;
            }

            afterReboot = false;
            return false;
        }

        errorCode = 0;
        afterReboot = output.IndexOf("reboot your computer", StringComparison.OrdinalIgnoreCase) > -1;

        return true;
    }

    /// <summary>
    /// Executes the specified <paramref name="command" />.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="output">The output of the command.</param>
    /// <returns><see cref="string" />.</returns>
    private bool ExecuteCommand(string command, out string output)
    {
        try
        {
            if (!_copied && !(_copied = CopyNetCfg()))
            {
                output = null;
                return false;
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _path,
                    Arguments = command,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    WorkingDirectory = _workingDirectory
                }
            };

            process.Start();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return true;
        }
        catch
        {
            output = null;
            return false;
        }
    }

    /// <summary>
    /// Copies the net cfg executable.
    /// </summary>
    private bool CopyNetCfg()
    {
        try
        {
            var fileName = Environment.Is64BitOperatingSystem ? "snetcfg.amd64.exe" : "snetcfg.i386.exe";

            using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("NdisApiDotNet.Resources." + fileName);

            if (resource != null)
            {
                using var file = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write);

                resource.CopyTo(file);

                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Deletes the net cfg executable.
    /// </summary>
    private void DeleteNetCfg()
    {
        try
        {
            if (File.Exists(_path))
                File.Delete(_path);
        }
        catch
        {
            // ignored.
        }
    }
}