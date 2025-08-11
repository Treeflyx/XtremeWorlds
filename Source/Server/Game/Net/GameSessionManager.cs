using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Server.Net;

namespace Server.Game.Net;

public sealed class GameSessionManager : INetworkSessionManager<GameSession>
{
    private readonly ILogger<GameSessionManager> _logger;
    private readonly ConcurrentQueue<int> _availableSessionIds = [];

    public GameSessionManager(ILogger<GameSessionManager> logger, IConfiguration configuration)
    {
        _logger = logger;

        var maxConnections = configuration.GetValue("Networking:MaxConnections", Core.Globals.Constant.MaxPlayers);

        foreach (var id in Enumerable.Range(1, maxConnections))
        {
            _availableSessionIds.Enqueue(id);
        }

        _logger.LogInformation("Initialized session manager with a session limit of {MaxSessions}", maxConnections);
    }

    public bool TryCreate(INetworkChannel channel, [NotNullWhen(true)] out GameSession? session)
    {
        if (!_availableSessionIds.TryDequeue(out var sessionId))
        {
            session = null;

            return false;
        }

        session = new GameSession(sessionId, channel, this);

        _logger.LogDebug("Created new session #{SessionId}", session.Id);

        return true;
    }

    public void Destroy(GameSession session)
    {
        _availableSessionIds.Enqueue(session.Id);

        _logger.LogDebug("Destroyed session #{SessionId}", session.Id);
    }
}