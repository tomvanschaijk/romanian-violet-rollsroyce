using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace RomanianVioletRollsRoyce.Crosscutting.HealthChecks
{
    public class MemoryUsageLoader : IMemoryUsageLoader
    {
        public MemoryUsage GetMemoryUsage() => IsUnix() ? GetUnixMetrics() : GetWindowsMetrics();

        private bool IsUnix() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        private MemoryUsage GetWindowsMetrics()
        {
            var output = string.Empty;
            var info = new ProcessStartInfo
            {
                FileName = "wmic",
                Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value",
                RedirectStandardOutput = true
            };

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            var lines = output.Trim().Split(new[] { "\n" }, StringSplitOptions.None);
            var freeMemoryParts = lines[0].Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

            var total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            var free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            var used = total - free;
            var metrics = new MemoryUsage(total, used, free);

            return metrics;
        }

        private MemoryUsage GetUnixMetrics()
        {
            var output = string.Empty;
            var info = new ProcessStartInfo("free -m")
            {
                FileName = "/bin/bash",
                Arguments = "-c \"free -m\"",
                RedirectStandardOutput = true
            };

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            var lines = output.Split(new[] { "\n" }, StringSplitOptions.None);
            var memory = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var metrics = new MemoryUsage(double.Parse(memory[1]), double.Parse(memory[2]), double.Parse(memory[3]));

            return metrics;
        }
    }
}
