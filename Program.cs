using Microsoft.AspNetCore.SignalR;
using Serilog;
using TermiunoServer;
using TermiunoServer.Api;
using TermiunoServer.Helpers;

File.WriteAllText("latest_log.txt", "");
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("latest_log.txt")
    .MinimumLevel.Information()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddCors();
builder.Services.AddSingleton<IUserIdProvider, TermiunoUserIdProvider>();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);
var app = builder.Build();

app.UseCors(b =>
{
    b.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials();
});

app.MapHub<GameHub>("/api");
//app.MapGet("/api", c => c.Response.WriteAsync("This is a SignalR server!"));
app.Run();