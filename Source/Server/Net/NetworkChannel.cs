using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;
using Core.Net;
using Microsoft.Extensions.Logging;

namespace Server.Net;

internal sealed class NetworkChannel<TSession>(ILogger<NetworkChannel<TSession>> logger, TcpClient tcpClient) : INetworkChannel where TSession : IDisposable
{
    private const int BufferSize = 1024;

    private readonly NetworkStream _networkStream = tcpClient.GetStream();
    private readonly Channel<byte[]> _sendChannel = Channel.CreateUnbounded<byte[]>();
    private bool _started;

    public string IpAddress { get; } = (tcpClient.Client.RemoteEndPoint as IPEndPoint)?.Address.ToString() ?? "(none)";

    public async Task StartAsync(INetworkChannelProxy channelProxy, TSession session, CancellationToken cancellationToken)
    {
        var started = Interlocked.Exchange(ref _started, true);
        if (started)
        {
            return;
        }

        if (Debugger.IsAttached)
        {
            await channelProxy.OnConnectedAsync(this, cancellationToken);

            await Task.WhenAll(
                RunSend(cancellationToken),
                RunReceive(channelProxy, cancellationToken));
        }
        else
        {
            try
            {
                await channelProxy.OnConnectedAsync(this, cancellationToken);

                await Task.WhenAll(
                    RunSend(cancellationToken),
                    RunReceive(channelProxy, cancellationToken));
            }
            catch (Exception ex) when (ex is IOException or ObjectDisposedException or OperationCanceledException)
            {
                logger.LogDebug(ex, "Network connection terminated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected exception handling network connection");
            }
            finally
            {
                await channelProxy.OnDisconnectedAsync(this, cancellationToken);

                tcpClient.Close();

                session.Dispose();
            }
        }
    }

    private async Task RunSend(CancellationToken cancellationToken)
    {
        await foreach (var bytes in _sendChannel.Reader.ReadAllAsync(cancellationToken))
        {
            await _networkStream.WriteAsync(bytes, cancellationToken);
        }
    }

    private async Task RunReceive(INetworkChannelProxy channelProxy, CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var bytes = new byte[BufferSize];

                var bytesRead = await _networkStream.ReadAsync(bytes, cancellationToken);
                if (bytesRead == 0)
                {
                    break;
                }

                await channelProxy.OnBytesReceivedAsync(this, bytes.AsSpan(0, bytesRead), cancellationToken);
            }
        }
        finally
        {
            _sendChannel.Writer.TryComplete();
        }
    }

    public void Send(byte[] bytes)
    {
        _sendChannel.Writer.TryWrite(bytes);
    }

    public void Send<TPacket>(TPacket packet) where TPacket : IPacket
    {
        var packetWriter = new PacketWriter();

        packet.Serialize(packetWriter);

        Send(packetWriter.GetBytes());
    }

    public void Close()
    {
        _sendChannel.Writer.TryComplete();
    }
}