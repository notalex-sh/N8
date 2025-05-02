using N8.Classes;
using System.Collections.Generic;

namespace N8.Lists
{
    // Track processes
    public static class TrackedProcesses
    {
        public static Dictionary<int, ProcessNode> ProcessList { get; } = new Dictionary<int, ProcessNode>();
        public static HashSet<int> PidList { get; set; } = new HashSet<int>();

        public static void Add(ProcessNode newProcess)
        {
            PidList.Add(newProcess.ProcessId);
            ProcessList.Add(newProcess.ProcessId, newProcess);
        }

    }
}