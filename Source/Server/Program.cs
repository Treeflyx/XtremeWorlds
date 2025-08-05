using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Server.Game;
using Server.Game.Net;
using Server.Net;
using Server.Services;

// Get the directory where the executable is located
var exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

// Create builder with content root set to executable's directory
var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    ContentRootPath = exeDir,
    Args = args
});

// Configure services and logging
builder.Services.AddSerilog(options => options.ReadFrom.Configuration(builder.Configuration));
builder.Services.AddHostedService<GameService>();
builder.Services.AddSingleton<IPlayerService, PlayerService>();
builder.Services.AddNetworkService<GameSession, GameSessionManager, GameNetworkService>();
builder.Services.AddHostedService<ConsoleInputService>();

var app = builder.Build();
await app.RunAsync();
