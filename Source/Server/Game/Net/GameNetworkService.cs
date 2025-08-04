using System.Security.Cryptography;
using Server.Game.Net.Protocol;
using Server.Net;

namespace Server.Game.Net;

public sealed class GameNetworkService : NetworkService<GameSession>
{
    public override Task OnConnectedAsync(GameSession session, CancellationToken cancellationToken)
    {
        session.Aes = Aes.Create();
        session.Channel.Send(new AesPacket(session.Aes.Key, session.Aes.IV));

        return Task.CompletedTask;
    }

    public override async Task OnDisconnectedAsync(GameSession session, CancellationToken cancellationToken)
    {
        await Server.Player.LeftGame(session.Id);
    }

    public override Task OnBytesReceivedAsync(GameSession session, ReadOnlySpan<byte> bytes, CancellationToken cancellationToken)
    {
        session.Parse(bytes);

        return Task.CompletedTask;
    }
}