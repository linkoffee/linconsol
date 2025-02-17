using System.Security.Principal;

namespace LinConsol.Commands
{
    static class UserInfo
    {
        public static void Execute()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            string userSid = identity.User?.Value ?? "Unknown";
            string username = Environment.UserName;
            string domain = Environment.UserDomainName;
            string rootDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            string role = isAdmin ? "Administrator" : "User";

            Console.WriteLine("<===========>USER INFO<===========>");
            Console.WriteLine($"  SID       : {userSid}");
            Console.WriteLine($"  Username  : {username}");
            Console.WriteLine($"  Role      : {role}");
            Console.WriteLine($"  Domain    : {domain}");
            Console.WriteLine($"  Root Dir  : {rootDir}");
            Console.WriteLine("  Groups    :");
            PrintUserGroups(identity);
            Console.WriteLine("<=================================>");
        }

        private static void PrintUserGroups(WindowsIdentity identity)
        {
            foreach (var group in identity.Groups?.Translate(typeof(NTAccount)) ?? new IdentityReferenceCollection())
            {
                Console.WriteLine($"    {group.Value}");
            }
        }
    }
}
