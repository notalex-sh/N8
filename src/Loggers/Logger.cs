using System.Threading.Tasks;
using System.Threading.Channels;
using N8.Classes;

namespace N8.Loggers
{
    public static class EventLoggers
    {
        public static async Task Logger(ChannelReader<LogEntry> logReader)
        {
            // TODO batching to not throttle I/O

            await foreach (var log in logReader.ReadAllAsync())
            {
                Console.WriteLine(log.ToString());
            }
        }
    }
}