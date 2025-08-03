namespace Server.Net;

internal interface INetworkChannelProxy
{
    Task OnConnectedAsync(INetworkChannel channel, CancellationToken cancellationToken);
    Task OnDisconnectedAsync(INetworkChannel channel, CancellationToken cancellationToken);
    Task OnBytesReceivedAsync(INetworkChannel channel, ReadOnlySpan<byte> bytes, CancellationToken cancellationToken);
}