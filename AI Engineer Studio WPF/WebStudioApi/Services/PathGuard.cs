namespace WebStudioApi.Services;

public sealed class PathGuard(IConfiguration configuration)
{
    private readonly string _root = Path.GetFullPath(configuration["ProjectsRoot"] ?? Path.Combine(AppContext.BaseDirectory, "projects"));
    public string Root => _root;
    public string Resolve(string relative)
    {
        if (string.IsNullOrWhiteSpace(relative) || Path.IsPathRooted(relative)) throw new ArgumentException("Путь должен быть относительным.");
        var full = Path.GetFullPath(Path.Combine(_root, relative));
        if (!full.StartsWith(_root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) && !string.Equals(full, _root, StringComparison.OrdinalIgnoreCase)) throw new UnauthorizedAccessException("Путь выходит за пределы рабочей области.");
        return full;
    }
}
