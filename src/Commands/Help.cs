namespace LinConsol.Commands
{
    static class Help
    {
        public static void Execute()
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
    }
}