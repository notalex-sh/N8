namespace N8.Classes
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; } 
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }    

        public LogEntry(DateTime timestamp, string eventType, int processId, string processName, string message, string target)
        {
            Timestamp = timestamp;
            EventType = eventType;
            ProcessId = processId;
            ProcessName = processName;
            Message = message;
            Target = target;
        }

        // TODO: LogEntry formatting
        public override string ToString()
        {
            return $"[{Timestamp:o}] [{EventType}] [PID: {ProcessId}] [Name: {ProcessName}] [Target: {Target}] {Message}";
        }
    }
}