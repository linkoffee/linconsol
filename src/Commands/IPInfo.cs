using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.Http;
using System.Threading.Tasks;

namespace LinConsol.Commands
{
    static class IPInfo
    {
        public static async Task Execute()
        {
            string localIpAddress = Utils.Network.GetLocalIPAddress();
            string publicIpAddress = await Utils.Network.GetPublicIPAddress();
            string location = await Utils.Network.GetLocation(publicIpAddress);
            string provider = await Utils.Network.GetProvider(publicIpAddress);

            Console.WriteLine("<==========>IP INFO<==========>");
            Console.WriteLine($"  Local IP   : {localIpAddress}");
            Console.WriteLine($"  Public IP  : {publicIpAddress}");
            Console.WriteLine($"  Location   : {location}");
            Console.WriteLine($"  Provider   : {provider}");
            Console.WriteLine("<=============================>");
        }

        public static void SetApiKey()
        {
            string existingApiKey = Environment.GetEnvironmentVariable("IPINFO_API_KEY");

            if (!string.IsNullOrEmpty(existingApiKey))
            {
                Console.WriteLine("An API key is already set. Do you want to replace it? (yes/no)");
                string? answer = Console.ReadLine()?.Trim().ToLower();

                if (answer != "yes")
                {
                    Console.WriteLine("Operation cancelled. The existing API key was not replaced.");
                    return;
                }
            }

            Console.WriteLine("Enter your new ipinfo.io API key:");
            string? newApiKey = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(newApiKey))
            {
                Console.WriteLine("API key cannot be empty.");
                return;
            }
            else if (newApiKey.Length != 14)
            {
                Console.WriteLine("API key must consist of 14 characters.");
                return;
            }

            Environment.SetEnvironmentVariable("IPINFO_API_KEY", newApiKey);
            Console.WriteLine("API key has been set successfully.");
        }

        public static void ShowApiKey()
        {
            string? apiKey = Environment.GetEnvironmentVariable("IPINFO_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("No API key is currently set.");
                Console.WriteLine("Please set your API key using the `ipinfo --setkey` command.");
            }
            else
            {
                string hiddenKey = $"{apiKey.Substring(0, 4)}******{apiKey.Substring(apiKey.Length - 4)}";
                Console.WriteLine($"Current API key: {hiddenKey}");
            }
        }
    }
}