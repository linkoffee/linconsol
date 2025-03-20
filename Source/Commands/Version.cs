namespace LinConsol.Commands
{
    static class Version
    {
        public static void Execute()
        {
            Console.WriteLine($"{ProgramInfo.Name} version - {ProgramInfo.Version}");
        }
    }
}