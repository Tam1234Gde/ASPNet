namespace WIS.MasRunner;

/// <summary>
/// Результат выполнения команды MAS в PowerShell.
/// </summary>
public sealed record MasResult(
    bool Success,
    int ExitCode,
    string StandardOutput,
    string StandardError)
{
    public static MasResult Fail(string error) =>
        new(false, -1, string.Empty, error);
}