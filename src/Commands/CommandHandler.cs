namespace LinConsol.Commands
{
    static class CommandHandler
    {
        static readonly Dictionary<string, Action<string[]>> CommandHandlers = new()
        {
            { "--help", args => Help.Execute() },
            { "--version", args => Version.Execute() },
            { "--exit", args => Exit.Execute() },
            { "--clear", args => Clear.Execute() },
            { "osinfo", args => OSInfo.Execute() },
            { "ipinfo", args => IPInfo.Execute().Wait() },
            { "ipinfo --setkey", args => IPInfo.SetApiKey() },
            { "ipinfo --showkey", args => IPInfo.ShowApiKey() },
            { "hwinfo", args => HWInfo.Execute() },
            { "timeinfo", args => TimeInfo.Execute() },
            { "batteryinfo", args => BatteryInfo.Execute() },
            { "processlist", ProcessList.Execute },
            { "ping", args => Ping.Execute() }
        };

        public static void ProcessCommand(string input)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            string command = parts[0].ToLower();
            string[] args = parts.Skip(1).ToArray();

            if (CommandHandlers.TryGetValue(command, out var handler))
            {
                handler(args);
            }
            else
            {
                Console.WriteLine($"Unknown command `{input}`.");
                Utils.SuggestCommand.SuggestClosestCommand(input, CommandHandlers.Keys);
                Console.WriteLine($"Or enter `--help` for list of available commands.");
            }
        }
    }
}