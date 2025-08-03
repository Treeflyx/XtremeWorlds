using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Server.Net;

public static class DependencyInjection
{
    public static void AddNetworkService<TSession, TSessionManager, TService>(this IServiceCollection services)
        where TSession : IDisposable
        where TSessionManager : class, INetworkSessionManager<TSession>
        where TService : class, INetworkService<TSession>
    {
        services.AddSingleton<INetworkSessionManager<TSession>, TSessionManager>();
        services.AddSingleton<INetworkService<TSession>, TService>();
        services.AddHostedService(serviceProvider =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<NetworkServiceHost<TSession>>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var service = serviceProvider.GetRequiredService<INetworkService<TSession>>();
            var sessionManager = serviceProvider.GetRequiredService<INetworkSessionManager<TSession>>();

            return new NetworkServiceHost<TSession>(logger, configuration, service, sessionManager, serviceProvider);
        });
    }
}