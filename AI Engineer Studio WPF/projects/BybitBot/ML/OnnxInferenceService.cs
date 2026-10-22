using BybitBot.Core;
using BybitBot.Domain;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.Extensions.Options;
namespace BybitBot.ML;
public sealed class OnnxInferenceService : IDisposable
{
    private readonly MlOptions _o; private readonly InferenceSession _session;
    public OnnxInferenceService(IOptions<BotOptions> options) { _o=options.Value.Ml; if (!File.Exists(_o.ModelPath)) throw new FileNotFoundException("ONNX-модель не найдена", _o.ModelPath); _session=new InferenceSession(_o.ModelPath); }
    public Prediction Predict(float[] f) { var input = new DenseTensor<float>(f, [1, f.Length]); using var result=_session.Run([NamedOnnxValue.CreateFromTensor(_o.InputName,input)]); var p=result.First(x=>x.Name==_o.OutputName).AsEnumerable<float>().ToArray(); var i=Array.IndexOf(p,p.Max()); return new Prediction((Signal)i,(decimal)p[i]); }
    public void Dispose()=>_session.Dispose();
}