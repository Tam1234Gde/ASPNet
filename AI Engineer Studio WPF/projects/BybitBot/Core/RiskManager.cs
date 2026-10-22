using BybitBot.Core;
namespace BybitBot.Core;
public sealed class RiskManager
{
    private readonly RiskOptions _risk; private DateTimeOffset _lastOrder = DateTimeOffset.MinValue; private decimal _dayStartEquity; private DateOnly _day;
    public RiskManager(Microsoft.Extensions.Options.IOptions<BotOptions> o) => _risk=o.Value.Risk;
    public bool CanTrade(decimal equity, int positions, int cooldown) { var today=DateOnly.FromDateTime(DateTime.UtcNow); if(today!=_day){_day=today;_dayStartEquity=equity;} return positions < _risk.MaxPositionCount && DateTimeOffset.UtcNow-_lastOrder >= TimeSpan.FromSeconds(cooldown) && equity >= _dayStartEquity*(1-_risk.DailyMaxLossPercent); }
    public decimal Quantity(decimal balance, decimal entry, decimal stopLoss) { var distance=Math.Abs(entry-stopLoss); return distance <= 0 ? 0 : Math.Floor((balance*_risk.MaxRiskPercent/distance)*1000)/1000; }
    public void MarkOrder() => _lastOrder=DateTimeOffset.UtcNow;
}