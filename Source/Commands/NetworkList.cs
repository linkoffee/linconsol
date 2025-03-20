using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace LinConsol.Commands
{
    static class NetworkList
    {
        public static void Execute() => Execute(Array.Empty<string>());

        public static void Execute(string[] args)
        {
            List<string> validColumns = new() { "Interface", "IPv4 Address", "MAC Address", "Status" };
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

            var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Select(nic => new
                {
                    Interface = nic.Name,
                    IPv4Address = nic.GetIPProperties().UnicastAddresses
                        .FirstOrDefault(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)?.Address.ToString() ?? "N/A",
                    MacAddress = string.Join(":", nic.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2"))),
                    Status = nic.OperationalStatus == OperationalStatus.Up ? "Connected" : "Disconnected"
                })
                .ToList();

            if (sortColumn != null)
            {
                interfaces = sortColumn switch
                {
                    "Interface" => isDescending ? interfaces.OrderByDescending(i => i.Interface).ToList() : interfaces.OrderBy(i => i.Interface).ToList(),
                    "IPv4 Address" => isDescending ? interfaces.OrderByDescending(i => i.IPv4Address).ToList() : interfaces.OrderBy(i => i.IPv4Address).ToList(),
                    "MAC Address" => isDescending ? interfaces.OrderByDescending(i => i.MacAddress).ToList() : interfaces.OrderBy(i => i.MacAddress).ToList(),
                    "Status" => isDescending ? interfaces.OrderByDescending(i => i.Status).ToList() : interfaces.OrderBy(i => i.Status).ToList(),
                    _ => interfaces
                };
            }

            int maxInterfaceLength = Math.Max("Interface".Length, interfaces.Max(i => i.Interface.Length));
            int maxIpLength = Math.Max("IPv4 Address".Length, interfaces.Max(i => i.IPv4Address.Length));
            int maxMacLength = Math.Max("MAC Address".Length, interfaces.Max(i => i.MacAddress.Length));
            int maxStatusLength = Math.Max("Status".Length, interfaces.Max(i => i.Status.Length));

            Console.WriteLine("");
            Console.WriteLine($"  {"Interface".PadRight(maxInterfaceLength)}  {"IPv4 Address".PadRight(maxIpLength)}  {"MAC Address".PadRight(maxMacLength)}  {"Status".PadRight(maxStatusLength)}");
            Console.WriteLine($"  {new string('=', maxInterfaceLength)}  {new string('=', maxIpLength)}  {new string('=', maxMacLength)}  {new string('=', maxStatusLength)}");

            foreach (var nic in interfaces)
            {
                Console.WriteLine($"  {nic.Interface.PadRight(maxInterfaceLength)}  {nic.IPv4Address.PadRight(maxIpLength)}  {nic.MacAddress.PadRight(maxMacLength)}  {nic.Status.PadRight(maxStatusLength)}");
            }
        }
    }
}
