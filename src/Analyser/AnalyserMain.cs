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
        public HashSet<string> FileReads = new();
        public HashSet<string> FileDeletes = new();
        public HashSet<string> RegistryCreates = new();
        public HashSet<string> RegistryOpens = new();
        public HashSet<string> RegistryDeletes = new();
        public HashSet<string> RegistrySetValues = new();
        public HashSet<string> RegistryDeleteValues = new();
        public HashSet<string> DnsQueries = new HashSet<string>();
        public HashSet<string> DriversLoaded = new();
        public HashSet<string> ServicesInstalled = new();
        public HashSet<string> TasksRegistered = new();
        public HashSet<string> PipesCreated = new();
        public HashSet<string> WmiQueries = new();

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
                    case FileRead:
                        ProcessEvent(FileReads, log.Target);
                        break;
                    case FileDelete:
                        ProcessEvent(FileDeletes, log.Target);
                        break;
                    case DnsQuery:
                        ProcessEvent(DnsQueries, log.Target);
                        break;
                    case RegistryCreate:
                        ProcessEvent(RegistryCreates, log.Target);
                        break;
                    case RegistryOpen:
                        ProcessEvent(RegistryOpens, log.Target);
                        break;
                    case RegistryDelete:
                        ProcessEvent(RegistryDeletes, log.Target);
                        break;
                    case RegistrySetValue:
                        ProcessEvent(RegistrySetValues, log.Target);
                        break;
                    case RegistryDeleteValue:
                        ProcessEvent(RegistryDeleteValues, log.Target);
                        break;
                    case DriverLoad:
                        ProcessEvent(DriversLoaded, log.Target); break;
                    case ServiceInstall:
                        ProcessEvent(ServicesInstalled, log.Target); break;
                    case ScheduledTaskRegister:
                        ProcessEvent(TasksRegistered, log.Target); break;
                    case PipeCreate:
                        ProcessEvent(PipesCreated, log.Target); break;
                    case WmiQuery:
                        ProcessEvent(WmiQueries, log.Target); break;
                }
            }
        }
    }
}