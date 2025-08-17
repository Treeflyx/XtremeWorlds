using Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Server.Net;
using System;
using System.Reflection;
using Core.Common;
using Core.Configurations;
using Core.Globals;
using Core.Net;
using static Core.Globals.Command;
using Type = Core.Globals.Type;

namespace Server.Game.Net;

public sealed class GamePacketParser : PacketParser<GamePacketId.FromClient, GameSession>
{
    public GamePacketParser()
    {
        Bind(GamePacketId.FromClient.CCheckPing, Packet_Ping);
        Bind(GamePacketId.FromClient.CLogin, Packet_Login);
        Bind(GamePacketId.FromClient.CRegister, Packet_Register);
        Bind(GamePacketId.FromClient.CAddChar, Packet_AddChar);
        Bind(GamePacketId.FromClient.CUseChar, Packet_UseChar);
        Bind(GamePacketId.FromClient.CDelChar, Packet_DelChar);
        Bind(GamePacketId.FromClient.CLogout, Packet_Logout);
        Bind(GamePacketId.FromClient.CSayMsg, Packet_SayMessage);
        Bind(GamePacketId.FromClient.CBroadcastMsg, Packet_BroadCastMsg);
        Bind(GamePacketId.FromClient.CPlayerMsg, Packet_PlayerMsg);
        Bind(GamePacketId.FromClient.CAdminMsg, Packet_AdminMsg);
        Bind(GamePacketId.FromClient.CPlayerMove, Packet_PlayerMove);
        Bind(GamePacketId.FromClient.CStopPlayerMove, Packet_StopPlayerMove);
        Bind(GamePacketId.FromClient.CPlayerDir, Packet_PlayerDirection);
        Bind(GamePacketId.FromClient.CUseItem, Packet_UseItem);
        Bind(GamePacketId.FromClient.CAttack, Packet_Attack);
        Bind(GamePacketId.FromClient.CPlayerInfoRequest, Packet_PlayerInfo);
        Bind(GamePacketId.FromClient.CWarpMeTo, Packet_WarpMeTo);
        Bind(GamePacketId.FromClient.CWarpToMe, Packet_WarpToMe);
        Bind(GamePacketId.FromClient.CWarpTo, Packet_WarpTo);
        Bind(GamePacketId.FromClient.CSetSprite, Packet_SetSprite);
        Bind(GamePacketId.FromClient.CGetStats, Packet_GetStats);
        Bind(GamePacketId.FromClient.CRequestNewMap, Packet_RequestNewMap);
        Bind(GamePacketId.FromClient.CSaveMap, Packet_MapData);
        Bind(GamePacketId.FromClient.CNeedMap, Packet_NeedMap);
        Bind(GamePacketId.FromClient.CMapGetItem, Item.HandleGetItem);
        Bind(GamePacketId.FromClient.CMapDropItem, Item.HandleDropItem);
        Bind(GamePacketId.FromClient.CMapRespawn, Packet_RespawnMap);
        Bind(GamePacketId.FromClient.CMapReport, Packet_MapReport);
        Bind(GamePacketId.FromClient.CKickPlayer, Packet_KickPlayer);
        Bind(GamePacketId.FromClient.CBanList, Packet_Banlist);
        Bind(GamePacketId.FromClient.CBanDestroy, Packet_DestroyBans);
        Bind(GamePacketId.FromClient.CBanPlayer, Packet_BanPlayer);

        Bind(GamePacketId.FromClient.CRequestEditMap, Packet_RequestEditMap);

        Bind(GamePacketId.FromClient.CSetAccess, Packet_SetAccess);
        Bind(GamePacketId.FromClient.CWhosOnline, Packet_WhosOnline);
        Bind(GamePacketId.FromClient.CSetMotd, Packet_SetMotd);
        Bind(GamePacketId.FromClient.CSearch, Packet_PlayerSearch);
        Bind(GamePacketId.FromClient.CSkills, Packet_Skills);
        Bind(GamePacketId.FromClient.CCast, Packet_Cast);
        Bind(GamePacketId.FromClient.CSwapInvSlots, Packet_SwapInvSlots);
        Bind(GamePacketId.FromClient.CSwapSkillSlots, Packet_SwapSkillSlots);

        Bind(GamePacketId.FromClient.CCheckPing, Packet_CheckPing);
        Bind(GamePacketId.FromClient.CUnequip, Packet_Unequip);
        Bind(GamePacketId.FromClient.CRequestPlayerData, Packet_RequestPlayerData);
        Bind(GamePacketId.FromClient.CRequestItem, Item.HandleRequestItem);
        Bind(GamePacketId.FromClient.CRequestNpc, Packet_RequestNpc);
        Bind(GamePacketId.FromClient.CRequestResource, Resource.HandleRequestResource);
        Bind(GamePacketId.FromClient.CSpawnItem, Packet_SpawnItem);
        Bind(GamePacketId.FromClient.CTrainStat, Packet_TrainStat);

        Bind(GamePacketId.FromClient.CRequestAnimation, Animation.HandleRequestAnimation);
        Bind(GamePacketId.FromClient.CRequestSkill, Packet_RequestSkill);
        Bind(GamePacketId.FromClient.CRequestShop, Packet_RequestShop);
        Bind(GamePacketId.FromClient.CRequestLevelUp, Packet_RequestLevelUp);
        Bind(GamePacketId.FromClient.CForgetSkill, Packet_ForgetSkill);
        Bind(GamePacketId.FromClient.CCloseShop, Packet_CloseShop);
        Bind(GamePacketId.FromClient.CBuyItem, Packet_BuyItem);
        Bind(GamePacketId.FromClient.CSellItem, Packet_SellItem);
        Bind(GamePacketId.FromClient.CChangeBankSlots, Packet_ChangeBankSlots);
        Bind(GamePacketId.FromClient.CDepositItem, Packet_DepositItem);
        Bind(GamePacketId.FromClient.CWithdrawItem, Packet_WithdrawItem);
        Bind(GamePacketId.FromClient.CCloseBank, Packet_CloseBank);
        Bind(GamePacketId.FromClient.CAdminWarp, Packet_AdminWarp);

        Bind(GamePacketId.FromClient.CTradeInvite, Packet_TradeInvite);
        Bind(GamePacketId.FromClient.CHandleTradeInvite, Packet_HandleTradeInvite);
        Bind(GamePacketId.FromClient.CAcceptTrade, Packet_AcceptTrade);
        Bind(GamePacketId.FromClient.CDeclineTrade, Packet_DeclineTrade);
        Bind(GamePacketId.FromClient.CTradeItem, Packet_TradeItem);
        Bind(GamePacketId.FromClient.CUntradeItem, Packet_UntradeItem);

        Bind(GamePacketId.FromClient.CAdmin, Packet_Admin);

        Bind(GamePacketId.FromClient.CSetHotbarSlot, Packet_SetHotbarSlot);
        Bind(GamePacketId.FromClient.CDeleteHotbarSlot, Packet_DeleteHotbarSlot);
        Bind(GamePacketId.FromClient.CUseHotbarSlot, Packet_UseHotbarSlot);

        Bind(GamePacketId.FromClient.CSkillLearn, Packet_SkillLearn);

        Bind(GamePacketId.FromClient.CEventChatReply, Event.Packet_EventChatReply);
        Bind(GamePacketId.FromClient.CEvent, Event.Packet_Event);
        Bind(GamePacketId.FromClient.CRequestSwitchesAndVariables, Event.Packet_RequestSwitchesAndVariables);
        Bind(GamePacketId.FromClient.CSwitchesAndVariables, Event.Packet_SwitchesAndVariables);

        Bind(GamePacketId.FromClient.CRequestProjectile, Projectile.HandleRequestProjectile);
        Bind(GamePacketId.FromClient.CClearProjectile, Projectile.HandleClearProjectile);

        Bind(GamePacketId.FromClient.CEmote, Packet_Emote);

        Bind(GamePacketId.FromClient.CRequestParty, Party.Packet_PartyRquest);
        Bind(GamePacketId.FromClient.CAcceptParty, Party.Packet_AcceptParty);
        Bind(GamePacketId.FromClient.CDeclineParty, Party.Packet_DeclineParty);
        Bind(GamePacketId.FromClient.CLeaveParty, Party.Packet_LeaveParty);
        Bind(GamePacketId.FromClient.CPartyChatMsg, Party.Packet_PartyChatMsg);
        Bind(GamePacketId.FromClient.CRequestEditItem, Item.HandleRequestEditItem);
        Bind(GamePacketId.FromClient.CSaveItem, Item.HandleSaveItem);
        Bind(GamePacketId.FromClient.CRequestEditNpc, Npc.HandleRequestEditNpc);
        Bind(GamePacketId.FromClient.CSaveNpc, Npc.HandleSaveNpc);
        Bind(GamePacketId.FromClient.CRequestEditShop, Packet_RequestEditShop);
        Bind(GamePacketId.FromClient.CSaveShop, Packet_SaveShop);
        Bind(GamePacketId.FromClient.CRequestEditSkill, Packet_RequestEditSkill);
        Bind(GamePacketId.FromClient.CSaveSkill, Packet_SaveSkill);
        Bind(GamePacketId.FromClient.CRequestEditResource, Resource.HandleRequestEditResource);
        Bind(GamePacketId.FromClient.CSaveResource, Resource.HandleSaveResource);
        Bind(GamePacketId.FromClient.CRequestEditAnimation, Animation.HandleRequestEditAnimation);
        Bind(GamePacketId.FromClient.CSaveAnimation, Animation.HandleSaveAnimation);
        Bind(GamePacketId.FromClient.CRequestEditProjectile, Projectile.HandleRequestEditProjectile);
        Bind(GamePacketId.FromClient.CSaveProjectile, Projectile.HandleSaveProjectile);
        Bind(GamePacketId.FromClient.CRequestEditJob, Packet_RequestEditJob);
        Bind(GamePacketId.FromClient.CSaveJob, Packet_SaveJob);

        Bind(GamePacketId.FromClient.CRequestMoral, Moral.HandleRequestMoral);
        Bind(GamePacketId.FromClient.CRequestEditMoral, Moral.HandleRequestEditMoral);
        Bind(GamePacketId.FromClient.CSaveMoral, Moral.HandleSaveMoral);

        Bind(GamePacketId.FromClient.CRequestEditScript, Script.HandleRequestEditScript);
        Bind(GamePacketId.FromClient.CSaveScript, Script.HandleSaveScript);

        Bind(GamePacketId.FromClient.CCloseEditor, Packet_CloseEditor);
    }

    private static void Packet_Ping(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        Data.TempPlayer[session.Id].DataPackets += 1;
    }

    private static void Packet_Login(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var reader = new PacketReader(bytes);

        if (NetworkConfig.IsPlaying(session.Id))
        {
            NetworkSend.AlertMsg(session, SystemMessage.Connection, Menu.Login);
            return;
        }

        if (NetworkConfig.IsLoggedIn(session.Id))
        {
            return;
        }
        
        if (General.GetShutDownTimer != null && General.GetShutDownTimer.IsRunning)
        {
            NetworkSend.AlertMsg(session, SystemMessage.ServerMaintenance, Menu.Login);
            return;
        }

        var usernameBytes = reader.ReadBytes().ToArray();
        var username = System.Text.Encoding.UTF8.GetString(session.Decrypt(usernameBytes)).ToLower().Replace("\0", "");

        var passwordBytes = reader.ReadBytes().ToArray();
        var password = System.Text.Encoding.UTF8.GetString(session.Decrypt(passwordBytes)).Replace("\0", "");

        // Get the current executing assembly
        var assembly = Assembly.GetExecutingAssembly();

        // Retrieve the version information
        var clientVersionBytes = reader.ReadBytes().ToArray();
        var serverVersion = assembly.GetName().Version.ToString();
        var clientVersion = System.Text.Encoding.UTF8.GetString(session.Decrypt(clientVersionBytes));

        // Check versions
        if (clientVersion != serverVersion)
        {
            NetworkSend.AlertMsg(session, SystemMessage.ClientOutdated, Menu.Login);
            return;
        }

        if (username.Length > Core.Globals.Constant.NameLength | username.Length < Core.Globals.Constant.MinNameLength)
        {
            NetworkSend.AlertMsg(session, SystemMessage.NameLengthInvalid);
            return;
        }

        if (NetworkConfig.IsMultiLogin(session.Id, username))
        {
            NetworkSend.AlertMsg(session, SystemMessage.MultipleAccountsNotAllowed, Menu.Login);
            return;
        }

        if (!Database.LoadAccount(session.Id, username))
        {
            NetworkSend.AlertMsg(session, SystemMessage.Login, Menu.Login);
            return;
        }

        if (GetPlayerPassword(session.Id) != password)
        {
            NetworkSend.AlertMsg(session, SystemMessage.WrongPassword, Menu.Login);
            return;
        }

        if (Database.IsBanned(session.Id, session.Channel.IpAddress))
        {
            NetworkSend.AlertMsg(session, SystemMessage.Banned, Menu.Login);
            return;
        }

        if (GetAccountLogin(session.Id) == "")
        {
            NetworkSend.AlertMsg(session, SystemMessage.DatabaseError, Menu.Login);
            return;
        }

        General.Logger.LogInformation("{AccountName} has logged in from {IpAddress}",
            GetAccountLogin(session.Id), session.Channel.IpAddress);

        NetworkSend.SendPlayerChars(session);
        NetworkSend.SendJobs(session);
    }

    private static void Packet_Register(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        if (NetworkConfig.IsPlaying(session.Id) ||
            NetworkConfig.IsLoggedIn(session.Id))
        {
            return;
        }

        // check if its banned
        // Cut off last portion of ip
        if (Database.IsBanned(session.Id, session.Channel.IpAddress))
        {
            NetworkSend.AlertMsg(session, SystemMessage.Banned, Menu.Register);
            return;
        }

        if (General.GetShutDownTimer is {IsRunning: true})
        {
            NetworkSend.AlertMsg(session, SystemMessage.ServerMaintenance, Menu.Register);
            return;
        }

        var usernameBytes = buffer.ReadBytes().ToArray();
        var username = System.Text.Encoding.UTF8.GetString(session.Decrypt(usernameBytes)).ToLower().Replace("\0", "");

        var passwordBytes = buffer.ReadBytes().ToArray();
        var password = System.Text.Encoding.UTF8.GetString(session.Decrypt(passwordBytes)).Replace("\0", "");

        // Get the current executing assembly
        var assembly = Assembly.GetExecutingAssembly();

        // Retrieve the version information
        var clientVersionBytes = buffer.ReadBytes().ToArray();
        var serverVersion = assembly.GetName().Version.ToString();
        var clientVersion = System.Text.Encoding.UTF8.GetString(session.Decrypt(clientVersionBytes));

        // Check versions
        if (clientVersion != serverVersion)
        {
            NetworkSend.AlertMsg(session, SystemMessage.ClientOutdated, Menu.Register);
            return;
        }

        var x = General.IsValidUsername(username);

        switch (x) // Check if the username is valid
        {
            case -1:
                NetworkSend.AlertMsg(session, SystemMessage.NameContainsIllegalChars, Menu.Register);
                return;

            case 0:
                NetworkSend.AlertMsg(session, SystemMessage.NameLengthInvalid, Menu.Register);
                return;
        }

        if (NetworkConfig.IsMultiLogin(session.Id, username))
        {
            NetworkSend.AlertMsg(session, SystemMessage.MultipleAccountsNotAllowed, Menu.Register);
            return;
        }

        var userData = Database.SelectRowByColumn("id", Database.GetStringHash(username), "account", "data");
        if (userData is not null)
        {
            NetworkSend.AlertMsg(session, SystemMessage.NameTaken, Menu.Register);
            return;
        }

        Database.RegisterAccount(session.Id, username, password);

        // send them to the character portal
        NetworkSend.SendPlayerChars(session);
        NetworkSend.SendJobs(session);
    }

    private static void Packet_AddChar(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        string name;
        byte slot;
        int sexNum;
        int jobNum;
        int sprite;
        int i;
        int n;
        var buffer = new PacketReader(bytes);

        if (!NetworkConfig.IsPlaying(session.Id))
        {
            slot = buffer.ReadByte();
            name = buffer.ReadString();
            sexNum = buffer.ReadInt32();
            jobNum = buffer.ReadInt32();

            if (slot < 1 | slot > Core.Globals.Constant.MaxChars)
            {
                NetworkSend.AlertMsg(session, SystemMessage.MaxCharactersReached, Menu.CharacterSelect);
                return;
            }

            if (Database.LoadCharacter(session.Id, slot))
            {
                NetworkSend.SendPlayerChars(session);
                return;
            }

            var x = General.IsValidUsername(name);

            // Check if the username is valid
            if (x == -1)
            {
                NetworkSend.AlertMsg(session, SystemMessage.NameContainsIllegalChars, Menu.Register);
                return;
            }
            else if (x == 0)
            {
                NetworkSend.AlertMsg(session, SystemMessage.NameLengthInvalid, Menu.Register);
                return;
            }

            // Check if name is already in use
            if (Data.Char?.Contains(name) == true)
            {
                NetworkSend.AlertMsg(session, SystemMessage.NameTaken, Menu.NewCharacter);
                return;
            }

            if (sexNum < (byte) Sex.Male | sexNum > (byte) Sex.Female)
                return;

            if (jobNum < 0 | jobNum > Core.Globals.Constant.MaxJobs)
                return;

            if (sexNum == (byte) Sex.Male)
            {
                sprite = Data.Job[jobNum].MaleSprite;
            }
            else
            {
                sprite = Data.Job[jobNum].FemaleSprite;
            }

            if (sprite == 0)
            {
                sprite = 1;
            }

            // Everything went ok, add the character
            Data.Char.Add(name);
            Database.AddChar(session.Id, slot, name, (byte) sexNum, (byte) jobNum, sprite);

            if (Data.Char.Count == 1)
                SetPlayerAccess(session.Id, (int) AccessLevel.Owner);

            Log.Add("Character " + name + " added to " + GetAccountLogin(session.Id) + "'s account.", Constant.PlayerLog);
            global::Server.Player.HandleUseChar(session);
        }
    }

    private static void Packet_UseChar(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var reader = new PacketReader(bytes);

        if (!NetworkConfig.IsPlaying(session.Id))
        {
            if (NetworkConfig.IsLoggedIn(session.Id))
            {
                var slot = reader.ReadByte();
                if (slot < 1 | slot > Core.Globals.Constant.MaxChars)
                {
                    NetworkSend.AlertMsg(session, SystemMessage.MaxCharactersReached, Menu.CharacterSelect);
                    return;
                }

                NetworkConfig.LoadAccount(session, Data.Account[session.Id].Login, slot);
            }
        }
        else
        {
            NetworkSend.AlertMsg(session, SystemMessage.Connection, Menu.Login);
        }
    }

    private static void Packet_DelChar(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        if (!NetworkConfig.IsPlaying(session.Id))
        {
            var slot = buffer.ReadByte();
            if (slot < 1 | slot > Core.Globals.Constant.MaxChars)
            {
                NetworkSend.AlertMsg(session, SystemMessage.MaxCharactersReached, Menu.CharacterSelect);
                return;
            }

            Database.LoadCharacter(session.Id, slot);
            Data.Char.Remove(GetPlayerName(session.Id));
            Database.ClearCharacter(session.Id);
            Database.SaveCharacter(session.Id, slot);

            // send them to the character portal
            NetworkSend.SendPlayerChars(session);
        }
        else
        {
            NetworkSend.AlertMsg(session, SystemMessage.Connection, Menu.Login);
        }
    }

    private static void Packet_Logout(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        if (!NetworkConfig.IsPlaying(session.Id))
        {
            return;
        }
        
        NetworkSend.SendLeftGame(session.Id);
        
        var task = Server.Player.LeftGame(session.Id);

        task.Wait();
    }

    private static void Packet_SayMessage(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var msg = buffer.ReadString();

        Log.Add("Map #" + GetPlayerMap(session.Id) + ": " + GetPlayerName(session.Id) + " says, '" + msg + "'", Constant.PlayerLog);

        NetworkSend.SayMsg_Map(GetPlayerMap(session.Id), session.Id, msg, (int) ColorName.White);
        NetworkSend.SendChatBubble(GetPlayerMap(session.Id), session.Id, (int) TargetType.Player, msg, (int) ColorName.White);
    }

    private static void Packet_BroadCastMsg(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var msg = buffer.ReadString();

        var s = "[Global] " + GetPlayerName(session.Id) + ": " + msg;
        NetworkSend.SayMsg_Global(session.Id, msg, (int) ColorName.White);
        Log.Add(s, Constant.PlayerLog);
        Console.WriteLine(s);
    }

    public static void Packet_PlayerMsg(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var otherPlayer = buffer.ReadString();
        var msg = buffer.ReadString();

        var otherPlayerIndex = GameLogic.FindPlayer(otherPlayer);
        if (otherPlayerIndex != session.Id)
        {
            if (otherPlayerIndex >= 0)
            {
                Log.Add(GetPlayerName(session.Id) + " tells " + GetPlayerName(session.Id) + ", '" + msg + "'", Constant.PlayerLog);
                NetworkSend.PlayerMsg(otherPlayerIndex, GetPlayerName(session.Id) + " tells you, '" + msg + "'", (int) ColorName.Pink);
                NetworkSend.PlayerMsg(session.Id, "You tell " + GetPlayerName(otherPlayerIndex) + ", '" + msg + "'", (int) ColorName.Pink);
            }
            else
            {
                NetworkSend.PlayerMsg(session.Id, "Player is not online.", (int) ColorName.BrightRed);
            }
        }
        else
        {
            NetworkSend.PlayerMsg(session.Id, "Cannot message your self!", (int) ColorName.BrightRed);
        }
    }

    private static void Packet_AdminMsg(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var s = default(string);
        var buffer = new PacketReader(bytes);

        var msg = buffer.ReadString();

        NetworkSend.AdminMsg(msg);
        Log.Add(s, Constant.PlayerLog);
        Console.WriteLine(s);
    }

    private static void Packet_PlayerMove(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        if (Data.TempPlayer[session.Id].GettingMap)
            return;

        var dir = buffer.ReadByte();
        var movement = buffer.ReadByte();
        var tmpX = buffer.ReadInt32();
        var tmpY = buffer.ReadInt32();

        SetPlayerDir(session.Id, dir);
        Data.Player[session.Id].Moving = movement;
    }

    public static void Packet_StopPlayerMove(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        if (Data.TempPlayer[session.Id].GettingMap)
            return;

        Data.Player[session.Id].IsMoving = false;
        Data.Player[session.Id].Moving = 0;
    }

    public static void Packet_PlayerDirection(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        if (Data.TempPlayer[session.Id].GettingMap == true)
            return;

        var dir = buffer.ReadInt32();

        // Prevent hacking
        if (dir < (byte) Direction.Up | dir > (byte) Direction.DownRight)
            return;

        SetPlayerDir(session.Id, dir);

        var packetWriter = new PacketWriter(12);
        
        packetWriter.WriteEnum( Packets.ServerPackets.SPlayerDir);
        packetWriter.WriteInt32(session.Id);
        packetWriter.WriteByte(GetPlayerDir(session.Id));
        
        NetworkConfig.SendDataToMapBut(session.Id, GetPlayerMap(session.Id), packetWriter.GetBytes());
    }

    public static void Packet_UseItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var invNum = buffer.ReadInt32();

        global::Server.Player.UseItem(session.Id, invNum);
    }

    public static void Packet_Attack(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var x = 0;
        var y = 0;

        // can't attack whilst casting
        if (Data.TempPlayer[session.Id].SkillBuffer >= 0)
            return;

        // can't attack whilst stunned
        if (Data.TempPlayer[session.Id].StunDuration > 0)
            return;

        NetworkSend.SendPlayerAttack(session.Id);

        // Projectile check
        if (GetPlayerEquipment(session.Id, Equipment.Weapon) >= 0)
        {
            if (Data.Item[GetPlayerEquipment(session.Id, Equipment.Weapon)].Projectile > 0) // Item has a projectile
            {
                if (Data.Item[GetPlayerEquipment(session.Id, Equipment.Weapon)].Ammo > 0)
                {
                    if (Convert.ToBoolean(global::Server.Player.HasItem(session.Id, Data.Item[GetPlayerEquipment(session.Id, Equipment.Weapon)].Ammo)))
                    {
                        global::Server.Player.TakeInv(session.Id, Data.Item[GetPlayerEquipment(session.Id, Equipment.Weapon)].Ammo, 1);
                        Projectile.PlayerFireProjectile(session.Id);
                        return;
                    }
                    else
                    {
                        NetworkSend.PlayerMsg(session.Id, "No more " + Data.Item[Data.Item[GetPlayerEquipment(session.Id, Equipment.Weapon)].Ammo].Name + " !", (int) ColorName.BrightRed);
                        return;
                    }
                }
                else
                {
                    Projectile.PlayerFireProjectile(session.Id);
                    return;
                }
            }
        }

        // Check tradeskills
        switch (GetPlayerDir(session.Id))
        {
            case (byte) Direction.Up:
            {
                if (GetPlayerY(session.Id) == 0)
                    return;
                x = GetPlayerX(session.Id);
                y = GetPlayerY(session.Id) - 1;
                break;
            }
            case (byte) Direction.Down:
            {
                if (GetPlayerY(session.Id) == Data.Map[GetPlayerMap(session.Id)].MaxY)
                    return;
                x = GetPlayerX(session.Id);
                y = GetPlayerY(session.Id) + 1;
                break;
            }
            case (byte) Direction.Left:
            {
                if (GetPlayerX(session.Id) == 0)
                    return;
                x = GetPlayerX(session.Id) - 1;
                y = GetPlayerY(session.Id);
                break;
            }
            case (byte) Direction.Right:
            {
                if (GetPlayerX(session.Id) == Data.Map[GetPlayerMap(session.Id)].MaxX)
                    return;
                x = GetPlayerX(session.Id) + 1;
                y = GetPlayerY(session.Id);
                break;
            }

            case var case4 when case4 == (byte) Direction.UpRight:
            {
                if (GetPlayerX(session.Id) == Data.Map[GetPlayerMap(session.Id)].MaxX)
                    return;
                if (GetPlayerY(session.Id) == Data.Map[GetPlayerMap(session.Id)].MaxY)
                    return;
                x = GetPlayerX(session.Id) + 1;
                y = GetPlayerY(session.Id) - 1;
                break;
            }

            case var case5 when case5 == (byte) Direction.UpLeft:
            {
                if (GetPlayerX(session.Id) == Data.Map[GetPlayerMap(session.Id)].MaxX)
                    return;
                if (GetPlayerY(session.Id) == Data.Map[GetPlayerMap(session.Id)].MaxY)
                    return;
                x = GetPlayerX(session.Id) - 1;
                y = GetPlayerY(session.Id) - 1;
                break;
            }

            case var case6 when case6 == (byte) Direction.DownRight:
            {
                if (GetPlayerX(session.Id) == Data.Map[GetPlayerMap(session.Id)].MaxX)
                    return;
                if (GetPlayerY(session.Id) == Data.Map[GetPlayerMap(session.Id)].MaxY)
                    return;
                x = GetPlayerX(session.Id) + 1;
                y = GetPlayerY(session.Id) + 1;
                break;
            }

            case var case7 when case7 == (byte) Direction.DownLeft:
            {
                if (GetPlayerX(session.Id) == Data.Map[GetPlayerMap(session.Id)].MaxX)
                    return;
                if (GetPlayerY(session.Id) == Data.Map[GetPlayerMap(session.Id)].MaxY)
                    return;
                x = GetPlayerX(session.Id) - 1;
                y = GetPlayerY(session.Id) + 1;
                break;
            }
        }

        Resource.CheckResource(session.Id, x, y);
    }

    public static void Packet_PlayerInfo(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        int n;
        var buffer = new PacketReader(bytes);

        var name = buffer.ReadString();
        var i = GameLogic.FindPlayer(name);

        if (i >= 0)
        {
            NetworkSend.PlayerMsg(session.Id, "Account:  " + GetAccountLogin(i) + ", Name: " + GetPlayerName(i), (int) ColorName.Yellow);

            if (GetPlayerAccess(session.Id) > (byte) AccessLevel.Moderator)
            {
                NetworkSend.PlayerMsg(session.Id, " Stats for " + GetPlayerName(i) + " ", (int) ColorName.Yellow);
                NetworkSend.PlayerMsg(session.Id, "Level: " + GetPlayerLevel(i) + "  Exp: " + GetPlayerExp(i) + "/" + GetPlayerNextLevel(i), (int) ColorName.Yellow);
                NetworkSend.PlayerMsg(session.Id, "HP: " + GetPlayerVital(i, Vital.Health) + "/" + GetPlayerMaxVital(i, Vital.Health) + "  MP: " + GetPlayerVital(i, Vital.Stamina) + "/" + GetPlayerMaxVital(i, Vital.Stamina) + "  SP: " + GetPlayerVital(i, Vital.Stamina) + "/" + GetPlayerMaxVital(i, Vital.Stamina), (int) ColorName.Yellow);
                NetworkSend.PlayerMsg(session.Id, "Strength: " + GetPlayerStat(i, Stat.Strength) + "  Defense: " + GetPlayerStat(i, Stat.Luck) + "  Magic: " + GetPlayerStat(i, Stat.Intelligence) + "  Speed: " + GetPlayerStat(i, Stat.Spirit), (int) ColorName.Yellow);
                n = GetPlayerStat(i, Stat.Strength) / 2 + GetPlayerLevel(i) / 2;
                i = GetPlayerStat(i, Stat.Luck) / 2 + GetPlayerLevel(i) / 2;

                if (n > 100)
                    n = 100;
                if (i > 100)
                    i = 100;
                NetworkSend.PlayerMsg(session.Id, "Critical Hit Chance: " + n + "%, Block Chance: " + i + "%", (int) ColorName.Yellow);
            }
        }
        else
        {
            NetworkSend.PlayerMsg(session.Id, "Player is not online.", (int) ColorName.BrightRed);
        }
    }

    public static void Packet_WarpMeTo(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Mapper)
            return;

        // The player
        var n = GameLogic.FindPlayer(buffer.ReadString());


        if (n != session.Id)
        {
            if (n >= 0)
            {
                global::Server.Player.PlayerWarp(session.Id, GetPlayerMap(n), GetPlayerX(n), GetPlayerY(n), (byte) Direction.Down);
                NetworkSend.PlayerMsg(n, GetPlayerName(session.Id) + " has warped to you.", (int) ColorName.Yellow);
                NetworkSend.PlayerMsg(session.Id, "You have been warped to " + GetPlayerName(n) + ".", (int) ColorName.Yellow);
                Log.Add(GetPlayerName(session.Id) + " has warped to " + GetPlayerName(n) + ", map #" + GetPlayerMap(n) + ".", Constant.AdminLog);
            }
            else
            {
                NetworkSend.PlayerMsg(session.Id, "Player is not online.", (int) ColorName.BrightRed);
            }
        }
        else
        {
            NetworkSend.PlayerMsg(session.Id, "You cannot warp to yourself, dumbass!", (int) ColorName.BrightRed);
        }
    }

    public static void Packet_WarpToMe(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Mapper)
            return;

        // The player
        var n = GameLogic.FindPlayer(buffer.ReadString());


        if (n != session.Id)
        {
            if (n >= 0)
            {
                global::Server.Player.PlayerWarp(n, GetPlayerMap(session.Id), GetPlayerX(session.Id), GetPlayerY(session.Id), (byte) Direction.Down);
                NetworkSend.PlayerMsg(n, "You have been summoned by " + GetPlayerName(session.Id) + ".", (int) ColorName.Yellow);
                NetworkSend.PlayerMsg(session.Id, GetPlayerName(n) + " has been summoned.", (int) ColorName.Yellow);
                Log.Add(GetPlayerName(session.Id) + " has warped " + GetPlayerName(n) + " to self, map #" + GetPlayerMap(session.Id) + ".", Constant.AdminLog);
            }
            else
            {
                NetworkSend.PlayerMsg(session.Id, "Player is not online.", (int) ColorName.BrightRed);
            }
        }
        else
        {
            NetworkSend.PlayerMsg(session.Id, "You cannot warp yourself to yourself, dumbass!", (int) ColorName.BrightRed);
        }
    }

    public static void Packet_WarpTo(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Mapper)
            return;

        // The map
        var n = buffer.ReadInt32();


        // Prevent hacking
        if (n < 0 | n > Core.Globals.Constant.MaxMaps)
            return;

        global::Server.Player.PlayerWarp(session.Id, n, GetPlayerX(session.Id), GetPlayerY(session.Id), (byte) Direction.Down);
        NetworkSend.PlayerMsg(session.Id, "You have been warped to map #" + n, (int) ColorName.Yellow);
        Log.Add(GetPlayerName(session.Id) + " warped to map #" + n + ".", Constant.AdminLog);
    }

    public static void Packet_SetSprite(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Mapper)
            return;

        // The sprite
        var n = buffer.ReadInt32();


        SetPlayerSprite(session.Id, n);
        NetworkSend.SendPlayerData(session.Id);
    }

    public static void Packet_GetStats(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        NetworkSend.PlayerMsg(session.Id, "Stats: " + GetPlayerName(session.Id), (int) ColorName.Yellow);
        NetworkSend.PlayerMsg(session.Id, "Level: " + GetPlayerLevel(session.Id) + "  Exp: " + GetPlayerExp(session.Id) + "/" + GetPlayerNextLevel(session.Id), (int) ColorName.Yellow);
        NetworkSend.PlayerMsg(session.Id, "HP: " + GetPlayerVital(session.Id, Vital.Health) + "/" + GetPlayerMaxVital(session.Id, Vital.Health) + "  MP: " + GetPlayerVital(session.Id, Vital.Stamina) + "/" + GetPlayerMaxVital(session.Id, Vital.Stamina) + "  SP: " + GetPlayerVital(session.Id, Vital.Stamina) + "/" + GetPlayerMaxVital(session.Id, Vital.Stamina), (int) ColorName.Yellow);
        NetworkSend.PlayerMsg(session.Id, "STR: " + GetPlayerStat(session.Id, Stat.Strength) + "  DEF: " + GetPlayerStat(session.Id, Stat.Luck) + "  MAGI: " + GetPlayerStat(session.Id, Stat.Intelligence) + "  Speed: " + GetPlayerStat(session.Id, Stat.Spirit), (int) ColorName.Yellow);
        var n = GetPlayerStat(session.Id, Stat.Strength) / 2 + GetPlayerLevel(session.Id) / 2;
        var i = GetPlayerStat(session.Id, Stat.Luck) / 2 + GetPlayerLevel(session.Id) / 2;

        if (n > 100)
            n = 100;
        if (i > 100)
            i = 100;
        NetworkSend.PlayerMsg(session.Id, "Critical Hit Chance: " + n + "%, Block Chance: " + i + "%", (int) ColorName.Yellow);
    }

    public static void Packet_RequestNewMap(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var dir = buffer.ReadInt32();


        global::Server.Player.PlayerMove(session.Id, dir, 1, true);
    }

    public static void Packet_MapData(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        int x;
        int y;

        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Mapper)
        {
            return;
        }


        var mapNum = GetPlayerMap(session.Id);

        var ii = Data.Map[mapNum].Revision + 1;
        Database.ClearMap(mapNum);
        
        var packetReader = new PacketReader(bytes);

        Data.Map[mapNum].Name = packetReader.ReadString();
        Data.Map[mapNum].Music = packetReader.ReadString();
        Data.Map[mapNum].Revision = ii;
        Data.Map[mapNum].Moral = (byte) packetReader.ReadInt32();
        Data.Map[mapNum].Tileset = packetReader.ReadInt32();
        Data.Map[mapNum].Up = packetReader.ReadInt32();
        Data.Map[mapNum].Down = packetReader.ReadInt32();
        Data.Map[mapNum].Left = packetReader.ReadInt32();
        Data.Map[mapNum].Right = packetReader.ReadInt32();
        Data.Map[mapNum].BootMap = packetReader.ReadInt32();
        Data.Map[mapNum].BootX = (byte) packetReader.ReadInt32();
        Data.Map[mapNum].BootY = (byte) packetReader.ReadInt32();
        Data.Map[mapNum].MaxX = (byte) packetReader.ReadInt32();
        Data.Map[mapNum].MaxY = (byte) packetReader.ReadInt32();
        Data.Map[mapNum].Weather = (byte) packetReader.ReadInt32();
        Data.Map[mapNum].Fog = packetReader.ReadInt32();
        Data.Map[mapNum].WeatherIntensity = packetReader.ReadInt32();
        Data.Map[mapNum].FogOpacity = (byte) packetReader.ReadInt32();
        Data.Map[mapNum].FogSpeed = (byte) packetReader.ReadInt32();
        Data.Map[mapNum].MapTint = packetReader.ReadBoolean();
        Data.Map[mapNum].MapTintR = (byte) packetReader.ReadInt32();
        Data.Map[mapNum].MapTintG = (byte) packetReader.ReadInt32();
        Data.Map[mapNum].MapTintB = (byte) packetReader.ReadInt32();
        Data.Map[mapNum].MapTintA = (byte) packetReader.ReadInt32();
        Data.Map[mapNum].Panorama = packetReader.ReadByte();
        Data.Map[mapNum].Parallax = packetReader.ReadByte();
        Data.Map[mapNum].Brightness = packetReader.ReadByte();
        Data.Map[mapNum].NoRespawn = packetReader.ReadBoolean();
        Data.Map[mapNum].Indoors = packetReader.ReadBoolean();
        Data.Map[mapNum].Shop = packetReader.ReadInt32();

        Data.Map[mapNum].Tile = new Type.Tile[Data.Map[mapNum].MaxX, Data.Map[mapNum].MaxY];
        
        for (x = 0; x < Core.Globals.Constant.MaxMapNpcs; x++)
        {
            Database.ClearMapNpc(x, mapNum);
            
            Data.Map[mapNum].Npc[x] = packetReader.ReadInt32();
        }

        {
            ref var withBlock = ref Data.Map[mapNum];
            var loopTo1 = (int) withBlock.MaxX;
            for (x = 0; x < loopTo1; x++)
            {
                var loopTo2 = (int) withBlock.MaxY;
                for (y = 0; y < loopTo2; y++)
                {
                    withBlock.Tile[x, y].Data1 = packetReader.ReadInt32();
                    withBlock.Tile[x, y].Data2 = packetReader.ReadInt32();
                    withBlock.Tile[x, y].Data3 = packetReader.ReadInt32();
                    withBlock.Tile[x, y].Data1_2 = packetReader.ReadInt32();
                    withBlock.Tile[x, y].Data2_2 = packetReader.ReadInt32();
                    withBlock.Tile[x, y].Data3_2 = packetReader.ReadInt32();
                    withBlock.Tile[x, y].DirBlock = (byte) packetReader.ReadInt32();
                    var loopTo3 = Enum.GetValues(typeof(MapLayer)).Length;
                    withBlock.Tile[x, y].Layer = new Type.Layer[loopTo3];
                    for (var i = 0; i < (int) loopTo3; i++)
                    {
                        withBlock.Tile[x, y].Layer[i].Tileset = packetReader.ReadInt32();
                        withBlock.Tile[x, y].Layer[i].X = packetReader.ReadInt32();
                        withBlock.Tile[x, y].Layer[i].Y = packetReader.ReadInt32();
                        withBlock.Tile[x, y].Layer[i].AutoTile = (byte) packetReader.ReadInt32();
                    }

                    withBlock.Tile[x, y].Type = (TileType) packetReader.ReadInt32();
                    withBlock.Tile[x, y].Type2 = (TileType) packetReader.ReadInt32();
                }
            }
        }

        Data.Map[mapNum].EventCount = packetReader.ReadInt32();

        if (Data.Map[mapNum].EventCount > 0)
        {
            Data.Map[mapNum].Event = new Type.Event[Data.Map[mapNum].EventCount];
            var loopTo4 = Data.Map[mapNum].EventCount;
            for (var i = 0; i < loopTo4; i++)
            {
                {
                    ref var withBlock1 = ref Data.Map[mapNum].Event[i];
                    withBlock1.Name = packetReader.ReadString();
                    withBlock1.Globals = packetReader.ReadByte();
                    withBlock1.X = packetReader.ReadInt32();
                    withBlock1.Y = packetReader.ReadInt32();
                    withBlock1.PageCount = packetReader.ReadInt32();
                }

                if (Data.Map[mapNum].Event[i].PageCount > 0)
                {
                    Data.Map[mapNum].Event[i].Pages = new Type.EventPage[Data.Map[mapNum].Event[i].PageCount];
                    Array.Resize(ref Data.TempPlayer[i].EventMap.EventPages, Data.Map[mapNum].Event[i].PageCount);

                    var loopTo5 = Data.Map[mapNum].Event[i].PageCount;
                    for (x = 0; x < (int) loopTo5; x++)
                    {
                        {
                            ref var withBlock2 = ref Data.Map[mapNum].Event[i].Pages[x];
                            withBlock2.ChkVariable = packetReader.ReadInt32();
                            withBlock2.VariableIndex = packetReader.ReadInt32();
                            withBlock2.VariableCondition = packetReader.ReadInt32();
                            withBlock2.VariableCompare = packetReader.ReadInt32();

                            withBlock2.ChkSwitch = packetReader.ReadInt32();
                            withBlock2.SwitchIndex = packetReader.ReadInt32();
                            withBlock2.SwitchCompare = packetReader.ReadInt32();

                            withBlock2.ChkHasItem = packetReader.ReadInt32();
                            withBlock2.HasItemIndex = packetReader.ReadInt32();
                            withBlock2.HasItemAmount = packetReader.ReadInt32();

                            withBlock2.ChkSelfSwitch = packetReader.ReadInt32();
                            withBlock2.SelfSwitchIndex = packetReader.ReadInt32();
                            withBlock2.SelfSwitchCompare = packetReader.ReadInt32();

                            withBlock2.GraphicType = packetReader.ReadByte();
                            withBlock2.Graphic = packetReader.ReadInt32();
                            withBlock2.GraphicX = packetReader.ReadInt32();
                            withBlock2.GraphicY = packetReader.ReadInt32();
                            withBlock2.GraphicX2 = packetReader.ReadInt32();
                            withBlock2.GraphicY2 = packetReader.ReadInt32();

                            withBlock2.MoveType = packetReader.ReadByte();
                            withBlock2.MoveSpeed = packetReader.ReadByte();
                            withBlock2.MoveFreq = packetReader.ReadByte();
                            withBlock2.MoveRouteCount = packetReader.ReadInt32();
                            withBlock2.IgnoreMoveRoute = packetReader.ReadInt32();
                            withBlock2.RepeatMoveRoute = packetReader.ReadInt32();

                            if (withBlock2.MoveRouteCount > 0)
                            {
                                Data.Map[mapNum].Event[i].Pages[x].MoveRoute = new Type.MoveRoute[withBlock2.MoveRouteCount];
                                var loopTo6 = withBlock2.MoveRouteCount;
                                for (y = 0; y < (int) loopTo6; y++)
                                {
                                    withBlock2.MoveRoute[y].Index = packetReader.ReadInt32();
                                    withBlock2.MoveRoute[y].Data1 = packetReader.ReadInt32();
                                    withBlock2.MoveRoute[y].Data2 = packetReader.ReadInt32();
                                    withBlock2.MoveRoute[y].Data3 = packetReader.ReadInt32();
                                    withBlock2.MoveRoute[y].Data4 = packetReader.ReadInt32();
                                    withBlock2.MoveRoute[y].Data5 = packetReader.ReadInt32();
                                    withBlock2.MoveRoute[y].Data6 = packetReader.ReadInt32();
                                }
                            }

                            withBlock2.WalkAnim = packetReader.ReadInt32();
                            withBlock2.DirFix = packetReader.ReadInt32();
                            withBlock2.WalkThrough = packetReader.ReadInt32();
                            withBlock2.ShowName = packetReader.ReadInt32();
                            withBlock2.Trigger = packetReader.ReadByte();
                            withBlock2.CommandListCount = packetReader.ReadInt32();
                            withBlock2.Position = packetReader.ReadByte();
                        }

                        if (Data.Map[mapNum].Event[i].Pages[x].CommandListCount > 0)
                        {
                            Data.Map[mapNum].Event[i].Pages[x].CommandList = new Type.CommandList[Data.Map[mapNum].Event[i].Pages[x].CommandListCount];
                            var loopTo7 = Data.Map[mapNum].Event[i].Pages[x].CommandListCount;
                            for (y = 0; y < (int) loopTo7; y++)
                            {
                                Data.Map[mapNum].Event[i].Pages[x].CommandList[y].CommandCount = packetReader.ReadInt32();
                                Data.Map[mapNum].Event[i].Pages[x].CommandList[y].ParentList = packetReader.ReadInt32();
                                if (Data.Map[mapNum].Event[i].Pages[x].CommandList[y].CommandCount > 0)
                                {
                                    Data.Map[mapNum].Event[i].Pages[x].CommandList[y].Commands = new Type.EventCommand[Data.Map[mapNum].Event[i].Pages[x].CommandList[y].CommandCount];
                                    for (int z = 0, loopTo8 = Data.Map[mapNum].Event[i].Pages[x].CommandList[y].CommandCount; z < (int) loopTo8; z++)
                                    {
                                        {
                                            ref var withBlock3 = ref Data.Map[mapNum].Event[i].Pages[x].CommandList[y].Commands[z];
                                            withBlock3.Index = packetReader.ReadInt32();
                                            withBlock3.Text1 = packetReader.ReadString();
                                            withBlock3.Text2 = packetReader.ReadString();
                                            withBlock3.Text3 = packetReader.ReadString();
                                            withBlock3.Text4 = packetReader.ReadString();
                                            withBlock3.Text5 = packetReader.ReadString();
                                            withBlock3.Data1 = packetReader.ReadInt32();
                                            withBlock3.Data2 = packetReader.ReadInt32();
                                            withBlock3.Data3 = packetReader.ReadInt32();
                                            withBlock3.Data4 = packetReader.ReadInt32();
                                            withBlock3.Data5 = packetReader.ReadInt32();
                                            withBlock3.Data6 = packetReader.ReadInt32();
                                            withBlock3.ConditionalBranch.CommandList = packetReader.ReadInt32();
                                            withBlock3.ConditionalBranch.Condition = packetReader.ReadInt32();
                                            withBlock3.ConditionalBranch.Data1 = packetReader.ReadInt32();
                                            withBlock3.ConditionalBranch.Data2 = packetReader.ReadInt32();
                                            withBlock3.ConditionalBranch.Data3 = packetReader.ReadInt32();
                                            withBlock3.ConditionalBranch.ElseCommandList = packetReader.ReadInt32();
                                            withBlock3.MoveRouteCount = packetReader.ReadInt32();
                                            var tmpCount = withBlock3.MoveRouteCount;
                                            if (tmpCount > 0)
                                            {
                                                Array.Resize(ref withBlock3.MoveRoute, tmpCount);
                                                for (int w = 0, loopTo9 = tmpCount; w < (int) loopTo9; w++)
                                                {
                                                    withBlock3.MoveRoute[w].Index = packetReader.ReadInt32();
                                                    withBlock3.MoveRoute[w].Data1 = packetReader.ReadInt32();
                                                    withBlock3.MoveRoute[w].Data2 = packetReader.ReadInt32();
                                                    withBlock3.MoveRoute[w].Data3 = packetReader.ReadInt32();
                                                    withBlock3.MoveRoute[w].Data4 = packetReader.ReadInt32();
                                                    withBlock3.MoveRoute[w].Data5 = packetReader.ReadInt32();
                                                    withBlock3.MoveRoute[w].Data6 = packetReader.ReadInt32();
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

        var loopTo13 = Data.Map[mapNum].EventCount;
        for (var i = 0; i < loopTo13; i++)
        {
            if (Data.Map[mapNum].Event[i].PageCount == 0)
            {
                Data.Map[mapNum].Event[i] = Data.Map[mapNum].Event[i + 1];
                Data.Map[mapNum].Event[i + 1] = default;
                Data.Map[mapNum].EventCount = Data.Map[mapNum].EventCount - 1;
            }
        } 

        // Save the map
        Database.SaveMap(mapNum);
        Npc.SpawnMapNpcs(mapNum);
        EventLogic.SpawnGlobalEvents(mapNum);

        foreach (var i in PlayerService.Instance.PlayerIds)
        {
            if (NetworkConfig.IsPlaying(i))
            {
                if (Data.Player[i].Map == mapNum)
                {
                    EventLogic.SpawnMapEventsFor(i, mapNum);
                }
            }
        }

        // Clear it all out
        var loopTo11 = Core.Globals.Constant.MaxMapItems;
        for (var i = 0; i < loopTo11; i++)
        {
            Item.SpawnItemSlot(i, -1, 0, GetPlayerMap(session.Id), Data.MapItem[GetPlayerMap(session.Id), i].X, Data.MapItem[GetPlayerMap(session.Id), i].Y);
            Database.ClearMapItem(i, GetPlayerMap(session.Id));
        }

        // Respawn
        Item.SpawnMapItems(GetPlayerMap(session.Id));
        Resource.CacheResources(mapNum);

        // Refresh map for everyone online
        foreach (var i in PlayerService.Instance.PlayerIds)
        {
            if (NetworkConfig.IsPlaying(i) & GetPlayerMap(i) == mapNum)
            {
                global::Server.Player.PlayerWarp(i, mapNum, GetPlayerX(i), GetPlayerY(i), (byte) Direction.Down);
                NetworkSend.SendMapData(i, mapNum, true);
            }
        }
    }

    private static void Packet_NeedMap(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // Get yes/no value
        var s = buffer.ReadInt32();


        // Check if data is needed to be sent
        if (s == 1)
        {
            NetworkSend.SendMapData(session.Id, GetPlayerMap(session.Id), true);
        }
        else
        {
            NetworkSend.SendMapData(session.Id, GetPlayerMap(session.Id), false);
        }

        if (Data.Map[GetPlayerMap(session.Id)].Shop >= 0 && Data.Map[GetPlayerMap(session.Id)].Shop < Core.Globals.Constant.MaxShops)
        {
            if (!string.IsNullOrEmpty(Data.Shop[Data.Map[GetPlayerMap(session.Id)].Shop].Name))
            {
                Data.TempPlayer[session.Id].InShop = Data.Map[GetPlayerMap(session.Id)].Shop;
                NetworkSend.SendOpenShop(session.Id, Data.Map[GetPlayerMap(session.Id)].Shop);
            }
        }

        Data.TempPlayer[session.Id].GettingMap = false;
    }

    public static void Packet_RespawnMap(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        int i;
        var buffer = new PacketReader(bytes);

        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Mapper)
            return;

        // Clear out it all
        var loopTo = Core.Globals.Constant.MaxMapItems;
        for (i = 0; i < loopTo; i++)
        {
            Item.SpawnItemSlot(i, -1, 0, GetPlayerMap(session.Id), Data.MapItem[GetPlayerMap(session.Id), i].X, Data.MapItem[GetPlayerMap(session.Id), i].Y);
            Database.ClearMapItem(i, GetPlayerMap(session.Id));
        }

        // Respawn
        Item.SpawnMapItems(GetPlayerMap(session.Id));

        // Respawn NpcS
        var loopTo1 = Core.Globals.Constant.MaxMapNpcs;
        for (i = 0; i < loopTo1; i++)
            Npc.SpawnNpc(i, GetPlayerMap(session.Id));

        EventLogic.SpawnMapEventsFor(session.Id, GetPlayerMap(session.Id));

        Resource.CacheResources(GetPlayerMap(session.Id));
        NetworkSend.PlayerMsg(session.Id, "Map respawned.", (int) ColorName.BrightGreen);
        Log.Add(GetPlayerName(session.Id) + " has respawned map #" + GetPlayerMap(session.Id), Constant.AdminLog);
    }

    public static void Packet_MapReport(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Mapper)
            return;

        NetworkSend.SendMapReport(session.Id);
    }

    public static void Packet_KickPlayer(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Moderator)
        {
            return;
        }

        // The player session.Id
        var n = GameLogic.FindPlayer(buffer.ReadString());


        if (n != session.Id)
        {
            if (n >= 0)
            {
                if (GetPlayerAccess(n) < GetPlayerAccess(session.Id))
                {
                    NetworkSend.GlobalMsg(GetPlayerName(n) + " has been kicked from " + SettingsManager.Instance.GameName + " by " + GetPlayerName(session.Id) + "!");
                    Log.Add(GetPlayerName(session.Id) + " has kicked " + GetPlayerName(n) + ".", Constant.AdminLog);
                    NetworkSend.AlertMsg(session, SystemMessage.Kicked, Menu.Login);
                }
                else
                {
                    NetworkSend.PlayerMsg(session.Id, "That is a higher or same access admin then you!", (int) ColorName.BrightRed);
                }
            }
            else
            {
                NetworkSend.PlayerMsg(session.Id, "Player is not online.", (int) ColorName.BrightRed);
            }
        }
        else
        {
            NetworkSend.PlayerMsg(session.Id, "You cannot kick yourself!", (int) ColorName.BrightRed);
        }
    }

    public static void Packet_Banlist(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Moderator)
        {
            return;
        }

        NetworkSend.PlayerMsg(session.Id, "Command /banlist is not available.", (int) ColorName.Yellow);
    }

    public static void Packet_DestroyBans(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Owner)
            return;

        var filename = System.IO.Path.Combine(DataPath.Database, "banlist.txt");

        if (File.Exists(filename))
            File.Delete(filename);

        NetworkSend.PlayerMsg(session.Id, "Ban list destroyed.", (int) ColorName.BrightGreen);
    }

    public static void Packet_BanPlayer(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Moderator)
            return;

        // The player session.Id
        var n = GameLogic.FindPlayer(buffer.ReadString());

        if (n != session.Id)
        {
            if (n >= 0)
            {
                if (GetPlayerAccess(n) < GetPlayerAccess(session.Id))
                {
                    Database.BanPlayer(n, session.Id);
                }
                else
                {
                    NetworkSend.PlayerMsg(session.Id, "That is a higher or same access admin then you!", (int) ColorName.BrightRed);
                }
            }
            else
            {
                NetworkSend.PlayerMsg(session.Id, "Player is not online.", (int) ColorName.BrightRed);
            }
        }
        else
        {
            NetworkSend.PlayerMsg(session.Id, "You cannot ban yourself!", (int) ColorName.BrightRed);
        }
    }

    private static void Packet_RequestEditMap(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Mapper)
            return;

        var user = IsEditorLocked(session.Id, EditorType.Map);

        if (!string.IsNullOrEmpty(user))
        {
            NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) ColorName.BrightRed);
            return;
        }

        Npc.SendNpcs(session.Id);
        Item.SendItems(session.Id);
        Animation.SendAnimations(session.Id);
        NetworkSend.SendShops(session.Id);
        Resource.SendResources(session.Id);
        Event.SendMapEventData(session.Id);
        Moral.SendMorals(session.Id);

        Data.TempPlayer[session.Id].Editor = EditorType.Map;

        var packetWriter = new PacketWriter(4);
        
        packetWriter.WriteEnum(Packets.ServerPackets.SEditMap);

        PlayerService.Instance.SendDataTo(session.Id, packetWriter.GetBytes());
    }

    public static void Packet_RequestEditShop(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
            return;

        var user = IsEditorLocked(session.Id, EditorType.Shop);

        if (!string.IsNullOrEmpty(user))
        {
            NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) ColorName.BrightRed);
            return;
        }

        Data.TempPlayer[session.Id].Editor = EditorType.Shop;

        Item.SendItems(session.Id);
        NetworkSend.SendShops(session.Id);

        var packetWriter = new PacketWriter(4);
        
        packetWriter.WriteEnum(Packets.ServerPackets.SShopEditor);
        
        PlayerService.Instance.SendDataTo(session.Id, packetWriter.GetBytes());
    }

    public static void Packet_SaveShop(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
            return;

        var shopNum = buffer.ReadInt32();

        // Prevent hacking
        if (shopNum < 0 | shopNum > Core.Globals.Constant.MaxShops)
            return;

        Data.Shop[shopNum].BuyRate = buffer.ReadInt32();
        Data.Shop[shopNum].Name = buffer.ReadString();

        for (int i = 0, loopTo = Core.Globals.Constant.MaxTrades; i < loopTo; i++)
        {
            Data.Shop[shopNum].TradeItem[i].CostItem = buffer.ReadInt32();
            Data.Shop[shopNum].TradeItem[i].CostValue = buffer.ReadInt32();
            Data.Shop[shopNum].TradeItem[i].Item = buffer.ReadInt32();
            Data.Shop[shopNum].TradeItem[i].ItemValue = buffer.ReadInt32();
        }


        // Save it
        NetworkSend.SendUpdateShopToAll(shopNum);
        Database.SaveShop(shopNum);
        Log.Add(GetAccountLogin(session.Id) + " saving shop #" + shopNum + ".", Constant.AdminLog);
    }

    public static void Packet_RequestEditSkill(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
            return;

        var user = IsEditorLocked(session.Id, EditorType.Skill);

        if (!string.IsNullOrEmpty(user))
        {
            NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) ColorName.BrightRed);
            return;
        }

        Data.TempPlayer[session.Id].Editor = EditorType.Skill;

        NetworkSend.SendJobs(session);
        Projectile.SendProjectiles(session.Id);
        Animation.SendAnimations(session.Id);
        NetworkSend.SendSkills(session.Id);

        var packetWriter = new PacketWriter(4);
        
        packetWriter.WriteEnum(Packets.ServerPackets.SSkillEditor);
        
        PlayerService.Instance.SendDataTo(session.Id, packetWriter.GetBytes());
    }

    public static void Packet_SaveSkill(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var skillNum = buffer.ReadInt32();

        // Prevent hacking
        if (skillNum < 0 | skillNum > Core.Globals.Constant.MaxSkills)
            return;

        Data.Skill[skillNum].AccessReq = buffer.ReadInt32();
        Data.Skill[skillNum].AoE = buffer.ReadInt32();
        Data.Skill[skillNum].CastAnim = buffer.ReadInt32();
        Data.Skill[skillNum].CastTime = buffer.ReadInt32();
        Data.Skill[skillNum].CdTime = buffer.ReadInt32();
        Data.Skill[skillNum].JobReq = buffer.ReadInt32();
        Data.Skill[skillNum].Dir = (byte) buffer.ReadInt32();
        Data.Skill[skillNum].Duration = buffer.ReadInt32();
        Data.Skill[skillNum].Icon = buffer.ReadInt32();
        Data.Skill[skillNum].Interval = buffer.ReadInt32();
    Data.Skill[skillNum].IsAoE = Convert.ToBoolean(buffer.ReadInt32());
        Data.Skill[skillNum].LevelReq = buffer.ReadInt32();
        Data.Skill[skillNum].Map = buffer.ReadInt32();
        Data.Skill[skillNum].MpCost = buffer.ReadInt32();
        Data.Skill[skillNum].Name = buffer.ReadString();
        Data.Skill[skillNum].Range = buffer.ReadInt32();
        Data.Skill[skillNum].SkillAnim = buffer.ReadInt32();
        Data.Skill[skillNum].StunDuration = buffer.ReadInt32();
        Data.Skill[skillNum].Type = (byte) buffer.ReadInt32();
        Data.Skill[skillNum].Vital = buffer.ReadInt32();
        Data.Skill[skillNum].X = buffer.ReadInt32();
        Data.Skill[skillNum].Y = buffer.ReadInt32();

        // projectiles
        Data.Skill[skillNum].IsProjectile = buffer.ReadInt32();
        Data.Skill[skillNum].Projectile = buffer.ReadInt32();

        Data.Skill[skillNum].KnockBack = (byte) buffer.ReadInt32();
        Data.Skill[skillNum].KnockBackTiles = (byte) buffer.ReadInt32();

        // Save it
        NetworkSend.SendUpdateSkillToAll(skillNum);
        Database.SaveSkill(skillNum);
        Log.Add(GetAccountLogin(session.Id) + " saved Skill #" + skillNum + ".", Constant.AdminLog);
    }

    public static void Packet_SetAccess(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Owner)
            return;

        // The session.Id
        var n = GameLogic.FindPlayer(buffer.ReadString());

        // The access
        var i = buffer.ReadInt32();

        // Check for invalid access level
        if (i >= (int) AccessLevel.Player && i <= (int) AccessLevel.Owner)
        {
            // Check if player is on
            if (n >= 0)
            {
                if (n != session.Id)
                {
                    // check to see if same level access is trying to change another access of the very same level and boot them if they are.
                    if (GetPlayerAccess(n) == GetPlayerAccess(session.Id))
                    {
                        NetworkSend.PlayerMsg(session.Id, "Invalid access level.", (int) ColorName.BrightRed);
                        return;
                    }
                }

                if (GetPlayerAccess(n) == (int) AccessLevel.Player && i > (int) AccessLevel.Player)
                {
                    NetworkSend.GlobalMsg(GetPlayerName(n) + " has been blessed with administrative access.");
                }

                SetPlayerAccess(n, (byte) i);
                NetworkSend.SendPlayerData(n);
                Log.Add(GetPlayerName(session.Id) + " has modified " + GetPlayerName(n) + "'s access.", Constant.AdminLog);
            }
            else
            {
                NetworkSend.PlayerMsg(session.Id, "Player is not online.", (int) ColorName.BrightRed);
            }
        }
        else
        {
            NetworkSend.PlayerMsg(session.Id, "Invalid access level.", (int) ColorName.BrightRed);
        }
    }

    public static void Packet_WhosOnline(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        NetworkSend.SendWhosOnline(session.Id);
    }

    public static void Packet_SetMotd(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Mapper)
            return;

        SettingsManager.Instance.Welcome = buffer.ReadString();
        SettingsManager.Save();

        NetworkSend.GlobalMsg("Welcome changed to: " + SettingsManager.Instance.Welcome);
        Log.Add(GetPlayerName(session.Id) + " changed welcome to: " + SettingsManager.Instance.Welcome, Constant.AdminLog);
    }

    public static void Packet_PlayerSearch(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var x = buffer.ReadInt32();
        var y = buffer.ReadInt32();
        var rclick = (byte) buffer.ReadInt32();

        // Prevent subscript out of range
        if (x < 0 | x > (int) Data.Map[GetPlayerMap(session.Id)].MaxX | y < 0 | y > (int) Data.Map[GetPlayerMap(session.Id)].MaxY)
            return;

        // Check for a player   
        foreach (var i in PlayerService.Instance.PlayerIds)
        {
            if (GetPlayerMap(session.Id) == GetPlayerMap(i))
            {
                if (GetPlayerX(i) == x)
                {
                    if (GetPlayerY(i) == y)
                    {
                        // Consider the player
                        if (i != session.Id)
                        {
                            if (GetPlayerLevel(i) >= GetPlayerLevel(session.Id) + 5)
                            {
                                NetworkSend.PlayerMsg(session.Id, "You wouldn't stand a chance.", (int) ColorName.BrightRed);
                            }

                            else if (GetPlayerLevel(i) > GetPlayerLevel(session.Id))
                            {
                                NetworkSend.PlayerMsg(session.Id, "This one seems to have an advantage over you.", (int) ColorName.Yellow);
                            }

                            else if (GetPlayerLevel(i) == GetPlayerLevel(session.Id))
                            {
                                NetworkSend.PlayerMsg(session.Id, "This would be an even fight.", (int) ColorName.White);
                            }

                            else if (GetPlayerLevel(session.Id) >= GetPlayerLevel(i) + 5)
                            {
                                NetworkSend.PlayerMsg(session.Id, "You could slaughter that player.", (int) ColorName.BrightBlue);
                            }

                            else if (GetPlayerLevel(session.Id) > GetPlayerLevel(i))
                            {
                                NetworkSend.PlayerMsg(session.Id, "You would have an advantage over that player.", (int) ColorName.BrightCyan);
                            }
                        }

                        // Change target
                        if (Data.TempPlayer[session.Id].TargetType == 0 | i != Data.TempPlayer[session.Id].Target)
                        {
                            Data.TempPlayer[session.Id].Target = i;
                            Data.TempPlayer[session.Id].TargetType = (byte) TargetType.Player;
                        }
                        else
                        {
                            Data.TempPlayer[session.Id].Target = -1;
                            Data.TempPlayer[session.Id].TargetType = 0;
                        }

                        if (Data.TempPlayer[session.Id].Target >= 0)
                        {
                            NetworkSend.PlayerMsg(session.Id, "Your target is now " + GetPlayerName(i) + ".", (int) ColorName.Yellow);
                        }

                        NetworkSend.SendTarget(session.Id, Data.TempPlayer[session.Id].Target, Data.TempPlayer[session.Id].TargetType);
                        if (rclick == 1)
                            NetworkSend.SendRightClick(session.Id);
                        return;
                    }
                }
            }
        }

        // Check for an item
        var loopTo1 = Core.Globals.Constant.MaxMapItems;
        for (var i = 0; i < loopTo1; i++)
        {
            if (Data.MapItem[GetPlayerMap(session.Id), i].Num >= 0)
            {
                if (!string.IsNullOrEmpty(Data.Item[(int) Data.MapItem[GetPlayerMap(session.Id), i].Num].Name))
                {
                    if ((int) Data.MapItem[GetPlayerMap(session.Id), i].X == x)
                    {
                        if ((int) Data.MapItem[GetPlayerMap(session.Id), i].Y == y)
                        {
                            NetworkSend.PlayerMsg(session.Id, "You see " + Data.MapItem[GetPlayerMap(session.Id), i].Value + " " + Data.Item[(int) Data.MapItem[GetPlayerMap(session.Id), i].Num].Name + ".", (int) ColorName.BrightGreen);
                            return;
                        }
                    }
                }
            }
        }

        // Check for an npc
        var loopTo2 = Core.Globals.Constant.MaxMapNpcs;
        for (var i = 0; i < loopTo2; i++)
        {
            if (Data.MapNpc[GetPlayerMap(session.Id)].Npc[i].Num >= 0)
            {
                if (Data.MapNpc[GetPlayerMap(session.Id)].Npc[i].X == x)
                {
                    if (Data.MapNpc[GetPlayerMap(session.Id)].Npc[i].Y == y)
                    {
                        // Change target
                        if (Data.TempPlayer[session.Id].TargetType == 0)
                        {
                            Data.TempPlayer[session.Id].Target = i;
                            Data.TempPlayer[session.Id].TargetType = (byte) TargetType.Npc;
                        }
                        else
                        {
                            Data.TempPlayer[session.Id].Target = -1;
                            Data.TempPlayer[session.Id].TargetType = 0;
                        }

                        if (Data.TempPlayer[session.Id].Target >= 0)
                        {
                            NetworkSend.PlayerMsg(session.Id, "Your target is now " + GameLogic.CheckGrammar(Data.Npc[(int) Data.MapNpc[GetPlayerMap(session.Id)].Npc[i].Num].Name) + ".", (int) ColorName.Yellow);
                        }

                        NetworkSend.SendTarget(session.Id, Data.TempPlayer[session.Id].Target, Data.TempPlayer[session.Id].TargetType);
                        return;
                    }
                }
            }
        }
    }

    public static void Packet_Skills(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        NetworkSend.SendPlayerSkills(session.Id);
    }

    public static void Packet_Cast(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // Skill slot
        var n = buffer.ReadInt32();


        if ((int) Data.Map[GetPlayerMap(session.Id)].Moral >= 0)
        {
            if (Data.Moral[Data.Map[GetPlayerMap(session.Id)].Moral].CanCast)
            {
                try
                {
                    Script.Instance?.BufferSkill(session.Id, n);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    public static void Packet_SwapInvSlots(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        if (Data.TempPlayer[session.Id].InTrade >= 0 | Data.TempPlayer[session.Id].InBank | Data.TempPlayer[session.Id].InShop >= 0)
            return;

        // Old Slot
        double oldSlot = buffer.ReadInt32();
        double newSlot = buffer.ReadInt32();


        global::Server.Player.PlayerSwitchInvSlots(session.Id, (int) oldSlot, (int) newSlot);
    }

    public static void Packet_SwapSkillSlots(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        if (Data.TempPlayer[session.Id].InTrade >= 0 | Data.TempPlayer[session.Id].InBank | Data.TempPlayer[session.Id].InShop >= 0)
            return;

        // Old Slot
        double oldSlot = buffer.ReadInt32();
        double newSlot = buffer.ReadInt32();


        global::Server.Player.PlayerSwitchSkillSlots(session.Id, (int) oldSlot, (int) newSlot);
    }

    public static void Packet_CheckPing(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var packetWriter = new PacketWriter(4);
        
        packetWriter.WriteEnum(Packets.ServerPackets.SSendPing);

        PlayerService.Instance.SendDataTo(session.Id, packetWriter.GetBytes());
    }

    public static void Packet_Unequip(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        Server.Player.UnequipItem(session.Id, buffer.ReadInt32());
    }

    public static void Packet_RequestPlayerData(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        NetworkSend.SendPlayerData(session.Id);
    }

    public static void Packet_RequestNpc(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var n = buffer.ReadInt32();

        if (n < 0 | n > Core.Globals.Constant.MaxNpcs)
            return;

        Npc.SendUpdateNpcTo(session.Id, n);
    }

    public static void Packet_SpawnItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // item
        var tmpItem = buffer.ReadInt32();
        var tmpAmount = buffer.ReadInt32();

        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
            return;

        Item.SpawnItem(tmpItem, tmpAmount, GetPlayerMap(session.Id), GetPlayerX(session.Id), GetPlayerY(session.Id));
    }

    public static void Packet_TrainStat(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // check points
        if (GetPlayerPoints(session.Id) == 0)
            return;

        // stat
        var tmpStat = buffer.ReadInt32();

        try
        {
            Script.Instance?.TrainStat(session.Id, tmpStat);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void Packet_RequestSkill(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var n = buffer.ReadInt32();

        if (n < 0 | n > Core.Globals.Constant.MaxSkills)
            return;

        NetworkSend.SendUpdateSkillTo(session.Id, n);
    }

    public static void Packet_RequestShop(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var n = buffer.ReadInt32();

        if (n < 0 | n > Core.Globals.Constant.MaxShops)
            return;

        NetworkSend.SendUpdateShopTo(session.Id, n);
    }

    public static void Packet_RequestLevelUp(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
            return;

        SetPlayerExp(session.Id, GetPlayerNextLevel(session.Id));
        Server.Player.CheckPlayerLevelUp(session.Id);
    }

    public static void Packet_ForgetSkill(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var skillSlot = buffer.ReadInt32();

        // Check for subscript out of range
        if (skillSlot < 0 | skillSlot > Core.Globals.Constant.MaxPlayerSkills)
            return;

        // dont let them forget a skill which is in CD
        if (Data.TempPlayer[session.Id].SkillCd[skillSlot] > 0)
        {
            NetworkSend.PlayerMsg(session.Id, "Cannot forget a skill which is cooling down!", (int) ColorName.BrightRed);
            return;
        }

        // dont let them forget a skill which is buffered
        if (Data.TempPlayer[session.Id].SkillBuffer == skillSlot)
        {
            NetworkSend.PlayerMsg(session.Id, "Cannot forget a skill which you are casting!", (int) ColorName.BrightRed);
            return;
        }

        Data.Player[session.Id].Skill[skillSlot].Num = -1;
        NetworkSend.SendPlayerSkills(session.Id);
    }

    public static void Packet_CloseShop(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        Data.TempPlayer[session.Id].InShop = -1;
    }

    public static void Packet_BuyItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var shopSlot = buffer.ReadInt32();

        // not in shop, exit out
        var shopMum = Data.TempPlayer[session.Id].InShop;

        if (shopMum < 0 | shopMum > Core.Globals.Constant.MaxShops)
            return;

        ref var withBlock = ref Data.Shop[(int) shopMum].TradeItem[shopSlot];

        // check trade exists
        if (withBlock.Item < 0)
            return;

        // check has the cost item
        var itemAmount = global::Server.Player.HasItem(session.Id, withBlock.CostItem);
        if (itemAmount == 0 | itemAmount < withBlock.CostValue)
        {
            NetworkSend.PlayerMsg(session.Id, "You do not have enough to buy this item.", (int) ColorName.BrightRed);
            NetworkSend.ResetShopAction();
            return;
        }

        // it's fine, let's go ahead
        for (int i = 0, loopTo = withBlock.CostValue; i < loopTo; i++)
            global::Server.Player.TakeInv(session.Id, withBlock.CostItem, withBlock.CostValue);
        global::Server.Player.GiveInv(session.Id, withBlock.Item, withBlock.ItemValue);

        // send confirmation message & reset their shop action
        NetworkSend.PlayerMsg(session.Id, "Trade successful.", (int) ColorName.BrightGreen);
        NetworkSend.ResetShopAction();
    }

    public static void Packet_SellItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var invSlot = buffer.ReadInt32();

        // if invalid, exit out
        if (invSlot < 0 | invSlot > Core.Globals.Constant.MaxInv)
            return;

        // has item?
        if (GetPlayerInv(session.Id, invSlot) < 0 | GetPlayerInv(session.Id, invSlot) > Core.Globals.Constant.MaxItems)
            return;

        // seems to be valid
        double itemNum = GetPlayerInv(session.Id, invSlot);
        var shopNum = Data.TempPlayer[session.Id].InShop;

        if (shopNum < 0 || shopNum > Core.Globals.Constant.MaxShops)
        {
            return;
        }

        // work out price
        var multiplier = Data.Shop[(int) shopNum].BuyRate / 100d;
        var price = (int) Math.Round(Data.Item[(int) itemNum].Price * multiplier);

        // item has cost?
        if (price < 0)
        {
            NetworkSend.PlayerMsg(session.Id, "The shop doesn't want that item.", (int) ColorName.Yellow);
            NetworkSend.ResetShopAction();
            return;
        }

        // take item and give gold
        global::Server.Player.TakeInv(session.Id, (int) itemNum, 1);
        global::Server.Player.GiveInv(session.Id, 0, price);

        // send confirmation message & reset their shop action
        NetworkSend.PlayerMsg(session.Id, "Sold the " + Data.Item[(int) itemNum].Name + " for " + price + " " + Data.Item[(int) itemNum].Name + "!", (int) ColorName.BrightGreen);
        NetworkSend.ResetShopAction();
    }

    public static void Packet_ChangeBankSlots(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var oldslot = buffer.ReadInt32();
        var newslot = buffer.ReadInt32();

        global::Server.Player.PlayerSwitchbankSlots(session.Id, oldslot, newslot);
    }

    public static void Packet_DepositItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var invslot = buffer.ReadInt32();
        var amount = buffer.ReadInt32();

        global::Server.Player.GiveBank(session.Id, invslot, amount);
    }

    public static void Packet_WithdrawItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var bankSlot = buffer.ReadByte();
        var amount = buffer.ReadInt32();

        global::Server.Player.TakeBank(session.Id, bankSlot, amount);
    }

    public static void Packet_CloseBank(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        Data.TempPlayer[session.Id].InBank = false;
    }

    public static void Packet_AdminWarp(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var x = buffer.ReadInt32();
        var y = buffer.ReadInt32();

        if (x < 0 || x >= Data.Map[GetPlayerMap(session.Id)].MaxX || y < 0 || y >= Data.Map[GetPlayerMap(session.Id)].MaxY)
            return;

        x *= 32;
        y *= 32;

        if (GetPlayerAccess(session.Id) >= (byte) AccessLevel.Mapper)
        {
            Data.Player[session.Id].IsMoving = false;

            // Set the information
            SetPlayerX(session.Id, x);
            SetPlayerY(session.Id, y);
            SetPlayerDir(session.Id, (byte) Direction.Down);
            NetworkSend.SendPlayerXyToMap(session.Id);
        }
    }

    public static void Packet_TradeInvite(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var name = buffer.ReadString();


        // Check for a player
        var tradeTarget = GameLogic.FindPlayer(name);

        if (tradeTarget < 0 | tradeTarget >= Core.Globals.Constant.MaxPlayers)
            return;

        // can't trade with yourself..
        if (tradeTarget == session.Id)
        {
            NetworkSend.PlayerMsg(session.Id, "You can't trade with yourself!", (int) ColorName.BrightRed);
            return;
        }

        // send the trade request
        Data.TempPlayer[session.Id].TradeRequest = tradeTarget;
        Data.TempPlayer[tradeTarget].TradeRequest = session.Id;

        NetworkSend.PlayerMsg(tradeTarget, GetPlayerName(session.Id) + " has invited you to trade.", (int) ColorName.Yellow);
        NetworkSend.PlayerMsg(session.Id, "You have invited " + GetPlayerName(tradeTarget) + " to trade.", (int) ColorName.BrightGreen);

        NetworkSend.SendTradeInvite(tradeTarget, session.Id);
    }

    public static void Packet_HandleTradeInvite(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var status = (byte) buffer.ReadInt32();


        var tradeTarget = Data.TempPlayer[session.Id].TradeRequest;

        if (tradeTarget < 0 | tradeTarget >= Core.Globals.Constant.MaxPlayers)
            return;

        if (status == 0)
        {
            NetworkSend.PlayerMsg(tradeTarget, GetPlayerName(session.Id) + " has declined your trade request.", (int) ColorName.BrightRed);
            NetworkSend.PlayerMsg(session.Id, "You have declined the trade with " + GetPlayerName(tradeTarget) + ".", (int) ColorName.BrightRed);
            Data.TempPlayer[session.Id].TradeRequest = -1;
            return;
        }

        // Let them tradetradeTarget
        if (Data.TempPlayer[tradeTarget].TradeRequest == session.Id)
        {
            // let them know they're trading
            NetworkSend.PlayerMsg(session.Id, "You have accepted " + GetPlayerName(tradeTarget) + "'s trade request.", (int) ColorName.Yellow);
            NetworkSend.PlayerMsg(tradeTarget, GetPlayerName(session.Id) + " has accepted your trade request.", (int) ColorName.BrightGreen);

            // clear the tradeRequest server-side
            Data.TempPlayer[session.Id].TradeRequest = -1;
            Data.TempPlayer[tradeTarget].TradeRequest = -1;

            // set that they're trading with each other
            Data.TempPlayer[session.Id].InTrade = tradeTarget;

            // clear out their trade offers
            Data.TempPlayer[tradeTarget].InTrade = session.Id;
            ;
            Array.Resize(ref Data.TempPlayer[session.Id].TradeOffer, Core.Globals.Constant.MaxInv);
            Array.Resize(ref Data.TempPlayer[tradeTarget].TradeOffer, Core.Globals.Constant.MaxInv);

            for (int i = 0, loopTo = Core.Globals.Constant.MaxInv; i < loopTo; i++)
            {
                Data.TempPlayer[session.Id].TradeOffer[i].Num = -1;
                Data.TempPlayer[session.Id].TradeOffer[i].Value = 0;
                Data.TempPlayer[tradeTarget].TradeOffer[i].Num = -1;
                Data.TempPlayer[tradeTarget].TradeOffer[i].Value = 0;
            }

            // Used to init the trade window clientside
            NetworkSend.SendTrade(session.Id, tradeTarget);
            NetworkSend.SendTrade(tradeTarget, session.Id);

            // Send the offer data - Used to clear their client
            NetworkSend.SendTradeUpdate(session.Id, 0);
            NetworkSend.SendTradeUpdate(session.Id, 1);
            NetworkSend.SendTradeUpdate(tradeTarget, 0);
            NetworkSend.SendTradeUpdate(tradeTarget, 1);
        }
    }

    public static void Packet_TradeInviteDecline(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        Data.TempPlayer[session.Id].TradeRequest = -1;
    }

    public static void Packet_AcceptTrade(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        int itemNum;
        int i;
        var tmpTradeItem = new Type.PlayerInv[Core.Globals.Constant.MaxInv];
        var tmpTradeItem2 = new Type.PlayerInv[Core.Globals.Constant.MaxInv];

        Data.TempPlayer[session.Id].AcceptTrade = true;

        var tradeTarget = (int) Data.TempPlayer[session.Id].InTrade;

        // if not both of them accept, then exit
        if (!Data.TempPlayer[tradeTarget].AcceptTrade)
        {
            NetworkSend.SendTradeStatus(session.Id, 2);
            NetworkSend.SendTradeStatus(tradeTarget, 1);
            return;
        }

        // take their items
        var loopTo = Core.Globals.Constant.MaxInv;
        for (i = 0; i < loopTo; i++)
        {
            tmpTradeItem[i].Num = -1;
            tmpTradeItem2[i].Num = -1;

            // player
            if (Data.TempPlayer[session.Id].TradeOffer[i].Num >= 0)
            {
                itemNum = (int) Data.Player[session.Id].Inv[(int) Data.TempPlayer[session.Id].TradeOffer[i].Num].Num;
                if (itemNum >= 0)
                {
                    // store temp
                    tmpTradeItem[i].Num = itemNum;
                    tmpTradeItem[i].Value = Data.TempPlayer[session.Id].TradeOffer[i].Value;
                    // take item
                    global::Server.Player.TakeInvSlot(session.Id, (int) Data.TempPlayer[session.Id].TradeOffer[i].Num, tmpTradeItem[i].Value);
                }
            }

            // target
            if (Data.TempPlayer[tradeTarget].TradeOffer[i].Num >= 0)
            {
                itemNum = GetPlayerInv(tradeTarget, (int) Data.TempPlayer[tradeTarget].TradeOffer[i].Num);
                if (itemNum >= 0)
                {
                    // store temp
                    tmpTradeItem2[i].Num = itemNum;
                    tmpTradeItem2[i].Value = Data.TempPlayer[tradeTarget].TradeOffer[i].Value;
                    // take item
                    global::Server.Player.TakeInvSlot(tradeTarget, (int) Data.TempPlayer[tradeTarget].TradeOffer[i].Num, tmpTradeItem2[i].Value);
                }
            }
        }

        // taken all items. now they can't not get items because of no inventory space.
        var loopTo1 = Core.Globals.Constant.MaxInv;
        for (i = 0; i < loopTo1; i++)
        {
            // player
            if (tmpTradeItem2[i].Num >= 0)
            {
                // give away!
                global::Server.Player.GiveInv(session.Id, (int) tmpTradeItem2[i].Num, tmpTradeItem2[i].Value, false);
            }

            // target
            if (tmpTradeItem[i].Num >= 0)
            {
                // give away!
                global::Server.Player.GiveInv(tradeTarget, (int) tmpTradeItem[i].Num, tmpTradeItem[i].Value, false);
            }
        }

        NetworkSend.SendInventory(session.Id);
        NetworkSend.SendInventory(tradeTarget);

        // they now have all the items. Clear out values + let them out of the trade.
        var loopTo2 = Core.Globals.Constant.MaxInv;
        for (i = 0; i < loopTo2; i++)
        {
            Data.TempPlayer[session.Id].TradeOffer[i].Num = -1;
            Data.TempPlayer[session.Id].TradeOffer[i].Value = 0;
            Data.TempPlayer[tradeTarget].TradeOffer[i].Num = -1;
            Data.TempPlayer[tradeTarget].TradeOffer[i].Value = 0;
        }

        Data.TempPlayer[session.Id].InTrade = -1;
        Data.TempPlayer[tradeTarget].InTrade = -1;

        NetworkSend.PlayerMsg(session.Id, "Trade completed.", (int) ColorName.BrightGreen);
        NetworkSend.PlayerMsg(tradeTarget, "Trade completed.", (int) ColorName.BrightGreen);

        NetworkSend.SendCloseTrade(session.Id);
        NetworkSend.SendCloseTrade(tradeTarget);
    }

    public static void Packet_DeclineTrade(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var tradeTarget = (int) Data.TempPlayer[session.Id].InTrade;

        for (int i = 0, loopTo = Core.Globals.Constant.MaxInv; i < loopTo; i++)
        {
            Data.TempPlayer[session.Id].TradeOffer[i].Num = -1;
            Data.TempPlayer[session.Id].TradeOffer[i].Value = 0;
            Data.TempPlayer[tradeTarget].TradeOffer[i].Num = -1;
            Data.TempPlayer[tradeTarget].TradeOffer[i].Value = 0;
        }

        Data.TempPlayer[session.Id].InTrade = -1;
        Data.TempPlayer[tradeTarget].InTrade = -1;

        NetworkSend.PlayerMsg(session.Id, "You declined the trade.", (int) ColorName.BrightRed);
        NetworkSend.PlayerMsg(tradeTarget, GetPlayerName(session.Id) + " has declined the trade.", (int) ColorName.BrightRed);

        NetworkSend.SendCloseTrade(session.Id);
        NetworkSend.SendCloseTrade(tradeTarget);
    }

    public static void Packet_TradeItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var emptyslot = default(int);
        int i;
        var buffer = new PacketReader(bytes);

        var invslot = buffer.ReadInt32();
        var amount = buffer.ReadInt32();


        if (invslot < 0 | invslot > Core.Globals.Constant.MaxInv)
            return;

        var itemnum = GetPlayerInv(session.Id, invslot);

        if (itemnum < 0 | itemnum > Core.Globals.Constant.MaxItems)
            return;

        // make sure they have the amount they offer
        if (amount < 0 | amount > GetPlayerInvValue(session.Id, invslot))
            return;

        if (Data.Item[itemnum].Type == (byte) ItemCategory.Currency | Data.Item[itemnum].Stackable == 1)
        {
            // check if already offering same currency item
            var loopTo = Core.Globals.Constant.MaxInv;
            for (i = 0; i < loopTo; i++)
            {
                if (Data.TempPlayer[session.Id].TradeOffer[i].Num == invslot)
                {
                    // add amount
                    Data.TempPlayer[session.Id].TradeOffer[i].Value = Data.TempPlayer[session.Id].TradeOffer[i].Value + amount;

                    // clamp to limits
                    if (Data.TempPlayer[session.Id].TradeOffer[i].Value > GetPlayerInvValue(session.Id, invslot))
                    {
                        Data.TempPlayer[session.Id].TradeOffer[i].Value = GetPlayerInvValue(session.Id, invslot);
                    }

                    // cancel any trade agreement
                    Data.TempPlayer[session.Id].AcceptTrade = false;
                    Data.TempPlayer[(int) Data.TempPlayer[session.Id].InTrade].AcceptTrade = false;

                    NetworkSend.SendTradeStatus(session.Id, 0);
                    NetworkSend.SendTradeStatus((int) Data.TempPlayer[session.Id].InTrade, 1);

                    NetworkSend.SendTradeUpdate(session.Id, 0);
                    NetworkSend.SendTradeUpdate(session.Id, 1);
                    NetworkSend.SendTradeUpdate((int) Data.TempPlayer[session.Id].InTrade, 0);
                    NetworkSend.SendTradeUpdate((int) Data.TempPlayer[session.Id].InTrade, 1);
                    return;
                }
            }
        }
        else
        {
            // make sure they're not already offering it
            var loopTo1 = Core.Globals.Constant.MaxInv;
            for (i = 0; i < loopTo1; i++)
            {
                if (Data.TempPlayer[session.Id].TradeOffer[i].Num == invslot)
                {
                    NetworkSend.PlayerMsg(session.Id, "You've already offered this item.", (int) ColorName.BrightRed);
                    return;
                }
            }
        }

        // not already offering - find earliest empty slot
        var loopTo2 = Core.Globals.Constant.MaxInv;
        for (i = 0; i < loopTo2; i++)
        {
            if (Data.TempPlayer[session.Id].TradeOffer[i].Num == -1)
            {
                emptyslot = i;
                break;
            }
        }

        Data.TempPlayer[session.Id].TradeOffer[emptyslot].Num = invslot;
        Data.TempPlayer[session.Id].TradeOffer[emptyslot].Value = amount;

        // cancel any trade agreement and send new data
        Data.TempPlayer[session.Id].AcceptTrade = false;
        Data.TempPlayer[(int) Data.TempPlayer[session.Id].InTrade].AcceptTrade = false;

        NetworkSend.SendTradeStatus(session.Id, 0);
        NetworkSend.SendTradeStatus((int) Data.TempPlayer[session.Id].InTrade, 0);

        NetworkSend.SendTradeUpdate(session.Id, 0);
        NetworkSend.SendTradeUpdate(session.Id, 1);
        NetworkSend.SendTradeUpdate((int) Data.TempPlayer[session.Id].InTrade, 0);
        NetworkSend.SendTradeUpdate((int) Data.TempPlayer[session.Id].InTrade, 1);
    }

    public static void Packet_UntradeItem(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var tradeslot = buffer.ReadInt32();


        if (tradeslot < 0 | tradeslot > Core.Globals.Constant.MaxInv)
            return;

        if (Data.TempPlayer[session.Id].TradeOffer[tradeslot].Num < 0)
            return;

        Data.TempPlayer[session.Id].TradeOffer[tradeslot].Num = -1;
        Data.TempPlayer[session.Id].TradeOffer[tradeslot].Value = 0;

        if (Data.TempPlayer[session.Id].AcceptTrade)
            Data.TempPlayer[session.Id].AcceptTrade = false;
        if (Data.TempPlayer[(int) Data.TempPlayer[session.Id].InTrade].AcceptTrade)
            Data.TempPlayer[(int) Data.TempPlayer[session.Id].InTrade].AcceptTrade = false;

        NetworkSend.SendTradeStatus(session.Id, 0);
        NetworkSend.SendTradeStatus((int) Data.TempPlayer[session.Id].InTrade, 0);

        NetworkSend.SendTradeUpdate(session.Id, 0);
        NetworkSend.SendTradeUpdate((int) Data.TempPlayer[session.Id].InTrade, 1);
    }

    public static void HackingAttempt(int index, string reason)
    {
        if (index > 0 & NetworkConfig.IsPlaying(index))
        {
            NetworkSend.GlobalMsg(GetAccountLogin(index) + "/" + GetPlayerName(index) + " has been booted for (" + reason + ")");
            var task = Server.Player.LeftGame(index);
            task.Wait();
        }
    }

    public static void Packet_Admin(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Moderator)
            return;

        NetworkSend.SendAdminPanel(session.Id);
    }

    public static void Packet_SetHotbarSlot(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var @type = (byte) buffer.ReadInt32();
        var newSlot = buffer.ReadInt32();
        var oldSlot = buffer.ReadInt32();
        var skill = buffer.ReadInt32();

        if (newSlot < 0 | newSlot > Core.Globals.Constant.MaxHotbar)
            return;

        if (type == (byte) PartOrigin.Hotbar)
        {
            if (oldSlot < 0 | oldSlot > Core.Globals.Constant.MaxHotbar)
                return;

            var oldItem = Data.Player[session.Id].Hotbar[oldSlot].Slot;
            var oldType = Data.Player[session.Id].Hotbar[oldSlot].SlotType;
            var newItem = Data.Player[session.Id].Hotbar[newSlot].Slot;
            var newType = Data.Player[session.Id].Hotbar[newSlot].SlotType;

            Data.Player[session.Id].Hotbar[newSlot].Slot = oldItem;
            Data.Player[session.Id].Hotbar[newSlot].SlotType = oldType;

            Data.Player[session.Id].Hotbar[oldSlot].Slot = newItem;
            Data.Player[session.Id].Hotbar[oldSlot].SlotType = newType;
        }
        else
        {
            Data.Player[session.Id].Hotbar[newSlot].Slot = skill;
            Data.Player[session.Id].Hotbar[newSlot].SlotType = type;
        }

        NetworkSend.SendHotbar(session.Id);
    }

    public static void Packet_DeleteHotbarSlot(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var slot = buffer.ReadInt32();

        if (slot < 0 | slot > Core.Globals.Constant.MaxHotbar)
            return;

        Data.Player[session.Id].Hotbar[slot].Slot = -1;
        Data.Player[session.Id].Hotbar[slot].SlotType = 0;

        NetworkSend.SendHotbar(session.Id);
    }

    public static void Packet_UseHotbarSlot(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var slot = buffer.ReadInt32();


        if (slot < 0 | slot > Core.Globals.Constant.MaxHotbar)
            return;

        if (Data.Player[session.Id].Hotbar[slot].Slot >= 0)
        {
            if (Data.Player[session.Id].Hotbar[slot].SlotType == (byte) DraggablePartType.Item)
            {
                global::Server.Player.UseItem(session.Id, global::Server.Player.FindItemSlot(session.Id, (int) Data.Player[session.Id].Hotbar[slot].Slot));
            }
        }

        NetworkSend.SendHotbar(session.Id);
    }

    public static void Packet_SkillLearn(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
            return;

        var skillNum = buffer.ReadInt32();

        try
        {
            Script.Instance?.LearnSkill(session.Id, -1, skillNum);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void Packet_RequestEditJob(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
            return;

        var user = IsEditorLocked(session.Id, EditorType.Job);

        if (!string.IsNullOrEmpty(user))
        {
            NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) ColorName.BrightRed);
            return;
        }

        Item.SendItems(session.Id);
        NetworkSend.SendJobs(session);

        Data.TempPlayer[session.Id].Editor = EditorType.Job;

        NetworkSend.SendJobs(session);

        NetworkSend.SendJobEditor(session.Id);
    }

    public static void Packet_SaveJob(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        int i;
        int z;
        int x;
        var buffer = new PacketReader(bytes);

        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
            return;

        var jobNum = buffer.ReadInt32();

        {
            ref var withBlock = ref Data.Job[jobNum];
            withBlock.Name = buffer.ReadString();
            withBlock.Desc = buffer.ReadString();

            withBlock.MaleSprite = buffer.ReadInt32();
            withBlock.FemaleSprite = buffer.ReadInt32();

            var loopTo = Enum.GetNames(typeof(Stat)).Length;
            for (x = 0; x < loopTo; x++)
                withBlock.Stat[x] = buffer.ReadInt32();

            for (var q = 0; q < Core.Globals.Constant.MaxStartItems; q++)
            {
                withBlock.StartItem[q] = buffer.ReadInt32();
                withBlock.StartValue[q] = buffer.ReadInt32();
            }

            withBlock.StartMap = buffer.ReadInt32();
            withBlock.StartX = buffer.ReadByte();
            withBlock.StartY = (byte) buffer.ReadInt32();
            withBlock.BaseExp = buffer.ReadInt32();
        }


        Database.SaveJob(jobNum);
        NetworkSend.SendJobToAll(session.Id);
    }

    private static void Packet_Emote(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var buffer = new PacketReader(bytes);

        var emote = buffer.ReadInt32();

        NetworkSend.SendEmote(session.Id, emote);
    }

    private static void Packet_CloseEditor(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        // Prevent hacking
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Mapper)
            return;

        if (Data.TempPlayer[session.Id].Editor == EditorType.None)
            return;

        Data.TempPlayer[session.Id].Editor = EditorType.None;
    }
}