using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace AIEngineerStudio.Services;
/// <summary>Cooperating Studio editions hold a lease while a workspace is open. No workspace files are changed.</summary>
public sealed class WorkspaceLease : IDisposable
{
    readonly FileStream stream;
    WorkspaceLease(FileStream stream) => this.stream = stream;
    public static WorkspaceLease Acquire(string directory)
    {
        string canonical = Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory)).ToUpperInvariant();
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(canonical)));
        var root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CORETECH", "WorkspaceLocks");
        Directory.CreateDirectory(root);
        try { return new WorkspaceLease(new FileStream(Path.Combine(root, hash + ".lock"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)); }
        catch (IOException ex) { throw new IOException("Рабочая папка уже открыта в другом экземпляре Studio. Закройте её там и повторите.", ex); }
    }
    public void Dispose() => stream.Dispose();
}
