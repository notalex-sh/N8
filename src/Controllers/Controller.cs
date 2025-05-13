using N8.Classes;
using N8.Utilities;
using N8.Lists;
using N8.Loggers;
using N8.ETW;
using System.Diagnostics;
using static N8.Enums.Generic.EventTypes;
using N8.Analyser;

namespace N8.Controllers
{
    public class Controller
    {
        public (int Left, int Top) MessageCords;
        public async Task Start(Target target)
        {
#pragma warning disable CS8604 // shouldnt null be for path, will have bigger issues if this is true (and should be caught earlier)
            TrackedFiles.Add(target.path);
#pragma warning restore CS8604 

            // spinner and messages during execution
            Output.WriteColor("[*] Running Target ", ConsoleColor.Yellow);
            (int Left, int Top) SpinnerCords = Console.GetCursorPosition();
            Console.WriteLine("\n");
            Output.WriteLineColor("=== Latest Message ===", ConsoleColor.Cyan);
            MessageCords = (0, SpinnerCords.Top + 3);
            var spinner = new Spinner(SpinnerCords.Left, SpinnerCords.Top, Spinners.GetRandom());
            spinner.Start();

            // unbounded channel for logs to allow the different threads to write logs into the queue
            var logQueue = new LogQueue();
            var loggerTask = EventLoggers.Logger(logQueue.LogReader, MessageCords);

            var etwMonitor = new ETWMonitor(logQueue.LogWriter);
            var etwTask = etwMonitor.StartMonitoring();

            // small delay to allow monitors to kick in
            await Task.Delay(1000);

            // todo command line args in process launch
            // Spawns new process and adds it to tracked processes list
            int targetPid = ProcessUtilities.StartProcess(target.path);
            TrackedProcesses.Add(

                targetPid
            );

#pragma warning disable CS8604 // ignore null reference
            var entry = new LogEntry(
                DateTime.Now,
                ProcessStart,
                targetPid,
                target.name,
                $"Target spawned by N8 PID={Process.GetCurrentProcess().Id}.",
                target.path
            );
#pragma warning restore CS8604 

            await logQueue.LogWriter.WriteAsync(entry);

            await Task.WhenAll(loggerTask, etwTask);

            // Stop spinner etc and clean up
            spinner.Stop();

            Generic.CleanUpExecution(SpinnerCords.Top);

            var analyser = new LogAnalyser(LogMemory.GetAll());
            analyser.GetSummary();
            
            Generic.ExecutionSummary(analyser);

        }
    }
}
