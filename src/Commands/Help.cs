namespace LinConsol.Commands
{
    static class Help
    {
        public static void Execute()
        {
            string appName = ProgramInfo.Name.ToLower();

            Console.WriteLine($"<====================================================>{ProgramInfo.Name} COMMANDS<====================================================>");
            Console.WriteLine($"  --help                                            :  Get all available {appName} commands.");
            Console.WriteLine($"  --version                                         :  Get current version of {appName}.");
            Console.WriteLine($"  --exit                                            :  Exit from {appName} console.");
            Console.WriteLine("  --clear                                           :  Clear console.");
            Console.WriteLine("  osinfo                                            :  Get OS system information.");
            Console.WriteLine("  ipinfo                                            :  Get IP-address of current device.");
            Console.WriteLine("  ipinfo --setkey                                   :  Set api-key of ipinfo.io service.");
            Console.WriteLine("  ipinfo --showkey                                  :  Show current api-key of ipinfo.io service.");
            Console.WriteLine("  hwinfo                                            :  Get hardware information.");
            Console.WriteLine("  timeinfo                                          :  Get device time information.");
            Console.WriteLine("  batteryinfo                                       :  Get information about the battery (ON LAPTOPS).");
            Console.WriteLine("  processlist --sort-by Column1,Column2,... --desc  :  Get information about all active processes in the system.");
            Console.WriteLine("                                                         <Column1,Column2,...> - In fact, any number of columns can be introduced (MIN 1, MAX 4).");
            Console.WriteLine("                                                           - PID (Process ID).");
            Console.WriteLine("                                                           - Name (Process name).");
            Console.WriteLine("                                                           - Memory Usage (Occupied memory in the process in MB).");
            Console.WriteLine("                                                           - State (Condition of the process).");
            Console.WriteLine("                                                         [--desc] - An optional parameter that will sort the process list from LARGE to SMALL value.");
            Console.WriteLine("  ping                                              :  Check internet connection on device.");
            Console.WriteLine("<======================================================================================================================================>");
        }
    }
}