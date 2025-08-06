using System;
using Client.Net;
using Core;
using Core;
using static Core.Global.Command;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Mirage.Sharp.Asfw;
using Mirage.Sharp.Asfw.IO;

namespace Client;

public sealed class GamePacketParser : PacketParser<Packets.ServerPackets>
{
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
        var buffer = new PacketReader(data);

        // Read key length
        byte keyLength = buffer.ReadByte();

        // Read key
        byte[] key = buffer.ReadBlock(keyLength).ToArray();

        byte ivLength = buffer.ReadByte();

        // Read IV
        byte[] iv = buffer.ReadBlock(ivLength).ToArray();

        General.AesKey = key;
        General.AesIV = iv;
    }

    private static void Packet_AlertMsg(ReadOnlyMemory<byte> data)
    {
        byte dialogueIndex;
        int menuReset;
        int kick;
        var buffer = new PacketReader(data);

        dialogueIndex = buffer.ReadByte();
        menuReset = buffer.ReadInt32();
        kick = buffer.ReadInt32();

        if (menuReset > 0)
        {
            Gui.HideWindows();

            switch (menuReset)
            {
                case (int) Core.Menu.Login:
                {
                    Gui.ShowWindow(Gui.GetWindowIndex("winLogin"));
                    break;
                }
                case (int) Core.Menu.CharacterSelect:
                {
                    Gui.ShowWindow(Gui.GetWindowIndex("winChars"));
                    break;
                }
                case (int) Core.Menu.JobSelection:
                {
                    Gui.ShowWindow(Gui.GetWindowIndex("winJobs"));
                    break;
                }
                case (int) Core.Menu.NewCharacter:
                {
                    Gui.ShowWindow(Gui.GetWindowIndex("winNewChar"));
                    break;
                }
                case (int) Core.Menu.MainMenu:
                {
                    Gui.ShowWindow(Gui.GetWindowIndex("winLogin"));
                    break;
                }
                case (int) Core.Menu.Register:
                {
                    Gui.ShowWindow(Gui.GetWindowIndex("winRegister"));
                    break;
                }
            }
        }
        else if (kick > 0 | GameState.InGame == true)
        {
            Gui.ShowWindow(Gui.GetWindowIndex("winLogin"));
        }

        GameLogic.DialogueAlert(dialogueIndex);
    }

    private static void Packet_LoginOk(ReadOnlyMemory<byte> data)
    {
        var buffer = new PacketReader(data);

        // Now we can receive game data
        GameState.MyIndex = buffer.ReadInt32();
    }

    public static void Packet_PlayerChars(ReadOnlyMemory<byte> data)
    {
        var buffer = new PacketReader(data);
        long I;
        long winNum;
        long conNum;
        var isSlotEmpty = new bool[Constant.MaxChars];
        long x;

        SettingsManager.Instance.Username = Gui.Windows[Gui.GetWindowIndex("winLogin")].Controls[(int) Gui.GetControlIndex("winLogin", "txtUsername")].Text;
        SettingsManager.Save();

        for (var i = 0L; i < Constant.MaxChars; i++)
        {
            GameState.CharName[(int) i]= buffer.ReadString();
            GameState.CharSprite[(int) i]= buffer.ReadInt32();
            GameState.CharAccess[(int) i]= buffer.ReadInt32();
            GameState.CharJob[(int) i]= buffer.ReadInt32();

            // set as empty or not
            if (Strings.Len(GameState.CharName[i]) == 0)
                isSlotEmpty[(int) i]= true;
        }


        Gui.HideWindows();
        Gui.ShowWindow(Gui.GetWindowIndex("winChars"));

        // set GUi window up
        winNum = Gui.GetWindowIndex("winChars");
        for (var i = 0L; i < Constant.MaxChars; i++)
        {
            conNum = Gui.GetControlIndex("winChars", "lblCharName_" + (i + 1));
            {
                var withBlock = Gui.Windows[winNum].Controls[(int) conNum];
                if (!isSlotEmpty[(int) i])
                {
                    withBlock.Text = GameState.CharName[(int) i];
                }
                else
                {
                    withBlock.Text = "Blank Slot";
                }
            }

            // hide/show buttons
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
        int i;
        var buffer = new PacketReader(data);

        i = buffer.ReadInt32();

        {
            ref var withBlock = ref Data.Job[i];
            withBlock.Name = buffer.ReadString();
            withBlock.Desc = buffer.ReadString();

            withBlock.MaleSprite = buffer.ReadInt32();
            withBlock.FemaleSprite = buffer.ReadInt32();

            int statCount = Enum.GetValues(typeof(Core.Stat)).Length;
            for (int q = 0; q < statCount; q++)
                withBlock.Stat[q]=buffer.ReadInt32();

            for (int q = 0; q < Core.Constant.MaxStartItems; q++)
            {
                withBlock.StartItem[q]= buffer.ReadInt32();
                withBlock.StartValue[q]= buffer.ReadInt32();
            }

            withBlock.StartMap = buffer.ReadInt32();
            withBlock.StartX = buffer.ReadByte();
            withBlock.StartY = buffer.ReadByte();
            withBlock.BaseExp = buffer.ReadInt32();
        }
    }

    public static void Packet_JobData(ReadOnlyMemory<byte> data)
    {
        int i;
        int x;
        var buffer = new PacketReader(data);

        for (i = 0; i < Constant.MaxJobs; i++)
        {
            ref var withBlock = ref Data.Job[i];
            withBlock.Name = buffer.ReadString();
            withBlock.Desc = buffer.ReadString();

            withBlock.MaleSprite = buffer.ReadInt32();
            withBlock.FemaleSprite = buffer.ReadInt32();

            int statCount = Enum.GetValues(typeof(Core.Stat)).Length;
            for (x = 0; x < statCount; x++)
                withBlock.Stat[x]= buffer.ReadInt32();

            for (int q = 0; q < Core.Constant.MaxStartItems; q++)
            {
                withBlock.StartItem[q]= buffer.ReadInt32();
                withBlock.StartValue[q]= buffer.ReadInt32();
            }

            withBlock.StartMap = buffer.ReadInt32();
            withBlock.StartX = buffer.ReadByte();
            withBlock.StartY = buffer.ReadByte();
            withBlock.BaseExp = buffer.ReadInt32();
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

        // show gui
        Gui.ShowWindow(Gui.GetWindowIndex("winHotbar"), resetPosition: false);
        Gui.ShowWindow(Gui.GetWindowIndex("winMenu"), resetPosition: false);
        Gui.ShowWindow(Gui.GetWindowIndex("winBars"), resetPosition: false);
        Gui.HideChat();

        General.GameInit();
    }

    private static void Packet_PlayerInv(ReadOnlyMemory<byte> data)
    {
        int i;
        int itemNum;
        int amount;
        var buffer = new PacketReader(data);

        for (i = 0; i < Constant.MaxInv; i++)
        {
            itemNum = buffer.ReadInt32();
            amount = buffer.ReadInt32();
            SetPlayerInv(GameState.MyIndex, i, itemNum);
            SetPlayerInvValue(GameState.MyIndex, i, amount);
        }

        GameLogic.SetGoldLabel();
    }

    private static void Packet_PlayerInvUpdate(ReadOnlyMemory<byte> data)
    {
        int n;
        int i;
        var buffer = new PacketReader(data);

        n = buffer.ReadInt32();

        SetPlayerInv(GameState.MyIndex, n, buffer.ReadInt32());
        SetPlayerInvValue(GameState.MyIndex, n, buffer.ReadInt32());

        GameLogic.SetGoldLabel();
    }

    private static void Packet_PlayerWornEquipment(ReadOnlyMemory<byte> data)
    {
        int i;
        int n;
        var buffer = new PacketReader(data);

        int equipmentCount = Enum.GetValues(typeof(Equipment)).Length;
        for (i = 0; i < equipmentCount; i++)
        {
            n = buffer.ReadInt32();
            SetPlayerEquipment(GameState.MyIndex, n, (Equipment) i);
        }
    }

    private static void Packet_NpcMove(ReadOnlyMemory<byte> data)
    {
        int mapNpcNum;
        int movement;
        int x;
        int y;
        byte dir;
        var buffer = new PacketReader(data);

        mapNpcNum = buffer.ReadInt32();
        x = buffer.ReadInt32();
        y = buffer.ReadInt32();
        dir = buffer.ReadByte();
        movement = buffer.ReadInt32();

        ref var withBlock = ref Data.MyMapNpc[mapNpcNum];
        withBlock.X = x;
        withBlock.Y = y;
        withBlock.Dir = dir;
        withBlock.Moving = (byte) movement;
    }

    private static void Packet_NpcDir(ReadOnlyMemory<byte> data)
    {
        byte dir;
        int i;
        var buffer = new PacketReader(data);

        i = buffer.ReadInt32();
        dir = buffer.ReadByte();

        {
            ref var withBlock = ref Data.MyMapNpc[i];
            withBlock.Dir = dir;
            withBlock.Moving = 0;
        }
    }

    private static void Packet_Attack(ReadOnlyMemory<byte> data)
    {
        int i;
        var buffer = new PacketReader(data);

        i = buffer.ReadInt32();

        // Set player to attacking
        Core.Data.Player[i].Attacking = 1;
        Core.Data.Player[i].AttackTimer = General.GetTickCount();
    }

    private static void Packet_NpcAttack(ReadOnlyMemory<byte> data)
    {
        int i;
        var buffer = new PacketReader(data);

        i = buffer.ReadInt32();

        // Set Npc to attacking
        Data.MyMapNpc[i].Attacking = 1;
        Data.MyMapNpc[i].AttackTimer = General.GetTickCount();
    }

    private static void Packet_GlobalMsg(ReadOnlyMemory<byte> data)
    {
        string msg;
        var buffer = new PacketReader(data);

        msg = buffer.ReadString();


        Text.AddText(msg, (int) Core.Color.Yellow, channel: (byte) ChatChannel.Broadcast);
    }

    private static void Packet_MapMsg(ReadOnlyMemory<byte> data)
    {
        string msg;
        var buffer = new PacketReader(data);

        msg = buffer.ReadString();


        Text.AddText(msg, (int) Core.Color.White, channel: (byte) ChatChannel.Map);
    }

    private static void Packet_AdminMsg(ReadOnlyMemory<byte> data)
    {
        string msg;
        var buffer = new PacketReader(data);

        msg = buffer.ReadString();


        Text.AddText(msg, (int) Core.Color.BrightCyan, channel: (byte) ChatChannel.Broadcast);
    }

    private static void Packet_PlayerMsg(ReadOnlyMemory<byte> data)
    {
        string msg;
        int color;
        var buffer = new PacketReader(data);

        msg = buffer.ReadString();
        color = buffer.ReadInt32();


        Text.AddText(msg, color, channel: (byte) ChatChannel.Private);
    }

    private static void Packet_SpawnItem(ReadOnlyMemory<byte> data)
    {
        int i;
        var buffer = new PacketReader(data);

        i = buffer.ReadInt32();

        {
            ref var withBlock = ref Data.MyMapItem[i];
            withBlock.Num = buffer.ReadInt32();
            withBlock.Value = buffer.ReadInt32();
            withBlock.X = buffer.ReadInt32();
            withBlock.Y = buffer.ReadInt32();
        }
    }

    private static void Packet_SpawnNpc(ReadOnlyMemory<byte> data)
    {
        int i;
        var buffer = new PacketReader(data);

        i = buffer.ReadInt32();

        ref var withBlock = ref Data.MyMapNpc[i];
        withBlock.Num = buffer.ReadInt32();
        withBlock.X = buffer.ReadInt32();
        withBlock.Y = buffer.ReadInt32();
        withBlock.Dir = buffer.ReadByte();

        for (i = 0; i < Enum.GetValues(typeof(Core.Vital)).Length; i++)
            withBlock.Vital[i]= buffer.ReadInt32();

        // Client use only
        withBlock.Moving = 0;
    }

    private static void Packet_NpcDead(ReadOnlyMemory<byte> data)
    {
        int i;
        var buffer = new PacketReader(data);

        i = buffer.ReadInt32();
        Map.ClearMapNpc(i);
    }

    private static void Packet_UpdateNpc(ReadOnlyMemory<byte> data)
    {
        int i;
        int x;
        var buffer = new PacketReader(data);

        i = buffer.ReadInt32();

        // Update the Npc
        Data.Npc[i].Animation = buffer.ReadInt32();
        Data.Npc[i].AttackSay = buffer.ReadString();
        Data.Npc[i].Behaviour = buffer.ReadByte();

        for (x = 0; x < Constant.MaxDropItems; x++)
        {
            Data.Npc[i].DropChance[x]= buffer.ReadInt32();
            Data.Npc[i].DropItem[x]= buffer.ReadInt32();
            Data.Npc[i].DropItemValue[x]= buffer.ReadInt32();
        }

        Data.Npc[i].Exp = buffer.ReadInt32();
        Data.Npc[i].Faction = buffer.ReadByte();
        Data.Npc[i].Hp = buffer.ReadInt32();
        Data.Npc[i].Name = buffer.ReadString();
        Data.Npc[i].Range = buffer.ReadByte();
        Data.Npc[i].SpawnTime = buffer.ReadByte();
        Data.Npc[i].SpawnSecs = buffer.ReadInt32();
        Data.Npc[i].Sprite = buffer.ReadInt32();

        int statCount = Enum.GetValues(typeof(Core.Stat)).Length;
        for (x = 0; x < statCount; x++)
            Data.Npc[i].Stat[x]= buffer.ReadByte();

        for (x = 0; x < Constant.MaxNpcSkills; x++)
            Data.Npc[i].Skill[x]= buffer.ReadByte();

        Data.Npc[i].Level = buffer.ReadByte();
        Data.Npc[i].Damage = buffer.ReadInt32();
    }

    private static void Packet_UpdateSkill(ReadOnlyMemory<byte> data)
    {
        int skillNum;
        var buffer = new PacketReader(data);
        skillNum = buffer.ReadInt32();

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
        Data.Skill[skillNum].IsAoE = Conversions.ToBoolean(buffer.ReadInt32());
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

        Data.Skill[skillNum].IsProjectile = buffer.ReadInt32();
        Data.Skill[skillNum].Projectile = buffer.ReadInt32();

        Data.Skill[skillNum].KnockBack = (byte) buffer.ReadInt32();
        Data.Skill[skillNum].KnockBackTiles = (byte) buffer.ReadInt32();
    }

    private static void Packet_Skills(ReadOnlyMemory<byte> data)
    {
        int i;
        var buffer = new PacketReader(data);

        for (i = 0; i < Constant.MaxPlayerSkills; i++)
            Core.Data.Player[GameState.MyIndex].Skill[i].Num = buffer.ReadInt32();
    }

    private static void Packet_LeftMap(ReadOnlyMemory<byte> data)
    {
        var buffer = new PacketReader(data);

        Player.ClearPlayer(buffer.ReadInt32());
    }

    private static void Packet_Ping(ReadOnlyMemory<byte> data)
    {
        GameState.PingEnd = General.GetTickCount();
        GameState.Ping = GameState.PingEnd - GameState.PingStart;
    }

    private static void Packet_ActionMessage(ReadOnlyMemory<byte> data)
    {
        int x;
        int y;
        string message;
        int color;
        int tmpType;
        var buffer = new PacketReader(data);

        message = buffer.ReadString();
        color = buffer.ReadInt32();
        tmpType = buffer.ReadInt32();
        x = buffer.ReadInt32();
        y = buffer.ReadInt32();


        GameLogic.CreateActionMsg(message, color, (byte) tmpType, x, y);
    }

    private static void Packet_Blood(ReadOnlyMemory<byte> data)
    {
        int x;
        int y;
        int sprite;
        var buffer = new PacketReader(data);

        x = buffer.ReadInt32();
        y = buffer.ReadInt32();

        // randomise sprite
        sprite = GameLogic.Rand(1, 3);

        GameState.BloodIndex = (byte) (GameState.BloodIndex + 1);
        if (GameState.BloodIndex >= byte.MaxValue)
            GameState.BloodIndex = 1;

        {
            ref var withBlock = ref Data.Blood[GameState.BloodIndex];
            withBlock.X = x;
            withBlock.Y = y;
            withBlock.Sprite = sprite;
            withBlock.Timer = General.GetTickCount();
        }
    }

    private static void Packet_NpcVitals(ReadOnlyMemory<byte> data)
    {
        double mapNpcNum;
        var buffer = new PacketReader(data);

        mapNpcNum = buffer.ReadInt32();
        for (int i = 0; i < Enum.GetValues(typeof(Core.Vital)).Length; i++)
            Data.MyMapNpc[(int) mapNpcNum].Vital[i]= buffer.ReadInt32();
    }

    private static void Packet_Cooldown(ReadOnlyMemory<byte> data)
    {
        int slot;
        var buffer = new PacketReader(data);

        slot = buffer.ReadInt32();
        Core.Data.Player[GameState.MyIndex].Skill[slot].Cd = General.GetTickCount();
    }

    private static void Packet_ClearSkillBuffer(ReadOnlyMemory<byte> data)
    {
        var buffer = new PacketReader(data);

        GameState.SkillBuffer = -1;
        GameState.SkillBufferTimer = 0;
    }

    private static void Packet_SayMessage(ReadOnlyMemory<byte> data)
    {
        int access;
        string name;
        string message;
        string header;
        bool pk;
        byte channelType;
        byte color;
        var buffer = new PacketReader(data);

        name = buffer.ReadString();
        access = buffer.ReadInt32();
        pk = buffer.ReadBoolean();
        message = buffer.ReadString();
        header = buffer.ReadString();

        // Check access level
        switch (access)
        {
            case (int) AccessLevel.Player:
            {
                color = (byte) Core.Color.White;
                break;
            }
            case (int) AccessLevel.Moderator:
            {
                color = (byte) Core.Color.Cyan;
                break;
            }
            case (int) AccessLevel.Mapper:
            {
                color = (byte) Core.Color.Green;
                break;
            }
            case (int) AccessLevel.Developer:
            {
                color = (byte) Core.Color.BrightBlue;
                break;
            }
            case (int) AccessLevel.Owner:
            {
                color = (byte) Core.Color.Yellow;
                break;
            }

            default:
            {
                color = (byte) Core.Color.White;
                break;
            }
        }

        if (pk)
            color = (byte) Core.Color.BrightRed;

        // find channel
        channelType = 0;
        switch (header ?? "")
        {
            case "[Map]:":
            {
                channelType = (byte) ChatChannel.Map;
                break;
            }
            case "[Global]:":
            {
                channelType = (byte) ChatChannel.Broadcast;
                break;
            }
        }

        // add to the chat box
        Text.AddText(header + " " + name + ": " + message, color, channel: channelType);
    }

    private static void Packet_Stunned(ReadOnlyMemory<byte> data)
    {
        var buffer = new PacketReader(data);

        GameState.StunDuration = buffer.ReadInt32();
    }

    private static void Packet_MapWornEquipment(ReadOnlyMemory<byte> data)
    {
        int playerNum;
        int n;
        var buffer = new PacketReader(data);

        playerNum = buffer.ReadInt32();
        int equipmentCount = Enum.GetValues(typeof(Equipment)).Length;
        for (int i = 0; i < equipmentCount; i++)
        {
            n = buffer.ReadInt32();
            SetPlayerEquipment(playerNum, n, (Equipment) i);
        }
    }

    private static void Packet_Target(ReadOnlyMemory<byte> data)
    {
        var buffer = new PacketReader(data);

        GameState.MyTarget = buffer.ReadInt32();
        GameState.MyTargetType = buffer.ReadInt32();
    }

    private static void Packet_MapReport(ReadOnlyMemory<byte> data)
    {
        int i;
        var buffer = new PacketReader(data);

        for (i = 0; i < Constant.MaxMaps; i++)
            GameState.MapNames[i]= buffer.ReadString();

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
        int index;
        int emote;
        var buffer = new PacketReader(data);
        index = buffer.ReadInt32();
        emote = buffer.ReadInt32();

        {
            ref var withBlock = ref Core.Data.Player[index];
            withBlock.Emote = emote;
            withBlock.EmoteTimer = General.GetTickCount() + 5000;
        }
    }

    private static void Packet_ChatBubble(ReadOnlyMemory<byte> data)
    {
        int targetType;
        int target;
        string message;
        int color;
        var buffer = new PacketReader(data);

        target = buffer.ReadInt32();
        targetType = buffer.ReadInt32();
        message = buffer.ReadString();
        color = buffer.ReadInt32();
        GameLogic.AddChatBubble(target, (byte) targetType, message, color);
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
        var buffer = new PacketReader(data);
        Clock.Instance.GameSpeed = buffer.ReadInt32();
        Clock.Instance.Time = new DateTime(BitConverter.ToInt64(buffer.ReadBytes().ToArray(), 0));
    }

    private static void Packet_Time(ReadOnlyMemory<byte> data)
    {
        var buffer = new PacketReader(data);

        Clock.Instance.TimeOfDay = (TimeOfDay) buffer.ReadByte();

        switch (Clock.Instance.TimeOfDay)
        {
            case TimeOfDay.Dawn:
            {
                Text.AddText("A chilling, refreshing, breeze has come with the morning.", (int) Core.Color.DarkGray);
                break;
            }

            case TimeOfDay.Day:
            {
                Text.AddText("Day has dawned in this region.", (int) Core.Color.DarkGray);
                break;
            }

            case TimeOfDay.Dusk:
            {
                Text.AddText("Dusk has begun darkening the skies...", (int) Core.Color.DarkGray);
                break;
            }

            default:
            {
                Text.AddText("Night has fallen upon the weary travelers.", (int) Core.Color.DarkGray);
                break;
            }
        }
    }

    public static void Packet_Hotbar(ReadOnlyMemory<byte> data)
    {
        int i;
        var buffer = new PacketReader(data);

        for (i = 0; i < Constant.MaxHotbar; i++)
        {
            Core.Data.Player[GameState.MyIndex].Hotbar[i].Slot = buffer.ReadInt32();
            Core.Data.Player[GameState.MyIndex].Hotbar[i].SlotType = (byte) buffer.ReadInt32();
        }
    }

    public static void Packet_EditMoral(ReadOnlyMemory<byte> data)
    {
        GameState.InitMoralEditor = true;
    }

    public static void Packet_UpdateMoral(ReadOnlyMemory<byte> data)
    {
        int i;
        var buffer = new PacketReader(data);

        i = buffer.ReadInt32();

        {
            ref var withBlock = ref Data.Moral[i];
            withBlock.Name = buffer.ReadString();
            withBlock.Color = buffer.ReadByte();
            withBlock.NpcBlock = buffer.ReadBoolean();
            withBlock.PlayerBlock = buffer.ReadBoolean();
            withBlock.DropItems = buffer.ReadBoolean();
            withBlock.CanCast = buffer.ReadBoolean();
            withBlock.CanDropItem = buffer.ReadBoolean();
            withBlock.CanPickupItem = buffer.ReadBoolean();
            withBlock.CanPk = buffer.ReadBoolean();
            withBlock.DropItems = buffer.ReadBoolean();
            withBlock.LoseExp = buffer.ReadBoolean();
        }
    }
}