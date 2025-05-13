using N8.Classes;
using static N8.Enums.Generic.EventTypes;

namespace N8.Analyser
{
    public class LogAnalyser
    {
        private LogEntry[] Logs;

        // each value being looked at in here for summaries etc, simple post execution at the moment
        // using a hashset to collect the relevant data for quick access times etc

        public HashSet<string> Processes = new HashSet<string>();
        public HashSet<string> Images = new HashSet<string>();

        public HashSet<string> IpInbound = new HashSet<string>();

        public HashSet<string> IpOutbound = new HashSet<string>();

        public HashSet<string> FileWrites = new HashSet<string>();

        public LogAnalyser(LogEntry[] logs)
        {
            Logs = logs;
        }

        // avoid duplicates
        private void ProcessEvent(HashSet<string> list, string value)
        {
            if (!list.Contains(value))
            {
                list.Add(value);
            }
        }

        // print summary

        public void GetSummary()
        {
            foreach (LogEntry log in Logs)
            {
                switch (log.EventType)
                {
                    case ProcessStart:
                        ProcessEvent(Processes, log.ProcessName);
                        break;
                    case ImageLoad:
                        ProcessEvent(Images, log.Target);
                        break;
                    case TcpConnect:
                    case TcpRecv:
                    case UdpRecv:
                        ProcessEvent(IpInbound, log.Target);
                        break;
                    case TcpSend:
                    case UdpSend:
                        ProcessEvent(IpOutbound, log.Target);
                        break;
                    case FileWrite:
                        ProcessEvent(FileWrites, log.Target);
                        break;
                    
                }
            }
        }
    }
}