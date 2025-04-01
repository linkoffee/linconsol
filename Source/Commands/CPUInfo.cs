using System.Management;
using LibreHardwareMonitor.Hardware;
using System.Globalization;

namespace LinConsol.Commands
{
    static class CPUInfo
    {
        public static void Execute()
        {
            Console.WriteLine("<=================> CPU INFO <=================>");
            
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                string name = obj["Name"]?.ToString()?.Trim() ?? "Unknown";
                string architecture = GetArchitecture(Convert.ToInt32(obj["Architecture"]));
                int cores = Convert.ToInt32(obj["NumberOfCores"]);
                int threads = Convert.ToInt32(obj["NumberOfLogicalProcessors"]);
                string clockSpeed = $"{Convert.ToDouble(obj["CurrentClockSpeed"]) / 1000:0.0} GHz";
                string maxClockSpeed = obj["MaxClockSpeed"] != null ? 
                    $"{Convert.ToDouble(obj["MaxClockSpeed"]) / 1000:0.0} GHz" : "Unknown";

                Console.WriteLine($"  Name             : {name}");
                Console.WriteLine($"  Architecture     : {architecture}");
                Console.WriteLine($"  Cores/Threads    : {cores}/{threads}");
                Console.WriteLine($"  Clock Speed      : {clockSpeed} (Max: {maxClockSpeed})");
            }

            var computer = new Computer { IsCpuEnabled = true };
            computer.Open();

            foreach (var hardware in computer.Hardware.Where(h => h.HardwareType == HardwareType.Cpu))
            {
                hardware.Update();

                Console.WriteLine($"  CPU Load         : {GetCpuLoad(hardware)}");
                Console.WriteLine($"  Temperature      : {GetCpuTemperature(hardware)}");
                
                if (hardware.Name.Contains("AMD") || hardware.Name.Contains("Intel"))
                {
                    Console.WriteLine($"  Technology       : {GetCpuTechnology(hardware.Name)}");
                }
            }

            computer.Close();
            Console.WriteLine("<============================================>");
        }

        private static string GetArchitecture(int architectureCode)
        {
            return architectureCode switch
            {
                0 => "x86",
                9 => "x64",
                _ => "Unknown"
            };
        }

        private static string GetCpuLoad(IHardware hardware)
        {
            try
            {
                var sensor = hardware.Sensors
                    .FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "CPU Total");
                
                return sensor?.Value != null ? $"{sensor.Value:0.0}%" : "Unknown";
            }
            catch
            {
                return "Error";
            }
        }

        private static string GetCpuTemperature(IHardware hardware)
        {
            try
            {
                var sensor = hardware.Sensors
                    .FirstOrDefault(s => s.SensorType == SensorType.Temperature && 
                                       (s.Name.Contains("Core") || s.Name.Contains("CPU")));
                
                return sensor?.Value != null ? $"{sensor.Value:0}°C" : "Unknown";
            }
            catch
            {
                return "Error";
            }
        }

        private static string GetCpuTechnology(string name)
        {
            if (name.Contains("AMD"))
            {
                if (name.Contains("Ryzen 2")) return "12nm (Zen+)";
                if (name.Contains("Ryzen 3")) return "7nm (Zen 2)";
                if (name.Contains("Ryzen 5")) return "7nm (Zen 3)";
                if (name.Contains("Ryzen 7")) return "5nm (Zen 4)";
                return "AMD Processor";
            }
            else if (name.Contains("Intel"))
            {
                if (name.Contains("12")) return "Intel 7 (10nm)";
                if (name.Contains("13")) return "Intel 7 (10nm)";
                if (name.Contains("14")) return "Intel 4 (7nm)";
                return "Intel Processor";
            }
            return "Unknown";
        }
    }
}