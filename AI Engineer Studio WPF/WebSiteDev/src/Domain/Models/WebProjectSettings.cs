namespace AIEngineerStudio.Models;
public sealed class WebProjectSettings
{
    public string ConfirmedStack { get; set; } = "";
    public string Requirements { get; set; } = "";
    public string AcceptanceCriteria { get; set; } = "";
    public string InstallCommand { get; set; } = "";
    public string BuildCommand { get; set; } = "";
    public string StartCommand { get; set; } = "";
    public string Url { get; set; } = "http://127.0.0.1:5173";
    public bool ExternalPreview { get; set; } = true;
    public int CommandTimeoutSeconds { get; set; } = 300;
    public int StartupTimeoutSeconds { get; set; } = 45;
}
