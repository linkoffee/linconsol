using System.Diagnostics;

namespace LinConsol.Commands
{
    static class ProcessList
    {
        public static void Execute() => Execute(Array.Empty<string>());

        public static void Execute(string[] args)
        {
            Process[] processes = Process.GetProcesses();
            List<string> validColumns = new() { "PID", "Name", "Memory Usage", "State" };

            bool isDescending = args.Contains("--desc");
            int sortArgsIndex = Array.IndexOf(args, "--sort-by");
            if (sortArgsIndex == -1 || sortArgsIndex == args.Length - 1)
            {
                Console.WriteLine("Usage: processlist --sort-by Column1,Column2,... --desc");
                return;
            }

            List<string> columnParts = new();
            for (int i = sortArgsIndex + 1; i < args.Length; i++)
            {
                if (args[i].StartsWith("--")) break;
                columnParts.Add(args[i]);
            }

            string sortColumn = string.Join(" ", columnParts);

            if (!validColumns.Contains(sortColumn))
            {
                Console.WriteLine($"Invalid column: {sortColumn}");
                Console.WriteLine($"Available columns: {string.Join(", ", validColumns)}");
                return;
            }

            int maxPidLength = Math.Max("PID".Length, processes.Max(p => p.Id.ToString().Length));
            int maxNameLength = Math.Max("Name".Length, processes.Max(p => p.ProcessName.Length));
            int maxMemoryLength = Math.Max("Memory Usage".Length, processes.Max(p => (p.PrivateMemorySize64 / 1024 / 1024).ToString().Length + 3));
            int maxStateLength = Math.Max("State".Length, processes.Max(p => p.Responding ? "Running".Length : "Not Responding".Length));

            Console.WriteLine("");
            Console.WriteLine($"  {"PID".PadRight(maxPidLength)}  {"Name".PadRight(maxNameLength)}  {"Memory Usage".PadRight(maxMemoryLength)}  {"State".PadRight(maxStateLength)}");
            Console.WriteLine($"  {"".PadRight(maxPidLength, '=')}  {"".PadRight(maxNameLength, '=')}  {"".PadRight(maxMemoryLength, '=')}  {"".PadRight(maxStateLength, '=')}");

            var sortedProcesses = processes.Select(p => new
            {
                PID = p.Id,
                Name = p.ProcessName,
                MemoryUsage = p.PrivateMemorySize64 / 1024 / 1024,
                State = p.Responding ? "Running" : "Not Responding"
            });

            sortedProcesses = sortColumn switch
            {
                "PID" => isDescending ? sortedProcesses.OrderByDescending(p => p.PID) : sortedProcesses.OrderBy(p => p.PID),
                "Name" => isDescending ? sortedProcesses.OrderByDescending(p => p.Name) : sortedProcesses.OrderBy(p => p.Name),
                "Memory Usage" => isDescending ? sortedProcesses.OrderByDescending(p => p.MemoryUsage) : sortedProcesses.OrderBy(p => p.MemoryUsage),
                "State" => isDescending ? sortedProcesses.OrderByDescending(p => p.State) : sortedProcesses.OrderBy(p => p.State),
                _ => sortedProcesses
            };

            foreach (var process in sortedProcesses)
            {
                string memoryUsageText = (process.MemoryUsage + " MB").PadRight(maxMemoryLength);
                Console.WriteLine($"  {process.PID.ToString().PadRight(maxPidLength)}  {process.Name.PadRight(maxNameLength)}  {memoryUsageText}  {process.State.PadRight(maxStateLength)}");
            }
        }
    }
}
