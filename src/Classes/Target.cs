using System.Diagnostics;
using System.Security.Cryptography;

using N8.Utilities;

namespace N8.Classes
{
    // the target executable to be analysed
    public class Target
    {
        public string? path { get; set; }
        public string? name { get; set; }
        public string? hash { get; set; }
        public string? size { get; set; }
        public string? modified { get; set; }
        public string? company { get; set; }

        public Target(string filePath)
        {
            if (!File.Exists(filePath))
                Generic.ExitError("Target file does not exist.");

            path = Path.GetFullPath(filePath);
            name = Path.GetFileName(filePath);
            hash = ComputeSHA256(filePath);

            var fileInfo = new FileInfo(filePath);
            size = fileInfo.Length.ToString();
            modified = fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");

            var versionInfo = FileVersionInfo.GetVersionInfo(filePath);
            company = versionInfo.CompanyName ?? "Unknown";
        }

        private string ComputeSHA256(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
