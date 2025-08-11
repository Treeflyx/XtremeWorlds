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

public static class Animation
{
    private static void SaveAnimation(int animationNum)
    {
        var json = JsonConvert.SerializeObject(Data.Animation[animationNum]);

        if (Database.RowExists(animationNum, "animation"))
        {
            Database.UpdateRow(animationNum, json, "animation", "data");
        }
        else
        {
            Database.InsertRow(animationNum, json, "animation");
        }
    }

    public static Task LoadAnimationsAsync()
    {
        return Parallel.ForEachAsync(Enumerable.Range(0, Core.Globals.Constant.MaxAnimations), LoadAnimationAsync);
    }

    private static async ValueTask LoadAnimationAsync(int animationNum, CancellationToken cancellationToken)
    {
        var data = await Database.SelectRowAsync(animationNum, "animation", "data");
        if (data is null)
        {
            ClearAnimation(animationNum);
            return;
        }

        var animationData = JObject.FromObject(data).ToObject<Type.Animation>();

        Data.Animation[animationNum] = animationData;
    }

    private static void ClearAnimation(int animationNum)
    {
        Data.Animation[animationNum].Name = "";
        Data.Animation[animationNum].Sound = "";
        Data.Animation[animationNum].Sprite = [0, 0];
        Data.Animation[animationNum].Frames = [0, 0];
        Data.Animation[animationNum].LoopCount = [0, 0];
        Data.Animation[animationNum].LoopTime = [0, 0];
    }

    public static void HandleRequestEditAnimation(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
        {
            return;
        }

        var user = IsEditorLocked(session.Id, EditorType.Animation);
        if (!string.IsNullOrEmpty(user))
        {
            NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) ColorName.BrightRed);
            return;
        }

        Data.TempPlayer[session.Id].Editor = EditorType.Animation;

        SendAnimations(session.Id);

        var packet = new PacketWriter(4);

        packet.WriteEnum(ServerPackets.SAnimationEditor);

        PlayerService.Instance.SendDataTo(session.Id, packet.GetBytes());
    }

    public static void HandleSaveAnimation(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        var animationNum = packetReader.ReadInt32();

        for (var i = 0; i < Data.Animation[animationNum].Frames.Length; i++)
        {
            Data.Animation[animationNum].Frames[i] = packetReader.ReadInt32();
        }

        for (var i = 0; i < Data.Animation[animationNum].LoopCount.Length; i++)
        {
            Data.Animation[animationNum].LoopCount[i] = packetReader.ReadInt32();
        }

        for (var i = 0; i < Data.Animation[animationNum].LoopTime.Length; i++)
        {
            Data.Animation[animationNum].LoopTime[i] = packetReader.ReadInt32();
        }

        Data.Animation[animationNum].Name = packetReader.ReadString();
        Data.Animation[animationNum].Sound = packetReader.ReadString();

        for (var i = 0; i < Data.Animation[animationNum].Sprite.Length; i++)
        {
            Data.Animation[animationNum].Sprite[i] = packetReader.ReadInt32();
        }

        SaveAnimation(animationNum);

        General.Logger.LogInformation("{AccountName} saved animation #{AnimationNum}",
            GetAccountLogin(session.Id), animationNum);

        SendUpdateAnimationToAll(animationNum);
    }

    public static void HandleRequestAnimation(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        var animationNum = packetReader.ReadInt32();
        if (animationNum is < 0 or > Core.Globals.Constant.MaxAnimations)
        {
            return;
        }

        SendUpdateAnimationTo(session.Id, animationNum);
    }

    public static void SendAnimation(int mapNum, int anim, int x, int y, byte lockType = 0, int lockindex = 0)
    {
        var packet = new PacketWriter(4);

        packet.WriteEnum(ServerPackets.SAnimation);
        packet.WriteInt32(anim);
        packet.WriteInt32(x);
        packet.WriteInt32(y);
        packet.WriteInt32(lockType);
        packet.WriteInt32(lockindex);

        NetworkConfig.SendDataToMap(mapNum, packet.GetBytes());
    }

    public static void SendAnimations(int playerId)
    {
        for (var animationNum = 0; animationNum < Core.Globals.Constant.MaxAnimations; animationNum++)
        {
            if (Data.Animation[animationNum].Name.Length > 0)
            {
                SendUpdateAnimationTo(playerId, animationNum);
            }
        }
    }

    private static void SendUpdateAnimationTo(int playerId, int animationNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SUpdateAnimation);

        WriteAnimationDataToPacket(animationNum, packet);

        PlayerService.Instance.SendDataTo(playerId, packet.GetBytes());
    }

    private static void SendUpdateAnimationToAll(int animationNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SUpdateAnimation);

        WriteAnimationDataToPacket(animationNum, packet);

        PlayerService.Instance.SendDataToAll(packet.GetBytes());
    }

    private static void WriteAnimationDataToPacket(int animationNum, PacketWriter packet)
    {
        packet.WriteInt32(animationNum);

        foreach (var frame in Data.Animation[animationNum].Frames)
        {
            packet.WriteInt32(frame);
        }

        foreach (var loopCount in Data.Animation[animationNum].LoopCount)
        {
            packet.WriteInt32(loopCount);
        }

        foreach (var loopTime in Data.Animation[animationNum].LoopTime)
        {
            packet.WriteInt32(loopTime);
        }

        packet.WriteString(Data.Animation[animationNum].Name);
        packet.WriteString(Data.Animation[animationNum].Sound);

        foreach (var sprite in Data.Animation[animationNum].Sprite)
        {
            packet.WriteInt32(sprite);
        }
    }
}