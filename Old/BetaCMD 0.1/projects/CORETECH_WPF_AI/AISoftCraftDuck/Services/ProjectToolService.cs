using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace AISoftCraftDuck.Services;

/// <summary>Инструменты агента. Все файловые пути ограничены активной папкой проекта.</summary>
public sealed class ProjectToolService
{
    private static readonly string[] Ignored = [".git", "bin", "obj", "node_modules", ".vs", "__pycache__"];

    public string Execute(string projectRoot, string name, Dictionary<string, object?> args)
    {
        try
        {
            return name switch
            {
                "list_files" => List(projectRoot, Arg(args, "directory", ".")),
                "read_file" => Read(projectRoot, Arg(args, "filepath", ""), args),
                "write_file" => Write(projectRoot, Arg(args, "filepath", ""), Arg(args, "content", "")),
                "edit_file_part" => Edit(projectRoot, Arg(args, "filepath", ""), Arg(args, "old_str", ""), Arg(args, "new_str", "")),
                "create_folder" => Create(projectRoot, Arg(args, "folder_path", "")),
                "delete_item" => Delete(projectRoot, Arg(args, "path", "")),
                "move_item" => Move(projectRoot, Arg(args, "source_path", ""), Arg(args, "destination_path", "")),
                "get_project_tree" => Tree(projectRoot),
                "search_code" => Search(projectRoot, Arg(args, "query", ""), Arg(args, "file_pattern", "*.*")),
                "execute_command" => Command(projectRoot, Arg(args, "command", ""), Math.Clamp(Int(args, "timeout", 60), 1, 600)),
                "get_system_info" => $"OS: {Environment.OSVersion}; .NET: {Environment.Version}; Project: {projectRoot}",
                _ => $"Неизвестный инструмент: {name}"
            };
        }
        catch (Exception ex) { return $"Ошибка инструмента {name}: {ex.Message}"; }
    }

    private static string Arg(Dictionary<string, object?> a, string key, string fallback) => a.TryGetValue(key, out var value) ? value?.ToString() ?? fallback : fallback;
    private static int Int(Dictionary<string, object?> a, string key, int fallback) => int.TryParse(Arg(a, key, ""), out var n) ? n : fallback;
    private static string Full(string root, string relative)
    {
        if (string.IsNullOrWhiteSpace(relative) || relative is "null" or "undefined") throw new ArgumentException("Путь не задан. Передайте относительный путь, например index.html");
        var basePath = Path.GetFullPath(root); var full = Path.GetFullPath(Path.Combine(basePath, relative));
        if (!full.StartsWith(basePath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) && !full.Equals(basePath, StringComparison.OrdinalIgnoreCase)) throw new UnauthorizedAccessException("Путь выходит за пределы проекта");
        return full;
    }
    private static string List(string root, string dir) => string.Join("\n", Directory.EnumerateFileSystemEntries(Full(root, dir)).Select(Path.GetFileName));
    private static string Read(string root, string file, Dictionary<string, object?> a)
    {
        var lines = File.ReadAllLines(Full(root, file), Encoding.UTF8); var start = Math.Max(1, Int(a, "start_line", 1)); var end = Math.Min(lines.Length, Int(a, "end_line", lines.Length));
        return string.Join("\n", lines.Skip(start - 1).Take(Math.Max(0, end - start + 1)).Select((x, i) => $"{start + i}: {x}"));
    }
    private static string Write(string root, string file, string content) { var path = Full(root, file); Directory.CreateDirectory(Path.GetDirectoryName(path)!); File.WriteAllText(path, content, Encoding.UTF8); return $"Файл создан/обновлён: {file}"; }
    private static string Edit(string root, string file, string oldText, string newText) { var path = Full(root, file); var text = File.ReadAllText(path); if (!text.Contains(oldText, StringComparison.Ordinal)) return "Фрагмент не найден"; File.WriteAllText(path, text.Replace(oldText, newText)); return $"Файл изменён: {file}"; }
    private static string Create(string root, string dir) { Directory.CreateDirectory(Full(root, dir)); return $"Папка создана: {dir}"; }
    private static string Delete(string root, string item) { var path = Full(root, item); if (Directory.Exists(path)) Directory.Delete(path, true); else File.Delete(path); return $"Удалено: {item}"; }
    private static string Move(string root, string source, string destination) { var src = Full(root, source); var dst = Full(root, destination); Directory.CreateDirectory(Path.GetDirectoryName(dst)!); if (Directory.Exists(src)) Directory.Move(src, dst); else File.Move(src, dst, true); return $"Перемещено: {source} → {destination}"; }
    private static string Tree(string root) => string.Join("\n", Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories).Where(x => !Ignored.Any(i => x.Split(Path.DirectorySeparatorChar).Contains(i))).Take(1000).Select(x => Path.GetRelativePath(root, x)));
    private static string Search(string root, string query, string pattern) { var rx = new Regex(query, RegexOptions.IgnoreCase); return string.Join("\n", Directory.EnumerateFiles(root, pattern, SearchOption.AllDirectories).Where(x => !Ignored.Any(i => x.Split(Path.DirectorySeparatorChar).Contains(i))).SelectMany(file => File.ReadLines(file).Select((line, i) => (file, line, i))).Where(x => rx.IsMatch(x.line)).Take(200).Select(x => $"{Path.GetRelativePath(root, x.file)}:{x.i + 1}: {x.line}")); }
    private static string Command(string root, string command, int timeout)
    {
        var psi = new ProcessStartInfo { FileName = OperatingSystem.IsWindows() ? "cmd.exe" : "/bin/sh", Arguments = OperatingSystem.IsWindows() ? $"/c {command}" : $"-c \"{command.Replace("\"", "\\\"")}\"", WorkingDirectory = root, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true };
        using var process = Process.Start(psi)!; if (!process.WaitForExit(timeout * 1000)) { try { process.Kill(true); } catch { } return $"Команда прервана по таймауту {timeout} сек."; }
        return $"Код: {process.ExitCode}\nSTDOUT:\n{process.StandardOutput.ReadToEnd()}\nSTDERR:\n{process.StandardError.ReadToEnd()}";
    }
}
