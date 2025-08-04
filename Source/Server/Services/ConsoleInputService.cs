using Core;
using Microsoft.Extensions.Hosting;
using static Core.Global.Command;

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

            var parts = line.Split(' ');
            if (parts.Length < 1)
            {
                continue;
            }

            await General.HandlePlayerCommandAsync(GameLogic.FindPlayer(parts[1]), parts[0]);

            switch (parts[0].ToLower() ?? "")
            {
                case "/help":
                {
                    #region Body

                    Console.WriteLine("/help, shows this message.");
                    Console.WriteLine("/exit, closes down the server.");
                    Console.WriteLine("/access, sets player access level, use with '/access name level goes from 1 for Player, to 5 to Owner.");
                    Console.WriteLine("/kick, kicks user from server, use with '/kick name'");
                    Console.WriteLine("/ban, bans user from server, use with '/ban name'");
                    Console.WriteLine("/shutdown, shuts down the server");
                    break;
                }

                #endregion

                case "/shutdown":
                {
                    #region Body

                    if (General.GetShutDownTimer.IsRunning)
                    {
                        General.GetShutDownTimer.Stop();
                        Console.WriteLine("Server shutdown has been cancelled!");
                        NetworkSend.GlobalMsg("Server shutdown has been cancelled!");
                    }
                    else
                    {
                        if (General.GetShutDownTimer.ElapsedTicks > 0L)
                        {
                            General.GetShutDownTimer.Restart();
                        }
                        else
                        {
                            General.GetShutDownTimer.Start();
                        }

                        Console.WriteLine("Server shutdown in " + SettingsManager.Instance.ServerShutdown + " seconds!");
                        NetworkSend.GlobalMsg("Server shutdown in " + SettingsManager.Instance.ServerShutdown + " seconds!");
                    }

                    break;
                }

                #endregion

                case "/exit":
                {
                    #region Body

                    await General.DestroyServerAsync();
                    break;
                }

                #endregion

                case "/access":
                {
                    #region Body

                    if (parts.Length < 3)
                        continue;

                    string name = parts[1];
                    int pindex = GameLogic.FindPlayer(name);
                    byte access;
                    byte.TryParse(parts[2], out access);

                    if (pindex == -1)
                    {
                        Console.WriteLine("Player name is empty or invalid. [Name not found]");
                    }
                    else
                    {
                        switch (access)
                        {
                            case (byte) AccessLevel.Player:
                            {
                                SetPlayerAccess(pindex, access);
                                NetworkSend.SendPlayerData(pindex);
                                NetworkSend.PlayerMsg(pindex, "Your access has been set to Player!", (int) Core.Color.Yellow);
                                Console.WriteLine("Successfully set the access level to " + access + " for player " + name);
                                break;
                            }
                            case (byte) AccessLevel.Moderator:
                            {
                                SetPlayerAccess(pindex, access);
                                NetworkSend.SendPlayerData(pindex);
                                NetworkSend.PlayerMsg(pindex, "Your access has been set to Moderator!", (int) Core.Color.Yellow);
                                Console.WriteLine("Successfully set the access level to " + access + " for player " + name);
                                break;
                            }
                            case (byte) AccessLevel.Mapper:
                            {
                                SetPlayerAccess(pindex, access);
                                NetworkSend.SendPlayerData(pindex);
                                NetworkSend.PlayerMsg(pindex, "Your access has been set to Mapper!", (int) Core.Color.Yellow);
                                Console.WriteLine("Successfully set the access level to " + access + " for player " + name);
                                break;
                            }
                            case (byte) AccessLevel.Developer:
                            {
                                SetPlayerAccess(pindex, access);
                                NetworkSend.SendPlayerData(pindex);
                                NetworkSend.PlayerMsg(pindex, "Your access has been set to Developer!", (int) Core.Color.Yellow);
                                Console.WriteLine("Successfully set the access level to " + access + " for player " + name);
                                break;
                            }
                            case (byte) AccessLevel.Owner:
                            {
                                SetPlayerAccess(pindex, access);
                                NetworkSend.SendPlayerData(pindex);
                                NetworkSend.PlayerMsg(pindex, "Your access has been set to Owner!", (int) Core.Color.Yellow);
                                Console.WriteLine("Successfully set the access level to " + access + " for player " + name);
                                break;
                            }

                            default:
                            {
                                Console.WriteLine("Failed to set the access level to " + access + " for player " + name);
                                break;
                            }
                        }
                    }

                    break;
                }

                #endregion

                case "/kick":
                {
                    #region Body

                    if (parts.Length < 2)
                        continue;

                    string name = parts[1];
                    int pindex = GameLogic.FindPlayer(name);
                    if (pindex == -1)
                    {
                        Console.WriteLine("Player name is empty or invalid.");
                    }
                    else
                    {
                        NetworkSend.AlertMsg(pindex, SystemMessage.Kicked);
                        await Player.LeftGame(pindex);
                    }

                    break;
                }

                #endregion

                case "/ban":
                {
                    #region Body

                    if (parts.Length < 2)
                        continue;

                    string name = parts[1];
                    int pindex = GameLogic.FindPlayer(name);
                    if (pindex == -1)
                    {
                        Console.WriteLine("Player name is empty or invalid. [Name not found]");
                    }
                    else
                    {
                        Database.ServerBanindex(pindex);
                    }

                    break;
                }

                #endregion

                case "/timespeed":
                {
                    #region Body

                    if (parts.Length < 2)
                        return;

                    double speed;
                    double.TryParse(parts[1], out speed);
                    Clock.Instance.GameSpeed = speed;
                    SettingsManager.Instance.TimeSpeed = speed;
                    SettingsManager.Save();
                    Console.WriteLine("Set GameSpeed to " + Clock.Instance.GameSpeed + " secs per seconds");
                    break;
                }

                case var case5 when case5 == "":
                {

                    #endregion

                    continue;
                }

                default:
                {
                    Console.WriteLine("Invalid  If you are unsure of the functions type '/help'.");
                    break;
                }
            }
        }
    }
}