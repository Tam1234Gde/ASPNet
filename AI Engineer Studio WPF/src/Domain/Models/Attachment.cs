using System.IO;
namespace AIEngineerStudio.Models;

public sealed record Attachment(string Name, string Path, string Mime, long Size)
{
    public string Kind => Mime.Split('/')[0] switch { "image" => "Изображение", "video" => "Видео", "audio" => "Аудио", _ => "Документ" };
    public string Summary => $"{Kind} · {Size / 1024d:N0} КБ";
}
