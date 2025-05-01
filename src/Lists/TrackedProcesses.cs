using N8.Classes;
using System.Collections.Generic;

namespace N8.Lists
{
    // Track processes
    public static class TrackedProcesses
    {
        public static List<ProcessNode> ProcessList { get; } = new List<ProcessNode>();

        public static void Add(ProcessNode newProcess)
        {
            ProcessList.Add(newProcess);
        }

    }
}