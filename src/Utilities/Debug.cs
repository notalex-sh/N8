using N8.Lists;

namespace N8.Utilities
{
    // random stuff for debugging
    public static class Debug
    {
        public static void PrintProcesses()
        {
            foreach (var kvp in TrackedProcesses.PidList)
            {
                Console.WriteLine($"{kvp.Key} = {kvp.Value}");
            }
        }
    }
}