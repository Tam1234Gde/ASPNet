using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Principal;
using System.Text;

namespace WIS.MasRunner;

public static class MasRunner
{
    // Прямой URL к актуальному чистому исходнику MAS_AIO.cmd (без HTML-оберток)
    private const string MasScriptUrl = "https://raw.githubusercontent.com/massgravel/Microsoft-Activation-Scripts/master/MAS/All-In-One-Version/MAS_AIO.cmd";

    public static MasResult ActivateOfficeOhook(TimeSpan? timeout = null)
    {
        timeout ??= TimeSpan.FromMinutes(10);

        if (!IsAdministrator())
            return RunElevatedSelf();

        return RunOhookDirect(timeout.Value);
    }

    private static MasResult RunOhookDirect(TimeSpan timeout)
    {
        string tempBat = Path.Combine(Path.GetTempPath(), $"mas_{Guid.NewGuid():N}.cmd");

        try
        {
            // 1. Скачиваем чистый cmd-скрипт без HTML
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("curl/8.0.0");

                string scriptContent = httpClient.GetStringAsync(MasScriptUrl)
                    .GetAwaiter().GetResult();

                // Проверка: если вернулся HTML вместо скрипта, выбрасываем ошибку
                if (scriptContent.TrimStart().StartsWith("<") || scriptContent.Contains("<!DOCTYPE html>"))
                {
                    return MasResult.Fail("Получен некорректный ответ (HTML вместо скрипта MAS). Проверьте интернет-соединение.");
                }

                File.WriteAllText(tempBat, scriptContent, Encoding.ASCII);
            }

            // Настройка кодировки: UTF-8 с fallback на системную
            Encoding encoding = Encoding.UTF8;
            try
            {
                encoding = Encoding.GetEncoding(866);
            }
            catch
            {
                try { encoding = Encoding.GetEncoding(Console.OutputEncoding.CodePage); } catch { }
            }

            // 2. Запуск cmd.exe полностью в фоне
            // Флаг /c выполняет команду и завершает процесс, CreateNoWindow отключает conhost
            ProcessStartInfo psi = new()
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"\"{tempBat}\" /Ohook\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = encoding,
                StandardErrorEncoding = encoding
            };

            using Process process = new() { StartInfo = psi };
            StringBuilder stdout = new();
            StringBuilder stderr = new();

            process.OutputDataReceived += (_, e) => { if (e.Data != null) stdout.AppendLine(e.Data); };
            process.ErrorDataReceived += (_, e) => { if (e.Data != null) stderr.AppendLine(e.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (!process.WaitForExit((int)timeout.TotalMilliseconds))
            {
                try { process.Kill(entireProcessTree: true); } catch { }
                return new MasResult(false, -1, stdout.ToString(), "Timeout exceeded.");
            }

            process.WaitForExit();

            return new MasResult(
                process.ExitCode == 0,
                process.ExitCode,
                stdout.ToString(),
                stderr.ToString());
        }
        catch (Exception ex)
        {
            return MasResult.Fail(ex.Message);
        }
        finally
        {
            if (File.Exists(tempBat))
            {
                try { File.Delete(tempBat); } catch { }
            }
        }
    }

    private static MasResult RunElevatedSelf()
    {
        ProcessStartInfo psi = new()
        {
            FileName = Environment.ProcessPath ?? Process.GetCurrentProcess().MainModule?.FileName ?? "dotnet",
            UseShellExecute = true,
            Verb = "runas",
            WindowStyle = ProcessWindowStyle.Hidden
        };

        try
        {
            using var proc = Process.Start(psi);
            proc?.WaitForExit();
            return new MasResult(proc?.ExitCode == 0, proc?.ExitCode ?? -1, string.Empty, string.Empty);
        }
        catch (Win32Exception ex) when (ex.NativeErrorCode == 1223)
        {
            return MasResult.Fail("Запрос UAC был отменен пользователем.");
        }
        catch (Exception ex)
        {
            return MasResult.Fail(ex.Message);
        }
    }

    private static bool IsAdministrator()
    {
        using WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}