using static N8.Enums.Generic;

namespace N8.Classes
{
    // log entries
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public EventTypes EventType { get; set; } 
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }    

        public LogEntry(DateTime timestamp, EventTypes eventType, int processId, string processName, string message, string target)
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
            return $"[{Timestamp:o}] [{EventType.ToString()}] [PID: {ProcessId}] [Name: {ProcessName}] [Target: {Target}] {Message}";
        }
    }
}