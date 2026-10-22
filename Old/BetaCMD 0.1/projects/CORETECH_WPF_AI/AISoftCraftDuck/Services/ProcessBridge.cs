using System.Diagnostics;
namespace AISoftCraftDuck.Services;
public interface IProcessBridge { Task<int> RunAsync(string fileName, string arguments, string workingDirectory, Action<string, bool> output, CancellationToken ct = default); }
public sealed class ProcessBridge : IProcessBridge
{
 public async Task<int> RunAsync(string fileName, string arguments, string workingDirectory, Action<string, bool> output, CancellationToken ct = default)
 {
  using var p = new Process { StartInfo = new ProcessStartInfo(fileName, arguments) { WorkingDirectory = workingDirectory, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true } };
  p.OutputDataReceived += (_, e) => { if (e.Data is not null) output(e.Data, false); }; p.ErrorDataReceived += (_, e) => { if (e.Data is not null) output(e.Data, true); };
  p.Start(); p.BeginOutputReadLine(); p.BeginErrorReadLine(); await p.WaitForExitAsync(ct); return p.ExitCode;
 }
}