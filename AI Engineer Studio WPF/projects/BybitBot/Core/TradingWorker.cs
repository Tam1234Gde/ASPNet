using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using BybitBot.Domain;
using BybitBot.Infrastructure.Bybit;
using BybitBot.Infrastructure.MarketData;
using BybitBot.ML;
using Microsoft.Extensions.Options;
namespace BybitBot.Core;
public sealed class TradingWorker(IBybitClient bybit, SlidingBarWindow bars, FeatureCalculator features, OnnxInferenceService ml, IAiSignalFilter aiFilter, RiskManager risk, IOptions<BotOptions> options, ILogger<TradingWorker> log) : BackgroundService
{
    private readonly BotOptions _o=options.Value;
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try { await StreamKlinesAsync(ct); } catch (OperationCanceledException) when (ct.IsCancellationRequested) { } catch (Exception ex) { log.LogError(ex,"WebSocket отключён; повтор через 5 секунд"); await Task.Delay(TimeSpan.FromSeconds(5),ct); }
        }
    }
    private async Task StreamKlinesAsync(CancellationToken ct)
    {
        using var ws=new ClientWebSocket(); await ws.ConnectAsync(new Uri(_o.Bybit.WebSocketUrl),ct);
        var subscription=Encoding.UTF8.GetBytes($"{{\"op\":\"subscribe\",\"args\":[\"kline.1.{_o.Trading.Symbol}\"]}}");
        await ws.SendAsync(subscription,WebSocketMessageType.Text,true,ct); log.LogInformation("Подключён поток kline: {Symbol}",_o.Trading.Symbol);
        var buffer=new byte[8192];
        while(ws.State==WebSocketState.Open && !ct.IsCancellationRequested)
        {
            var message=await ws.ReceiveAsync(buffer,ct); if(message.MessageType==WebSocketMessageType.Close) break;
            var json=Encoding.UTF8.GetString(buffer,0,message.Count); using var doc=JsonDocument.Parse(json);
            if(!doc.RootElement.TryGetProperty("topic",out var topic)||!topic.GetString()!.StartsWith("kline.")) continue;
            foreach(var x in doc.RootElement.GetProperty("data").EnumerateArray()) if(x.GetProperty("confirm").GetBoolean())
                await OnCompletedBarAsync(new Bar(DateTimeOffset.FromUnixTimeMilliseconds(x.GetProperty("start").GetInt64()),D(x,"open"),D(x,"high"),D(x,"low"),D(x,"close"),D(x,"volume"),true),ct);
        }
    }
    public async Task OnCompletedBarAsync(Bar bar, CancellationToken ct)
    {
        bars.Add(bar,_o.Trading.BarWindow); var f=features.Calculate(bars.Snapshot()); if(f is null) return;
        var prediction=ml.Predict(f); if(prediction.Signal==Signal.Flat || prediction.Confidence<_o.Trading.ConfidenceThreshold) return;
        if (!await aiFilter.ApproveAsync(prediction, f, bar, ct)) return;
        var wallet=await bybit.GetWalletAsync(ct); var open=await bybit.GetPositionsAsync(_o.Trading.Symbol,ct);
        if(!risk.CanTrade(wallet.Equity,open.Count,_o.Trading.CooldownSeconds)) { log.LogWarning("Сделка заблокирована риск-лимитами"); return; }
        var snapshot=bars.Snapshot(); var atr=snapshot.TakeLast(14).Average(x=>x.High-x.Low); var isLong=prediction.Signal==Signal.Long;
        var sl=isLong ? bar.Close-atr*_o.Risk.StopLossAtrMultiplier : bar.Close+atr*_o.Risk.StopLossAtrMultiplier; var tp=isLong ? bar.Close+atr*_o.Risk.TakeProfitAtrMultiplier : bar.Close-atr*_o.Risk.TakeProfitAtrMultiplier;
        var qty=risk.Quantity(wallet.AvailableBalance,bar.Close,sl); if(qty<=0) return;
        await bybit.PlaceOrderAsync(new OrderRequest(_o.Trading.Symbol,isLong?"Buy":"Sell",qty,sl,tp),ct); risk.MarkOrder(); log.LogWarning("Ордер отправлен: {Signal}, confidence={Confidence}, qty={Qty}",prediction.Signal,prediction.Confidence,qty);
    }
    private static decimal D(JsonElement x,string n)=>decimal.Parse(x.GetProperty(n).GetString()!,System.Globalization.CultureInfo.InvariantCulture);
}