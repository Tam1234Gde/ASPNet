using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace BybitBot.Desktop;

public partial class MainWindow : Window
{
    private readonly string _settingsPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "appsettings.json"));
    public MainWindow() { InitializeComponent(); LoadSettings(); }

    private void LoadSettings()
    {
        if (!File.Exists(_settingsPath)) { StatusText.Text = "Файл appsettings.json не найден."; return; }
        try
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(_settingsPath)); var bot = doc.RootElement.GetProperty("Bot");
            var bybit = bot.GetProperty("Bybit"); EnvironmentBox.SelectedIndex = bybit.GetProperty("Environment").GetString()?.Equals("Mainnet", StringComparison.OrdinalIgnoreCase) == true ? 1 : 0;
            BybitKeyBox.Text = bybit.GetProperty("ApiKey").GetString() ?? ""; SymbolBox.Text = bot.GetProperty("Trading").GetProperty("Symbol").GetString() ?? "BTCUSDT";
            ConfidenceBox.Text = bot.GetProperty("Trading").GetProperty("ConfidenceThreshold").GetDecimal().ToString(CultureInfo.InvariantCulture); RiskBox.Text = bot.GetProperty("Risk").GetProperty("MaxRiskPercent").GetDecimal().ToString(CultureInfo.InvariantCulture);
            if (bot.TryGetProperty("Ai", out var ai)) { AiEnabledBox.IsChecked = ai.GetProperty("Enabled").GetBoolean(); AiModelBox.Text = ai.GetProperty("Model").GetString() ?? ""; AiUrlBox.Text = ai.GetProperty("BaseUrl").GetString() ?? ""; }
            StatusText.Text = "Настройки загружены. Секреты не отображаются.";
        } catch (Exception ex) { StatusText.Text = "Ошибка чтения: " + ex.Message; }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (!decimal.TryParse(ConfidenceBox.Text, CultureInfo.InvariantCulture, out var confidence) || confidence is < 0 or > 1 || !decimal.TryParse(RiskBox.Text, CultureInfo.InvariantCulture, out var risk) || risk is <= 0 or > 1) { StatusText.Text = "Проверьте порог уверенности и риск: значения должны быть в диапазоне 0..1."; return; }
        if (EnvironmentBox.SelectedIndex == 1 && MessageBox.Show("Включается Mainnet с реальными средствами. Вы уверены?", "ВНИМАНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
        var data = new { Bot = new { Bybit = new { Environment = ((ComboBoxItem)EnvironmentBox.SelectedItem).Content.ToString(), ApiKey = BybitKeyBox.Text, ApiSecret = BybitSecretBox.Password, ReceiveWindow = 5000, TimeoutSeconds = 15 }, Trading = new { Symbol = SymbolBox.Text.Trim().ToUpperInvariant(), BarWindow = 100, ConfidenceThreshold = confidence, CooldownSeconds = 300 }, Risk = new { MaxRiskPercent = risk, DailyMaxLossPercent = 0.03m, MaxPositionCount = 1, StopLossAtrMultiplier = 2m, TakeProfitAtrMultiplier = 3m }, Ml = new { ModelPath = "model.onnx", InputName = "features", OutputName = "probabilities" }, Ai = new { Enabled = AiEnabledBox.IsChecked == true, BaseUrl = AiUrlBox.Text.Trim(), ApiKey = AiTokenBox.Password, Model = AiModelBox.Text.Trim(), TimeoutSeconds = 20 } }, Logging = new { LogLevel = new { Default = "Information", Microsoft = "Warning" } } };
        File.WriteAllText(_settingsPath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
        StatusText.Text = "Сохранено. Перезапустите Worker, чтобы применить настройки.";
    }
}
