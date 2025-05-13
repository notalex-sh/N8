using System.Collections.Concurrent;
using System.Text;
using N8.Classes;

namespace N8.Lists
{
    public static class LogMemory
    {
        // in memory logs and helper functions
        private static readonly ConcurrentQueue<LogEntry> entries = new ConcurrentQueue<LogEntry>();

        public static void Enqueue(LogEntry entry)
        {
            entries.Enqueue(entry);
        }

        public static LogEntry[] GetAll()
        {
            return entries.ToArray();
        }

        public static string LogsToString()
        {
            var sb = new StringBuilder();
            foreach (var item in GetAll())
            {
                sb.Append($"{item.ToString()}\n");
            }
            return sb.ToString();
        }
    }
}