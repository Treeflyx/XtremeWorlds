using Core;
using Core.Globals;
using Core.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Game;
using Server.Game.Net;
using Server.Net;
using static Core.Net.Packets;
using static Core.Globals.Command;
using Type = Core.Globals.Type;

namespace Server;

public static class Moral
{
    private static void ClearMoral(int moralNum)
    {
        Data.Moral[moralNum].Name = "";
        Data.Moral[moralNum].Color = 0;
        Data.Moral[moralNum].CanCast = false;
        Data.Moral[moralNum].CanDropItem = false;
        Data.Moral[moralNum].CanPk = false;
        Data.Moral[moralNum].CanPickupItem = false;
        Data.Moral[moralNum].CanUseItem = false;
        Data.Moral[moralNum].DropItems = false;
        Data.Moral[moralNum].LoseExp = false;
        Data.Moral[moralNum].NpcBlock = false;
        Data.Moral[moralNum].PlayerBlock = false;
    }

    private static async ValueTask LoadMoralAsync(int moralNum, CancellationToken cancellationToken)
    {
        var data = await Database.SelectRowAsync(moralNum, "moral", "data");
        if (data is null)
        {
            ClearMoral(moralNum);
            return;
        }

        var moralData = JObject.FromObject(data).ToObject<Type.Moral>();

        Data.Moral[moralNum] = moralData;
    }

    public static async Task LoadMoralsAsync()
    {
        await Parallel.ForEachAsync(Enumerable.Range(0, Core.Globals.Constant.MaxMorals), LoadMoralAsync);
    }

    private static void SaveMoral(int moralNum)
    {
        var json = JsonConvert.SerializeObject(Data.Moral[moralNum]);

        if (Database.RowExists(moralNum, "moral"))
        {
            Database.UpdateRow(moralNum, json, "moral", "data");
        }
        else
        {
            Database.InsertRow(moralNum, json, "moral");
        }
    }

    public static void SendMorals(int playerId)
    {
        for (var moralNum = 0; moralNum < Core.Globals.Constant.MaxMorals; moralNum++)
        {
            if (Data.Moral[moralNum].Name.Length > 0)
            {
                SendUpdateMoralTo(playerId, moralNum);
            }
        }
    }

    public static void SendUpdateMoralTo(int playerId, int moralNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SUpdateMoral);

        WriteMoralDataToPacket(moralNum, packet);

        PlayerService.Instance.SendDataTo(playerId, packet.GetBytes());
    }

    private static void SendUpdateMoralToAll(int moralNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SUpdateMoral);

        WriteMoralDataToPacket(moralNum, packet);

        PlayerService.Instance.SendDataToAll(packet.GetBytes());
    }

    private static void WriteMoralDataToPacket(int moralNum, PacketWriter packet)
    {
        packet.WriteInt32(moralNum);
        packet.WriteString(Data.Moral[moralNum].Name);
        packet.WriteByte(Data.Moral[moralNum].Color);
        packet.WriteBoolean(Data.Moral[moralNum].NpcBlock);
        packet.WriteBoolean(Data.Moral[moralNum].PlayerBlock);
        packet.WriteBoolean(Data.Moral[moralNum].DropItems);
        packet.WriteBoolean(Data.Moral[moralNum].CanCast);
        packet.WriteBoolean(Data.Moral[moralNum].CanDropItem);
        packet.WriteBoolean(Data.Moral[moralNum].CanPickupItem);
        packet.WriteBoolean(Data.Moral[moralNum].CanPk);
        packet.WriteBoolean(Data.Moral[moralNum].DropItems);
        packet.WriteBoolean(Data.Moral[moralNum].LoseExp);
    }

    public static void HandleRequestEditMoral(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
        {
            return;
        }

        var user = IsEditorLocked(session.Id, EditorType.Moral);
        if (!string.IsNullOrEmpty(user))
        {
            NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) ColorName.BrightRed);
            return;
        }

        SendMorals(session.Id);

        Data.TempPlayer[session.Id].Editor = EditorType.Moral;

        var packet = new PacketWriter(4);

        packet.WriteEnum(ServerPackets.SMoralEditor);

        PlayerService.Instance.SendDataTo(session.Id, packet.GetBytes());
    }

    public static void HandleSaveMoral(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
        {
            return;
        }

        var moralNum = packetReader.ReadInt32();
        if (moralNum is < 0 or > Core.Globals.Constant.MaxMorals)
        {
            return;
        }

        ref var moral = ref Data.Moral[moralNum];

        moral.Name = packetReader.ReadString();
        moral.Color = packetReader.ReadByte();
        moral.CanCast = packetReader.ReadBoolean();
        moral.CanPk = packetReader.ReadBoolean();
        moral.CanDropItem = packetReader.ReadBoolean();
        moral.CanPickupItem = packetReader.ReadBoolean();
        moral.CanUseItem = packetReader.ReadBoolean();
        moral.DropItems = packetReader.ReadBoolean();
        moral.LoseExp = packetReader.ReadBoolean();
        moral.PlayerBlock = packetReader.ReadBoolean();
        moral.NpcBlock = packetReader.ReadBoolean();

        SaveMoral(moralNum);

        General.Logger.LogInformation("{AccountName} saved moral #{MoralNum}",
            GetAccountLogin(session.Id), moralNum);

        SendUpdateMoralToAll(moralNum);
        SendMorals(session.Id);
    }

    public static void HandleRequestMoral(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        SendMorals(session.Id);
    }
}