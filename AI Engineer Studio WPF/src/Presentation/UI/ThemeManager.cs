using System.Windows; using System.Windows.Media;
namespace AIEngineerStudio.UI;
public static class ThemeManager
{
    public static void Apply(MainWindow window, string theme)
    {
        var colors = theme == "Светлая" ? new[]{("AppBg","#F6F8FA"),("PanelBg","#FFFFFF"),("SubBg","#F0F2F5"),("CardBg","#FFFFFF"),("Border","#D0D7DE"),("Text","#1F2328"),("Body","#3B424A"),("Muted","#6E7781"),("Accent","#6E40C9"),("Success","#1A7F37")} : new[]{("AppBg","#0D1117"),("PanelBg","#161B22"),("SubBg","#090D13"),("CardBg","#21262D"),("Border","#30363D"),("Text","#F0F6FC"),("Body","#C9D1D9"),("Muted","#8B949E"),("Accent","#7C3AED"),("Success","#3FB950")};
        foreach(var (key, value) in colors) window.Resources[key] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
    }
}
