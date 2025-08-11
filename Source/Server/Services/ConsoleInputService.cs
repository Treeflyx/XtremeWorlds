using Core;
using Microsoft.Extensions.Hosting;
using static Core.Globals.Command;

namespace Server.Services;

public sealed class ConsoleInputService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (Console.IsInputRedirected)
        {
            return;
        }

        await ConsoleThreadAsync(stoppingToken);
    }

    public static async Task ConsoleThreadAsync(CancellationToken cancellationToken)
    {
        await using var stream = Console.OpenStandardInput();

        using var streamReader = new StreamReader(stream);

        while (!cancellationToken.IsCancellationRequested)
        {
            var line = await streamReader.ReadLineAsync(cancellationToken);
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            var command = line.Split(' ');
            if (command.Length < 1)
            {
                continue;
            }

            await General.HandlePlayerCommandAsync(command);
        }
    }
}