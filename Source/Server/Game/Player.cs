using Server.Net;

namespace Server.Game;

public sealed class Player(int id, INetworkChannel channel)
{
    public int Id { get; } = id;
    public string IpAddress { get; } = channel.IpAddress;

    public void Send(byte[] bytes)
    {
        channel.Send(bytes);
    }

    public void Disconnect()
    {
        channel.Close();
    }
}