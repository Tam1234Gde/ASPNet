namespace BybitBot.Domain;

public sealed record Bar(DateTimeOffset OpenTime, decimal Open, decimal High, decimal Low, decimal Close, decimal Volume, bool Confirmed);
public enum Signal { Flat = 0, Long = 1, Short = 2 }
public sealed record Prediction(Signal Signal, decimal Confidence);
public sealed record Position(string Symbol, string Side, decimal Size, decimal AvgPrice, decimal UnrealisedPnl);
public sealed record OrderRequest(string Symbol, string Side, decimal Quantity, decimal StopLoss, decimal TakeProfit, string? Price = null);
public sealed record WalletBalance(decimal AvailableBalance, decimal Equity);