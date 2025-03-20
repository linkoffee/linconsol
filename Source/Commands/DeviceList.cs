using System.Management;

namespace LinConsol.Commands
{
    static class DeviceList
    {
        public static void Execute() => Execute(Array.Empty<string>());

        public static void Execute(string[] args)
        {
            List<(string Name, string Type, string Status)> devices = GetConnectedDevices();

            List<string> validColumns = new() { "Name", "Device Type", "Status" };

            bool isDescending = args.Contains("--desc");
            int sortArgsIndex = Array.IndexOf(args, "--sort-by");
            string? sortColumn = null;

            if (sortArgsIndex != -1 && sortArgsIndex < args.Length - 1)
            {
                sortColumn = string.Join(" ", args.Skip(sortArgsIndex + 1).TakeWhile(arg => !arg.StartsWith("--")));
                if (!validColumns.Contains(sortColumn))
                {
                    Console.WriteLine($"Invalid column: {sortColumn}");
                    Console.WriteLine($"Available columns: {string.Join(", ", validColumns)}");
                    return;
                }
            }

            int maxNameLength = Math.Max("Name".Length, devices.Max(d => d.Name.Length));
            int maxTypeLength = Math.Max("Device Type".Length, devices.Max(d => d.Type.Length));
            int maxStatusLength = Math.Max("Status".Length, devices.Max(d => d.Status.Length));

            Console.WriteLine("");
            Console.WriteLine($"  {"Name".PadRight(maxNameLength)}  {"Device Type".PadRight(maxTypeLength)}  {"Status".PadRight(maxStatusLength)}");
            Console.WriteLine($"  {"".PadRight(maxNameLength, '=')}  {"".PadRight(maxTypeLength, '=')}  {"".PadRight(maxStatusLength, '=')}");

            var sortedDevices = devices.AsEnumerable();

            if (sortColumn != null)
            {
                sortedDevices = sortColumn switch
                {
                    "Name" => isDescending ? sortedDevices.OrderByDescending(d => d.Name) : sortedDevices.OrderBy(d => d.Name),
                    "Device Type" => isDescending ? sortedDevices.OrderByDescending(d => d.Type) : sortedDevices.OrderBy(d => d.Type),
                    "Status" => isDescending ? sortedDevices.OrderByDescending(d => d.Status) : sortedDevices.OrderBy(d => d.Status),
                    _ => sortedDevices
                };
            }

            foreach (var device in sortedDevices)
            {
                Console.WriteLine($"  {device.Name.PadRight(maxNameLength)}  {device.Type.PadRight(maxTypeLength)}  {device.Status.PadRight(maxStatusLength)}");
            }
        }

        private static List<(string Name, string Type, string Status)> GetConnectedDevices()
        {
            List<(string Name, string Type, string Status)> devices = new();

            try
            {
                using ManagementObjectSearcher searcher = new("SELECT * FROM Win32_PnPEntity");
                foreach (ManagementObject obj in searcher.Get())
                {
                    string name = obj["Name"]?.ToString() ?? "Unknown Device";
                    string deviceType = obj["PNPClass"]?.ToString() ?? "Other";
                    string status = obj["Status"]?.ToString() ?? "Unknown";

                    devices.Add((name, deviceType, status));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving device list: {ex.Message}");
            }

            return devices;
        }
    }
}
