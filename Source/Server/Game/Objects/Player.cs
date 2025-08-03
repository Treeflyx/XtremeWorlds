using Core;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Mirage.Sharp.Asfw;
using Mirage.Sharp.Asfw.Network;
using System;
using System.Data;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Autofac.Features.Indexed;
using Microsoft.Xna.Framework.Input;
using static Core.Global.Command;
using static Core.Packets;
using static Core.Type;

namespace Server
{

    public class Player
    {
        #region Data

        public static void CheckPlayerLevelUp(int index)
        {
            try
            {
                Script.Instance?.CheckPlayerLevelUp(index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion

        #region Incoming Packets

        public static void HandleUseChar(int index)
        {
            // Set the flag so we know the person is in the game
            Core.Data.TempPlayer[index].InGame = true;

            // Send an ok to client to start receiving in game data
            NetworkSend.SendLoginOk(index);
            JoinGame(index);
            string text = string.Format("{0} | {1} has began playing {2}.", GetAccountLogin(index), GetPlayerName(index), SettingsManager.Instance.GameName);
            Core.Log.Add(text, Constant.PlayerLog);
            Console.WriteLine(text);
            
        }

        #endregion

        #region Outgoing Packets

        public static void SendLeaveMap(int index, int mapNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) ServerPackets.SLeftMap);
            buffer.WriteInt32(index);
            NetworkConfig.SendDataToMapBut(index, mapNum, buffer.UnreadData, buffer.WritePosition);

            buffer.Dispose();
        }

        #endregion

        #region Movement
        public static void PlayerWarp(int index, int mapNum, int x, int y, int dir)
        {
            int oldMap;
            int i;
            ByteStream buffer;

            // Check for subscript out of range
            if (NetworkConfig.IsPlaying(index) == false | mapNum < 0 | mapNum > Core.Constant.MaxMaps)
                return;

            // Check if you are out of bounds
            if (x > Data.Map[mapNum].MaxX)
                x = Data.Map[mapNum].MaxX;

            if (y > Data.Map[mapNum].MaxY)
                y = Data.Map[mapNum].MaxY;

            x *= 32;
            y *= 32;

            Core.Data.TempPlayer[index].EventProcessingCount = 0;
            Core.Data.TempPlayer[index].EventMap.CurrentEvents = 0;

            // clear target
            Core.Data.TempPlayer[index].Target = 0;
            Core.Data.TempPlayer[index].TargetType = 0;
            NetworkSend.SendTarget(index, 0, 0);

            // clear events
            Core.Data.TempPlayer[index].EventMap.CurrentEvents = 0;

            // Save old map to send erase player data to
            oldMap = GetPlayerMap(index);

            if (oldMap != mapNum)
            {
                try
                {
                    Script.Instance?.LeaveMap(index, oldMap);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                SendLeaveMap(index, oldMap);
            }

            SetPlayerMap(index, mapNum);
            SetPlayerX(index, x);
            SetPlayerY(index, y);
            SetPlayerDir(index, dir);

            NetworkSend.SendPlayerXy(index);

            // send equipment of all people on new map
            if (GameLogic.GetTotalMapPlayers(mapNum) > 0)
            {
                var loopTo = NetworkConfig.Socket.HighIndex;
                for (i = 0; i < loopTo; i++)
                {
                    if (NetworkConfig.IsPlaying(i))
                    {
                        if (GetPlayerMap(i) == mapNum)
                        {
                            NetworkSend.SendMapEquipmentTo(i, index);
                        }
                    }
                }
            }

            // Now we check if there were any players left on the map the player just left, and if not stop processing npcs
            if (GameLogic.GetTotalMapPlayers(oldMap) == 0)
            {
                // Regenerate all Npcs' health
                var loopTo1 = Core.Constant.MaxMapNpcs;
                for (i = 0; i < loopTo1; i++)
                {
                    if (Data.MapNpc[oldMap].Npc[i].Num >= 0)
                    {
                        Data.MapNpc[oldMap].Npc[i].Vital[(byte) Vital.Health] = GameLogic.GetNpcMaxVital((int)Data.MapNpc[oldMap].Npc[i].Num, Vital.Health);
                    }

                }
            }

            // Sets it so we know to process npcs on the map
            Core.Data.TempPlayer[index].GettingMap = true;

            Moral.SendUpdateMoralTo(index, Data.Map[mapNum].Moral);

            buffer = new ByteStream(4);
            buffer.WriteInt32((int) ServerPackets.SCheckForMap);
            buffer.WriteInt32(mapNum);
            buffer.WriteInt32(Data.Map[mapNum].Revision);
            NetworkConfig.Socket.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

            buffer.Dispose();

        }

        public static void PlayerMove(int index, int dir, int movement, bool expectingWarp)
        {
            int mapNum;
            int x;
            int y;
            bool beginEvent;
            bool moved;
            var didWarp = default(bool);
            byte newMapX;
            byte newMapY;
            var vital = default(int);
            int color;
            var amount = default(int);

            // Check for subscript out of range
           if (dir < (int)Direction.Up || dir > (int)Direction.DownRight || movement < (int)MovementState.Standing || movement > (int)MovementState.Running)
            {
                return;
            }

            // Prevent player from moving if they have casted a skill
            if (Core.Data.TempPlayer[index].SkillBuffer >= 0)
            {
                NetworkSend.SendPlayerXy(index);
                return;
            }

            // Cant move if in the bank
            if (Core.Data.TempPlayer[index].InBank)
            {
                NetworkSend.SendPlayerXy(index);
                return;
            }

            // if stunned, stop them moving
            if (Core.Data.TempPlayer[index].StunDuration > 0)
            {
                NetworkSend.SendPlayerXy(index);
                return;
            }

            if (Core.Data.TempPlayer[index].InShop >= 0 || Core.Data.TempPlayer[index].InBank)
            {
                NetworkSend.SendPlayerXy(index);
                return;
            }

            SetPlayerDir(index, dir);
            moved = false;
            mapNum = GetPlayerMap(index);

            switch ((Direction)dir)
            {
                case Direction.Up:
                    if (GetPlayerY(index) > 0)
                    {
                        x = GetPlayerRawX(index);
                        y = GetPlayerRawY(index) - 1;

                        if (IsTileBlocked(index, mapNum, GetPlayerX(index), GetPlayerY(index), Direction.Up))
                        {
                            break;
                        }

                        SetPlayerY(index, GetPlayerRawY(index) - 1);
                        moved = true;

                        for (int i = 0, loopTo2 = Core.Data.TempPlayer[index].EventMap.CurrentEvents; i < loopTo2; i++)
                            EventLogic.TriggerEvent(index, i, 1, GetPlayerX(index), GetPlayerY(index));
                    }
                    else if (Data.Map[mapNum].Tile[GetPlayerX(index), GetPlayerY(index)].Type != TileType.NoCrossing && Data.Map[mapNum].Tile[GetPlayerX(index), GetPlayerY(index)].Type2 != TileType.NoCrossing)
                    {
                        if (Data.Map[GetPlayerMap(index)].Up > 0)
                        {
                            newMapY = Data.Map[Data.Map[GetPlayerMap(index)].Up].MaxY;
                            PlayerWarp(index, Data.Map[GetPlayerMap(index)].Up, GetPlayerX(index), newMapY, (int)Direction.Up);
                            didWarp = true;
                            moved = true;
                        }
                    }
                    break;

                case Direction.Down:
                    if (GetPlayerY(index) < Data.Map[mapNum].MaxY - 1)
                    {
                        x = GetPlayerRawX(index);
                        y = GetPlayerRawY(index) + 1;

                        if (IsTileBlocked(index, mapNum, GetPlayerX(index), GetPlayerY(index), Direction.Down))
                        {
                            break;
                        }

                        SetPlayerY(index, GetPlayerRawY(index) + 1);
                        moved = true;

                        for (int i = 0, loopTo1 = Core.Data.TempPlayer[index].EventMap.CurrentEvents; i < loopTo1; i++)
                            EventLogic.TriggerEvent(index, i, 1, GetPlayerX(index), GetPlayerY(index));
                    }
                    else if (Data.Map[GetPlayerMap(index)].Tile[GetPlayerX(index), GetPlayerY(index)].Type != TileType.NoCrossing && Data.Map[GetPlayerMap(index)].Tile[GetPlayerX(index), GetPlayerY(index)].Type2 != TileType.NoCrossing)
                    {
                        if (Data.Map[GetPlayerMap(index)].Down > 0)
                        {
                            PlayerWarp(index, Data.Map[GetPlayerMap(index)].Down, GetPlayerX(index), 0, (int)Direction.Down);
                            didWarp = true;
                            moved = true;
                        }
                    }
                    break;

                case Direction.Left:
                    if (GetPlayerX(index) > 0)
                    {
                        x = GetPlayerRawX(index) - 1;
                        y = GetPlayerRawY(index);

                        if (IsTileBlocked(index, mapNum, GetPlayerX(index), GetPlayerY(index), Direction.Left))
                        {
                            break;
                        }

                        SetPlayerX(index, GetPlayerRawX(index) - 1);
                        moved = true;

                        for (int i = 0, loopTo2 = Core.Data.TempPlayer[index].EventMap.CurrentEvents; i < loopTo2; i++)
                            EventLogic.TriggerEvent(index, i, 1, GetPlayerX(index), GetPlayerY(index));
                    }
                    else if (Data.Map[GetPlayerMap(index)].Tile[GetPlayerX(index), GetPlayerY(index)].Type != TileType.NoCrossing && Data.Map[GetPlayerMap(index)].Tile[GetPlayerX(index), GetPlayerY(index)].Type2 != TileType.NoCrossing)
                    {
                        if (Data.Map[GetPlayerMap(index)].Left > 0)
                        {
                            newMapX = Data.Map[Data.Map[GetPlayerMap(index)].Left].MaxX;
                            PlayerWarp(index, Data.Map[GetPlayerMap(index)].Left, newMapX, GetPlayerY(index), (int)Direction.Left);
                            didWarp = true;
                            moved = true;
                        }
                    }
                    break;

                case Direction.Right:
                    if (GetPlayerX(index) < Data.Map[mapNum].MaxX - 1)
                    {
                        x = GetPlayerRawX(index) + 1;
                        y = GetPlayerRawY(index);

                        if (IsTileBlocked(index, mapNum, GetPlayerX(index), GetPlayerY(index), Direction.Right))
                        {
                            break;
                        }

                        SetPlayerX(index, GetPlayerRawX(index) + 1);
                        moved = true;

                        for (int i = 0, loopTo3 = Core.Data.TempPlayer[index].EventMap.CurrentEvents; i < loopTo3; i++)
                            EventLogic.TriggerEvent(index, i, 1, GetPlayerX(index), GetPlayerY(index));
                    }
                    else if (Data.Map[GetPlayerMap(index)].Tile[GetPlayerX(index), GetPlayerY(index)].Type != TileType.NoCrossing && Data.Map[GetPlayerMap(index)].Tile[GetPlayerX(index), GetPlayerY(index)].Type2 != TileType.NoCrossing)
                    {
                        if (Data.Map[GetPlayerMap(index)].Right > 0)
                        {
                            PlayerWarp(index, Data.Map[GetPlayerMap(index)].Right, 0, GetPlayerY(index), (int)Direction.Right);
                            didWarp = true;
                            moved = true;
                        }
                    }
                    break;

                case Direction.UpRight:
                    if (GetPlayerY(index) > 0 && GetPlayerX(index) < Data.Map[mapNum].MaxX - 1)
                    {
                        x = GetPlayerRawX(index) + 1;
                        y = GetPlayerRawY(index) - 1;

                        if (IsTileBlocked(index, mapNum, GetPlayerX(index), GetPlayerY(index), Direction.UpRight))
                        {
                            break;
                        }

                        SetPlayerX(index, GetPlayerRawX(index) + 1);
                        SetPlayerY(index, GetPlayerRawY(index) - 1);
                        moved = true;

                        for (int i = 0, loopTo4 = Core.Data.TempPlayer[index].EventMap.CurrentEvents; i < loopTo4; i++)
                            EventLogic.TriggerEvent(index, i, 1, GetPlayerX(index), GetPlayerY(index));
                    }
                    break;

                case Direction.UpLeft:
                    if (GetPlayerY(index) > 0 && GetPlayerX(index) > 0)
                    {
                        x = GetPlayerX(index) - 1;
                        y = GetPlayerY(index) - 1;

                        if (IsTileBlocked(index, mapNum, GetPlayerX(index), GetPlayerY(index), Direction.UpLeft))
                        {
                            break;
                        }

                        SetPlayerX(index, GetPlayerRawX(index) - 1);
                        SetPlayerY(index, GetPlayerRawY(index) - 1);
                        moved = true;

                        for (int i = 0, loopTo5 = Core.Data.TempPlayer[index].EventMap.CurrentEvents; i < loopTo5; i++)
                            EventLogic.TriggerEvent(index, i, 1, GetPlayerX(index), GetPlayerY(index));
                    }
                    break;

                case Direction.DownRight:
                    if (GetPlayerY(index) < Data.Map[mapNum].MaxY - 1 && GetPlayerX(index) < Data.Map[mapNum].MaxX - 1)
                    {
                        x = GetPlayerX(index) + 1;
                        y = GetPlayerY(index) + 1;

                        if (IsTileBlocked(index, mapNum, GetPlayerX(index), GetPlayerY(index), Direction.DownRight))
                        {
                            break;
                        }

                        SetPlayerX(index, GetPlayerRawX(index) + 1);
                        SetPlayerY(index, GetPlayerRawY(index) + 1);
                        moved = true;

                        for (int i = 0, loopTo6 = Core.Data.TempPlayer[index].EventMap.CurrentEvents; i < loopTo6; i++)
                            EventLogic.TriggerEvent(index, i, 1, GetPlayerX(index), GetPlayerY(index));
                    }
                    break;

                case Direction.DownLeft:
                    if (GetPlayerY(index) < Data.Map[mapNum].MaxY - 1 && GetPlayerX(index) > 0)
                    {
                        x = GetPlayerX(index) - 1;
                        y = GetPlayerY(index) + 1;

                        if (IsTileBlocked(index, mapNum, GetPlayerX(index), GetPlayerY(index), Direction.DownLeft))
                        {
                            break;
                        }

                        SetPlayerX(index, GetPlayerRawX(index) - 1);
                        SetPlayerY(index, GetPlayerRawY(index) + 1);
                        moved = true;

                        for (int i = 0, loopTo7 = Core.Data.TempPlayer[index].EventMap.CurrentEvents; i < loopTo7; i++)
                            EventLogic.TriggerEvent(index, i, 1, GetPlayerX(index), GetPlayerY(index));
                    }
                    break;
            }

            if (GetPlayerX(index) >= 0 && GetPlayerY(index) >= 0 && GetPlayerX(index) < Data.Map[GetPlayerMap(index)].MaxX && GetPlayerY(index) < Data.Map[GetPlayerMap(index)].MaxY)
            {
                ref var withBlock = ref Data.Map[GetPlayerMap(index)].Tile[GetPlayerX(index), GetPlayerY(index)];
                mapNum = -1;
                x = 0;
                y = 0;

                // Check to see if the tile is a warp tile, and if so warp them
                if (withBlock.Type == TileType.Warp)
                {
                    mapNum = withBlock.Data1;
                    x = withBlock.Data2 * 32;
                    y = withBlock.Data3 * 32;
                }

                if (withBlock.Type2 == TileType.Warp)
                {
                    mapNum = withBlock.Data12;
                    x = withBlock.Data22;
                    y = withBlock.Data32;
                }

                if (mapNum >= 0)
                {
                    PlayerWarp(index, (int)mapNum, x, y, (int)Direction.Down);

                    didWarp = true;
                    moved = true;
                }

                x = -1;
                y = 0;

                // Check for a shop, and if so open it
                if (withBlock.Type == TileType.Shop)
                {
                    x = withBlock.Data1;
                }

                if (withBlock.Type2 == TileType.Shop)
                {
                    x = withBlock.Data12;
                }

                if (x >= 0) // shop exists?
                {
                    if (Strings.Len(Data.Shop[x].Name) > 0) // name exists?
                    {
                        NetworkSend.SendOpenShop(index, x);
                        Core.Data.TempPlayer[index].InShop = x; // stops movement and the like
                    }
                }

                // Check to see if the tile is a bank, and if so send bank
                if (withBlock.Type == TileType.Bank | withBlock.Type2 == TileType.Bank)
                {
                    NetworkSend.SendBank(index);
                    Core.Data.TempPlayer[index].InBank = true;
                    moved = true;
                }

                // Check if it's a heal tile
                if (withBlock.Type == TileType.Heal)
                {
                    vital = withBlock.Data1;
                    amount = withBlock.Data2;
                }

                if (withBlock.Type2 == TileType.Heal)
                {
                    vital = withBlock.Data12;
                    amount += withBlock.Data22;
                }

                if (vital > 0)
                {
                    if (!(GetPlayerVital(index, (Vital)vital) == GetPlayerMaxVital(index, (Vital)vital)))
                    {
                        if (vital == (byte)Vital.Health)
                        {
                            color = (int) Core.Color.BrightGreen;
                        }
                        else
                        {
                            color = (int) Core.Color.BrightBlue;
                        }

                        NetworkSend.SendActionMsg(GetPlayerMap(index), "+" + amount, color, (byte)Core.ActionMessageType.Scroll, GetPlayerX(index) * 32, GetPlayerY(index) * 32, 1);
                        SetPlayerVital(index, (Vital)vital, GetPlayerVital(index, (Vital)vital) + amount);
                        NetworkSend.PlayerMsg(index, "You feel rejuvenating forces coursing through your body.", (int) Core.Color.BrightGreen);
                        NetworkSend.SendVital(index, (Vital)vital);
                    }
                    moved = true;
                }

                // Check if it's a trap tile
                if (withBlock.Type == TileType.Trap)
                {
                    amount = withBlock.Data1;
                }

                if (withBlock.Type2 == TileType.Trap)
                {
                    amount += withBlock.Data12;
                }

                if (amount > 0)
                {
                    NetworkSend.SendActionMsg(GetPlayerMap(index), "-" + amount, (int) Core.Color.BrightRed, (byte)Core.ActionMessageType.Scroll, GetPlayerX(index) * 32, GetPlayerY(index) * 32, 1);
                    if (GetPlayerVital(index, (Vital)Vital.Health) - amount < 0)
                    {
                        KillPlayer(index);
                        NetworkSend.PlayerMsg(index, "You've been killed by a trap.", (int) Core.Color.BrightRed);
                    }
                    else
                    {
                        SetPlayerVital(index, (Vital)Vital.Health, GetPlayerVital(index, (Vital)Vital.Health) - amount);
                        NetworkSend.PlayerMsg(index, "You've been injured by a trap.", (int) Core.Color.BrightRed);
                        NetworkSend.SendVital(index, (Vital)Vital.Health);
                    }
                    moved = true;
                }

            }

            // They tried to hack
            if (moved == false | expectingWarp & !didWarp)
            {
                PlayerWarp(index, GetPlayerMap(index), GetPlayerX(index), GetPlayerY(index), (byte)Direction.Down);
            }

            x = GetPlayerX(index);
            y = GetPlayerY(index);

            if (moved)
            {
                Data.Player[index].IsMoving = true;
                NetworkSend.SendPlayerXyToMap(index);

                try
                {
                    Script.Instance?.PlayerMove(index);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (Core.Data.TempPlayer[index].EventMap.CurrentEvents > 0)
                {
                    for (int i = 0, loopTo8 = Core.Data.TempPlayer[index].EventMap.CurrentEvents; i < loopTo8; i++)
                    {
                        beginEvent = false;

                        if (Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId >= 0)
                        {
                            if ((int)Data.Map[GetPlayerMap(index)].Event[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].Globals == 1)
                            {
                                if (Data.Map[GetPlayerMap(index)].Event[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].X == x & Data.Map[GetPlayerMap(index)].Event[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].Y == y & (int)Data.Map[GetPlayerMap(index)].Event[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].Pages[Core.Data.TempPlayer[index].EventMap.EventPages[i].PageId].Trigger == 1 & Core.Data.TempPlayer[index].EventMap.EventPages[i].Visible == true)
                                    beginEvent = true;
                            }
                            else if (Core.Data.TempPlayer[index].EventMap.EventPages[i].X == x & Core.Data.TempPlayer[index].EventMap.EventPages[i].Y == y & (int)Data.Map[GetPlayerMap(index)].Event[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].Pages[Core.Data.TempPlayer[index].EventMap.EventPages[i].PageId].Trigger == 1 & Core.Data.TempPlayer[index].EventMap.EventPages[i].Visible == true)
                                beginEvent = true;
                          
                            if (beginEvent)
                            {
                                // Process this event, it is on-touch and everything checks out.
                                if (Data.Map[GetPlayerMap(index)].Event[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].Pages[Core.Data.TempPlayer[index].EventMap.EventPages[i].PageId].CommandListCount > 0)
                                {
                                    Core.Data.TempPlayer[index].EventProcessing[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].Active = 0;
                                    Core.Data.TempPlayer[index].EventProcessing[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].ActionTimer = General.GetTimeMs();
                                    Core.Data.TempPlayer[index].EventProcessing[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].CurList = 0;
                                    Core.Data.TempPlayer[index].EventProcessing[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].CurSlot = 0;
                                    Core.Data.TempPlayer[index].EventProcessing[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].EventId = Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId;
                                    Core.Data.TempPlayer[index].EventProcessing[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].PageId = Core.Data.TempPlayer[index].EventMap.EventPages[i].PageId;
                                    Core.Data.TempPlayer[index].EventProcessing[Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId].WaitingForResponse = 0;

                                    int eventId = Core.Data.TempPlayer[index].EventMap.EventPages[i].EventId;
                                    int pageId = Core.Data.TempPlayer[index].EventMap.EventPages[i].PageId;
                                    int commandListCount = Data.Map[GetPlayerMap(index)].Event[eventId].Pages[pageId].CommandListCount;

                                    Array.Resize(ref Core.Data.TempPlayer[index].EventProcessing[eventId].ListLeftOff, commandListCount);
                                }
                                beginEvent = false;
                            }
                        }
                    }
                }
            }
        }

        public static bool IsTileBlocked(int index, int mapNum, int x, int y, Direction dir)
        {      
            // Check for Npc and player blocking  
            var loopTo = NetworkConfig.Socket.HighIndex;
            for (int i = 0; i < loopTo; i++)
            {
                if (Data.Moral[Data.Map[mapNum].Moral].PlayerBlock)
                {
                    if (NetworkConfig.IsPlaying(i) & GetPlayerMap(i) == mapNum)
                    {
                        if (GetPlayerX(i) == x && GetPlayerY(i) == y)
                        {
                            return true;
                        }
                    }
                }
            }

            var loopTo2 = Core.Constant.MaxMapNpcs;
            for (int i = 0; i < loopTo2; i++)
            {
                if (Data.Moral[Data.Map[mapNum].Moral].NpcBlock)
                {
                    if (Data.MapNpc[mapNum].Npc[i].Num >= 0)
                    {
                        if (Data.MapNpc[mapNum].Npc[i].X == x && Data.MapNpc[mapNum].Npc[i].Y == y)
                        {
                            return true;
                        }
                    }
                }
            }

            // Check to make sure that the tile is walkable
            if (IsDirBlocked(ref Data.Map[mapNum].Tile[x, y].DirBlock, (byte)dir))
            {
                return true;
            }

            if (Data.Map[mapNum].Tile[x, y].Type == TileType.Blocked || Data.Map[mapNum].Tile[x, y].Type2 == TileType.Blocked)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Inventory

        public static int HasItem(int index, int itemNum)
        {
            int hasItemRet = default;
            int i;

            // Check for subscript out of range
            if (itemNum < 0 | itemNum > Core.Constant.MaxItems)
            {
                return hasItemRet;
            }

            var loopTo = Core.Constant.MaxInv;
            for (i = 0; i < loopTo; i++)
            {
                // Check to see if the player has the item
                if (GetPlayerInv(index, i) == itemNum)
                {
                    if (Core.Data.Item[itemNum].Type == (byte)ItemCategory.Currency | Core.Data.Item[itemNum].Stackable == 1)
                    {
                        hasItemRet += GetPlayerInvValue(index, i);
                    }
                    else
                    {
                        hasItemRet += 1;
                    }
                }
            }

            return hasItemRet;

        }

        public static int FindItemSlot(int index, int itemNum)
        {
            int findItemSlotRet = default;
            int i;

            findItemSlotRet = 0;

            // Check for subscript out of range
            if (itemNum < 0 | itemNum > Core.Constant.MaxItems)
            {
                return findItemSlotRet;
            }

            var loopTo = Core.Constant.MaxInv;
            for (i = 0; i < loopTo; i++)
            {
                // Check to see if the player has the item
                if (GetPlayerInv(index, i) == itemNum)
                {
                    findItemSlotRet = i;
                    return findItemSlotRet;
                }
            }

            return findItemSlotRet;

        }

        public static void PlayerMapGetItem(int index)
        {
            int i;
            int itemnum;
            int n;
            int mapNum;
            string msg;

            mapNum = GetPlayerMap(index);

            var loopTo = Core.Constant.MaxMapItems;
            for (i = 0; i < loopTo; i++)
            {
                // See if theres even an item here
                if (Data.MapItem[mapNum, i].Num >= 0 & Data.MapItem[mapNum, i].Num < Core.Constant.MaxItems)
                {
                    // our drop?
                    if (CanPlayerPickupItem(index, i))
                    {
                        // Check if item is at the same location as the player
                        if (Data.MapItem[mapNum, i].X == GetPlayerX(index))
                        {
                            if (Data.MapItem[mapNum, i].Y == GetPlayerY(index))
                            {
                                // Find open slot
                                n = FindOpenInvSlot(index, (int)Data.MapItem[mapNum, i].Num);

                                // Open slot available?
                                if (n != -1)
                                {
                                    // Set item in players inventor
                                    itemnum = (int)Data.MapItem[mapNum, i].Num;

                                    SetPlayerInv(index, n, (int)Data.MapItem[mapNum, i].Num);

                                    if (Core.Data.Item[GetPlayerInv(index, n)].Type == (byte)ItemCategory.Currency | Core.Data.Item[GetPlayerInv(index, n)].Stackable == 1)
                                    {
                                        SetPlayerInvValue(index, n, GetPlayerInvValue(index, n) + Data.MapItem[mapNum, i].Value);
                                        msg = Data.MapItem[mapNum, i].Value + " " + Core.Data.Item[GetPlayerInv(index, n)].Name;
                                    }
                                    else
                                    {
                                        SetPlayerInvValue(index, n, 1);
                                        msg = Core.Data.Item[GetPlayerInv(index, n)].Name;
                                    }

                                    // Erase item from the map
                                    Item.SpawnItemSlot(i, -1, 0, GetPlayerMap(index), Data.MapItem[mapNum, i].X, Data.MapItem[mapNum, i].Y);
                                    NetworkSend.SendInventoryUpdate(index, n);                                 
                                    NetworkSend.SendActionMsg(GetPlayerMap(index), msg, (int) Color.White, (byte)Core.ActionMessageType.Static, GetPlayerX(index) * 32, GetPlayerY(index) * 32);
                                    break;
                                }
                                else
                                {
                                    NetworkSend.PlayerMsg(index, "Your inventory is full.", (int) Color.BrightRed);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static bool CanPlayerPickupItem(int index, int mapitemNum)
        {
            bool canPlayerPickupItemRet = default;
            int mapNum;

            mapNum = GetPlayerMap(index);

            if (Data.Map[mapNum].Moral >= 0)
            {
                if (Data.Moral[Data.Map[mapNum].Moral].CanPickupItem)
                {
                    // no lock or locked to player?
                    if (string.IsNullOrEmpty(Data.MapItem[mapNum, mapitemNum].PlayerName) | Data.MapItem[mapNum, mapitemNum].PlayerName == GetPlayerName(index))
                    {
                        canPlayerPickupItemRet = true;
                        return canPlayerPickupItemRet;
                    }
                }
                else
                {
                    NetworkSend.PlayerMsg(index, "You can't pickup items here!", (int) Color.BrightRed);
                }
            }

            canPlayerPickupItemRet = false;
            return canPlayerPickupItemRet;
        }

        public static int FindOpenInvSlot(int index, int itemNum)
        {
            int findOpenInvSlotRet = default;
            int i;

            // Check for subscript out of range
            if (Conversions.ToInteger(NetworkConfig.IsPlaying(index)) == 0 | itemNum < 0 | itemNum > Core.Constant.MaxItems)
            {
                return findOpenInvSlotRet;
            }

            if (Core.Data.Item[itemNum].Type == (byte)ItemCategory.Currency | Core.Data.Item[itemNum].Stackable == 1)
            {
                // If currency then check to see if they already have an instance of the item and add it to that
                var loopTo = Core.Constant.MaxInv;
                for (i = 0; i < loopTo; i++)
                {
                    if (GetPlayerInv(index, i) == itemNum)
                    {
                        findOpenInvSlotRet = i;
                        return findOpenInvSlotRet;
                    }
                }
            }

            var loopTo1 = Core.Constant.MaxInv;
            for (i = 0; i < loopTo1; i++)
            {
                // Try to find an open free slot
                if (GetPlayerInv(index, i) == -1)
                {
                    findOpenInvSlotRet = i;
                    return findOpenInvSlotRet;
                }
            }

            findOpenInvSlotRet = -1;
            return findOpenInvSlotRet;
        }

        public static bool TakeInv(int index, int itemNum, int itemVal)
        {
            bool takeInvRet = default;
            int i;

            takeInvRet = false;

            // Check for subscript out of range
            if (Conversions.ToInteger(NetworkConfig.IsPlaying(index)) == 0 | itemNum < 0 | itemNum > Core.Constant.MaxItems)
            {
                return takeInvRet;
            }

            var loopTo = Core.Constant.MaxInv;
            for (i = 0; i < loopTo; i++)
            {

                // Check to see if the player has the item
                if (GetPlayerInv(index, i) == itemNum)
                {
                    if (Core.Data.Item[itemNum].Type == (byte)ItemCategory.Currency | Core.Data.Item[itemNum].Stackable == 1)
                    {

                        // Is what we are trying to take away more then what they have?  If so just set it to zero
                        if (itemVal >= GetPlayerInvValue(index, i))
                        {
                            takeInvRet = true;
                        }
                        else
                        {
                            SetPlayerInvValue(index, i, GetPlayerInvValue(index, i) - itemVal);
                            NetworkSend.SendInventoryUpdate(index, i);
                        }
                    }
                    else
                    {
                        takeInvRet = true;
                    }

                    if (takeInvRet)
                    {
                        SetPlayerInv(index, i, -1);
                        SetPlayerInvValue(index, i, 0);
                        // Send the inventory update
                        NetworkSend.SendInventoryUpdate(index, i);
                        return takeInvRet;
                    }
                }

            }

            return takeInvRet;

        }

        public static bool GiveInv(int index, int itemNum, int itemVal, bool sendUpdate = true)
        {
            bool giveInvRet = default;
            int i;

            // Check for subscript out of range
            if (Conversions.ToInteger(NetworkConfig.IsPlaying(index)) == 0 | itemNum < 0 | itemNum > Core.Constant.MaxItems)
            {
                giveInvRet = false;
                return giveInvRet;
            }

            i = FindOpenInvSlot(index, itemNum);

            // Check to see if inventory is full
            if (i != -1)
            {
                if (itemVal == 0)
                    itemVal = 1;

                SetPlayerInv(index, i, itemNum);
                SetPlayerInvValue(index, i, GetPlayerInvValue(index, i) + itemVal);
                if (sendUpdate)
                    NetworkSend.SendInventoryUpdate(index, i);
                giveInvRet = true;
            }
            else
            {
                NetworkSend.PlayerMsg(index, "Your inventory is full.", (int)Core.Color.BrightRed);
                giveInvRet = false;
            }

            return giveInvRet;

        }

        public static void PlayerMapDropItem(int index, int invNum, int amount)
        {
            int i;

            // Check for subscript out of range
            if (Conversions.ToInteger(NetworkConfig.IsPlaying(index)) == 0 | invNum < 0 | invNum > Core.Constant.MaxInv)
            {
                return;
            }

            // check the player isn't doing something
            if (Core.Data.TempPlayer[index].InBank | Core.Data.TempPlayer[index].InShop >= 0 | Core.Data.TempPlayer[index].InTrade >= 0)
                return;

            if (Conversions.ToInteger(Data.Moral[GetPlayerMap(index)].CanDropItem) == 0)
            {
                NetworkSend.PlayerMsg(index, "You can't drop items here!", (int) Color.BrightRed);
                return;
            }

            if (GetPlayerInv(index, invNum) >= 0)
            {
                if (GetPlayerInv(index, invNum) < Core.Constant.MaxItems)
                {
                    i = Item.FindOpenMapItemSlot(GetPlayerMap(index));

                    if (i != 0)
                    {
                        {
                            var withBlock = Data.MapItem[GetPlayerMap(index), i];
                            withBlock.Num = GetPlayerInv(index, invNum);
                            withBlock.X = (byte)GetPlayerX(index);
                            withBlock.Y = (byte)GetPlayerY(index);
                            withBlock.PlayerName = GetPlayerName(index);
                            withBlock.PlayerTimer = General.GetTimeMs() + Constant.ItemSpawnTime;

                            withBlock.CanDespawn = true;
                            withBlock.DespawnTimer = General.GetTimeMs() + Constant.ItemDespawnTime;

                            if (Core.Data.Item[GetPlayerInv(index, invNum)].Type == (byte)ItemCategory.Currency | Core.Data.Item[GetPlayerInv(index, invNum)].Stackable == 1)
                            {
                                // Check if its more then they have and if so drop it all
                                if (amount >= GetPlayerInvValue(index, invNum))
                                {
                                    amount = GetPlayerInvValue(index, invNum);
                                    withBlock.Value = amount;
                                    SetPlayerInv(index, invNum, -1);
                                    SetPlayerInvValue(index, invNum, 0);
                                }
                                else
                                {
                                    withBlock.Value = amount;
                                    SetPlayerInvValue(index, invNum, GetPlayerInvValue(index, invNum) - amount);
                                }
                                NetworkSend.MapMsg(GetPlayerMap(index), string.Format("{0} has dropped {1} ({2}x).", GetPlayerName(index), GameLogic.CheckGrammar(Core.Data.Item[GetPlayerInv(index, invNum)].Name), amount), (int) Color.Yellow);
                            }
                            else
                            {
                                // It's not a currency object so this is easy
                                withBlock.Value = 0;

                                // send message
                                NetworkSend.MapMsg(GetPlayerMap(index), string.Format("{0} has dropped {1}.", GetPlayerName(index), GameLogic.CheckGrammar(Core.Data.Item[GetPlayerInv(index, invNum)].Name)), (int) Color.Yellow);
                                SetPlayerInv(index, invNum, -1);
                                SetPlayerInvValue(index, invNum, 0);
                            }

                            // Send inventory update
                            NetworkSend.SendInventoryUpdate(index, invNum);
                            // Spawn the item before we set the num or we'll get a different free map item slot
                            Item.SpawnItemSlot(i, (int)withBlock.Num, amount, GetPlayerMap(index), GetPlayerX(index), GetPlayerY(index));
                        }
                    }
                    else
                    {
                        NetworkSend.PlayerMsg(index, "Too many items already on the ground.", (int) Color.Yellow);
                    }
                }
            }

        }

        public static bool TakeInvSlot(int index, int invSlot, int itemVal)
        {
            bool takeInvSlotRet = default;
            object itemNum;

            takeInvSlotRet = false;

            // Check for subscript out of range
            if (Conversions.ToInteger(NetworkConfig.IsPlaying(index)) == 0 | invSlot < 0 | invSlot > Core.Constant.MaxItems)
                return takeInvSlotRet;

            itemNum = GetPlayerInv(index, invSlot);

            if (Core.Data.Item[Conversions.ToInteger(itemNum)].Type == (byte)ItemCategory.Currency | Core.Data.Item[Conversions.ToInteger(itemNum)].Stackable == 1)
            {

                // Is what we are trying to take away more then what they have?  If so just set it to zero
                if (itemVal >= GetPlayerInvValue(index, invSlot))
                {
                    takeInvSlotRet = true;
                }
                else
                {
                    SetPlayerInvValue(index, invSlot, GetPlayerInvValue(index, invSlot) - itemVal);
                }
            }
            else
            {
                takeInvSlotRet = true;
            }

            if (takeInvSlotRet)
            {
                SetPlayerInv(index, invSlot, -1);
                SetPlayerInvValue(index, invSlot, 0);
                return takeInvSlotRet;
            }

            return takeInvSlotRet;

        }

        public static bool CanPlayerUseItem(int index, int itemNum)
        {
            int i;

            if ((int)Data.Map[GetPlayerMap(index)].Moral >= 0)
            {
                if (Data.Moral[Data.Map[GetPlayerMap(index)].Moral].CanUseItem == false)
                {
                    NetworkSend.PlayerMsg(index, "You can't use items here!", (int) Color.BrightRed);
                    return false;
                }
            }

            var loopTo = Enum.GetNames(typeof(Stat)).Length;
            for (i = 0; i < loopTo; i++)
            {
                if (GetPlayerStat(index, (Stat)i) < Core.Data.Item[itemNum].StatReq[i])
                {
                    NetworkSend.PlayerMsg(index, "You do not meet the stat requirements to use this item.", (int) Color.BrightRed);
                    return false;
                }
            }

            if (Core.Data.Item[itemNum].LevelReq > GetPlayerLevel(index))
            {
                NetworkSend.PlayerMsg(index, "You do not meet the level requirements to use this item.", (int) Color.BrightRed);
                return false;
            }

            // Make sure they are the right job
            if (!(Core.Data.Item[itemNum].JobReq == GetPlayerJob(index)) & !(Core.Data.Item[itemNum].JobReq == -1))
            {
                NetworkSend.PlayerMsg(index, "You do not meet the job requirements to use this item.", (int) Color.BrightRed);
                return false;
            }

            // access requirement
            if (!(GetPlayerAccess(index) >= Core.Data.Item[itemNum].AccessReq))
            {
                NetworkSend.PlayerMsg(index, "You do not meet the access requirement to equip this item.", (int) Color.BrightRed);
                return false;
            }

            // check the player isn't doing something
            if (Core.Data.TempPlayer[index].InBank == true | Core.Data.TempPlayer[index].InShop >= 0 | Core.Data.TempPlayer[index].InTrade >= 0)
            {
                NetworkSend.PlayerMsg(index, "You can't use items while in a bank, shop, or trade!", (int) Color.BrightRed);
                return false;
            }

            return true;

        }

        public static void UseItem(int index, int invNum)
        {
            int itemNum;

            // Prevent hacking
            if (invNum < 0 | invNum > Core.Constant.MaxInv)
                return;

            itemNum = GetPlayerInv(index, invNum);

            if (itemNum < 0 | itemNum > Core.Constant.MaxItems)
                return;

            if (!CanPlayerUseItem(index, itemNum))
                return;

            try
            {
                Script.Instance?.UseItem(index, itemNum, invNum);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static void PlayerSwitchInvSlots(int index, int oldSlot, int newSlot)
        {
            int oldNum;
            int oldValue;
            int oldRarity;
            string oldPrefix;
            string oldSuffix;
            int oldSpeed;
            int oldDamage;
            int newNum;
            int newValue;
            int newRarity;
            string newPrefix;
            string newSuffix;
            int newSpeed;
            int newDamage;

            if (oldSlot == -1 | newSlot == -1)
                return;

            oldNum = GetPlayerInv(index, oldSlot);
            oldValue = GetPlayerInvValue(index, oldSlot);
            newNum = GetPlayerInv(index, newSlot);
            newValue = GetPlayerInvValue(index, newSlot);

            if (newNum >= 0)
            {
                if (oldNum == newNum & Core.Data.Item[newNum].Stackable == 1) // same item, if we can stack it, lets do that :P
                {
                    SetPlayerInv(index, newSlot, newNum);
                    SetPlayerInvValue(index, newSlot, oldValue + newValue);
                    SetPlayerInv(index, oldSlot, 0);
                    SetPlayerInvValue(index, oldSlot, 0);
                }
                else
                {
                    SetPlayerInv(index, newSlot, oldNum);
                    SetPlayerInvValue(index, newSlot, oldValue);
                    SetPlayerInv(index, oldSlot, newNum);
                    SetPlayerInvValue(index, oldSlot, newValue);
                }
            }
            else
            {
                SetPlayerInv(index, newSlot, oldNum);
                SetPlayerInvValue(index, newSlot, oldValue);
                SetPlayerInv(index, oldSlot, newNum);
                SetPlayerInvValue(index, oldSlot, newValue);
            }

            NetworkSend.SendInventory(index);
        }

        public static void PlayerSwitchSkillSlots(int index, int oldSlot, int newSlot)
        {
            double oldNum;
            int oldValue;
            int oldRarity;
            string oldPrefix;
            string oldSuffix;
            int oldSpeed;
            int oldDamage;
            int newNum;
            int newValue;
            int newRarity;
            string newPrefix;
            string newSuffix;
            int newSpeed;
            int newDamage;

            if (oldSlot == -1 | newSlot == -1)
                return;

            oldNum = GetPlayerSkill(index, (int)oldSlot);
            oldValue = GetPlayerSkillCd(index, (int)oldSlot);
            newNum = GetPlayerSkill(index, (int)newSlot);
            newValue = GetPlayerSkillCd(index, (int)newSlot);

            if (newNum >= 0)
            {
                if (oldNum == newNum & Core.Data.Item[(int)newNum].Stackable == 1) // same item, if we can stack it, lets do that :P
                {
                    SetPlayerSkill(index, (int)newSlot, newNum);
                    SetPlayerSkillCd(index, (int)newSlot, newValue);
                    SetPlayerSkill(index, (int)oldSlot, 0);
                    SetPlayerSkillCd(index, (int)oldSlot, 0);
                }
                else
                {
                    SetPlayerSkill(index, (int)newSlot, (int)oldNum);
                    SetPlayerSkillCd(index, (int)newSlot, oldValue);
                    SetPlayerSkill(index, (int)oldSlot, (int)newNum);
                    SetPlayerSkillCd(index, (int)oldSlot, newValue);
                }
            }
            else
            {
                SetPlayerSkill(index, (int)newSlot, (int)oldNum);
                SetPlayerSkillCd(index, (int)newSlot, oldValue);
                SetPlayerSkill(index, (int)oldSlot, (int)newNum);
                SetPlayerSkillCd(index, (int)oldSlot, newValue);
            }

            NetworkSend.SendPlayerSkills(index);
        }

        #endregion

        #region Equipment

        public static void CheckEquippedItems(int index)
        {
            double itemNum;
            int i;

            // We want to check incase an admin takes away an object but they had it equipped
            var loopTo = Enum.GetNames(typeof(Equipment)).Length;;
            for (i = 0; i < (int)loopTo; i++)
            {
                itemNum = GetPlayerEquipment(index, (Equipment)i);

                if (itemNum >= 0)
                {

                    switch (i)
                    {
                        case (byte)Equipment.Weapon:
                            {

                                if (Core.Data.Item[(int)itemNum].SubType != (byte)Equipment.Weapon)
                                    SetPlayerEquipment(index, -1, (Equipment)i);
                                break;
                            }
                        case (byte)Equipment.Armor:
                            {

                                if (Core.Data.Item[(int)itemNum].SubType != (byte)Equipment.Armor)
                                    SetPlayerEquipment(index, -1, (Equipment)i);
                                break;
                            }
                        case (byte)Equipment.Helmet:
                            {

                                if (Core.Data.Item[(int)itemNum].SubType != (byte)Equipment.Helmet)
                                    SetPlayerEquipment(index, -1, (Equipment)i);
                                break;
                            }
                        case (byte)Equipment.Shield:
                            {

                                if (Core.Data.Item[(int)itemNum].SubType != (byte)Equipment.Shield)
                                    SetPlayerEquipment(index, -1, (Equipment)i);
                                break;
                            }
                    }
                }
                else
                {
                    SetPlayerEquipment(index, -1, (Equipment)i);
                }

            }

        }

        public static void PlayerUnequipItem(int index, int eqSlot)
        {
            int i;
            int m;
            int itemNum;

            var eqCount = Enum.GetNames(typeof(Equipment)).Length;

            if (eqSlot < 1 | eqSlot > eqCount)
                return; // exit out early if error'd

            if (GetPlayerEquipment(index, (Equipment)eqSlot) < 0 || GetPlayerEquipment(index, (Equipment)eqSlot) > Core.Constant.MaxItems)
                return;

            if (FindOpenInvSlot(index, GetPlayerEquipment(index, (Equipment)eqSlot)) >= 0)
            {
                itemNum = GetPlayerEquipment(index, (Equipment)eqSlot);

                m = FindOpenInvSlot(index, (int)Core.Data.Player[index].Equipment[eqSlot]);
                SetPlayerInv(index, m, Core.Data.Player[index].Equipment[eqSlot]);
                SetPlayerInvValue(index, m, 0);

                NetworkSend.PlayerMsg(index, "You unequip " + GameLogic.CheckGrammar(Core.Data.Item[GetPlayerEquipment(index, (Equipment)eqSlot)].Name), (int) Color.Yellow);

                // remove equipment
                SetPlayerEquipment(index, -1, (Equipment)eqSlot);
                NetworkSend.SendWornEquipment(index);
                NetworkSend.SendMapEquipment(index);
                NetworkSend.SendStats(index);
                NetworkSend.SendInventory(index);

                // send vitals
                NetworkSend.SendVitals(index);
            }
            else
            {
                NetworkSend.PlayerMsg(index, "Your inventory is full.", (int) Color.BrightRed);
            }

        }

        public static void JoinGame(int index)
        {
            try
            {
                Script.Instance?.JoinGame(index);

                General.UpdateCaption();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static async System.Threading.Tasks.Task LeftGame(int index)
        {
            try
            {
                Script.Instance?.LeftGame(index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (Core.Data.TempPlayer[index].InGame)
            {
                await Database.SaveCharacterAsync(index, Core.Data.TempPlayer[index].Slot);
                await Database.SaveBankAsync(index);
            }

            Database.ClearPlayer(index);

            General.UpdateCaption();
        }

        public static int KillPlayer(int index)
        {
            try
            {
                int exp = Script.Instance?.KillPlayer(index);
                return exp;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;

        }

        public static void OnDeath(int index)
        {
            try
            {
                // Clear skill casting
                Core.Data.TempPlayer[index].SkillBuffer = -1;
                Core.Data.TempPlayer[index].SkillBufferTimer = 0;
                NetworkSend.SendClearSkillBuffer(index);

                Script.Instance?.OnDeath();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        #endregion

        #region Bank

        public static void GiveBank(int index, int invSlot, int amount)
        {
            byte bankSlot;
            int itemNum;

            if (invSlot < 0 | invSlot > Core.Constant.MaxInv)
                return;

            if (amount < 0)
                amount = 0;

            if (GetPlayerInvValue(index, invSlot) < amount & GetPlayerInv(index, invSlot) == 0)
                return;

            bankSlot = FindOpenbankSlot(index, GetPlayerInv(index, invSlot));
            itemNum = GetPlayerInv(index, invSlot);

            if (bankSlot >= 0)
            {
                if (Core.Data.Item[GetPlayerInv(index, invSlot)].Type == (byte)ItemCategory.Currency | Core.Data.Item[GetPlayerInv(index, invSlot)].Stackable == 1)
                {
                    if (GetPlayerBank(index, bankSlot) == GetPlayerInv(index, invSlot))
                    {
                        SetPlayerBankValue(index, bankSlot, GetPlayerBankValue(index, bankSlot) + amount);
                        TakeInv(index, GetPlayerInv(index, invSlot), amount);
                    }
                    else
                    {
                        SetPlayerBank(index, bankSlot, GetPlayerInv(index, invSlot));
                        SetPlayerBankValue(index, bankSlot, amount);
                        TakeInv(index, GetPlayerInv(index, invSlot), amount);
                    }
                }
                else if (GetPlayerBank(index, bankSlot) == GetPlayerInv(index, invSlot))
                {
                    SetPlayerBankValue(index, bankSlot, GetPlayerBankValue(index, bankSlot) + 1);
                    TakeInv(index, GetPlayerInv(index, invSlot), 0);
                }
                else
                {
                    SetPlayerBank(index, bankSlot, itemNum);
                    SetPlayerBankValue(index, bankSlot, 1);
                    TakeInv(index, GetPlayerInv(index, invSlot), 0);
                }

                NetworkSend.SendBank(index);
            }

        }

        public static int GetPlayerBank(int index, byte bankSlot)
        {
            int getPlayerBankRet = default;
            getPlayerBankRet = Data.Bank[index].Item[bankSlot].Num;
            return getPlayerBankRet;
        }

        public static void SetPlayerBank(int index, byte bankSlot, int itemNum)
        {
            Data.Bank[index].Item[bankSlot].Num = itemNum;
        }

        public static int GetPlayerBankValue(int index, byte bankSlot)
        {
            int getPlayerBankValueRet = default;
            getPlayerBankValueRet = Data.Bank[index].Item[bankSlot].Value;
            return getPlayerBankValueRet;
        }

        public static void SetPlayerBankValue(int index, byte bankSlot, int value)
        {
            Data.Bank[index].Item[bankSlot].Value = value;
        }

        public static byte FindOpenbankSlot(int index, int itemNum)
        {
            byte findOpenbankSlotRet = default;
            int i;

            if (!NetworkConfig.IsPlaying(index))
                return findOpenbankSlotRet;
            if (itemNum < 0 | itemNum > Core.Constant.MaxItems)
                return findOpenbankSlotRet;

            if (Core.Data.Item[itemNum].Type == (byte)ItemCategory.Currency | Core.Data.Item[itemNum].Stackable == 1)
            {
                var loopTo = Core.Constant.MaxBank;
                for (i = 0; i < loopTo; i++)
                {
                    if (GetPlayerBank(index, (byte)i) == itemNum)
                    {
                        findOpenbankSlotRet = (byte)i;
                        return findOpenbankSlotRet;
                    }
                }
            }

            var loopTo1 = Core.Constant.MaxBank;
            for (i = 0; i < loopTo1; i++)
            {
                if (GetPlayerBank(index, (byte)i) == -1)
                {
                    findOpenbankSlotRet = (byte)i;
                    return findOpenbankSlotRet;
                }
            }

            return findOpenbankSlotRet;

        }

        public static void TakeBank(int index, byte bankSlot, int amount)
        {
            int invSlot;

            if (bankSlot < 0 | bankSlot > Core.Constant.MaxBank)
                return;

            if (amount < 0)
                amount = 0;

            if (GetPlayerBankValue(index, bankSlot) < amount)
                return;

            invSlot = FindOpenInvSlot(index, GetPlayerBank(index, bankSlot));

            if (invSlot >= 0)
            {
                if (Core.Data.Item[GetPlayerBank(index, bankSlot)].Type == (byte)ItemCategory.Currency | Core.Data.Item[GetPlayerBank(index, bankSlot)].Stackable == 1)
                {
                    GiveInv(index, GetPlayerBank(index, bankSlot), amount);
                    SetPlayerBankValue(index, bankSlot, GetPlayerBankValue(index, bankSlot) - amount);
                    if (GetPlayerBankValue(index, bankSlot) < 0)
                    {
                        SetPlayerBank(index, bankSlot, 0);
                        SetPlayerBankValue(index, bankSlot, 0);
                    }
                }
                else if (GetPlayerBank(index, bankSlot) == GetPlayerInv(index, (int)invSlot))
                {
                    if (GetPlayerBankValue(index, bankSlot) > 1)
                    {
                        GiveInv(index, GetPlayerBank(index, bankSlot), 0);
                        SetPlayerBankValue(index, bankSlot, GetPlayerBankValue(index, bankSlot) - 1);
                    }
                }
                else
                {
                    GiveInv(index, GetPlayerBank(index, bankSlot), 0);
                    SetPlayerBank(index, bankSlot, -1);
                    SetPlayerBankValue(index, bankSlot, 0);
                }

            }

            NetworkSend.SendBank(index);
        }

        public static void PlayerSwitchbankSlots(int index, int oldSlot, int newSlot)
        {
            int oldNum;
            int oldValue;
            int newNum;
            int newValue;
            int i;

            if (oldSlot == -1 | newSlot == -1)
                return;

            oldNum = GetPlayerBank(index, (byte)oldSlot);
            oldValue = GetPlayerBankValue(index, (byte)oldSlot);
            newNum = GetPlayerBank(index, (byte)newSlot);
            newValue = GetPlayerBankValue(index, (byte)newSlot);

            SetPlayerBank(index, (byte)newSlot, oldNum);
            SetPlayerBankValue(index, (byte)newSlot, oldValue);

            SetPlayerBank(index, (byte)oldSlot, newNum);
            SetPlayerBankValue(index, (byte)oldSlot, newValue);

            NetworkSend.SendBank(index);
        }

        #endregion

    }
}