using Core;
using Core.Configurations;
using Core.Globals;
using Core.Net;
using Microsoft.Extensions.Logging;
using Server.Game;
using Server.Game.Net;
using Server.Net;
using static Core.Globals.Command;
using static Core.Net.Packets;

namespace Server;

public static class Player
{
    public static void CheckPlayerLevelUp(int playerId)
    {
        try
        {
            Script.Instance?.CheckPlayerLevelUp(playerId);
        }
        catch (Exception ex)
        {
            General.Logger.LogError(ex, "[Script] Error in {MethodName}", nameof(CheckPlayerLevelUp));
        }
    }

    public static void HandleUseChar(GameSession session)
    {
        PlayerService.Instance.AddPlayer(session.Id, session.Channel);

        // Set the flag so we know the person is in the game
        Data.TempPlayer[session.Id].InGame = true;

        // Send an ok to client to start receiving in game data
        NetworkSend.SendLoginOk(session.Id);

        JoinGame(session.Id);

        General.Logger.LogInformation("{AccountName} | {PlayerName} has began playing {GameName}",
            GetAccountLogin(session.Id), GetPlayerName(session.Id),
            SettingsManager.Instance.GameName);
    }

    public static void SendLeaveMap(int playerId, int mapNum)
    {
        var packet = new PacketWriter(4);

        packet.WriteEnum(ServerPackets.SLeftMap);
        packet.WriteInt32(playerId);

        NetworkConfig.SendDataToMapBut(playerId, mapNum, packet.GetBytes());
    }

    public static void PlayerWarp(int playerId, int mapNum, int x, int y, int dir)
    {
        if (!NetworkConfig.IsPlaying(playerId) || mapNum < 0 || mapNum >= Core.Globals.Constant.MaxMaps)
        {
            return;
        }

        x = Math.Clamp(x, 0, Data.Map[mapNum].MaxX) * 32;
        y = Math.Clamp(y, 0, Data.Map[mapNum].MaxY) * 32;

        Data.TempPlayer[playerId].EventProcessingCount = 0;
        Data.TempPlayer[playerId].EventMap.CurrentEvents = 0; // Clear events
        Data.TempPlayer[playerId].Target = 0;
        Data.TempPlayer[playerId].TargetType = 0;

        NetworkSend.SendTarget(playerId, 0, 0);

        // Save old map to send erase player data to
        var oldMapNum = GetPlayerMap(playerId);
        if (oldMapNum != mapNum)
        {
            try
            {
                Script.Instance?.LeaveMap(playerId, oldMapNum);
            }
            catch (Exception ex)
            {
                General.Logger.LogError(ex, "[Script] Error in {MethodName}", "LeaveMap");
            }

            SendLeaveMap(playerId, oldMapNum);
        }

        SetPlayerMap(playerId, mapNum);
        SetPlayerX(playerId, x);
        SetPlayerY(playerId, y);
        SetPlayerDir(playerId, dir);

        NetworkSend.SendPlayerXy(playerId);

        // Send equipment of all people on new map
        if (GameLogic.GetTotalMapPlayers(mapNum) > 0)
        {
            foreach (var otherPlayerId in PlayerService.Instance.PlayerIds)
            {
                if (GetPlayerMap(otherPlayerId) == mapNum)
                {
                    NetworkSend.SendMapEquipmentTo(otherPlayerId, playerId);
                }
            }
        }

        // Now we check if there were any players left on the map the player just left, and if not stop processing npcs
        if (GameLogic.GetTotalMapPlayers(oldMapNum) == 0)
        {
            // Regenerate all Npcs' health
            for (var mapNpcNum = 0; mapNpcNum < Core.Globals.Constant.MaxMapNpcs; mapNpcNum++)
            {
                if (Data.MapNpc[oldMapNum].Npc[mapNpcNum].Num >= 0)
                {
                    Data.MapNpc[oldMapNum].Npc[mapNpcNum].Vital[(byte) Vital.Health] = GameLogic.GetNpcMaxVital(Data.MapNpc[oldMapNum].Npc[mapNpcNum].Num, Vital.Health);
                }
            }
        }

        // Sets it so we know to process npcs on the map
        Data.TempPlayer[playerId].GettingMap = true;

        Moral.SendUpdateMoralTo(playerId, Data.Map[mapNum].Moral);

        var packet = new PacketWriter(12);

        packet.WriteEnum(ServerPackets.SCheckForMap);
        packet.WriteInt32(mapNum);
        packet.WriteInt32(Data.Map[mapNum].Revision);

        PlayerService.Instance.SendDataTo(playerId, packet.GetBytes());
    }

    public static void PlayerMove(int playerId, int dir, int movement, bool expectingWarp)
    {
        int x;
        int y;
        var didWarp = false;
        var vital = 0;
        var amount = 0;

        // Check for subscript out of range
        var count = System.Enum.GetValues(typeof(MovementState)).Length;
        var count2 = System.Enum.GetValues(typeof(Direction)).Length;
		if (dir < (int) Direction.Up || dir > count2 || movement < 0 || movement > count)
        {
            return;
        }

        // Prevent player from moving if they have casted a skill
        if (Data.TempPlayer[playerId].SkillBuffer >= 0)
        {
            NetworkSend.SendPlayerXy(playerId);
            return;
        }

        // Cant move if in the bank
        if (Data.TempPlayer[playerId].InBank)
        {
            NetworkSend.SendPlayerXy(playerId);
            return;
        }

        // if stunned, stop them moving
        if (Data.TempPlayer[playerId].StunDuration > 0)
        {
            NetworkSend.SendPlayerXy(playerId);
            return;
        }

        if (Data.TempPlayer[playerId].InShop >= 0 || Data.TempPlayer[playerId].InBank)
        {
            NetworkSend.SendPlayerXy(playerId);
            return;
        }

        SetPlayerDir(playerId, dir);
        var moved = false;
        var mapNum = GetPlayerMap(playerId);

        switch ((Direction) dir)
        {
            case Direction.Up:
                if (GetPlayerY(playerId) > 0)
                {
                    if (IsTileBlocked(mapNum, GetPlayerX(playerId), GetPlayerY(playerId), Direction.Up))
                    {
                        break;
                    }

                    SetPlayerY(playerId, GetPlayerRawY(playerId) - 1);
                    moved = true;
                }
                else if (Data.Map[mapNum].Tile[GetPlayerX(playerId), GetPlayerY(playerId)].Type != TileType.NoCrossing && Data.Map[mapNum].Tile[GetPlayerX(playerId), GetPlayerY(playerId)].Type2 != TileType.NoCrossing)
                {
                    if (Data.Map[GetPlayerMap(playerId)].Up > 0)
                    {
                        var newMapY = Data.Map[Data.Map[GetPlayerMap(playerId)].Up].MaxY;
                        
                        PlayerWarp(playerId, Data.Map[GetPlayerMap(playerId)].Up, GetPlayerX(playerId), newMapY, (int) Direction.Up);
                        
                        didWarp = true;
                        moved = true;
                    }
                }

                break;

            case Direction.Down:
                if (GetPlayerY(playerId) < Data.Map[mapNum].MaxY - 1)
                {
                    if (IsTileBlocked(mapNum, GetPlayerX(playerId), GetPlayerY(playerId), Direction.Down))
                    {
                        break;
                    }

                    SetPlayerY(playerId, GetPlayerRawY(playerId) + 1);
                    
                    moved = true;
                }
                else if (Data.Map[GetPlayerMap(playerId)].Tile[GetPlayerX(playerId), GetPlayerY(playerId)].Type != TileType.NoCrossing && Data.Map[GetPlayerMap(playerId)].Tile[GetPlayerX(playerId), GetPlayerY(playerId)].Type2 != TileType.NoCrossing)
                {
                    if (Data.Map[GetPlayerMap(playerId)].Down > 0)
                    {
                        PlayerWarp(playerId, Data.Map[GetPlayerMap(playerId)].Down, GetPlayerX(playerId), 0, (int) Direction.Down);
                        
                        didWarp = true;
                        moved = true;
                    }
                }

                break;

            case Direction.Left:
                if (GetPlayerX(playerId) > 0)
                {
                    if (IsTileBlocked(mapNum, GetPlayerX(playerId), GetPlayerY(playerId), Direction.Left))
                    {
                        break;
                    }

                    SetPlayerX(playerId, GetPlayerRawX(playerId) - 1);
                    
                    moved = true;
                }
                else if (Data.Map[GetPlayerMap(playerId)].Tile[GetPlayerX(playerId), GetPlayerY(playerId)].Type != TileType.NoCrossing && Data.Map[GetPlayerMap(playerId)].Tile[GetPlayerX(playerId), GetPlayerY(playerId)].Type2 != TileType.NoCrossing)
                {
                    if (Data.Map[GetPlayerMap(playerId)].Left > 0)
                    {
                        var newMapX = Data.Map[Data.Map[GetPlayerMap(playerId)].Left].MaxX;
                        
                        PlayerWarp(playerId, Data.Map[GetPlayerMap(playerId)].Left, newMapX, GetPlayerY(playerId), (int) Direction.Left);
                        
                        didWarp = true;
                        moved = true;
                    }
                }

                break;

            case Direction.Right:
                if (GetPlayerX(playerId) < Data.Map[mapNum].MaxX - 1)
                {
                    if (IsTileBlocked(mapNum, GetPlayerX(playerId), GetPlayerY(playerId), Direction.Right))
                    {
                        break;
                    }

                    SetPlayerX(playerId, GetPlayerRawX(playerId) + 1);
                    
                    moved = true;
                }
                else if (Data.Map[GetPlayerMap(playerId)].Tile[GetPlayerX(playerId), GetPlayerY(playerId)].Type != TileType.NoCrossing && Data.Map[GetPlayerMap(playerId)].Tile[GetPlayerX(playerId), GetPlayerY(playerId)].Type2 != TileType.NoCrossing)
                {
                    if (Data.Map[GetPlayerMap(playerId)].Right > 0)
                    {
                        PlayerWarp(playerId, Data.Map[GetPlayerMap(playerId)].Right, 0, GetPlayerY(playerId), (int) Direction.Right);
                        
                        didWarp = true;
                        moved = true;
                    }
                }

                break;

            case Direction.UpRight:
                if (GetPlayerY(playerId) > 0 && GetPlayerX(playerId) < Data.Map[mapNum].MaxX - 1)
                {
                    if (IsTileBlocked(mapNum, GetPlayerX(playerId), GetPlayerY(playerId), Direction.UpRight))
                    {
                        break;
                    }

                    SetPlayerX(playerId, GetPlayerRawX(playerId) + 1);
                    SetPlayerY(playerId, GetPlayerRawY(playerId) - 1);
                    
                    moved = true;
                }

                break;

            case Direction.UpLeft:
                if (GetPlayerY(playerId) > 0 && GetPlayerX(playerId) > 0)
                {
                    if (IsTileBlocked(mapNum, GetPlayerX(playerId), GetPlayerY(playerId), Direction.UpLeft))
                    {
                        break;
                    }

                    SetPlayerX(playerId, GetPlayerRawX(playerId) - 1);
                    SetPlayerY(playerId, GetPlayerRawY(playerId) - 1);
                    
                    moved = true;
                }

                break;

            case Direction.DownRight:
                if (GetPlayerY(playerId) < Data.Map[mapNum].MaxY - 1 && GetPlayerX(playerId) < Data.Map[mapNum].MaxX - 1)
                {
                    if (IsTileBlocked(mapNum, GetPlayerX(playerId), GetPlayerY(playerId), Direction.DownRight))
                    {
                        break;
                    }

                    SetPlayerX(playerId, GetPlayerRawX(playerId) + 1);
                    SetPlayerY(playerId, GetPlayerRawY(playerId) + 1);
                    
                    moved = true;
                }

                break;

            case Direction.DownLeft:
                if (GetPlayerY(playerId) < Data.Map[mapNum].MaxY - 1 && GetPlayerX(playerId) > 0)
                {
                    if (IsTileBlocked(mapNum, GetPlayerX(playerId), GetPlayerY(playerId), Direction.DownLeft))
                    {
                        break;
                    }

                    SetPlayerX(playerId, GetPlayerRawX(playerId) - 1);
                    SetPlayerY(playerId, GetPlayerRawY(playerId) + 1);

                    moved = true;
                }

                break;
        }

        if (GetPlayerX(playerId) >= 0 &&
            GetPlayerY(playerId) >= 0 &&
            GetPlayerX(playerId) < Data.Map[GetPlayerMap(playerId)].MaxX &&
            GetPlayerY(playerId) < Data.Map[GetPlayerMap(playerId)].MaxY)
        {
            for (var i = 0; i < Data.TempPlayer[playerId].EventMap.CurrentEvents; i++)
            {
                EventLogic.TriggerEvent(playerId, i, 1, GetPlayerX(playerId), GetPlayerY(playerId));
            }

            ref var tile = ref Data.Map[GetPlayerMap(playerId)].Tile[GetPlayerX(playerId), GetPlayerY(playerId)];

            mapNum = -1;
            x = 0;
            y = 0;

            // Check to see if the tile is a warp tile, and if so warp them
            if (tile.Type == TileType.Warp)
            {
                mapNum = tile.Data1;
                x = tile.Data2 * 32;
                y = tile.Data3 * 32;
            }

            if (tile.Type2 == TileType.Warp)
            {
                mapNum = tile.Data1_2;
                x = tile.Data2_2;
                y = tile.Data3_2;
            }

            if (mapNum >= 0 && mapNum < Core.Globals.Constant.MaxMaps)
            {
                PlayerWarp(playerId, mapNum, x, y, (int) Direction.Down);

                didWarp = true;
                moved = true;
            }

            x = -1;
            if (tile.Type == TileType.Shop)
            {
                x = tile.Data1;
            }

            if (tile.Type2 == TileType.Shop)
            {
                x = tile.Data1_2;
            }

            if (x >= 0) // shop exists?
            {
                if (Data.Shop[x].Name.Length > 0)
                {
                    NetworkSend.SendOpenShop(playerId, x);
                    
                    Data.TempPlayer[playerId].InShop = x;
                }
            }

            // Check to see if the tile is a bank, and if so send bank
            if (tile.Type == TileType.Bank || tile.Type2 == TileType.Bank)
            {
                NetworkSend.SendBank(playerId);
                
                Data.TempPlayer[playerId].InBank = true;
                
                moved = true;
            }

            // Check if it's a heal tile
            if (tile.Type == TileType.Heal)
            {
                vital = tile.Data1;
                amount = tile.Data2;
            }

            if (tile.Type2 == TileType.Heal)
            {
                vital = tile.Data1_2;
                amount += tile.Data2_2;
            }

            if (vital > 0)
            {
                if (GetPlayerVital(playerId, (Vital) vital) != GetPlayerMaxVital(playerId, (Vital) vital))
                {
                    int color;
                    if (vital == (byte) Vital.Health)
                    {
                        color = (int) ColorName.BrightGreen;
                    }
                    else
                    {
                        color = (int) ColorName.BrightBlue;
                    }

                    NetworkSend.SendActionMsg(GetPlayerMap(playerId), "+" + amount, color, (byte) ActionMessageType.Scroll, GetPlayerX(playerId) * 32, GetPlayerY(playerId) * 32, 1);

                    SetPlayerVital(playerId, (Vital) vital, GetPlayerVital(playerId, (Vital) vital) + amount);

                    NetworkSend.PlayerMsg(playerId, "You feel rejuvenating forces coursing through your body.", (int) ColorName.BrightGreen);
                    NetworkSend.SendVital(playerId, (Vital) vital);
                }

                moved = true;
            }

            // Check if it's a trap tile
            if (tile.Type == TileType.Trap)
            {
                amount = tile.Data1;
            }

            if (tile.Type2 == TileType.Trap)
            {
                amount += tile.Data1_2;
            }

            if (amount > 0)
            {
                NetworkSend.SendActionMsg(GetPlayerMap(playerId), "-" + amount, (int) ColorName.BrightRed, (byte) ActionMessageType.Scroll, GetPlayerX(playerId) * 32, GetPlayerY(playerId) * 32, 1);
                if (GetPlayerVital(playerId, Vital.Health) - amount < 0)
                {
                    KillPlayer(playerId);
                    NetworkSend.PlayerMsg(playerId, "You've been killed by a trap.", (int) ColorName.BrightRed);
                }
                else
                {
                    SetPlayerVital(playerId, Vital.Health, GetPlayerVital(playerId, Vital.Health) - amount);
                    NetworkSend.PlayerMsg(playerId, "You've been injured by a trap.", (int) ColorName.BrightRed);
                    NetworkSend.SendVital(playerId, Vital.Health);
                }

                moved = true;
            }
        }

        // They tried to hack
        if (!moved || (expectingWarp && !didWarp))
        {
            PlayerWarp(playerId, GetPlayerMap(playerId), GetPlayerX(playerId), GetPlayerY(playerId), (byte) Direction.Down);
        }

        x = GetPlayerX(playerId);
        y = GetPlayerY(playerId);

        if (!moved)
        {
            return;
        }
        
        Data.Player[playerId].IsMoving = true;
        
        NetworkSend.SendPlayerXyToMap(playerId);
        try
        {
            Script.Instance?.PlayerMove(playerId);
        }
        catch (Exception ex)
        {
            General.Logger.LogError(ex, "[Script] Error in {MethodName}", nameof(PlayerMove));
        }

        if (Data.TempPlayer[playerId].EventMap.CurrentEvents <= 0)
        {
            return;
        }
            
        for (var i = 0; i < Data.TempPlayer[playerId].EventMap.CurrentEvents; i++)
        {
            var beginEvent = false;

            if (Data.TempPlayer[playerId].EventMap.EventPages[i].EventId < 0)
            {
                continue;
            }
                
            if (Data.Map[GetPlayerMap(playerId)].Event[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].Globals == 1)
            {
                if (Data.Map[GetPlayerMap(playerId)].Event[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].X == x & Data.Map[GetPlayerMap(playerId)].Event[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].Y == y & Data.Map[GetPlayerMap(playerId)].Event[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].Pages[Data.TempPlayer[playerId].EventMap.EventPages[i].PageId].Trigger == 1 & Data.TempPlayer[playerId].EventMap.EventPages[i].Visible)
                {
                    beginEvent = true;
                }
            }
            else if (Data.TempPlayer[playerId].EventMap.EventPages[i].X == x & Data.TempPlayer[playerId].EventMap.EventPages[i].Y == y & Data.Map[GetPlayerMap(playerId)].Event[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].Pages[Data.TempPlayer[playerId].EventMap.EventPages[i].PageId].Trigger == 1 & Data.TempPlayer[playerId].EventMap.EventPages[i].Visible)
            {
                beginEvent = true;
            }

            if (!beginEvent)
            {
                continue;
            }
            
            // Process this event, it is on-touch and everything checks out.
            if (Data.Map[GetPlayerMap(playerId)].Event[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].Pages[Data.TempPlayer[playerId].EventMap.EventPages[i].PageId].CommandListCount > 0)
            {
                Data.TempPlayer[playerId].EventProcessing[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].Active = 0;
                Data.TempPlayer[playerId].EventProcessing[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].ActionTimer = General.GetTimeMs();
                Data.TempPlayer[playerId].EventProcessing[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].CurList = 0;
                Data.TempPlayer[playerId].EventProcessing[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].CurSlot = 0;
                Data.TempPlayer[playerId].EventProcessing[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].EventId = Data.TempPlayer[playerId].EventMap.EventPages[i].EventId;
                Data.TempPlayer[playerId].EventProcessing[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].PageId = Data.TempPlayer[playerId].EventMap.EventPages[i].PageId;
                Data.TempPlayer[playerId].EventProcessing[Data.TempPlayer[playerId].EventMap.EventPages[i].EventId].WaitingForResponse = 0;

                var eventId = Data.TempPlayer[playerId].EventMap.EventPages[i].EventId;
                var pageId = Data.TempPlayer[playerId].EventMap.EventPages[i].PageId;
                var commandListCount = Data.Map[GetPlayerMap(playerId)].Event[eventId].Pages[pageId].CommandListCount;

                Array.Resize(ref Data.TempPlayer[playerId].EventProcessing[eventId].ListLeftOff, commandListCount);
            }

            beginEvent = false;
        }
    }

    public static bool IsTileBlocked(int mapNum, int x, int y, Direction dir)
    {
        try
        {
            if (Data.Moral[Data.Map[mapNum].Moral].PlayerBlock)
            {
                foreach (var playerId in PlayerService.Instance.PlayerIds)
                {
                    if (GetPlayerMap(playerId) == mapNum &&
                        GetPlayerX(playerId) == x &&
                        GetPlayerY(playerId) == y)
                    {
                        return true;
                    }
                }
            }

            if (Data.Moral[Data.Map[mapNum].Moral].NpcBlock)
            {
                for (var mapNpcNum = 0; mapNpcNum < Core.Globals.Constant.MaxMapNpcs; mapNpcNum++)
                {
                    if (Data.MapNpc[mapNum].Npc[mapNpcNum].Num >= 0 &&
                        Data.MapNpc[mapNum].Npc[mapNpcNum].X == x &&
                        Data.MapNpc[mapNum].Npc[mapNpcNum].Y == y)
                    {
                        return true;
                    }
                }
            }

            // Check to make sure that the tile is walkable
            if (IsDirBlocked(Data.Map[mapNum].Tile[x, y].DirBlock, dir))
            {
                return true;
            }

            return Data.Map[mapNum].Tile[x, y].Type == TileType.Blocked ||
                   Data.Map[mapNum].Tile[x, y].Type2 == TileType.Blocked;
        }
        catch (Exception ex)
        {
            return false;
        }
     }

    public static int HasItem(int playerId, int itemNum)
    {
        if (itemNum is < 0 or > Core.Globals.Constant.MaxItems)
        {
            return 0;
        }

        var totalQuantity = 0;
        for (var invSlot = 0; invSlot < Core.Globals.Constant.MaxInv; invSlot++)
        {
            if (GetPlayerInv(playerId, invSlot) != itemNum)
            {
                continue;
            }

            if (Data.Item[itemNum].Type == (byte) ItemCategory.Currency || Data.Item[itemNum].Stackable == 1)
            {
                totalQuantity += GetPlayerInvValue(playerId, invSlot);
            }
            else
            {
                totalQuantity += 1;
            }
        }

        return totalQuantity;
    }

    public static int FindItemSlot(int playerId, int itemNum)
    {
        if (itemNum is < 0 or > Core.Globals.Constant.MaxItems)
        {
            return -1;
        }

        for (var invSlot = 0; invSlot < Core.Globals.Constant.MaxInv; invSlot++)
        {
            if (GetPlayerInv(playerId, invSlot) == itemNum)
            {
                return invSlot;
            }
        }

        return -1;
    }

    public static void MapGetItem(int playerId)
    {
        var mapNum = GetPlayerMap(playerId);

        for (var mapItemNum = 0; mapItemNum < Core.Globals.Constant.MaxMapItems; mapItemNum++)
        {
            if (Data.MapItem[mapNum, mapItemNum].Num < 0 ||
                Data.MapItem[mapNum, mapItemNum].Num >= Core.Globals.Constant.MaxItems)
            {
                continue;
            }

            if (!CanPlayerPickupItem(playerId, mapItemNum))
            {
                continue;
            }

            if (Data.MapItem[mapNum, mapItemNum].X != GetPlayerX(playerId) || Data.MapItem[mapNum, mapItemNum].Y != GetPlayerY(playerId))
            {
                continue;
            }

            var slot = FindOpenInvSlot(playerId, Data.MapItem[mapNum, mapItemNum].Num);
            if (slot == -1)
            {
                NetworkSend.PlayerMsg(playerId, "Your inventory is full.", (int) ColorName.BrightRed);
                break;
            }

            try
            {
                Script.Instance?.MapGetItem(playerId, mapNum, mapItemNum, slot);
            }
            catch (Exception ex)
            {
                General.Logger.LogError(ex, "[Script] Error in {MethodName}", nameof(MapGetItem));
            }

            break;
        }
    }

    public static bool CanPlayerPickupItem(int playerId, int mapitemNum)
    {
        var mapNum = GetPlayerMap(playerId);

        if (Data.Map[mapNum].Moral < 0)
        {
            return false;
        }

        if (!Data.Moral[Data.Map[mapNum].Moral].CanPickupItem)
        {
            NetworkSend.PlayerMsg(playerId, "You can't pickup items here!", (int) ColorName.BrightRed);
            return false;
        }

        if (string.IsNullOrEmpty(Data.MapItem[mapNum, mapitemNum].PlayerName) ||
            Data.MapItem[mapNum, mapitemNum].PlayerName == GetPlayerName(playerId))
        {
            return true;
        }

        return false;
    }

    public static int FindOpenInvSlot(int playerId, int itemNum)
    {
        if (!NetworkConfig.IsPlaying(playerId) || itemNum < 0 || itemNum > Core.Globals.Constant.MaxItems)
        {
            return -1;
        }

        if (Data.Item[itemNum].Type == (byte) ItemCategory.Currency ||
            Data.Item[itemNum].Stackable == 1)
        {
            for (var invSlot = 0; invSlot < Core.Globals.Constant.MaxInv; invSlot++)
            {
                if (GetPlayerInv(playerId, invSlot) == itemNum)
                {
                    return invSlot;
                }
            }
        }

        for (var invSlot = 0; invSlot < Core.Globals.Constant.MaxInv; invSlot++)
        {
            if (GetPlayerInv(playerId, invSlot) == -1)
            {
                return invSlot;
            }
        }

        return -1;
    }

    public static bool TakeInv(int playerId, int itemNum, int itemVal)
    {
        if (!NetworkConfig.IsPlaying(playerId) || itemNum < 0 || itemNum > Core.Globals.Constant.MaxItems)
        {
            return false;
        }

        var clearInvSlot = false;

        for (var invSlot = 0; invSlot < Core.Globals.Constant.MaxInv; invSlot++)
        {
            // Check to see if the player has the item
            if (GetPlayerInv(playerId, invSlot) != itemNum)
            {
                continue;
            }

            if (Data.Item[itemNum].Type == (byte) ItemCategory.Currency ||
                Data.Item[itemNum].Stackable == 1)
            {
                // Is what we are trying to take away more then what they have?  If so just set it to zero
                if (itemVal >= GetPlayerInvValue(playerId, invSlot))
                {
                    clearInvSlot = true;
                }
                else
                {
                    SetPlayerInvValue(playerId, invSlot, GetPlayerInvValue(playerId, invSlot) - itemVal);

                    NetworkSend.SendInventoryUpdate(playerId, invSlot);
                }
            }
            else
            {
                clearInvSlot = true;
            }

            if (!clearInvSlot)
            {
                continue;
            }

            SetPlayerInv(playerId, invSlot, -1);
            SetPlayerInvValue(playerId, invSlot, 0);

            NetworkSend.SendInventoryUpdate(playerId, invSlot);

            return true;
        }

        return false;
    }

    public static bool GiveInv(int playerId, int itemNum, int itemVal, bool sendUpdate = true)
    {
        if (!NetworkConfig.IsPlaying(playerId) || itemNum < 0 || itemNum > Core.Globals.Constant.MaxItems)
        {
            return false;
        }

        var slot = FindOpenInvSlot(playerId, itemNum);
        if (slot == -1)
        {
            NetworkSend.PlayerMsg(playerId, "Your inventory is full.", (int) ColorName.BrightRed);
            return false;
        }

        itemVal = Math.Max(itemVal, 1);

        SetPlayerInv(playerId, slot, itemNum);
        SetPlayerInvValue(playerId, slot, GetPlayerInvValue(playerId, slot) + itemVal);

        if (sendUpdate)
        {
            NetworkSend.SendInventoryUpdate(playerId, slot);
        }

        return true;
    }

    public static void MapDropItem(int playerId, int invNum, int amount)
    {
        if (!NetworkConfig.IsPlaying(playerId) || invNum < 0 || invNum > Core.Globals.Constant.MaxInv)
        {
            return;
        }

        // Check the player isn't doing something
        if (Data.TempPlayer[playerId].InBank ||
            Data.TempPlayer[playerId].InShop >= 0 ||
            Data.TempPlayer[playerId].InTrade >= 0)
        {
            return;
        }

        if (!Data.Moral[GetPlayerMap(playerId)].CanDropItem)
        {
            NetworkSend.PlayerMsg(playerId, "You can't drop items here!", (int) ColorName.BrightRed);
            return;
        }

        var itemNum = GetPlayerInv(playerId, invNum);
        if (itemNum is < 0 or >= Core.Globals.Constant.MaxItems)
        {
            return;
        }

        var slot = Item.FindOpenMapItemSlot(GetPlayerMap(playerId));
        if (slot != -1)
        {
            var mapNum = GetPlayerMap(playerId);

            ref var item = ref Data.Item[itemNum];
            ref var mapItem = ref Data.MapItem[mapNum, slot];

            mapItem.Num = itemNum;
            mapItem.X = GetPlayerX(playerId);
            mapItem.Y = GetPlayerY(playerId);
            mapItem.PlayerName = GetPlayerName(playerId);
            mapItem.PlayerTimer = General.GetTimeMs() + Constant.ItemSpawnTime;
            mapItem.DespawnTimer = General.GetTimeMs() + Constant.ItemDespawnTime;
            mapItem.CanDespawn = true;

            try
            {
                Script.Instance?.MapDropItem(playerId, slot, invNum, amount, mapNum, item, itemNum);
            }
            catch (Exception ex)
            {
                General.Logger.LogError(ex, "[Script] Error in {MethodName}", nameof(MapDropItem));
            }
        }
        else
        {
            NetworkSend.PlayerMsg(playerId, "Too many items already on the ground.", (int) ColorName.Yellow);
        }
    }

    public static bool TakeInvSlot(int playerId, int invSlot, int itemVal)
    {
        var takeInvSlot = false;

        if (!NetworkConfig.IsPlaying(playerId) || invSlot < 0 || invSlot > Core.Globals.Constant.MaxItems)
        {
            return false;
        }

        var itemNum = GetPlayerInv(playerId, invSlot);

        if (Data.Item[itemNum].Type == (byte) ItemCategory.Currency ||
            Data.Item[itemNum].Stackable == 1)
        {
            // Is what we are trying to take away more then what they have?  If so just set it to zero
            if (itemVal >= GetPlayerInvValue(playerId, invSlot))
            {
                takeInvSlot = true;
            }
            else
            {
                SetPlayerInvValue(playerId, invSlot, GetPlayerInvValue(playerId, invSlot) - itemVal);
            }
        }
        else
        {
            takeInvSlot = true;
        }

        if (!takeInvSlot)
        {
            return false;
        }

        SetPlayerInv(playerId, invSlot, -1);
        SetPlayerInvValue(playerId, invSlot, 0);

        return true;
    }

    public static bool CanPlayerUseItem(int playerId, int itemNum)
    {
        if (Data.Map[GetPlayerMap(playerId)].Moral >= 0)
        {
            if (!Data.Moral[Data.Map[GetPlayerMap(playerId)].Moral].CanUseItem)
            {
                NetworkSend.PlayerMsg(playerId, "You can't use items here!", (int) ColorName.BrightRed);
                return false;
            }
        }

        var stats = Enum.GetValues<Stat>();
        foreach (var stat in stats)
        {
            if (GetPlayerStat(playerId, stat) >= Data.Item[itemNum].StatReq[(int) stat])
            {
                continue;
            }

            NetworkSend.PlayerMsg(playerId, "You do not meet the stat requirements to use this item.", (int) ColorName.BrightRed);
            return false;
        }

        if (Data.Item[itemNum].LevelReq > GetPlayerLevel(playerId))
        {
            NetworkSend.PlayerMsg(playerId, "You do not meet the level requirements to use this item.", (int) ColorName.BrightRed);
            return false;
        }

        if (Data.Item[itemNum].JobReq != -1 && Data.Item[itemNum].JobReq != GetPlayerJob(playerId))
        {
            NetworkSend.PlayerMsg(playerId, "You do not meet the job requirements to use this item.", (int) ColorName.BrightRed);
            return false;
        }

        if (GetPlayerAccess(playerId) < Data.Item[itemNum].AccessReq)
        {
            NetworkSend.PlayerMsg(playerId, "You do not meet the access requirement to equip this item.", (int) ColorName.BrightRed);
            return false;
        }

        if (!Data.TempPlayer[playerId].InBank && Data.TempPlayer[playerId].InShop < 0 && Data.TempPlayer[playerId].InTrade < 0)
        {
            return true;
        }

        NetworkSend.PlayerMsg(playerId, "You can't use items while in a bank, shop, or trade!", (int) ColorName.BrightRed);
        return false;
    }

    public static void UseItem(int playerId, int invNum)
    {
        if (invNum is < 0 or > Core.Globals.Constant.MaxInv)
        {
            return;
        }

        var itemNum = GetPlayerInv(playerId, invNum);
        if (itemNum is < 0 or > Core.Globals.Constant.MaxItems)
        {
            return;
        }

        if (!CanPlayerUseItem(playerId, itemNum))
        {
            return;
        }

        try
        {
            Script.Instance?.UseItem(playerId, itemNum, invNum);
        }
        catch (Exception ex)
        {
            General.Logger.LogError(ex, "[Script] Error in {MethodName}", nameof(UseItem));
        }
    }

    public static void PlayerSwitchInvSlots(int playerId, int oldSlot, int newSlot)
    {
        if (oldSlot == -1 || newSlot == -1)
        {
            return;
        }

        var oldNum = GetPlayerInv(playerId, oldSlot);
        var oldValue = GetPlayerInvValue(playerId, oldSlot);
        var newNum = GetPlayerInv(playerId, newSlot);
        var newValue = GetPlayerInvValue(playerId, newSlot);

        if (newNum >= 0)
        {
            if (oldNum == newNum & Data.Item[newNum].Stackable == 1) // Same item, if we can stack it, lets do that :P
            {
                SetPlayerInv(playerId, newSlot, newNum);
                SetPlayerInvValue(playerId, newSlot, oldValue + newValue);
                SetPlayerInv(playerId, oldSlot, 0);
                SetPlayerInvValue(playerId, oldSlot, 0);
            }
            else
            {
                SetPlayerInv(playerId, newSlot, oldNum);
                SetPlayerInvValue(playerId, newSlot, oldValue);
                SetPlayerInv(playerId, oldSlot, newNum);
                SetPlayerInvValue(playerId, oldSlot, newValue);
            }
        }
        else
        {
            SetPlayerInv(playerId, newSlot, oldNum);
            SetPlayerInvValue(playerId, newSlot, oldValue);
            SetPlayerInv(playerId, oldSlot, newNum);
            SetPlayerInvValue(playerId, oldSlot, newValue);
        }

        NetworkSend.SendInventory(playerId);
    }

    public static void PlayerSwitchSkillSlots(int playerId, int oldSlot, int newSlot)
    {
        if (oldSlot == -1 || newSlot == -1)
        {
            return;
        }

        var oldNum = GetPlayerSkill(playerId, oldSlot);
        var oldValue = GetPlayerSkillCd(playerId, oldSlot);
        var newNum = GetPlayerSkill(playerId, newSlot);
        var newValue = GetPlayerSkillCd(playerId, newSlot);

        if (newNum >= 0)
        {
            if (oldNum == newNum & Data.Item[newNum].Stackable == 1) // Same item, if we can stack it, lets do that :P
            {
                SetPlayerSkill(playerId, newSlot, newNum);
                SetPlayerSkillCd(playerId, newSlot, newValue);
                SetPlayerSkill(playerId, oldSlot, 0);
                SetPlayerSkillCd(playerId, oldSlot, 0);
            }
            else
            {
                SetPlayerSkill(playerId, newSlot, oldNum);
                SetPlayerSkillCd(playerId, newSlot, oldValue);
                SetPlayerSkill(playerId, oldSlot, newNum);
                SetPlayerSkillCd(playerId, oldSlot, newValue);
            }
        }
        else
        {
            SetPlayerSkill(playerId, newSlot, oldNum);
            SetPlayerSkillCd(playerId, newSlot, oldValue);
            SetPlayerSkill(playerId, oldSlot, newNum);
            SetPlayerSkillCd(playerId, oldSlot, newValue);
        }

        NetworkSend.SendPlayerSkills(playerId);
    }

    public static void CheckEquippedItems(int playerId)
    {
        var equipments = Enum.GetValues<Equipment>();

        foreach (var equipment in equipments)
        {
            var itemNum = GetPlayerEquipment(playerId, equipment);
            if (itemNum < 0)
            {
                SetPlayerEquipment(playerId, -1, equipment);
                continue;
            }

            if (Data.Item[itemNum].SubType != (byte) equipment)
            {
                SetPlayerEquipment(playerId, -1, equipment);
            }
        }
    }

    public static void UnequipItem(int playerId, int eqSlot)
    {
        var eqCount = Enum.GetNames<Equipment>().Length;
        if (eqSlot < 1 || eqSlot > eqCount)
        {
            return;
        }

        var itemNum = GetPlayerEquipment(playerId, (Equipment) eqSlot);
        if (itemNum is < 0 or > Core.Globals.Constant.MaxItems)
        {
            return;
        }

        if (FindOpenInvSlot(playerId, itemNum) >= 0)
        {
            try
            {
                Script.Instance?.UnequipItem(playerId);
            }
            catch (Exception ex)
            {
                General.Logger.LogError(ex, "[Script] Error in {MethodName}", nameof(UnequipItem));
            }
        }
        else
        {
            NetworkSend.PlayerMsg(playerId, "Your inventory is full.", (int) ColorName.BrightRed);
        }
    }

    public static void JoinGame(int playerId)
    {
        try
        {
            Script.Instance?.JoinGame(playerId);

            General.UpdateCaption();
        }
        catch (Exception ex)
        {
            General.Logger.LogError(ex, "[Script] Error in {MethodName}", nameof(JoinGame));
        }
    }

    public static async Task LeftGame(int playerId)
    {
        General.Logger.LogInformation("{AccountName} | {PlayerName} has stopped playing {GameName}",
            GetAccountLogin(playerId), GetPlayerName(playerId),
            SettingsManager.Instance.GameName);
        
        try
        {
            Script.Instance?.LeftGame(playerId);
        }
        catch (Exception ex)
        {
            General.Logger.LogError(ex, "[Script] Error in {MethodName}", nameof(LeftGame));
        }

        if (Data.TempPlayer[playerId].InGame)
        {
            await Database.SaveCharacterAsync(playerId, Data.TempPlayer[playerId].Slot);
            await Database.SaveBankAsync(playerId);
        }
        
        Database.ClearPlayer(playerId);

        PlayerService.Instance.RemovePlayer(playerId);
        
        Data.TempPlayer[playerId].InGame = false;
        
        General.UpdateCaption();
    }

    public static int KillPlayer(int playerId)
    {
        try
        {
            return Script.Instance?.KillPlayer(playerId);
        }
        catch (Exception ex)
        {
            General.Logger.LogError(ex, "[Script] Error in {MethodName}", nameof(KillPlayer));
        }

        return 0;
    }

    public static void GiveBank(int playerId, int invSlot, int amount)
    {
        if (invSlot is < 0 or > Core.Globals.Constant.MaxInv)
        {
            return;
        }

        amount = Math.Max(amount, 0);
        if (GetPlayerInvValue(playerId, invSlot) < amount && GetPlayerInv(playerId, invSlot) == 0)
        {
            return;
        }

        var bankSlot = FindOpenbankSlot(playerId, GetPlayerInv(playerId, invSlot));
        if (bankSlot == -1)
        {
            return;
        }

        var itemNum = GetPlayerInv(playerId, invSlot);

        if (Data.Item[GetPlayerInv(playerId, invSlot)].Type == (byte) ItemCategory.Currency ||
            Data.Item[GetPlayerInv(playerId, invSlot)].Stackable == 1)
        {
            if (GetPlayerBank(playerId, bankSlot) == GetPlayerInv(playerId, invSlot))
            {
                SetPlayerBankValue(playerId, bankSlot, GetPlayerBankValue(playerId, bankSlot) + amount);

                TakeInv(playerId, GetPlayerInv(playerId, invSlot), amount);
            }
            else
            {
                SetPlayerBank(playerId, bankSlot, GetPlayerInv(playerId, invSlot));
                SetPlayerBankValue(playerId, bankSlot, amount);

                TakeInv(playerId, GetPlayerInv(playerId, invSlot), amount);
            }
        }
        else if (GetPlayerBank(playerId, bankSlot) == GetPlayerInv(playerId, invSlot))
        {
            SetPlayerBankValue(playerId, bankSlot, GetPlayerBankValue(playerId, bankSlot) + 1);

            TakeInv(playerId, GetPlayerInv(playerId, invSlot), 0);
        }
        else
        {
            SetPlayerBank(playerId, bankSlot, itemNum);
            SetPlayerBankValue(playerId, bankSlot, 1);

            TakeInv(playerId, GetPlayerInv(playerId, invSlot), 0);
        }

        NetworkSend.SendBank(playerId);
    }

    public static int GetPlayerBank(int playerId, int bankSlot)
    {
        return Data.Bank[playerId].Item[bankSlot].Num;
    }

    public static void SetPlayerBank(int playerId, int bankSlot, int itemNum)
    {
        Data.Bank[playerId].Item[bankSlot].Num = itemNum;
    }

    public static int GetPlayerBankValue(int playerId, int bankSlot)
    {
        return Data.Bank[playerId].Item[bankSlot].Value;
    }

    public static void SetPlayerBankValue(int playerId, int bankSlot, int value)
    {
        Data.Bank[playerId].Item[bankSlot].Value = value;
    }

    public static int FindOpenbankSlot(int playerId, int itemNum)
    {
        if (!NetworkConfig.IsPlaying(playerId) || itemNum is < 0 or > Core.Globals.Constant.MaxItems)
        {
            return -1;
        }

        if (Data.Item[itemNum].Type == (byte) ItemCategory.Currency ||
            Data.Item[itemNum].Stackable == 1)
        {
            for (var bankSlot = 0; bankSlot < Core.Globals.Constant.MaxBank; bankSlot++)
            {
                if (GetPlayerBank(playerId, bankSlot) == itemNum)
                {
                    return bankSlot;
                }
            }
        }

        for (var bankSlot = 0; bankSlot < Core.Globals.Constant.MaxBank; bankSlot++)
        {
            if (GetPlayerBank(playerId, bankSlot) == -1)
            {
                return bankSlot;
            }
        }

        return -1;
    }

    public static void TakeBank(int playerId, int bankSlot, int amount)
    {
        if (bankSlot is < 0 or > Core.Globals.Constant.MaxBank)
        {
            return;
        }

        amount = Math.Max(amount, 0);
        if (GetPlayerBankValue(playerId, bankSlot) < amount)
        {
            return;
        }

        var invSlot = FindOpenInvSlot(playerId, GetPlayerBank(playerId, bankSlot));

        if (invSlot >= 0)
        {
            if (Data.Item[GetPlayerBank(playerId, bankSlot)].Type == (byte) ItemCategory.Currency ||
                Data.Item[GetPlayerBank(playerId, bankSlot)].Stackable == 1)
            {
                GiveInv(playerId, GetPlayerBank(playerId, bankSlot), amount);
                SetPlayerBankValue(playerId, bankSlot, GetPlayerBankValue(playerId, bankSlot) - amount);

                if (GetPlayerBankValue(playerId, bankSlot) < 0)
                {
                    SetPlayerBank(playerId, bankSlot, 0);
                    SetPlayerBankValue(playerId, bankSlot, 0);
                }
            }
            else if (GetPlayerBank(playerId, bankSlot) == GetPlayerInv(playerId, invSlot))
            {
                if (GetPlayerBankValue(playerId, bankSlot) > 1)
                {
                    GiveInv(playerId, GetPlayerBank(playerId, bankSlot), 0);
                    SetPlayerBankValue(playerId, bankSlot, GetPlayerBankValue(playerId, bankSlot) - 1);
                }
            }
            else
            {
                GiveInv(playerId, GetPlayerBank(playerId, bankSlot), 0);
                SetPlayerBank(playerId, bankSlot, -1);
                SetPlayerBankValue(playerId, bankSlot, 0);
            }
        }

        NetworkSend.SendBank(playerId);
    }

    public static void PlayerSwitchbankSlots(int playerId, int oldSlot, int newSlot)
    {
        if (oldSlot == -1 | newSlot == -1)
        {
            return;
        }

        var oldNum = GetPlayerBank(playerId, oldSlot);
        var oldValue = GetPlayerBankValue(playerId, oldSlot);
        var newNum = GetPlayerBank(playerId, newSlot);
        var newValue = GetPlayerBankValue(playerId, newSlot);

        SetPlayerBank(playerId, newSlot, oldNum);
        SetPlayerBankValue(playerId, newSlot, oldValue);

        SetPlayerBank(playerId, oldSlot, newNum);
        SetPlayerBankValue(playerId, oldSlot, newValue);

        NetworkSend.SendBank(playerId);
    }
}