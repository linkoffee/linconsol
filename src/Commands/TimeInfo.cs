using System.Management;

namespace LinConsol.Commands
{
    static class TimeInfo
    {
        public static void Execute()
        {
            Console.WriteLine("<==========>TIME INFO<==========>");
            Console.WriteLine($"  Current Time       : {GetCurrentTime()}");
            Console.WriteLine($"  Time Zone          : {GetTimeZone()}");
            Console.WriteLine($"  System Uptime      : {GetSystemUptime()}");
            Console.WriteLine($"  User Session Time  : {GetUserSessionTime()}");
            Console.WriteLine("<===============================>");
        }

        private static string GetCurrentTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private static string GetTimeZone()
        {
            TimeZoneInfo timeZone = TimeZoneInfo.Local;
            string utcOffset = timeZone.BaseUtcOffset.ToString(@"hh\:mm");
            return $"UTC{(timeZone.BaseUtcOffset.Hours >= 0 ? "+" : "")}{utcOffset} ({timeZone.StandardName})";
        }

        private static string GetSystemUptime()
        {
            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
            return $"{uptime.Days} days, {uptime.Hours} hours, {uptime.Minutes} minutes";
        }

        private static string GetUserSessionTime()
        {
            var searcher = new ManagementObjectSearcher("SELECT StartTime FROM Win32_LogonSession WHERE LogonType = 2");
            foreach (var item in searcher.Get())
            {
                DateTime loginTime = ManagementDateTimeConverter.ToDateTime(item["StartTime"].ToString());
                TimeSpan sessionDuration = DateTime.Now - loginTime;
                return $"{sessionDuration.Days} days, {sessionDuration.Hours} hours, {sessionDuration.Minutes} minutes";
            }
            return "Unknown";
        }
    }
}