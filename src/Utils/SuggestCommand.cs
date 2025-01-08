using System;
using System.Collections.Generic;
using System.Linq;

namespace LinConsol.Utils
{
    static class SuggestCommand
    {
        public static void SuggestClosestCommand(string input, IEnumerable<string> commands)
        {
            var suggestions = commands.Where(cmd => cmd.Contains(input)).ToList();
            if (suggestions.Any())
            {
                Console.WriteLine("Did you mean:");
                foreach (var suggestion in suggestions)
                {
                    Console.WriteLine($"  {suggestion}");
                }
            }
        }
    }
}