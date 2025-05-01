using CommandLine;

namespace N8.Utilities
{
    // Parse command line arguments w CommandLine
    public class Options
    {
        [Option("target", Required = true, HelpText = "Path to executable being examined.")]
        public string? Target { get; set; }
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

}