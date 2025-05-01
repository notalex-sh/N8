namespace N8.Classes
{
    // Monitored processes
    public class ProcessNode
    {
        public int ProcessId { get; }
        public string ProcessName { get; }
        public string Privilege { get; set; } = "Unknown";
        public string CommandLine { get; set; } = "";

        public bool IsAlive { get; set; } = false;
        public List<ProcessNode> Children { get; } = new List<ProcessNode>();

        public ProcessNode(int processId, string processName)
        {
            ProcessId = processId;
            ProcessName = processName;
        }

    }
}
