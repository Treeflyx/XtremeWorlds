using Core;
using Microsoft.Extensions.Logging;
using Server.Game;
using Server.Game.Net;
using Server.Net;
using static Core.Global.Command;
using static Core.Packets;

namespace Server;

public static class Npc
{
    public static async Task SpawnAllMapNpcs()
    {
        await Task.WhenAll(Enumerable
            .Range(0, Core.Constant.MaxMapNpcs)
            .Select(SpawnMapNpcs));
    }

    public static async Task SpawnMapNpcs(int mapNum)
    {
        await Task.WhenAll(Enumerable
            .Range(0, Core.Constant.MaxMapNpcs)
            .Select(mapNpcNum => Task.Run(() =>
                SpawnNpc(mapNpcNum, mapNum))));
    }

    public static void SpawnNpc(int mapNpcNum, int mapNum)
    {
        var spawned = false;

        if (Data.Map[mapNum].NoRespawn)
        {
            return;
        }

        var npcNum = Data.Map[mapNum].Npc[mapNpcNum];
        if (mapNpcNum == 0 || npcNum < 0 || npcNum > Core.Constant.MaxNpcs)
        {
            return;
        }

        if (Data.Npc[npcNum].SpawnTime != (byte) Clock.Instance.TimeOfDay && Data.Npc[npcNum].SpawnTime != 0)
        {
            Database.ClearMapNpc(mapNpcNum, mapNum);

            SendMapNpcsToMap(mapNum);

            return;
        }

        Data.MapNpc[mapNum].Npc[mapNpcNum].Num = npcNum;
        Data.MapNpc[mapNum].Npc[mapNpcNum].Target = 0;
        Data.MapNpc[mapNum].Npc[mapNpcNum].TargetType = 0; // Clear

        var vitals = Enum.GetValues<Vital>();
        foreach (var vital in vitals)
        {
            Data.MapNpc[mapNum].Npc[mapNpcNum].Vital[(int) vital] = GameLogic.GetNpcMaxVital(npcNum, vital);
        }
        
        Data.MapNpc[mapNum].Npc[mapNpcNum].Dir = (byte) (Random.Shared.NextDouble() * 4f);

        for (var x = 0; x < Data.Map[mapNum].MaxX; x++)
        {
            for (var y = 0; y < Data.Map[mapNum].MaxY; y++)
            {
                if (Data.Map[mapNum].Tile[x, y].Type != TileType.NpcSpawn ||
                    Data.Map[mapNum].Tile[x, y].Data1 != mapNpcNum)
                {
                    continue;
                }

                Data.MapNpc[mapNum].Npc[mapNpcNum].X = x * 32;
                Data.MapNpc[mapNum].Npc[mapNpcNum].Dir = (byte) Data.Map[mapNum].Tile[x, y].Data2;

                spawned = true;
                break;
            }
        }

        if (!spawned)
        {
            var i = 0;
            while (i < 1000)
            {
                var x = (int) Math.Round(General.GetRandom.NextDouble(0d, Data.Map[mapNum].MaxX - 1));
                var y = (int) Math.Round(General.GetRandom.NextDouble(0d, Data.Map[mapNum].MaxY - 1));

                if (x > Data.Map[mapNum].MaxX) x = Data.Map[mapNum].MaxX - 1;
                if (y > Data.Map[mapNum].MaxY) y = Data.Map[mapNum].MaxY - 1;

                if (NpcTileIsOpen(mapNum, x, y))
                {
                    Data.MapNpc[mapNum].Npc[mapNpcNum].X = x * 32;
                    Data.MapNpc[mapNum].Npc[mapNpcNum].Y = y * 32;

                    spawned = true;
                    break;
                }

                ++i;
            }
        }

        // Didn't spawn, so now we'll just try to find a free tile
        if (!spawned)
        {
            for (var x = 0; x < Data.Map[mapNum].MaxX; x++)
            {
                for (var y = 0; y < Data.Map[mapNum].MaxY; y++)
                {
                    if (!NpcTileIsOpen(mapNum, x, y))
                    {
                        continue;
                    }

                    Data.MapNpc[mapNum].Npc[mapNpcNum].X = x * 32;
                    Data.MapNpc[mapNum].Npc[mapNpcNum].Y = y * 32;

                    spawned = true;
                }
            }
        }

        // If we suceeded in spawning then send it to everyone
        if (spawned)
        {
            var packet = new PacketWriter();

            packet.WriteInt32((int) ServerPackets.SSpawnNpc);
            packet.WriteInt32(mapNpcNum);
            packet.WriteInt32(Data.MapNpc[mapNum].Npc[mapNpcNum].Num);
            packet.WriteInt32(Data.MapNpc[mapNum].Npc[mapNpcNum].X);
            packet.WriteInt32(Data.MapNpc[mapNum].Npc[mapNpcNum].Y);
            packet.WriteByte(Data.MapNpc[mapNum].Npc[mapNpcNum].Dir);

            var vitalCount = Enum.GetValues<Vital>().Length;
            for (var i = 0; i < vitalCount; i++)
            {
                packet.WriteInt32(Data.MapNpc[mapNum].Npc[mapNpcNum].Vital[i]);
            }

            NetworkConfig.SendDataToMap(mapNum, packet.GetBytes());
        }

        SendMapNpcVitals(mapNum, (byte) mapNpcNum);
    }

    public static bool NpcTileIsOpen(int mapNum, int x, int y)
    {
        foreach (var playerId in PlayerService.Instance.PlayerIds)
        {
            if (GetPlayerMap(playerId) == mapNum &&
                GetPlayerX(playerId) == x &&
                GetPlayerY(playerId) == y)
            {
                return false;
            }
        }

        for (var mapNpcNum = 0; mapNpcNum < Core.Constant.MaxMapNpcs; mapNpcNum++)
        {
            if (Data.MapNpc[mapNum].Npc[mapNpcNum].Num >= 0 &&
                Data.MapNpc[mapNum].Npc[mapNpcNum].X == x &&
                Data.MapNpc[mapNum].Npc[mapNpcNum].Y == y)
            {
                return false;
            }
        }

        if (Data.Map[mapNum].Tile[x, y].Type != TileType.NpcSpawn &&
            Data.Map[mapNum].Tile[x, y].Type != TileType.Item &&
            Data.Map[mapNum].Tile[x, y].Type != TileType.None &&
            Data.Map[mapNum].Tile[x, y].Type2 != TileType.NpcSpawn &&
            Data.Map[mapNum].Tile[x, y].Type2 != TileType.Item &&
            Data.Map[mapNum].Tile[x, y].Type2 != TileType.None)
        {
            return false;
        }

        return true;
    }

    public static bool CanNpcMove(int mapNum, int mapNpcNum, byte dir)
    {
        bool canNpcMoveRet = false;
        int n;
        int n2;
        int npcX;
        int npcY;

        if (mapNum < 0 || mapNum > Core.Constant.MaxMaps || mapNpcNum < 0 || mapNpcNum >= Core.Constant.MaxMapNpcs || dir > (byte) Direction.DownRight)
        {
            return canNpcMoveRet;
        }

        var x = Data.MapNpc[mapNum].Npc[mapNpcNum].X;
        var y = Data.MapNpc[mapNum].Npc[mapNpcNum].Y;

        var tileX = (int) Math.Floor((double) x / 32);
        var tileY = (int) Math.Floor((double) y / 32);

        canNpcMoveRet = true;

        switch (dir)
        {
            case (byte) Direction.Up:
            {
                // Check to make sure not outside of boundaries
                if (tileY > 0)
                {
                    n = (int) Data.Map[mapNum].Tile[tileX, tileY - 1].Type;
                    n2 = (int) Data.Map[mapNum].Tile[tileX, tileY - 1].Type2;

                    // Check to make sure that the tile is walkable
                    if (n != (byte) TileType.None &&
                        n != (byte) TileType.Item &&
                        n != (byte) TileType.NpcSpawn &&
                        n2 != (byte) TileType.None &&
                        n2 != (byte) TileType.Item &&
                        n2 != (byte) TileType.NpcSpawn)
                    {
                        return false;
                    }

                    // Check to make sure that there is not a player in the way
                    foreach (var i in PlayerService.Instance.PlayerIds)
                    {
                        if (GetPlayerMap(i) == mapNum && GetPlayerX(i) == tileX & GetPlayerY(i) == tileY - 1)
                        {
                            return false;
                        }
                    }

                    // Check to make sure that there is not another npc in the way
                    for (var i = 0; i < Core.Constant.MaxMapNpcs; i++)
                    {
                        npcX = (int) Math.Floor((double) Data.MapNpc[mapNum].Npc[i].X / 32);
                        npcY = (int) Math.Floor((double) Data.MapNpc[mapNum].Npc[i].Y / 32);

                        if (i != mapNpcNum && Data.MapNpc[mapNum].Npc[i].Num >= 0 && npcX == tileX && npcY == tileY - 1)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    canNpcMoveRet = false;
                }

                break;
            }

            case (byte) Direction.Down:
            {
                if (tileY < Data.Map[mapNum].MaxY - 1)
                {
                    n = (int) Data.Map[mapNum].Tile[tileX, tileY + 1].Type;
                    n2 = (int) Data.Map[mapNum].Tile[tileX, tileY + 1].Type2;

                    if (n != (byte) TileType.None & n != (byte) TileType.Item & n != (byte) TileType.NpcSpawn & n2 != (byte) TileType.None & n2 != (byte) TileType.Item & n2 != (byte) TileType.NpcSpawn)
                    {
                        return false;
                    }

                    foreach (var i in PlayerService.Instance.PlayerIds)
                    {
                        if (GetPlayerMap(i) == mapNum && GetPlayerX(i) == tileX && GetPlayerY(i) == tileY + 1)
                        {
                            return false;
                        }
                    }

                    for (var i = 0; i < Core.Constant.MaxMapNpcs; i++)
                    {
                        npcX = (int) Math.Floor((double) Data.MapNpc[mapNum].Npc[i].X / 32);
                        npcY = (int) Math.Floor((double) Data.MapNpc[mapNum].Npc[i].Y / 32);
                        
                        if (i != mapNpcNum & Data.MapNpc[mapNum].Npc[i].Num >= 0 & npcX == tileX & npcY == tileY + 1)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    canNpcMoveRet = false;
                }

                break;
            }

            case (byte) Direction.Left:
            {
                if (tileX > 0)
                {
                    n = (int) Data.Map[mapNum].Tile[tileX - 1, tileY].Type;
                    n2 = (int) Data.Map[mapNum].Tile[tileX - 1, tileY].Type2;

                    if (n != (byte) TileType.None & n != (byte) TileType.Item & n != (byte) TileType.NpcSpawn & n2 != (byte) TileType.None & n2 != (byte) TileType.Item & n2 != (byte) TileType.NpcSpawn)
                    {
                        return false;
                    }

                    foreach (var i in PlayerService.Instance.PlayerIds)
                    {
                        if (GetPlayerMap(i) == mapNum && GetPlayerX(i) == tileX - 1 && GetPlayerY(i) == tileY)
                        {
                            return false;
                        }
                    }
                    
                    for (var i = 0; i < Core.Constant.MaxMapNpcs; i++)
                    {
                        npcX = (int) Math.Floor((double) Data.MapNpc[mapNum].Npc[i].X / 32);
                        npcY = (int) Math.Floor((double) Data.MapNpc[mapNum].Npc[i].Y / 32);
                        
                        if (i != mapNpcNum & Data.MapNpc[mapNum].Npc[i].Num >= 0 & npcX == tileX - 1 & npcY == tileY)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    canNpcMoveRet = false;
                }

                break;
            }

            case (byte) Direction.Right:
            {
                if (tileX < Data.Map[mapNum].MaxX - 1)
                {
                    n = (int) Data.Map[mapNum].Tile[tileX + 1, tileY].Type;
                    n2 = (int) Data.Map[mapNum].Tile[tileX + 1, tileY].Type2;

                    if (n != (byte) TileType.None & n != (byte) TileType.Item & n != (byte) TileType.NpcSpawn & n2 != (byte) TileType.None & n2 != (byte) TileType.Item & n2 != (byte) TileType.NpcSpawn)
                    {
                        return false;
                    }

                    foreach (var playerId in PlayerService.Instance.PlayerIds)
                    {
                        if (GetPlayerMap(playerId) == mapNum & GetPlayerX(playerId) == tileX + 1 & GetPlayerY(playerId) == tileY)
                        {
                            return false;
                        }
                    }
                    
                    for (var i = 0; i < Core.Constant.MaxMapNpcs; i++)
                    {
                        npcX = (int) Math.Floor((double) Data.MapNpc[mapNum].Npc[i].X / 32);
                        npcY = (int) Math.Floor((double) Data.MapNpc[mapNum].Npc[i].Y / 32);
                        
                        if (i != mapNpcNum & Data.MapNpc[mapNum].Npc[i].Num >= 0 & npcX == tileX + 1 & npcY == tileY)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    canNpcMoveRet = false;
                }

                break;
            }
        }

        if (Data.MapNpc[mapNum].Npc[mapNpcNum].SkillBuffer >= 0)
        {
            return false;
        }

        return canNpcMoveRet;
    }

    public static void NpcMove(int mapNum, int mapNpcNum, byte dir, int movement)
    {
        // MovementState enum count
        var count = System.Enum.GetValues(typeof(MovementState)).Length;
        if (mapNum < 0 || mapNum > Core.Constant.MaxMaps || mapNpcNum < 0 || mapNpcNum >= Core.Constant.MaxMapNpcs || dir > (byte)Direction.DownRight || movement < 0 || movement > count)
        {
            return;
        }

        // Check if the intended move would go off map boundaries
        int nextX = Data.MapNpc[mapNum].Npc[mapNpcNum].X;
        int nextY = Data.MapNpc[mapNum].Npc[mapNpcNum].Y;

        switch (dir)
        {
            case (byte)Direction.Up:
                nextY -= 1;
                break;
            case (byte)Direction.Down:
                nextY += 1;
                break;
            case (byte)Direction.Left:
                nextX -= 1;
                break;
            case (byte)Direction.Right:
                nextX += 1;
                break;
        }

        if (nextX < 0 || nextY < 0 || nextX >= Data.Map[mapNum].MaxX * 32 || nextY >= Data.Map[mapNum].MaxY * 32)
        {
            Data.MapNpc[mapNum].Npc[mapNpcNum].Moving = 0;
            return;
        }

        Data.MapNpc[mapNum].Npc[mapNpcNum].Dir = dir;
        Data.MapNpc[mapNum].Npc[mapNpcNum].Y = nextY;

        var buffer = new PacketWriter(4);

        buffer.WriteEnum(ServerPackets.SNpcMove);
        buffer.WriteInt32(mapNpcNum);
        buffer.WriteInt32(Data.MapNpc[mapNum].Npc[mapNpcNum].X);
        buffer.WriteInt32(Data.MapNpc[mapNum].Npc[mapNpcNum].Y);
        buffer.WriteByte(Data.MapNpc[mapNum].Npc[mapNpcNum].Dir);
        buffer.WriteInt32(movement);

        NetworkConfig.SendDataToMap(mapNum, buffer.GetBytes());
    }

    public static void NpcDir(int mapNum, int mapNpcNum, byte dir)
    {
        if (mapNum < 0 || mapNum > Core.Constant.MaxMaps || mapNpcNum < 0 || mapNpcNum >= Core.Constant.MaxMapNpcs || dir > (byte) Direction.DownRight)
        {
            return;
        }

        Data.MapNpc[mapNum].Npc[mapNpcNum].Dir = dir;

        var packet = new PacketWriter(9);

        packet.WriteEnum(ServerPackets.SNpcDir);
        packet.WriteInt32(mapNpcNum);
        packet.WriteByte(dir);

        NetworkConfig.SendDataToMap(mapNum, packet.GetBytes());
    }

    private static void SendMapNpcsToMap(int mapNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SMapNpcData);

        for (var mapNpcNum = 0; mapNpcNum < Core.Constant.MaxMapNpcs; mapNpcNum++)
        {
            packet.WriteInt32(Data.MapNpc[mapNum].Npc[mapNpcNum].Num);
            packet.WriteInt32(Data.MapNpc[mapNum].Npc[mapNpcNum].X);
            packet.WriteInt32(Data.MapNpc[mapNum].Npc[mapNpcNum].Y);
            packet.WriteByte(Data.MapNpc[mapNum].Npc[mapNpcNum].Dir);
        }

        NetworkConfig.SendDataToMap(mapNum, packet.GetBytes());
    }

    public static void HandleRequestEditNpc(GameSession session, ReadOnlySpan<byte> bytes)
    {
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
        {
            return;
        }

        var user = IsEditorLocked(session.Id, EditorType.Npc);
        if (!string.IsNullOrEmpty(user))
        {
            NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) Color.BrightRed);
            return;
        }

        Data.TempPlayer[session.Id].Editor = EditorType.Npc;

        Item.SendItems(session.Id);
        Animation.SendAnimations(session.Id);
        NetworkSend.SendSkills(session.Id);

        SendNpcs(session.Id);

        var packet = new PacketWriter(4);

        packet.WriteEnum(ServerPackets.SNpcEditor);

        PlayerService.Instance.SendDataTo(session.Id, packet.GetBytes());
    }

    public static void HandleSaveNpc(GameSession session, ReadOnlySpan<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
        {
            return;
        }

        var npcNum = packetReader.ReadInt32();
        if (npcNum < 0 | npcNum > Core.Constant.MaxNpcs)
        {
            return;
        }

        Data.Npc[npcNum].Animation = packetReader.ReadInt32();
        Data.Npc[npcNum].AttackSay = packetReader.ReadString();
        Data.Npc[npcNum].Behaviour = packetReader.ReadByte();

        for (var i = 0; i < Core.Constant.MaxDropItems; i++)
        {
            Data.Npc[npcNum].DropChance[i] = packetReader.ReadInt32();
            Data.Npc[npcNum].DropItem[i] = packetReader.ReadInt32();
            Data.Npc[npcNum].DropItemValue[i] = packetReader.ReadInt32();
        }

        Data.Npc[npcNum].Exp = packetReader.ReadInt32();
        Data.Npc[npcNum].Faction = packetReader.ReadByte();
        Data.Npc[npcNum].Hp = packetReader.ReadInt32();
        Data.Npc[npcNum].Name = packetReader.ReadString();
        Data.Npc[npcNum].Range = packetReader.ReadByte();
        Data.Npc[npcNum].SpawnTime = packetReader.ReadByte();
        Data.Npc[npcNum].SpawnSecs = packetReader.ReadInt32();
        Data.Npc[npcNum].Sprite = packetReader.ReadInt32();

        var statCount = Enum.GetValues<Stat>().Length;
        for (var i = 0; i < statCount; i++)
        {
            Data.Npc[npcNum].Stat[i] = packetReader.ReadByte();
        }

        for (var i = 0; i < Core.Constant.MaxNpcSkills; i++)
        {
            Data.Npc[npcNum].Skill[i] = packetReader.ReadByte();
        }

        Data.Npc[npcNum].Level = packetReader.ReadByte();
        Data.Npc[npcNum].Damage = packetReader.ReadInt32();

        Database.SaveNpc(npcNum);

        General.Logger.LogInformation("{AccountName} saved NPC #{NpcNum}",
            GetAccountLogin(session.Id), npcNum);

        SendUpdateNpcToAll(npcNum);
    }

    public static void SendNpcs(int playerId)
    {
        for (var npcNum = 0; npcNum < Core.Constant.MaxNpcs; npcNum++)
        {
            if (Data.Npc[npcNum].Name.Length > 0)
            {
                SendUpdateNpcTo(playerId, npcNum);
            }
        }
    }

    public static void SendUpdateNpcTo(int playerId, int npcNum)
    {
        var buffer = new PacketWriter();

        buffer.WriteEnum(ServerPackets.SUpdateNpc);
        buffer.WriteInt32(npcNum);
        buffer.WriteInt32(Data.Npc[npcNum].Animation);
        buffer.WriteString(Data.Npc[npcNum].AttackSay);
        buffer.WriteByte(Data.Npc[npcNum].Behaviour);

        for (var i = 0; i < Core.Constant.MaxDropItems; i++)
        {
            buffer.WriteInt32(Data.Npc[npcNum].DropChance[i]);
            buffer.WriteInt32(Data.Npc[npcNum].DropItem[i]);
            buffer.WriteInt32(Data.Npc[npcNum].DropItemValue[i]);
        }

        buffer.WriteInt32(Data.Npc[npcNum].Exp);
        buffer.WriteByte(Data.Npc[npcNum].Faction);
        buffer.WriteInt32(Data.Npc[npcNum].Hp);
        buffer.WriteString(Data.Npc[npcNum].Name);
        buffer.WriteByte(Data.Npc[npcNum].Range);
        buffer.WriteByte(Data.Npc[npcNum].SpawnTime);
        buffer.WriteInt32(Data.Npc[npcNum].SpawnSecs);
        buffer.WriteInt32(Data.Npc[npcNum].Sprite);

        var statCount = Enum.GetValues<Stat>().Length;
        for (var i = 0; i < statCount; i++)
        {
            buffer.WriteByte(Data.Npc[npcNum].Stat[i]);
        }

        for (var i = 0; i < Core.Constant.MaxNpcSkills; i++)
        {
            buffer.WriteByte(Data.Npc[npcNum].Skill[i]);
        }

        buffer.WriteInt32(Data.Npc[npcNum].Level);
        buffer.WriteInt32(Data.Npc[npcNum].Damage);

        PlayerService.Instance.SendDataTo(playerId, buffer.GetBytes());
    }

    private static void SendUpdateNpcToAll(int npcNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SUpdateNpc);
        packet.WriteInt32(npcNum);
        packet.WriteInt32(Data.Npc[npcNum].Animation);
        packet.WriteString(Data.Npc[npcNum].AttackSay);
        packet.WriteByte(Data.Npc[npcNum].Behaviour);

        for (var i = 0; i < Core.Constant.MaxDropItems; i++)
        {
            packet.WriteInt32(Data.Npc[npcNum].DropChance[i]);
            packet.WriteInt32(Data.Npc[npcNum].DropItem[i]);
            packet.WriteInt32(Data.Npc[npcNum].DropItemValue[i]);
        }

        packet.WriteInt32(Data.Npc[npcNum].Exp);
        packet.WriteByte(Data.Npc[npcNum].Faction);
        packet.WriteInt32(Data.Npc[npcNum].Hp);
        packet.WriteString(Data.Npc[npcNum].Name);
        packet.WriteByte(Data.Npc[npcNum].Range);
        packet.WriteByte(Data.Npc[npcNum].SpawnTime);
        packet.WriteInt32(Data.Npc[npcNum].SpawnSecs);
        packet.WriteInt32(Data.Npc[npcNum].Sprite);

        var statCount = Enum.GetValues<Stat>().Length;
        for (var i = 0; i < statCount; i++)
        {
            packet.WriteByte(Data.Npc[npcNum].Stat[i]);
        }

        for (var i = 0; i < Core.Constant.MaxNpcSkills; i++)
        {
            packet.WriteByte(Data.Npc[npcNum].Skill[i]);
        }

        packet.WriteInt32(Data.Npc[npcNum].Level);
        packet.WriteInt32(Data.Npc[npcNum].Damage);

        PlayerService.Instance.SendDataToAll(packet.GetBytes());
    }

    private static void SendMapNpcVitals(int mapNum, byte mapNpcNum)
    {
        var packet = new PacketWriter(4);

        packet.WriteInt32((int) ServerPackets.SMapNpcVitals);
        packet.WriteInt32(mapNpcNum);

        var vitalCount = Enum.GetValues<Vital>().Length;
        for (var i = 0; i < vitalCount; i++)
        {
            packet.WriteInt32(Data.MapNpc[mapNum].Npc[mapNpcNum].Vital[i]);
        }

        NetworkConfig.SendDataToMap(mapNum, packet.GetBytes());
    }
}