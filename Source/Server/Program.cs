using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Server.Game;
using Server.Game.Net;
using Server.Net;
using Server.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog(options => options.ReadFrom.Configuration(builder.Configuration));
builder.Services.AddHostedService<GameService>();
builder.Services.AddSingleton<IPlayerService, PlayerService>();
builder.Services.AddNetworkService<GameSession, GameSessionManager, GameNetworkService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<ConsoleInputService>();
}

var app = builder.Build();

await app.RunAsync();