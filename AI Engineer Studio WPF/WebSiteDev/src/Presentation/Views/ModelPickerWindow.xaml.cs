using AIEngineerStudio.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace AIEngineerStudio.Views;

public partial class ModelPickerWindow : Window
{
    sealed record ModelRow(AiModelInfo Model, bool IsCurrent)
    {
        public string Summary => $"{Model.Provider} · {Model.PriceCoefficient}";
        public string PurposeLabel => string.Join(" · ", Model.Purposes);
        public string CurrentLabel => IsCurrent ? "✓ ТЕКУЩАЯ" : string.Empty;
    }

    readonly ObservableCollection<ModelRow> rows = new();
    readonly IReadOnlyList<string> purposes = new[] { "Все назначения", "Универсальные задачи", "Программирование", "Анализ и рассуждения", "Тексты и документы", "Быстрые ответы" };
    readonly string initialModel = string.Empty;
    AiModelInfo? selectedModel;

    public AiModelInfo? SelectedModel => selectedModel;

    public ModelPickerWindow(string currentModel)
    {
        initialModel = currentModel;
        InitializeComponent();
        PurposeBox.ItemsSource = purposes;
        PurposeBox.SelectedIndex = 0;
        ModelList.ItemsSource = rows;
        Rebuild();
        ModelList.SelectedItem = rows.FirstOrDefault(row => row.Model.Id == currentModel) ?? rows.FirstOrDefault();
    }

    void Rebuild()
    {
        string query = SearchBox?.Text.Trim() ?? string.Empty;
        string? purpose = PurposeBox?.SelectedItem as string;
        List<AiModelInfo> filtered = ModelCatalog.All.Where(model =>
            (purpose is null || purpose == "Все назначения" || model.Purposes.Contains(purpose)) &&
            (query.Length == 0 || $"{model.Id} {model.Provider} {model.Category} {string.Join(" ", model.Purposes)} {model.Description} {model.BestFor}".Contains(query, StringComparison.OrdinalIgnoreCase))).ToList();
        string? previousId = (ModelList?.SelectedItem as ModelRow)?.Model.Id;
        rows.Clear();
        foreach (AiModelInfo model in filtered) rows.Add(new ModelRow(model, model.Id == initialModel));
        if (ModelList != null) ModelList.SelectedItem = rows.FirstOrDefault(row => row.Model.Id == previousId) ?? rows.FirstOrDefault(row => row.IsCurrent) ?? rows.FirstOrDefault();
        if (ResultCount != null) ResultCount.Text = $"Показано моделей: {rows.Count} из {ModelCatalog.All.Count}";
        if (EmptyState != null) EmptyState.Visibility = rows.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    void FilterChanged(object sender, TextChangedEventArgs e) => Rebuild();
    void PurposeChanged(object sender, SelectionChangedEventArgs e) => Rebuild();

    void ModelChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ModelList.SelectedItem is not ModelRow row)
        {
            selectedModel = null;
            if (Details != null) Details.DataContext = null;
            if (ApplyButton != null) ApplyButton.IsEnabled = false;
            return;
        }
        selectedModel = row.Model;
        Details.DataContext = row;
        ApplyButton.IsEnabled = true;
    }

    void Apply_Click(object sender, RoutedEventArgs e)
    {
        if (selectedModel is null) return;
        DialogResult = true;

    }
}
