using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;

namespace LinConsol.Utils
{
    static class Network
    {
        public static string GetLocalIPAddress()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    foreach (var ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }
            return "Unknown";
        }

        public static async Task<string> GetPublicIPAddress()
        {
            using HttpClient client = new();
            try
            {
                string publicIp = await client.GetStringAsync("https://api.ipify.org");
                return publicIp.Trim();
            }
            catch
            {
                return "Unknown";
            }
        }

        public static async Task<IpInfo?> FetchIpInfo(string ipAddress)
        {
            string? apiKey = ConfigManager.GetApiKey();
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("Error: IPINFO API key not set. Please set your API key using the `ipinfo --setkey` command.");
                Console.WriteLine("Get personal API key: https://ipinfo.io/account/token");
                return null;
            }

            string apiUrl = $"https://ipinfo.io/{ipAddress}/json?token={apiKey}";
            using HttpClient client = new();
            try
            {
                string response = await client.GetStringAsync(apiUrl);
                if (response.Contains("\"readme\":"))
                {
                    Console.WriteLine("API key might be missing or not valid.");
                    return null;
                }

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<IpInfo>(response, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching IP info: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> GetLocation(string ipAddress)
        {
            var ipInfo = await FetchIpInfo(ipAddress);
            if (ipInfo != null && !string.IsNullOrEmpty(ipInfo.City) && !string.IsNullOrEmpty(ipInfo.Region) && !string.IsNullOrEmpty(ipInfo.Country))
            {
                return $"{ipInfo.City}, {ipInfo.Region}, {ipInfo.Country}";
            }
            return "Unknown Location";
        }

        public static async Task<string> GetProvider(string ipAddress)
        {
            var ipInfo = await FetchIpInfo(ipAddress);
            return ipInfo?.Org ?? "Unknown Provider";
        }

        public class IpInfo
        {
            public string? Ip { get; set; }
            public string? City { get; set; }
            public string? Region { get; set; }
            public string? Country { get; set; }
            public string? Loc { get; set; }
            public string? Org { get; set; }
            public string? Postal { get; set; }
            public string? Timezone { get; set; }
        }
    }
}