using CommandLine;

// kind of

namespace N8.Utilities
{
    // Parse command line arguments w CommandLine
    public class Options
    {
        [Option("target", Required = true, HelpText = "Path to executable being examined.")]
        public string? Target { get; set; }

        [Option("verbose", Required = false, HelpText = "Enable verbose output.")]
        public bool Verbose { get; set; } = false;
    }

    public static class ArgumentParser
    {
        public static Options? ParsedOptions { get; private set; }

        public static bool ParseArguments(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opts =>
                {
                    ParsedOptions = opts;
                })
                .WithNotParsed(errs =>
                {
                    foreach (var error in errs)
                    {
                        Console.WriteLine(error.ToString());
                    }
                });

            return result.Tag == ParserResultType.Parsed;
        }
    }

    public class Spinner(int left, int top, string[] sequence)
    {
        // fun spinner class for while stuffs goin on
        private int Left = left;
        private int Top = top;
        private int Delay = 100;
        private string[] Sequence = sequence;
        private int Current;
        private bool Active = false;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private Thread SpinnerThread;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        private void SpinLoop()
        {
            while (Active)
            {
                Console.SetCursorPosition(Left, Top);
                Output.WriteColor(Sequence[Current], ConsoleColor.Yellow);
                Current = (Current + 1) % Sequence.Length;
                Thread.Sleep(Delay);
            }
        }

        public void Start()
        {
            if (Active) return;
            Active = true;
            SpinnerThread = new Thread(SpinLoop)
            {
                IsBackground = true
            };
            SpinnerThread.Start();
        }

        public void Stop()
        {
            if (!Active) return;
            Active = false;
            SpinnerThread.Join();
            Console.SetCursorPosition(Left, Top);
            Output.WriteColor("[TARGET EXITED]", ConsoleColor.Yellow);
        }
    }

}