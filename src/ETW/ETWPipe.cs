using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using N8.Lists;
using N8.Classes;
using static N8.Enums.Generic.EventTypes;
using Microsoft.Diagnostics.Tracing;

namespace N8.ETW
{
    public partial class ETWMonitor
    {
        private void OnPipeCreate(FileIOCreateTraceData d)
        {
            if (d.FileName.Contains("\\pipe\\", StringComparison.OrdinalIgnoreCase))
            {
                LogWriter.TryWrite(new LogEntry(
                    DateTime.Now, PipeCreate, d.ProcessID, d.ProcessName,
                    $"Created pipe {d.FileName}", d.FileName));
            }
        }

    }
}
