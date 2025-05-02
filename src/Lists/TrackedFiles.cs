using N8.Classes;
using System.Collections.Generic;

namespace N8.Lists
{
    // Track files written to by monitored processes
    public static class TrackedFiles
    {
        public static Dictionary<string, FileNode> FileList { get; } = new Dictionary<string, FileNode>();
        public static HashSet<string> PathList { get; set; } = new HashSet<string>();

        public static void Add(FileNode newFile)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            PathList.Add(newFile.path);
            FileList.Add(newFile.path, newFile);
#pragma warning restore CS8604 // Possible null reference argument.
        }

    }
}