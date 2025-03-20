using System.Net.NetworkInformation;

namespace LinConsol.Commands
{
    static class Ping
    {
        public static void Execute()
        {
            try
            {
                using (System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping())
                {
                    PingReply reply = ping.Send("8.8.8.8", 3000); // Google DNS
                    if (reply.Status == IPStatus.Success)
                    {
                        Console.WriteLine("pong!");
                        Console.WriteLine("Internet connection is STABLE.");
                    }
                    else
                    {
                        Console.WriteLine("Internet connection is MISSING.");
                    }
                }
            }
            catch (PingException ex)
            {
                Console.WriteLine($"Network error: Unable to reach the server.\n  {ex.Message}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}.");
            }
        }
    }
}