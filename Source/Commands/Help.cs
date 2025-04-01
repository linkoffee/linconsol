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
            Console.WriteLine("  gpuinfo                                  :  Get detailed information about the PC GPU.");
            Console.WriteLine("  cpuinfo                                  :  Get detailed information about the PC CPU.");
            Console.WriteLine("  timeinfo                                 :  Get device time information.");
            Console.WriteLine("  userinfo                                 :  Get information about the current system user.");
            Console.WriteLine("  batteryinfo                              :  Get information about the battery (ON LAPTOPS).");
            Console.WriteLine("  processlist                              :  Get a list of all active processes in the system (Without sorting).");
            Console.WriteLine("  processlist --sort-by ColumnName --desc  :  Get a list of all active processes in the system.");
            Console.WriteLine("                                                <ColumnName> - The name of the column on which sorting occurs (MAX 1).");
            Console.WriteLine("                                                  - PID (Process ID).");
            Console.WriteLine("                                                  - Name (Process name).");
            Console.WriteLine("                                                  - Memory Usage (Occupied memory in the process in MB).");
            Console.WriteLine("                                                  - State (Condition of the process).");
            Console.WriteLine("                                                [--desc] - An optional parameter that will sort the process list from LARGE to SMALL value.");
            Console.WriteLine("  networklist                              :  Get a list of all network interfaces (Without sorting).");
            Console.WriteLine("  networklist --sort-by ColumnName --desc  :  Get a list of all network interfaces.");
            Console.WriteLine("                                                <ColumnName> - The name of the column on which sorting occurs (MAX 1).");
            Console.WriteLine("                                                  - Interface (Interface name).");
            Console.WriteLine("                                                  - IPv4 Address.");
            Console.WriteLine("                                                  - MAC Address.");
            Console.WriteLine("                                                  - Status (Condition of the interface).");
            Console.WriteLine("                                                [--desc] - An optional parameter that will sort the process list from LARGE to SMALL value.");
            Console.WriteLine("  devicelist                               :  Get a list of all connected devices (Without sorting).");
            Console.WriteLine("  devicelist --sort-by ColumnName --desc   :  Get a list of all connected devices.");
            Console.WriteLine("                                                <ColumnName> - The name of the column on which sorting occurs (MAX 1).");
            Console.WriteLine("                                                  - Name (Device Name).");
            Console.WriteLine("                                                  - Device Type.");
            Console.WriteLine("                                                  - Status (Condition of the device).");
            Console.WriteLine("                                                [--desc] - An optional parameter that will sort the process list from LARGE to SMALL value.");
            Console.WriteLine("  ping                                     :  Check internet connection on device.");
            Console.WriteLine("<================================================================================>");
        }
    }
}