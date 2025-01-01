using System;
using System.Net.NetworkInformation;
using System.Net;

static class ProgramInfo
{
    public static readonly string Version = "0.0.10\n  Release date: 2025.01.01";
    public static readonly string Name = "LINCONSOL";
}

class LinConsol
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Welcome to {ProgramInfo.Name}!");
        Console.WriteLine($"Enter `--help` for list of available commands.");
        Console.WriteLine("Enter `--exit` to exit from console.");

        while (true)
        {
            Console.Write("\n> ");
            string input = Console.ReadLine()?.Trim();

            if (input.Equals("--exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            ProcessCommand(input);
        }
    }

    static void ProcessCommand(string command)
    {
        switch (command.ToLower())
        {
            case "--help":
                ShowHelp();
                break;
            case "--version":
                ShowVersion();
                break;
            case "osinfo":
                ShowOSInfo();
                break;
            case "ipaddress":
                ShowIPAddress();
                break;
            case "internetconn":
                CheckInternetConnection();
                break;
            default:
                Console.WriteLine($"Unknown command `{command}`.");
                Console.WriteLine($"Enter `--help` for list of available commands.");
                break;
        }
    }

    static void ShowHelp()
    {
        Console.WriteLine($"<==========>{ProgramInfo.Name} COMMANDS<==========>");
        Console.WriteLine($"  --help        :    Get all available {ProgramInfo.Name.ToLower()} commands.");
        Console.WriteLine($"  --version     :    Get current version of {ProgramInfo.Name.ToLower()}.");
        Console.WriteLine($"  --exit        :    Exit from {ProgramInfo.Name.ToLower()} console.");
        Console.WriteLine("  osinfo        :    Get OS system information.");
        Console.WriteLine("  ipaddress     :    Get IP-address of current device.");
        Console.WriteLine("  internetconn  :    Check internet connection on device.");
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
        Console.WriteLine($"  OS          : {Environment.OSVersion}");
        Console.WriteLine($"  System Type : {FindSystemType()}");
        Console.WriteLine($"  PC-name     : {Environment.MachineName}");
        Console.WriteLine($"  Username    : {Environment.UserName}");
        Console.WriteLine("<=============================>");
    }

    static void ShowIPAddress()
    {
        string ipAddress = "Unknown";

        try
        {
            string hostName = Dns.GetHostName();
            var ipAddresses = Dns.GetHostAddresses(hostName);

            foreach (var ip in ipAddresses)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddress = ip.ToString();
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting ip address: {ex.Message}");
        }

        Console.WriteLine("<==========>IP ADDRESS<==========>");
        Console.WriteLine($"  IP         : {ipAddress}");
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
                    Console.WriteLine("Internet connection is STABLE.");
                }
                else
                {
                    Console.WriteLine("Internet connection is MISSING.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking internet connection: {ex.Message}");
        }
    }
}
