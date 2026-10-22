namespace AIEngineerStudio.Models;

public enum RemoteOperatingSystem
{
    Linux,
    Windows
}

public enum WindowsAppManager
{
    NotConfigured,
    WindowsService,
    IisApplicationPool
}

public enum RemoteAuthentication
{
    PrivateKey,
    Password
}

public sealed class RemoteProject
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public WindowsAppManager AppManager { get; set; } = WindowsAppManager.NotConfigured;
    public string ApplicationName { get; set; } = "";
    public string StatePath { get; set; } = "";
    public string DatabaseExclusions { get; set; } = "data;App_Data;*.db;*.sqlite;*.sqlite3;*.mdf;*.ldf;*.ndf;*-wal;*-shm";
    public bool DatabaseExclusionsReviewed { get; set; }
    public int ConnectionTimeoutSeconds { get; set; } = 30;
    public int OperationTimeoutSeconds { get; set; } = 120;
    public string Name { get; set; } = "Сервер";
    public string Host { get; set; } = "";
    public int Port { get; set; } = 22;
    public string User { get; set; } = "";
    public string RemotePath { get; set; } = "";
    public RemoteOperatingSystem OperatingSystem { get; set; } = RemoteOperatingSystem.Linux;
    public RemoteAuthentication Authentication { get; set; } = RemoteAuthentication.PrivateKey;
    public string PrivateKeyPath { get; set; } = "";
    public string Password { get; set; } = "";
    public string HostFingerprint { get; set; } = "";
    public string DeployCommand { get; set; } = "";
    public string ExcludePatterns { get; set; } = ".git;.svn;.hg;bin;obj;artifacts;.vs;.idea;*.user;*.suo;node_modules";
    public bool AutoSync { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
