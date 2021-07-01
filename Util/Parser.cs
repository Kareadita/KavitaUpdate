using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using KavitaUpdate.Update;
using OperatingSystem = KavitaUpdate.Update.OperatingSystem;

namespace KavitaUpdate.Util
{
    public static class Parser
    {
        public static readonly Regex NetCoreAsset = new Regex(@"(linux|osx|windows)-(x64|arm|arm64|x86)", RegexOptions.Compiled);

        public static readonly Regex WindowsAsset = new Regex(@"win(dows)?(-(x64|arm|arm64|x86))?\.(zip|tar\.gz)$", RegexOptions.Compiled);

        public static readonly Regex LinuxAsset = new Regex(@"linux(-(x64|arm|arm64|x86))?\.tar.gz$", RegexOptions.Compiled);

        public static readonly Regex OsxAsset = new Regex(@"osx(-(x64|arm|arm64|x86))?\.tar.gz$", RegexOptions.Compiled);

        public static readonly Regex ArchRegex = new Regex(@"core-(?<arch>x64|arm|arm64|x86)\.", RegexOptions.Compiled);

        public static OperatingSystem? ParseOs(string file)
        {
            if (WindowsAsset.IsMatch(file))
            {
                return OperatingSystem.Windows;
            }
            if (LinuxAsset.IsMatch(file))
            {
                return OperatingSystem.Linux;
            }
            if (OsxAsset.IsMatch(file))
            {
                return OperatingSystem.Osx;
            }

            return null;
        }

        public static Runtime ParseRuntime(string file)
        {
            return NetCoreAsset.IsMatch(file) ? Runtime.NetCore : Runtime.DotNet;
        }

        public static Architecture ParseArchitecture(string file)
        {
            var match = ArchRegex.Match(file);

            if (!match.Success)
            {
                return Architecture.X64;
            }

            return match.Groups["arch"].Value switch
            {
                "arm64" => Architecture.Arm64,
                "arm" => Architecture.Arm,
                "x64" => Architecture.X64,
                "x86" => Architecture.X86,
                _     => throw new ArgumentException(message: "Invalid architecture")
            };
        }
    }
}
