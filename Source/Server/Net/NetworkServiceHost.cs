using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Server.Net;

internal sealed class NetworkServiceHost<TSession>(
    ILogger<NetworkServiceHost<TSession>> logger,
    IConfiguration configuration,
    INetworkService<TSession> service,
    INetworkSessionManager<TSession> sessionManager,
    IServiceProvider serviceProvider)
    : BackgroundService
    where TSession : IDisposable
{
    private sealed class NetworkChannelProxy(ILogger logger, INetworkService<TSession> serviceHost, TSession user) : INetworkChannelProxy
    {
        public Task OnConnectedAsync(INetworkChannel channel, CancellationToken cancellationToken)
        {
            logger.LogInformation("Client from {IpAddress} has connected", channel.IpAddress);

            return serviceHost.OnConnectedAsync(user, cancellationToken);
        }

        public Task OnDisconnectedAsync(INetworkChannel channel, CancellationToken cancellationToken)
        {
            logger.LogInformation("Client from {IpAddress} has disconnected", channel.IpAddress);

            return serviceHost.OnDisconnectedAsync(user, cancellationToken);
        }

        public Task OnBytesReceivedAsync(INetworkChannel channel, ReadOnlySpan<byte> bytes, CancellationToken cancellationToken)
        {
            logger.LogTrace("Received {NumberOfBytes} from {IpAddress}", bytes.Length, channel.IpAddress);

            return serviceHost.OnBytesReceivedAsync(user, bytes, cancellationToken);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var port = configuration.GetValue("Networking:Port", 7234);

        var tcpListener = new TcpListener(IPAddress.Any, port);

        logger.LogInformation("Network service started on port {Port}", port);

        try
        {
            tcpListener.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync(stoppingToken);

                HandleTcpClient(tcpClient, stoppingToken);
            }
        }
        catch (TaskCanceledException)
        {
        }
        finally
        {
            tcpListener.Stop();

            logger.LogInformation("Network service stopped");
        }
    }

    private void HandleTcpClient(TcpClient tcpClient, CancellationToken cancellationToken)
    {
        var connectionLogger = serviceProvider.GetRequiredService<ILogger<NetworkChannel<TSession>>>();
        var connection = new NetworkChannel<TSession>(connectionLogger, tcpClient);

        if (!sessionManager.TryCreate(connection, out var user))
        {
            logger.LogInformation("Client from {IpAddress} has been rejected - server full", connection.IpAddress);

            tcpClient.Close();
            return;
        }

        var proxy = new NetworkChannelProxy(connectionLogger, service, user);

        _ = connection.StartAsync(proxy, user, cancellationToken);
    }
}