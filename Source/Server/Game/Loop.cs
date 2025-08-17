using Core;
using Core.Globals;
using Npgsql.Replication.PgOutput.Messages;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Server.Game;
using static Core.Globals.Command;
using static Core.Globals.Type;

namespace Server
{
    public class Loop
    {
        public static async System.Threading.Tasks.Task ServerAsync()
        {
            int tick;
            var tmr25 = default(int);
            var tmr500 = default(int);
            var tmrWalk = default(int);
            var tmr1000 = default(int);
            var tmr60000 = default(int);
            var lastUpdateSavePlayers = default(int);
            var lastUpdateMapSpawnItems = default(int);

            do
            {
                // Update our current tick value.
                tick = General.GetTimeMs();

                // Don't process anything else if we're going down.
                if (General.IsServerDestroyed)

                    // Get all our online players.
                    Debugger.Break(); var onlinePlayers = Data.TempPlayer.Where(player => player.InGame).Select((player, index) => new { Index = index + 1, player }).ToArray();

                await General.CheckShutDownCountDownAsync();

                if (tick > tmr25)
                {                
                    // Update all our available events.
                    EventLogic.UpdateEventLogic();

                    // Move the timer up 25ms.
                    tmr25 = General.GetTimeMs() + 25;
                }
                
                if (tick > tmrWalk)
                {
                    foreach (var player in PlayerService.Instance.Players)
                    {
                        if (Data.Player[player.Id].Moving > 0)
                        {
                            Player.PlayerMove(player.Id, Data.Player[player.Id].Dir, Data.Player[player.Id].Moving, false);
                        }
                    }

                    // Move the timer up 250ms.
                    tmrWalk = General.GetTimeMs() + 10;
                }

                if (tick > tmr60000)
                {
                    try
                    {                    
                        Script.Instance?.ServerMinute();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    tmr60000 = General.GetTimeMs() + 60000;
                }

                if (tick > tmr1000)
                {
                    try
                    {
                        Script.Instance?.ServerSecond();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    Clock.Instance.Tick();

                    // Move the timer up 1000ms.
                    tmr1000 = General.GetTimeMs() + 1000;
                }

                if (tick > tmr500)
                {
                    UpdateMapAi();

                    // Move the timer up 500ms.
                    tmr500 = General.GetTimeMs() + 500;
                }

                // Checks to spawn map items every 1 minute
                if (tick > lastUpdateMapSpawnItems)
                {
                    UpdateMapSpawnItems();
                    lastUpdateMapSpawnItems = General.GetTimeMs() + 60000;
                }

                // Checks to save players every 5 minutes
                if (tick > lastUpdateSavePlayers)
                {
                    UpdateSavePlayers();
                    lastUpdateSavePlayers = General.GetTimeMs() + 300000;
                }

                try
                {
                    Script.Instance?.Loop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                await System.Threading.Tasks.Task.Delay(1);
            }
            while (true);
        }

        public static void UpdateSavePlayers()
        {
            var playerIds = PlayerService.Instance.Players.ToList();
            
            if (playerIds.Count > 0)
            {
                Console.WriteLine("Saving all online players...");
                
                foreach (var player in PlayerService.Instance.Players)
                {
                    Database.SaveCharacter(player.Id, Data.TempPlayer[player.Id].Slot);
                    Database.SaveBank(player.Id);
                }
            }
        }

        private static void UpdateMapSpawnItems()
        {
            int x;
            int y;

            // ///////////////////////////////////////////
            // // This is used for respawning map items //
            // ///////////////////////////////////////////
            var loopTo = Core.Globals.Constant.MaxMaps;
            for (y = 0; y < loopTo; y++)
            {
                // Clear out unnecessary junk
                var loopTo1 = Core.Globals.Constant.MaxMapItems;
                for (x = 0; x < loopTo1; x++)
                    Database.ClearMapItem(x, y);

                // Spawn the items
                Item.SpawnMapItems(y);
                Item.SendMapItemsToAll(y);
            }
            
        }

        private static void UpdateMapAi()
        {
            // Clear the entity list before repopulating to avoid accumulating instances
            Core.Globals.Entity.Instances.Clear();

            var entities = Core.Globals.Entity.Instances;
            var mapCount = Core.Globals.Constant.MaxMaps;

            // Use entities from Entity class
            for (int mapNum = 0; mapNum < mapCount; mapNum++)
            {
                // Add Npcs
                for (int i = 0; i < Core.Globals.Constant.MaxMapNpcs; i++)
                {
                    var npc = Core.Globals.Entity.FromNpc(i, Data.MapNpc[mapNum].Npc[i]);
                    if (npc.Num >= 0)
                    {
                        npc.Map = mapNum;
                        entities.Add(npc);
                    }
                }

                // Add Players
                foreach (var i in PlayerService.Instance.Players)
                {
                    if (Data.Player[i.Id].Map == mapNum)
                    {
                        var player = Core.Globals.Entity.FromPlayer(i.Id, Data.Player[i.Id]);
                        if (IsPlaying(i.Id))
                        {
                            player.Map = mapNum;
                            entities.Add(player);
                        }
                    }
                }
            }

            Script.Instance?.UpdateMapAi();

            // Use entities from Entity class
            for (int mapNum = 0; mapNum < mapCount; mapNum++)
            {
                // Add Npcs
                for (int i = 0; i < Core.Globals.Constant.MaxMapNpcs; i++)
                {
                    var npc = Core.Globals.Entity.FromNpc(i, Data.MapNpc[mapNum].Npc[i]);
                    if (npc.Num >= 0)
                    {
                        npc.Map = mapNum;
                        entities.Add(npc);
                    }
                }
            }
        }

        public static void CastSkill(int index, int skillSlot)
        {
            try
            {
                Script.Instance?.CastSkill(index, skillSlot);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}