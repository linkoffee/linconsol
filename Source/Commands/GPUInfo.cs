using System.Globalization;
using System.Management;
using LibreHardwareMonitor.Hardware;

namespace LinConsol.Commands
{
    static class GPUInfo
    {
        public static void Execute()
        {
            Console.WriteLine("<=================> GPU INFO <=================>");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get())
            {
                string name = obj["Name"]?.ToString() ?? "Unknown";
                string manufacturer = obj["AdapterCompatibility"]?.ToString() ?? "Unknown";
                string vram = obj["AdapterRAM"] != null ? $"{(Convert.ToInt64(obj["AdapterRAM"]) / 1073741824)} GB" : "Unknown";
                string driver = obj["DriverVersion"]?.ToString() ?? "Unknown";
                string pcie = obj["PNPDeviceID"]?.ToString()?.Contains("PCI") == true ? "PCIe" : "Unknown";
                string memoryBus = obj["AdapterDACType"]?.ToString() ?? "Unknown";

                Console.WriteLine($"  Name                : {name}");
                Console.WriteLine($"  Manufacturer        : {manufacturer}");
                Console.WriteLine($"  VRAM                : {vram}");
                Console.WriteLine($"  Memory Bus          : {memoryBus}");
                Console.WriteLine($"  Driver              : {driver}");
                Console.WriteLine($"  PCIe Version        : {pcie}");
                Console.WriteLine($"  GPU Load            : {GetGpuLoad()}");
                Console.WriteLine($"  GPU Temperature     : {GetGpuTemperature()}");
            }
            Console.WriteLine("<============================================>");
        }

        private static string GetGpuLoad()
        {
            try
            {
                var computer = new Computer { IsGpuEnabled = true };
                computer.Open();

                foreach (var hardware in computer.Hardware)
                {
                    if (hardware.HardwareType == HardwareType.GpuNvidia || 
                        hardware.HardwareType == HardwareType.GpuAmd ||
                        hardware.HardwareType == HardwareType.GpuIntel)
                    {
                        hardware.Update();
                        
                        var loadSensor = hardware.Sensors
                            .FirstOrDefault(s => s.SensorType == SensorType.Load && 
                                            (s.Name.Contains("GPU Core") || 
                                            s.Name.Contains("D3D 3D") || 
                                            s.Name.Contains("GPU")));
                        
                        loadSensor ??= hardware.Sensors
                            .FirstOrDefault(s => s.SensorType == SensorType.Load);
                        
                        if (loadSensor?.Value != null)
                        {
                            computer.Close();
                            return string.Format(CultureInfo.InvariantCulture, "{0:0.0}%", loadSensor.Value);
                        }
                    }
                }
                
                computer.Close();
                return "Unknown";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private static string GetGpuTemperature()
        {
            var computer = new Computer
            {
                IsGpuEnabled = true
            };
            computer.Open();

            string temperature = "Unknown";

            foreach (var hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAmd)
                {
                    hardware.Update();
                    var sensor = hardware.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Temperature);
                    if (sensor != null && sensor.Value.HasValue)
                    {
                        temperature = $"{sensor.Value.Value}°C";
                        break;
                    }
                }
            }

            computer.Close();
            return temperature;
        }
    }
}
