using System.Text.Json.Serialization;
namespace AIEngineerStudio.Models;

public sealed class ProjectDocument
{
    public WebProjectSettings Web { get; set; } = new();
    public string DiscussionId { get; set; } = Guid.NewGuid().ToString("N");
    public bool SurveyStopped { get; set; }
    public string TaskContext { get; set; } = "";
    public string Name { get; set; } = "";
    public string WorkingDirectory { get; set; } = "";
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = "";
    public string Stack { get; set; } = "Другой";
    public List<RemoteProject> RemoteProjects { get; set; } = new();
    public List<ChatMessage> History { get; set; } = new();
}
