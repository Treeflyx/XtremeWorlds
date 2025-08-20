using System.Buffers;
using System.Net.Sockets;
using System.Threading.Channels;

namespace Client.Net;

public sealed class NetworkClient
{
    private Channel<byte[]>? _sendChannel;
    private volatile bool _isConnected; // true only when TCP is connected
    private int _started; // 0/1 guard to avoid starting twice

    public bool Connected => _isConnected;

    public async Task StartAsync(string hostname, int port, INetworkEventHandler eventHandler, CancellationToken cancellationToken)
    {
        // Ensure only a single runner loop
        if (Interlocked.Exchange(ref _started, 1) == 1)
        {
            return;
        }

        _sendChannel = Channel.CreateUnbounded<byte[]>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

    try
        {
            Console.WriteLine("Connecting to server...");
            
            while (!cancellationToken.IsCancellationRequested)
            {
                _isConnected = false; // assume disconnected until we actually connect
                TcpClient tcpClient = null;
                try
                {
                    tcpClient = new TcpClient();

                    var connect = tcpClient.ConnectAsync(hostname, port, cancellationToken).AsTask();
                    var timeout = Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);

                    // TcpClient.ConnectAsync ignores CancellationTokens; race a timeout instead
                    await Task.WhenAny(connect, timeout);

                    if (!tcpClient.Connected)
                    {
                        // Avoid tight loop when server is down
                        await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
                        continue;
                    }

                    Console.WriteLine("Connected to server successfully");
                    _isConnected = true;

                    await RunAsync(tcpClient, _sendChannel, eventHandler, cancellationToken);

                    Console.WriteLine("Reconnecting...");
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Socket error while connecting: {ex.Message}");
                    // Backoff a bit before retry
                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                }
                catch (ObjectDisposedException)
                {
                    // Disposed due to shutdown; exit loop
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Network connect loop error: {ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                }
                finally
                {
            try { tcpClient?.Close(); } catch { }
            _isConnected = false; // mark disconnected on any exit
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
        _isConnected = false;
        Interlocked.Exchange(ref _started, 0);
        }
    }

    private static async Task RunAsync(TcpClient tcpClient, Channel<byte[]> sendChannel, INetworkEventHandler eventHandler, CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            RunReceive(tcpClient, eventHandler,
                cancellationToken),
            RunSend(tcpClient, sendChannel,
                cancellationToken));
    }

    private static async Task RunReceive(TcpClient tcpClient, INetworkEventHandler eventHandler, CancellationToken cancellationToken)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(4096);

        try
        {
            var networkStream = tcpClient.GetStream();

            while (true)
            {
                int bytesReceived;
                try
                {
                    bytesReceived = await networkStream.ReadAsync(buffer, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    // Graceful shutdown
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Receive error: {ex.Message}");
                    break;
                }
                if (bytesReceived == 0)
                {
                    Console.WriteLine("Connection with the server has been lost");
                    break;
                }

                try
                {
                    await eventHandler.OnBytesReceivedAsync(buffer.AsMemory(0, bytesReceived), cancellationToken);
                }
                catch (Exception ex)
                {
                    // Swallow handler errors to avoid tearing down the connection
                    Console.WriteLine($"OnBytesReceived handler error: {ex.Message}");
                }
            }
        }
        finally
        {
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
        catch (SocketException ex)
        {
            Console.WriteLine($"Socket error while sending: {ex.Message}");
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