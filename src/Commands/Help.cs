namespace LinConsol.Commands
{
    static class Help
    {
        public static void Execute()
        {
            string appName = ProgramInfo.Name.ToLower();

            Console.WriteLine($"<=========================>{ProgramInfo.Name} COMMANDS<=========================>");
            Console.WriteLine($"  --help                                   :  Get all available {appName} commands.");
            Console.WriteLine($"  --version                                :  Get current version of {appName}.");
            Console.WriteLine($"  --exit                                   :  Exit from {appName} console.");
            Console.WriteLine("  --clear                                  :  Clear console.");
            Console.WriteLine("  osinfo                                   :  Get OS system information.");
            Console.WriteLine("  ipinfo                                   :  Get IP-address of current device.");
            Console.WriteLine("  ipinfo --setkey                          :  Set api-key of ipinfo.io service.");
            Console.WriteLine("  ipinfo --showkey                         :  Show current api-key of ipinfo.io service.");
            Console.WriteLine("  hwinfo                                   :  Get hardware information.");
            Console.WriteLine("  timeinfo                                 :  Get device time information.");
            Console.WriteLine("  userinfo                                 :  Get information about the current system user.");
            Console.WriteLine("  batteryinfo                              :  Get information about the battery (ON LAPTOPS).");
            Console.WriteLine("  processlist                              :  Get information about all active processes in the system (Without sorting).");
            Console.WriteLine("  processlist --sort-by ColumnName --desc  :  Get information about all active processes in the system.");
            Console.WriteLine("                                                <ColumnName> - The name of the column on which sorting occurs (MAX 1).");
            Console.WriteLine("                                                  - PID (Process ID).");
            Console.WriteLine("                                                  - Name (Process name).");
            Console.WriteLine("                                                  - Memory Usage (Occupied memory in the process in MB).");
            Console.WriteLine("                                                  - State (Condition of the process).");
            Console.WriteLine("                                                [--desc] - An optional parameter that will sort the process list from LARGE to SMALL value.");
            Console.WriteLine("  ping                                     :  Check internet connection on device.");
            Console.WriteLine("<================================================================================>");
        }
    }
}