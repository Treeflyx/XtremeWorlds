using Core;
using Core.Globals;
using Core.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Game;
using Server.Game.Net;
using Server.Net;
using static Core.Globals.Command;
using static Core.Net.Packets;
using Type = Core.Globals.Type;

namespace Server;

public static class Item
{
    private static void SaveItem(int itemNum)
    {
        var json = JsonConvert.SerializeObject(Data.Item[itemNum]);

        if (Database.RowExists(itemNum, "item"))
        {
            Database.UpdateRow(itemNum, json, "item", "data");
        }
        else
        {
            Database.InsertRow(itemNum, json, "item");
        }
    }

    public static async Task LoadItemsAsync()
    {
        await Parallel.ForEachAsync(Enumerable.Range(0, Core.Globals.Constant.MaxItems), LoadItemAsync);
    }

    private static async ValueTask LoadItemAsync(int itemNum, CancellationToken cancellationToken)
    {
        var data = await Database.SelectRowAsync(itemNum, "item", "data");
        if (data is null)
        {
            ClearItem(itemNum);
            return;
        }

        var itemData = JObject.FromObject(data).ToObject<Type.Item>();

        Data.Item[itemNum] = itemData;
    }

    private static void ClearItem(int itemNum)
    {
        Data.Item[itemNum].Name = "";
        Data.Item[itemNum].Description = "";
        Data.Item[itemNum].Stackable = 1;
    }

    public static void SendMapItemsToAll(int mapNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SMapItemData);

        for (var i = 0; i < Core.Globals.Constant.MaxMapItems; i++)
        {
            packet.WriteInt32(Data.MapItem[mapNum, i].Num);
            packet.WriteInt32(Data.MapItem[mapNum, i].Value);
            packet.WriteInt32(Data.MapItem[mapNum, i].X);
            packet.WriteInt32(Data.MapItem[mapNum, i].Y);
        }

        NetworkConfig.SendDataToMap(mapNum, packet.GetBytes());
    }

    public static void SpawnItem(int itemNum, int itemVal, int mapNum, int x, int y)
    {
        if (itemNum < 0 || itemNum > Core.Globals.Constant.MaxItems || mapNum < 0 || mapNum >= Core.Globals.Constant.MaxMaps)
        {
            return;
        }

        var slot = FindOpenMapItemSlot(mapNum);
        if (slot == -1)
        {
            return;
        }

        SpawnItemSlot(slot, itemNum, itemVal, mapNum, x, y);
    }

    public static void SpawnItemSlot(int mapItemSlot, int itemNum, int itemVal, int mapNum, int x, int y)
    {
        if (mapItemSlot < 0 || mapItemSlot > Core.Globals.Constant.MaxMapItems || itemNum < 0 || itemNum > Core.Globals.Constant.MaxItems || mapNum < 0 || mapNum >= Core.Globals.Constant.MaxMaps)
        {
            return;
        }

        Data.MapItem[mapNum, mapItemSlot].Num = itemNum;
        Data.MapItem[mapNum, mapItemSlot].Value = itemVal;
        Data.MapItem[mapNum, mapItemSlot].X = x * 32;
        Data.MapItem[mapNum, mapItemSlot].Y = y * 32;

        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SSpawnItem);
        packet.WriteInt32(mapItemSlot);
        packet.WriteInt32(itemNum);
        packet.WriteInt32(itemVal);
        packet.WriteInt32(x);
        packet.WriteInt32(y);

        NetworkConfig.SendDataToMap(mapNum, packet.GetBytes());
    }

    public static int FindOpenMapItemSlot(int mapNum)
    {
        if (mapNum is < 0 or > Core.Globals.Constant.MaxMaps)
        {
            return -1;
        }

        for (var mapItemNum = 0; mapItemNum < Core.Globals.Constant.MaxMapItems; mapItemNum++)
        {
            if (Data.MapItem[mapNum, mapItemNum].Num == -1)
            {
                return mapItemNum;
            }
        }

        return -1;
    }

    public static void SpawnAllMapsItems()
    {
        for (var mapNum = 0; mapNum < Core.Globals.Constant.MaxMaps; mapNum++)
        {
            SpawnMapItems(mapNum);
        }
    }

    public static void SpawnMapItems(int mapNum)
    {
        if (mapNum is < 0 or > Core.Globals.Constant.MaxMaps)
        {
            return;
        }

        if (Data.Map[mapNum].NoRespawn)
        {
            return;
        }

        for (var x = 0; x < Data.Map[mapNum].MaxX; x++)
        {
            for (var y = 0; y < Data.Map[mapNum].MaxY; y++)
            {
                if (Data.Map[mapNum].Tile[x, y].Type == TileType.Item)
                {
                    if (Data.Item[Data.Map[mapNum].Tile[x, y].Data1].Type == (byte) ItemCategory.Currency ||
                        Data.Item[Data.Map[mapNum].Tile[x, y].Data1].Stackable == 1)
                    {
                        var value = Data.Map[mapNum].Tile[x, y].Data2 < 1 ? 1 : Data.Map[mapNum].Tile[x, y].Data2;

                        SpawnItem(Data.Map[mapNum].Tile[x, y].Data1, value, mapNum, x, y);
                    }
                    else
                    {
                        SpawnItem(Data.Map[mapNum].Tile[x, y].Data1, Data.Map[mapNum].Tile[x, y].Data2, mapNum, x, y);
                    }
                }

                if (Data.Map[mapNum].Tile[x, y].Type2 == TileType.Item)
                {
                    if (Data.Item[Data.Map[mapNum].Tile[x, y].Data1_2].Type == (byte) ItemCategory.Currency ||
                        Data.Item[Data.Map[mapNum].Tile[x, y].Data1_2].Stackable == 1)
                    {
                        var value = Data.Map[mapNum].Tile[x, y].Data2_2 < 1 ? 1 : Data.Map[mapNum].Tile[x, y].Data2_2;

                        SpawnItem(Data.Map[mapNum].Tile[x, y].Data1_2, value, mapNum, x, y);
                    }
                    else
                    {
                        SpawnItem(Data.Map[mapNum].Tile[x, y].Data1_2, Data.Map[mapNum].Tile[x, y].Data2_2, mapNum, x, y);
                    }
                }
            }
        }
    }

    public static void HandleRequestItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        var itemNum = packetReader.ReadInt32();
        if (itemNum is < 0 or > Core.Globals.Constant.MaxItems)
        {
            return;
        }

        SendUpdateItemTo(session.Id, itemNum);
    }

    public static void HandleRequestEditItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Mapper)
        {
            return;
        }

        var user = IsEditorLocked(session.Id, EditorType.Item);
        if (!string.IsNullOrEmpty(user))
        {
            NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) ColorName.BrightRed);
            return;
        }

        Data.TempPlayer[session.Id].Editor = EditorType.Item;

        Animation.SendAnimations(session.Id);
        Projectile.SendProjectiles(session.Id);
        NetworkSend.SendJobs(session);

        SendItems(session.Id);

        var packet = new PacketWriter(4);

        packet.WriteEnum(ServerPackets.SItemEditor);

        PlayerService.Instance.SendDataTo(session.Id, packet.GetBytes());
    }

    public static void HandleSaveItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
        {
            return;
        }

        var itemNum = packetReader.ReadInt32();
        if (itemNum is < 0 or > Core.Globals.Constant.MaxItems)
        {
            return;
        }

        Data.Item[itemNum].AccessReq = packetReader.ReadInt32();

        var statCount = Enum.GetNames<Stat>().Length;
        for (var i = 0; i < statCount; i++)
        {
            Data.Item[itemNum].AddStat[i] = (byte) packetReader.ReadInt32();
        }

        Data.Item[itemNum].Animation = packetReader.ReadInt32();
        Data.Item[itemNum].BindType = (byte) packetReader.ReadInt32();
        Data.Item[itemNum].JobReq = packetReader.ReadInt32();
        Data.Item[itemNum].Data1 = packetReader.ReadInt32();
        Data.Item[itemNum].Data2 = packetReader.ReadInt32();
        Data.Item[itemNum].Data3 = packetReader.ReadInt32();
        Data.Item[itemNum].LevelReq = packetReader.ReadInt32();
        Data.Item[itemNum].Mastery = (byte) packetReader.ReadInt32();
        Data.Item[itemNum].Name = packetReader.ReadString();
        Data.Item[itemNum].Paperdoll = packetReader.ReadInt32();
        Data.Item[itemNum].Icon = packetReader.ReadInt32();
        Data.Item[itemNum].Price = packetReader.ReadInt32();
        Data.Item[itemNum].Rarity = (byte) packetReader.ReadInt32();
        Data.Item[itemNum].Speed = packetReader.ReadInt32();
        Data.Item[itemNum].Stackable = (byte) packetReader.ReadInt32();
        Data.Item[itemNum].Description = packetReader.ReadString();

        for (var i = 0; i < statCount; i++)
        {
            Data.Item[itemNum].StatReq[i] = (byte) packetReader.ReadInt32();
        }

        Data.Item[itemNum].Type = (byte) packetReader.ReadInt32();
        Data.Item[itemNum].SubType = (byte) packetReader.ReadInt32();
        Data.Item[itemNum].ItemLevel = (byte) packetReader.ReadInt32();
        Data.Item[itemNum].KnockBack = (byte) packetReader.ReadInt32();
        Data.Item[itemNum].KnockBackTiles = (byte) packetReader.ReadInt32();
        Data.Item[itemNum].Projectile = packetReader.ReadInt32();
        Data.Item[itemNum].Ammo = packetReader.ReadInt32();

        SaveItem(itemNum);

        General.Logger.LogInformation("{AccountName} saved item #{ItemNum}",
            GetAccountLogin(session.Id), itemNum);

        SendUpdateItemToAll(itemNum);
    }

    public static void HandleGetItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        Player.MapGetItem(session.Id);
    }

    public static void HandleDropItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var invNum = buffer.ReadInt32();
        var amount = buffer.ReadInt32();

        if (Data.TempPlayer[session.Id].InBank || Data.TempPlayer[session.Id].InShop >= 0)
        {
            return;
        }

        if (invNum is < 0 or > Core.Globals.Constant.MaxInv)
        {
            return;
        }

        if (GetPlayerInv(session.Id, invNum) < 0 || GetPlayerInv(session.Id, invNum) > Core.Globals.Constant.MaxItems)
        {
            return;
        }

        if (Data.Item[GetPlayerInv(session.Id, invNum)].Type == (byte) ItemCategory.Currency ||
            Data.Item[GetPlayerInv(session.Id, invNum)].Stackable == 1)
        {
            if (amount < 0 | amount > GetPlayerInvValue(session.Id, invNum))
            {
                return;
            }
        }

        Player.MapDropItem(session.Id, invNum, amount);
    }

    public static void SendItems(int playerId)
    {
        for (var itemNum = 0; itemNum < Core.Globals.Constant.MaxItems; itemNum++)
        {
            if (Data.Item[itemNum].Name.Length > 0)
            {
                SendUpdateItemTo(playerId, itemNum);
            }
        }
    }

    public static void SendUpdateItemTo(int playerId, int itemNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SUpdateItem);

        WriteItemDataToPacket(itemNum, packet);

        PlayerService.Instance.SendDataTo(playerId, packet.GetBytes());
    }

    public static void SendUpdateItemToAll(int itemNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SUpdateItem);

        WriteItemDataToPacket(itemNum, packet);

        PlayerService.Instance.SendDataToAll(packet.GetBytes());
    }

    private static void WriteItemDataToPacket(int itemNum, PacketWriter packet)
    {
        var statCount = Enum.GetNames<Stat>().Length;

        packet.WriteInt32(itemNum);
        packet.WriteInt32(Data.Item[itemNum].AccessReq);

        for (var i = 0; i < statCount; i++)
        {
            packet.WriteInt32(Data.Item[itemNum].AddStat[i]);
        }

        packet.WriteInt32(Data.Item[itemNum].Animation);
        packet.WriteInt32(Data.Item[itemNum].BindType);
        packet.WriteInt32(Data.Item[itemNum].JobReq);
        packet.WriteInt32(Data.Item[itemNum].Data1);
        packet.WriteInt32(Data.Item[itemNum].Data2);
        packet.WriteInt32(Data.Item[itemNum].Data3);
        packet.WriteInt32(Data.Item[itemNum].LevelReq);
        packet.WriteInt32(Data.Item[itemNum].Mastery);
        packet.WriteString(Data.Item[itemNum].Name);
        packet.WriteInt32(Data.Item[itemNum].Paperdoll);
        packet.WriteInt32(Data.Item[itemNum].Icon);
        packet.WriteInt32(Data.Item[itemNum].Price);
        packet.WriteInt32(Data.Item[itemNum].Rarity);
        packet.WriteInt32(Data.Item[itemNum].Speed);
        packet.WriteInt32(Data.Item[itemNum].Stackable);
        packet.WriteString(Data.Item[itemNum].Description);

        for (var i = 0; i < statCount; i++)
        {
            packet.WriteInt32(Data.Item[itemNum].StatReq[i]);
        }

        packet.WriteInt32(Data.Item[itemNum].Type);
        packet.WriteInt32(Data.Item[itemNum].SubType);
        packet.WriteInt32(Data.Item[itemNum].ItemLevel);
        packet.WriteInt32(Data.Item[itemNum].KnockBack);
        packet.WriteInt32(Data.Item[itemNum].KnockBackTiles);
        packet.WriteInt32(Data.Item[itemNum].Projectile);
        packet.WriteInt32(Data.Item[itemNum].Ammo);
    }
}