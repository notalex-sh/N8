using System.Diagnostics.Tracing;
using System.Threading.Channels;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;
using N8.Classes;
using N8.Lists;
using N8.Utilities;

/*
Event types

ProcessStart
ProcessStop
*/

namespace N8.ETW
{
    public partial class ETWMonitor
    {
        private ChannelWriter<LogEntry> LogWriter;
        private readonly TraceEventSession ETWSession;
        public ETWMonitor(ChannelWriter<LogEntry> logWriter)
        {
            LogWriter = logWriter;

            // Creete kernel sessions
            ETWSession = new TraceEventSession("N8Session") { StopOnDispose = true };
            ETWSession.EnableKernelProvider(
                KernelTraceEventParser.Keywords.Process
                | KernelTraceEventParser.Keywords.FileIOInit
                | KernelTraceEventParser.Keywords.FileIO
                | KernelTraceEventParser.Keywords.ImageLoad
                | KernelTraceEventParser.Keywords.Registry
                | KernelTraceEventParser.Keywords.NetworkTCPIP
            );
            // hook up sessions to processors

            // proc
            ETWSession.Source.Kernel.ProcessStart += OnProcessStart;
            ETWSession.Source.Kernel.ProcessStop += OnProcessStop;
            ETWSession.Source.Kernel.FileIOWrite += OnFileWrite;

            ETWSession.Source.Kernel.ImageLoad += OnImageLoad;
            ETWSession.Source.Kernel.TcpIpConnect += OnTcpConnect;
            ETWSession.Source.Kernel.TcpIpSend += OnTcpSend;
            ETWSession.Source.Kernel.TcpIpRecv += OnTcpRecv;
            ETWSession.Source.Kernel.UdpIpSend += OnUdpSend;
            ETWSession.Source.Kernel.UdpIpRecv += OnUdpRecv;
        }

        // start monitoring and dispose functions
        public Task StartMonitoring()
        {
            return Task.Run(() =>
            {
                try
                {
                ETWSession.Source.Process();    
                }
                finally
                {
                    LogWriter.Complete();
                }
            });

        }

        private void StopMonitoring()
        {
            ETWSession.Dispose();
        }

    }
}