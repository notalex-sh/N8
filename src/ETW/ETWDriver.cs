using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using N8.Lists;
using N8.Classes;
using static N8.Enums.Generic.EventTypes;
using Microsoft.Diagnostics.Tracing;

namespace N8.ETW
{
    public partial class ETWMonitor
    {
        private void OnKernelImageLoad(ImageLoadTraceData data)
        {
            if (data.FileName.EndsWith(".sys", StringComparison.OrdinalIgnoreCase))
            {
                LogWriter.TryWrite(new LogEntry(
                    DateTime.Now, DriverLoad, data.ProcessID, data.ProcessName,
                    $"Loaded driver {data.FileName}", data.FileName));
            }
        }
    }
}
