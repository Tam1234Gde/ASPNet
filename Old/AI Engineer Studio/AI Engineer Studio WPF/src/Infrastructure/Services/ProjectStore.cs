using System.IO;
using System.Text.Json;
using AIEngineerStudio.Models;
namespace AIEngineerStudio.Services;
public sealed class ProjectStore
{
    readonly string projectsDirectory = Path.Combine(AppContext.BaseDirectory, "Projects");
    public ProjectStore() => Directory.CreateDirectory(projectsDirectory);
    public IReadOnlyList<string> Names => Directory.EnumerateFiles(projectsDirectory,"*.json").Select(x=>Path.GetFileNameWithoutExtension(x) ?? "").Where(x=>x.Length>0).OrderBy(x=>x,StringComparer.OrdinalIgnoreCase).ToList();
    public ProjectDocument Create(string name, string directory, string description) { var p=new ProjectDocument{Name=name,WorkingDirectory=directory,Description=description}; Save(p); return p; }
    public ProjectDocument Load(string name) => JsonSerializer.Deserialize<ProjectDocument>(File.ReadAllText(Path.Combine(projectsDirectory, name+".json"))) ?? throw new InvalidDataException("Некорректный проект.");
    public void Save(ProjectDocument project) { project.UpdatedAt=DateTime.UtcNow; File.WriteAllText(Path.Combine(projectsDirectory,project.Name+".json"),JsonSerializer.Serialize(project,new JsonSerializerOptions{WriteIndented=true})); }
}
