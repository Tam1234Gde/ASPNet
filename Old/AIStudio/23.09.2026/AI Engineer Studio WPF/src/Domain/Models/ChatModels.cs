namespace AIEngineerStudio.Models;

public sealed record ChatMessage(string Role, string Content);
public sealed record NeuralRequest(string ApiKey, string BaseUrl, string Model, double Temperature, int MaxTokens, string SystemPrompt, string Context, IReadOnlyList<ChatMessage> History);
