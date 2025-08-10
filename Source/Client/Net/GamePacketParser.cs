using Core;
using Core.Net;
using static Core.Global.Command;
using Color = Core.Color;

namespace Client.Net;

public sealed class GamePacketParser : PacketParser<Packets.ServerPackets>
{
    // Cache enum sizes to avoid reflection on every update
    private static readonly int EquipmentCount = Enum.GetValues<Equipment>().Length;
    private static readonly int StatCount = Enum.GetValues<Stat>().Length;
    private static readonly int VitalCount = Enum.GetValues<Vital>().Length;

    public GamePacketParser()
    {
        Bind(Packets.ServerPackets.SAes, Packet_Aes);
        Bind(Packets.ServerPackets.SAlertMsg, Packet_AlertMsg);
        Bind(Packets.ServerPackets.SLoginOk, Packet_LoginOk);
        Bind(Packets.ServerPackets.SPlayerChars, Packet_PlayerChars);
        Bind(Packets.ServerPackets.SUpdateJob, Packet_UpdateJob);
        Bind(Packets.ServerPackets.SJobData, Packet_JobData);
        Bind(Packets.ServerPackets.SInGame, Packet_InGame);
        Bind(Packets.ServerPackets.SPlayerInv, Packet_PlayerInv);
        Bind(Packets.ServerPackets.SPlayerInvUpdate, Packet_PlayerInvUpdate);
        Bind(Packets.ServerPackets.SPlayerWornEq, Packet_PlayerWornEquipment);
        Bind(Packets.ServerPackets.SPlayerHp, Player.Packet_PlayerHP);
        Bind(Packets.ServerPackets.SPlayerMp, Player.Packet_PlayerMP);
        Bind(Packets.ServerPackets.SPlayerSp, Player.Packet_PlayerSP);
        Bind(Packets.ServerPackets.SPlayerStats, Player.Packet_PlayerStats);
        Bind(Packets.ServerPackets.SPlayerData, Player.Packet_PlayerData);
        Bind(Packets.ServerPackets.SNpcMove, Packet_NpcMove);
        Bind(Packets.ServerPackets.SPlayerDir, Player.Packet_PlayerDir);
        Bind(Packets.ServerPackets.SNpcDir, Packet_NpcDir);
        Bind(Packets.ServerPackets.SPlayerXy, Player.Packet_PlayerXY);
        Bind(Packets.ServerPackets.SAttack, Packet_Attack);
        Bind(Packets.ServerPackets.SNpcAttack, Packet_NpcAttack);
        Bind(Packets.ServerPackets.SCheckForMap, Map.Packet_CheckMap);
        Bind(Packets.ServerPackets.SMapData, Map.MapData);
        Bind(Packets.ServerPackets.SMapNpcData, Map.Packet_MapNpcData);
        Bind(Packets.ServerPackets.SMapNpcUpdate, Map.Packet_MapNpcUpdate);
        Bind(Packets.ServerPackets.SGlobalMsg, Packet_GlobalMsg);
        Bind(Packets.ServerPackets.SAdminMsg, Packet_AdminMsg);
        Bind(Packets.ServerPackets.SPlayerMsg, Packet_PlayerMsg);
        Bind(Packets.ServerPackets.SMapMsg, Packet_MapMsg);
        Bind(Packets.ServerPackets.SSpawnItem, Packet_SpawnItem);
        Bind(Packets.ServerPackets.SUpdateItem, Item.Packet_UpdateItem);
        Bind(Packets.ServerPackets.SSpawnNpc, Packet_SpawnNpc);
        Bind(Packets.ServerPackets.SNpcDead, Packet_NpcDead);
        Bind(Packets.ServerPackets.SUpdateNpc, Packet_UpdateNpc);
        Bind(Packets.ServerPackets.SEditMap, Map.Packet_EditMap);
        Bind(Packets.ServerPackets.SUpdateShop, Shop.Packet_UpdateShop);
        Bind(Packets.ServerPackets.SUpdateSkill, Packet_UpdateSkill);
        Bind(Packets.ServerPackets.SSkills, Packet_Skills);
        Bind(Packets.ServerPackets.SLeftMap, Packet_LeftMap);
        Bind(Packets.ServerPackets.SMapResource, MapResource.Packet_MapResource);
        Bind(Packets.ServerPackets.SUpdateResource, MapResource.Packet_UpdateResource);
        Bind(Packets.ServerPackets.SSendPing, Packet_Ping);
        Bind(Packets.ServerPackets.SActionMsg, Packet_ActionMessage);
        Bind(Packets.ServerPackets.SPlayerExp, Player.Packet_PlayerExp);
        Bind(Packets.ServerPackets.SBlood, Packet_Blood);
        Bind(Packets.ServerPackets.SUpdateAnimation, Animation.Packet_UpdateAnimation);
        Bind(Packets.ServerPackets.SAnimation, Animation.Packet_Animation);
        Bind(Packets.ServerPackets.SMapNpcVitals, Packet_NpcVitals);
        Bind(Packets.ServerPackets.SCooldown, Packet_Cooldown);
        Bind(Packets.ServerPackets.SClearSkillBuffer, Packet_ClearSkillBuffer);
        Bind(Packets.ServerPackets.SSayMsg, Packet_SayMessage);
        Bind(Packets.ServerPackets.SOpenShop, Shop.Packet_OpenShop);
        Bind(Packets.ServerPackets.SResetShopAction, Shop.Packet_ResetShopAction);
        Bind(Packets.ServerPackets.SStunned, Packet_Stunned);
        Bind(Packets.ServerPackets.SMapWornEq, Packet_MapWornEquipment);
        Bind(Packets.ServerPackets.SBank, Bank.Packet_OpenBank);
        Bind(Packets.ServerPackets.SLeftGame, Packet_LeftGame);
        Bind(Packets.ServerPackets.STradeInvite, Trade.Packet_TradeInvite);
        Bind(Packets.ServerPackets.STrade, Trade.Packet_Trade);
        Bind(Packets.ServerPackets.SCloseTrade, Trade.Packet_CloseTrade);
        Bind(Packets.ServerPackets.STradeUpdate, Trade.Packet_TradeUpdate);
        Bind(Packets.ServerPackets.STradeStatus, Trade.Packet_TradeStatus);
        Bind(Packets.ServerPackets.SMapReport, Packet_MapReport);
        Bind(Packets.ServerPackets.STarget, Packet_Target);
        Bind(Packets.ServerPackets.SAdmin, Packet_Admin);
        Bind(Packets.ServerPackets.SCritical, Packet_Critical);
        Bind(Packets.ServerPackets.SrClick, Packet_RClick);
        Bind(Packets.ServerPackets.SHotbar, Packet_Hotbar);
        Bind(Packets.ServerPackets.SSpawnEvent, Event.Packet_SpawnEvent);
        Bind(Packets.ServerPackets.SEventMove, Event.Packet_EventMove);
        Bind(Packets.ServerPackets.SEventDir, Event.Packet_EventDir);
        Bind(Packets.ServerPackets.SEventChat, Event.Packet_EventChat);
        Bind(Packets.ServerPackets.SEventStart, Event.Packet_EventStart);
        Bind(Packets.ServerPackets.SEventEnd, Event.Packet_EventEnd);
        Bind(Packets.ServerPackets.SPlayBgm, Event.Packet_PlayBGM);
        Bind(Packets.ServerPackets.SPlaySound, Event.Packet_PlaySound);
        Bind(Packets.ServerPackets.SFadeoutBgm, Event.Packet_FadeOutBGM);
        Bind(Packets.ServerPackets.SStopSound, Event.Packet_StopSound);
        Bind(Packets.ServerPackets.SSwitchesAndVariables, Event.Packet_SwitchesAndVariables);
        Bind(Packets.ServerPackets.SMapEventData, Event.Packet_MapEventData);
        Bind(Packets.ServerPackets.SChatBubble, Packet_ChatBubble);
        Bind(Packets.ServerPackets.SSpecialEffect, Event.Packet_SpecialEffect);
        Bind(Packets.ServerPackets.SPic, Event.Packet_Picture);
        Bind(Packets.ServerPackets.SHoldPlayer, Event.Packet_HoldPlayer);
        Bind(Packets.ServerPackets.SUpdateProjectile, Projectile.HandleUpdateProjectile);
        Bind(Packets.ServerPackets.SMapProjectile, Projectile.HandleMapProjectile);
        Bind(Packets.ServerPackets.SEmote, Packet_Emote);
        Bind(Packets.ServerPackets.SPartyInvite, Party.Packet_PartyInvite);
        Bind(Packets.ServerPackets.SPartyUpdate, Party.Packet_PartyUpdate);
        Bind(Packets.ServerPackets.SPartyVitals, Party.Packet_PartyVitals);
        Bind(Packets.ServerPackets.SClock, Packet_Clock);
        Bind(Packets.ServerPackets.STime, Packet_Time);
        Bind(Packets.ServerPackets.SScriptEditor, Script.Packet_EditScript);
        Bind(Packets.ServerPackets.SItemEditor, Packet_EditItem);
        Bind(Packets.ServerPackets.SNpcEditor, Packet_NpcEditor);
        Bind(Packets.ServerPackets.SShopEditor, Packet_EditShop);
        Bind(Packets.ServerPackets.SSkillEditor, Packet_EditSkill);
        Bind(Packets.ServerPackets.SResourceEditor, Packet_ResourceEditor);
        Bind(Packets.ServerPackets.SAnimationEditor, Packet_AnimationEditor);
        Bind(Packets.ServerPackets.SProjectileEditor, HandleProjectileEditor);
        Bind(Packets.ServerPackets.SJobEditor, Packet_JobEditor);
        Bind(Packets.ServerPackets.SUpdateMoral, Packet_UpdateMoral);
        Bind(Packets.ServerPackets.SMoralEditor, Packet_EditMoral);
    }

    private static void Packet_Aes(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var keyLength = packetReader.ReadByte();
        var key = packetReader.ReadBlock(keyLength).ToArray();

        var ivLength = packetReader.ReadByte();
        var iv = packetReader.ReadBlock(ivLength).ToArray();

        General.AesKey = key;
        General.AesIV = iv;
    }

    private static void Packet_AlertMsg(ReadOnlyMemory<byte> data)
    {
        var buffer = new PacketReader(data);

        var dialogueIndex = buffer.ReadByte();
        var menuReset = buffer.ReadInt32();
        var kick = buffer.ReadInt32();

        if (menuReset > 0)
        {
            Gui.HideWindows();

            switch ((Menu) menuReset)
            {
                case Menu.Login:
                    Gui.ShowWindow(Gui.GetWindowIndex("winLogin"));
                    break;

                case Menu.CharacterSelect:
                    Gui.ShowWindow(Gui.GetWindowIndex("winChars"));
                    break;

                case Menu.JobSelection:
                    Gui.ShowWindow(Gui.GetWindowIndex("winJobs"));
                    break;

                case Menu.NewCharacter:
                    Gui.ShowWindow(Gui.GetWindowIndex("winNewChar"));
                    break;

                case Menu.MainMenu:
                    Gui.ShowWindow(Gui.GetWindowIndex("winLogin"));
                    break;

                case Menu.Register:
                    Gui.ShowWindow(Gui.GetWindowIndex("winRegister"));
                    break;
            }
        }
        else if (kick > 0 || GameState.InGame)
        {
            Gui.ShowWindow(Gui.GetWindowIndex("winLogin"));
        }

        GameLogic.DialogueAlert(dialogueIndex);
    }

    private static void Packet_LoginOk(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        GameState.MyIndex = packetReader.ReadInt32();
    }

    public static void Packet_PlayerChars(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var isSlotEmpty = new bool[Constant.MaxChars];

        SettingsManager.Instance.Username = Gui.Windows[Gui.GetWindowIndex("winLogin")].Controls[Gui.GetControlIndex("winLogin", "txtUsername")].Text;
        SettingsManager.Save();

        for (var i = 0; i < Constant.MaxChars; i++)
        {
            GameState.CharName[i] = packetReader.ReadString();
            GameState.CharSprite[i] = packetReader.ReadInt32();
            GameState.CharAccess[i] = packetReader.ReadInt32();
            GameState.CharJob[i] = packetReader.ReadInt32();

            // set as empty or not
            if (GameState.CharName[i].Length == 0)
            {
                isSlotEmpty[i] = true;
            }
        }


        Gui.HideWindows();
        Gui.ShowWindow(Gui.GetWindowIndex("winChars"));

        long winNum = Gui.GetWindowIndex("winChars");
        for (var i = 0L; i < Constant.MaxChars; i++)
        {
            long conNum = Gui.GetControlIndex("winChars", "lblCharName_" + (i + 1));
            {
                var control = Gui.Windows[winNum].Controls[(int) conNum];

                control.Text = !isSlotEmpty[(int) i] ? GameState.CharName[(int) i] : "Blank Slot";
            }

            if (isSlotEmpty[(int) i])
            {
                // create button
                conNum = Gui.GetControlIndex("winChars", "btnCreateChar_" + (i + 1));
                Gui.Windows[winNum].Controls[(int) conNum].Visible = true;

                // select button
                conNum = Gui.GetControlIndex("winChars", "btnSelectChar_" + (i + 1));
                Gui.Windows[winNum].Controls[(int) conNum].Visible = false;

                // delete button
                conNum = Gui.GetControlIndex("winChars", "btnDelChar_" + (i + 1));
                Gui.Windows[winNum].Controls[(int) conNum].Visible = false;
            }
            else
            {
                // create button
                conNum = Gui.GetControlIndex("winChars", "btnCreateChar_" + (i + 1));
                Gui.Windows[winNum].Controls[(int) conNum].Visible = false;

                // select button
                conNum = Gui.GetControlIndex("winChars", "btnSelectChar_" + (i + 1));
                Gui.Windows[winNum].Controls[(int) conNum].Visible = true;

                // delete button
                conNum = Gui.GetControlIndex("winChars", "btnDelChar_" + (i + 1));
                Gui.Windows[winNum].Controls[(int) conNum].Visible = true;
            }
        }
    }

    public static void Packet_UpdateJob(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var jobNum = packetReader.ReadInt32();

        ref var job = ref Data.Job[jobNum];

        job.Name = packetReader.ReadString();
        job.Desc = packetReader.ReadString();
        job.MaleSprite = packetReader.ReadInt32();
        job.FemaleSprite = packetReader.ReadInt32();

        for (var i = 0; i < StatCount; i++)
        {
            job.Stat[i] = packetReader.ReadInt32();
        }

        for (var i = 0; i < Constant.MaxStartItems; i++)
        {
            job.StartItem[i] = packetReader.ReadInt32();
            job.StartValue[i] = packetReader.ReadInt32();
        }

        job.StartMap = packetReader.ReadInt32();
        job.StartX = packetReader.ReadByte();
        job.StartY = packetReader.ReadByte();
        job.BaseExp = packetReader.ReadInt32();
    }

    public static void Packet_JobData(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        for (var jobNum = 0; jobNum < Constant.MaxJobs; jobNum++)
        {
            ref var job = ref Data.Job[jobNum];

            job.Name = packetReader.ReadString();
            job.Desc = packetReader.ReadString();
            job.MaleSprite = packetReader.ReadInt32();
            job.FemaleSprite = packetReader.ReadInt32();

            for (var i = 0; i < StatCount; i++)
            {
                job.Stat[i] = packetReader.ReadInt32();
            }

            for (var i = 0; i < Constant.MaxStartItems; i++)
            {
                job.StartItem[i] = packetReader.ReadInt32();
                job.StartValue[i] = packetReader.ReadInt32();
            }

            job.StartMap = packetReader.ReadInt32();
            job.StartX = packetReader.ReadByte();
            job.StartY = packetReader.ReadByte();
            job.BaseExp = packetReader.ReadInt32();
        }
    }

    private static void Packet_InGame(ReadOnlyMemory<byte> data)
    {
        GameState.InMenu = false;
        GameState.InGame = true;

        Gui.HideWindows();

        GameState.CanMoveNow = true;
        GameState.MyEditorType = EditorType.None;
        GameState.SkillBuffer = -1;
        GameState.InShop = -1;

        Gui.ShowWindow(Gui.GetWindowIndex("winHotbar"), resetPosition: false);
        Gui.ShowWindow(Gui.GetWindowIndex("winMenu"), resetPosition: false);
        Gui.ShowWindow(Gui.GetWindowIndex("winBars"), resetPosition: false);
        Gui.HideChat();

        General.GameInit();
    }

    private static void Packet_PlayerInv(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        for (var i = 0; i < Constant.MaxInv; i++)
        {
            var itemNum = packetReader.ReadInt32();
            var amount = packetReader.ReadInt32();

            SetPlayerInv(GameState.MyIndex, i, itemNum);
            SetPlayerInvValue(GameState.MyIndex, i, amount);
        }

        GameLogic.SetGoldLabel();
    }

    private static void Packet_PlayerInvUpdate(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var invSlot = packetReader.ReadInt32();

        SetPlayerInv(GameState.MyIndex, invSlot, packetReader.ReadInt32());
        SetPlayerInvValue(GameState.MyIndex, invSlot, packetReader.ReadInt32());

        GameLogic.SetGoldLabel();
    }

    private static void Packet_PlayerWornEquipment(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        for (var i = 0; i < EquipmentCount; i++)
        {
            var itemNum = packetReader.ReadInt32();

            SetPlayerEquipment(GameState.MyIndex, itemNum, (Equipment) i);
        }
    }

    private static void Packet_NpcMove(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var mapNpcNum = packetReader.ReadInt32();
        var x = packetReader.ReadInt32();
        var y = packetReader.ReadInt32();
        var dir = packetReader.ReadByte();
        var movement = packetReader.ReadInt32();

        ref var mapNpc = ref Data.MyMapNpc[mapNpcNum];

        mapNpc.X = x;
        mapNpc.Y = y;
        mapNpc.Dir = dir;
        mapNpc.Moving = (byte) movement;
    }

    private static void Packet_NpcDir(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var mapNpcNum = packetReader.ReadInt32();
        var dir = packetReader.ReadByte();

        ref var mapNpc = ref Data.MyMapNpc[mapNpcNum];

        mapNpc.Dir = dir;
        mapNpc.Moving = 0;
    }

    private static void Packet_Attack(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var playerIndex = packetReader.ReadInt32();

        Data.Player[playerIndex].Attacking = 1;
        Data.Player[playerIndex].AttackTimer = General.GetTickCount();
    }

    private static void Packet_NpcAttack(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var mapNpcNum = packetReader.ReadInt32();

        Data.MyMapNpc[mapNpcNum].Attacking = 1;
        Data.MyMapNpc[mapNpcNum].AttackTimer = General.GetTickCount();
    }

    private static void Packet_GlobalMsg(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var message = packetReader.ReadString();

        Text.AddText(message, (int) Color.Yellow, channel: (byte) ChatChannel.Broadcast);
    }

    private static void Packet_MapMsg(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var message = packetReader.ReadString();

        Text.AddText(message, (int) Color.White, channel: (byte) ChatChannel.Map);
    }

    private static void Packet_AdminMsg(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var message = packetReader.ReadString();

        Text.AddText(message, (int) Color.BrightCyan, channel: (byte) ChatChannel.Broadcast);
    }

    private static void Packet_PlayerMsg(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var message = packetReader.ReadString();
        var color = packetReader.ReadInt32();

        Text.AddText(message, color, channel: (byte) ChatChannel.Private);
    }

    private static void Packet_SpawnItem(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var mapItemNum = packetReader.ReadInt32();

        ref var mapItem = ref Data.MyMapItem[mapItemNum];

        mapItem.Num = packetReader.ReadInt32();
        mapItem.Value = packetReader.ReadInt32();
        mapItem.X = packetReader.ReadInt32();
        mapItem.Y = packetReader.ReadInt32();
    }

    private static void Packet_SpawnNpc(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var mapNpcNum = packetReader.ReadInt32();

        ref var mapNpc = ref Data.MyMapNpc[mapNpcNum];

        mapNpc.Num = packetReader.ReadInt32();
        mapNpc.X = packetReader.ReadInt32();
        mapNpc.Y = packetReader.ReadInt32();
        mapNpc.Dir = packetReader.ReadByte();

        for (mapNpcNum = 0; mapNpcNum < VitalCount; mapNpcNum++)
        {
            mapNpc.Vital[mapNpcNum] = packetReader.ReadInt32();
        }

        mapNpc.Moving = 0;
    }

    private static void Packet_NpcDead(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var mapNpcNum = packetReader.ReadInt32();

        Map.ClearMapNpc(mapNpcNum);
    }

    private static void Packet_UpdateNpc(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var npcNum = packetReader.ReadInt32();

        Data.Npc[npcNum].Animation = packetReader.ReadInt32();
        Data.Npc[npcNum].AttackSay = packetReader.ReadString();
        Data.Npc[npcNum].Behaviour = packetReader.ReadByte();

        for (var i = 0; i < Constant.MaxDropItems; i++)
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

        for (var i = 0; i < StatCount; i++)
        {
            Data.Npc[npcNum].Stat[i] = packetReader.ReadByte();
        }

        for (var i = 0; i < Constant.MaxNpcSkills; i++)
        {
            Data.Npc[npcNum].Skill[i] = packetReader.ReadByte();
        }

        Data.Npc[npcNum].Level = packetReader.ReadByte();
        Data.Npc[npcNum].Damage = packetReader.ReadInt32();
    }

    private static void Packet_UpdateSkill(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var skillNum = packetReader.ReadInt32();

        Data.Skill[skillNum].AccessReq = packetReader.ReadInt32();
        Data.Skill[skillNum].AoE = packetReader.ReadInt32();
        Data.Skill[skillNum].CastAnim = packetReader.ReadInt32();
        Data.Skill[skillNum].CastTime = packetReader.ReadInt32();
        Data.Skill[skillNum].CdTime = packetReader.ReadInt32();
        Data.Skill[skillNum].JobReq = packetReader.ReadInt32();
        Data.Skill[skillNum].Dir = (byte) packetReader.ReadInt32();
        Data.Skill[skillNum].Duration = packetReader.ReadInt32();
        Data.Skill[skillNum].Icon = packetReader.ReadInt32();
        Data.Skill[skillNum].Interval = packetReader.ReadInt32();
        Data.Skill[skillNum].IsAoE = packetReader.ReadInt32() != 0;
        Data.Skill[skillNum].LevelReq = packetReader.ReadInt32();
        Data.Skill[skillNum].Map = packetReader.ReadInt32();
        Data.Skill[skillNum].MpCost = packetReader.ReadInt32();
        Data.Skill[skillNum].Name = packetReader.ReadString();
        Data.Skill[skillNum].Range = packetReader.ReadInt32();
        Data.Skill[skillNum].SkillAnim = packetReader.ReadInt32();
        Data.Skill[skillNum].StunDuration = packetReader.ReadInt32();
        Data.Skill[skillNum].Type = (byte) packetReader.ReadInt32();
        Data.Skill[skillNum].Vital = packetReader.ReadInt32();
        Data.Skill[skillNum].X = packetReader.ReadInt32();
        Data.Skill[skillNum].Y = packetReader.ReadInt32();
        Data.Skill[skillNum].IsProjectile = packetReader.ReadInt32();
        Data.Skill[skillNum].Projectile = packetReader.ReadInt32();
        Data.Skill[skillNum].KnockBack = (byte) packetReader.ReadInt32();
        Data.Skill[skillNum].KnockBackTiles = (byte) packetReader.ReadInt32();
    }

    private static void Packet_Skills(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        for (var i = 0; i < Constant.MaxPlayerSkills; i++)
        {
            Data.Player[GameState.MyIndex].Skill[i].Num = packetReader.ReadInt32();
        }
    }

    private static void Packet_LeftMap(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        Player.ClearPlayer(packetReader.ReadInt32());
    }

    private static void Packet_Ping(ReadOnlyMemory<byte> data)
    {
        GameState.PingEnd = General.GetTickCount();
        GameState.Ping = GameState.PingEnd - GameState.PingStart;
    }

    private static void Packet_ActionMessage(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var message = packetReader.ReadString();
        var color = packetReader.ReadInt32();
        var tmpType = packetReader.ReadInt32();
        var x = packetReader.ReadInt32();
        var y = packetReader.ReadInt32();


        GameLogic.CreateActionMsg(message, color, (byte) tmpType, x, y);
    }

    private static void Packet_Blood(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var x = packetReader.ReadInt32();
        var y = packetReader.ReadInt32();

        var sprite = GameLogic.Rand(1, 3);

        GameState.BloodIndex = (byte) (GameState.BloodIndex + 1);
        if (GameState.BloodIndex >= byte.MaxValue)
        {
            GameState.BloodIndex = 1;
        }

        ref var blood = ref Data.Blood[GameState.BloodIndex];

        blood.X = x;
        blood.Y = y;
        blood.Sprite = sprite;
        blood.Timer = General.GetTickCount();
    }

    private static void Packet_NpcVitals(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var mapNpcNum = packetReader.ReadInt32();
        for (var i = 0; i < VitalCount; i++)
        {
            Data.MyMapNpc[mapNpcNum].Vital[i] = packetReader.ReadInt32();
        }
    }

    private static void Packet_Cooldown(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var slot = packetReader.ReadInt32();

        Data.Player[GameState.MyIndex].Skill[slot].Cd = General.GetTickCount();
    }

    private static void Packet_ClearSkillBuffer(ReadOnlyMemory<byte> data)
    {
        GameState.SkillBuffer = -1;
        GameState.SkillBufferTimer = 0;
    }

    private static void Packet_SayMessage(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var name = packetReader.ReadString();
        var access = (AccessLevel) packetReader.ReadInt32();
        var pk = packetReader.ReadBoolean();
        var message = packetReader.ReadString();
        var header = packetReader.ReadString();

        // Check access level
        var color = access switch
        {
            AccessLevel.Player => (byte) Color.White,
            AccessLevel.Moderator => (byte) Color.Cyan,
            AccessLevel.Mapper => (byte) Color.Green,
            AccessLevel.Developer => (byte) Color.BrightBlue,
            AccessLevel.Owner => (byte) Color.Yellow,
            _ => (byte) Color.White
        };

        if (pk)
        {
            color = (byte) Color.BrightRed;
        }

        var channelType = header switch
        {
            "[Map]:" => (byte) ChatChannel.Map,
            "[Global]:" => (byte) ChatChannel.Broadcast,
            _ => (byte) 0
        };

        // add to the chat box
        Text.AddText(header + " " + name + ": " + message, color, channel: channelType);
    }

    private static void Packet_Stunned(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        GameState.StunDuration = packetReader.ReadInt32();
    }

    private static void Packet_MapWornEquipment(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var playerNum = packetReader.ReadInt32();

        for (var i = 0; i < EquipmentCount; i++)
        {
            var itemNum = packetReader.ReadInt32();

            SetPlayerEquipment(playerNum, itemNum, (Equipment) i);
        }
    }

    private static void Packet_Target(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        GameState.MyTarget = packetReader.ReadInt32();
        GameState.MyTargetType = packetReader.ReadInt32();
    }

    private static void Packet_MapReport(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        for (var i = 0; i < Constant.MaxMaps; i++)
        {
            GameState.MapNames[i] = packetReader.ReadString();
        }

        GameState.InitMapReport = true;
    }

    private static void Packet_Admin(ReadOnlyMemory<byte> data)
    {
        GameState.InitAdminForm = true;
    }

    private static void Packet_Critical(ReadOnlyMemory<byte> data)
    {
        GameState.ShakeTimerEnabled = true;
        GameState.ShakeTimer = General.GetTickCount();
    }

    private static void Packet_RClick(ReadOnlyMemory<byte> data)
    {
    }

    private static void Packet_Emote(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var playerIndex = packetReader.ReadInt32();

        ref var player = ref Data.Player[playerIndex];

        player.Emote = packetReader.ReadInt32();
        player.EmoteTimer = General.GetTickCount() + 5000;
    }

    private static void Packet_ChatBubble(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        GameLogic.AddChatBubble(
            target: packetReader.ReadInt32(),
            targetType: (byte) packetReader.ReadInt32(),
            msg: packetReader.ReadString(),
            color: packetReader.ReadInt32());
    }

    private static void Packet_LeftGame(ReadOnlyMemory<byte> data)
    {
        GameLogic.LogoutGame();
    }

    // *****************
    // ***  EDITORS  ***
    // *****************
    private static void Packet_AnimationEditor(ReadOnlyMemory<byte> data)
    {
        GameState.InitAnimationEditor = true;
    }

    private static void Packet_JobEditor(ReadOnlyMemory<byte> data)
    {
        GameState.InitJobEditor = true;
    }

    public static void Packet_EditItem(ReadOnlyMemory<byte> data)
    {
        GameState.InitItemEditor = true;
    }

    private static void Packet_NpcEditor(ReadOnlyMemory<byte> data)
    {
        GameState.InitNpcEditor = true;
    }

    private static void Packet_ResourceEditor(ReadOnlyMemory<byte> data)
    {
        GameState.InitResourceEditor = true;
    }

    public static void HandleProjectileEditor(ReadOnlyMemory<byte> data)
    {
        GameState.InitProjectileEditor = true;
    }

    private static void Packet_EditShop(ReadOnlyMemory<byte> data)
    {
        GameState.InitShopEditor = true;
    }

    private static void Packet_EditSkill(ReadOnlyMemory<byte> data)
    {
        GameState.InitSkillEditor = true;
    }

    private static void Packet_Clock(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        Clock.Instance.GameSpeed = packetReader.ReadInt32();
        Clock.Instance.Time = new DateTime(BitConverter.ToInt64(packetReader.ReadBytes().ToArray(), 0));
    }

    private static void Packet_Time(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        Clock.Instance.TimeOfDay = (TimeOfDay) packetReader.ReadByte();

        switch (Clock.Instance.TimeOfDay)
        {
            case TimeOfDay.Dawn:
                Text.AddText("A chilling, refreshing, breeze has come with the morning.", (int) Color.DarkGray);
                break;

            case TimeOfDay.Day:
                Text.AddText("Day has dawned in this region.", (int) Color.DarkGray);
                break;

            case TimeOfDay.Dusk:
                Text.AddText("Dusk has begun darkening the skies...", (int) Color.DarkGray);
                break;

            default:
                Text.AddText("Night has fallen upon the weary travelers.", (int) Color.DarkGray);
                break;
        }
    }

    public static void Packet_Hotbar(ReadOnlyMemory<byte> data)
    {
        var buffer = new PacketReader(data);

        for (var i = 0; i < Constant.MaxHotbar; i++)
        {
            Data.Player[GameState.MyIndex].Hotbar[i].Slot = buffer.ReadInt32();
            Data.Player[GameState.MyIndex].Hotbar[i].SlotType = (byte) buffer.ReadInt32();
        }
    }

    public static void Packet_EditMoral(ReadOnlyMemory<byte> data)
    {
        GameState.InitMoralEditor = true;
    }

    public static void Packet_UpdateMoral(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var moralNum = packetReader.ReadInt32();

        ref var moral = ref Data.Moral[moralNum];

        moral.Name = packetReader.ReadString();
        moral.Color = packetReader.ReadByte();
        moral.NpcBlock = packetReader.ReadBoolean();
        moral.PlayerBlock = packetReader.ReadBoolean();
        moral.DropItems = packetReader.ReadBoolean();
        moral.CanCast = packetReader.ReadBoolean();
        moral.CanDropItem = packetReader.ReadBoolean();
        moral.CanPickupItem = packetReader.ReadBoolean();
        moral.CanPk = packetReader.ReadBoolean();
        moral.DropItems = packetReader.ReadBoolean();
        moral.LoseExp = packetReader.ReadBoolean();
    }
}