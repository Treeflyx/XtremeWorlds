using Core;
using Core.Globals;
using Core.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Server.Game;
using Server.Game.Net;
using Server.Net;
using static Core.Globals.Command;
using static Core.Net.Packets;
using Type = Core.Globals.Type;

namespace Server;

public static class Projectile
{
    private static void SaveProjectile(int projectileNum)
    {
        var json = JsonConvert.SerializeObject(Data.Projectile[projectileNum]);

        if (Database.RowExists(projectileNum, "projectile"))
        {
            Database.UpdateRow(projectileNum, json, "projectile", "data");
        }
        else
        {
            Database.InsertRow(projectileNum, json, "projectile");
        }
    }

    public static async Task LoadProjectilesAsync()
    {
        await Parallel.ForEachAsync(Enumerable.Range(0, Core.Globals.Constant.MaxProjectiles), LoadProjectileAsync);
    }

    private static async ValueTask LoadProjectileAsync(int projectileNum, CancellationToken cancellationToken)
    {
        var data = await Database.SelectRowAsync(projectileNum, "projectile", "data");
        if (data is null)
        {
            ClearProjectile(projectileNum);
            return;
        }

        var projectileData = data.ToObject<Type.Projectile>();

        Data.Projectile[projectileNum] = projectileData;
    }

    private static void ClearMapProjectile(int mapNum, int mapProjectileNum)
    {
        Data.MapProjectile[mapNum, mapProjectileNum].ProjectileNum = 0;
        Data.MapProjectile[mapNum, mapProjectileNum].Owner = 0;
        Data.MapProjectile[mapNum, mapProjectileNum].OwnerType = 0;
        Data.MapProjectile[mapNum, mapProjectileNum].X = 0;
        Data.MapProjectile[mapNum, mapProjectileNum].Y = 0;
        Data.MapProjectile[mapNum, mapProjectileNum].Dir = 0;
        Data.MapProjectile[mapNum, mapProjectileNum].Timer = 0;
    }

    private static void ClearProjectile(int projectileNum)
    {
        Data.Projectile[projectileNum].Name = "";
        Data.Projectile[projectileNum].Sprite = 0;
        Data.Projectile[projectileNum].Range = 0;
        Data.Projectile[projectileNum].Speed = 0;
        Data.Projectile[projectileNum].Damage = 0;
    }

    public static void HandleRequestEditProjectile(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
        {
            return;
        }

        var user = IsEditorLocked(session.Id, EditorType.Projectile);
        if (!string.IsNullOrEmpty(user))
        {
            NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) ColorName.BrightRed);
            return;
        }

        SendProjectiles(session.Id);

        Data.TempPlayer[session.Id].Editor = EditorType.Projectile;

        var buffer = new PacketWriter(4);

        buffer.WriteEnum(ServerPackets.SProjectileEditor);

        PlayerService.Instance.SendDataTo(session.Id, buffer.GetBytes());
    }

    public static void HandleSaveProjectile(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
        {
            return;
        }

        var projectileNum = packetReader.ReadInt32();
        if (projectileNum is < 0 or > Core.Globals.Constant.MaxProjectiles)
        {
            return;
        }

        Data.Projectile[projectileNum].Name = packetReader.ReadString();
        Data.Projectile[projectileNum].Sprite = packetReader.ReadInt32();
        Data.Projectile[projectileNum].Range = (byte) packetReader.ReadInt32();
        Data.Projectile[projectileNum].Speed = packetReader.ReadInt32();
        Data.Projectile[projectileNum].Damage = packetReader.ReadInt32();

        SaveProjectile(projectileNum);

        General.Logger.LogInformation("{AccountName} saved projectile #{ProjectileNum}",
            GetAccountLogin(session.Id), projectileNum);

        SendUpdateProjectileToAll(projectileNum);
    }

    public static void HandleRequestProjectile(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        var projectileNum = packetReader.ReadInt32();

        SendUpdateProjectileTo(session.Id, projectileNum);
    }

    public static void HandleClearProjectile(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        var projectileNum = packetReader.ReadInt32();
        _ = packetReader.ReadInt32(); // Target Index
        _ = (TargetType) packetReader.ReadInt32(); // Target TYpe
        _ = packetReader.ReadInt32(); // Target Zone

        var mapNum = GetPlayerMap(session.Id);

        ClearMapProjectile(mapNum, projectileNum);
    }

    private static void SendUpdateProjectileToAll(int projectileNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SUpdateProjectile);
        packet.WriteInt32(projectileNum);
        packet.WriteString(Data.Projectile[projectileNum].Name);
        packet.WriteInt32(Data.Projectile[projectileNum].Sprite);
        packet.WriteInt32(Data.Projectile[projectileNum].Range);
        packet.WriteInt32(Data.Projectile[projectileNum].Speed);
        packet.WriteInt32(Data.Projectile[projectileNum].Damage);

        PlayerService.Instance.SendDataToAll(packet.GetBytes());
    }

    private static void SendUpdateProjectileTo(int playerId, int projectileNum)
    {
        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SUpdateProjectile);
        packet.WriteInt32(projectileNum);
        packet.WriteString(Data.Projectile[projectileNum].Name);
        packet.WriteInt32(Data.Projectile[projectileNum].Sprite);
        packet.WriteInt32(Data.Projectile[projectileNum].Range);
        packet.WriteInt32(Data.Projectile[projectileNum].Speed);
        packet.WriteInt32(Data.Projectile[projectileNum].Damage);

        PlayerService.Instance.SendDataTo(playerId, packet.GetBytes());
    }

    public static void SendProjectiles(int playerId)
    {
        for (var projectileNum = 0; projectileNum < Core.Globals.Constant.MaxProjectiles; projectileNum++)
        {
            if (Data.Projectile[projectileNum].Name.Length > 0)
            {
                SendUpdateProjectileTo(playerId, projectileNum);
            }
        }
    }

    private static void SendProjectileToMap(int mapNum, int projectileNum)
    {
        var mapProjectile = Data.MapProjectile[mapNum, projectileNum];

        var packet = new PacketWriter(4);

        packet.WriteEnum(ServerPackets.SMapProjectile);
        packet.WriteInt32(projectileNum);
        packet.WriteInt32(mapProjectile.ProjectileNum);
        packet.WriteInt32(mapProjectile.Owner);
        packet.WriteByte(mapProjectile.OwnerType);
        packet.WriteByte(mapProjectile.Dir);
        packet.WriteInt32(mapProjectile.X);
        packet.WriteInt32(mapProjectile.Y);

        NetworkConfig.SendDataToMap(mapNum, packet.GetBytes());
    }

    public static void PlayerFireProjectile(int playerId, int skillNum = 0)
    {
        var mapNum = GetPlayerMap(playerId);
        var mapProjectileNum = 0;

        for (var i = 0; i < Core.Globals.Constant.MaxProjectiles; i++)
        {
            if (Data.MapProjectile[mapNum, i].ProjectileNum != -1)
            {
                continue;
            }

            mapProjectileNum = i;
            break;
        }

        var projectileNum = skillNum > 0 ? Data.Skill[skillNum].Projectile : Data.Item[GetPlayerEquipment(playerId, Equipment.Weapon)].Projectile;
        if (projectileNum == -1)
        {
            return;
        }

        ref var mapProjectile = ref Data.MapProjectile[mapNum, mapProjectileNum];

        mapProjectile.ProjectileNum = projectileNum;
        mapProjectile.Owner = playerId;
        mapProjectile.OwnerType = (byte) TargetType.Player;
        mapProjectile.Dir = GetPlayerDir(playerId);
        mapProjectile.X = GetPlayerX(playerId);
        mapProjectile.Y = GetPlayerY(playerId);
        mapProjectile.Timer = General.GetTimeMs() + 60000;

        SendProjectileToMap(mapNum, mapProjectileNum);
    }
}