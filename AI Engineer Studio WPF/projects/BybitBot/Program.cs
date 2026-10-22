using BybitBot.Core;
using BybitBot.Infrastructure.Bybit;
using BybitBot.Infrastructure.MarketData;
using BybitBot.ML;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<BotOptions>(builder.Configuration.GetSection("Bot"));
builder.Services.AddHttpClient<IBybitClient, BybitClient>((sp, c) =>
{
    var o = sp.GetRequiredService<IOptions<BotOptions>>().Value;
    c.BaseAddress = new Uri(o.Bybit.RestBaseUrl);
    c.Timeout = TimeSpan.FromSeconds(o.Bybit.TimeoutSeconds);
});
builder.Services.AddSingleton<BybitSignatureGenerator>();
builder.Services.AddSingleton<SlidingBarWindow>();
builder.Services.AddSingleton<FeatureCalculator>();
builder.Services.AddSingleton<OnnxInferenceService>();
builder.Services.AddHttpClient<IAiSignalFilter, AiSignalFilter>();
builder.Services.AddSingleton<RiskManager>();
builder.Services.AddHostedService<TradingWorker>();

using var host = builder.Build();
var options = host.Services.GetRequiredService<IOptions<BotOptions>>().Value;
Console.ForegroundColor = options.Bybit.Environment.Equals("Mainnet", StringComparison.OrdinalIgnoreCase) ? ConsoleColor.Red : ConsoleColor.Yellow;
Console.WriteLine($"\n{'=', 70}\n ВНИМАНИЕ: АКТИВНА СЕТЬ {options.Bybit.Environment.ToUpperInvariant()}\n REST: {options.Bybit.RestBaseUrl}\n {'=', 70}\n");
Console.ResetColor();
await host.RunAsync();