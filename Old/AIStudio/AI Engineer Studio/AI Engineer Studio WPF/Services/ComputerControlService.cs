using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
namespace AIEngineerStudio.Services;

/// <summary>Ограниченный локальный desktop-инструментарий. Выполняется только после явного включения пользователем.</summary>
public sealed class ComputerControlService
{
    [DllImport("user32.dll")] static extern bool SetCursorPos(int x, int y);
    [DllImport("user32.dll")] static extern void mouse_event(uint flags, uint dx, uint dy, uint data, UIntPtr extra);
    const uint LeftDown=0x02, LeftUp=0x04, RightDown=0x08, RightUp=0x10;

    static IEnumerable<ScreenInfo> GetScreens() => new[] { new ScreenInfo { Bounds = new Rectangle(0, 0, GetSystemMetrics(0), GetSystemMetrics(1)) } };
    [DllImport("user32.dll")] static extern int GetSystemMetrics(int index);
    sealed class ScreenInfo { public Rectangle Bounds { get; init; } }

    public string Screenshot(string filepath)
    {
        var bounds = GetScreens().Select(s => s.Bounds)
            .Aggregate(Rectangle.Empty, Rectangle.Union);
        using var bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
        using (var graphics = Graphics.FromImage(bitmap))
            graphics.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);
        Directory.CreateDirectory(Path.GetDirectoryName(filepath)!);
        bitmap.Save(filepath, ImageFormat.Png);
        return $"Скриншот сохранён: {filepath}";
    }

    public string MoveMouse(int x, int y) { EnsureEnabled(); SetCursorPos(x,y); return $"Курсор перемещён в ({x}, {y})"; }
    public string Click(bool right=false) { EnsureEnabled(); mouse_event(right?RightDown:LeftDown,0,0,0,UIntPtr.Zero); mouse_event(right?RightUp:LeftUp,0,0,0,UIntPtr.Zero); return right?"Выполнен правый клик":"Выполнен левый клик"; }
    public string GetCursorPosition() { GetCursorPos(out var p); return $"Курсор: ({p.X}, {p.Y})"; }
    public bool Enabled { get; set; }
    void EnsureEnabled() { if (!Enabled) throw new InvalidOperationException("Управление компьютером отключено в настройках приложения."); }
    [DllImport("user32.dll")] static extern bool GetCursorPos(out POINT point);
    struct POINT { public int X; public int Y; }
}
