using Server.Net;

namespace Server.Game;

public interface IPlayerService
{
    IEnumerable<Player> Players { get; }
    bool IsConnected(int playerId);
    void AddPlayer(int playerId, INetworkChannel channel);
    bool RemovePlayer(int playerId);
}