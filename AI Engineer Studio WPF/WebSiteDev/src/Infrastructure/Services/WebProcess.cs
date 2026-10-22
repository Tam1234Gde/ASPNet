using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

namespace AIEngineerStudio.Services;

/// <summary>One owned process tree. Output is drained concurrently and bounded.</summary>
public sealed class WebProcess : IDisposable
{
    Process? process;
    readonly object gate = new();
    readonly Queue<string> lines = new();
    public string Status { get; private set; } = "Остановлен";
    public bool Running => process is { HasExited: false };
    public string Log { get { lock (gate) return string.Join(Environment.NewLine, lines); } }
    void Append(string? line) { if (line is null) return; lock (gate) { lines.Enqueue(line.Length > 2000 ? line[..2000] : line); while (lines.Count > 250) lines.Dequeue(); } }
    public async Task RunAsync(string directory, string command, int seconds, Uri? readiness, CancellationToken token)
    {
        if (Running) throw new InvalidOperationException("Процесс уже запущен.");
        if (string.IsNullOrWhiteSpace(command)) throw new InvalidOperationException("Не задана команда.");
        if (readiness != null)
        {
            if (!readiness.IsLoopback || readiness.Scheme != "http") throw new InvalidOperationException("Для предпросмотра требуется локальный http URL.");
            using var probe = new TcpClient();
            try { await probe.ConnectAsync(readiness.Host, readiness.Port, token); throw new InvalidOperationException("Порт уже занят. Чужой сервер не будет остановлен."); }
            catch (SocketException) { }
        }
        process?.Dispose();
        lock (gate) lines.Clear();
        var psi = new ProcessStartInfo("cmd.exe") { WorkingDirectory = directory, UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true, RedirectStandardInput = true };
        psi.ArgumentList.Add("/d"); psi.ArgumentList.Add("/s"); psi.ArgumentList.Add("/c"); psi.ArgumentList.Add(command);
        var owned = new Process { StartInfo = psi, EnableRaisingEvents = true };
        process = owned;
        owned.OutputDataReceived += (_, e) => Append(e.Data);
        owned.ErrorDataReceived += (_, e) => Append(e.Data);
        owned.Exited += (_, _) => { if (ReferenceEquals(process, owned)) Status = $"Завершён: {owned.ExitCode}"; };
        Status = "Запуск";
        owned.Start(); owned.StandardInput.Close(); owned.BeginOutputReadLine(); owned.BeginErrorReadLine();
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(token);
        timeout.CancelAfter(TimeSpan.FromSeconds(Math.Clamp(seconds, 1, 3600)));
        try
        {
            if (readiness == null)
            {
                await owned.WaitForExitAsync(timeout.Token);
                if (owned.ExitCode != 0) throw new InvalidOperationException($"Команда завершилась с кодом {owned.ExitCode}. См. журнал.");
                Status = "Команда завершена (0)";
                return;
            }
            using var http = new HttpClient(new HttpClientHandler { UseProxy = false }) { Timeout = TimeSpan.FromSeconds(2) };
            while (true)
            {
                timeout.Token.ThrowIfCancellationRequested();
                if (owned.HasExited) throw new InvalidOperationException($"Сервер завершился до готовности: {owned.ExitCode}.");
                try
                {
                    using var result = await http.GetAsync(readiness, HttpCompletionOption.ResponseHeadersRead, timeout.Token);
                    if (result.IsSuccessStatusCode && !owned.HasExited) { Status = "Сервер отвечает HTTP. Функциональность сайта не проверена."; return; }
                }
                catch (HttpRequestException) { }
                catch (TaskCanceledException) when (!timeout.IsCancellationRequested) { }
                await Task.Delay(250, timeout.Token);
            }
        }
        catch { Stop(); Status = token.IsCancellationRequested ? "Отменено" : "Ошибка/таймаут — см. журнал"; throw; }
    }
    public void Stop()
    {
        try { if (process is { HasExited: false }) process.Kill(entireProcessTree: true); }
        catch (InvalidOperationException) { }
        Status = "Остановлен";
    }
    public void Dispose() { Stop(); process?.Dispose(); }
}
