using System.Runtime.InteropServices;

namespace LinConsol.Commands
{
    static class OSInfo
    {
        public static void Execute()
        {
            string FindSystemType() => Environment.Is64BitOperatingSystem ? "x64-based" : "x32-based";

            Console.WriteLine("<==========>OS INFO<==========>");
            Console.WriteLine($"  OS            : {RuntimeInformation.OSDescription}");
            Console.WriteLine($"  System Type   : {FindSystemType()}");
            Console.WriteLine($"  PC-name       : {Environment.MachineName}");
            Console.WriteLine($"  Username      : {Environment.UserName}");
            Console.WriteLine($"  CPU           : {Environment.ProcessorCount}");
            Console.WriteLine($"  Disk Space    : {GetDiskSpace()}");
            Console.WriteLine("<=============================>");
        }

        private static string GetDiskSpace()
        {
            try
            {
                var driveInfos = DriveInfo.GetDrives();
                string diskSpaceInfo = "";

                foreach (var drive in driveInfos)
                {
                    if (drive.IsReady)
                    {
                        long totalSize = drive.TotalSize / (1024 * 1024 * 1024);
                        long freeSpace = drive.TotalFreeSpace / (1024 * 1024 * 1024);
                        diskSpaceInfo += $"\n    {drive.Name} | {freeSpace} GB free of {totalSize} GB";
                    }
                }

                return string.IsNullOrEmpty(diskSpaceInfo) ? "Unavailable" : diskSpaceInfo.TrimEnd();
            }
            catch
            {
                return "Unavailable";
            }
        }
    }
}