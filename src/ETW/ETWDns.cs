using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using N8.Lists;
using N8.Classes;
using static N8.Enums.Generic.EventTypes;
using Microsoft.Diagnostics.Tracing;

namespace N8.ETW
{
    public partial class ETWMonitor
    {
        private void OnDnsQuery(TraceEvent data)
        {
            if (TrackedProcesses.PidList.ContainsKey(data.ProcessID))
            {
                var queryName = (string)data.PayloadByName("QueryName");
                LogWriter.TryWrite(new LogEntry(
                    DateTime.UtcNow, DnsQuery, data.ProcessID,
                    data.ProcessName, $"Query {queryName}", queryName
                ));
            }
            ;
        }
    }
}
