using System.Text.Json;
using AISoftCraftDuck.Models;
using AISoftCraftDuck.ViewModels;

namespace AISoftCraftDuck.Services;

public sealed record WorkspaceProject(string Id, string Name, string Directory, DateTime UpdatedAt);
public sealed record StoredMessage(string Role, string Content, string? Model);

/// <summary>Local project registry and per-project chat persistence.</summary>
public sealed class WorkspaceService
{
    private readonly string root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CORETECH_IT_DEV", "workspaces");
    private readonly JsonSerializerOptions json = new() { WriteIndented = true };
    private readonly SemaphoreSlim fileLock = new(1, 1);
    private string IndexPath => Path.Combine(root, "projects.json");

    public WorkspaceService() => Directory.CreateDirectory(root);
    public async Task<List<WorkspaceProject>> LoadAsync() => File.Exists(IndexPath)
        ? await JsonSerializer.DeserializeAsync<List<WorkspaceProject>>(File.OpenRead(IndexPath), json) ?? [] : [];
    public async Task SaveAsync(IEnumerable<WorkspaceProject> projects)
    {
        await using var stream = File.Create(IndexPath);
        await JsonSerializer.SerializeAsync(stream, projects, json);
    }
    public async Task<List<StoredMessage>> LoadChatAsync(string id)
    {
        var path = ChatPath(id);
        return File.Exists(path) ? await JsonSerializer.DeserializeAsync<List<StoredMessage>>(File.OpenRead(path), json) ?? [] : [];
    }
    public async Task SaveChatAsync(string id, IEnumerable<ChatItem> messages)
    {
        var data = messages.Select(x => new StoredMessage(x.Role, x.Content, x.Role == "You" ? null : x.SenderLabel)).ToArray();
        await fileLock.WaitAsync();
        try
        {
            var path = ChatPath(id); var temporary = path + ".tmp";
            await using (var stream = File.Create(temporary)) await JsonSerializer.SerializeAsync(stream, data, json);
            File.Move(temporary, path, true); // Atomic replacement prevents a partially written history.
        }
        finally { fileLock.Release(); }
    }
    public void DeleteChat(string id) { var path = ChatPath(id); if (File.Exists(path)) File.Delete(path); }
    private string ChatPath(string id) => Path.Combine(root, $"{id}.chat.json");
}
