using Core;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Mirage.Sharp.Asfw;
using Mirage.Sharp.Asfw.IO;
using static Core.Packets;
using static Core.Global.Command;
using Server.Game;
using Server.Game.Net;
using Server.Net;

namespace Server;

public class NetworkSend
{
    public static void AlertMsg(GameSession session, SystemMessage  menuNo, Menu menuReset = 0, bool kick = true)
    {
        var writer = new PacketWriter(16);

        writer.WriteInt32((int) ServerPackets.SAlertMsg);
        writer.WriteByte((byte) menuNo);
        writer.WriteInt32((byte) menuReset);
        writer.WriteInt32(kick ? 1 : 0);

        session.Channel.Send(writer.GetBytes());

        Player.LeftGame(session.Id);
    }

    public static void GlobalMsg(string msg)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SGlobalMsg);
        buffer.WriteString(msg);
        NetworkConfig.SendDataToAll(buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void PlayerMsg(int index, string msg, int color)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SPlayerMsg);
        buffer.WriteString(msg);
        buffer.WriteInt32(color);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendPlayerChars(GameSession session)
    {
        var writer = new PacketWriter(4);

        writer.WriteEnum(ServerPackets.SPlayerChars);

        for (var i = 0; i < Core.Constant.MaxChars; i++)
        {
            Database.LoadCharacter(session.Id, i + 1);

            writer.WriteString(Data.Player[session.Id].Name);
            writer.WriteInt32(Data.Player[session.Id].Sprite);
            writer.WriteInt32(Data.Player[session.Id].Access);
            writer.WriteInt32(Data.Player[session.Id].Job);

            Database.ClearCharacter(session.Id);
        }

        session.Channel.Send(writer.GetBytes());
    }

    public static void SendUpdateJob(int index, int jobNum)
    {
        int i;
        int n;
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SUpdateJob);
        buffer.WriteInt32(jobNum);

        buffer.WriteString(Data.Job[jobNum].Name);
        buffer.WriteString(Data.Job[jobNum].Desc);

        buffer.WriteInt32(Data.Job[jobNum].MaleSprite);
        buffer.WriteInt32(Data.Job[jobNum].FemaleSprite);

        int statCount = Enum.GetValues(typeof(Core.Stat)).Length;
        for (int q = 0, loopTo = statCount; q < loopTo; q++)
            buffer.WriteInt32(Data.Job[jobNum].Stat[Conversions.ToInteger(q)]);

        for (int q = 0; q < Core.Constant.MaxStartItems; q++)
        {
            buffer.WriteInt32(Data.Job[jobNum].StartItem[q]);
            buffer.WriteInt32(Data.Job[jobNum].StartValue[q]);
        }

        buffer.WriteInt32(Data.Job[jobNum].StartMap);
        buffer.WriteByte(Data.Job[jobNum].StartX);
        buffer.WriteByte(Data.Job[jobNum].StartY);
        buffer.WriteInt32(Data.Job[jobNum].BaseExp);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendCloseTrade(int index)
    {
        var buffer = new ByteStream(4);
        buffer.WriteInt32((int) ServerPackets.SCloseTrade);
        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendExp(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SPlayerExp);
        buffer.WriteInt32(index);
        buffer.WriteInt32(GetPlayerExp(index));
        buffer.WriteInt32(GetPlayerNextLevel(index));

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendLoginOk(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SLoginOk);
        buffer.WriteInt32(index);
        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendInGame(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SInGame);
        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendJobs(GameSession session)
    {
        var writer = new PacketWriter();

        writer.WriteEnum(ServerPackets.SJobData);

        for (var i = 0; i < Core.Constant.MaxJobs; i++)
        {
            writer.WriteRaw(Database.JobData(i));
        }

        session.Channel.Send(writer.GetBytes());
    }

    public static void SendJobToAll(int jobNum)
    {
        int i;
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SJobData);
        buffer.WriteBlock(Database.JobData(jobNum));
        NetworkConfig.SendDataToAll(buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendUpdateJobTo(int index, int jobNum)
    {
        ByteStream buffer;
        buffer = new ByteStream(4);
        buffer.WriteInt32((int) ServerPackets.SUpdateJob);

        buffer.WriteBlock(Database.JobData(jobNum));

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendUpdateJobToAll(int jobNum)
    {
        ByteStream buffer;
        buffer = new ByteStream(4);
        buffer.WriteInt32((int) ServerPackets.SUpdateJob);

        buffer.WriteBlock(Database.JobData(jobNum));

        NetworkConfig.SendDataToAll(buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendInventory(int index)
    {
        int i;
        int n;
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SPlayerInv);

        var loopTo = Core.Constant.MaxInv;
        for (i = 0; i < loopTo; i++)
        {
            buffer.WriteInt32(GetPlayerInv(index, i));
            buffer.WriteInt32(GetPlayerInvValue(index, i));
        }

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendLeftMap(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SLeftMap);
        buffer.WriteInt32(index);
        NetworkConfig.SendDataToAllBut(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendLeftGame(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SLeftGame);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendMapEquipment(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SMapWornEq);
        buffer.WriteInt32(index);
        int equipmentCount = Enum.GetValues(typeof(Equipment)).Length;
        for (int i = 0; i < equipmentCount; i++)
            buffer.WriteInt32(GetPlayerEquipment(index, (Equipment) i));

        NetworkConfig.SendDataToMap(GetPlayerMap(index), buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendMapEquipmentTo(int playerNum, int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SMapWornEq);
        buffer.WriteInt32(playerNum);
        int equipmentCount = Enum.GetValues(typeof(Equipment)).Length;
        for (int i = 0; i < equipmentCount; i++)
            buffer.WriteInt32(GetPlayerEquipment(playerNum, (Equipment) i));

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendShops(int index)
    {
        int i;

        var loopTo = Core.Constant.MaxShops;
        for (i = 0; i < loopTo; i++)
        {
            if (Data.Shop[i].Name.Length > 0)
            {
                SendUpdateShopTo(index, i);
            }
        }
    }

    public static void SendUpdateShopTo(int index, int shopNum)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SUpdateShop);

        buffer.WriteInt32(shopNum);

        buffer.WriteInt32(Data.Shop[shopNum].BuyRate);
        buffer.WriteString(Data.Shop[shopNum].Name);

        for (int i = 0, loopTo = Core.Constant.MaxTrades; i < loopTo; i++)
        {
            buffer.WriteInt32(Data.Shop[shopNum].TradeItem[i].CostItem);
            buffer.WriteInt32(Data.Shop[shopNum].TradeItem[i].CostValue);
            buffer.WriteInt32(Data.Shop[shopNum].TradeItem[i].Item);
            buffer.WriteInt32(Data.Shop[shopNum].TradeItem[i].ItemValue);
        }

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendUpdateShopToAll(int shopNum)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SUpdateShop);

        buffer.WriteInt32(shopNum);
        buffer.WriteInt32(Data.Shop[shopNum].BuyRate);
        buffer.WriteString(Data.Shop[shopNum].Name);

        for (int i = 0, loopTo = Core.Constant.MaxTrades; i < loopTo; i++)
        {
            buffer.WriteInt32(Data.Shop[shopNum].TradeItem[i].CostItem);
            buffer.WriteInt32(Data.Shop[shopNum].TradeItem[i].CostValue);
            buffer.WriteInt32(Data.Shop[shopNum].TradeItem[i].Item);
            buffer.WriteInt32(Data.Shop[shopNum].TradeItem[i].ItemValue);
        }

        NetworkConfig.SendDataToAll(buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendSkills(int index)
    {
        int i;

        var loopTo = Core.Constant.MaxSkills;
        for (i = 0; i < loopTo; i++)
        {
            if (Data.Skill[i].Name.Length > 0)
            {
                SendUpdateSkillTo(index, i);
            }
        }
    }

    public static void SendUpdateSkillTo(int index, int skillNum)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SUpdateSkill);
        buffer.WriteInt32(skillNum);
        buffer.WriteInt32(Data.Skill[skillNum].AccessReq);
        buffer.WriteInt32(Data.Skill[skillNum].AoE);
        buffer.WriteInt32(Data.Skill[skillNum].CastAnim);
        buffer.WriteInt32(Data.Skill[skillNum].CastTime);
        buffer.WriteInt32(Data.Skill[skillNum].CdTime);
        buffer.WriteInt32(Data.Skill[skillNum].JobReq);
        buffer.WriteInt32(Data.Skill[skillNum].Dir);
        buffer.WriteInt32(Data.Skill[skillNum].Duration);
        buffer.WriteInt32(Data.Skill[skillNum].Icon);
        buffer.WriteInt32(Data.Skill[skillNum].Interval);
        buffer.WriteInt32(Conversions.ToInteger(Data.Skill[skillNum].IsAoE));
        buffer.WriteInt32(Data.Skill[skillNum].LevelReq);
        buffer.WriteInt32(Data.Skill[skillNum].Map);
        buffer.WriteInt32(Data.Skill[skillNum].MpCost);
        buffer.WriteString(Data.Skill[skillNum].Name);
        buffer.WriteInt32(Data.Skill[skillNum].Range);
        buffer.WriteInt32(Data.Skill[skillNum].SkillAnim);
        buffer.WriteInt32(Data.Skill[skillNum].StunDuration);
        buffer.WriteInt32(Data.Skill[skillNum].Type);
        buffer.WriteInt32(Data.Skill[skillNum].Vital);
        buffer.WriteInt32(Data.Skill[skillNum].X);
        buffer.WriteInt32(Data.Skill[skillNum].Y);

        // projectiles
        buffer.WriteInt32(Data.Skill[skillNum].IsProjectile);
        buffer.WriteInt32(Data.Skill[skillNum].Projectile);

        buffer.WriteInt32(Data.Skill[skillNum].KnockBack);
        buffer.WriteInt32(Data.Skill[skillNum].KnockBackTiles);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendUpdateSkillToAll(int skillNum)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SUpdateSkill);
        buffer.WriteInt32(skillNum);
        buffer.WriteInt32(Data.Skill[skillNum].AccessReq);
        buffer.WriteInt32(Data.Skill[skillNum].AoE);
        buffer.WriteInt32(Data.Skill[skillNum].CastAnim);
        buffer.WriteInt32(Data.Skill[skillNum].CastTime);
        buffer.WriteInt32(Data.Skill[skillNum].CdTime);
        buffer.WriteInt32(Data.Skill[skillNum].JobReq);
        buffer.WriteInt32(Data.Skill[skillNum].Dir);
        buffer.WriteInt32(Data.Skill[skillNum].Duration);
        buffer.WriteInt32(Data.Skill[skillNum].Icon);
        buffer.WriteInt32(Data.Skill[skillNum].Interval);
        buffer.WriteInt32(Conversions.ToInteger(Data.Skill[skillNum].IsAoE));
        buffer.WriteInt32(Data.Skill[skillNum].LevelReq);
        buffer.WriteInt32(Data.Skill[skillNum].Map);
        buffer.WriteInt32(Data.Skill[skillNum].MpCost);
        buffer.WriteString(Data.Skill[skillNum].Name);
        buffer.WriteInt32(Data.Skill[skillNum].Range);
        buffer.WriteInt32(Data.Skill[skillNum].SkillAnim);
        buffer.WriteInt32(Data.Skill[skillNum].StunDuration);
        buffer.WriteInt32(Data.Skill[skillNum].Type);
        buffer.WriteInt32(Data.Skill[skillNum].Vital);
        buffer.WriteInt32(Data.Skill[skillNum].X);
        buffer.WriteInt32(Data.Skill[skillNum].Y);

        buffer.WriteInt32(Data.Skill[skillNum].IsProjectile);
        buffer.WriteInt32(Data.Skill[skillNum].Projectile);

        buffer.WriteInt32(Data.Skill[skillNum].KnockBack);
        buffer.WriteInt32(Data.Skill[skillNum].KnockBackTiles);

        NetworkConfig.SendDataToAll(buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendStats(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SPlayerStats);
        buffer.WriteInt32(index);

        int statCount = Enum.GetValues(typeof(Stat)).Length;
        for (int i = 0; i < statCount; i++)
            buffer.WriteInt32(GetPlayerStat(index, (Stat) i));

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendVitals(int index)
    {
        var vitalCount = Enum.GetValues(typeof(Vital)).Length;
        for (int i = 0; i < vitalCount; i++)
            SendVital(index, (Vital) i);
    }

    public static void SendVital(int index, Vital vital)
    {
        var buffer = new ByteStream(4);

        // Get our packet type.
        switch (vital)
        {
            case Vital.Health:
            {
                buffer.WriteInt32((int) ServerPackets.SPlayerHp);
                break;
            }
            case Vital.Mana:
            {
                buffer.WriteInt32((int) ServerPackets.SPlayerMp);
                break;
            }
            case Vital.Stamina:
            {
                buffer.WriteInt32((int) ServerPackets.SPlayerSp);
                break;
            }
        }

        // Set and send related data.
        buffer.WriteInt32(GetPlayerVital(index, vital));
        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();

        // send vitals to party if in one
        if (Core.Data.TempPlayer[index].InParty >= 0)
            Party.SendPartyVitals(Core.Data.TempPlayer[index].InParty, index);
    }

    public static void SendWelcome(int index)
    {
        // Send them welcome
        if (SettingsManager.Instance.Welcome.Length > 0)
        {
            PlayerMsg(index, SettingsManager.Instance.Welcome, (int) (int) Core.Color.BrightCyan);
        }

        // Send whos online
        SendWhosOnline(index);
    }

    public static void SendWhosOnline(int index)
    {
        string s = "";
        var n = default(int);

        if (GetPlayerAccess(index) < (int) AccessLevel.Moderator)
            return;

        foreach (var i in PlayerService.Instance.PlayerIds)
        {
            if (i != index & GetPlayerName(i) != "")
            {
                s = s + GetPlayerName(i) + ", ";
                n = n + 1;
            }
        }

        if (n == 0)
        {
            s = "There are no other players online.";
        }
        else
        {
            s = Strings.Mid(s, 1, Strings.Len(s) - 2);
            s = "There are " + n + " other players online: " + s + ".";
        }

        PlayerMsg(index, s, (int) (int) Core.Color.White);
    }

    public static void SendWornEquipment(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SPlayerWornEq);

        int equipmentCount = Enum.GetValues(typeof(Equipment)).Length;
        for (int i = 0; i < equipmentCount; i++)
            buffer.WriteInt32(GetPlayerEquipment(index, (Equipment) i));

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendMapData(int index, int mapNum, bool sendMap)
    {
        var buffer = new ByteStream(4);
        byte[] data;

        if (sendMap)
        {
            buffer.WriteInt32(1);
            buffer.WriteInt32(mapNum);
            buffer.WriteString(Data.Map[mapNum].Name);
            buffer.WriteString(Data.Map[mapNum].Music);
            buffer.WriteInt32(Data.Map[mapNum].Revision);
            buffer.WriteInt32(Data.Map[mapNum].Moral);
            buffer.WriteInt32(Data.Map[mapNum].Tileset);
            buffer.WriteInt32(Data.Map[mapNum].Up);
            buffer.WriteInt32(Data.Map[mapNum].Down);
            buffer.WriteInt32(Data.Map[mapNum].Left);
            buffer.WriteInt32(Data.Map[mapNum].Right);
            buffer.WriteInt32(Data.Map[mapNum].BootMap);
            buffer.WriteInt32(Data.Map[mapNum].BootX);
            buffer.WriteInt32(Data.Map[mapNum].BootY);
            buffer.WriteInt32(Data.Map[mapNum].MaxX);
            buffer.WriteInt32(Data.Map[mapNum].MaxY);
            buffer.WriteInt32(Data.Map[mapNum].Weather);
            buffer.WriteInt32(Data.Map[mapNum].Fog);
            buffer.WriteInt32(Data.Map[mapNum].WeatherIntensity);
            buffer.WriteInt32(Data.Map[mapNum].FogOpacity);
            buffer.WriteInt32(Data.Map[mapNum].FogSpeed);
            buffer.WriteBoolean(Data.Map[mapNum].MapTint);
            buffer.WriteInt32(Data.Map[mapNum].MapTintR);
            buffer.WriteInt32(Data.Map[mapNum].MapTintG);
            buffer.WriteInt32(Data.Map[mapNum].MapTintB);
            buffer.WriteInt32(Data.Map[mapNum].MapTintA);
            buffer.WriteByte(Data.Map[mapNum].Panorama);
            buffer.WriteByte(Data.Map[mapNum].Parallax);
            buffer.WriteByte(Data.Map[mapNum].Brightness);
            buffer.WriteBoolean(Data.Map[mapNum].NoRespawn);
            buffer.WriteBoolean(Data.Map[mapNum].Indoors);
            buffer.WriteInt32(Data.Map[mapNum].Shop);

            for (int i = 0, loopTo = Core.Constant.MaxMapNpcs; i < loopTo; i++)
                buffer.WriteInt32(Data.Map[mapNum].Npc[i]);

            for (int x = 0, loopTo1 = Data.Map[mapNum].MaxX; x < (int) loopTo1; x++)
            {
                for (int y = 0, loopTo2 = Data.Map[mapNum].MaxY; y < (int) loopTo2; y++)
                {
                    buffer.WriteInt32(Data.Map[mapNum].Tile[x, y].Data1);
                    buffer.WriteInt32(Data.Map[mapNum].Tile[x, y].Data2);
                    buffer.WriteInt32(Data.Map[mapNum].Tile[x, y].Data3);
                    buffer.WriteInt32(Data.Map[mapNum].Tile[x, y].Data1_2);
                    buffer.WriteInt32(Data.Map[mapNum].Tile[x, y].Data2_2);
                    buffer.WriteInt32(Data.Map[mapNum].Tile[x, y].Data3_2);
                    buffer.WriteInt32(Data.Map[mapNum].Tile[x, y].DirBlock);
                    for (int i = 0, loopTo3 = (int) Enum.GetValues(typeof(MapLayer)).Length; i < loopTo3; i++)
                    {
                        buffer.WriteInt32(Data.Map[mapNum].Tile[x, y].Layer[i].Tileset);
                        buffer.WriteInt32(Data.Map[mapNum].Tile[x, y].Layer[i].X);
                        buffer.WriteInt32(Data.Map[mapNum].Tile[x, y].Layer[i].Y);
                        buffer.WriteInt32(Data.Map[mapNum].Tile[x, y].Layer[i].AutoTile);
                    }

                    buffer.WriteInt32((int) Data.Map[mapNum].Tile[x, y].Type);
                    buffer.WriteInt32((int) Data.Map[mapNum].Tile[x, y].Type2);
                }
            }

            buffer.WriteInt32(Data.Map[mapNum].EventCount);

            if (Data.Map[mapNum].EventCount > 0)
            {
                for (int i = 0, loopTo4 = Data.Map[mapNum].EventCount; i < loopTo4; i++)
                {
                    {
                        ref var withBlock = ref Data.Map[mapNum].Event[i];
                        buffer.WriteString(withBlock.Name);
                        buffer.WriteByte(withBlock.Globals);
                        buffer.WriteInt32(withBlock.X);
                        buffer.WriteInt32(withBlock.Y);
                        buffer.WriteInt32(withBlock.PageCount);
                    }

                    if (Data.Map[mapNum].Event[i].PageCount > 0)
                    {
                        for (int x = 0, loopTo5 = Data.Map[mapNum].Event[i].PageCount; x < loopTo5; x++)
                        {
                            {
                                ref var withBlock1 = ref Data.Map[mapNum].Event[i].Pages[x];
                                buffer.WriteInt32(withBlock1.ChkVariable);
                                buffer.WriteInt32(withBlock1.VariableIndex);
                                buffer.WriteInt32(withBlock1.VariableCondition);
                                buffer.WriteInt32(withBlock1.VariableCompare);
                                buffer.WriteInt32(withBlock1.ChkSwitch);
                                buffer.WriteInt32(withBlock1.SwitchIndex);
                                buffer.WriteInt32(withBlock1.SwitchCompare);
                                buffer.WriteInt32(withBlock1.ChkHasItem);
                                buffer.WriteInt32(withBlock1.HasItemIndex);
                                buffer.WriteInt32(withBlock1.HasItemAmount);
                                buffer.WriteInt32(withBlock1.ChkSelfSwitch);
                                buffer.WriteInt32(withBlock1.SelfSwitchIndex);
                                buffer.WriteInt32(withBlock1.SelfSwitchCompare);
                                buffer.WriteByte(withBlock1.GraphicType);
                                buffer.WriteInt32(withBlock1.Graphic);
                                buffer.WriteInt32(withBlock1.GraphicX);
                                buffer.WriteInt32(withBlock1.GraphicY);
                                buffer.WriteInt32(withBlock1.GraphicX2);
                                buffer.WriteInt32(withBlock1.GraphicY2);
                                buffer.WriteByte(withBlock1.MoveType);
                                buffer.WriteByte(withBlock1.MoveSpeed);
                                buffer.WriteByte(withBlock1.MoveFreq);
                                buffer.WriteInt32(withBlock1.MoveRouteCount);
                                buffer.WriteInt32(withBlock1.IgnoreMoveRoute);
                                buffer.WriteInt32(withBlock1.RepeatMoveRoute);

                                if (withBlock1.MoveRouteCount > 0)
                                {
                                    for (int y = 0, loopTo6 = withBlock1.MoveRouteCount; y < (int) loopTo6; y++)
                                    {
                                        buffer.WriteInt32(withBlock1.MoveRoute[y].Index);
                                        buffer.WriteInt32(withBlock1.MoveRoute[y].Data1);
                                        buffer.WriteInt32(withBlock1.MoveRoute[y].Data2);
                                        buffer.WriteInt32(withBlock1.MoveRoute[y].Data3);
                                        buffer.WriteInt32(withBlock1.MoveRoute[y].Data4);
                                        buffer.WriteInt32(withBlock1.MoveRoute[y].Data5);
                                        buffer.WriteInt32(withBlock1.MoveRoute[y].Data6);
                                    }
                                }

                                buffer.WriteInt32(withBlock1.WalkAnim);
                                buffer.WriteInt32(withBlock1.DirFix);
                                buffer.WriteInt32(withBlock1.WalkThrough);
                                buffer.WriteInt32(withBlock1.ShowName);
                                buffer.WriteByte(withBlock1.Trigger);
                                buffer.WriteInt32(withBlock1.CommandListCount);
                                buffer.WriteByte(withBlock1.Position);
                            }

                            if (Data.Map[mapNum].Event[i].Pages[x].CommandListCount > 0)
                            {
                                for (int y = 0, loopTo7 = Data.Map[mapNum].Event[i].Pages[x].CommandListCount; y < loopTo7; y++)
                                {
                                    buffer.WriteInt32(Data.Map[mapNum].Event[i].Pages[x].CommandList[y].CommandCount);
                                    buffer.WriteInt32(Data.Map[mapNum].Event[i].Pages[x].CommandList[y].ParentList);
                                    if (Data.Map[mapNum].Event[i].Pages[x].CommandList[y].CommandCount > 0)
                                    {
                                        for (int z = 0, loopTo8 = Data.Map[mapNum].Event[i].Pages[x].CommandList[y].CommandCount; z < loopTo8; z++)
                                        {
                                            {
                                                ref var withBlock2 = ref Data.Map[mapNum].Event[i].Pages[x].CommandList[y].Commands[z];
                                                buffer.WriteInt32(withBlock2.Index);
                                                buffer.WriteString(withBlock2.Text1);
                                                buffer.WriteString(withBlock2.Text2);
                                                buffer.WriteString(withBlock2.Text3);
                                                buffer.WriteString(withBlock2.Text4);
                                                buffer.WriteString(withBlock2.Text5);
                                                buffer.WriteInt32(withBlock2.Data1);
                                                buffer.WriteInt32(withBlock2.Data2);
                                                buffer.WriteInt32(withBlock2.Data3);
                                                buffer.WriteInt32(withBlock2.Data4);
                                                buffer.WriteInt32(withBlock2.Data5);
                                                buffer.WriteInt32(withBlock2.Data6);
                                                buffer.WriteInt32(withBlock2.ConditionalBranch.CommandList);
                                                buffer.WriteInt32(withBlock2.ConditionalBranch.Condition);
                                                buffer.WriteInt32(withBlock2.ConditionalBranch.Data1);
                                                buffer.WriteInt32(withBlock2.ConditionalBranch.Data2);
                                                buffer.WriteInt32(withBlock2.ConditionalBranch.Data3);
                                                buffer.WriteInt32(withBlock2.ConditionalBranch.ElseCommandList);
                                                buffer.WriteInt32(withBlock2.MoveRouteCount);
                                                if (withBlock2.MoveRouteCount > 0)
                                                {
                                                    for (int w = 0, loopTo9 = withBlock2.MoveRouteCount; w < loopTo9; w++)
                                                    {
                                                        buffer.WriteInt32(withBlock2.MoveRoute[w].Index);
                                                        buffer.WriteInt32(withBlock2.MoveRoute[w].Data1);
                                                        buffer.WriteInt32(withBlock2.MoveRoute[w].Data2);
                                                        buffer.WriteInt32(withBlock2.MoveRoute[w].Data3);
                                                        buffer.WriteInt32(withBlock2.MoveRoute[w].Data4);
                                                        buffer.WriteInt32(withBlock2.MoveRoute[w].Data5);
                                                        buffer.WriteInt32(withBlock2.MoveRoute[w].Data6);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            buffer.WriteInt32(0);
        }

        for (int i = 0, loopTo10 = Core.Constant.MaxMapItems; i < loopTo10; i++)
        {
            buffer.WriteInt32((int) Data.MapItem[mapNum, i].Num);
            buffer.WriteInt32(Data.MapItem[mapNum, i].Value);
            buffer.WriteInt32(Data.MapItem[mapNum, i].X);
            buffer.WriteInt32(Data.MapItem[mapNum, i].Y);
        }

        var vitalCount = System.Enum.GetNames(typeof(Vital)).Length;

        for (int i = 0, loopTo11 = Core.Constant.MaxMapNpcs; i < loopTo11; i++)
        {
            buffer.WriteInt32((int) Data.MapNpc[mapNum].Npc[i].Num);
            buffer.WriteInt32(Data.MapNpc[mapNum].Npc[i].X);
            buffer.WriteInt32(Data.MapNpc[mapNum].Npc[i].Y);
            buffer.WriteByte(Data.MapNpc[mapNum].Npc[i].Dir);
            for (int x = 0; x < vitalCount; x++)
            {
                buffer.WriteInt32(Data.MapNpc[mapNum].Npc[i].Vital[x]);
            }
        }

        if (Data.MapResource[GetPlayerMap(index)].ResourceCount > 0)
        {
            buffer.WriteInt32(1);
            buffer.WriteInt32(Data.MapResource[GetPlayerMap(index)].ResourceCount);

            for (int i = 0, loopTo12 = Data.MapResource[GetPlayerMap(index)].ResourceCount; i < loopTo12; i++)
            {
                buffer.WriteByte(Data.MapResource[GetPlayerMap(index)].ResourceData[i].State);
                buffer.WriteInt32(Data.MapResource[GetPlayerMap(index)].ResourceData[i].X);
                buffer.WriteInt32(Data.MapResource[GetPlayerMap(index)].ResourceData[i].Y);
            }
        }
        else
        {
            buffer.WriteInt32(0);
        }

        data = Compression.CompressBytes(buffer.ToArray());
        buffer = new ByteStream(4);
        buffer.WriteInt32((int) ServerPackets.SMapData);
        buffer.WriteBlock(data);
        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendJoinMap(int index)
    {
        try
        {
            Script.Instance?.JoinMap(index);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static byte[] PlayerData(int index)
    {
        byte[] playerData = default;
        var buffer = new ByteStream(4);
        int i;

        buffer.WriteInt32((int) ServerPackets.SPlayerData);
        buffer.WriteInt32(index);
        buffer.WriteString(GetPlayerName(index));
        buffer.WriteInt32(GetPlayerJob(index));
        buffer.WriteInt32(GetPlayerLevel(index));
        buffer.WriteInt32(GetPlayerPoints(index));
        buffer.WriteInt32(GetPlayerSprite(index));
        buffer.WriteInt32(GetPlayerMap(index));
        buffer.WriteByte(GetPlayerAccess(index));
        buffer.WriteBoolean(GetPlayerPk(index));

        var statCount = Enum.GetValues(typeof(Stat)).Length;
        for (i = 0; i < statCount; i++)
            buffer.WriteInt32(GetPlayerStat(index, (Stat) i));

        var resourceCount = Enum.GetValues(typeof(ResourceSkill)).Length;
        for (i = 0; i < resourceCount; i++)
        {
            buffer.WriteInt32(GetPlayerGatherSkillLvl(index, i));
            buffer.WriteInt32(GetPlayerGatherSkillExp(index, i));
            buffer.WriteInt32(GetPlayerGatherSkillMaxExp(index, i));
        }

        playerData = buffer.ToArray();

        buffer.Dispose();
        return playerData;
    }

    public static void SendPlayerXy(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SPlayerXy);
        buffer.WriteInt32(index);
        buffer.WriteInt32(GetPlayerRawX(index));
        buffer.WriteInt32(GetPlayerRawY(index));
        buffer.WriteByte(GetPlayerDir(index));
        buffer.WriteByte(Data.Player[index].Moving);
        buffer.WriteBoolean(Data.Player[index].IsMoving);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendPlayerXyTo(int index, int playerNum)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SPlayerXy);
        buffer.WriteInt32(playerNum);
        buffer.WriteInt32(GetPlayerRawX(playerNum));
        buffer.WriteInt32(GetPlayerRawY(playerNum));
        buffer.WriteByte(GetPlayerDir(playerNum));
        buffer.WriteByte(Data.Player[index].Moving);
        buffer.WriteBoolean(Data.Player[index].IsMoving);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendPlayerXyToMap(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SPlayerXy);
        buffer.WriteInt32(index);
        buffer.WriteInt32(GetPlayerRawX(index));
        buffer.WriteInt32(GetPlayerRawY(index));
        buffer.WriteByte(GetPlayerDir(index));
        buffer.WriteByte(Data.Player[index].Moving);
        buffer.WriteBoolean(Data.Player[index].IsMoving);

        NetworkConfig.SendDataToMap(GetPlayerMap(index), buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void MapMsg(int mapNum, string msg, byte color)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SMapMsg);
        buffer.WriteString(msg);

        NetworkConfig.SendDataToMap(mapNum, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void AdminMsg(int mapNum, string msg, byte color)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SAdminMsg);
        buffer.WriteString(msg);

        foreach (var i in PlayerService.Instance.PlayerIds)
        {
            if (GetPlayerAccess(i) >= (int) AccessLevel.Moderator)
            {
                NetworkConfig.SendDataTo(i, buffer.UnreadData, buffer.WritePosition);
            }
        }

        buffer.Dispose();
    }

    public static void SendActionMsg(int mapNum, string message, int color, int msgType, int x, int y, int playerOnlyNum = -1)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SActionMsg);
        buffer.WriteString(message);
        buffer.WriteInt32(color);
        buffer.WriteInt32(msgType);
        buffer.WriteInt32(x);
        buffer.WriteInt32(y);

        if (playerOnlyNum >= 0)
        {
            NetworkConfig.SendDataTo(playerOnlyNum, buffer.UnreadData, buffer.WritePosition);
        }
        else
        {
            NetworkConfig.SendDataToMap(mapNum, buffer.UnreadData, buffer.WritePosition);
        }

        buffer.Dispose();
    }

    public static void SayMsg_Map(int mapNum, int index, string message, int sayColor)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SSayMsg);
        buffer.WriteString(GetPlayerName(index));
        buffer.WriteInt32(GetPlayerAccess(index));
        buffer.WriteBoolean(GetPlayerPk(index));
        buffer.WriteString(message);
        buffer.WriteString("[Map]:");
        buffer.WriteInt32(sayColor);

        NetworkConfig.SendDataToMap(mapNum, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SayMsg_Global(int index, string message, int sayColor)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SSayMsg);
        buffer.WriteString(GetPlayerName(index));
        buffer.WriteInt32(GetPlayerAccess(index));
        buffer.WriteBoolean(GetPlayerPk(index));
        buffer.WriteString(message);
        buffer.WriteString("[Global]:");
        buffer.WriteInt32(sayColor);

        NetworkConfig.SendDataToAll(buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendPlayerData(int index)
    {
        byte[] data = PlayerData(index);
        NetworkConfig.SendDataToMap(GetPlayerMap(index), data, data.Length);
    }

    public static void SendInventoryUpdate(int index, int invSlot)
    {
        var buffer = new ByteStream(4);
        int n;

        buffer.WriteInt32((int) ServerPackets.SPlayerInvUpdate);

        buffer.WriteInt32(invSlot);

        buffer.WriteInt32(GetPlayerInv(index, invSlot));
        buffer.WriteInt32(GetPlayerInvValue(index, invSlot));

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendOpenShop(int index, int shopNum)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SOpenShop);
        buffer.WriteInt32(shopNum);
        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void ResetShopAction(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SResetShopAction);

        NetworkConfig.SendDataToAll(buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendBank(int index)
    {
        var buffer = new ByteStream(4);
        int i;

        buffer.WriteInt32((int) ServerPackets.SBank);

        var loopTo = Core.Constant.MaxBank;
        for (i = 0; i < loopTo; i++)
        {
            buffer.WriteInt32((int) Data.Bank[index].Item[i].Num);
            buffer.WriteInt32(Data.Bank[index].Item[i].Value);
        }

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendClearSkillBuffer(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SClearSkillBuffer);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendTradeInvite(int index, int tradeindex)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.STradeInvite);
        buffer.WriteInt32(tradeindex);
        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendTrade(int index, int tradeTarget)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.STrade);
        buffer.WriteInt32(tradeTarget);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendTradeUpdate(int index, byte dataType)
    {
        var buffer = new ByteStream(4);
        int i;
        double tradeTarget;
        var totalWorth = default(int);

        tradeTarget = Core.Data.TempPlayer[index].InTrade;

        if (tradeTarget == -1)
            return;

        buffer.WriteInt32((int) ServerPackets.STradeUpdate);
        buffer.WriteInt32(dataType);

        if (dataType == 0) // own inventory
        {
            var loopTo = Core.Constant.MaxInv;
            for (i = 0; i < loopTo; i++)
            {
                if (Core.Data.TempPlayer[index].TradeOffer[i].Num >= 0)
                {
                    buffer.WriteInt32((int) Core.Data.TempPlayer[index].TradeOffer[i].Num);
                    buffer.WriteInt32(Core.Data.TempPlayer[index].TradeOffer[i].Value);

                    // add total worth
                    // currency?
                    if (Core.Data.Item[(int) Core.Data.TempPlayer[index].TradeOffer[i].Num].Type == (int) ItemCategory.Currency || Core.Data.Item[(int) Core.Data.TempPlayer[index].TradeOffer[i].Num].Stackable == 1)
                    {
                        if (Core.Data.TempPlayer[index].TradeOffer[i].Value == 0)
                            Core.Data.TempPlayer[index].TradeOffer[i].Value = 0;
                        totalWorth = totalWorth + Core.Data.Item[GetPlayerInv(index, (int) Core.Data.TempPlayer[index].TradeOffer[i].Num)].Price * Core.Data.TempPlayer[index].TradeOffer[i].Value;
                    }
                    else
                    {
                        totalWorth = totalWorth + Core.Data.Item[GetPlayerInv(index, (int) Core.Data.TempPlayer[index].TradeOffer[i].Num)].Price;
                    }
                }
                else
                {
                    buffer.WriteInt32(-1);
                    buffer.WriteInt32(0);
                }
            }
        }
        else if (dataType == 1) // other inventory
        {
            var loopTo1 = Core.Constant.MaxInv;
            for (i = 0; i < loopTo1; i++)
            {
                if (Core.Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num >= 0)
                {
                    buffer.WriteInt32(GetPlayerInv((int) tradeTarget, (int) Core.Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num));
                    buffer.WriteInt32(Core.Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Value);

                    // add total worth
                    if (GetPlayerInv((int) tradeTarget, (int) Core.Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num) >= 0)
                    {
                        // currency?
                        if (Core.Data.Item[GetPlayerInv((int) tradeTarget, (int) Core.Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num)].Type == (int) ItemCategory.Currency || Core.Data.Item[GetPlayerInv((int) tradeTarget, (int) Core.Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num)].Stackable == 1)
                        {
                            if (Core.Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Value == 0)
                                Core.Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Value = 0;
                            totalWorth = totalWorth + Core.Data.Item[GetPlayerInv((int) tradeTarget, (int) Core.Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num)].Price * Core.Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Value;
                        }
                        else
                        {
                            totalWorth = totalWorth + Core.Data.Item[GetPlayerInv((int) tradeTarget, (int) Core.Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num)].Price;
                        }
                    }
                }
                else
                {
                    buffer.WriteInt32(-1);
                    buffer.WriteInt32(0);
                }
            }
        }

        // send total worth of trade
        buffer.WriteInt32(totalWorth);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendTradeStatus(int index, byte status)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.STradeStatus);
        buffer.WriteInt32(status);
        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendStunned(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SStunned);
        buffer.WriteInt32(Core.Data.TempPlayer[index].StunDuration);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendBlood(int mapNum, int x, int y)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SBlood);
        buffer.WriteInt32(x);
        buffer.WriteInt32(y);

        NetworkConfig.SendDataToMap(mapNum, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendPlayerSkills(int index)
    {
        int i;
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SSkills);

        var loopTo = Core.Constant.MaxPlayerSkills;
        for (i = 0; i < loopTo; i++)
            buffer.WriteInt32(GetPlayerSkill(index, i));

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendCooldown(int index, int slot)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SCooldown);
        buffer.WriteInt32(slot);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendTarget(int index, int target, int targetType)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.STarget);
        buffer.WriteInt32(target);
        buffer.WriteInt32(targetType);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendMapReport(int index)
    {
        var buffer = new ByteStream(4);
        int I;

        buffer.WriteInt32((int) ServerPackets.SMapReport);

        var loopTo = Core.Constant.MaxMaps;
        for (var i = 0; i < loopTo; i++)
            buffer.WriteString(Data.Map[i].Name);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendAdminPanel(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SAdmin);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendMapNames(int index)
    {
        var buffer = new ByteStream(4);
        int I;

        buffer.WriteInt32((int) ServerPackets.SMapNames);

        var loopTo = Core.Constant.MaxMaps;
        for (var i = 0; i < loopTo; i++)
            buffer.WriteString(Data.Map[i].Name);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendHotbar(int index)
    {
        var buffer = new ByteStream(4);
        int i;

        buffer.WriteInt32((int) ServerPackets.SHotbar);

        var loopTo = Core.Constant.MaxHotbar;
        for (i = 0; i < loopTo; i++)
        {
            buffer.WriteInt32(Core.Data.Player[index].Hotbar[i].Slot);
            buffer.WriteInt32(Core.Data.Player[index].Hotbar[i].SlotType);
        }

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendCritical(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SCritical);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendRightClick(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SrClick);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendJobEditor(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SJobEditor);

        NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendEmote(int index, int emote)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SEmote);

        buffer.WriteInt32(index);
        buffer.WriteInt32(emote);

        NetworkConfig.SendDataToMap(GetPlayerMap(index), buffer.UnreadData, buffer.WritePosition);

        buffer.Dispose();
    }

    public static void SendChatBubble(int mapNum, int target, int targetType, string message, int color)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SChatBubble);

        buffer.WriteInt32(target);
        buffer.WriteInt32(targetType);
        buffer.WriteString(message);
        buffer.WriteInt32(color);
        NetworkConfig.SendDataToMap(mapNum, buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendPlayerAttack(int index)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SAttack);
        buffer.WriteInt32(index);
        NetworkConfig.SendDataToMapBut(index, GetPlayerMap(index), buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }

    public static void SendNpcAttack(int mapNum, int npcIndex)
    {
        var buffer = new ByteStream(4);

        buffer.WriteInt32((int) ServerPackets.SNpcAttack);
        buffer.WriteInt32(npcIndex);
        NetworkConfig.SendDataToMap(mapNum, buffer.UnreadData, buffer.WritePosition);
        buffer.Dispose();
    }
}