using N8.Lists;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using static N8.Enums.Generic.EventTypes;
using N8.Classes;
using N8.Utilities;


namespace N8.ETW
{
    public partial class ETWMonitor
    {
        private void OnTcpConnect(TcpIpConnectTraceData d)
        {
            if (!TrackedProcesses.PidList.ContainsKey(d.ProcessID)) return;
            if (Generic.IsLoopback(d.daddr)) return;
            if (Generic.IsLoopback(d.saddr)) return;

            var entry = new LogEntry(
                DateTime.Now,
                TcpConnect,
                d.ProcessID,
                $"{d.saddr}:{d.sport} → {d.daddr}:{d.dport}",
                $"PID {d.ProcessID} opened TCP connection from {d.saddr}:{d.sport} to {d.daddr}:{d.dport}",
                $"{d.daddr}:{d.dport}"
            );
            LogWriter.WriteAsync(entry);
        }

        private void OnTcpSend(TcpIpSendTraceData d)
        {
            if (!TrackedProcesses.PidList.ContainsKey(d.ProcessID)) return;
            if (Generic.IsLoopback(d.daddr)) return;

            var entry = new LogEntry(
                DateTime.Now,
                TcpSend,
                d.ProcessID,
                $"{d.daddr}:{d.dport}",
                $"PID {d.ProcessID} sent {d.size} bytes to {d.daddr}:{d.dport}",
                $"{d.daddr}:{d.dport}"
            );
            LogWriter.WriteAsync(entry);
        }

        private void OnTcpRecv(TcpIpTraceData d)
        {
            if (!TrackedProcesses.PidList.ContainsKey(d.ProcessID)) return;
            if (Generic.IsLoopback(d.saddr)) return;

            var entry = new LogEntry(
                DateTime.Now,
                TcpRecv,
                d.ProcessID,
                $"{d.saddr}:{d.sport}",
                $"PID {d.ProcessID} received {d.size} bytes from {d.daddr}:{d.dport}",
                $"{d.saddr}:{d.sport}"
            );
            LogWriter.WriteAsync(entry);
        }

        private void OnUdpSend(UdpIpTraceData d)
        {
            if (!TrackedProcesses.PidList.ContainsKey(d.ProcessID)) return;
            if (Generic.IsLoopback(d.daddr)) return;

            var entry = new LogEntry(
                DateTime.Now,
                UdpSend,
                d.ProcessID,
                $"{d.daddr}:{d.dport}",
                $"PID {d.ProcessID} sent {d.size} bytes over UDP to {d.daddr}:{d.dport}",
                $"{d.daddr}:{d.dport}"
            );
            LogWriter.WriteAsync(entry);
        }

        private void OnUdpRecv(UdpIpTraceData d)
        {
            if (!TrackedProcesses.PidList.ContainsKey(d.ProcessID)) return;
            if (Generic.IsLoopback(d.saddr)) return;

            var entry = new LogEntry(
                DateTime.Now,
                UdpRecv,
                d.ProcessID,
                $"{d.saddr}:{d.sport}",
                $"PID {d.ProcessID} received {d.size} bytes over UDP from {d.saddr}:{d.sport}",
                $"{d.daddr}:{d.dport}"
            );
            LogWriter.WriteAsync(entry);
        }
    }
}