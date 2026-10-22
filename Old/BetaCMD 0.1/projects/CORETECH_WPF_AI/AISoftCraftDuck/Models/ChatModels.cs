namespace AISoftCraftDuck.Models;

public enum ProviderKind { OpenAiCompatible, Anthropic, ClaudeCli, CodexCli, KimiCli, HermesCli, GrokCli }
public sealed record ChatMessage(string Role, string Content, IReadOnlyList<Attachment>? Attachments = null);
public sealed record Attachment(string FileName, string MimeType, byte[] Content);
public sealed record ModelInfo(string Id, ProviderKind Provider, bool SupportsVision = true, bool SupportsThinking = true);
public sealed record DuckSettings(string BaseUrl, string? ApiKey, string? AnthropicToken, string? OpenAiKey, string? Proxy, string VersionControl = "None", bool VersionControlLocal = true);
public sealed record FileDirective(string Path, string Content);
