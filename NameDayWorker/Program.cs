// See https://aka.ms/new-console-template for more information

using NameDayWorker;
using Serilog;

using var log = new LoggerConfiguration()
    .WriteTo.Seq(serverUrl: "http://localhost:5341")
    .Enrich.WithProperty("AppName", "NameDayWorker")
    .CreateLogger();

Helper.Log = log;

log.Information("Start...");

Console.WriteLine("Please press letter for search names: ");

var startLetters = Console.ReadKey().KeyChar.ToString();

var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\Data\\svatky.txt");
Console.WriteLine(filePath);

try
{
    var storage = new DayNameStorage(filePath);

    foreach (var (day, list) in storage.DayNames)
    {
        var names = list.Where(p => p.StartsWith(startLetters, StringComparison.OrdinalIgnoreCase)).ToList();
        if (names.Any())
        {
            Console.WriteLine(day + " " + string.Join(',', names));
        }
    }
}
catch (Exception e)
{
    log.Error(e, "Unexpected error");
    Console.WriteLine("Error");
}

Console.WriteLine("Finnish.");

// while (true)
// {
//     Task.Delay(5000);
//     log.Information("Hello, World!");
// }