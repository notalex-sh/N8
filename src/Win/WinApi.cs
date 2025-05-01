using System.Runtime.InteropServices;
using System.Text;

/*
User priv process spawning "borrowed" from:
 - https://github.com/H4NM/WhoYouCalling/blob/main/WhoYouCalling/Win32/WinAPI.cs
 - https://github.com/H4NM/WhoYouCalling/blob/554cd530feb10394538b82b2e86e01069a99c482/WhoYouCalling/Process/ProcessManager.cs#L204

Very clever thanks!
*/
namespace N8.Win
{
    public static class WinAPI
    {
        // Retrieves a handle to the shell's desktop window (typically explorer.exe).
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetShellWindow();

        // Retrieves the process ID associated with the given window handle.
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        // Opens an existing local process object.
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        // Retrieves the full name of the executable file for the process.
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool QueryFullProcessImageName(IntPtr hProcess, int dwFlags, StringBuilder lpExeName, ref int lpdwSize);

        // Closes an open object handle.
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        // Returns a pseudo handle for the current process.
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        // Retrieves the locally unique identifier (LUID) for the specified privilege.
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref LUID lpLuid);

        // Enables or disables privileges in the specified access token.
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool AdjustTokenPrivileges(
            IntPtr TokenHandle,
            bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES NewState,
            uint BufferLength,
            IntPtr PreviousState,
            IntPtr ReturnLength);

        // Opens the access token associated with a process.
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool OpenProcessToken(
            IntPtr ProcessHandle,
            uint DesiredAccess,
            out IntPtr TokenHandle);

        // Creates a new access token that duplicates an existing token.
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DuplicateTokenEx(
            IntPtr hExistingToken,
            uint dwDesiredAccess,
            IntPtr lpTokenAttributes,
            int ImpersonationLevel,
            int TokenType,
            out IntPtr phNewToken);

        // Creates a new process and its primary thread using a specified token.
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CreateProcessWithTokenW(
            IntPtr hToken,
            uint dwLogonFlags,
            string? lpApplicationName,
            string lpCommandLine,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        internal static bool DuplicateTokenEx(nint hShellToken, uint tokenAccess, nint zero, int impersonationSecurity, uint assignPrimary, out nint hUserToken)
        {
            throw new NotImplementedException();
        }

        // Structures used by the API functions

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID_AND_ATTRIBUTES
        {
            public LUID Luid;
            public uint Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public uint PrivilegeCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public LUID_AND_ATTRIBUTES[] Privileges;
        }
    }
}