using System.Text.Json.Serialization;
namespace AIEngineerStudio.Models;
public sealed class ProjectDocument
{
    public string Name { get; set; } = "";
    public string WorkingDirectory { get; set; } = "";
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = "";
    public List<ChatMessage> History { get; set; } = new();
}
