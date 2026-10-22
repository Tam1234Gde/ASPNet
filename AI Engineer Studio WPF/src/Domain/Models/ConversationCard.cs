using System.Text.Json;

namespace AIEngineerStudio.Models;

public sealed class ChoiceOption
{
    public string Id { get; set; } = "";
    public string Text { get; set; } = "";
    public string Description { get; set; } = "";
    public bool Recommended { get; set; }
}
public sealed class ConversationCard
{
    public string Kind { get; set; } = "text";
    public string Text { get; set; } = "";
    public string Description { get; set; } = "";
    public bool Recommended { get; set; }
    public string Id { get; set; } = "";
    public string DiscussionId { get; set; } = "";
    public string ProjectId { get; set; } = "";
    public string ChatId { get; set; } = "";
    public bool NeedsUpdate { get; set; }
    public bool AllowAdvice { get; set; } = true;
    public bool AllowSkip { get; set; } = true;
    public string Selection { get; set; } = "single";
    public bool AllowCustomAnswer { get; set; } = true;
    public bool AllowFinish { get; set; } = true;
    public List<ChoiceOption> Options { get; set; } = new();
    public string State { get; set; } = "pending";
    public string Answer { get; set; } = "";
    public string CustomDraft { get; set; } = "";
    public List<string> SelectedIds { get; set; } = new();

    public static ConversationCard? Parse(string raw)
    {
        try
        {
            ConversationCard? card = JsonSerializer.Deserialize<ConversationCard>(raw, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (card is null || string.IsNullOrWhiteSpace(card.Text) || card.Text.Length > 100000) return null;
            card.Options ??= new();
            card.SelectedIds ??= new();
            if (card.Kind == "prompt") return card;
            if (card.Kind != "question" || string.IsNullOrWhiteSpace(card.Id) || card.Id.Length > 128 ||
                card.Selection is not ("single" or "multiple" or "custom") || card.Options.Count > 12 ||
                (card.Selection != "custom" && card.Options.Count < 2) || (card.Selection == "custom" && !card.AllowCustomAnswer) ||
                card.Options.Any(o => o is null || string.IsNullOrWhiteSpace(o.Id) || o.Id.Length > 128 || string.IsNullOrWhiteSpace(o.Text) || o.Text.Length > 1000 || o.Description is null || o.Description.Length > 1000) ||
                card.Options.Select(o => o.Id).Distinct().Count() != card.Options.Count) return null;
            if (card.State is not ("pending" or "answered" or "skipped" or "completed" or "expired" or "error")) return null;
            card.SelectedIds ??= new();
            if (card.SelectedIds.Any(id => !card.Options.Any(o => o.Id == id))) return null;
            return card;
        }
        catch (JsonException) { return null; }
    }
    public static string Readable(string raw)
    {
        try
        {
            using JsonDocument document = JsonDocument.Parse(raw);
            if (document.RootElement.ValueKind == JsonValueKind.Object && document.RootElement.TryGetProperty("text", out JsonElement text) && text.ValueKind == JsonValueKind.String)
                return text.GetString() ?? raw;
            List<string> parts = new();
            Collect(document.RootElement, parts);
            return parts.Count > 0 ? string.Join("\n", parts) : raw;
        }
        catch (JsonException) { return raw; }
    }
    static void Collect(JsonElement element, List<string> parts)
    {
        if (element.ValueKind == JsonValueKind.String) parts.Add(element.GetString() ?? "");
        else if (element.ValueKind == JsonValueKind.Object)
            foreach (JsonProperty property in element.EnumerateObject()) Collect(property.Value, parts);
        else if (element.ValueKind == JsonValueKind.Array)
            foreach (JsonElement item in element.EnumerateArray()) Collect(item, parts);
    }
    public string ContextText() => Text + (Kind == "question" ? "\n" + string.Join("\n", Options.Select(o => "• " + o.Text + (string.IsNullOrWhiteSpace(o.Description) ? "" : " — " + o.Description))) + $"\nСостояние: {State}. Фактический ответ: {Answer}" : (NeedsUpdate ? "\nИтог устарел, требуется перепроверка." : ""));
}
