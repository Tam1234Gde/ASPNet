using System.IO;

namespace AIEngineerStudio.Services;

// No recursive OS enumeration: inaccessible folders and junctions cannot abort the tree.
public sealed class ExplorerEntry
{
    public string Path { get; init; } = "";
    public bool IsDirectory { get; init; }
    public bool IsLink { get; init; }
    public string? Error { get; init; }
    public List<ExplorerEntry>? Children { get; set; }
}

public static class ExplorerFileService
{
    static readonly HashSet<string> Generated = new(StringComparer.OrdinalIgnoreCase)
        { ".git", ".vs", "bin", "obj", "node_modules", "__pycache__", ".venv", "artifacts" };

    public static List<ExplorerEntry> Read(string directory, bool showGenerated, CancellationToken token)
    {
        List<ExplorerEntry> entries = new();
        try
        {
            foreach (string path in Directory.EnumerateFileSystemEntries(directory))
            {
                token.ThrowIfCancellationRequested();
                try
                {
                    FileAttributes attributes = File.GetAttributes(path);
                    bool folder = attributes.HasFlag(FileAttributes.Directory);
                    if (!showGenerated && folder && Generated.Contains(System.IO.Path.GetFileName(path))) continue;
                    entries.Add(new ExplorerEntry { Path = path, IsDirectory = folder,
                        IsLink = attributes.HasFlag(FileAttributes.ReparsePoint) });
                }
                catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
                { entries.Add(new ExplorerEntry { Path = path, Error = exception.Message }); }
            }
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        { entries.Add(new ExplorerEntry { Path = directory, Error = exception.Message }); }
        return entries.OrderByDescending(entry => entry.IsDirectory)
            .ThenBy(entry => System.IO.Path.GetFileName(entry.Path), StringComparer.OrdinalIgnoreCase).ToList();
    }

    public static List<ExplorerEntry> Search(string directory, string root, string query, bool showGenerated, CancellationToken token)
    {
        List<ExplorerEntry> result = new();
        foreach (ExplorerEntry entry in Read(directory, showGenerated, token))
        {
            token.ThrowIfCancellationRequested();
            bool matches = System.IO.Path.GetRelativePath(root, entry.Path).Contains(query, StringComparison.OrdinalIgnoreCase);
            if (entry.IsDirectory && !entry.IsLink && entry.Error is null)
            {
                // Matching folders stay lazy; non-matching ancestors contain only matching descendants.
                if (!matches) entry.Children = Search(entry.Path, root, query, showGenerated, token);
                if (matches || entry.Children?.Count > 0) result.Add(entry);
            }
            else if (matches || entry.Error is not null) result.Add(entry);
        }
        return result;
    }
}
