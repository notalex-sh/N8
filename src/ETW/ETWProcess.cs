using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using static N8.Enums.Generic.EventTypes;
using N8.Lists;
using N8.Classes;

namespace N8.ETW
{
    public partial class ETWMonitor
    {
        // process spawning
        private void OnProcessStart(ProcessTraceData data)
        {
            if (TrackedProcesses.PidList.ContainsKey(data.ParentID))
            {

                TrackedProcesses.Add(
                    data.ProcessID
                );


                var entry = new LogEntry(
                    DateTime.Now,
                    ProcessStart,
                    data.ProcessID,
                    data.ProcessName,
                    $"{data.ProcessID} spawned by PID={data.ParentID}.",
                    data.ImageFileName
                );
                LogWriter.WriteAsync(entry);
            }
        }

        // process exiting
        private void OnProcessStop(ProcessTraceData data)
        {
            if (TrackedProcesses.PidList.ContainsKey(data.ProcessID))
            {

                TrackedProcesses.Stop(
                    data.ProcessID
                );

                var entry = new LogEntry(
                    DateTime.Now,
                    ProcessStop,
                    data.ProcessID,
                    data.ImageFileName,
                    $"{data.ProcessID} has exited.",
                    data.ImageFileName
                );
                LogWriter.WriteAsync(entry);

                if (TrackedProcesses.AllDead())
                {
                    StopMonitoring();
                }
            }
        }
    }
}
