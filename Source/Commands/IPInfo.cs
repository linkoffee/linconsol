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
            string ipType = Utils.Network.GetIpType(publicIpAddress);
            string vpnStatus = await Utils.Network.CheckVpnProxy(publicIpAddress);

            Console.WriteLine("<==========>IP INFO<==========>");
            Console.WriteLine($"  Local IP   : {localIpAddress}");
            Console.WriteLine($"  Public IP  : {publicIpAddress}");
            Console.WriteLine($"  IP Type    : {ipType}");
            Console.WriteLine($"  VPN/Proxy  : {vpnStatus}");
            Console.WriteLine($"  Location   : {location}");
            Console.WriteLine($"  Provider   : {provider}");
            Console.WriteLine("<=============================>");
        }

        public static void SetApiKey()
        {
            string? existingApiKey = Utils.ConfigManager.GetApiKey();

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

            Utils.ConfigManager.SetApiKey(newApiKey);
            Console.WriteLine("API key has been saved successfully.");
        }

        public static void ShowApiKey()
        {
            string? apiKey = Utils.ConfigManager.GetApiKey();

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