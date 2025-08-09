namespace Client.Net;

public interface INetworkEventHandler
{
    Task OnBytesReceivedAsync(ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken);
}