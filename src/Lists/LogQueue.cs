using System.Threading.Channels;
using N8.Classes;

namespace N8.Lists
{
    public class LogQueue
    {
        // unbounded channel for logs, as well as classes for writing to/from the channel
        public Channel<LogEntry> LogChannel { get; }
        public ChannelWriter<LogEntry> LogWriter { get; }
        public ChannelReader<LogEntry> LogReader { get; }

        public LogQueue()
        {   
            // TODO unbounded for now, might make bounded depending on performance
            LogChannel = Channel.CreateUnbounded<LogEntry>();
            LogWriter = LogChannel.Writer;
            LogReader = LogChannel.Reader;
        }

    }
}