using BybitBot.Domain;
namespace BybitBot.Infrastructure.MarketData;
public sealed class SlidingBarWindow
{
    private readonly object _gate = new(); private readonly Queue<Bar> _bars = new();
    public void Add(Bar bar, int maxSize) { if (!bar.Confirmed) return; lock (_gate) { if (_bars.Count > 0 && _bars.Last().OpenTime == bar.OpenTime) return; _bars.Enqueue(bar); while (_bars.Count > maxSize) _bars.Dequeue(); } }
    public IReadOnlyList<Bar> Snapshot() { lock (_gate) return _bars.ToArray(); }
}