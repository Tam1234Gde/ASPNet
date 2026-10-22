using System.IO;
using System.Text;
namespace AIEngineerStudio.Services;

public sealed class ProjectFileService
{
    private static readonly string[] IgnoredDirectories = [".git", "bin", "obj", ".vs", "node_modules", "__pycache__"];
    public IEnumerable<string> EnumerateFiles(string root, string filter = "")
    {
        if (!Directory.Exists(root)) yield break;
        IEnumerable<string> files;
        try { files = Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories); } catch { yield break; }
        foreach (var file in files.Take(1000))
        {
            if (IgnoredDirectories.Any(x => file.Contains(Path.DirectorySeparatorChar + x + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))) continue;
            var relative = Path.GetRelativePath(root, file);
            if (string.IsNullOrWhiteSpace(filter) || relative.Contains(filter, StringComparison.OrdinalIgnoreCase)) yield return file;
        }
    }
    public string ReadContext(IEnumerable<string> files, string root, int maxFiles = 15, int maxCharsPerFile = 12000)
    {
        var output = new StringBuilder();
        foreach (var file in files.Take(maxFiles))
        {
            try { var text = File.ReadAllText(file); output.AppendLine($"\n--- {Path.GetRelativePath(root, file)} ---\n{text[..Math.Min(text.Length, maxCharsPerFile)]}"); } catch { }
        }
        return output.ToString();
    }
}
