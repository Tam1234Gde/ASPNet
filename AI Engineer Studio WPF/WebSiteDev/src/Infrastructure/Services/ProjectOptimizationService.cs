using System.Text;
using System.IO;
using System.Text.Json;
namespace AIEngineerStudio.Services;
public sealed class ProjectOptimizationService
{
 static readonly HashSet<string> Ignored=new(StringComparer.OrdinalIgnoreCase){".git",".vs",".idea","bin","obj","node_modules","artifacts","__pycache__", ".coretech"};
 static readonly HashSet<string> Secrets=new(StringComparer.OrdinalIgnoreCase){".env",".env.local",".env.production","secrets.json","appsettings.Development.json"};
 readonly string root,dir,index; Dictionary<string,Entry> files=new(StringComparer.OrdinalIgnoreCase); DateTime updated;
 public int FileCount=>files.Count; public ProjectOptimizationService(string path,string appId="website") { root=Path.GetFullPath(path); dir=Path.Combine(root,".coretech",appId); index=Path.Combine(dir,"project-index.json"); Load(); }
 public Task RefreshInBackgroundAsync(CancellationToken ct=default)=>Task.Run(()=>Refresh(ct),ct);
 void Refresh(CancellationToken ct){Directory.CreateDirectory(dir);var next=new Dictionary<string,Entry>(StringComparer.OrdinalIgnoreCase);foreach(var p in Enumerate(root,ct)){var i=new FileInfo(p);var r=Path.GetRelativePath(root,p).Replace('\\','/');next[r]=new(r,i.Length,i.LastWriteTimeUtc);}files=next;updated=DateTime.UtcNow;File.WriteAllText(index,JsonSerializer.Serialize(new Document(updated,next.Values),new JsonSerializerOptions{WriteIndented=true}));}
 IEnumerable<string> Enumerate(string d,CancellationToken ct){IEnumerable<string> fs;try{fs=Directory.EnumerateFiles(d);}catch{yield break;}foreach(var p in fs){ct.ThrowIfCancellationRequested();if(!Secrets.Contains(Path.GetFileName(p)))yield return p;}IEnumerable<string> ds;try{ds=Directory.EnumerateDirectories(d);}catch{yield break;}foreach(var x in ds){if(Ignored.Contains(Path.GetFileName(x)))continue;foreach(var p in Enumerate(x,ct))yield return p;}}
 void Load(){try{if(!File.Exists(index))return;var d=JsonSerializer.Deserialize<Document>(File.ReadAllText(index));if(d?.Files!=null)files=d.Files.ToDictionary(x=>x.Path,StringComparer.OrdinalIgnoreCase);updated=d?.UpdatedAt??DateTime.MinValue;}catch{files=new(StringComparer.OrdinalIgnoreCase);}}
 public string Context(string query,int max=24){var terms=query.Split(new[]{' ','\r','\n','/','\\',':','.'},StringSplitOptions.RemoveEmptyEntries).Where(x=>x.Length>2).Take(25).ToArray();var pick=files.Values.OrderByDescending(x=>terms.Sum(t=>x.Path.Contains(t,StringComparison.OrdinalIgnoreCase)?3:0)).Take(max);return "Индекс проекта (данные):\n"+string.Join('\n',pick.Select(x=>$"- {x.Path} ({x.Length} байт, {x.LastWriteUtc:O})"));}
 public string SelectModel(string query,string configured,bool enabled,out string reason){if(!enabled){reason="автоматический выбор отключён";return configured;}bool hard=query.Length>900||query.Contains("реализ",StringComparison.OrdinalIgnoreCase)||query.Contains("архитект",StringComparison.OrdinalIgnoreCase)||query.Contains("рефактор",StringComparison.OrdinalIgnoreCase);reason=hard?"сложная разработка или большой запрос":"быстрый анализ";return hard?configured:(configured.Contains("terra",StringComparison.OrdinalIgnoreCase)?"gpt-5.6-luna":configured);}
 public string Metrics()=>$"Индекс: {FileCount} файлов, обновлён {updated:O}";
 record Document(DateTime UpdatedAt,IEnumerable<Entry> Files); record Entry(string Path,long Length,DateTime LastWriteUtc);
}