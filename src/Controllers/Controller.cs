using N8.Classes;
using N8.Utilities;
using N8.Lists;
using N8.Loggers;
using N8.ETW;
using System.Threading.Tasks;

namespace N8.Controllers
{
    public class Controller
    {
        public async Task Start(Target target)
        {
#pragma warning disable CS8604 // shouldnt be for path, null is probably null
            TrackedFiles.FileList.Add(new FileNode(target.path, null));
#pragma warning restore CS8604 

            // Currently an unbounded channel, to have the ETW thread write to the queue and have file I/O in another thread
            // allows non-blocking I/O 
            var logQueue = new LogQueue();

            TrackedProcesses.ProcessList.Add(
                // todo command line args in process launch
                // Spawns new process by adding it to tracked processes list
                new ProcessNode(
                    ProcessUtilities.StartProcess(TrackedFiles.FileList[0]),
                    TrackedFiles.FileList[0].name ?? "Unknown",
                    "Standard User",
                    "",
                    true    
                )
            );

            // TODO get actual priv and command line args of spawned process
            await logQueue.LogWriter.WriteAsync(
                new LogEntry(
                    DateTime.Now,
                    "Target Process Spawned",
                    TrackedProcesses.ProcessList[0].ProcessId,
                    TrackedProcesses.ProcessList[0].ProcessName,
                    "N8 spawned the target process",
                    TrackedFiles.FileList[0].path ?? target.path
                )
            );

            var logger = EventLoggers.Logger(logQueue.LogReader);
            var etwmonitor = new ETWMonitor(logQueue.LogWriter);

            await Task.WhenAll(logger, etwmonitor.StartMonitoring());

            // TODO start ETW monitoring and tracking thy processes
            
            // idea is that controller will start etw monitoring and control everything listening to signals and stopping when all proc exited
            // will maintain list of monitored processes and files written to by any of these (to track if they are also spawned)
            
            // currently am able to spawn process correctly, as well as setup lists for monitoring
        }
    }
}
