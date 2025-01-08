using System;
using System.Runtime.InteropServices;

namespace LinConsol.Commands
{
    static class OSInfo
    {
        public static void Execute()
        {
            string FindSystemType() => Environment.Is64BitOperatingSystem ? "x64-based" : "x32-based";

            Console.WriteLine("<==========>OS INFO<==========>");
            Console.WriteLine($"  OS          : {RuntimeInformation.OSDescription}");
            Console.WriteLine($"  System Type : {FindSystemType()}");
            Console.WriteLine($"  PC-name     : {Environment.MachineName}");
            Console.WriteLine($"  Username    : {Environment.UserName}");
            Console.WriteLine("<=============================>");
        }
    }
}