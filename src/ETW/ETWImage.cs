using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using N8.Lists;
using N8.Classes;
using static N8.Enums.Generic.EventTypes;

namespace N8.ETW
{
    public partial class ETWMonitor
    {
        private void OnImageLoad(ImageLoadTraceData data)
        {
            if (TrackedFiles.PathList.ContainsKey(data.FileName))
            {

                if (!TrackedProcesses.PidList.ContainsKey(data.ProcessID))
                {
                    TrackedProcesses.Add(
                        data.ProcessID
                    );
                }



                var entry = new LogEntry(
                    DateTime.Now,
                    ImageLoad,
                    data.ProcessID,
                    data.ProcessName,
                    $"{data.FileName} loaded by {data.ProcessID}.",
                    data.FileName
                );
                LogWriter.WriteAsync(entry);
            }
        }
    }
}
