namespace WebStudioApi.Services;

public sealed class AuditService(IWebHostEnvironment environment)
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    public async Task WriteAsync(HttpContext http, string action, string target, string result)
    {
        var dir = Path.Combine(environment.ContentRootPath, "data"); Directory.CreateDirectory(dir);
        var line = $"{DateTimeOffset.UtcNow:O}\t{http.User.Identity?.Name ?? "anonymous"}\t{action}\t{target}\t{result}{Environment.NewLine}";
        await _gate.WaitAsync(); try { await File.AppendAllTextAsync(Path.Combine(dir, "audit.log"), line); } finally { _gate.Release(); }
    }
}
