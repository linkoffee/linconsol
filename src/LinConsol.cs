using LinConsol.Commands;

namespace LinConsol
{
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

                CommandHandler.ProcessCommand(input);
            }
        }
    }
}