using System.Linq;
using System.Collections.Concurrent;

namespace N8.Lists
{
    // Track processes
    public static class TrackedProcesses
    {
        public static ConcurrentDictionary<int, bool> PidList { get; set; } = new ConcurrentDictionary<int, bool>();

        public static void Add(int ProcessID)
        {
            PidList[ProcessID] = true;
        }

        public static void Stop(int ProcessID)
        {
            PidList[ProcessID] = false;
        }

        // Check if any processes are still running, used to signal end of sandboxing session
        public static bool AllDead()
        {
            return PidList.Values.All(pid => pid == false);
        }

    }
}