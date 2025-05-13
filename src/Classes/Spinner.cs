namespace N8.Classes 
{
    public static class Spinners
    {
        private static readonly Random rnd = new Random();
        public static readonly string[][] sequences = new[]
        {
            // shoutout chatgpt for these baller spinner designs!
            new[] { "/", "-", "\\", "|" },
            new[] { ".", "o", "0", "o" },
            new[] { "+", "x", "+", "x" },
            new[] { "V", "<", "^", ">" }
        };

        public static string[] GetRandom()
        {
            return sequences[rnd.Next(sequences.Length)];
        }
    }
}