using System.IO;
using System.Text.Json;
using AISoftCraftDuck.Models;
namespace AISoftCraftDuck.Services;
public interface IEnvService { DuckSettings Current { get; } Task SaveAsync(DuckSettings settings, CancellationToken ct = default); Task SyncDeveloperConfigsAsync(DuckSettings settings, CancellationToken ct = default); }
public sealed class EnvService : IEnvService
{
    private DuckSettings _current; public DuckSettings Current => _current; public EnvService() => _current = Read();
    private static DuckSettings Read()
    {
        var settings = new DuckSettings("https://apiduck.sytes.net/v1", null, null, null, null); var path = Path.Combine(Directory.GetCurrentDirectory(), ".env"); if (!File.Exists(path)) return settings;
        try { string? baseUrl = null, apiKey = null, openAiKey = null, vcs = null; bool local = true;
            foreach (var line in File.ReadLines(path)) { var p = line.Trim().Split('=', 2); if (p.Length != 2 || p[0].StartsWith('#')) continue; var v = p[1].Trim().Trim('"'); switch (p[0].Trim()) { case "ANTHROPIC_BASE_URL": case "APIDUCK_BASE_URL": baseUrl = v; break; case "ANTHROPIC_AUTH_TOKEN": case "CVC_API_KEY": apiKey = v; break; case "OPENAI_API_KEY": openAiKey = v; break; case "VERSION_CONTROL": vcs = v; break; case "VERSION_CONTROL_LOCAL": bool.TryParse(v, out local); break; } }
            return new DuckSettings(baseUrl ?? settings.BaseUrl, apiKey, apiKey, openAiKey, null, vcs ?? "None", local);
        } catch { return settings; }
    }
    public async Task SaveAsync(DuckSettings s, CancellationToken ct = default)
    {
        _current = s with { ApiKey = NormalizeKey(s.ApiKey), OpenAiKey = NormalizeKey(s.OpenAiKey) }; s = _current; var content = $"ANTHROPIC_BASE_URL=\"{s.BaseUrl}\"\nANTHROPIC_AUTH_TOKEN=\"{s.ApiKey ?? ""}\"\nANTHROPIC_MODEL=\"claude-sonnet-4-6\"\nOPENAI_API_KEY=\"{s.OpenAiKey ?? ""}\"\nVERSION_CONTROL=\"{s.VersionControl}\"\nVERSION_CONTROL_LOCAL=\"{s.VersionControlLocal}\"\n";
        await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), ".env"), content, ct); Environment.SetEnvironmentVariable("APIDUCK_BASE_URL", s.BaseUrl); Environment.SetEnvironmentVariable("CVC_API_KEY", s.ApiKey); Environment.SetEnvironmentVariable("ANTHROPIC_AUTH_TOKEN", s.ApiKey); Environment.SetEnvironmentVariable("OPENAI_API_KEY", s.OpenAiKey); await SyncDeveloperConfigsAsync(s, ct);
    }
    private static string? NormalizeKey(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var key = value.Trim().Trim('"', '\'');
        if (key.StartsWith("Authorization:", StringComparison.OrdinalIgnoreCase)) key = key["Authorization:".Length..].Trim();
        return key.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? key[7..].Trim() : key;
    }
    public async Task SyncDeveloperConfigsAsync(DuckSettings s, CancellationToken ct = default)
    {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); var local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); var json = JsonSerializer.Serialize(new { base_url = s.BaseUrl, api_key = s.ApiKey }, new JsonSerializerOptions { WriteIndented = true });
        var files = new Dictionary<string,string> { [Path.Combine(home,".claude","settings.json")]=json, [Path.Combine(home,".codex","config.toml")]=$"base_url = \"{s.BaseUrl}\"\napi_key = \"{s.OpenAiKey ?? s.ApiKey}\"\n", [Path.Combine(local,"hermes","config.yaml")]=$"base_url: {s.BaseUrl}\napi_key: {s.ApiKey}\n" };
        foreach (var item in files) try { Directory.CreateDirectory(Path.GetDirectoryName(item.Key)!); await File.WriteAllTextAsync(item.Key,item.Value,ct); } catch { }
    }
}
