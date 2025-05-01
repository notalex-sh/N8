using System.Threading.Channels;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;
using N8.Classes;

namespace N8.ETW
{
    public partial class ETWMonitor
    {
        private ChannelWriter<LogEntry> LogWriter;

        public ETWMonitor(ChannelWriter<LogEntry> logWriter)
        {
            LogWriter = logWriter;
        }

        // simple hookup of ETW kernel source, just looking at process starting

        public Task StartMonitoring()
        {
            return Task.Run(() =>
            {
                using var session = new TraceEventSession("N8MonitorSession");
                session.EnableKernelProvider(KernelTraceEventParser.Keywords.Process);
                session.Source.Kernel.ProcessStart += data =>
                {
                    var entry = new LogEntry(
                        DateTime.Now,
                        "ProcessStart",
                        data.ProcessID,
                        data.ProcessName,
                        $"{data.ProcessID} spawned by PID={data.ParentID}.",
                        data.ImageFileName
                    );

                    LogWriter.WriteAsync(entry);
                };
                session.Source.Process();
                LogWriter.Complete();
            });

        }
    }
}