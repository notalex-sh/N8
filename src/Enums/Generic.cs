namespace N8.Enums
{
    public static class Generic
    {
        public enum TokenIntegrityLevel
        {
            // integirty levels for different processes, minimum of file and user 
            Unknown = 0,
            Low = 0x1000,
            Medium = 0x2000,
            High = 0x3000,
            System = 0x4000
        }

        public enum EventTypes
        {
            // event types to be processed
            ProcessStart,
            ProcessStop,
            FileWrite,
            FileRead,
            FileDelete,
            ImageLoad,
            TcpConnect,
            TcpSend,
            TcpRecv,
            UdpSend,
            UdpRecv,
            RegistryCreate,
            RegistryOpen,
            RegistryDelete,
            RegistrySetValue,
            RegistryDeleteValue,
            DnsQuery,
            DnsResponse,
            DriverLoad,
            ServiceInstall,
            ScheduledTaskRegister,
            PipeCreate,
            WmiQuery
        }
    }
}