using System.Diagnostics;
using System.Text.Json;

namespace WebStudioApi.Services;

public sealed class BridgeService(IConfiguration config)
{
    public async Task<string> AskAsync(string projectPath, string prompt, string? model, CancellationToken ct)
    {
        var python = config["PythonPath"] ?? "python3";
        var bridge = Path.GetFullPath(config["BridgePath"] ?? Path.Combine(AppContext.BaseDirectory, "neural_bridge.py"));
        if (!File.Exists(bridge)) throw new FileNotFoundException("AI bridge не найден.", bridge);
        var request = new { api_key = config["ApiKey"], base_url = config["BaseUrl"], model = model ?? config["Model"], system_prompt = "Работай только с контекстом проекта. Не раскрывай секреты.", context = projectPath, history = new[] { new { role = "user", content = prompt } } };
        var psi = new ProcessStartInfo(python, $"\"{bridge}\"") { WorkingDirectory = projectPath, UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true, CreateNoWindow = true };
        using var p = Process.Start(psi) ?? throw new InvalidOperationException("Не удалось запустить bridge.");
        await p.StandardInput.WriteAsync(JsonSerializer.Serialize(request)); p.StandardInput.Close();
        var output = await p.StandardOutput.ReadToEndAsync(ct); await p.WaitForExitAsync(ct);
        using var doc = JsonDocument.Parse(output); var root = doc.RootElement;
        if (root.TryGetProperty("ok", out var ok) && !ok.GetBoolean()) throw new InvalidOperationException(root.GetProperty("error").GetString());
        return root.TryGetProperty("content", out var content) ? content.GetString() ?? "Пустой ответ." : output;
    }
}
