namespace N8.Classes
{
    // Monitored processes
    public class ProcessNode
    {
        public int ProcessId { get; }
        public string ProcessName { get; }
        public string Privilege { get; } = "Unknown";
        public string CommandLine { get; } = "";

        public bool IsAlive { get; } = false;
        public List<ProcessNode> Children { get; set; } = new List<ProcessNode>();

        public ProcessNode(int processId, string processName, string priviledge="Unknown", string commandLine="", bool isAlive=false )
        {
            ProcessId = processId;
            ProcessName = processName;
            Privilege = priviledge;
            CommandLine = commandLine;
            IsAlive = isAlive;
        }

    }
}
