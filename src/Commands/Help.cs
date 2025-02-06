namespace LinConsol.Commands
{
    static class Help
    {
        public static void Execute()
        {
            string appName = ProgramInfo.Name.ToLower();

            Console.WriteLine($"<==========>{ProgramInfo.Name} COMMANDS<==========>");
            Console.WriteLine($"  --help            :    Get all available {appName} commands.");
            Console.WriteLine($"  --version         :    Get current version of {appName}.");
            Console.WriteLine($"  --exit            :    Exit from {appName} console.");
            Console.WriteLine("  --clear           :    Clear console.");
            Console.WriteLine("  osinfo            :    Get OS system information.");
            Console.WriteLine("  ipinfo            :    Get IP-address of current device.");
            Console.WriteLine("  ipinfo --setkey   :    Set api-key of ipinfo.io service.");
            Console.WriteLine("  ipinfo --showkey  :    Show current api-key of ipinfo.io service.");
            Console.WriteLine("  hwinfo            :    Get hardware information.");
            Console.WriteLine("  timeinfo          :    Get device time information.");
            Console.WriteLine("  ping              :    Check internet connection on device.");
            Console.WriteLine("<========================================>");
        }
    }
}