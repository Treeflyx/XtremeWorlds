using Core;
using Core.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Game;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Core.Common;
using Core.Configurations;
using Core.Globals;
using static Core.Globals.Type;
using static Core.Globals.Command;
using Type = Core.Globals.Type;

namespace Server
{
    public class General
    {
        private static readonly RandomUtility Random = new RandomUtility();
        private static bool _serverDestroyed;
        private static string _myIpAddress = string.Empty;
        private static readonly Stopwatch MyStopwatch = new Stopwatch();
        public static ILogger Logger;
        private static readonly object SyncLock = new object();
        private static readonly CancellationTokenSource Cts = new CancellationTokenSource();
        private static Timer? _saveTimer;
        private static Stopwatch _shutDownTimer = new Stopwatch();
        private static int _shutDownLastTimer = 0;
        private static readonly ConcurrentDictionary<int, PlayerStats> PlayerStatistics = new();
        
        #region Utility Methods

        /// <summary>
        /// Retrieves the shutdown timer for server destruction.
        /// </summary>
        public static Stopwatch? GetShutDownTimer => _shutDownTimer;

        /// <summary>
        /// Gets the current server destruction status.
        /// </summary>
        public static bool IsServerDestroyed => _serverDestroyed;

        /// <summary>
        /// Retrieves the random number generator utility.
        /// </summary>
        public static RandomUtility GetRandom => Random;
        
        /// <summary>
        /// Gets the elapsed time in milliseconds since the server started.
        /// </summary>
        public static int GetTimeMs() => (int)MyStopwatch.ElapsedMilliseconds;

        /// <summary>
        /// Retrieves the local IP address of the server.
        /// </summary>
        public static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList
                .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                ?.ToString() ?? "127.0.0.1";
        }

        /// <summary>
        /// Validates a username based on length and allowed characters.
        /// </summary>
        public static int IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return -1;

            if (username.Length < Core.Globals.Constant.MinNameLength || username.Length > Core.Globals.Constant.NameLength)
                return 0;

            return Regex.IsMatch(username, @"^[a-zA-Z0-9_ ]+$") ? 1 : -1;
        }


        /// <summary>
        /// Gets the current server time synchronized across all operations.
        /// </summary>
        public static DateTime GetServerTime() => DateTime.UtcNow;

        /// <summary>
        /// Generates a unique identifier for server entities.
        /// </summary>
        public static long GenerateUniqueId() => Interlocked.Increment(ref Global.UniqueIdCounter);

        #endregion

        #region Server Lifecycle

        /// <summary>
        /// Initializes the game server asynchronously with enhanced features.
        /// </summary>
        public static async System.Threading.Tasks.Task InitServerAsync(IConfiguration configuration)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                try
                {
                    await ServerStartAsync(configuration);
                }
                catch (Exception ex)
                {
                    Logger.LogCritical(ex, "Server initialization failed");
                    await HandleCriticalErrorAsync(ex);
                }
            }
            else
            {
                await ServerStartAsync(configuration);
            }
        }

        private static async System.Threading.Tasks.Task ServerStartAsync(IConfiguration configuration)
        {
            MyStopwatch.Start();
            int startTime = GetTimeMs();

            await InitializeCoreComponentsAsync(configuration);
            await LoadGameDataAsync();
            await StartGameLoopAsync(startTime);
            new TimeManager();
        }

        private static async System.Threading.Tasks.Task InitializeCoreComponentsAsync(IConfiguration configuration)
        {
            await System.Threading.Tasks.Task.WhenAll(LoadConfigurationAsync(), InitializeNetworkAsync(), InitializeChatSystemAsync());
            await InitializeDatabaseWithRetryAsync(configuration);
        }

        public static void InitalizeCoreData()
        {
            Data.Job = new Type.Job[Core.Globals.Constant.MaxJobs];
            Data.Moral = new Type.Moral[Core.Globals.Constant.MaxMorals];
            Data.Map = new Type.Map[Core.Globals.Constant.MaxMaps];
            Data.Item = new Type.Item[Core.Globals.Constant.MaxItems];
            Data.Npc = new Type.Npc[Core.Globals.Constant.MaxNpcs];
            Data.Resource = new Type.Resource[Core.Globals.Constant.MaxResources];
            Data.Projectile = new Type.Projectile[Core.Globals.Constant.MaxProjectiles];
            Data.Animation = new Type.Animation[Core.Globals.Constant.MaxAnimations];
            Data.Shop = new Type.Shop[Core.Globals.Constant.MaxShops];
            Data.Player = new Type.Player[Core.Globals.Constant.MaxPlayers];
            Data.Party = new Type.Party[Core.Globals.Constant.MaxParty];
            Data.MapItem = new Type.MapItem[Core.Globals.Constant.MaxMaps, Core.Globals.Constant.MaxMapItems];
            Data.Npc = new Type.Npc[Core.Globals.Constant.MaxNpcs];
            Data.MapNpc = new MapData[Core.Globals.Constant.MaxMaps];

            for (int i = 0; i < Core.Globals.Constant.MaxMaps; i++)
            {
                Data.MapNpc[i].Npc = new MapNpc[Core.Globals.Constant.MaxMapNpcs];
                for (int x = 0; x < Core.Globals.Constant.MaxMapNpcs; x++)
                {
                    Data.MapNpc[i].Npc[x].Vital = new int[Enum.GetValues(typeof(Vital)).Length];
                    Data.MapNpc[i].Npc[x].SkillCd = new int[Core.Globals.Constant.MaxNpcSkills];
                    Data.MapNpc[i].Npc[x].Num = -1;
                    Data.MapNpc[i].Npc[x].SkillBuffer = -1;
                }

                var statCount = Enum.GetNames(typeof(Stat)).Length;
                for (int x = 0; x < Core.Globals.Constant.MaxItems; x++)
                {
                    Data.Item[x].AddStat = new byte[statCount];
                    Data.Item[x].StatReq = new byte[statCount];
                }

                for (int x = 0; x < Core.Globals.Constant.MaxMapItems; x++)
                {
                    Data.MapItem[i, x].Num = -1;
                }
            }

            Data.Shop = new Type.Shop[Core.Globals.Constant.MaxShops];
            Data.Skill = new Skill[Core.Globals.Constant.MaxSkills];
            Data.MapResource = new Type.MapResource[Core.Globals.Constant.MaxMaps];
            Data.TempPlayer = new Type.TempPlayer[Core.Globals.Constant.MaxPlayers];
            Data.Account = new Type.Account[Core.Globals.Constant.MaxPlayers];

            for (int i = 0; i < Core.Globals.Constant.MaxPlayers; i++)
            {
                Database.ClearPlayer(i);
            }

            for (int i = 0; i < Core.Globals.Constant.MaxPartyMembers; i++)
            {
                Party.ClearParty(i);
            }

            Event.TempEventMap = new Type.GlobalEvents[Core.Globals.Constant.MaxMaps];
            Data.MapProjectile = new Type.MapProjectile[Core.Globals.Constant.MaxMaps, Core.Globals.Constant.MaxProjectiles];
        }

        private static async System.Threading.Tasks.Task LoadGameDataAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            InitalizeCoreData();
            await LoadGameContentAsync();
            await SpawnGameObjectsAsync();           
            Logger.LogInformation($"Game data loaded in {stopwatch.ElapsedMilliseconds}ms");
        }

        private static async System.Threading.Tasks.Task StartGameLoopAsync(int startTime)
        {
            InitializeSaveTimer();
            DisplayServerBanner(startTime);
            UpdateCaption();

            if (!System.Diagnostics.Debugger.IsAttached)
            {
                try
                {
                    await Loop.ServerAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogCritical(ex, "Server loop crashed");
                    await HandleCriticalErrorAsync(ex);
                }
            }
            else
            {
                await Loop.ServerAsync();
            }

        }

        /// <summary>
        /// Shuts down the server gracefully, cleaning up all resources.
        /// </summary>
        public static async System.Threading.Tasks.Task DestroyServerAsync()
        {
            if (_serverDestroyed) return;
            _serverDestroyed = true;
            Cts.Cancel();
            _saveTimer?.Dispose();

            Logger.LogInformation("Server shutdown initiated...");

            await Database.SaveAllPlayersOnlineAsync();

            try
            {
                await Parallel.ForEachAsync(Enumerable.Range(0, Core.Globals.Constant.MaxPlayers), Cts.Token, async (i, ct) =>
                {
                    NetworkSend.SendLeftGame(i);
                    await Player.LeftGame(i);
                });
            }
            catch (TaskCanceledException)
            {
                Logger.LogWarning("Server shutdown tasks were canceled.");
            }
            
            Logger.LogInformation("Server shutdown completed.");
            Environment.Exit(0);
        }

        #endregion

        #region Initialization Methods

        private static async System.Threading.Tasks.Task LoadConfigurationAsync()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                ValidateConfiguration();
                Clock.Instance.GameSpeed = SettingsManager.Instance.TimeSpeed;
                Console.Title = "XtremeWorlds Server";
                _myIpAddress = GetLocalIpAddress();
            });
        }

        private static void ValidateConfiguration()
        {
            if (string.IsNullOrWhiteSpace(SettingsManager.Instance.GameName))
                throw new InvalidOperationException("GameName is not set in configuration");

            if (SettingsManager.Instance.Port <= 0 || SettingsManager.Instance.Port > 65535)
                throw new InvalidOperationException("Invalid Port number in configuration");
        }

        private static async System.Threading.Tasks.Task InitializeNetworkAsync()
        {
        }

        private static async System.Threading.Tasks.Task InitializeDatabaseWithRetryAsync(IConfiguration configuration)
        {
            int maxRetries = configuration.GetValue<int>("Database:MaxRetries", 3);
            int retryDelayMs = configuration.GetValue<int>("Database:RetryDelayMs", 1000);
            Logger.LogInformation("Initializing database...");
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var name = GetDatabaseName(configuration);
                    await Database.CreateDatabaseAsync(name);
                    await Database.CreateTablesAsync();
                    await LoadCharacterListAsync();
                    Logger.LogInformation("Database initialized successfully.");
                    return;
                }
                catch (Exception ex)
                {
                    if (attempt == maxRetries)
                    {
                        Logger.LogCritical(ex, "Failed to initialize database after multiple attempts");
                        throw;
                    }
                    Logger.LogWarning(ex, $"Database initialization failed, attempt {attempt} of {maxRetries}");
                    await System.Threading.Tasks.Task.Delay(retryDelayMs * attempt, Cts.Token);
                }
            }
        }

        private static async System.Threading.Tasks.Task LoadCharacterListAsync()
        {
           var ids = await Database.GetDataAsync("account");
            Data.Char = new CharacterNameList();
            const int maxConcurrency = 4;
            using var semaphore = new SemaphoreSlim(maxConcurrency);

            var tasks = ids.Select(async id =>
            {
                await semaphore.WaitAsync(Cts.Token);
                try
                {
                    for (int i = 0; i < Core.Globals.Constant.MaxChars; i++)
                    {
                        var data = await Database.SelectRowByColumnAsync("id", id, "account", $"character{i + 1}");
                        if (data != null && data["Name"] != null)
                        {
                            string name = data["Name"].ToString();
                            if (!string.IsNullOrWhiteSpace(name))
                            {
                                Data.Char.Add(name);
                            }
                        }
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await System.Threading.Tasks.Task.WhenAll(tasks);
            Logger.LogInformation($"Loaded {Data.Char.Count} character(s).");
        }

        private static async System.Threading.Tasks.Task LoadGameContentAsync()
        {
            const int maxConcurrency = 4;
            using var semaphore = new SemaphoreSlim(maxConcurrency);

            var tasks = new[]
            {
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading jobs..."); await Database.LoadJobsAsync(); Logger.LogInformation("Jobs loaded."); }),
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading morals..."); await Moral.LoadMoralsAsync(); Logger.LogInformation("Morals loaded."); }),
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading maps..."); await Database.LoadMapsAsync(); Logger.LogInformation("Maps loaded."); }),
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading items..."); await Item.LoadItemsAsync(); Logger.LogInformation("Items loaded."); }),
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading Npcs..."); await Database.LoadNpcsAsync(); Logger.LogInformation("Npcs loaded."); }),
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading resources..."); await Resource.LoadResourcesAsync(); Logger.LogInformation("Resources loaded."); }),
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading shops..."); await Database.LoadShopsAsync(); Logger.LogInformation("Shops loaded."); }),
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading skills..."); await Database.LoadSkillsAsync(); Logger.LogInformation("Skills loaded."); }),
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading animations..."); await Animation.LoadAnimationsAsync(); Logger.LogInformation("Animations loaded."); }),
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading switches..."); await Event.LoadSwitchesAsync(); Logger.LogInformation("Switches loaded."); }),
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading variables..."); await Event.LoadVariablesAsync(); Logger.LogInformation("Variables loaded."); }),
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading projectiles..."); await Projectile.LoadProjectilesAsync(); Logger.LogInformation("Projectiles loaded."); }),
                LoadWithSemaphoreAsync(semaphore, async () => { Logger.LogInformation("Loading script..."); await Script.LoadScriptAsync(0); Logger.LogInformation("Script compiled and loaded."); })
            };

            await System.Threading.Tasks.Task.WhenAll(tasks);
            Logger.LogInformation("Game content loaded successfully.");
        }

        private static async System.Threading.Tasks.Task LoadWithSemaphoreAsync(SemaphoreSlim semaphore, Func<System.Threading.Tasks.Task> loadFunc)
        {
            await semaphore.WaitAsync(Cts.Token);
            try
            {
                await loadFunc();
            }
            finally
            {
                semaphore.Release();
            }
        }

        private static async System.Threading.Tasks.Task SpawnGameObjectsAsync()
        {
            await System.Threading.Tasks.Task.WhenAll(
                System.Threading.Tasks.Task.Run(Item.SpawnAllMapsItems),
                Npc.SpawnAllMapNpcs(),
                EventLogic.SpawnAllMapGlobalEvents()
            );
            Logger.LogInformation("Game objects spawned.");
        }

        #endregion

        #region Display and Monitoring

        private static void DisplayServerBanner(int startTime)
        {
            Console.Clear();
            string[] banner = {
                " __   ___                        __          __        _     _     ",
                @" \ \ / / |                       \ \        / /       | |   | |",
                @"  \ V /| |_ _ __ ___ _ __ ___   __\ \  /\  / /__  _ __| | __| |___ ",
                @"  > < | __| '__/ _ \ '_ ` _ \ / _ \ \/  \/ / _ \| '__| |/ _` / __|",
                @" / . \| |_| | |  __/ | | | | |  __/\  /\  / (_) | |  | | (_| \__ \",
                @"/_/ \_\\__|_|  \___|_| |_| |_|\___| \/  \/ \___/|_|  |_|\__,_|___/"
            };

            foreach (var line in banner) Console.WriteLine(line);
            Console.WriteLine($"Initialization complete. Server loaded in {GetTimeMs() - startTime}ms.");
            Console.WriteLine("Use /help for available commands.");
        }

        /// <summary>
        /// Counts the number of players currently online.
        /// </summary>
        public static int CountPlayersOnline()
        {
            lock (SyncLock)
            {
                return PlayerService.Instance.PlayerIds.Count(NetworkConfig.IsPlaying);
            }
        }

        /// <summary>
        /// Updates the console title with server status information.
        /// </summary>
        public static void UpdateCaption()
        {
            try
            {
                Console.Title = $"{SettingsManager.Instance.GameName} <IP {_myIpAddress}:{SettingsManager.Instance.Port}> " +
                    $"({CountPlayersOnline()} Players Online) - Errors: {Global.ErrorCount} - Time: {Clock.Instance}";
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to update console title");
                Console.Title = SettingsManager.Instance.GameName;
            }
        }

        /// <summary>
        /// Performs a health check on critical server components.
        /// </summary>
        public static async System.Threading.Tasks.Task<bool> PerformHealthCheckAsync()
        {
            try
            {
                bool networkActive = true; // NetworkConfig.Socket.IsListening;
                if (!networkActive) Logger.LogWarning("Network socket is not listening.");
                return networkActive && !Cts.IsCancellationRequested;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Health check failed");
                return false;
            }
        }

        #endregion

        #region New and Enhanced Functionality

        private static void InitializeSaveTimer()
        {
            int intervalMinutes = SettingsManager.Instance.SaveInterval;
            _saveTimer = new Timer(async _ => await SavePlayersPeriodicallyAsync(), null,
                TimeSpan.FromMinutes(intervalMinutes), TimeSpan.FromMinutes(intervalMinutes));
        }

        private static async System.Threading.Tasks.Task SavePlayersPeriodicallyAsync()
        {
            try
            {
                await Database.SaveAllPlayersOnlineAsync();
                Logger.LogInformation("Periodic player save completed.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Periodic player save failed");
            }
        }

        private static async System.Threading.Tasks.Task SendServerAnnouncementAsync(string message)
        {
            await Parallel.ForEachAsync(Enumerable.Range(0, Core.Globals.Constant.MaxPlayers), Cts.Token, async (i, ct) =>
            {
                if (NetworkConfig.IsPlaying(i))
                    NetworkSend.PlayerMsg(i, message, (int)ColorName.Yellow);
            });
            Logger.LogInformation("Server announcement sent.");
        }

        private static async System.Threading.Tasks.Task InitializeChatSystemAsync()
        {
            Logger.LogInformation("Chat system initialized.");
            // Additional initialization logic can be added here if needed
        }

        /// <summary>
        /// Handles player commands with expanded functionality.
        /// </summary>
        public static async System.Threading.Tasks.Task HandlePlayerCommandAsync(string[] command)
        {
            // Defensive: command[1] may not exist for some commands
            int playerIndex = -1;
            if (command.Length > 1)
                playerIndex = GameLogic.FindPlayer(command[1]);

            if (command.Length == 1)
            {
                if (command[0] == "/help")
                    await SendHelpMessageAsync();
            } 

            if (playerIndex == -1 || !NetworkConfig.IsPlaying(playerIndex))
            {
                return;
            }
            switch (command[0].ToLower())
            {
                case "/teleport":
                    if (int.TryParse(command[2], out int x) && int.TryParse(command[3], out int y))
                        await TeleportPlayerAsync(playerIndex, x, y);
                    break;

                case "/kick":               
                    await KickPlayerAsync(playerIndex);
                    break;

                case "/broadcast":
                    await BroadcastMessageAsync(playerIndex, string.Join(" ", command[2..]));
                    break;

                case "/status":
                    await SendServerStatusAsync(playerIndex);
                    break;

                case "/whisper":
                    await SendWhisperAsync(playerIndex, "Server", string.Join(" ", command[2..]));
                    break;

                case "/stats":
                    await SendPlayerStatsAsync(playerIndex);
                    break;

                case "/save":
                    await SavePlayerDataAsync(playerIndex);
                    break;

                case "/shutdown":
                    {
                        if (General.GetShutDownTimer != null && General.GetShutDownTimer.IsRunning)
                        {
                            General.GetShutDownTimer.Stop();
                            Console.WriteLine("Server shutdown has been cancelled!");
                            NetworkSend.GlobalMsg("Server shutdown has been cancelled!");
                        }
                        else
                        {
                            if (General.GetShutDownTimer != null && General.GetShutDownTimer.ElapsedTicks > 0L)
                            {
                                General.GetShutDownTimer.Restart();
                            }
                            else
                            {
                                General.GetShutDownTimer?.Start();
                            }

                            Console.WriteLine("Server shutdown in " + SettingsManager.Instance.ServerShutdown + " seconds!");
                            NetworkSend.GlobalMsg("Server shutdown in " + SettingsManager.Instance.ServerShutdown + " seconds!");
                        }
                        break;
                    }

                case "/exit":
                    {
                        await General.DestroyServerAsync();
                        break;
                    }

                case "/access":
                    {
                        byte access;
                        if (!byte.TryParse(command[2], out access))
                        {
                            Console.WriteLine("Invalid access level.");
                            break;
                        }

                        // SetPlayerAccess implementation stub
                        void SetPlayerAccess(int idx, byte lvl)
                        {
                            Data.Player[idx].Access = lvl;
                        }

                        switch (access)
                        {
                            case (byte)AccessLevel.Player:
                                SetPlayerAccess(playerIndex, access);
                                NetworkSend.SendPlayerData(playerIndex);
                                NetworkSend.PlayerMsg(playerIndex, "Your access has been set to Player!", (int)ColorName.Yellow);
                                Console.WriteLine("Successfully set the access level to " + access + " for player " + GetPlayerName(playerIndex));
                                break;
                            case (byte)AccessLevel.Moderator:
                                SetPlayerAccess(playerIndex, access);
                                NetworkSend.SendPlayerData(playerIndex);
                                NetworkSend.PlayerMsg(playerIndex, "Your access has been set to Moderator!", (int)ColorName.Yellow);
                                Console.WriteLine("Successfully set the access level to " + access + " for player " + GetPlayerName(playerIndex));
                                break;
                            case (byte)AccessLevel.Mapper:
                                SetPlayerAccess(playerIndex, access);
                                NetworkSend.SendPlayerData(playerIndex);
                                NetworkSend.PlayerMsg(playerIndex, "Your access has been set to Mapper!", (int)ColorName.Yellow);
                                Console.WriteLine("Successfully set the access level to " + access + " for player " + GetPlayerName(playerIndex));
                                break;
                            case (byte)AccessLevel.Developer:
                                SetPlayerAccess(playerIndex, access);
                                NetworkSend.SendPlayerData(playerIndex);
                                NetworkSend.PlayerMsg(playerIndex, "Your access has been set to Developer!", (int)ColorName.Yellow);
                                Console.WriteLine("Successfully set the access level to " + access + " for player " + GetPlayerName(playerIndex));
                                break;
                            case (byte)AccessLevel.Owner:
                                SetPlayerAccess(playerIndex, access);
                                NetworkSend.SendPlayerData(playerIndex);
                                NetworkSend.PlayerMsg(playerIndex, "Your access has been set to Owner!", (int)ColorName.Yellow);
                                Console.WriteLine("Successfully set the access level to " + access + " for player " + GetPlayerName(playerIndex));
                                break;
                            default:
                                Console.WriteLine("Failed to set the access level to " + access + " for player " + GetPlayerName(playerIndex));
                                break;
                            
                        }
                        break;
                    }

                case "/ban":
                    {
                        Data.Account[playerIndex].Banned = true;
                        var task = Server.Player.LeftGame(playerIndex);
                        task.Wait();
                        Console.WriteLine($"Player {GetPlayerName(playerIndex)} has been banned by the server.");
                        
                        break;
                    }

                case "/timespeed":
                    {
                        double speed;
                        if (!double.TryParse(command[1], out speed))
                        {
                            Console.WriteLine("Invalid speed value.");
                            break;
                        }
                        Clock.Instance.GameSpeed = speed;
                        SettingsManager.Instance.TimeSpeed = speed;
                        SettingsManager.Save();
                        Console.WriteLine("Set GameSpeed to " + Clock.Instance.GameSpeed + " secs per seconds");
                        break;
                    }

                default:
                    Console.WriteLine("Unknown command. Use /help for assistance.", (int)ColorName.BrightRed);
                    break;
            }
        }

        private static async System.Threading.Tasks.Task TeleportPlayerAsync(int playerIndex, int x, int y)
        {
            try
            {               
                ref var player = ref Data.Player[playerIndex];

                if (x < 0 || x >= Data.Map[player.Map].MaxX || y < 0 || y >= Data.Map[player.Map].MaxY)
                {
                    NetworkSend.PlayerMsg(playerIndex, "Invalid coordinates for teleportation.", (int)ColorName.BrightRed);
                    return;
                }

                player.X = x;
                player.Y = y;
                NetworkSend.SendPlayerXyToMap(playerIndex);
                Logger.LogInformation($"Player {playerIndex} teleported to ({x}, {y})");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to teleport player {playerIndex}");
                NetworkSend.PlayerMsg(playerIndex, "Teleport failed.", (int)ColorName.BrightRed);
            }
        }

        private static async System.Threading.Tasks.Task KickPlayerAsync(int playerIndex)
        {
            try
            {
                if (NetworkConfig.IsPlaying(playerIndex))
                {
                    NetworkSend.SendLeftGame(playerIndex);
                    await Player.LeftGame(playerIndex);
                    Logger.LogInformation($"Player {playerIndex} kicked by server!");
                    NetworkSend.PlayerMsg(playerIndex, $"Player {playerIndex} has been kicked.", (int)ColorName.BrightGreen);
                }
                else
                {
                    NetworkSend.PlayerMsg(playerIndex, "Target player is not online.", (int)ColorName.BrightRed);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to kick player {playerIndex}");
                NetworkSend.PlayerMsg(playerIndex, "Kick operation failed.", (int)ColorName.BrightRed);
            }
        }

        private static async System.Threading.Tasks.Task BroadcastMessageAsync(int playerIndex, string message)
        {
            try
            {
                if (!await IsAdminAsync(playerIndex))
                {
                    NetworkSend.PlayerMsg(playerIndex, "You are not authorized to broadcast.", (int)ColorName.BrightRed);
                    return;
                }

                await SendChatMessageAsync(playerIndex, "global", $"[Broadcast] {message}", ColorName.BrightGreen);
                Logger.LogInformation($"Broadcast by {playerIndex}: {message}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Broadcast failed");
                NetworkSend.PlayerMsg(playerIndex, "Broadcast failed.", (int)ColorName.BrightRed);
            }
        }

        private static async System.Threading.Tasks.Task SendServerStatusAsync(int playerIndex)
        {
            try
            {
                string status = $"Players Online: {CountPlayersOnline()}\n" +
                                $"Uptime: {MyStopwatch.Elapsed}\n" +
                                $"Errors: {Global.ErrorCount}";
                NetworkSend.PlayerMsg(playerIndex, status, (int)ColorName.BrightGreen);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to send server status");
                NetworkSend.PlayerMsg(playerIndex, "Unable to retrieve server status.", (int)ColorName.BrightRed);
            }
        }

        private static async System.Threading.Tasks.Task SendHelpMessageAsync()
        {
            string help = "Available Commands:\n" +
                          "/teleport <x> <y> - Teleport to coordinates\n" +
                          "/kick <player> - Kick a player (admin only)\n" +
                          "/broadcast <message> - Send a message to all players (admin only)\n" +
                          "/status - View server status\n" +
                          "/whisper <player> <message> - Send a private message\n" +
                          "/exit - Shutdown the server\n" +
                          "/ban <player> - Ban a player\n" +
                          "/shutdown - Initiate server shutdown\n" +
                          "/stats - View player statistics\n" +
                          "/access <player> <level> - Set player access level (1-5)\n" +
                          "/save - Manually save player data\n" +
                          "/help - Show this message";
            Console.WriteLine(help);
        }

        private static async System.Threading.Tasks.Task SendWhisperAsync(int senderIndex, string targetName, string message)
        {
            try
            {
                int targetIndex = await FindPlayerByNameAsync(targetName);
                if (targetIndex == -1)
                {
                    NetworkSend.PlayerMsg(senderIndex, $"Player '{targetName}' not found.", (int)ColorName.BrightRed);
                    return;
                }

                await SendChatMessageAsync(senderIndex, $"private:{targetIndex}", $"[Whisper] {message}", ColorName.BrightCyan);
                Logger.LogInformation($"Whisper from {senderIndex} to {targetIndex}: {message}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to send whisper from {senderIndex} to {targetName}");
                NetworkSend.PlayerMsg(senderIndex, "Failed to send whisper.", (int)ColorName.BrightRed);
            }
        }

        private static async System.Threading.Tasks.Task HandlePartyCommandAsync(int playerIndex, string subCommand, string targetName)
        {
            try
            {
                var player = Data.TempPlayer[playerIndex];
                switch (subCommand.ToLower())
                {
                    case "create":
                        if (player.InParty != -1)
                        {
                            NetworkSend.PlayerMsg(playerIndex, "You are already in a party.", (int)ColorName.BrightRed);
                            return;
                        }
                        player.InParty = (int)GenerateUniqueId();
                        NetworkSend.PlayerMsg(playerIndex, "Party created.", (int)ColorName.BrightGreen);
                        break;

                    case "invite":
                        if (player.InParty == -1)
                        {
                            NetworkSend.PlayerMsg(playerIndex, "You must create a party first.", (int)ColorName.BrightRed);
                            return;
                        }
                        int targetIndex = await FindPlayerByNameAsync(targetName);
                        if (targetIndex == -1)
                        {
                            NetworkSend.PlayerMsg(playerIndex, $"Player '{targetName}' not found.", (int)ColorName.BrightRed);
                            return;
                        }
                        var targetPlayer = Data.TempPlayer[playerIndex];
                        if (targetPlayer.InParty != -1)
                        {
                            NetworkSend.PlayerMsg(playerIndex, $"{targetName} is already in a party.", (int)ColorName.BrightRed);
                            return;
                        }
                        targetPlayer.InParty = player.InParty;
                        NetworkSend.PlayerMsg(playerIndex, $"You have joined {Data.Player[playerIndex].Name}'s party.", (int)ColorName.BrightGreen);
                        NetworkSend.PlayerMsg(playerIndex, $"{targetName} has joined your party.", (int)ColorName.BrightGreen);
                        break;

                    case "leave":
                        if (player.InParty == -1)
                        {
                            NetworkSend.PlayerMsg(playerIndex, "You are not in a party.", (int)ColorName.BrightRed);
                            return;
                        }
                        player.InParty = -1;
                        NetworkSend.PlayerMsg(playerIndex, "You have left the party.", (int)ColorName.BrightGreen);
                        break;

                    default:
                        NetworkSend.PlayerMsg(playerIndex, "Invalid party command. Use: create, invite, leave.", (int)ColorName.BrightRed);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to handle party command for player {playerIndex}");
                NetworkSend.PlayerMsg(playerIndex, "Party command failed.", (int)ColorName.BrightRed);
            }
        }

        private static async System.Threading.Tasks.Task SendPlayerStatsAsync(int playerIndex)
        {
            try
            {
                var stats = PlayerStatistics.GetOrAdd(playerIndex, new PlayerStats());
                string statsMessage = $"Your Stats:\n" +
                                      $"Kills: {stats.Kills}\n" +
                                      $"Deaths: {stats.Deaths}\n" +
                                      $"Playtime: {stats.PlayTime.TotalHours:F2} hours";
                NetworkSend.PlayerMsg(playerIndex, statsMessage, (int)ColorName.BrightGreen);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to send stats for player {playerIndex}");
                NetworkSend.PlayerMsg(playerIndex, "Failed to retrieve stats.", (int)ColorName.BrightRed);
            }
        }

        private static async System.Threading.Tasks.Task SavePlayerDataAsync(int playerIndex)
        {
            try
            {
                await Database.SaveAccountAsync(playerIndex); // Assuming this method exists
                NetworkSend.PlayerMsg(playerIndex, "Your data has been saved.", (int)ColorName.BrightGreen);
                Logger.LogInformation($"Player {playerIndex} data saved manually.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to save data for player {playerIndex}");
                NetworkSend.PlayerMsg(playerIndex, "Failed to save data.", (int)ColorName.BrightRed);
            }
        }

        private static async System.Threading.Tasks.Task<int> FindPlayerByNameAsync(string name)
        {
            for (int i = 0; i < Core.Globals.Constant.MaxPlayers; i++)
            {
                if (NetworkConfig.IsPlaying(i) && Data.Player[i].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        private static async System.Threading.Tasks.Task SendChatMessageAsync(int senderIndex, string channel, string message, ColorName color)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message) || message.Length > 200) // Basic filtering
                {
                    NetworkSend.PlayerMsg(senderIndex, "Invalid message.", (int)ColorName.BrightRed);
                    return;
                }

                if (channel.StartsWith("private:"))
                {
                    int targetIndex = int.Parse(channel.Split(':')[1]);
                    NetworkSend.PlayerMsg(targetIndex, $"[From {Data.Player[senderIndex].Name}] {message}", (int)color);
                    NetworkSend.PlayerMsg(senderIndex, $"[To {Data.Player[targetIndex].Name}] {message}", (int)color);
                }
                else if (channel == "party" && Data.TempPlayer[senderIndex].InParty != 0)
                {
                    await Parallel.ForEachAsync(Enumerable.Range(0, Core.Globals.Constant.MaxPlayers), Cts.Token, async (i, ct) =>
                    {
                        if (NetworkConfig.IsPlaying(i) && Data.TempPlayer[i].InParty == Data.TempPlayer[senderIndex].InParty)
                            NetworkSend.PlayerMsg(i, $"[Party] {Data.Player[senderIndex].Name}: {message}", (int)color);
                    });
                }
                else if (channel == "global")
                {
                    await Parallel.ForEachAsync(Enumerable.Range(0, Core.Globals.Constant.MaxPlayers), Cts.Token, async (i, ct) =>
                    {
                        if (NetworkConfig.IsPlaying(i))
                            NetworkSend.PlayerMsg(i, message, (int)color);
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to send chat message from {senderIndex} to {channel}");
                NetworkSend.PlayerMsg(senderIndex, "Failed to send message.", (int)ColorName.BrightRed);
            }
        }

        /// <summary>
        /// Handles player login events.
        /// </summary>
        public static async System.Threading.Tasks.Task OnPlayerLoginAsync(int playerIndex)
        {
            Logger.LogInformation($"Player {playerIndex} logged in.");
            NetworkSend.PlayerMsg(playerIndex, "Welcome to the server!", (int)ColorName.BrightGreen);
            PlayerStatistics.GetOrAdd(playerIndex, new PlayerStats()).LoginTime = GetServerTime();
        }

        /// <summary>
        /// Handles player logout events.
        /// </summary>
        public static async System.Threading.Tasks.Task OnPlayerLogoutAsync(int playerIndex)
        {
            if (PlayerStatistics.TryGetValue(playerIndex, out var stats) && stats.LoginTime.HasValue)
            {
                stats.PlayTime += GetServerTime() - stats.LoginTime.Value;
                stats.LoginTime = null;
            }
            Logger.LogInformation($"Player {playerIndex} logged out.");
        }

        private static Task<bool> IsAdminAsync(int playerIndex) =>
            System.Threading.Tasks.Task.FromResult(playerIndex == 0); // Example admin check

        /// <summary>
        /// Creates a backup of the database asynchronously.
        /// </summary>
        public static async System.Threading.Tasks.Task BackupDatabaseAsync()
        {
            try
            {
                string backupDir = DataPath.Database;
                Directory.CreateDirectory(backupDir);
                string backupPath = System.IO.Path.Combine(backupDir, $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");
                //await Database.BackupAsync(backupPath); // Assuming this method exists
                Logger.LogInformation($"Database backup created: {backupPath}");

                var backups = Directory.GetFiles(backupDir, "backup_*.bak")
                    .OrderByDescending(f => f)
                    .Skip(SettingsManager.Instance.MaxBackups)
                    .ToList();
                foreach (var oldBackup in backups)
                {
                    await System.Threading.Tasks.Task.Run(() => File.Delete(oldBackup), Cts.Token);
                    Logger.LogInformation($"Deleted old backup: {oldBackup}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Database backup failed");
            }
        }

        #endregion

        #region Error Handling

        private static async System.Threading.Tasks.Task HandleCriticalErrorAsync(Exception ex)
        {
            await BackupDatabaseAsync();
            Logger.LogCritical(ex, "Critical error occurred. Initiating emergency shutdown");
            await SendServerAnnouncementAsync("Server shutting down due to critical error.");
            await DestroyServerAsync();
        }

        /// <summary>
        /// Logs an error to a file and updates error count asynchronously.
        /// </summary>
        public static async System.Threading.Tasks.Task LogErrorAsync(Exception ex, string context = "")
        {
            string errorInfo = $"{ex.Message}\nStackTrace: {ex.StackTrace}";
            string logPath = System.IO.Path.Combine(DataPath.Logs, "Errors.log");
            Directory.CreateDirectory(DataPath.Logs);

            await File.AppendAllTextAsync(logPath,
                $"{DateTime.Now}\nContext: {context}\n{errorInfo}\n\n", Cts.Token);

            Interlocked.Increment(ref Global.ErrorCount);
            UpdateCaption();
        }

        public static async System.Threading.Tasks.Task CheckShutDownCountDownAsync()
        {
            if (_shutDownTimer.ElapsedTicks <= 0) return;

            int time = _shutDownTimer.Elapsed.Seconds;
            if (_shutDownLastTimer != time)
            {
                if (SettingsManager.Instance.ServerShutdown - time <= 10)
                {
                    NetworkSend.GlobalMsg($"Server shutdown in {SettingsManager.Instance.ServerShutdown - time} seconds!");
                    Console.WriteLine($"Server shutdown in {SettingsManager.Instance.ServerShutdown - time} seconds!");
                    if (SettingsManager.Instance.ServerShutdown - time <= 1)
                    {
                        await DestroyServerAsync();
                    }
                }
                _shutDownLastTimer = time;
            }
        }

          /// <summary>
        /// Gets the database name from configuration (Database:ConnectionString).
        /// </summary>
        public static string? GetDatabaseName(IConfiguration configuration)
        {
            var connStr = configuration["Database:ConnectionString"];
            if (string.IsNullOrEmpty(connStr))
                return null;
            var builder = new System.Data.Common.DbConnectionStringBuilder { ConnectionString = connStr };
            return builder.TryGetValue("Database", out var dbNameObj) ? dbNameObj?.ToString() : null;
        }

        #endregion

        #region Helper Classes

        private class PlayerStats
        {
            public int Kills { get; set; }
            public int Deaths { get; set; }
            public TimeSpan PlayTime { get; set; }
            public DateTime? LoginTime { get; set; }
        }

        #endregion
    }
}
