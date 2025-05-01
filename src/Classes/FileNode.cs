using N8.Utilities;

namespace N8.Classes
{
    // Files written to by processes
    // This is to detect if any of them are started by any processes, to try to get around evasion methods
    // e.g. PPID spoofing, or even if just respawning with admin perms PPID will not be related to any tracked PIDs so will look in FileNode list as well
    public class FileNode
    {
        public string? path { get; set; }
        public string? name { get; set; }

        public ProcessNode? createdBy { get; set; }
        
        public FileNode(string filePath, ProcessNode? parent)
        {
            if (!File.Exists(filePath))
                Generic.ExitError("File does not exist.");

            path = Path.GetFullPath(filePath);
            name = Path.GetFileName(filePath);
            createdBy = parent;
        }

    }
}
