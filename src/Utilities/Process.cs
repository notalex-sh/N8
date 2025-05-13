using System.Runtime.InteropServices;
using N8.Win;
using N8.Constants;

namespace N8.Utilities
{
    public static class ProcessUtilities
    {
        // Helper to enable the SeIncreaseQuotaPrivilege
        private static void EnableSeIncreaseQuotaPrivilege()
        {
            IntPtr hProcessToken = IntPtr.Zero;
            try
            {
                IntPtr currentProcess = WinAPI.GetCurrentProcess();
                if (!WinAPI.OpenProcessToken(currentProcess, TokenPrivileges.AdjustPrivileges, out hProcessToken))
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Exception($"Failed to lookup privilege value for current process, error code: {errorCode}");
                }

                WinAPI.TOKEN_PRIVILEGES tkp = new WinAPI.TOKEN_PRIVILEGES
                {
                    PrivilegeCount = 1,
                    Privileges = new WinAPI.LUID_AND_ATTRIBUTES[1]
                };

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type blah blah.
                if (!WinAPI.LookupPrivilegeValue(null, "SeIncreaseQuotaPrivilege", ref tkp.Privileges[0].Luid))
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Exception($"Failed to lookup privilege value, error code: {errorCode}");
                }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                tkp.Privileges[0].Attributes = TokenPrivileges.SePrivilegeEnabled;

                if (!WinAPI.AdjustTokenPrivileges(hProcessToken, false, ref tkp, 0, IntPtr.Zero, IntPtr.Zero))
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Exception($"Failed to adjust token privileges, error code: {errorCode}");
                }
            }
            finally
            {
                if (hProcessToken != IntPtr.Zero)
                    WinAPI.CloseHandle(hProcessToken);
            }
        }

        // spawn target proc with user priviledge
        public static int StartProcess(string FilePath)
        {
            EnableSeIncreaseQuotaPrivilege();
            IntPtr shellWnd = WinAPI.GetShellWindow();
            if (shellWnd == IntPtr.Zero)
                throw new Exception("Shell window not found.");

            WinAPI.GetWindowThreadProcessId(shellWnd, out uint shellPid);

            IntPtr hShellProcess = WinAPI.OpenProcess(SecurityFlags.QueryInformation, false, shellPid);
            if (hShellProcess == IntPtr.Zero)
                throw new Exception("Failed to open shell process.");

            if (!WinAPI.OpenProcessToken(hShellProcess, TokenPrivileges.Duplicate, out IntPtr hShellToken))
                throw new Exception("Failed to open process token.");

            uint tokenAccess = TokenPrivileges.Query |
                               TokenPrivileges.AssignPrimary |
                               TokenPrivileges.Duplicate |
                               TokenPrivileges.AdjustDefault |
                               TokenPrivileges.AdjustSessionID;

            if (!WinAPI.DuplicateTokenEx(
                    hShellToken,
                    tokenAccess,
                    IntPtr.Zero,
                    SecurityFlags.ImpersonationSecurity,
                    TokenPrivileges.AssignPrimaryToken,
                    out IntPtr hUserToken))
            {
                throw new Exception("Failed to duplicate token.");
            }


            WinAPI.STARTUPINFO startupInfo = new WinAPI.STARTUPINFO();
            startupInfo.cb = Marshal.SizeOf(startupInfo);
            WinAPI.PROCESS_INFORMATION processInfo;

            string rootPath = FilePath;
            string commandLine = $"{rootPath}";

            // Determine the working directory.
            string workingDirectory = Path.GetDirectoryName(rootPath) ?? Directory.GetCurrentDirectory();

            if (!WinAPI.CreateProcessWithTokenW(
                    hUserToken,
                    SecurityFlags.LogonFlags,
                    null,
                    commandLine,
                    SecurityFlags.CreationFlags,
                    IntPtr.Zero,
                    workingDirectory,
                    ref startupInfo,
                    out processInfo))
            {
                throw new Exception("Failed to create process with user token.");
            }

            WinAPI.CloseHandle(hUserToken);
            WinAPI.CloseHandle(hShellToken);
            WinAPI.CloseHandle(hShellProcess);

            // return pid to add to proc list and monitor
            return (int)processInfo.dwProcessId;
        }
    }
}
