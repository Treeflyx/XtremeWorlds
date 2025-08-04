namespace Server.Net;

public interface INetworkService<in TSession>
{
    /// <summary>
    /// Called when a new remote client has connected.
    /// </summary>
    /// <param name="session">The session associated with the remote client.</param>
    /// <param name="cancellationToken"></param>
    Task OnConnectedAsync(TSession session, CancellationToken cancellationToken);

    /// <summary>
    /// Called when a remote client has disconnected.
    /// </summary>
    /// <param name="session">The session associated with the remote client.</param>
    /// <param name="cancellationToken"></param>
    Task OnDisconnectedAsync(TSession session, CancellationToken cancellationToken);

    /// <summary>
    /// Called when bytes have been received from a remote client.
    /// </summary>
    /// <param name="session">The session associated with the remote client.</param>
    /// <param name="bytes">The bytes received.</param>
    /// <param name="cancellationToken"></param>
    Task OnBytesReceivedAsync(TSession session, ReadOnlySpan<byte> bytes, CancellationToken cancellationToken);
}