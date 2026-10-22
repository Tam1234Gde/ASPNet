using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using AIEngineerStudio.Models;
namespace AIEngineerStudio.Services;

public sealed class NeuralBridgeService
{
    public async Task<string> AskAsync(string pythonPath, string bridgePath, NeuralRequest request, CancellationToken cancellationToken)
    {
        if (!File.Exists(bridgePath)) throw new FileNotFoundException("Не найден neural_bridge.py", bridgePath);
        var psi = new ProcessStartInfo(pythonPath, $"\"{bridgePath}\"") { RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true, StandardInputEncoding = new UTF8Encoding(false), StandardOutputEncoding = new UTF8Encoding(false), StandardErrorEncoding = new UTF8Encoding(false) };
        using var process = Process.Start(psi) ?? throw new InvalidOperationException("Не удалось запустить Python bridge.");
        await process.StandardInput.WriteAsync(JsonSerializer.Serialize(new { api_key=request.ApiKey, base_url=request.BaseUrl, model=request.Model, temperature=request.Temperature, max_tokens=request.MaxTokens, system_prompt=request.SystemPrompt, context=request.Context, history=request.History.Select(x => new { role=x.Role, content=x.Content }) }));
        process.StandardInput.Close();
        using var registration = cancellationToken.Register(() => { try { if (!process.HasExited) process.Kill(true); } catch { } });
        var output = await process.StandardOutput.ReadToEndAsync(cancellationToken); var error = await process.StandardError.ReadToEndAsync(cancellationToken); await process.WaitForExitAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(output)) throw new InvalidOperationException(string.IsNullOrWhiteSpace(error) ? "Bridge не вернул ответ." : error);
        using var doc = JsonDocument.Parse(output);
        if (!doc.RootElement.GetProperty("ok").GetBoolean()) throw new InvalidOperationException(doc.RootElement.GetProperty("error").GetString() ?? "Ошибка нейросети.");
        return doc.RootElement.GetProperty("content").GetString() ?? "Пустой ответ модели.";
    }
}
