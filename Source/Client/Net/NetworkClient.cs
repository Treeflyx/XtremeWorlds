using System.Buffers;
using System.Net.Sockets;
using System.Threading.Channels;

namespace Client.Net;

public sealed class NetworkClient
{
    private Channel<byte[]>? _sendChannel;
    private bool _connected;

    public bool Connected => _connected;

    public async Task StartAsync(string hostname, int port, INetworkEventHandler eventHandler, CancellationToken cancellationToken)
    {
        var connected = Interlocked.Exchange(ref _connected, true);
        if (connected)
        {
            return;
        }

        _sendChannel = Channel.CreateUnbounded<byte[]>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        var tcpClient = new TcpClient();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var connect = tcpClient.ConnectAsync(hostname, port, cancellationToken).AsTask();
                var timeout = Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);

                // Bit of a awkard timeout mechanism here because TcpClient.ConnectAsync does not respect CancellationToken's
                await Task.WhenAny(connect, timeout);

                if (!tcpClient.Connected)
                {
                    Console.WriteLine("Timeout");

                    continue;
                }

                await eventHandler.OnConnectedAsync(cancellationToken);

                await RunAsync(tcpClient, _sendChannel, eventHandler, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Cancelled");
        }
        finally
        {
            Interlocked.Exchange(ref _connected, false);
        }
    }

    private static async Task RunAsync(TcpClient tcpClient, Channel<byte[]> sendChannel, INetworkEventHandler eventHandler, CancellationToken cancellationToken)
    {
        try
        {
            await Task.WhenAll(
                RunReceive(tcpClient, eventHandler,
                    cancellationToken),
                RunSend(tcpClient, sendChannel,
                    cancellationToken));
        }
        finally
        {
            await eventHandler.OnDisconnectedAsync(cancellationToken);
        }
    }

    private static async Task RunReceive(TcpClient tcpClient, INetworkEventHandler eventHandler, CancellationToken cancellationToken)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(4096);

        try
        {
            var networkStream = tcpClient.GetStream();

            while (true)
            {
                var bytesReceived = await networkStream.ReadAsync(buffer, cancellationToken);
                if (bytesReceived == 0)
                {
                    break;
                }

                await eventHandler.OnBytesReceivedAsync(buffer.AsMemory(0, bytesReceived), cancellationToken);
            }
        }
        finally
        {
            tcpClient.Close();

            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private static async Task RunSend(TcpClient tcpClient, Channel<byte[]> sendChannel, CancellationToken cancellationToken)
    {
        try
        {
            var networkStream = tcpClient.GetStream();

            await foreach (var bytes in sendChannel.Reader.ReadAllAsync(cancellationToken))
            {
                await networkStream.WriteAsync(bytes, cancellationToken);
            }
        }
        catch (ObjectDisposedException) // Happens when RunReceive closes the TcpClient and disposes the stream
        {
        }
        finally
        {
            sendChannel.Writer.TryComplete();
        }
    }

    public void Send(byte[] bytes)
    {
        _sendChannel?.Writer.TryWrite(bytes);
    }
}