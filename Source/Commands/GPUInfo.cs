using System.Management;
using LibreHardwareMonitor.Hardware;
using SharpDX.DXGI;

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
                using (var factory = new Factory1())
                {
                    int adapterCount = factory.GetAdapterCount1();
                    if (adapterCount == 0)
                        return "No GPU adapters found";

                    for (int i = 0; i < adapterCount; i++)
                    {
                        using (var adapter = factory.GetAdapter1(i))
                        {
                            var desc = adapter.Description1;
                            long dedicatedMemory = desc.DedicatedVideoMemory;

                            if (dedicatedMemory > 0)
                            {
                                long memoryUsedMB = dedicatedMemory / 1048576;
                                return $"{memoryUsedMB} MB used";
                            }
                        }
                    }
                }
                return "No dedicated memory detected";
            }
            catch (OverflowException ex)
            {
                return $"Overflow error ({ex.Message})";
            }
            catch (Exception ex)
            {
                return $"Error getting load ({ex.Message})";
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
