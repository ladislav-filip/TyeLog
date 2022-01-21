// See https://aka.ms/new-console-template for more information

using Serilog;

using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(serverUrl: "http://localhost:5341")
    .CreateLogger();

log.Information("Hello, World!");

while (true)
{
    Task.Delay(5000);
    log.Information("Hello, World!");
}