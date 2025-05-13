using System.Security.Cryptography;
using System.Security.Principal;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using N8.Constants;
using System.Net;
using N8.Analyser;

namespace N8.Utilities
{
    public static class Generic
    {
        // Confirm n8 is being run as administrator
        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        // Exit 1 with error message
        public static void ExitError(string message)
        {
            Output.WriteLineColor($"[!] ERROR: {message}", ConsoleColor.Red);
            Environment.Exit(1);
        }

        public static void Warning(string message)
        {
            Output.WriteLineColor($"[!] WARNING: {message}", ConsoleColor.Yellow);
        }

        // Gets file hash for welcome message
        public static string GetFileHash(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }

        // Print welcome message

        // TODO: add more relevant stuff
        public static void WelcomeMessage()
        {
            Console.Clear();
            Output.WriteLineColor(Messages.welcomemessage, ConsoleColor.Green);
            Output.WriteLineColor($"[*] N8 {ApplicationInformation.version}", ConsoleColor.Yellow);
            Console.WriteLine();

            Output.WriteLineColor("=== Target Information ===", ConsoleColor.Cyan);
            Output.WriteColor("   [+] Name: ", ConsoleColor.Green);
            Output.WriteLineColor(Program.Target?.name ?? "[No file]", ConsoleColor.White);
            Output.WriteColor("   [+] Path: ", ConsoleColor.Green);
            Output.WriteLineColor(Program.Target?.path ?? "-", ConsoleColor.Gray);
            Output.WriteColor("   [+] Size: ", ConsoleColor.Green);
            Output.WriteLineColor(Program.Target?.size + " bytes" ?? "-", ConsoleColor.White);
            Output.WriteColor("   [+] Modified: ", ConsoleColor.Green);
            Output.WriteLineColor(Program.Target?.modified ?? "-", ConsoleColor.White);
            Output.WriteColor("   [+] SHA256: ", ConsoleColor.Green);
            Output.WriteLineColor(Program.Target?.hash ?? "-", ConsoleColor.DarkCyan);
            Console.WriteLine();

            Output.WriteLineColor("=== Execution Configuration ===", ConsoleColor.Cyan);
            Output.WriteColor("   [+] Running as: ", ConsoleColor.Green);
            Output.WriteLineColor("Standard User", ConsoleColor.White);
            Output.WriteColor("   [+] N8 PID: ", ConsoleColor.Green);
            Output.WriteLineColor($"{Process.GetCurrentProcess().Id}", ConsoleColor.White);
            Console.WriteLine();
        }

        // ignore loopback
        public static bool IsLoopback(IPAddress addr)
        => IPAddress.IsLoopback(addr);

        // stuff for controller usage to display output from the execution
        public static void ExecutionSummary(LogAnalyser analyser)
        {
            Output.WriteLineColor("[*] Execution Summary\n", ConsoleColor.Yellow);
            Output.PrintSummary("Processes",   analyser.Processes);
            Output.PrintSummary("Images:",      analyser.Images);
            Output.PrintSummary("IPs Inbound:", analyser.IpInbound);
            Output.PrintSummary("IPs Outbound:",analyser.IpOutbound);
            Output.PrintSummary("File Writes:", analyser.FileWrites);
        }

        public static void CleanUpExecution(int top)
        {
            Output.ClearLine(top);
            Output.ClearLine(top + 2);
            Output.ClearLine(top + 3);
            Console.SetCursorPosition(0, top);
        }
    }


}
