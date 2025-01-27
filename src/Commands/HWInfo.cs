using System.Management;

namespace LinConsol.Commands
{
    static class HWInfo
    {
        public static void Execute()
        {
            Console.WriteLine("<==========>HARDWARE INFO<==========>");
            Console.WriteLine($"  CPU            : {GetCpuInfo()}");
            Console.WriteLine($"  GPU            : {GetGpuInfo()}");
            Console.WriteLine($"  Total RAM      : {GetTotalRAM()}");
            Console.WriteLine($"  Free RAM       : {GetFreeRAM()}");
            Console.WriteLine($"  Motherboard    : {GetMotherboardInfo()}");
            Console.WriteLine($"  BIOS Version   : {GetBiosVersion()}");
            Console.WriteLine($"  Disk Drives    : {GetDiskDrivesInfo()}");
            Console.WriteLine("<===================================>");
        }

        private static string GetCpuInfo()
        {
            using (var searcher = new ManagementObjectSearcher("select Name from Win32_Processor"))
            {
                foreach (var item in searcher.Get())
                {
                    return item["Name"].ToString();
                }
            }
            return "Unknown";
        }

        private static string GetGpuInfo()
        {
            using (var searcher = new ManagementObjectSearcher("select Name from Win32_VideoController"))
            {
                foreach (var item in searcher.Get())
                {
                    return item["Name"].ToString();
                }
            }
            return "Unknown";
        }

        private static string GetTotalRAM()
        {
            using (var searcher = new ManagementObjectSearcher("select TotalVisibleMemorySize from Win32_OperatingSystem"))
            {
                foreach (var item in searcher.Get())
                {
                    return $"{Convert.ToInt64(item["TotalVisibleMemorySize"]) / 1024} GB";
                }
            }
            return "Unknown";
        }

        private static string GetFreeRAM()
        {
            using (var searcher = new ManagementObjectSearcher("select FreePhysicalMemory from Win32_OperatingSystem"))
            {
                foreach (var item in searcher.Get())
                {
                    return $"{Convert.ToInt64(item["FreePhysicalMemory"]) / 1024} GB";
                }
            }
            return "Unknown";
        }

        private static string GetMotherboardInfo()
        {
            using (var searcher = new ManagementObjectSearcher("select Product, Manufacturer from Win32_BaseBoard"))
            {
                foreach (var item in searcher.Get())
                {
                    return $"{item["Manufacturer"]} {item["Product"]}";
                }
            }
            return "Unknown";
        }
        
        private static string GetBiosVersion()
        {
            using (var searcher = new ManagementObjectSearcher("select SMBIOSBIOSVersion from Win32_BIOS"))
            {
                foreach (var item in searcher.Get())
                {
                    return item["SMBIOSBIOSVersion"].ToString();
                }
            }
            return "Unknown";
        }

        private static string GetDiskDrivesInfo()
        {
            string result = "";
            using (var searcher = new ManagementObjectSearcher("select Model, Size from Win32_DiskDrive"))
            {
                foreach (var item in searcher.Get())
                {
                    var sizeInGB = Convert.ToInt64(item["Size"]) / (1024 * 1024 * 1024);
                    result += $"\n    {item["Model"]} | {sizeInGB} GB";
                }
            }
            return string.IsNullOrEmpty(result) ? "No Disk Drives Found" : result.TrimEnd();
        }
    }
}