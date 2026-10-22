using WIS.MasRunner;

var res = MasRunner.ActivateOfficeOhook();

Console.WriteLine($"Успешно: {res.Success}");
Console.WriteLine($"Код выхода: {res.ExitCode}");
Console.WriteLine("--- StandardOutput ---");
Console.WriteLine(res.StandardOutput);

if (!string.IsNullOrEmpty(res.StandardError))
{
    Console.WriteLine("--- StandardError ---");
    Console.WriteLine(res.StandardError);
}