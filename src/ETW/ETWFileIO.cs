using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using N8.Lists;
using N8.Classes;
using static N8.Enums.Generic.EventTypes;

namespace N8.ETW
{
    public partial class ETWMonitor
    {
        private void OnFileWrite(FileIOReadWriteTraceData data)
        {
            if (TrackedProcesses.PidList.ContainsKey(data.ProcessID) && !data.FileName.Contains("\\pipe\\", StringComparison.OrdinalIgnoreCase))
            {
                var entry = new LogEntry(
                    DateTime.Now,
                    FileWrite,
                    data.ProcessID,
                    data.ProcessName,
                    $"{data.ProcessID} wrote {data.IoSize} bytes to {data.FileName}.",
                    data.FileName
                );
                LogWriter.TryWrite(entry);
            }
        }

        private void OnFileRead(FileIOReadWriteTraceData data)
        {
            if (TrackedProcesses.PidList.ContainsKey(data.ProcessID))
            {
                var entry = new LogEntry(
                    DateTime.Now,
                    FileRead,
                    data.ProcessID,
                    data.ProcessName,
                    $"{data.ProcessID} read {data.IoSize} bytes from {data.FileName}.",
                    data.FileName
                );
                LogWriter.TryWrite(entry);
            }
        }

        private void OnFileDelete(FileIONameTraceData data)
        {
            if (TrackedProcesses.PidList.ContainsKey(data.ProcessID))
            {
                var entry = new LogEntry(
                    DateTime.Now,
                    FileDelete,
                    data.ProcessID,
                    data.ProcessName,
                    $"{data.ProcessID} deleted {data.FileName}.",
                    data.FileName
                );
                LogWriter.TryWrite(entry);
            }
        }

    }
}
