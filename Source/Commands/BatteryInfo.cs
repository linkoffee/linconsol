using System.Management;

namespace LinConsol.Commands
{
    static class BatteryInfo
    {
        public static void Execute()
        {
            var batteryStatus = GetBatteryStatus();
            var chargeLevel = GetChargeLevel();
            var timeLeft = GetBatteryTimeLeft();

            Notification();

            Console.WriteLine("<============>BATTERY INFO<============>");
            Console.WriteLine($"  Status       : {batteryStatus}");
            Console.WriteLine($"  Charge Level : {chargeLevel}");
            Console.WriteLine($"  Time Left    : {timeLeft}");
            Console.WriteLine("<======================================>");
        }

        private static string GetBatteryStatus()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT BatteryStatus FROM Win32_Battery");
                foreach (ManagementObject obj in searcher.Get())
                {
                    if (obj["BatteryStatus"] != null)
                    {
                        int status = Convert.ToInt32(obj["BatteryStatus"]);
                        return status switch
                        {
                            1 => "Discharging",
                            2 => "Charging",
                            3 => "Full",
                            4 => "Low",
                            5 => "Critical",
                            _ => "Unknown"
                        };
                    }
                }
            }
            catch (Exception)
            {
                return "Error retrieving battery status";
            }
            return "No battery detected";
        }

        private static string GetChargeLevel()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Battery");
                var battery = searcher.Get().GetEnumerator();
                if (battery.MoveNext())
                {
                    return $"{battery.Current["EstimatedChargeRemaining"]}%";
                }
            }
            catch (Exception)
            {
                return "N/A";
            }
            return "N/A";
        }

        private static string GetBatteryTimeLeft()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Battery");
                var battery = searcher.Get().GetEnumerator();
                if (battery.MoveNext())
                {
                    var remainingMinutes = Convert.ToInt32(battery.Current["EstimatedRunTime"]);
                    if (remainingMinutes > 60)
                    {
                        int hours = remainingMinutes / 60;
                        int minutes = remainingMinutes % 60;
                        return $"{hours} hours {minutes} minutes";
                    }
                    else
                    {
                        return $"{remainingMinutes} minutes";
                    }
                }
            }
            catch (Exception)
            {
                return "N/A";
            }
            return "N/A";
        }

        private static void Notification()
        {
            string chargeLevel = GetChargeLevel();

            if (chargeLevel == "N/A")
            {
                Console.WriteLine("It was not possible to determine information about the battery.\nPerhaps you use a desktop PC, not a laptop.");
            }
        }
    }
}
