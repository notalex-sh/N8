using System.Collections.Concurrent;
using System.IO;

namespace N8.Lists
{
    // track files written to by monitored processes
    public static class TrackedFiles
    {
        public static ConcurrentDictionary<string, byte> PathList { get; set; } = new ConcurrentDictionary<string, byte>();

        public static void Add(string FilePath)
        {
            PathList[FilePath] = 0;
        }

        public static string GetFileName(string FilePath)
        {
            return GetFileName(FilePath);
        }

    }
}