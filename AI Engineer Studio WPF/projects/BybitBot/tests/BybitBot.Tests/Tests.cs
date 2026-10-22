using System;
using System.Linq;
using Xunit;
using BybitBot.Domain;
using BybitBot.Infrastructure.Bybit;
using BybitBot.ML;
namespace BybitBot.Tests;
public sealed class Tests
{
 [Fact] public void Signature_IsBybitHmacSha256() => Assert.Equal("f7bc83f430538424b13298e6aa6fb143ef4d59a14946175997479dbc2d1a3cd8",new BybitSignatureGenerator().Generate(1,"key",5000,"payload","secret"));
 [Fact] public void Features_Require16Bars_AndContainFiveValues() { var b=Enumerable.Range(0,16).Select(i=>new Bar(DateTimeOffset.UtcNow.AddMinutes(i),100+i,102+i,99+i,101+i,1,true)).ToArray(); Assert.Equal(5,new FeatureCalculator().Calculate(b)!.Length); }
}