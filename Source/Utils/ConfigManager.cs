using System.Text.Json;

namespace LinConsol.Utils
{
    static class ConfigManager
    {
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        private class Config
        {
            public string? IPINFO_API_KEY { get; set; }
        }

        private static Config LoadConfig()
        {
            if (!File.Exists(ConfigFilePath))
            {
                return new Config();
            }

            try
            {
                string json = File.ReadAllText(ConfigFilePath);
                return JsonSerializer.Deserialize<Config>(json) ?? new Config();
            }
            catch
            {
                Console.WriteLine("Error reading config. Using default settings.");
                return new Config();
            }
        }

        private static void SaveConfig(Config config)
        {
            try
            {
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions {WriteIndented = true});
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving config: {ex.Message}");
            }
        }

        public static string? GetApiKey()
        {
            var config = LoadConfig();
            return config.IPINFO_API_KEY;
        }

        public static void SetApiKey(string apiKey)
        {
            var config = LoadConfig();
            config.IPINFO_API_KEY = apiKey;
            SaveConfig(config);
        }
    }
}