namespace Client.Net;

public interface INetworkEventHandler
{
    Task OnConnectedAsync(CancellationToken cancellationToken);
    Task OnDisconnectedAsync(CancellationToken cancellationToken);
    Task OnBytesReceivedAsync(ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken);
}