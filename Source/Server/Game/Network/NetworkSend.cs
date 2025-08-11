using Core;
using Core.Configurations;
using Core.Globals;
using Core.Net;
using Server.Game;
using Server.Game.Net;
using static Core.Net.Packets;
using static Core.Globals.Command;

namespace Server;

public static class NetworkSend
{
    private static readonly int EquipmentCount = Enum.GetValues<Equipment>().Length;
    private static readonly int StatCount = Enum.GetValues<Stat>().Length;
    private static readonly int VitalCount = Enum.GetValues<Vital>().Length;
    private static readonly int MapLayerCount = Enum.GetValues<MapLayer>().Length;
    private static readonly int ResourceSkillCount = Enum.GetValues<ResourceSkill>().Length;

    public static void AlertMsg(GameSession session, SystemMessage menuNo, Menu menuReset = 0, bool kick = true)
    {
        var packetWriter = new PacketWriter(16);

        packetWriter.WriteEnum(ServerPackets.SAlertMsg);
        packetWriter.WriteByte((byte) menuNo);
        packetWriter.WriteInt32((byte) menuReset);
        packetWriter.WriteInt32(kick ? 1 : 0);

        session.Channel.Send(packetWriter.GetBytes());

        _ = Player.LeftGame(session.Id);
    }

    public static void GlobalMsg(string message)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SGlobalMsg);
        packetWriter.WriteString(message);

        PlayerService.Instance.SendDataToAll(packetWriter.GetBytes());
    }

    public static void PlayerMsg(int playerId, string message, int color)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SPlayerMsg);
        packetWriter.WriteString(message);
        packetWriter.WriteInt32(color);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendPlayerChars(GameSession session)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SPlayerChars);

        for (var i = 0; i < Core.Globals.Constant.MaxChars; i++)
        {
            Database.LoadCharacter(session.Id, i + 1);

            packetWriter.WriteString(Data.Player[session.Id].Name);
            packetWriter.WriteInt32(Data.Player[session.Id].Sprite);
            packetWriter.WriteInt32(Data.Player[session.Id].Access);
            packetWriter.WriteInt32(Data.Player[session.Id].Job);

            Database.ClearCharacter(session.Id);
        }

        session.Channel.Send(packetWriter.GetBytes());
    }

    public static void SendCloseTrade(int playerId)
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(ServerPackets.SCloseTrade);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendExp(int playerId)
    {
        var packetWriter = new PacketWriter(16);

        packetWriter.WriteEnum(ServerPackets.SPlayerExp);
        packetWriter.WriteInt32(playerId);
        packetWriter.WriteInt32(GetPlayerExp(playerId));
        packetWriter.WriteInt32(GetPlayerNextLevel(playerId));

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendLoginOk(int playerId)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(ServerPackets.SLoginOk);
        packetWriter.WriteInt32(playerId);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendInGame(int playerId)
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(ServerPackets.SInGame);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendJobs(GameSession session)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SJobData);

        for (var i = 0; i < Core.Globals.Constant.MaxJobs; i++)
        {
            Database.WriteJobDataToPacket(i, packetWriter);
        }

        session.Channel.Send(packetWriter.GetBytes());
    }

    public static void SendJobToAll(int jobNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SJobData);

        Database.WriteJobDataToPacket(jobNum, packetWriter);

        PlayerService.Instance.SendDataToAll(packetWriter.GetBytes());
    }

    public static void SendInventory(int playerId)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SPlayerInv);

        for (var i = 0; i < Core.Globals.Constant.MaxInv; i++)
        {
            packetWriter.WriteInt32(GetPlayerInv(playerId, i));
            packetWriter.WriteInt32(GetPlayerInvValue(playerId, i));
        }

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendLeftGame(int playerId)
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(ServerPackets.SLeftGame);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendMapEquipment(int playerId)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SMapWornEq);
        packetWriter.WriteInt32(playerId);

        for (var i = 0; i < EquipmentCount; i++)
        {
            packetWriter.WriteInt32(GetPlayerEquipment(playerId, (Equipment) i));
        }

        NetworkConfig.SendDataToMap(GetPlayerMap(playerId), packetWriter.GetBytes());
    }

    public static void SendMapEquipmentTo(int equipmentPlayerId, int sendToPlayerId)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SMapWornEq);
        packetWriter.WriteInt32(equipmentPlayerId);

        for (var i = 0; i < EquipmentCount; i++)
        {
            packetWriter.WriteInt32(GetPlayerEquipment(equipmentPlayerId, (Equipment) i));
        }

        PlayerService.Instance.SendDataTo(sendToPlayerId, packetWriter.GetBytes());
    }

    public static void SendShops(int playerId)
    {
        for (var i = 0; i < Core.Globals.Constant.MaxShops; i++)
        {
            if (Data.Shop[i].Name.Length > 0)
            {
                SendUpdateShopTo(playerId, i);
            }
        }
    }

    public static void SendUpdateShopTo(int playerId, int shopNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SUpdateShop);
        packetWriter.WriteInt32(shopNum);
        packetWriter.WriteInt32(Data.Shop[shopNum].BuyRate);
        packetWriter.WriteString(Data.Shop[shopNum].Name);

        for (var i = 0; i < Core.Globals.Constant.MaxTrades; i++)
        {
            packetWriter.WriteInt32(Data.Shop[shopNum].TradeItem[i].CostItem);
            packetWriter.WriteInt32(Data.Shop[shopNum].TradeItem[i].CostValue);
            packetWriter.WriteInt32(Data.Shop[shopNum].TradeItem[i].Item);
            packetWriter.WriteInt32(Data.Shop[shopNum].TradeItem[i].ItemValue);
        }

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendUpdateShopToAll(int shopNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SUpdateShop);
        packetWriter.WriteInt32(shopNum);
        packetWriter.WriteInt32(Data.Shop[shopNum].BuyRate);
        packetWriter.WriteString(Data.Shop[shopNum].Name);

        for (var i = 0; i < Core.Globals.Constant.MaxTrades; i++)
        {
            packetWriter.WriteInt32(Data.Shop[shopNum].TradeItem[i].CostItem);
            packetWriter.WriteInt32(Data.Shop[shopNum].TradeItem[i].CostValue);
            packetWriter.WriteInt32(Data.Shop[shopNum].TradeItem[i].Item);
            packetWriter.WriteInt32(Data.Shop[shopNum].TradeItem[i].ItemValue);
        }

        PlayerService.Instance.SendDataToAll(packetWriter.GetBytes());
    }

    public static void SendSkills(int playerId)
    {
        for (var i = 0; i < Core.Globals.Constant.MaxSkills; i++)
        {
            if (Data.Skill[i].Name.Length > 0)
            {
                SendUpdateSkillTo(playerId, i);
            }
        }
    }

    public static void SendUpdateSkillTo(int playerId, int skillNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SUpdateSkill);
        packetWriter.WriteInt32(skillNum);
        packetWriter.WriteInt32(Data.Skill[skillNum].AccessReq);
        packetWriter.WriteInt32(Data.Skill[skillNum].AoE);
        packetWriter.WriteInt32(Data.Skill[skillNum].CastAnim);
        packetWriter.WriteInt32(Data.Skill[skillNum].CastTime);
        packetWriter.WriteInt32(Data.Skill[skillNum].CdTime);
        packetWriter.WriteInt32(Data.Skill[skillNum].JobReq);
        packetWriter.WriteInt32(Data.Skill[skillNum].Dir);
        packetWriter.WriteInt32(Data.Skill[skillNum].Duration);
        packetWriter.WriteInt32(Data.Skill[skillNum].Icon);
        packetWriter.WriteInt32(Data.Skill[skillNum].Interval);
        packetWriter.WriteInt32(Data.Skill[skillNum].IsAoE ? 1 : 0);
        packetWriter.WriteInt32(Data.Skill[skillNum].LevelReq);
        packetWriter.WriteInt32(Data.Skill[skillNum].Map);
        packetWriter.WriteInt32(Data.Skill[skillNum].MpCost);
        packetWriter.WriteString(Data.Skill[skillNum].Name);
        packetWriter.WriteInt32(Data.Skill[skillNum].Range);
        packetWriter.WriteInt32(Data.Skill[skillNum].SkillAnim);
        packetWriter.WriteInt32(Data.Skill[skillNum].StunDuration);
        packetWriter.WriteInt32(Data.Skill[skillNum].Type);
        packetWriter.WriteInt32(Data.Skill[skillNum].Vital);
        packetWriter.WriteInt32(Data.Skill[skillNum].X);
        packetWriter.WriteInt32(Data.Skill[skillNum].Y);
        packetWriter.WriteInt32(Data.Skill[skillNum].IsProjectile);
        packetWriter.WriteInt32(Data.Skill[skillNum].Projectile);
        packetWriter.WriteInt32(Data.Skill[skillNum].KnockBack);
        packetWriter.WriteInt32(Data.Skill[skillNum].KnockBackTiles);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendUpdateSkillToAll(int skillNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SUpdateSkill);
        packetWriter.WriteInt32(skillNum);
        packetWriter.WriteInt32(Data.Skill[skillNum].AccessReq);
        packetWriter.WriteInt32(Data.Skill[skillNum].AoE);
        packetWriter.WriteInt32(Data.Skill[skillNum].CastAnim);
        packetWriter.WriteInt32(Data.Skill[skillNum].CastTime);
        packetWriter.WriteInt32(Data.Skill[skillNum].CdTime);
        packetWriter.WriteInt32(Data.Skill[skillNum].JobReq);
        packetWriter.WriteInt32(Data.Skill[skillNum].Dir);
        packetWriter.WriteInt32(Data.Skill[skillNum].Duration);
        packetWriter.WriteInt32(Data.Skill[skillNum].Icon);
        packetWriter.WriteInt32(Data.Skill[skillNum].IsAoE ? 1 : 0);
        packetWriter.WriteInt32(Data.Skill[skillNum].LevelReq);
        packetWriter.WriteInt32(Data.Skill[skillNum].Map);
        packetWriter.WriteInt32(Data.Skill[skillNum].MpCost);
        packetWriter.WriteString(Data.Skill[skillNum].Name);
        packetWriter.WriteInt32(Data.Skill[skillNum].Range);
        packetWriter.WriteInt32(Data.Skill[skillNum].SkillAnim);
        packetWriter.WriteInt32(Data.Skill[skillNum].StunDuration);
        packetWriter.WriteInt32(Data.Skill[skillNum].Type);
        packetWriter.WriteInt32(Data.Skill[skillNum].Vital);
        packetWriter.WriteInt32(Data.Skill[skillNum].X);
        packetWriter.WriteInt32(Data.Skill[skillNum].Y);
        packetWriter.WriteInt32(Data.Skill[skillNum].IsProjectile);
        packetWriter.WriteInt32(Data.Skill[skillNum].Projectile);
        packetWriter.WriteInt32(Data.Skill[skillNum].KnockBack);
        packetWriter.WriteInt32(Data.Skill[skillNum].KnockBackTiles);

        PlayerService.Instance.SendDataToAll(packetWriter.GetBytes());
    }

    public static void SendStats(int playerId)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SPlayerStats);
        packetWriter.WriteInt32(playerId);

        for (var i = 0; i < StatCount; i++)
        {
            packetWriter.WriteInt32(GetPlayerStat(playerId, (Stat) i));
        }

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendVitals(int playerId)
    {
        for (var i = 0; i < VitalCount; i++)
        {
            SendVital(playerId, (Vital) i);
        }
    }

    public static void SendVital(int playerId, Vital vital)
    {
        var packetWriter = new PacketWriter(8);

        switch (vital)
        {
            case Vital.Health:
                packetWriter.WriteEnum(ServerPackets.SPlayerHp);
                break;

            case Vital.Mana:
                packetWriter.WriteEnum(ServerPackets.SPlayerMp);
                break;

            case Vital.Stamina:
                packetWriter.WriteEnum(ServerPackets.SPlayerSp);
                break;
        }

        packetWriter.WriteInt32(GetPlayerVital(playerId, vital));

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());

        if (Data.TempPlayer[playerId].InParty >= 0)
        {
            Party.SendPartyVitals(Data.TempPlayer[playerId].InParty, playerId);
        }
    }

    public static void SendWelcome(int playerId)
    {
        if (SettingsManager.Instance.Welcome.Length > 0)
        {
            PlayerMsg(playerId, SettingsManager.Instance.Welcome, (int) ColorName.BrightCyan);
        }

        SendWhosOnline(playerId);
    }

    public static void SendWhosOnline(int playerId)
    {
        if (GetPlayerAccess(playerId) < (int) AccessLevel.Moderator)
        {
            return;
        }

        var playerNames = PlayerService.Instance.PlayerIds
            .Where(otherPlayerId => otherPlayerId != playerId)
            .Select(GetPlayerName)
            .ToArray();
        
        string message;
        if (playerNames.Length == 0)
        {
            message = "There are no other players online.";
        }
        else
        {
            message = "There are " + playerNames.Length + " other players online: " + string.Join(", ", playerNames) + ".";
        }

        PlayerMsg(playerId, message, (int) ColorName.White);
    }

    public static void SendWornEquipment(int playerId)
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(ServerPackets.SPlayerWornEq);

        for (var i = 0; i < EquipmentCount; i++)
        {
            packetWriter.WriteInt32(GetPlayerEquipment(playerId, (Equipment) i));
        }

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendMapData(int playerId, int mapNum, bool sendMap)
    {
        var packetWriter = new PacketWriter();
        
        packetWriter.WriteEnum(ServerPackets.SMapData);

        if (sendMap)
        {
            packetWriter.WriteInt32(1);
            packetWriter.WriteInt32(mapNum);
            packetWriter.WriteString(Data.Map[mapNum].Name);
            packetWriter.WriteString(Data.Map[mapNum].Music);
            packetWriter.WriteInt32(Data.Map[mapNum].Revision);
            packetWriter.WriteInt32(Data.Map[mapNum].Moral);
            packetWriter.WriteInt32(Data.Map[mapNum].Tileset);
            packetWriter.WriteInt32(Data.Map[mapNum].Up);
            packetWriter.WriteInt32(Data.Map[mapNum].Down);
            packetWriter.WriteInt32(Data.Map[mapNum].Left);
            packetWriter.WriteInt32(Data.Map[mapNum].Right);
            packetWriter.WriteInt32(Data.Map[mapNum].BootMap);
            packetWriter.WriteInt32(Data.Map[mapNum].BootX);
            packetWriter.WriteInt32(Data.Map[mapNum].BootY);
            packetWriter.WriteInt32(Data.Map[mapNum].MaxX);
            packetWriter.WriteInt32(Data.Map[mapNum].MaxY);
            packetWriter.WriteInt32(Data.Map[mapNum].Weather);
            packetWriter.WriteInt32(Data.Map[mapNum].Fog);
            packetWriter.WriteInt32(Data.Map[mapNum].WeatherIntensity);
            packetWriter.WriteInt32(Data.Map[mapNum].FogOpacity);
            packetWriter.WriteInt32(Data.Map[mapNum].FogSpeed);
            packetWriter.WriteBoolean(Data.Map[mapNum].MapTint);
            packetWriter.WriteInt32(Data.Map[mapNum].MapTintR);
            packetWriter.WriteInt32(Data.Map[mapNum].MapTintG);
            packetWriter.WriteInt32(Data.Map[mapNum].MapTintB);
            packetWriter.WriteInt32(Data.Map[mapNum].MapTintA);
            packetWriter.WriteByte(Data.Map[mapNum].Panorama);
            packetWriter.WriteByte(Data.Map[mapNum].Parallax);
            packetWriter.WriteByte(Data.Map[mapNum].Brightness);
            packetWriter.WriteBoolean(Data.Map[mapNum].NoRespawn);
            packetWriter.WriteBoolean(Data.Map[mapNum].Indoors);
            packetWriter.WriteInt32(Data.Map[mapNum].Shop);

            for (var i = 0; i < Core.Globals.Constant.MaxMapNpcs; i++)
            {
                packetWriter.WriteInt32(Data.Map[mapNum].Npc[i]);
            }

            for (var x = 0; x < Data.Map[mapNum].MaxX; x++)
            {
                for (var y = 0; y < Data.Map[mapNum].MaxY; y++)
                {
                    packetWriter.WriteInt32(Data.Map[mapNum].Tile[x, y].Data1);
                    packetWriter.WriteInt32(Data.Map[mapNum].Tile[x, y].Data2);
                    packetWriter.WriteInt32(Data.Map[mapNum].Tile[x, y].Data3);
                    packetWriter.WriteInt32(Data.Map[mapNum].Tile[x, y].Data1_2);
                    packetWriter.WriteInt32(Data.Map[mapNum].Tile[x, y].Data2_2);
                    packetWriter.WriteInt32(Data.Map[mapNum].Tile[x, y].Data3_2);
                    packetWriter.WriteInt32(Data.Map[mapNum].Tile[x, y].DirBlock);

                    for (var i = 0; i < MapLayerCount; i++)
                    {
                        packetWriter.WriteInt32(Data.Map[mapNum].Tile[x, y].Layer[i].Tileset);
                        packetWriter.WriteInt32(Data.Map[mapNum].Tile[x, y].Layer[i].X);
                        packetWriter.WriteInt32(Data.Map[mapNum].Tile[x, y].Layer[i].Y);
                        packetWriter.WriteInt32(Data.Map[mapNum].Tile[x, y].Layer[i].AutoTile);
                    }

                    packetWriter.WriteInt32((int) Data.Map[mapNum].Tile[x, y].Type);
                    packetWriter.WriteInt32((int) Data.Map[mapNum].Tile[x, y].Type2);
                }
            }

            packetWriter.WriteInt32(Data.Map[mapNum].EventCount);

            if (Data.Map[mapNum].EventCount > 0)
            {
                for (var i = 0; i < Data.Map[mapNum].EventCount; i++)
                {
                    ref var @event = ref Data.Map[mapNum].Event[i];

                    packetWriter.WriteString(@event.Name);
                    packetWriter.WriteByte(@event.Globals);
                    packetWriter.WriteInt32(@event.X);
                    packetWriter.WriteInt32(@event.Y);
                    packetWriter.WriteInt32(@event.PageCount);

                    if (Data.Map[mapNum].Event[i].PageCount == 0)
                    {
                        continue;
                    }

                    for (var x = 0; x < Data.Map[mapNum].Event[i].PageCount; x++)
                    {
                        ref var eventPage = ref Data.Map[mapNum].Event[i].Pages[x];

                        packetWriter.WriteInt32(eventPage.ChkVariable);
                        packetWriter.WriteInt32(eventPage.VariableIndex);
                        packetWriter.WriteInt32(eventPage.VariableCondition);
                        packetWriter.WriteInt32(eventPage.VariableCompare);
                        packetWriter.WriteInt32(eventPage.ChkSwitch);
                        packetWriter.WriteInt32(eventPage.SwitchIndex);
                        packetWriter.WriteInt32(eventPage.SwitchCompare);
                        packetWriter.WriteInt32(eventPage.ChkHasItem);
                        packetWriter.WriteInt32(eventPage.HasItemIndex);
                        packetWriter.WriteInt32(eventPage.HasItemAmount);
                        packetWriter.WriteInt32(eventPage.ChkSelfSwitch);
                        packetWriter.WriteInt32(eventPage.SelfSwitchIndex);
                        packetWriter.WriteInt32(eventPage.SelfSwitchCompare);
                        packetWriter.WriteByte(eventPage.GraphicType);
                        packetWriter.WriteInt32(eventPage.Graphic);
                        packetWriter.WriteInt32(eventPage.GraphicX);
                        packetWriter.WriteInt32(eventPage.GraphicY);
                        packetWriter.WriteInt32(eventPage.GraphicX2);
                        packetWriter.WriteInt32(eventPage.GraphicY2);
                        packetWriter.WriteByte(eventPage.MoveType);
                        packetWriter.WriteByte(eventPage.MoveSpeed);
                        packetWriter.WriteByte(eventPage.MoveFreq);
                        packetWriter.WriteInt32(eventPage.MoveRouteCount);
                        packetWriter.WriteInt32(eventPage.IgnoreMoveRoute);
                        packetWriter.WriteInt32(eventPage.RepeatMoveRoute);

                        if (eventPage.MoveRouteCount > 0)
                        {
                            for (int y = 0, loopTo6 = eventPage.MoveRouteCount; y < loopTo6; y++)
                            {
                                packetWriter.WriteInt32(eventPage.MoveRoute[y].Index);
                                packetWriter.WriteInt32(eventPage.MoveRoute[y].Data1);
                                packetWriter.WriteInt32(eventPage.MoveRoute[y].Data2);
                                packetWriter.WriteInt32(eventPage.MoveRoute[y].Data3);
                                packetWriter.WriteInt32(eventPage.MoveRoute[y].Data4);
                                packetWriter.WriteInt32(eventPage.MoveRoute[y].Data5);
                                packetWriter.WriteInt32(eventPage.MoveRoute[y].Data6);
                            }
                        }

                        packetWriter.WriteInt32(eventPage.WalkAnim);
                        packetWriter.WriteInt32(eventPage.DirFix);
                        packetWriter.WriteInt32(eventPage.WalkThrough);
                        packetWriter.WriteInt32(eventPage.ShowName);
                        packetWriter.WriteByte(eventPage.Trigger);
                        packetWriter.WriteInt32(eventPage.CommandListCount);
                        packetWriter.WriteByte(eventPage.Position);

                        if (Data.Map[mapNum].Event[i].Pages[x].CommandListCount == 0)
                        {
                            continue;
                        }

                        for (var y = 0; y < Data.Map[mapNum].Event[i].Pages[x].CommandListCount; y++)
                        {
                            packetWriter.WriteInt32(Data.Map[mapNum].Event[i].Pages[x].CommandList[y].CommandCount);
                            packetWriter.WriteInt32(Data.Map[mapNum].Event[i].Pages[x].CommandList[y].ParentList);

                            if (Data.Map[mapNum].Event[i].Pages[x].CommandList[y].CommandCount == 0)
                            {
                                continue;
                            }

                            for (var z = 0; z < Data.Map[mapNum].Event[i].Pages[x].CommandList[y].CommandCount; z++)
                            {
                                ref var eventCommand = ref Data.Map[mapNum].Event[i].Pages[x].CommandList[y].Commands[z];

                                packetWriter.WriteInt32(eventCommand.Index);
                                packetWriter.WriteString(eventCommand.Text1);
                                packetWriter.WriteString(eventCommand.Text2);
                                packetWriter.WriteString(eventCommand.Text3);
                                packetWriter.WriteString(eventCommand.Text4);
                                packetWriter.WriteString(eventCommand.Text5);
                                packetWriter.WriteInt32(eventCommand.Data1);
                                packetWriter.WriteInt32(eventCommand.Data2);
                                packetWriter.WriteInt32(eventCommand.Data3);
                                packetWriter.WriteInt32(eventCommand.Data4);
                                packetWriter.WriteInt32(eventCommand.Data5);
                                packetWriter.WriteInt32(eventCommand.Data6);
                                packetWriter.WriteInt32(eventCommand.ConditionalBranch.CommandList);
                                packetWriter.WriteInt32(eventCommand.ConditionalBranch.Condition);
                                packetWriter.WriteInt32(eventCommand.ConditionalBranch.Data1);
                                packetWriter.WriteInt32(eventCommand.ConditionalBranch.Data2);
                                packetWriter.WriteInt32(eventCommand.ConditionalBranch.Data3);
                                packetWriter.WriteInt32(eventCommand.ConditionalBranch.ElseCommandList);
                                packetWriter.WriteInt32(eventCommand.MoveRouteCount);

                                if (eventCommand.MoveRouteCount == 0)
                                {
                                    continue;
                                }

                                for (var w = 0; w < eventCommand.MoveRouteCount; w++)
                                {
                                    packetWriter.WriteInt32(eventCommand.MoveRoute[w].Index);
                                    packetWriter.WriteInt32(eventCommand.MoveRoute[w].Data1);
                                    packetWriter.WriteInt32(eventCommand.MoveRoute[w].Data2);
                                    packetWriter.WriteInt32(eventCommand.MoveRoute[w].Data3);
                                    packetWriter.WriteInt32(eventCommand.MoveRoute[w].Data4);
                                    packetWriter.WriteInt32(eventCommand.MoveRoute[w].Data5);
                                    packetWriter.WriteInt32(eventCommand.MoveRoute[w].Data6);
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            packetWriter.WriteInt32(0);
        }

        for (var i = 0; i < Core.Globals.Constant.MaxMapItems; i++)
        {
            packetWriter.WriteInt32(Data.MapItem[mapNum, i].Num);
            packetWriter.WriteInt32(Data.MapItem[mapNum, i].Value);
            packetWriter.WriteInt32(Data.MapItem[mapNum, i].X);
            packetWriter.WriteInt32(Data.MapItem[mapNum, i].Y);
        }

        for (var i = 0; i < Core.Globals.Constant.MaxMapNpcs; i++)
        {
            packetWriter.WriteInt32(Data.MapNpc[mapNum].Npc[i].Num);
            packetWriter.WriteInt32(Data.MapNpc[mapNum].Npc[i].X);
            packetWriter.WriteInt32(Data.MapNpc[mapNum].Npc[i].Y);
            packetWriter.WriteByte(Data.MapNpc[mapNum].Npc[i].Dir);

            for (var x = 0; x < VitalCount; x++)
            {
                packetWriter.WriteInt32(Data.MapNpc[mapNum].Npc[i].Vital[x]);
            }
        }

        if (Data.MapResource[GetPlayerMap(playerId)].ResourceCount > 0)
        {
            packetWriter.WriteInt32(1);
            packetWriter.WriteInt32(Data.MapResource[GetPlayerMap(playerId)].ResourceCount);

            for (var i = 0; i < Data.MapResource[GetPlayerMap(playerId)].ResourceCount; i++)
            {
                packetWriter.WriteByte(Data.MapResource[GetPlayerMap(playerId)].ResourceData[i].State);
                packetWriter.WriteInt32(Data.MapResource[GetPlayerMap(playerId)].ResourceData[i].X);
                packetWriter.WriteInt32(Data.MapResource[GetPlayerMap(playerId)].ResourceData[i].Y);
            }
        }
        else
        {
            packetWriter.WriteInt32(0);
        }
        
        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendJoinMap(int playerId)
    {
        try
        {
            Script.Instance?.JoinMap(playerId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static byte[] GetPlayerDataPacket(int playerId)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SPlayerData);
        packetWriter.WriteInt32(playerId);
        packetWriter.WriteString(GetPlayerName(playerId));
        packetWriter.WriteInt32(GetPlayerJob(playerId));
        packetWriter.WriteInt32(GetPlayerLevel(playerId));
        packetWriter.WriteInt32(GetPlayerPoints(playerId));
        packetWriter.WriteInt32(GetPlayerSprite(playerId));
        packetWriter.WriteInt32(GetPlayerMap(playerId));
        packetWriter.WriteByte(GetPlayerAccess(playerId));
        packetWriter.WriteBoolean(GetPlayerPk(playerId));

        for (var i = 0; i < StatCount; i++)
        {
            packetWriter.WriteInt32(GetPlayerStat(playerId, (Stat) i));
        }

        for (var i = 0; i < ResourceSkillCount; i++)
        {
            packetWriter.WriteInt32(GetPlayerGatherSkillLvl(playerId, i));
            packetWriter.WriteInt32(GetPlayerGatherSkillExp(playerId, i));
            packetWriter.WriteInt32(GetPlayerGatherSkillMaxExp(playerId, i));
        }

        return packetWriter.GetBytes();
    }

    public static void SendPlayerXy(int playerId)
    {
        SendPlayerXyTo(playerId, playerId);
    }

    public static void SendPlayerXyTo(int sendToPlayerId, int positionPlayerId)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SPlayerXy);
        packetWriter.WriteInt32(positionPlayerId);
        packetWriter.WriteInt32(GetPlayerRawX(positionPlayerId));
        packetWriter.WriteInt32(GetPlayerRawY(positionPlayerId));
        packetWriter.WriteByte(GetPlayerDir(positionPlayerId));
        packetWriter.WriteByte(Data.Player[sendToPlayerId].Moving);
        packetWriter.WriteBoolean(Data.Player[sendToPlayerId].IsMoving);

        PlayerService.Instance.SendDataTo(sendToPlayerId, packetWriter.GetBytes());
    }

    public static void SendPlayerXyToMap(int playerId)
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(ServerPackets.SPlayerXy);
        packetWriter.WriteInt32(playerId);
        packetWriter.WriteInt32(GetPlayerRawX(playerId));
        packetWriter.WriteInt32(GetPlayerRawY(playerId));
        packetWriter.WriteByte(GetPlayerDir(playerId));
        packetWriter.WriteByte(Data.Player[playerId].Moving);
        packetWriter.WriteBoolean(Data.Player[playerId].IsMoving);

        NetworkConfig.SendDataToMap(GetPlayerMap(playerId), packetWriter.GetBytes());
    }

    public static void MapMsg(int mapNum, string message)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SMapMsg);
        packetWriter.WriteString(message);

        NetworkConfig.SendDataToMap(mapNum, packetWriter.GetBytes());
    }

    public static void AdminMsg(string message)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SAdminMsg);
        packetWriter.WriteString(message);

        foreach (var playerId in PlayerService.Instance.PlayerIds)
        {
            if (GetPlayerAccess(playerId) >= (int) AccessLevel.Moderator)
            {
                PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
            }
        }
    }

    public static void SendActionMsg(int mapNum, string message, int color, int msgType, int x, int y, int playerOnlyNum = -1)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SActionMsg);
        packetWriter.WriteString(message);
        packetWriter.WriteInt32(color);
        packetWriter.WriteInt32(msgType);
        packetWriter.WriteInt32(x);
        packetWriter.WriteInt32(y);

        if (playerOnlyNum >= 0)
        {
            PlayerService.Instance.SendDataTo(playerOnlyNum, packetWriter.GetBytes());
        }
        else
        {
            NetworkConfig.SendDataToMap(mapNum, packetWriter.GetBytes());
        }
    }

    public static void SayMsg_Map(int mapNum, int playerId, string message, int sayColor)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SSayMsg);
        packetWriter.WriteString(GetPlayerName(playerId));
        packetWriter.WriteInt32(GetPlayerAccess(playerId));
        packetWriter.WriteBoolean(GetPlayerPk(playerId));
        packetWriter.WriteString(message);
        packetWriter.WriteString("[Map]:");
        packetWriter.WriteInt32(sayColor);

        NetworkConfig.SendDataToMap(mapNum, packetWriter.GetBytes());
    }

    public static void SayMsg_Global(int playerId, string message, int sayColor)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SSayMsg);
        packetWriter.WriteString(GetPlayerName(playerId));
        packetWriter.WriteInt32(GetPlayerAccess(playerId));
        packetWriter.WriteBoolean(GetPlayerPk(playerId));
        packetWriter.WriteString(message);
        packetWriter.WriteString("[Global]:");
        packetWriter.WriteInt32(sayColor);

        PlayerService.Instance.SendDataToAll(packetWriter.GetBytes());
    }

    public static void SendPlayerData(int playerId)
    {
        NetworkConfig.SendDataToMap(GetPlayerMap(playerId), GetPlayerDataPacket(playerId));
    }

    public static void SendInventoryUpdate(int playerId, int invSlot)
    {
        var packetWriter = new PacketWriter(16);

        packetWriter.WriteEnum(ServerPackets.SPlayerInvUpdate);
        packetWriter.WriteInt32(invSlot);
        packetWriter.WriteInt32(GetPlayerInv(playerId, invSlot));
        packetWriter.WriteInt32(GetPlayerInvValue(playerId, invSlot));

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendOpenShop(int playerId, int shopNum)
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(ServerPackets.SOpenShop);
        packetWriter.WriteInt32(shopNum);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void ResetShopAction()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(ServerPackets.SResetShopAction);

        PlayerService.Instance.SendDataToAll(packetWriter.GetBytes());
    }

    public static void SendBank(int playerId)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SBank);

        for (var i = 0; i < Core.Globals.Constant.MaxBank; i++)
        {
            packetWriter.WriteInt32(Data.Bank[playerId].Item[i].Num);
            packetWriter.WriteInt32(Data.Bank[playerId].Item[i].Value);
        }

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendTradeInvite(int playerId, int tradeindex)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(ServerPackets.STradeInvite);
        packetWriter.WriteInt32(tradeindex);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendTrade(int playerId, int tradeTarget)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(ServerPackets.STrade);
        packetWriter.WriteInt32(tradeTarget);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendTradeUpdate(int playerId, byte dataType)
    {
        var packetWriter = new PacketWriter();

        var totalWorth = 0;

        var tradeTarget = Data.TempPlayer[playerId].InTrade;
        if (tradeTarget == -1)
        {
            return;
        }

        packetWriter.WriteEnum(ServerPackets.STradeUpdate);
        packetWriter.WriteInt32(dataType);

        switch (dataType)
        {
            // own inventory
            case 0:
            {
                for (var i = 0; i < Core.Globals.Constant.MaxInv; i++)
                {
                    if (Data.TempPlayer[playerId].TradeOffer[i].Num >= 0)
                    {
                        packetWriter.WriteInt32(Data.TempPlayer[playerId].TradeOffer[i].Num);
                        packetWriter.WriteInt32(Data.TempPlayer[playerId].TradeOffer[i].Value);

                        if (Data.Item[Data.TempPlayer[playerId].TradeOffer[i].Num].Type == (int) ItemCategory.Currency || Data.Item[Data.TempPlayer[playerId].TradeOffer[i].Num].Stackable == 1)
                        {
                            totalWorth += Data.Item[GetPlayerInv(playerId, Data.TempPlayer[playerId].TradeOffer[i].Num)].Price * Data.TempPlayer[playerId].TradeOffer[i].Value;
                        }
                        else
                        {
                            totalWorth += Data.Item[GetPlayerInv(playerId, Data.TempPlayer[playerId].TradeOffer[i].Num)].Price;
                        }
                    }
                    else
                    {
                        packetWriter.WriteInt32(-1);
                        packetWriter.WriteInt32(0);
                    }
                }

                break;
            }

            // other inventory
            case 1:
            {
                for (var i = 0; i < Core.Globals.Constant.MaxInv; i++)
                {
                    if (Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num >= 0)
                    {
                        packetWriter.WriteInt32(GetPlayerInv((int) tradeTarget, Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num));
                        packetWriter.WriteInt32(Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Value);

                        if (GetPlayerInv((int) tradeTarget, Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num) < 0)
                        {
                            continue;
                        }

                        if (Data.Item[GetPlayerInv((int) tradeTarget, Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num)].Type == (int) ItemCategory.Currency || Data.Item[GetPlayerInv((int) tradeTarget, Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num)].Stackable == 1)
                        {
                            totalWorth += Data.Item[GetPlayerInv((int) tradeTarget, Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num)].Price * Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Value;
                        }
                        else
                        {
                            totalWorth += Data.Item[GetPlayerInv((int) tradeTarget, Data.TempPlayer[(int) tradeTarget].TradeOffer[i].Num)].Price;
                        }
                    }
                    else
                    {
                        packetWriter.WriteInt32(-1);
                        packetWriter.WriteInt32(0);
                    }
                }

                break;
            }
        }

        // send total worth of trade
        packetWriter.WriteInt32(totalWorth);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendTradeStatus(int playerId, byte status)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(ServerPackets.STradeStatus);
        packetWriter.WriteInt32(status);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendPlayerSkills(int playerId)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SSkills);

        for (var i = 0; i < Core.Globals.Constant.MaxPlayerSkills; i++)
        {
            packetWriter.WriteInt32(GetPlayerSkill(playerId, i));
        }

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendTarget(int playerId, int target, int targetType)
    {
        var packetWriter = new PacketWriter(12);

        packetWriter.WriteEnum(ServerPackets.STarget);
        packetWriter.WriteInt32(target);
        packetWriter.WriteInt32(targetType);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendMapReport(int playerId)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SMapReport);

        for (var i = 0; i < Core.Globals.Constant.MaxMaps; i++)
        {
            packetWriter.WriteString(Data.Map[i].Name);
        }

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendAdminPanel(int playerId)
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(ServerPackets.SAdmin);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendHotbar(int playerId)
    {
        var packetWriter = new PacketWriter(4 + Core.Globals.Constant.MaxHotbar * 8);

        packetWriter.WriteEnum(ServerPackets.SHotbar);

        for (var i = 0; i < Core.Globals.Constant.MaxHotbar; i++)
        {
            packetWriter.WriteInt32(Data.Player[playerId].Hotbar[i].Slot);
            packetWriter.WriteInt32(Data.Player[playerId].Hotbar[i].SlotType);
        }

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendRightClick(int playerId)
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(ServerPackets.SrClick);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendJobEditor(int playerId)
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(ServerPackets.SJobEditor);

        PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
    }

    public static void SendEmote(int playerId, int emote)
    {
        var packetWriter = new PacketWriter(12);

        packetWriter.WriteEnum(ServerPackets.SEmote);
        packetWriter.WriteInt32(playerId);
        packetWriter.WriteInt32(emote);

        NetworkConfig.SendDataToMap(GetPlayerMap(playerId), packetWriter.GetBytes());
    }

    public static void SendChatBubble(int mapNum, int target, int targetType, string message, int color)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SChatBubble);
        packetWriter.WriteInt32(target);
        packetWriter.WriteInt32(targetType);
        packetWriter.WriteString(message);
        packetWriter.WriteInt32(color);

        NetworkConfig.SendDataToMap(mapNum, packetWriter.GetBytes());
    }

    public static void SendPlayerAttack(int playerId)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(ServerPackets.SAttack);
        packetWriter.WriteInt32(playerId);

        NetworkConfig.SendDataToMapBut(playerId, GetPlayerMap(playerId), packetWriter.GetBytes());
    }
}