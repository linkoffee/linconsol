using System;
using System.Collections.Generic;

namespace LinConsol.Commands
{
    static class CommandHandler
    {
        static readonly Dictionary<string, Action> CommandHandlers = new()
        {
            { "--help", Help.Execute },
            { "--version", Version.Execute },
            { "--exit", Exit },
            { "osinfo", OSInfo.Execute },
            { "ipinfo", () => IPInfo.Execute().Wait() },
            { "ipinfo --setkey", IPInfo.SetApiKey },
            { "ipinfo --showkey", IPInfo.ShowApiKey },
            { "ping", Ping.Execute }
        };

        public static void ProcessCommand(string command)
        {
            if (CommandHandlers.TryGetValue(command.ToLower(), out var handler))
            {
                handler();
            }
            else
            {
                Console.WriteLine($"Unknown command `{command}`.");
                Utils.SuggestCommand.SuggestClosestCommand(command, CommandHandlers.Keys);
                Console.WriteLine($"Or enter `--help` for list of available commands.");
            }
        }

        static void Exit()
        {
            Environment.Exit(0);
        }
    }
}