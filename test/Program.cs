using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // If we were launched with the "child" flag, run child logic:
        if (args.Length > 0 && args[0].Equals("child", StringComparison.OrdinalIgnoreCase))
        {
            RunAsChild();
        }
        else
        {
            RunAsParent();
        }
    }

    static void RunAsParent()
    {
        Console.WriteLine("Parent: launching child process...");

        // Path to our own EXE
        var exePath = Process.GetCurrentProcess().MainModule!.FileName!;

        // Start a new instance of ourselves with the "child" arg
        var startInfo = new ProcessStartInfo(exePath, "child")
        {
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false
        };

        using var child = Process.Start(startInfo);
        if (child == null)
        {
            Console.WriteLine("Parent: failed to start child.");
            return;
        }

        // Wait until the child exits
        child.WaitForExit();
        Console.WriteLine("Parent: child has exited. Check your Downloads folder.");
    }

    static void RunAsChild()
    {
        Console.WriteLine("Child: started. Press Enter to curl the site…");
        Console.ReadLine();

        // Prepare the ProcessStartInfo
        var psi = new ProcessStartInfo("curl", "https://example.com")
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        var output = new StringWriter();
        var error = new StringWriter();

        using (var proc = new Process { StartInfo = psi, EnableRaisingEvents = true })
        {
            // Hook up the async callbacks
            proc.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null) output.WriteLine(e.Data);
            };
            proc.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null) error.WriteLine(e.Data);
            };

            // Start process *before* beginning read
            proc.Start();

            // Tell .NET to begin pumping those streams
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            // Wait for curl to exit AND for the async readers to drain
            proc.WaitForExit();
            // There's an overload that ensures async handlers are flushed:
            proc.WaitForExit(5000);  // optional timeout for safety

            // Now you can access full buffers
            string html = output.ToString();
            string stderr = error.ToString();

            // Print out the first 200 chars for brevity
            Console.WriteLine(html.Length > 200
                ? html.Substring(0, 200) + "…"
                : html);

            if (!string.IsNullOrEmpty(stderr))
            {
                Console.WriteLine("curl stderr:");
                Console.WriteLine(stderr);
            }

            Console.WriteLine($"Child: curl exited with code {proc.ExitCode}");
        }

        // 2) Continue with the original file-write logic
        string downloads = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads"
        );
        string outPath = Path.Combine(downloads, "child_output.txt");
        File.WriteAllText(outPath,
            $"Child ran at {DateTime.Now:O}");
        Console.WriteLine($"Child: wrote file to {outPath}");
    }

}
