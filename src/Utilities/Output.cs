namespace N8.Utilities
{
    public static class Output
    {
        // cool output colouring
        public static void WriteLineColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
            Console.Write("\n");
        }

        public static void WriteColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void ClearLine(int top)
        {
            Console.SetCursorPosition(0, top);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, top);
        }

        // summary for printing execution summaries, might need to move elsewhere
        public static void PrintSummary(string title, IEnumerable<string> items)
        {
            WriteLineColor($"=== {title} ===", ConsoleColor.Cyan);
            foreach (var item in items.OrderBy(x => x))
            {
                WriteColor("   [!] ", ConsoleColor.Blue);
                WriteLineColor(item, ConsoleColor.White);
            }
            Console.WriteLine(); 
        }   
    }
}