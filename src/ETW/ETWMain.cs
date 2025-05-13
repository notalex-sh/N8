using System.Diagnostics.Tracing;
using System.Threading.Channels;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;
using N8.Classes;
using N8.Lists;
using N8.Utilities;
using N8.Constants;
using Microsoft.Diagnostics.Tracing;

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
            ETWSession.EnableProvider(
                "Microsoft-Windows-DNS-Client"
            );

            // hook up sessions to processors

            // proc
            ETWSession.Source.Kernel.ProcessStart += OnProcessStart;
            ETWSession.Source.Kernel.ProcessStop += OnProcessStop;

            // filesystem
            ETWSession.Source.Kernel.FileIOWrite += OnFileWrite;
            ETWSession.Source.Kernel.FileIORead += OnFileRead;
            ETWSession.Source.Kernel.FileIOFileDelete += OnFileDelete;

            // images
            ETWSession.Source.Kernel.ImageLoad += OnImageLoad;

            // registry

            ETWSession.Source.Kernel.RegistryCreate += OnRegCreate;
            ETWSession.Source.Kernel.RegistryOpen += OnRegOpen;
            ETWSession.Source.Kernel.RegistryDelete += OnRegDelete;
            ETWSession.Source.Kernel.RegistrySetValue += OnRegSetValue;
            ETWSession.Source.Kernel.RegistryDeleteValue += OnRegDeleteValue;
            // tcp/udp interactiona
            ETWSession.Source.Kernel.TcpIpConnect += OnTcpConnect;
            ETWSession.Source.Kernel.TcpIpSend += OnTcpSend;
            ETWSession.Source.Kernel.TcpIpRecv += OnTcpRecv;
            ETWSession.Source.Kernel.UdpIpSend += OnUdpSend;
            ETWSession.Source.Kernel.UdpIpRecv += OnUdpRecv;

            //dns
            // eventid 3006 = dns query
            ETWSession.Source.Dynamic.AddCallbackForProviderEvent(
                "Microsoft-Windows-DNS-Client",
                "EventID(3006)",
                OnDnsQuery
            );

            // user-mode providers  ➜ Service / Task / WMI
            ETWSession.EnableProvider("Microsoft-Windows-Service Control Manager");
            ETWSession.EnableProvider("Microsoft-Windows-TaskScheduler");
            ETWSession.EnableProvider("Microsoft-Windows-WMI-Activity");

            // kernel callbacks already exist; we just add two filters below
            ETWSession.Source.Kernel.ImageLoad += OnKernelImageLoad;
            ETWSession.Source.Kernel.FileIOCreate += OnPipeCreate;

            ETWSession.Source.Dynamic.AddCallbackForProviderEvent(
                "Microsoft-Windows-Service Control Manager", "7045", OnServiceInstall);

            ETWSession.Source.Dynamic.AddCallbackForProviderEvent(
                "Microsoft-Windows-TaskScheduler", "106", OnTaskRegister);

            ETWSession.Source.Dynamic.AddCallbackForProviderEvent(
                "Microsoft-Windows-WMI-Activity", "5858", OnWmiQuery);
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