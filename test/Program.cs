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

        // Get the path to our own EXE
        var exePath = Process.GetCurrentProcess().MainModule!.FileName!;

        // Start a new instance of ourselves with the "child" arg
        var startInfo = new ProcessStartInfo(exePath, "child")
        {
            UseShellExecute = false,
            // inherit the same console window so child can ReadLine() here
            RedirectStandardOutput = false,
            RedirectStandardError  = false
        };

        using var child = Process.Start(startInfo);

        if (child == null)
        {
            Console.WriteLine("Parent: failed to start child.");
            return;
        }

        // Wait until the child exits (i.e. after you press Enter in it)
        child.WaitForExit();

        Console.WriteLine("Parent: child has exited. Check your Downloads folder.");
    }

    static void RunAsChild()
    {
        Console.WriteLine("Child: started. Press Enter to continue…");
        Console.ReadLine();

        // Determine the Downloads folder (Windows 8+ or fallback)
        string downloads =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads"
        );
        if (string.IsNullOrEmpty(downloads))
            downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

        // Write the output file
        string outPath = Path.Combine(downloads, "child_output.txt");
        File.WriteAllText(outPath,
            $"Child ran at {DateTime.Now:O}");

        Console.WriteLine($"Child: wrote file to {outPath}");
    }
}
