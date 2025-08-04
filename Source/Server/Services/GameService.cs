using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Game;

namespace Server.Services;

public sealed class GameService(
    ILogger<GameService> logger,
    IConfiguration configuration, 
    IPlayerService playerService, 
    IHostApplicationLifetime lifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        General.Logger = logger;
        
        Database.ConnectionString = configuration.GetValue<string>("Database:ConnectionString") ?? throw new InvalidOperationException("Database connection string not found in configuration");
        
        await General.InitServerAsync(configuration);

        foreach (var player in playerService.Players)
        {
            await Player.LeftGame(player.Id);
        }

        lifetime.StopApplication();
    }
}