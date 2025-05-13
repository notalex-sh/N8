using System.Threading.Channels;
using N8.Classes;
using N8.Utilities;
using N8.Lists;

namespace N8.Loggers
{
    public static class EventLoggers
    {
        public static async Task Logger(ChannelReader<LogEntry> logReader, (int Left, int Top) MessageCords)
        {
            // parent for logging
            // TODO filestream to logfile, batched writes
            await foreach (var log in logReader.ReadAllAsync())
            {
                LogMemory.Enqueue(log);
                Console.SetCursorPosition(MessageCords.Left, MessageCords.Top);
                Output.ClearLine(MessageCords.Top);
                Output.WriteLineColor($"   [+] {log.Message}", ConsoleColor.Green);
            }
        }
    }
}