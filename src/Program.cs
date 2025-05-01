using N8.Utilities;
using N8.Classes;
using N8.Controllers;

namespace N8
{
    class Program
    {
        public static string? TargetPath { get; set; }
        public static Target? Target { get; set; }
        static void Main(string[] args)
        {
            // Stop if not admin
            if (!Generic.IsAdministrator())
            {
                Generic.ExitError("N8 requires administrator privileges.");
            }

            // Parse arguments
            if (!ArgumentParser.ParseArguments(args))
            {
                Generic.ExitError("Failed to parse command-line arguments.");
                return;
            }

            // Collect command-line input
            var options = ArgumentParser.ParsedOptions!;
            TargetPath = options.Target;

#pragma warning disable CS8604 // i dont know how this would be null tbh, if it is ill probably know about it because the program will break
            Target = new Target(TargetPath);
#pragma warning restore CS8604

            Generic.WelcomeMessage();

            // Start the main controller
            var controller = new Controller();
            controller.Start(Target);
        }
    }
}
