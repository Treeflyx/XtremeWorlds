using Server.Net;

namespace Server.Game;

public sealed class PlayerService : IPlayerService
{
    public static PlayerService Instance { get; } = new();

    private readonly LinkedList<int> _playerIds = [];

    private readonly LinkedList<Player> _players = [];

    public IEnumerable<Player> Players => _players;
    public IEnumerable<int> PlayerIds => _playerIds;

    public bool IsConnected(int playerId)
    {
        return _players.Any(x => x.Id == playerId);
    }

    public void AddPlayer(int playerId, INetworkChannel channel)
    {
        _playerIds.AddLast(playerId);
        _players.AddLast(new Player(playerId, channel));
    }

    public bool RemovePlayer(int playerId)
    {
        var player = _players.FirstOrDefault(x => x.Id == playerId);
        if (player is null)
        {
            return false;
        }

        _playerIds.Remove(playerId);
        _players.Remove(player);

        return true;
    }

    public void SendDataToAll(ReadOnlySpan<byte> data, int head)
    {
        var buffer = new byte[head + 4];

        BitConverter.TryWriteBytes(buffer, head);

        data.CopyTo(buffer.AsSpan(4));

        SendDataToAll(buffer);
    }

    public void SendDataToAll(byte[] bytes)
    {
        foreach (var player in _players)
        {
            player.Send(bytes);
        }
    }

    public void SendDataTo(int playerId, ReadOnlySpan<byte> data, int head)
    {
        var player = _players.FirstOrDefault(x => x.Id == playerId);
        if (player is null)
        {
            return;
        }

        var buffer = new byte[head + 4];

        BitConverter.TryWriteBytes(buffer, head);

        data.CopyTo(buffer.AsSpan(4));

        player.Send(buffer);
    }

    public void SendDataTo(int playerId, byte[] bytes)
    {
        var player = _players.FirstOrDefault(x => x.Id == playerId);

        player?.Send(bytes);
    }

    public string ClientIp(int playerId)
    {
        var player = _players.FirstOrDefault(x => x.Id == playerId);
        if (player is not null)
        {
            return player.IpAddress;
        }

        return string.Empty;
    }
}