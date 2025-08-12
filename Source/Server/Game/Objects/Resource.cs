using Core;
using Core.Globals;
using Core.Net;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Game;
using Server.Game.Net;
using Server.Net;
using static Core.Globals.Command;
using static Core.Net.Packets;
using Type = Core.Globals.Type;

namespace Server;

public static class Resource
{
    private static void SaveResource(int resourceNum)
    {
        var json = JsonConvert.SerializeObject(Data.Resource[resourceNum]);

        if (Database.RowExists(resourceNum, "resource"))
        {
            Database.UpdateRow(resourceNum, json, "resource", "data");
        }
        else
        {
            Database.InsertRow(resourceNum, json, "resource");
        }
    }

    public static async Task LoadResourcesAsync()
    {
        await Parallel.ForEachAsync(Enumerable.Range(0, Core.Globals.Constant.MaxResources), LoadResourceAsync);
    }

    public static async ValueTask LoadResourceAsync(int resourceNum, CancellationToken cancellationToken)
    {
        var data = await Database.SelectRowAsync(resourceNum, "resource", "data");
        if (data is null)
        {
            ClearResource(resourceNum);
            return;
        }

        var resourceData = JObject.FromObject(data).ToObject<Type.Resource>();

        Data.Resource[resourceNum] = resourceData;
    }

    public static void ClearResource(int resourceNum)
    {
        Data.Resource[resourceNum].Name = "";
        Data.Resource[resourceNum].EmptyMessage = "";
        Data.Resource[resourceNum].SuccessMessage = "";
    }

    public static void CacheResources(int mapNum)
    {
        var resourceCount = 0;

        for (var x = 0; x < Data.Map[mapNum].MaxX; x++)
        {
            for (var y = 0; y < Data.Map[mapNum].MaxY; y++)
            {
                if (Data.Map[mapNum].Tile[x, y].Type != TileType.Resource &&
                    Data.Map[mapNum].Tile[x, y].Type2 != TileType.Resource)
                {
                    continue;
                }

                resourceCount++;

                Array.Resize(ref Data.MapResource[mapNum].ResourceData, resourceCount);

                Data.MapResource[mapNum].ResourceData[resourceCount - 1].X = x;
                Data.MapResource[mapNum].ResourceData[resourceCount - 1].Y = y;
                Data.MapResource[mapNum].ResourceData[resourceCount - 1].Health = (byte) Data.Resource[Data.Map[mapNum].Tile[x, y].Data1].Health;
            }
        }

        Data.MapResource[mapNum].ResourceCount = resourceCount;
    }

    public static void CheckResourceLevelUp(int playerId, int skillSlot)
    {
        var levels = 0;

        if (GetPlayerGatherSkillLvl(playerId, skillSlot) == Core.Globals.Constant.MaxLevel)
        {
            return;
        }

        while (GetPlayerGatherSkillExp(playerId, skillSlot) >= GetPlayerGatherSkillMaxExp(playerId, skillSlot))
        {
            var expRollover = GetPlayerGatherSkillExp(playerId, skillSlot) - GetPlayerGatherSkillMaxExp(playerId, skillSlot);

            SetPlayerGatherSkillLvl(playerId, skillSlot, GetPlayerGatherSkillLvl(playerId, skillSlot) + 1);
            SetPlayerGatherSkillExp(playerId, skillSlot, expRollover);
            SetPlayerGatherSkillMaxExp(playerId, skillSlot, GetSkillNextLevel(playerId, skillSlot));

            levels++;
        }

        if (levels == 0)
        {
            return;
        }

        NetworkSend.PlayerMsg(playerId, levels == 1
            ? $"Your {GetResourceSkillName((ResourceSkill) skillSlot)} has gone up a level!"
            : $"Your {GetResourceSkillName((ResourceSkill) skillSlot)} has gone up by {levels} levels!", (int) ColorName.BrightGreen);

        NetworkSend.SendPlayerData(playerId);
    }

    public static void HandleRequestEditResource(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
        {
            return;
        }

        var user = IsEditorLocked(session.Id, EditorType.Resource);
        if (!string.IsNullOrEmpty(user))
        {
            NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) ColorName.BrightRed);
            return;
        }

        Data.TempPlayer[session.Id].Editor = EditorType.Resource;

        Item.SendItems(session.Id);
        Animation.SendAnimations(session.Id);

        SendResources(session.Id);

        var packet = new PacketWriter(4);

        packet.WriteEnum(ServerPackets.SResourceEditor);

        PlayerService.Instance.SendDataTo(session.Id, packet.GetBytes());
    }

    public static void HandleSaveResource(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
        {
            return;
        }

        var resourcenum = packetReader.ReadInt32();
        if (resourcenum is < 0 or > Core.Globals.Constant.MaxResources)
        {
            return;
        }

        Data.Resource[resourcenum].Animation = packetReader.ReadInt32();
        Data.Resource[resourcenum].EmptyMessage = packetReader.ReadString();
        Data.Resource[resourcenum].ExhaustedImage = packetReader.ReadInt32();
        Data.Resource[resourcenum].Health = packetReader.ReadInt32();
        Data.Resource[resourcenum].ExpReward = packetReader.ReadInt32();
        Data.Resource[resourcenum].ItemReward = packetReader.ReadInt32();
        Data.Resource[resourcenum].Name = packetReader.ReadString();
        Data.Resource[resourcenum].ResourceImage = packetReader.ReadInt32();
        Data.Resource[resourcenum].ResourceType = packetReader.ReadInt32();
        Data.Resource[resourcenum].RespawnTime = packetReader.ReadInt32();
        Data.Resource[resourcenum].SuccessMessage = packetReader.ReadString();
        Data.Resource[resourcenum].LvlRequired = packetReader.ReadInt32();
        Data.Resource[resourcenum].ToolRequired = packetReader.ReadInt32();
        Data.Resource[resourcenum].Walkthrough = packetReader.ReadBoolean();

        SaveResource(resourcenum);

        General.Logger.LogInformation("{AccountName} saved Resource #{Resourcenum}",
            GetAccountLogin(session.Id), resourcenum);

        SendUpdateResourceToAll(resourcenum);
    }

    public static void HandleRequestResource(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        var resourceNum = packetReader.ReadInt32();
        if (resourceNum < 0 | resourceNum > Core.Globals.Constant.MaxResources)
        {
            return;
        }

        SendUpdateResourceTo(session.Id, resourceNum);
    }

    public static void SendMapResourceToMap(int mapNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SMapResource);
        packet.WriteInt32(Data.MapResource[mapNum].ResourceCount);

        if (Data.MapResource[mapNum].ResourceCount > 0)
        {
            for (var i = 0; i < Data.MapResource[mapNum].ResourceCount; i++)
            {
                packet.WriteByte(Data.MapResource[mapNum].ResourceData[i].State);
                packet.WriteInt32(Data.MapResource[mapNum].ResourceData[i].X);
                packet.WriteInt32(Data.MapResource[mapNum].ResourceData[i].Y);
            }
        }

        NetworkConfig.SendDataToMap(mapNum, packet.GetBytes());
    }

    public static void SendResources(int playerId)
    {
        for (var resourceNum = 0; resourceNum < Core.Globals.Constant.MaxResources; resourceNum++)
        {
            if (Data.Resource[resourceNum].Name.Length > 0)
            {
                SendUpdateResourceTo(playerId, resourceNum);
            }
        }
    }

    public static void SendUpdateResourceTo(int playerId, int resourceNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SUpdateResource);

        WriteResourceDataToPacket(resourceNum, packet);

        PlayerService.Instance.SendDataTo(playerId, packet.GetBytes());
    }

    public static void SendUpdateResourceToAll(int resourceNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SUpdateResource);

        WriteResourceDataToPacket(resourceNum, packet);

        PlayerService.Instance.SendDataToAll(packet.GetBytes());
    }

    private static void WriteResourceDataToPacket(int resourceNum, PacketWriter packet)
    {
        packet.WriteInt32(resourceNum);
        packet.WriteInt32(Data.Resource[resourceNum].Animation);
        packet.WriteString(Data.Resource[resourceNum].EmptyMessage);
        packet.WriteInt32(Data.Resource[resourceNum].ExhaustedImage);
        packet.WriteInt32(Data.Resource[resourceNum].Health);
        packet.WriteInt32(Data.Resource[resourceNum].ExpReward);
        packet.WriteInt32(Data.Resource[resourceNum].ItemReward);
        packet.WriteString(Data.Resource[resourceNum].Name);
        packet.WriteInt32(Data.Resource[resourceNum].ResourceImage);
        packet.WriteInt32(Data.Resource[resourceNum].ResourceType);
        packet.WriteInt32(Data.Resource[resourceNum].RespawnTime);
        packet.WriteString(Data.Resource[resourceNum].SuccessMessage);
        packet.WriteInt32(Data.Resource[resourceNum].LvlRequired);
        packet.WriteInt32(Data.Resource[resourceNum].ToolRequired);
        packet.WriteBoolean(Data.Resource[resourceNum].Walkthrough);
    }

    public static void CheckResource(int playerId, int x, int y)
    {
        var mapNum = GetPlayerMap(playerId);

        if (x < 0 || y < 0 || x >= Data.MyMap.MaxX || y >= Data.MyMap.MaxY)
        {
            return;
        }

        if (Data.Map[mapNum].Tile[x, y].Type != TileType.Resource &&
            Data.Map[mapNum].Tile[x, y].Type2 != TileType.Resource)
        {
            return;
        }

        var resourceNum = 0;
        var resourceIndex = Data.Map[mapNum].Tile[x, y].Data1;
        var resourceType = (byte) Data.Resource[resourceIndex].ResourceType;

        for (var i = 0; i < Data.MapResource[mapNum].ResourceCount; i++)
        {
            if (Data.MapResource[mapNum].ResourceData[i].X == x &&
                Data.MapResource[mapNum].ResourceData[i].Y == y)
            {
                resourceNum = i;
            }
        }

        if (resourceNum < 0)
        {
            return;
        }

        if (GetPlayerEquipment(playerId, Equipment.Weapon) < 0 && Data.Resource[resourceIndex].ToolRequired != 0)
        {
            NetworkSend.PlayerMsg(playerId, "You need a tool to gather this resource.", (int) ColorName.Yellow);
            return;
        }

        if (Data.Item[GetPlayerEquipment(playerId, Equipment.Weapon)].Data3 != Data.Resource[resourceIndex].ToolRequired)
        {
            NetworkSend.PlayerMsg(playerId, "You have the wrong type of tool equiped.", (int) ColorName.Yellow);
            return;
        }

        if (Data.Resource[resourceIndex].ItemReward > 0)
        {
            if (Player.FindOpenInvSlot(playerId, Data.Resource[resourceIndex].ItemReward) == 0)
            {
                NetworkSend.PlayerMsg(playerId, "You have no inventory space.", (int) ColorName.Yellow);
                return;
            }
        }

        if (Data.Resource[resourceIndex].LvlRequired > GetPlayerGatherSkillLvl(playerId, resourceType))
        {
            NetworkSend.PlayerMsg(playerId, "Your level is too low!", (int) ColorName.Yellow);
            return;
        }

        if (Data.MapResource[mapNum].ResourceData[resourceNum].State != 0)
        {
            NetworkSend.SendActionMsg(mapNum, Data.Resource[resourceIndex].EmptyMessage, (int) ColorName.BrightRed, 1, GetPlayerX(playerId) * 32, GetPlayerY(playerId) * 32);
            return;
        }

        var resourceX = Data.MapResource[mapNum].ResourceData[resourceNum].X;
        var resourceY = Data.MapResource[mapNum].ResourceData[resourceNum].Y;

        int damage;
        if (Data.Resource[resourceIndex].ToolRequired == 0)
        {
            damage = 1 * GetPlayerGatherSkillLvl(playerId, resourceType);
        }
        else
        {
            damage = Data.Item[GetPlayerEquipment(playerId, Equipment.Weapon)].Data2;
        }

        if (damage <= 0)
        {
            NetworkSend.SendActionMsg(mapNum, "Miss!", (int) ColorName.BrightRed, 1, resourceX * 32, resourceY * 32);
            return;
        }

        if (Data.MapResource[mapNum].ResourceData[resourceNum].Health - damage >= 0)
        {
            Data.MapResource[mapNum].ResourceData[resourceNum].Health = (byte) (Data.MapResource[mapNum].ResourceData[resourceNum].Health - damage);
            NetworkSend.SendActionMsg(mapNum, "-" + damage, (int) ColorName.BrightRed, 1, resourceX * 32, resourceY * 32);
            Animation.SendAnimation(mapNum, Data.Resource[resourceIndex].Animation, resourceX, resourceY);

            return;
        }

        Data.MapResource[mapNum].ResourceData[resourceNum].State = 0; // Cut
        Data.MapResource[mapNum].ResourceData[resourceNum].Timer = General.GetTimeMs();

        SendMapResourceToMap(mapNum);

        NetworkSend.SendActionMsg(mapNum, Data.Resource[resourceIndex].SuccessMessage, (int) ColorName.BrightGreen, 1, GetPlayerX(playerId) * 32, GetPlayerY(playerId) * 32);
        Player.GiveInv(playerId, Data.Resource[resourceIndex].ItemReward, 1);
        Animation.SendAnimation(mapNum, Data.Resource[resourceIndex].Animation, resourceX, resourceY);

        SetPlayerGatherSkillExp(playerId, resourceType, GetPlayerGatherSkillExp(playerId, resourceType) + Data.Resource[resourceIndex].ExpReward);

        NetworkSend.PlayerMsg(playerId, $"Your {GetResourceSkillName((ResourceSkill) resourceType)} has earned {Data.Resource[resourceIndex].ExpReward} experience. ({GetPlayerGatherSkillExp(playerId, resourceType)}/{GetPlayerGatherSkillMaxExp(playerId, resourceType)})", (int) ColorName.BrightGreen);
        NetworkSend.SendPlayerData(playerId);

        CheckResourceLevelUp(playerId, resourceType);
    }
}