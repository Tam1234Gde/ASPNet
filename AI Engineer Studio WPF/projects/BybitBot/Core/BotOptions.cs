namespace BybitBot.Core;

public sealed class BotOptions
{
    public BybitOptions Bybit { get; init; } = new();
    public TradingOptions Trading { get; init; } = new();
    public RiskOptions Risk { get; init; } = new();
    public MlOptions Ml { get; init; } = new();
    public AiOptions Ai { get; init; } = new();
}
public sealed class BybitOptions
{
    public string Environment { get; init; } = "Testnet";
    public string ApiKey { get; init; } = "";
    public string ApiSecret { get; init; } = "";
    public int ReceiveWindow { get; init; } = 5000;
    public int TimeoutSeconds { get; init; } = 15;
    public string RestBaseUrl => Environment.Equals("Mainnet", StringComparison.OrdinalIgnoreCase) ? "https://api.bybit.com" : "https://api-testnet.bybit.com";
    public string WebSocketUrl => Environment.Equals("Mainnet", StringComparison.OrdinalIgnoreCase) ? "wss://stream.bybit.com/v5/public/linear" : "wss://stream-testnet.bybit.com/v5/public/linear";
}
public sealed class TradingOptions { public string Symbol { get; init; } = "BTCUSDT"; public int BarWindow { get; init; } = 100; public decimal ConfidenceThreshold { get; init; } = 0.65m; public int CooldownSeconds { get; init; } = 300; }
public sealed class RiskOptions { public decimal MaxRiskPercent { get; init; } = 0.01m; public decimal DailyMaxLossPercent { get; init; } = 0.03m; public int MaxPositionCount { get; init; } = 1; public decimal StopLossAtrMultiplier { get; init; } = 2m; public decimal TakeProfitAtrMultiplier { get; init; } = 3m; }
public sealed class MlOptions { public string ModelPath { get; init; } = "model.onnx"; public string InputName { get; init; } = "features"; public string OutputName { get; init; } = "probabilities"; }

// LLM используется только как консервативный фильтр уже рассчитанного ONNX-сигнала.
// Он не получает API-ключи Bybit и не имеет возможности создать ордер.
public sealed class AiOptions
{
    public bool Enabled { get; init; }
    public string BaseUrl { get; init; } = "https://apiduck.sytes.net";
    public string ApiKey { get; init; } = "";
    public string Model { get; init; } = "claude-sonnet-4-6";
    public int TimeoutSeconds { get; init; } = 20;
}