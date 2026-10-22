using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BybitBot.Core;
using BybitBot.Domain;
using Microsoft.Extensions.Options;
using Polly;

namespace BybitBot.Infrastructure.Bybit;
public interface IBybitClient { Task<WalletBalance> GetWalletAsync(CancellationToken ct); Task<IReadOnlyList<Position>> GetPositionsAsync(string symbol, CancellationToken ct); Task PlaceOrderAsync(OrderRequest request, CancellationToken ct); }
public sealed class BybitClient(HttpClient http, BybitSignatureGenerator signer, IOptions<BotOptions> options, ILogger<BybitClient> log) : IBybitClient
{
    private readonly BotOptions _o = options.Value;
    private static readonly JsonSerializerOptions Json = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    public async Task<WalletBalance> GetWalletAsync(CancellationToken ct)
    {
        using var doc = await SendAsync(HttpMethod.Get, "/v5/account/wallet-balance?accountType=UNIFIED", null, ct);
        var coin = doc.RootElement.GetProperty("result").GetProperty("list")[0].GetProperty("coin").EnumerateArray().FirstOrDefault(x => x.GetProperty("coin").GetString() == "USDT");
        return new WalletBalance(Parse(coin, "availableToWithdraw"), Parse(coin, "walletBalance"));
    }
    public async Task<IReadOnlyList<Position>> GetPositionsAsync(string symbol, CancellationToken ct)
    {
        using var doc = await SendAsync(HttpMethod.Get, $"/v5/position/list?category=linear&symbol={symbol}", null, ct);
        return doc.RootElement.GetProperty("result").GetProperty("list").EnumerateArray().Where(x => Parse(x, "size") > 0).Select(x => new Position(symbol, x.GetProperty("side").GetString()!, Parse(x,"size"), Parse(x,"avgPrice"), Parse(x,"unrealisedPnl"))).ToArray();
    }
    public async Task PlaceOrderAsync(OrderRequest r, CancellationToken ct)
    {
        if (r.StopLoss <= 0 || r.TakeProfit <= 0) throw new ArgumentException("Stop-Loss и Take-Profit обязательны.");
        var body = JsonSerializer.Serialize(new { category="linear", symbol=r.Symbol, side=r.Side, orderType=r.Price is null ? "Market" : "Limit", qty=r.Quantity.ToString(CultureInfo.InvariantCulture), price=r.Price, timeInForce="GTC", takeProfit=r.TakeProfit.ToString(CultureInfo.InvariantCulture), stopLoss=r.StopLoss.ToString(CultureInfo.InvariantCulture), tpslMode="Full" }, Json);
        using var _ = await SendAsync(HttpMethod.Post, "/v5/order/create", body, ct);
    }
    private async Task<JsonDocument> SendAsync(HttpMethod method, string path, string? body, CancellationToken ct)
    {
        var retry = new ResiliencePipelineBuilder<HttpResponseMessage>().AddRetry(new Polly.Retry.RetryStrategyOptions<HttpResponseMessage> { MaxRetryAttempts=3, Delay=TimeSpan.FromSeconds(1), BackoffType=Polly.DelayBackoffType.Exponential, ShouldHandle=new PredicateBuilder<HttpResponseMessage>().Handle<HttpRequestException>().HandleResult(r => r.StatusCode == HttpStatusCode.TooManyRequests || (int)r.StatusCode >= 500) }).Build();
        var response = await retry.ExecuteAsync(async token =>
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); var payload = method == HttpMethod.Get ? path[(path.IndexOf('?') + 1)..] : body ?? "";
            var req = new HttpRequestMessage(method, path); req.Headers.Add("X-BAPI-API-KEY", _o.Bybit.ApiKey); req.Headers.Add("X-BAPI-TIMESTAMP", timestamp.ToString()); req.Headers.Add("X-BAPI-RECV-WINDOW", _o.Bybit.ReceiveWindow.ToString()); req.Headers.Add("X-BAPI-SIGN", signer.Generate(timestamp, _o.Bybit.ApiKey, _o.Bybit.ReceiveWindow, payload, _o.Bybit.ApiSecret));
            if (body is not null) req.Content = new StringContent(body, Encoding.UTF8, "application/json"); return await http.SendAsync(req, token);
        }, ct);
        var text = await response.Content.ReadAsStringAsync(ct); response.EnsureSuccessStatusCode(); var doc = JsonDocument.Parse(text);
        if (doc.RootElement.GetProperty("retCode").GetInt32() != 0) throw new InvalidOperationException($"Bybit error {doc.RootElement.GetProperty("retCode")}: {doc.RootElement.GetProperty("retMsg")}");
        return doc;
    }
    private static decimal Parse(JsonElement x, string name) => x.TryGetProperty(name, out var v) && decimal.TryParse(v.GetString(), CultureInfo.InvariantCulture, out var n) ? n : 0;
}