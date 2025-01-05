using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Net.Sockets;

static class ProgramInfo
{
    public static readonly string Version = "0.0.11\n  Release date: 2025.01.05";
    public static readonly string Name = "LINCONSOL";
}

class LinConsol
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Welcome to {ProgramInfo.Name}!");
        Console.WriteLine($"Enter `--help` for list of available commands.");

        while (true)
        {
            Console.Write("\n> ");
            string input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Please enter a command.\nType `--help` for assistance.");
                continue;
            }

            ProcessCommand(input);
        }
    }

    static readonly Dictionary<string, Action> CommandHandlers = new()
    {
        { "--help", ShowHelp },
        { "--version", ShowVersion },
        { "--exit", ExitProgram },
        { "osinfo", ShowOSInfo },
        { "ipinfo", () => ShowIPAddress().Wait() },
        { "ping", CheckInternetConnection }
    };

    static void ProcessCommand(string command)
    {
        if (CommandHandlers.TryGetValue(command.ToLower(), out var handler))
            {
                handler();
            }
            else
            {
                Console.WriteLine($"Unknown command `{command}`.");
                SuggestClosestCommand(command);
                Console.WriteLine($"Or enter `--help` for list of available commands.");
            }
    }

    static void SuggestClosestCommand(string input)
    {
        var suggestions = CommandHandlers.Keys.Where(cmd => cmd.Contains(input)).ToList();

        if (suggestions.Any())
        {
            Console.WriteLine("Did you mean: ");
            foreach (var suggestion in suggestions)
            {
                Console.WriteLine($"  {suggestion}");
            }
        }
    }

    static void ExitProgram()
    {
        Environment.Exit(0);
    }

    static void ShowHelp()
    {
        string appName = ProgramInfo.Name.ToLower();

        Console.WriteLine($"<==========>{ProgramInfo.Name} COMMANDS<==========>");
        Console.WriteLine($"  --help        :    Get all available {appName} commands.");
        Console.WriteLine($"  --version     :    Get current version of {appName}.");
        Console.WriteLine($"  --exit        :    Exit from {appName} console.");
        Console.WriteLine("  osinfo        :    Get OS system information.");
        Console.WriteLine("  ipinfo        :    Get IP-address of current device.");
        Console.WriteLine("  ping          :    Check internet connection on device.");
        Console.WriteLine("<========================================>");
    }

    static void ShowVersion()
    {
        Console.WriteLine($"{ProgramInfo.Name} version - {ProgramInfo.Version}");
    }

    static void ShowOSInfo()
    {
        static string FindSystemType()
        {
            return Environment.Is64BitOperatingSystem ? "x64-based" : "x32-based";
        }

        Console.WriteLine("<==========>OS INFO<==========>");
        Console.WriteLine($"  OS          : {RuntimeInformation.OSDescription}");
        Console.WriteLine($"  System Type : {FindSystemType()}");
        Console.WriteLine($"  PC-name     : {Environment.MachineName}");
        Console.WriteLine($"  Username    : {Environment.UserName}");
        Console.WriteLine("<=============================>");
    }

    static async Task<string> GetPublicIPAddress()
    {
        using HttpClient client = new();
        try
        {
            string publicIp = await client.GetStringAsync("https://api.ipify.org");
            return publicIp.Trim();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching public IP address: {ex.Message}");
            return "Unknown";
        }
    }

    static async Task ShowIPAddress()
    {
        string localIpAddress = "Unknown";
        string publicIpAddress = await GetPublicIPAddress();

        try
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            localIpAddress = ip.Address.ToString();
                            break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting local IP address: {ex.Message}");
        }

        Console.WriteLine("<==========>IP INFO<==========>");
        Console.WriteLine($"  Local IP   : {localIpAddress}");
        Console.WriteLine($"  Public IP  : {publicIpAddress}");
        Console.WriteLine("  Location   : not available now");
        Console.WriteLine("  Provider   : not available now");
        Console.WriteLine("<=============================>");
    }

    static void CheckInternetConnection()
    {
        try
        {
            using (var ping = new Ping())
            {
                PingReply reply = ping.Send("8.8.8.8", 3000); // google dns
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine("pong!");
                    Console.WriteLine("Internet connection is STABLE.");
                }
                else
                {
                    Console.WriteLine("Internet connection is MISSING.");
                }
            }
        }
        catch (PingException ex)
        {
            Console.WriteLine($"Network error: Unable to reach the server.\n  {ex.Message}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}.");
        }
    }
}
