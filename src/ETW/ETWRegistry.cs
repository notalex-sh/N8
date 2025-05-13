using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using N8.Lists;
using N8.Classes;
using static N8.Enums.Generic.EventTypes;

namespace N8.ETW
{
    public partial class ETWMonitor
    {
        private void OnRegCreate(RegistryTraceData data)
        {
            if (TrackedProcesses.PidList.ContainsKey(data.ProcessID))
            {
                var path = data.KeyName;
                LogWriter.TryWrite(new LogEntry(
                    DateTime.Now,
                    RegistryCreate,
                    data.ProcessID,
                    data.ProcessName,
                    $"Created registry key: {path}",
                    path
                ));
            }
        }

        private void OnRegOpen(RegistryTraceData data)
        {
            if (TrackedProcesses.PidList.ContainsKey(data.ProcessID))
            {
                var path = data.KeyName;
                LogWriter.TryWrite(new LogEntry(
                    DateTime.Now,
                    RegistryOpen,
                    data.ProcessID,
                    data.ProcessName,
                    $"Opened registry key: {path}",
                    path
                ));
            }
        }

        private void OnRegDelete(RegistryTraceData data)
        {
            if (TrackedProcesses.PidList.ContainsKey(data.ProcessID))
            {
                var path = data.KeyName;
                LogWriter.TryWrite(new LogEntry(
                    DateTime.Now,
                    RegistryDelete,
                    data.ProcessID,
                    data.ProcessName,
                    $"Deleted registry key: {path}",
                    path
                ));
            }
        }

        private void OnRegSetValue(RegistryTraceData data)
        {
            if (TrackedProcesses.PidList.ContainsKey(data.ProcessID))
            {
                var fullPath = $"{data.KeyName}\\{data.ValueName}";
                LogWriter.TryWrite(new LogEntry(
                    DateTime.Now,
                    RegistrySetValue,
                    data.ProcessID,
                    data.ProcessName,
                    $"Set registry value: {fullPath}",
                    fullPath
                ));
            }
        }

        private void OnRegDeleteValue(RegistryTraceData data)
        {
            if (TrackedProcesses.PidList.ContainsKey(data.ProcessID))
            {
                var fullPath = $"{data.KeyName}\\{data.ValueName}";
                LogWriter.TryWrite(new LogEntry(
                    DateTime.Now,
                    RegistryDeleteValue,
                    data.ProcessID,
                    data.ProcessName,
                    $"Deleted registry value: {fullPath}",
                    fullPath
                ));
            }
        }
    }
}
