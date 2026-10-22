namespace AIEngineerStudio.Models;

public sealed record ChatMessage(string Role, string Content, IReadOnlyList<Attachment>? Attachments = null, DateTime? SentAt = null, long? DurationMs = null, ChatType ChatType = ChatType.Development, ConversationCard? Card = null, string? ReplyToQuestionId = null);
public sealed record NeuralRequest(string ApiKey, string BaseUrl, string Model, double Temperature, int MaxTokens, string SystemPrompt, string Context, IReadOnlyList<ChatMessage> History);
