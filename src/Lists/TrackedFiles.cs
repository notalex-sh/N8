using N8.Classes;
using System.Collections.Generic;

namespace N8.Lists
{
    // Track files written to by monitored processes
    public static class TrackedFiles
    {
        public static List<FileNode> FileList { get; } = new List<FileNode>();

        public static void Add(FileNode newFile)
        {
            FileList.Add(newFile);
        }

    }
}