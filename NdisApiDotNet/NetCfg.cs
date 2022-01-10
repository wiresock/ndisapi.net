// ----------------------------------------------
// <copyright file="NetCfg.cs" company="NT Kernel">
//    Copyright (c) 2000-2018 NT Kernel Resources / Contributors
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

namespace NdisApiDotNet
{
    public class NetCfg : IDisposable
    {
        private readonly bool _isCopied;
        private readonly string _path;
        private readonly string _startPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetCfg"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="ArgumentException">Should not contain the executable name. - path</exception>
        public NetCfg(string path)
        {
            if (path.Contains("netcfg.exe"))
                throw new ArgumentException("Should not contain the executable name.", nameof(path));


            _startPath = path;
            _path = Path.Combine(path, "netcfg.exe");

            _isCopied = CopyNetCfg(_path);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_isCopied)
                DeleteNetCfg(_path);
        }

        /// <summary>
        /// Determines whether the specified component identifier is installed.
        /// </summary>
        /// <param name="componentId">The component identifier.</param>
        /// <returns><c>true</c> if the specified component identifier is installed; otherwise, <c>false</c>.</returns>
        public bool IsInstalled(string componentId)
        {
            return ExecuteCommand($"-v -q {componentId}").IndexOf("not installed", StringComparison.OrdinalIgnoreCase) == -1;
        }

        /// <summary>
        /// Installs the specified inf file.
        /// </summary>
        /// <param name="infFileName">Name of the inf file.</param>
        /// <param name="componentId">The component identifier.</param>
        /// <param name="afterReboot">if set to <c>true</c>, requires a reboot for the changes to take affect.</param>
        /// <param name="errorCode">The error code.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException">Should not contain a path. - infFileName</exception>
        public bool Install(string infFileName, string componentId, out bool afterReboot, out uint errorCode)
        {
            if (infFileName.Contains("\\") || infFileName.Contains("//")) throw new ArgumentException("Should not contain a path.", nameof(infFileName));


            string result = ExecuteCommand($"-v -l {infFileName} -c s -i {componentId}");
            return ParseShowHrMessage(result, out afterReboot, out errorCode);
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
            string result = ExecuteCommand($"-v -u {componentId}");
            return ParseShowHrMessage(result, out afterReboot, out errorCode);
        }

        /// <summary>
        /// Parses the result of the <c>ShowHrMessage</c> function.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="afterReboot">if set to <c>true</c>, requires a reboot for the changes to take affect.</param>
        /// <param name="errorCode">The error code.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        private static bool ParseShowHrMessage(string result, out bool afterReboot, out uint errorCode)
        {
            if (result.IndexOf("failed", StringComparison.OrdinalIgnoreCase) > -1 || result.IndexOf("Error code", StringComparison.OrdinalIgnoreCase) > -1)
            {
                if (result.IndexOf("Error code", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    string hex = result.Substring(result.IndexOf("Error code:", StringComparison.OrdinalIgnoreCase) + 11).Trim().Replace("0x", string.Empty);
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
            afterReboot = result.IndexOf("reboot your computer", StringComparison.OrdinalIgnoreCase) > -1;

            return true;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns><see cref="string" />.</returns>
        private string ExecuteCommand(string command)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _path,
                    Arguments = command,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    WorkingDirectory = _startPath
                }
            };
            process.Start();

            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }

        /// <summary>
        /// Copies the net cfg.
        /// </summary>
        /// <param name="path">The path.</param>
        private static bool CopyNetCfg(string path)
        {
            try
            {
                string fileName = Environment.Is64BitOperatingSystem ? "snetcfg.amd64.exe" : "snetcfg.i386.exe";
                using (Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("NdisApiDotNet.Resources." + fileName))
                {
                    if (resource != null)
                    {
                        using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            resource.CopyTo(file);

                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes the net cfg.
        /// </summary>
        /// <param name="path">The path.</param>
        private static void DeleteNetCfg(string path)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}