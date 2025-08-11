using Core;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.Json;
using Core.Globals;
using Core.Net;
using Server.Game;
using Server.Game.Net;
using Server.Net;
using static Core.Globals.Command;
using static Core.Net.Packets;
using static Core.Globals.Type;
using EventCommand = Core.Globals.EventCommand;
using Type = Core.Globals.Type;

namespace Server
{
    public class Event
    {
        #region Globals

        public static GlobalEvents[] TempEventMap = new GlobalEvents[Core.Globals.Constant.MaxMaps + 1];
        public static string[] Switches = new string[Core.Globals.Constant.MaxSwitches];
        public static string[] Variables = new string[Core.Globals.Constant.MaxVariables];
        private static readonly ConcurrentBag<ScheduledEvent> ScheduledEvents = new ConcurrentBag<ScheduledEvent>();
        private static readonly object TempEventLock = new object();

        internal const int PathfindingType = 0; // 0: None, 1: Random, 2: BFS (existing), 3: A* (new)

        // Effect Constants
        internal const int EffectTypeFadein = 2;
        internal const int EffectTypeFadeout = 0;
        internal const int EffectTypeFlash = 3;
        internal const int EffectTypeFog = 4;
        internal const int EffectTypeWeather = 5;
        internal const int EffectTypeTint = 6;
        internal const int EffectTypeScreenShake = 7; // New effect

        #endregion

        #region Database

        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public static void CreateSwitches()
        {
            Switches = new string[Core.Globals.Constant.MaxSwitches];
            Array.Fill(Switches, string.Empty);
            SaveSwitches();
            General.Logger.LogInformation("Switches initialized and saved.");
        }

        public static void CreateVariables()
        {
            Variables = new string[Core.Globals.Constant.MaxVariables];
            Array.Fill(Variables, string.Empty);
            SaveVariables();
            General.Logger.LogInformation("Variables initialized and saved.");
        }

        public static void SaveSwitches()
        {
            try
            {
                var jsonPath = System.IO.Path.Combine(DataPath.Database, "Switches.json");
                var json = JsonSerializer.Serialize(Switches, options);
                
                File.WriteAllText(jsonPath, json);
            }
            catch (Exception ex)
            {
                General.Logger.LogError(ex, "Failed to save Switches.");
                throw;
            }
        }

        public static void SaveVariables()
        {
            try
            {
                var jsonPath = System.IO.Path.Combine(DataPath.Database, "Variables.json");
                var json = JsonSerializer.Serialize(Variables, options);
                
                File.WriteAllText(jsonPath, json);
            }
            catch (Exception ex)
            {
                General.Logger.LogError(ex, "Failed to save Variables.");
                throw;
            }
        }

        public static async System.Threading.Tasks.Task LoadSwitchesAsync()
        {
            try
            {
                var jsonPath = System.IO.Path.Combine(DataPath.Database, "Switches.json");
                var json = await File.ReadAllTextAsync(jsonPath);
                
                Switches = JsonSerializer.Deserialize<string[]>(json, options) ?? [];
                
                if (Switches.Length != Core.Globals.Constant.MaxSwitches)
                {
                    General.Logger.LogWarning("Switches.json not found or invalid. Creating new switches.");
                    CreateSwitches();
                }
            }
            catch (Exception ex)
            {
                General.Logger.LogError(ex, "Failed to load Switches.json. Creating new switches.");
                
                CreateSwitches();
            }
        }

        public static async System.Threading.Tasks.Task LoadVariablesAsync()
        {
            try
            {
                var jsonPath = System.IO.Path.Combine(DataPath.Database, "Variables.json");
                var json = await File.ReadAllTextAsync(jsonPath);
                
                Variables = JsonSerializer.Deserialize<string[]>(json, options) ?? [];

                if (Variables.Length != Core.Globals.Constant.MaxVariables)
                {
                    General.Logger.LogWarning("Variables.json not found or invalid. Creating new variables.");
                    CreateVariables();
                }
            }
            catch (Exception ex)
            {
                General.Logger.LogError(ex, "Failed to load Variables.json. Creating new variables.");
                CreateVariables();
            }
        }

        #endregion

        #region Movement

        // Helper methods for CanEventMove
        private static bool IsTileWalkable(int mapNum, int x, int y)
        {
            if (x < 0 || x > Data.Map[mapNum].MaxX || y < 0 || y > Data.Map[mapNum].MaxY) return false;
            var tile = Data.Map[mapNum].Tile[x, y];
            return tile.Type != TileType.Blocked && tile.Type2 != TileType.Blocked &&
                   (tile.Type == TileType.Item || tile.Type == TileType.NpcSpawn ||
                    tile.Type2 == TileType.Item || tile.Type2 == TileType.NpcSpawn);
        }

        private static bool IsPlayerBlocking(int index, int mapNum, int x, int y, int eventId)
        {
            foreach (var i in PlayerService.Instance.PlayerIds)
            {
                if (NetworkConfig.IsPlaying(i) && GetPlayerMap(i) == mapNum && GetPlayerX(i) == x && GetPlayerY(i) == y)
                {
                    if (Data.Map[mapNum].Event[eventId].Pages[Data.TempPlayer[index].EventMap.EventPages[eventId].PageId].Trigger == 1)
                    {
                        StartEventProcessing(index, eventId, mapNum);
                    }

                    return true;
                }
            }

            return false;
        }

        private static void StartEventProcessing(int index, int eventId, int mapNum)
        {
            var pageId = Data.TempPlayer[index].EventMap.EventPages[eventId].PageId;
            if (Data.Map[mapNum].Event[eventId].Pages[pageId].CommandListCount <= 0) return;

            var processing = Data.TempPlayer[index].EventProcessing[eventId];
            processing.Active = 0;
            processing.ActionTimer = General.GetTimeMs();
            processing.CurList = 0;
            processing.CurSlot = 0;
            processing.EventId = eventId;
            processing.PageId = pageId;
            processing.WaitingForResponse = 0;
            processing.ListLeftOff = new int[Data.Map[mapNum].Event[eventId].Pages[pageId].CommandListCount];
        }

        private static bool IsNpcBlocking(int mapNum, int x, int y)
        {
            for (var i = 0; i < Core.Globals.Constant.MaxMapNpcs; i++)
            {
                if (Data.MapNpc[mapNum].Npc[i].X == x && Data.MapNpc[mapNum].Npc[i].Y == y)
                    return true;
            }

            return false;
        }

        private static bool IsDirectionBlocked(int mapNum, int x, int y, byte dir) =>
            IsDirBlocked(Data.Map[mapNum].Tile[x, y].DirBlock, (Direction)dir);

        public static bool CanEventMove(int index, int mapNum, int x, int y, int eventId, int walkThrough, byte dir, bool globalEvent = false)
        {
            if (!IsValidMapAndDirection(mapNum, dir)) return false;

            int targetX = x, targetY = y;
            switch (dir)
            {
                case (byte) Direction.Up: targetY--; break;
                case (byte) Direction.Down: targetY++; break;
                case (byte) Direction.Left: targetX--; break;
                case (byte) Direction.Right: targetX++; break;
                default: return false;
            }

            if (targetX < 0 || targetX > Data.Map[mapNum].MaxX || targetY < 0 || targetY > Data.Map[mapNum].MaxY) return false;
            if (walkThrough == 1) return true;

            return IsTileWalkable(mapNum, targetX, targetY) &&
                   !IsPlayerBlocking(index, mapNum, targetX, targetY, eventId) &&
                   !IsNpcBlocking(mapNum, targetX, targetY) &&
                   !IsDirectionBlocked(mapNum, x, y, dir);
        }

        private static bool IsValidMapAndDirection(int mapNum, byte dir) =>
            mapNum >= 0 && mapNum < Core.Globals.Constant.MaxMaps && dir >= 0 && dir <= System.Enum.GetValues(typeof(Direction)).Length;

        public static void EventDir(int playerIndex, int mapNum, int eventId, int dir, bool globalEvent = false)
        {
            if (!IsValidMapAndDirection(mapNum, (byte) dir)) return;

            var eventIndex = GetEventIndex(playerIndex, eventId, globalEvent);
            if (eventIndex == -1) return;

            lock (TempEventLock)
            {
                if (globalEvent)
                {
                    if (Data.Map[mapNum].Event[eventId].Pages[0].DirFix == 0)
                        TempEventMap[mapNum].Event[eventId].Dir = dir;
                }
                else if (Data.Map[mapNum].Event[eventId].Pages[Data.TempPlayer[playerIndex].EventMap.EventPages[eventIndex].PageId].DirFix == 0)
                    Data.TempPlayer[playerIndex].EventMap.EventPages[eventIndex].Dir = dir;
            }

            SendEventDirection(mapNum, eventId, globalEvent ? TempEventMap[mapNum].Event[eventId].Dir : Data.TempPlayer[playerIndex].EventMap.EventPages[eventIndex].Dir);
        }

        private static int GetEventIndex(int playerIndex, int eventId, bool globalEvent)
        {
            if (globalEvent) return eventId;
            if (Data.TempPlayer[playerIndex].EventMap.CurrentEvents <= 0) return -1;

            for (var i = 0; i < Data.TempPlayer[playerIndex].EventMap.CurrentEvents; i++)
            {
                if (eventId == i)
                    return i;
            }

            return -1;
        }

        private static void SendEventDirection(int mapNum, int eventId, int currentDir)
        {
            var packetWriter = new PacketWriter(12);

            packetWriter.WriteEnum(ServerPackets.SEventDir);
            packetWriter.WriteInt32(eventId);
            packetWriter.WriteInt32(currentDir);

            NetworkConfig.SendDataToMap(mapNum, packetWriter.GetBytes());
        }

        public static void EventMove(int index, int mapNum, int eventId, int dir, int movementSpeed, bool globalEvent = false)
        {
            if (!IsValidMapAndDirection(mapNum, (byte) dir)) return;

            var eventIndex = GetEventIndex(index, eventId, globalEvent);
            if (eventIndex == -1) return;

            lock (TempEventLock)
            {
                if (globalEvent)
                {
                    var eventData = TempEventMap[mapNum].Event[eventIndex];
                    if (Data.Map[mapNum].Event[eventId].Pages[0].DirFix == 0)
                        eventData.Dir = dir;

                    switch (dir)
                    {
                        case (byte) Direction.Up: eventData.Y--; break;
                        case (byte) Direction.Down: eventData.Y++; break;
                        case (byte) Direction.Left: eventData.X--; break;
                        case (byte) Direction.Right: eventData.X++; break;
                    }

                    SendEventMove(mapNum, eventId, eventData.X, eventData.Y, dir, eventData.Dir, movementSpeed, 0);
                }
                else
                {
                    var eventData = Data.TempPlayer[index].EventMap.EventPages[eventIndex];
                    if (Data.Map[mapNum].Event[eventId].Pages[Data.TempPlayer[index].EventMap.EventPages[eventIndex].PageId].DirFix == 0)
                        eventData.Dir = dir;

                    switch (dir)
                    {
                        case (byte) Direction.Up: eventData.Y--; break;
                        case (byte) Direction.Down: eventData.Y++; break;
                        case (byte) Direction.Left: eventData.X--; break;
                        case (byte) Direction.Right: eventData.X++; break;
                    }

                    SendEventMove(mapNum, eventId, eventData.X, eventData.Y, dir, eventData.Dir, movementSpeed, index);
                }
            }
        }

        private static void SendEventMove(int mapNum, int eventId, int x, int y, int dir, int currentDir, int speed, int index = -1)
        {
            var packetWriter = new PacketWriter(24);

            packetWriter.WriteEnum(ServerPackets.SEventMove);
            packetWriter.WriteInt32(eventId);
            packetWriter.WriteInt32(x);
            packetWriter.WriteInt32(y);
            packetWriter.WriteInt32(dir);
            packetWriter.WriteInt32(currentDir);
            packetWriter.WriteInt32(speed);

            if (index == -1)
            {
                NetworkConfig.SendDataToMap(mapNum, packetWriter.GetBytes());
            }
            else
            {
                PlayerService.Instance.SendDataTo(index, packetWriter.GetBytes());
            }
        }

        public static bool IsOneBlockAway(int x1, int y1, int x2, int y2) =>
            (x1 == x2 && (y1 == y2 - 1 || y1 == y2 + 1)) || (y1 == y2 && (x1 == x2 - 1 || x1 == x2 + 1));

        public static byte GetNpcDir(int x, int y, int x1, int y1)
        {
            byte direction = (int) Direction.Right;
            var maxDistance = 0;
            UpdateDirectionAndDistance(x - x1, (int) Direction.Right, (int) Direction.Left, ref direction, ref maxDistance);
            UpdateDirectionAndDistance(y - y1, (int) Direction.Down, (int) Direction.Up, ref direction, ref maxDistance);
            return direction;
        }

        private static void UpdateDirectionAndDistance(int diff, int posDir, int negDir, ref byte direction, ref int maxDistance)
        {
            var absDiff = Math.Abs(diff);
            if (absDiff > maxDistance)
            {
                direction = (byte) (diff > 0 ? posDir : negDir);
                maxDistance = absDiff;
            }
        }

        public static int CanEventMoveTowardsPlayer(int playerId, int mapNum, int eventId)
        {
            if (!IsValidPlayerEvent(playerId, mapNum, eventId)) return 4; // Invalid direction as failure

            var (px, py, ex, ey, walkThrough) = GetPlayerAndEventPositions(playerId, mapNum, eventId);
            return PathfindingType switch
            {
                1 => RandomMoveTowardsPlayer(playerId, mapNum, eventId, ex, ey, px, py, walkThrough),
                2 => BfsMoveTowardsPlayer(playerId, mapNum, eventId, ex, ey, px, py, walkThrough),
                3 => AStarMoveTowardsPlayer(playerId, mapNum, eventId, ex, ey, px, py, walkThrough), // New A* pathfinding
                _ => RandomDirection()
            };
        }

        private static bool IsValidPlayerEvent(int playerId, int mapNum, int eventId) =>
            playerId >= 0 && playerId < Core.Globals.Constant.MaxPlayers &&
            mapNum >= 0 && mapNum < Core.Globals.Constant.MaxMaps &&
            eventId >= 0 && eventId < Data.TempPlayer[playerId].EventMap.CurrentEvents;

        private static (int px, int py, int ex, int ey, int walkThrough) GetPlayerAndEventPositions(int playerId, int mapNum, int eventId)
        {
            int px = GetPlayerX(playerId), py = GetPlayerY(playerId);
            var eventPage = Data.TempPlayer[playerId].EventMap.EventPages[eventId];
            return (px, py, eventPage.X, eventPage.Y,
                Data.Map[mapNum].Event[eventPage.EventId].Pages[eventPage.PageId].WalkThrough);
        }

        private static int RandomMoveTowardsPlayer(int playerId, int mapNum, int eventId, int ex, int ey, int px, int py, int walkThrough)
        {
            var i = Random.Shared.Next(0, 4);
            foreach (var dir in GetDirectionOrder(i))
            {
                if (ShouldMoveTowards(ex, ey, px, py, dir) && CanEventMove(playerId, mapNum, ex, ey, eventId, walkThrough, (byte) dir, false))
                {
                    return dir;
                }
            }

            return RandomDirection();
        }

        private static IEnumerable<int> GetDirectionOrder(int start) =>
            Enumerable.Range(0, 4).Select(i => (start + i) % 4);

        private static bool ShouldMoveTowards(int ex, int ey, int px, int py, int dir) =>
            dir switch
            {
                (int) Direction.Up => ey > py,
                (int) Direction.Down => ey < py,
                (int) Direction.Left => ex > px,
                (int) Direction.Right => ex < px,
                _ => false
            };

        private static int BfsMoveTowardsPlayer(int playerId, int mapNum, int eventId, int ex, int ey, int px, int py, int walkThrough)
        {
            // Existing BFS implementation (simplified here for brevity)
            var queue = new Queue<(int x, int y)>();
            var visited = new HashSet<(int, int)>();
            var parent = new Dictionary<(int, int), (int, int)>();
            queue.Enqueue((ex, ey));
            visited.Add((ex, ey));

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();
                if (x == px && y == py)
                {
                    var current = (x, y);
                    while (parent[current] != (ex, ey))
                        current = parent[current];
                    return GetDirectionFromStep(ex, ey, current.Item1, current.Item2);
                }

                foreach (var (dx, dy, dir) in new[] {(0, -1, (int) Direction.Up), (0, 1, (int) Direction.Down), (-1, 0, (int) Direction.Left), (1, 0, (int) Direction.Right)})
                {
                    int nx = x + dx, ny = y + dy;
                    if (IsValidMove(playerId, mapNum, eventId, nx, ny, walkThrough, visited))
                    {
                        queue.Enqueue((nx, ny));
                        visited.Add((nx, ny));
                        parent[(nx, ny)] = (x, y);
                    }
                }
            }

            return 4; // No path found
        }

        private static bool IsValidMove(int playerId, int mapNum, int eventId, int x, int y, int walkThrough, HashSet<(int, int)> visited) =>
            x >= 0 && x <= Data.Map[mapNum].MaxX && y >= 0 && y <= Data.Map[mapNum].MaxY &&
            !visited.Contains((x, y)) && CanEventMove(playerId, mapNum, x, y, eventId, walkThrough, 0, false);

        private static int GetDirectionFromStep(int ex, int ey, int nx, int ny) =>
            nx > ex ? (int) Direction.Right : nx < ex ? (int) Direction.Left : ny > ey ? (int) Direction.Down : (int) Direction.Up;

        // New A* Pathfinding
        private static int AStarMoveTowardsPlayer(int playerId, int mapNum, int eventId, int ex, int ey, int px, int py, int walkThrough)
        {
            var openSet = new PriorityQueue<(int x, int y, int fScore)>(Comparer<(int x, int y, int fScore)>.Create((a, b) => a.fScore.CompareTo(b.fScore)));
            var cameFrom = new Dictionary<(int, int), (int, int)>();
            var gScore = new Dictionary<(int, int), int> {[(ex, ey)] = 0};
            var fScore = new Dictionary<(int, int), int> {[(ex, ey)] = Heuristic(ex, ey, px, py)};
            openSet.Enqueue((ex, ey, fScore[(ex, ey)]));

            while (openSet.Count > 0)
            {
                var (x, y, _) = openSet.Dequeue();
                if (x == px && y == py)
                {
                    var current = (x, y);
                    while (cameFrom[current] != (ex, ey))
                        current = cameFrom[current];
                    return GetDirectionFromStep(ex, ey, current.Item1, current.Item2);
                }

                foreach (var (dx, dy, dir) in new[] {(0, -1, (int) Direction.Up), (0, 1, (int) Direction.Down), (-1, 0, (int) Direction.Left), (1, 0, (int) Direction.Right)})
                {
                    int nx = x + dx, ny = y + dy;
                    if (!IsWithinMapBounds(mapNum, nx, ny) || !CanEventMove(playerId, mapNum, x, y, eventId, walkThrough, (byte) dir, false)) continue;

                    var tentativeGScore = gScore[(x, y)] + 1;
                    if (!gScore.ContainsKey((nx, ny)) || tentativeGScore < gScore[(nx, ny)])
                    {
                        cameFrom[(nx, ny)] = (x, y);
                        gScore[(nx, ny)] = tentativeGScore;
                        fScore[(nx, ny)] = gScore[(nx, ny)] + Heuristic(nx, ny, px, py);
                        openSet.Enqueue((nx, ny, fScore[(nx, ny)]));
                    }
                }
            }

            return 4; // No path found
        }

        private static bool IsWithinMapBounds(int mapNum, int x, int y) =>
            x >= 0 && x <= Data.Map[mapNum].MaxX && y >= 0 && y <= Data.Map[mapNum].MaxY;

        private static int Heuristic(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

        private static int RandomDirection() => General.GetRandom.NextInt(0, 4);

        public static int CanEventMoveAwayFromPlayer(int playerId, int mapNum, int eventId)
        {
            if (!IsValidPlayerEvent(playerId, mapNum, eventId)) return 5;

            var (px, py, ex, ey, walkThrough) = GetPlayerAndEventPositions(playerId, mapNum, eventId);
            var i = General.GetRandom.NextInt(0, 4);
            foreach (var dir in GetDirectionOrder(i))
            {
                if (ShouldMoveAway(ex, ey, px, py, dir) && CanEventMove(playerId, mapNum, ex, ey, eventId, walkThrough, (byte) dir, false))
                    return dir;
            }

            return RandomDirection();
        }

        private static bool ShouldMoveAway(int ex, int ey, int px, int py, int dir) =>
            dir switch
            {
                (int) Direction.Up => ey < py,
                (int) Direction.Down => ey > py,
                (int) Direction.Left => ex < px,
                (int) Direction.Right => ex > px,
                _ => false
            };

        public static int GetDirToPlayer(int playerId, int mapNum, int eventId)
        {
            if (!IsValidPlayerEvent(playerId, mapNum, eventId)) return (int) Direction.Right;
            var (px, py, ex, ey, _) = GetPlayerAndEventPositions(playerId, mapNum, eventId);
            return GetNpcDir(ex, ey, px, py);
        }

        public static int GetDirAwayFromPlayer(int playerId, int mapNum, int eventId)
        {
            if (!IsValidPlayerEvent(playerId, mapNum, eventId)) return (int) Direction.Right;
            var (px, py, ex, ey, _) = GetPlayerAndEventPositions(playerId, mapNum, eventId);
            byte direction = (int) Direction.Right;
            var maxDistance = 0;
            UpdateDirectionAndDistance(px - ex, (int) Direction.Left, (int) Direction.Right, ref direction, ref maxDistance);
            UpdateDirectionAndDistance(py - ey, (int) Direction.Up, (int) Direction.Down, ref direction, ref maxDistance);
            return direction;
        }

        // New Movement Behaviors
        public static void PatrolEvent(int index, int mapNum, int eventId, List<(int x, int y)> patrolPath, int speed, bool globalEvent = false)
        {
            if (!patrolPath.Any()) return;
            var currentStep = TempEventMap[mapNum].Event[eventId].PatrolStep % patrolPath.Count;
            var (targetX, targetY) = patrolPath[currentStep];
            var dir = GetDirectionToTarget(TempEventMap[mapNum].Event[eventId].X, TempEventMap[mapNum].Event[eventId].Y, targetX, targetY);
            if (CanEventMove(index, mapNum, TempEventMap[mapNum].Event[eventId].X, TempEventMap[mapNum].Event[eventId].Y, eventId, 0, (byte) dir, globalEvent))
            {
                EventMove(index, mapNum, eventId, dir, speed, globalEvent);
                if (TempEventMap[mapNum].Event[eventId].X == targetX && TempEventMap[mapNum].Event[eventId].Y == targetY)
                    TempEventMap[mapNum].Event[eventId].PatrolStep++;
            }
        }

        private static int GetDirectionToTarget(int x, int y, int tx, int ty) =>
            tx > x ? (int) Direction.Right : tx < x ? (int) Direction.Left : ty > y ? (int) Direction.Down : (int) Direction.Up;

        public static void FollowPlayer(int index, int mapNum, int eventId, int targetPlayerId, int speed, bool globalEvent = false)
        {
            var dir = CanEventMoveTowardsPlayer(targetPlayerId, mapNum, eventId);
            if (dir != 4)
                EventMove(index, mapNum, eventId, dir, speed, globalEvent);
        }

        #endregion

        #region Incoming Packets

        public static void Packet_EventChatReply(GameSession session, ReadOnlyMemory<byte> bytes)
        {
            var buffer = new PacketReader(bytes);
            int eventId = buffer.ReadInt32(), pageId = buffer.ReadInt32(), reply = buffer.ReadInt32();

            General.Logger.LogInformation($"Player {session.Id} responded to event {eventId} with reply {reply}");
            ProcessEventReply(session.Id, eventId, pageId, reply);
        }

        private static void ProcessEventReply(int index, int eventId, int pageId, int reply)
        {
            for (var i = 0; i < Data.TempPlayer[index].EventProcessingCount; i++)
            {
                var proc = Data.TempPlayer[index].EventProcessing[i];
                if (proc.EventId != eventId || proc.PageId != pageId || proc.WaitingForResponse != 1) continue;

                var cmd = Data.Map[GetPlayerMap(index)].Event[eventId].Pages[pageId].CommandList[proc.CurList].Commands[proc.CurSlot - 1];
                if (reply == 0 && cmd.Index == (byte) EventCommand.ShowText)
                    proc.WaitingForResponse = 0;
                else if (reply > 0 && cmd.Index == (byte) EventCommand.ShowChoices)
                    UpdateEventProcessing(index, i, reply, cmd);
            }
        }

        private static void UpdateEventProcessing(int index, int procIndex, int reply, Type.EventCommand cmd)
        {
            var proc = Data.TempPlayer[index].EventProcessing[procIndex];
            proc.ListLeftOff[proc.CurList] = proc.CurSlot - 1;
            proc.CurList = reply switch
            {
                1 => cmd.Data1,
                2 => cmd.Data2,
                3 => cmd.Data3,
                4 => cmd.Data4,
                _ => proc.CurList
            };
            proc.CurSlot = 0;
            proc.WaitingForResponse = 0;
        }

        public static void Packet_Event(GameSession session, ReadOnlyMemory<byte> bytes)
        {
            var buffer = new PacketReader(bytes);
            var eventId = buffer.ReadInt32();
            EventLogic.TriggerEvent(session.Id, eventId, 0, GetPlayerX(session.Id), GetPlayerY(session.Id));
        }

        public static void Packet_RequestSwitchesAndVariables(GameSession session, ReadOnlyMemory<byte> bytes) => SendSwitchesAndVariables(session.Id);

        public static void Packet_SwitchesAndVariables(GameSession session, ReadOnlyMemory<byte> bytes)
        {
            var buffer = new PacketReader(bytes);
            for (var i = 0; i < Core.Globals.Constant.MaxSwitches; i++) Switches[i] = buffer.ReadString();
            for (var i = 0; i < Core.Globals.Constant.MaxVariables; i++) Variables[i] = buffer.ReadString();

            SaveSwitches();
            SaveVariables();
            SendSwitchesAndVariables(0, true);
        }

        #endregion

        #region Outgoing Packets

        public static void SendSpecialEffect(int index, int effectType, int data1 = 0, int data2 = 0, int data3 = 0, int data4 = 0)
        {
            var buffer = new PacketWriter(24);

            buffer.WriteEnum(ServerPackets.SSpecialEffect);
            buffer.WriteInt32(effectType);

            switch (effectType)
            {
                case EffectTypeFadein:
                case EffectTypeFadeout:
                case EffectTypeFlash:
                    break;
                case EffectTypeFog:
                    buffer.WriteInt32(data1); // Fog number
                    buffer.WriteInt32(data2); // Movement speed
                    buffer.WriteInt32(data3); // Opacity
                    break;
                case EffectTypeWeather:
                    buffer.WriteInt32(data1); // Weather type
                    buffer.WriteInt32(data2); // Intensity
                    break;
                case EffectTypeTint:
                    buffer.WriteInt32(data1); // Red
                    buffer.WriteInt32(data2); // Green
                    buffer.WriteInt32(data3); // Blue
                    buffer.WriteInt32(data4); // Alpha
                    break;
                case EffectTypeScreenShake:
                    buffer.WriteInt32(data1); // Intensity
                    buffer.WriteInt32(data2); // Duration
                    break;
                default:
                    General.Logger.LogWarning($"Unknown effect type {effectType} sent to player {index}");
                    return;
            }

            PlayerService.Instance.SendDataTo(index, buffer.GetBytes());
        }

        public static void SendSwitchesAndVariables(int index, bool everyone = false)
        {
            var buffer = new PacketWriter(4 + (Core.Globals.Constant.MaxSwitches + Core.Globals.Constant.MaxVariables) * 256);
            buffer.WriteEnum(ServerPackets.SSwitchesAndVariables);
            for (var i = 0; i < Core.Globals.Constant.MaxSwitches; i++) buffer.WriteString(Switches[i]);
            for (var i = 0; i < Core.Globals.Constant.MaxVariables; i++) buffer.WriteString(Variables[i]);

            if (everyone)
            {
                PlayerService.Instance.SendDataToAll(buffer.GetBytes());
            }
            else
            {
                PlayerService.Instance.SendDataTo(index, buffer.GetBytes());
            }
        }

        public static void SendMapEventData(int index)
        {
            var buffer = new PacketWriter(4);

            var mapNum = GetPlayerMap(index);

            buffer.WriteEnum(ServerPackets.SMapEventData);
            buffer.WriteInt32(Data.Map[mapNum].EventCount);

            if (Data.Map[mapNum].EventCount > 0)
            {
                SerializeMapEvents(buffer, mapNum);
            }

            PlayerService.Instance.SendDataTo(index, buffer.GetBytes());

            SendSwitchesAndVariables(index);
        }

        private static void SerializeMapEvents(PacketWriter buffer, int mapNum)
        {
            for (var i = 0; i < Data.Map[mapNum].EventCount; i++)
            {
                var ev = Data.Map[mapNum].Event[i];

                buffer.WriteString(ev.Name);
                buffer.WriteByte(ev.Globals);
                buffer.WriteInt32(ev.X);
                buffer.WriteInt32(ev.Y);
                buffer.WriteInt32(ev.PageCount);

                if (ev.PageCount > 0)
                    SerializeEventPages(buffer, mapNum, i, ev.PageCount);
            }
        }

        private static void SerializeEventPages(PacketWriter buffer, int mapNum, int eventIndex, int pageCount)
        {
            for (var x = 0; x < pageCount; x++)
            {
                var page = Data.Map[mapNum].Event[eventIndex].Pages[x];
                SerializePageConditions(buffer, page);
                SerializePageGraphics(buffer, page);
                SerializePageMovement(buffer, page);
                SerializePageCommands(buffer, mapNum, eventIndex, x, page);
            }
        }

        private static void SerializePageConditions(PacketWriter buffer, EventPage page)
        {
            buffer.WriteInt32(page.ChkVariable);
            buffer.WriteInt32(page.VariableIndex);
            buffer.WriteInt32(page.VariableCondition);
            buffer.WriteInt32(page.VariableCompare);
            buffer.WriteInt32(page.ChkSwitch);
            buffer.WriteInt32(page.SwitchIndex);
            buffer.WriteInt32(page.SwitchCompare);
            buffer.WriteInt32(page.ChkHasItem);
            buffer.WriteInt32(page.HasItemIndex);
            buffer.WriteInt32(page.HasItemAmount);
            buffer.WriteInt32(page.ChkSelfSwitch);
            buffer.WriteInt32(page.SelfSwitchIndex);
            buffer.WriteInt32(page.SelfSwitchCompare);
        }

        private static void SerializePageGraphics(PacketWriter packetWriter, EventPage page)
        {
            packetWriter.WriteByte(page.GraphicType);
            packetWriter.WriteInt32(page.Graphic);
            packetWriter.WriteInt32(page.GraphicX);
            packetWriter.WriteInt32(page.GraphicY);
            packetWriter.WriteInt32(page.GraphicX2);
            packetWriter.WriteInt32(page.GraphicY2);
        }

        private static void SerializePageMovement(PacketWriter packetWriter, EventPage page)
        {
            packetWriter.WriteByte(page.MoveType);
            packetWriter.WriteByte(page.MoveSpeed);
            packetWriter.WriteByte(page.MoveFreq);
            packetWriter.WriteInt32(page.MoveRouteCount);
            packetWriter.WriteInt32(page.IgnoreMoveRoute);
            packetWriter.WriteInt32(page.RepeatMoveRoute);

            if (page.MoveRouteCount > 0)
            {
                for (var y = 0; y < page.MoveRouteCount; y++)
                {
                    ref var route = ref page.MoveRoute[y];

                    packetWriter.WriteInt32(route.Index);
                    packetWriter.WriteInt32(route.Data1);
                    packetWriter.WriteInt32(route.Data2);
                    packetWriter.WriteInt32(route.Data3);
                    packetWriter.WriteInt32(route.Data4);
                    packetWriter.WriteInt32(route.Data5);
                    packetWriter.WriteInt32(route.Data6);
                }
            }

            packetWriter.WriteInt32(page.WalkAnim);
            packetWriter.WriteInt32(page.DirFix);
            packetWriter.WriteInt32(page.WalkThrough);
            packetWriter.WriteInt32(page.ShowName);
            packetWriter.WriteByte(page.Trigger);
            packetWriter.WriteInt32(page.CommandListCount);
            packetWriter.WriteByte(page.Position);
        }

        private static void SerializePageCommands(PacketWriter buffer, int mapNum, int eventIndex, int pageIndex, EventPage page)
        {
            if (page.CommandListCount <= 0) return;
            for (var y = 0; y < page.CommandListCount; y++)
            {
                var cmdList = Data.Map[mapNum].Event[eventIndex].Pages[pageIndex].CommandList[y];
                buffer.WriteInt32(cmdList.CommandCount);
                buffer.WriteInt32(cmdList.ParentList);
                if (cmdList.CommandCount > 0)
                {
                    for (var z = 0; z < cmdList.CommandCount; z++)
                    {
                        var cmd = cmdList.Commands[z];

                        SerializeCommand(buffer, cmd);
                    }
                }
            }
        }

        private static void SerializeCommand(PacketWriter buffer, Type.EventCommand cmd)
        {
            buffer.WriteInt32(cmd.Index);
            buffer.WriteString(cmd.Text1);
            buffer.WriteString(cmd.Text2);
            buffer.WriteString(cmd.Text3);
            buffer.WriteString(cmd.Text4);
            buffer.WriteString(cmd.Text5);
            buffer.WriteInt32(cmd.Data1);
            buffer.WriteInt32(cmd.Data2);
            buffer.WriteInt32(cmd.Data3);
            buffer.WriteInt32(cmd.Data4);
            buffer.WriteInt32(cmd.Data5);
            buffer.WriteInt32(cmd.Data6);
            buffer.WriteInt32(cmd.ConditionalBranch.CommandList);
            buffer.WriteInt32(cmd.ConditionalBranch.Condition);
            buffer.WriteInt32(cmd.ConditionalBranch.Data1);
            buffer.WriteInt32(cmd.ConditionalBranch.Data2);
            buffer.WriteInt32(cmd.ConditionalBranch.Data3);
            buffer.WriteInt32(cmd.ConditionalBranch.ElseCommandList);
            buffer.WriteInt32(cmd.MoveRouteCount);
            if (cmd.MoveRouteCount > 0)
            {
                for (var w = 0; w < cmd.MoveRouteCount; w++)
                {
                    var route = cmd.MoveRoute[w];
                    buffer.WriteInt32(route.Index);
                    buffer.WriteInt32(route.Data1);
                    buffer.WriteInt32(route.Data2);
                    buffer.WriteInt32(route.Data3);
                    buffer.WriteInt32(route.Data4);
                    buffer.WriteInt32(route.Data5);
                    buffer.WriteInt32(route.Data6);
                }
            }
        }

        #endregion

        #region New Features

        // Scheduled Events
        public struct ScheduledEvent
        {
            public int EventId;
            public DateTime TriggerTime;
            public int MapNum;
        }

        public static void ScheduleEvent(int eventId, DateTime triggerTime, int mapNum)
        {
            ScheduledEvents.Add(new ScheduledEvent {EventId = eventId, TriggerTime = triggerTime, MapNum = mapNum});
            General.Logger.LogInformation($"Scheduled event {eventId} on map {mapNum} for {triggerTime}");
        }

        public static void CheckScheduledEvents()
        {
            var now = DateTime.Now;
            foreach (var ev in ScheduledEvents.ToList())
            {
                if (now >= ev.TriggerTime)
                {
                    TriggerScheduledEvent(ev);
                    ScheduledEvents.TryTake(out _);
                }
            }
        }

        private static void TriggerScheduledEvent(ScheduledEvent ev)
        {
            foreach (var i in PlayerService.Instance.PlayerIds)
            {
                if (NetworkConfig.IsPlaying(i) && GetPlayerMap(i) == ev.MapNum)
                    EventLogic.TriggerEvent(i, ev.EventId, 0, TempEventMap[ev.MapNum].Event[ev.EventId].X, TempEventMap[ev.MapNum].Event[ev.EventId].Y);
            }

            General.Logger.LogInformation($"Triggered scheduled event {ev.EventId} on map {ev.MapNum}");
        }

        // Action-Based Triggers
        public static void TriggerOnPlayerAction(int index, string actionType, int value)
        {
            var mapNum = GetPlayerMap(index);
            for (var i = 0; i < Data.Map[mapNum].EventCount; i++)
            {
                var page = Data.Map[mapNum].Event[i].Pages[Data.TempPlayer[index].EventMap.EventPages[i].PageId];
                if (page.ChkVariable == 1 && page.VariableIndex == GetActionVariableIndex(actionType) && page.VariableCompare == value)
                    EventLogic.TriggerEvent(index, i, 0, GetPlayerX(index), GetPlayerY(index));
            }
        }

        private static int GetActionVariableIndex(string actionType) =>
            actionType switch
            {
                "Kills" => 1,
                "ItemsCollected" => 2,
                _ => 0
            };

        // Environment Effects
        public static void ChangeMapWeather(int mapNum, int weatherType, int intensity)
        {
            foreach (var i in PlayerService.Instance.PlayerIds)
            {
                if (NetworkConfig.IsPlaying(i) && GetPlayerMap(i) == mapNum)
                    SendSpecialEffect(i, EffectTypeWeather, weatherType, intensity);
            }
        }

        #endregion

        #region Helper Classes

        // Simple Priority Queue for A* Pathfinding
        private class PriorityQueue<T>
        {
            private readonly List<T> _items = new List<T>();
            private readonly IComparer<T> _comparer;

            public PriorityQueue(IComparer<T> comparer) => this._comparer = comparer;
            public int Count => _items.Count;

            public void Enqueue(T item)
            {
                _items.Add(item);
                _items.Sort(_comparer);
            }

            public T Dequeue()
            {
                if (_items.Count == 0) throw new InvalidOperationException("Queue is empty");
                var item = _items[0];
                _items.RemoveAt(0);
                return item;
            }
        }

        #endregion
    }
}