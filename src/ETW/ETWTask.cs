using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using N8.Lists;
using N8.Classes;
using static N8.Enums.Generic.EventTypes;
using Microsoft.Diagnostics.Tracing;

namespace N8.ETW
{
    public partial class ETWMonitor
    {
        private void OnTaskRegister(TraceEvent e)
        {
            var task = e.PayloadByName("TaskName") as string;
#pragma warning disable CS8604 // Possible null reference argument.
            LogWriter.TryWrite(new LogEntry(
                DateTime.Now, ScheduledTaskRegister, e.ProcessID, e.ProcessName,
                $"Registered task {task}", task));
#pragma warning restore CS8604 // Possible null reference argument.
        }

    }
}
