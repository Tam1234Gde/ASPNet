using BybitBot.Domain;
namespace BybitBot.ML;
public sealed class FeatureCalculator
{
    public float[]? Calculate(IReadOnlyList<Bar> bars)
    {
        if (bars.Count < 16) return null; var last = bars[^1]; var prev = bars[^2];
        var returns = bars.Skip(1).Select((b,i) => Math.Log((double)(b.Close / bars[i].Close))).ToArray();
        var gains = returns.TakeLast(14).Where(x=>x>0).DefaultIfEmpty().Average(); var losses = -returns.TakeLast(14).Where(x=>x<0).DefaultIfEmpty().Average(); var rsi = losses == 0 ? 100 : 100 - 100 / (1 + gains / losses);
        var atr = bars.Skip(1).Select((b,i) => Math.Max(b.High-b.Low, Math.Max(Math.Abs(b.High-bars[i].Close), Math.Abs(b.Low-bars[i].Close)))).TakeLast(14).Average();
        var spread = (last.High-last.Low)/last.Close; return [(float)returns[^1], (float)(rsi/100d), (float)(atr/last.Close), (float)spread, (float)((last.Close-prev.Close)/prev.Close)];
    }
}