using N8.Classes;
using N8.Utilities;
using N8.Lists;
using N8.Loggers;
using N8.ETW;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Diagnostics;

namespace N8.Controllers
{
    public class Controller
    {
        public async Task Start(Target target)
        {
#pragma warning disable CS8604 // shouldnt be for path, null is probably null
            TrackedFiles.Add(new FileNode(target.path, null));
#pragma warning restore CS8604 

            // Currently an unbounded channel, to have the ETW thread write to the queue and have file I/O in another thread
            // allows non-blocking I/O 
            var logQueue = new LogQueue();

            var loggerTask = EventLoggers.Logger(logQueue.LogReader);

            var etwMonitor = new ETWMonitor(logQueue.LogWriter);
            var etwTask = etwMonitor.StartMonitoring();

            await Task.Delay(1000);

            TrackedProcesses.Add(
                // todo command line args in process launch
                // Spawns new process by adding it to tracked processes list
                new ProcessNode(
                    Process.GetCurrentProcess().Id,
                    "N8 Launcher",
                    "Administrator",
                    "",
                    true
                )
            );

            // stored as var for now but may need to change this
        
            int targetPid = ProcessUtilities.StartProcess(TrackedFiles.FileList[target.path]);

#pragma warning disable CS8604 // Possible null reference argument.
            var entry = new LogEntry(
                DateTime.Now,
                "ProcessStart",
                Process.GetCurrentProcess().Id,
                "N8",
                $"Target spawned by N8 PID={Process.GetCurrentProcess().Id}.",
                target.name
            );
#pragma warning restore CS8604 // Possible null reference argument.

            await logQueue.LogWriter.WriteAsync(entry);





            await Task.WhenAll(loggerTask, etwTask);

            // TODO start ETW monitoring and tracking thy processes

            // idea is that controller will start etw monitoring and control everything listening to signals and stopping when all proc exited
            // will maintain list of monitored processes and files written to by any of these (to track if they are also spawned)

            // currently am able to spawn process correctly, as well as setup lists for monitoring
        }
    }
}
