using System.Diagnostics;

Console.WriteLine("Запуск CMD...");

var startInfo = new ProcessStartInfo
{
    FileName = "cmd.exe",
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    CreateNoWindow = true,
    StandardOutputEncoding = System.Text.Encoding.UTF8,
    StandardErrorEncoding = System.Text.Encoding.UTF8,
    Arguments = "/c \"echo Привет из CMD & dir\""
};

using var process = new Process { StartInfo = startInfo };
process.Start();

string output = await process.StandardOutput.ReadToEndAsync();
string error = await process.StandardError.ReadToEndAsync();
await process.WaitForExitAsync();

Console.WriteLine(output);
if (!string.IsNullOrWhiteSpace(error))
    Console.Error.WriteLine(error);

Console.WriteLine($"CMD завершён с кодом: {process.ExitCode}");
