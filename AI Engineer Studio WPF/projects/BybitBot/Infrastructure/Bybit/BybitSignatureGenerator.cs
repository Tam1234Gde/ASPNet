using System.Security.Cryptography;
using System.Text;
namespace BybitBot.Infrastructure.Bybit;
public sealed class BybitSignatureGenerator
{
    public string Generate(long timestamp, string apiKey, int recvWindow, string payload, string apiSecret)
    {
        var value = $"{timestamp}{apiKey}{recvWindow}{payload}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
        return Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
    }
}