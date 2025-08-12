using Core;
using Core.Globals;
using CSScripting;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualBasic;
using Server;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Reflection;
using static Core.Globals.Command;
using static Core.Net.Packets;
using static Core.Globals.Type;
using static Server.Animation;
using static Server.Event;
using static Server.Item;
using static Server.Moral;
using static Server.NetworkSend;
using static Server.Npc;
using static Server.Party;
using static Server.Player;
using static Server.Projectile;
using static Server.Resource;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Core.Configurations;
using Server.Game;
using Constant = Core.Globals.Constant;
using EventCommand = Core.Globals.EventCommand;
using Type = Core.Globals.Type;

public class Script
{
    public void Loop()
    {

    }

    public void ServerSecond()
    {

    }


    public void ServerMinute()
    {

    }

    public void JoinGame(int index)
    {
        // Warp the player to his saved location
        PlayerWarp(index, GetPlayerMap(index), GetPlayerX(index), GetPlayerY(index), (byte)Direction.Down);

        // Notify everyone that a player has joined the game.
        NetworkSend.GlobalMsg(string.Format("{0} has joined {1}!", GetPlayerName(index), SettingsManager.Instance.GameName));

        // Send all the required game data to the user.
        CheckEquippedItems(index);
        NetworkSend.SendInventory(index);
        NetworkSend.SendWornEquipment(index);
        NetworkSend.SendExp(index);
        NetworkSend.SendHotbar(index);
        NetworkSend.SendPlayerSkills(index);
        NetworkSend.SendStats(index);
        NetworkSend.SendJoinMap(index);

        // Send the flag so they know they can start doing stuff
        NetworkSend.SendInGame(index);

        // Send welcome messages
        NetworkSend.SendWelcome(index);
    }

    public void MapDropItem(int index, int mapSlot, int invSlot, int amount, int mapNum, Type.Item item, int itemNum)
    {
        // Determine if the item is currency or stackable
        if (item.Type == (byte)ItemCategory.Currency || item.Stackable == 1)
        {
            // Check if dropping more than the player has, drop all if so
            var playerInvValue = GetPlayerInvValue(index, invSlot);
            if (amount >= playerInvValue)
            {
                amount = playerInvValue;
                SetPlayerInv(index, invSlot, -1);
                SetPlayerInvValue(index, invSlot, 0);
            }
            else
            {
                SetPlayerInvValue(index, invSlot, playerInvValue - amount);
            }
            NetworkSend.MapMsg(mapNum, string.Format("{0} has dropped {1} ({2}x).", GetPlayerName(index), GameLogic.CheckGrammar(item.Name), amount));
        }
        else
        {
            // Not a currency or stackable item
            SetPlayerInv(index, invSlot, -1);
            SetPlayerInvValue(index, invSlot, 0);

            NetworkSend.MapMsg(mapNum, string.Format("{0} has dropped {1}.", GetPlayerName(index), GameLogic.CheckGrammar(item.Name)));
        }

        // Send inventory update
        NetworkSend.SendInventoryUpdate(index, invSlot);

        // Spawn the item on the map
        Server.Item.SpawnItemSlot(mapSlot, itemNum, amount, mapNum, GetPlayerX(index), GetPlayerY(index));
    }

    public void MapGetItem(int index, int mapNum, int mapSlot, int invSlot)
    {
        // Set item in players inventor
        int itemNum = (int)Data.MapItem[mapNum, mapSlot].Num;

        SetPlayerInv(index, invSlot, (int)Data.MapItem[mapNum, mapSlot].Num);

        string msg;

        if (Data.Item[GetPlayerInv(index, invSlot)].Type == (byte)ItemCategory.Currency | Data.Item[GetPlayerInv(index, invSlot)].Stackable == 1)
        {
            SetPlayerInvValue(index, invSlot, GetPlayerInvValue(index, invSlot) + Data.MapItem[mapNum, mapSlot].Value);
            msg = Data.MapItem[mapNum, mapSlot].Value + " " + Data.Item[GetPlayerInv(index, invSlot)].Name;
        }
        else
        {
            SetPlayerInvValue(index, invSlot, 1);
            msg = Data.Item[GetPlayerInv(index, invSlot)].Name;
        }

        // Erase item from the map
        Server.Item.SpawnItemSlot(mapSlot, -1, 0, GetPlayerMap(index), Data.MapItem[mapNum, mapSlot].X, Data.MapItem[mapNum, mapSlot].Y);
        NetworkSend.SendInventoryUpdate(index, invSlot);
        NetworkSend.SendActionMsg(GetPlayerMap(index), msg, (int)ColorName.White, (byte)ActionMessageType.Static, GetPlayerX(index) * 32, GetPlayerY(index) * 32);
    }

    public void UnEquipItem(int index, int itemNum, int eqSlot)
    {
        int m;

        itemNum = GetPlayerEquipment(index, (Equipment)eqSlot);

        m = FindOpenInvSlot(index, (int)Data.Player[index].Equipment[eqSlot]);
        SetPlayerInv(index, m, Data.Player[index].Equipment[eqSlot]);
        SetPlayerInvValue(index, m, 0);

        NetworkSend.PlayerMsg(index, "You unequip " + GameLogic.CheckGrammar(Data.Item[GetPlayerEquipment(index, (Equipment)eqSlot)].Name), (int)ColorName.Yellow);

        // remove equipment
        SetPlayerEquipment(index, -1, (Equipment)eqSlot);
        NetworkSend.SendWornEquipment(index);
        NetworkSend.SendMapEquipment(index);
        NetworkSend.SendStats(index);
        NetworkSend.SendInventory(index);

        // send vitals
        NetworkSend.SendVitals(index);
    }

    public void UseItem(int index, int itemNum, int invNum)
    {
        int i;
        int n;
        var tempItem = default(int);
        int m;
        var tempdata = new int[Enum.GetValues(typeof(Stat)).Length + 4];
        var tempstr = new string[3];

        // Find out what kind of item it is
        switch (Data.Item[itemNum].Type)
        {
            case (byte)ItemCategory.Equipment:
                {
                    switch (Data.Item[itemNum].SubType)
                    {
                        case (byte)Equipment.Weapon:
                            {

                                if (GetPlayerEquipment(index, Equipment.Weapon) >= 0)
                                {
                                    tempItem = GetPlayerEquipment(index, Equipment.Weapon);
                                }

                                SetPlayerEquipment(index, itemNum, Equipment.Weapon);

                                NetworkSend.PlayerMsg(index, "You equip " + GameLogic.CheckGrammar(Data.Item[itemNum].Name), (int)ColorName.BrightGreen);
                                TakeInv(index, itemNum, 1);

                                if (tempItem >= 0) // give back the stored item
                                {
                                    m = FindOpenInvSlot(index, tempItem);
                                    SetPlayerInv(index, m, tempItem);
                                    SetPlayerInvValue(index, m, 0);
                                }

                                NetworkSend.SendWornEquipment(index);
                                NetworkSend.SendMapEquipment(index);
                                NetworkSend.SendInventory(index);
                                NetworkSend.SendInventoryUpdate(index, invNum);
                                NetworkSend.SendStats(index);

                                // send vitals
                                NetworkSend.SendVitals(index);
                                break;
                            }

                        case (byte)Equipment.Armor:
                            {
                                if (GetPlayerEquipment(index, Equipment.Armor) >= 0)
                                {
                                    tempItem = GetPlayerEquipment(index, Equipment.Armor);
                                }

                                SetPlayerEquipment(index, itemNum, Equipment.Armor);

                                NetworkSend.PlayerMsg(index, "You equip " + GameLogic.CheckGrammar(Data.Item[itemNum].Name), (int)ColorName.BrightGreen);
                                TakeInv(index, itemNum, 1);

                                if (tempItem >= 0) // Return their old equipment to their inventory.
                                {
                                    m = FindOpenInvSlot(index, tempItem);
                                    SetPlayerInv(index, m, tempItem);
                                    SetPlayerInvValue(index, m, 0);
                                }

                                NetworkSend.SendWornEquipment(index);
                                NetworkSend.SendMapEquipment(index);

                                NetworkSend.SendInventory(index);
                                NetworkSend.SendStats(index);

                                // send vitals
                                NetworkSend.SendVitals(index);
                                break;
                            }

                        case (byte)Equipment.Helmet:
                            {
                                if (GetPlayerEquipment(index, Equipment.Helmet) >= 0)
                                {
                                    tempItem = GetPlayerEquipment(index, Equipment.Helmet);
                                }

                                SetPlayerEquipment(index, itemNum, Equipment.Helmet);

                                NetworkSend.PlayerMsg(index, "You equip " + GameLogic.CheckGrammar(Data.Item[itemNum].Name), (int)ColorName.BrightGreen);
                                TakeInv(index, itemNum, 1);

                                if (tempItem >= 0) // give back the stored item
                                {
                                    m = FindOpenInvSlot(index, tempItem);
                                    SetPlayerInv(index, m,  tempItem);
                                    SetPlayerInvValue(index, m, 0);
                                }

                                NetworkSend.SendWornEquipment(index);
                                NetworkSend.SendMapEquipment(index);
                                NetworkSend.SendInventory(index);
                                NetworkSend.SendStats(index);

                                // send vitals
                                NetworkSend.SendVitals(index);
                                break;
                            }

                        case (byte)Equipment.Shield:
                            {
                                if (GetPlayerEquipment(index, Equipment.Shield) >= 0)
                                {
                                    tempItem = GetPlayerEquipment(index, Equipment.Shield);
                                }

                                SetPlayerEquipment(index, itemNum, Equipment.Shield);

                                NetworkSend.PlayerMsg(index, "You equip " + GameLogic.CheckGrammar(Data.Item[itemNum].Name), (int)ColorName.BrightGreen);
                                TakeInv(index, itemNum, 1);

                                if (tempItem >= 0) // give back the stored item
                                {
                                    m = FindOpenInvSlot(index, tempItem);
                                    SetPlayerInv(index, m, tempItem);
                                    SetPlayerInvValue(index, m, 0);
                                }

                                NetworkSend.SendWornEquipment(index);
                                NetworkSend.SendMapEquipment(index);
                                NetworkSend.SendInventory(index);
                                NetworkSend.SendStats(index);

                                // send vitals
                                NetworkSend.SendVitals(index);
                                break;
                            }

                    }

                    break;
                }

            case (byte)ItemCategory.Consumable:
                {
                    switch (Data.Item[itemNum].SubType)
                    {
                        case (byte)ConsumableEffect.RestoresHealth:
                            {
                                NetworkSend.SendActionMsg(GetPlayerMap(index), "+" + Data.Item[itemNum].Data1, (int)ColorName.BrightGreen, (byte)ActionMessageType.Scroll, GetPlayerX(index) * 32, GetPlayerY(index) * 32);
                                Server.Animation.SendAnimation(GetPlayerMap(index), Data.Item[itemNum].Animation, 0, 0, (byte)TargetType.Player, index);
                                SetPlayerVital(index, Vital.Health, GetPlayerVital(index, Vital.Health) + Data.Item[itemNum].Data1);
                                if (Data.Item[itemNum].Stackable == 1)
                                {
                                    TakeInv(index, itemNum, 1);
                                }
                                else
                                {
                                    TakeInv(index, itemNum, 0);
                                }
                                NetworkSend.SendVital(index, Vital.Health);
                                break;
                            }

                        case (byte)ConsumableEffect.RestoresMana:
                            {
                                NetworkSend.SendActionMsg(GetPlayerMap(index), "+" + Data.Item[itemNum].Data1, (int)ColorName.BrightBlue, (byte)ActionMessageType.Scroll, GetPlayerX(index) * 32, GetPlayerY(index) * 32);
                                Server.Animation.SendAnimation(GetPlayerMap(index), Data.Item[itemNum].Animation, 0, 0, (byte)TargetType.Player, index);
                                SetPlayerVital(index, Vital.Stamina, GetPlayerVital(index, Vital.Stamina) + Data.Item[itemNum].Data1);
                                if (Data.Item[itemNum].Stackable == 1)
                                {
                                    TakeInv(index, itemNum, 1);
                                }
                                else
                                {
                                    TakeInv(index, itemNum, 0);
                                }
                                NetworkSend.SendVital(index, Vital.Stamina);
                                break;
                            }

                        case (byte)ConsumableEffect.RestoresStamina:
                            {
                                Server.Animation.SendAnimation(GetPlayerMap(index), Data.Item[itemNum].Animation, 0, 0, (byte)TargetType.Player, index);
                                SetPlayerVital(index, Vital.Stamina, GetPlayerVital(index, Vital.Stamina) + Data.Item[itemNum].Data1);
                                if (Data.Item[itemNum].Stackable == 1)
                                {
                                    TakeInv(index, itemNum, 1);
                                }
                                else
                                {
                                    TakeInv(index, itemNum, 0);
                                }
                                NetworkSend.SendVital(index, Vital.Stamina);
                                break;
                            }

                        case (byte)ConsumableEffect.GrantsExperience:
                            {
                                Server.Animation.SendAnimation(GetPlayerMap(index), Data.Item[itemNum].Animation, 0, 0, (byte)TargetType.Player, index);
                                SetPlayerExp(index, GetPlayerExp(index) + Data.Item[itemNum].Data1);
                                if (Data.Item[itemNum].Stackable == 1)
                                {
                                    TakeInv(index, itemNum, 1);
                                }
                                else
                                {
                                    TakeInv(index, itemNum, 0);
                                }
                                NetworkSend.SendExp(index);
                                break;
                            }

                    }

                    break;
                }

            case (byte)ItemCategory.Projectile:
                {
                    if (Data.Item[itemNum].Ammo > 0)
                    {
                        if (HasItem(index, Data.Item[itemNum].Ammo) > 0)
                        {
                            TakeInv(index, Data.Item[itemNum].Ammo, 1);
                            Server.Projectile.PlayerFireProjectile(index);
                        }
                        else
                        {
                            NetworkSend.PlayerMsg(index, "No More " + Data.Item[Data.Item[GetPlayerEquipment(index, Equipment.Weapon)].Ammo].Name + " !", (int)ColorName.BrightRed);
                            return;
                        }
                    }
                    else
                    {
                        Server.Projectile.PlayerFireProjectile(index);
                        return;
                    }

                    break;
                }

            case (byte)ItemCategory.Event:
                {
                    n = Data.Item[itemNum].Data1;

                    switch (Data.Item[itemNum].SubType)
                    {
                        case (byte)EventCommand.ModifyVariable:
                            {
                                Data.Player[index].Variables[n] = Data.Item[itemNum].Data2;
                                break;
                            }
                        case (byte)EventCommand.ModifySwitch:
                            {
                                Data.Player[index].Switches[n] = (byte)Data.Item[itemNum].Data2;
                                break;
                            }
                        case (byte)EventCommand.Key:
                            {
                                EventLogic.TriggerEvent(index, 1, 0, GetPlayerX(index), GetPlayerY(index));
                                break;
                            }
                    }

                    break;
                }

            case (byte)ItemCategory.Skill:
                {
                    PlayerLearnSkill(index, itemNum);
                    break;
                }
        }
    }

    public static void PlayerLearnSkill(int index, int itemNum, int skillNum = -1)
    {
        int n;
        int i;

        // Get the skill num
        if (skillNum >= 0)
        {
            n = skillNum;
        }
        else
        {
            n = Data.Item[itemNum].Data1;
        }

        if (n < 0 | n > Constant.MaxSkills)
            return;

        // Make sure they are the right class
        if (Data.Skill[n].JobReq == GetPlayerJob(index) | Data.Skill[n].JobReq == -1)
        {
            // Make sure they are the right level
            i = Data.Skill[n].LevelReq;

            if (i <= GetPlayerLevel(index))
            {
                i = FindOpenSkill(index);

                // Make sure they have an open skill slot
                if (i >= 0)
                {
                    // Make sure they dont already have the skill
                    if (!HasSkill(index, n))
                    {
                        SetPlayerSkill(index, i, n);
                        if (itemNum >= 0)
                        {
                            Server.Animation.SendAnimation(GetPlayerMap(index), Data.Item[itemNum].Animation, 0, 0, (byte)TargetType.Player, index);
                            TakeInv(index, itemNum, 0);
                        }
                        NetworkSend.PlayerMsg(index, "You study the skill carefully.", (int)ColorName.Yellow);
                        NetworkSend.PlayerMsg(index, "You have learned a new skill!", (int)ColorName.BrightGreen);
                        NetworkSend.SendPlayerSkills(index);
                    }
                    else
                    {
                        NetworkSend.PlayerMsg(index, "You have already learned this skill!", (int)ColorName.BrightRed);
                    }
                }
                else
                {
                    NetworkSend.PlayerMsg(index, "You have learned all that you can learn!", (int)ColorName.BrightRed);
                }
            }
            else
            {
                NetworkSend.PlayerMsg(index, "You must be level " + i + " to learn this skill.", (int)ColorName.Yellow);
            }
        }
        else
        {
            NetworkSend.PlayerMsg(index, string.Format("Only {0} can use this skill.", GameLogic.CheckGrammar(Data.Job[Data.Skill[n].JobReq].Name, 1)), (int)ColorName.BrightRed);
        }
    }

    public void JoinMap(int index)
    {
        byte[] data;
        int dataSize;
        int mapNum = GetPlayerMap(index);

        // Send all players on current map to index
        foreach (var player in PlayerService.Instance.Players)
        {
            if (IsPlaying(player.Id))
            {
                if (player.Id != index)
                {
                    if (GetPlayerMap(player.Id) == mapNum)
                    {
                        data = GetPlayerDataPacket(player.Id);
                        PlayerService.Instance.SendDataTo(index, data);
                        SendPlayerXyTo(index, player.Id);
                        NetworkSend.SendMapEquipmentTo(index, player.Id);
                    }
                }
            }
        }

        EventLogic.SpawnMapEventsFor(index, GetPlayerMap(index));

        // Send index's player data to everyone on the map including himself
        data = GetPlayerDataPacket(index);
        NetworkConfig.SendDataToMap(mapNum, data);
        SendPlayerXyToMap(index);
        NetworkSend.SendMapEquipment(index);
        NetworkSend.SendVitals(index);
    }

    public void LeaveMap(int index, int mapNum)
    {

    }

    public void LeftGame(int index)
    {

    }

    public void OnDeath(int index)
    {
        // Set HP to nothing
        SetPlayerVital(index, Vital.Health, 0);

        // Restore vitals
        var count = System.Enum.GetValues(typeof(Vital)).Length;
        for (int i = 0, loopTo = count; i < loopTo; i++)
            SetPlayerVital(index, (Vital)i, GetPlayerMaxVital(index, (Vital)i));

        // If the player the attacker killed was a pk then take it away
        if (GetPlayerPk(index))
        {
            SetPlayerPk(index, false);
        }

        ref var withBlock = ref Data.Map[GetPlayerMap(index)];

        // Warp player away
        SetPlayerDir(index, (byte)Direction.Down);

        // to the bootmap if it is set
        if (withBlock.BootMap > 0)
        {
            PlayerWarp(index, withBlock.BootMap, withBlock.BootX, withBlock.BootY, (int)Direction.Down);
        }
        else
        {
            PlayerWarp(index, Data.Job[GetPlayerJob(index)].StartMap, Data.Job[GetPlayerJob(index)].StartX, Data.Job[GetPlayerJob(index)].StartY, (int)Direction.Down);
        }
    }

    public void BufferSkill(int mapNum, int index, int skillNum)
    {
  
    }

    public int KillPlayer(int index)
    {
        int exp = GetPlayerExp(index) / 3;
        
        if (exp == 0)
        {
            NetworkSend.PlayerMsg(index, "You've lost no experience.", (int)ColorName.BrightGreen);
        }
        else
        {                   
            NetworkSend.SendExp(index);
            NetworkSend.PlayerMsg(index, string.Format("You've lost {0} experience.", exp), (int)ColorName.BrightRed);
        }

        return exp;
    }

    public void TrainStat(int index, int tmpStat)
    {
        // make sure their stats are not maxed
        if (GetPlayerRawStat(index, (Stat)tmpStat) >= Constant.MaxStats)
        {
            NetworkSend.PlayerMsg(index, "You cannot spend any more points on that stat.", (int)ColorName.BrightRed);
            return;
        }

        // increment stat
        SetPlayerStat(index, (Stat)tmpStat, GetPlayerRawStat(index, (Stat)tmpStat) + 1);

        // decrement points
        SetPlayerPoints(index, GetPlayerPoints(index) - 1);

        // send player new data
        NetworkSend.SendPlayerData(index);
    }

    public void PlayerMove(int index)
    {

    }

    public void UpdateMapAi()
    {

        long tickCount = General.GetTimeMs();
        var entities = Core.Globals.Entity.Instances;

        for (int x = 0; x < entities.Count; x++)
        {
            var entity = entities[x];
            var mapNum = entity.Map;
            if (entity == null) continue;

            // Only process entities that are Npcs
            if (entity.Num < 0) continue;

            // check if they've completed casting, and if so set the actual skill going
            if (entity.SkillBuffer >= 0)
            {
                if (General.GetTimeMs() > entity.SkillBufferTimer + Data.Skill[entity.SkillBuffer].CastTime * 1000)
                {
                    if (Data.Moral[Data.Map[mapNum].Moral].CanCast)
                    {
                        //BufferSkill(mapNum, [Core.Globals.Entity.Index(entity), entity.SkillBuffer);
                        entity.SkillBuffer = -1;
                        entity.SkillBufferTimer = 0;
                    }
                }
            }
            else
            {
                // ATTACKING ON SIGHT
                if (entity.Behaviour == (byte)NpcBehavior.AttackOnSight || entity.Behaviour == (byte)NpcBehavior.Guard)
                {
                    // make sure it's not stunned
                    if (!(entity.StunDuration > 0))
                    {
                        foreach (var player in PlayerService.Instance.Players)
                        {
                            if (NetworkConfig.IsPlaying(player.Id))
                            {
                                if (GetPlayerMap(player.Id) == mapNum && entity.TargetType == 0 && GetPlayerAccess(player.Id) <= (byte)AccessLevel.Moderator)
                                {
                                    int n = entity.Range;
                                    int distanceX = entity.X - GetPlayerX(player.Id);
                                    int distanceY = entity.Y - GetPlayerY(player.Id);

                                    if (distanceX < 0) distanceX *= -1;
                                    if (distanceY < 0) distanceY *= -1;

                                    if (distanceX <= n && distanceY <= n)
                                    {
                                        if (entity.Behaviour == (byte)NpcBehavior.AttackOnSight || GetPlayerPk(player.Id))
                                        {
                                            if (!string.IsNullOrEmpty(entity.AttackSay))
                                            {
                                                NetworkSend.PlayerMsg(player.Id, GameLogic.CheckGrammar(entity.Name, 1) + " says, '" + entity.AttackSay + "' to you.", (int)ColorName.Yellow);
                                            }
                                            entity.TargetType = (byte)TargetType.Player;
                                            entity.Target = player.Id;
                                        }
                                    }
                                }
                            }
                        }

                        // Check if target was found for Npc targeting
                        if (entity.TargetType == 0 && entity.Faction > 0)
                        {
                            for (int i = 0; i < entities.Count; i++)
                            {
                                var otherEntity = entities[i];
                                if (otherEntity != null && otherEntity.Num >= 0)
                                {
                                    if (otherEntity.Map != mapNum) continue;
                                    if (ReferenceEquals(otherEntity, entity)) continue;
                                    if ((int)otherEntity.Faction > 0 && otherEntity.Faction != entity.Faction)
                                    {
                                        int n = entity.Range;
                                        int distanceX = entity.X - otherEntity.X;
                                        int distanceY = entity.Y - otherEntity.Y;

                                        if (distanceX < 0) distanceX *= -1;
                                        if (distanceY < 0) distanceY *= -1;

                                        if (distanceX <= n && distanceY <= n && entity.Behaviour == (byte)NpcBehavior.AttackOnSight)
                                        {
                                            entity.TargetType = (byte)TargetType.Npc;
                                            entity.Target = i;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                bool targetVerify = false;

                // Npc walking/targeting
                if (entity.StunDuration > 0)
                {
                    if (General.GetTimeMs() > entity.StunTimer + entity.StunDuration * 1000)
                    {
                        entity.StunDuration = 0;
                        entity.StunTimer = 0;
                    }
                }
                else
                {
                    int target = entity.Target;
                    byte targetType = entity.TargetType;
                    int targetX = 0, targetY = 0;

                    if (entity.Type == Core.Globals.Entity.EntityType.Npc)
                    {
                        if (entity.Behaviour != (byte)NpcBehavior.ShopKeeper && entity.Behaviour != (byte)NpcBehavior.QuestGiver)
                        {
                            if (target > 0)
                            {
                                if (entities[mapNum].Map == mapNum)
                                {
                                    targetVerify = true;
                                    targetX = entities[target].X;
                                    targetY = entities[target].Y;
                                }
                                else
                                {
                                    entity.TargetType = 0;
                                    entity.Target = 0;
                                }
                            }

                            if (targetVerify)
                            {
                                if (!Server.Event.IsOneBlockAway(targetX, targetY, (int)entity.X, (int)entity.Y))
                                {
                                    int i = EventLogic.FindNpcPath(mapNum, Core.Globals.Entity.Index(entity), targetX, targetY);
                                    if (i < 4)
                                    {
                                        if (Server.Npc.CanNpcMove(mapNum, Core.Globals.Entity.Index(entity), (byte)i))
                                        {
                                            Server.Npc.NpcMove(mapNum, Core.Globals.Entity.Index(entity), (byte)i, (int)MovementState.Walking);
                                        }
                                    }
                                    else
                                    {
                                        i = (int)Math.Round(new Random().NextDouble() * 3) + 1;
                                        if (i == 1)
                                        {
                                            i = (int)Math.Round(new Random().NextDouble() * 3) + 1;
                                            if (Server.Npc.CanNpcMove(mapNum, Core.Globals.Entity.Index(entity), (byte)i))
                                            {
                                                Server.Npc.NpcMove(mapNum, Core.Globals.Entity.Index(entity), (byte)i, (int)MovementState.Walking);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Server.Npc.NpcDir(mapNum, Core.Globals.Entity.Index(entity), Server.Event.GetNpcDir(targetX, targetY, (int)entity.X, (int)entity.Y));
                                }
                            }
                            else
                            {
                                int i = (int)Math.Round(new Random().NextDouble() * 4);
                                if (i == 1)
                                {
                                    i = (int)Math.Round(new Random().NextDouble() * 4);
                                    if (Server.Npc.CanNpcMove(mapNum, Core.Globals.Entity.Index(entity), (byte)i))
                                    {
                                        Server.Npc.NpcMove(mapNum, Core.Globals.Entity.Index(entity), (byte)i, (int)MovementState.Walking);
                                    }
                                }
                            }
                        }
                    }

                    // Npcs attack targets
                    int attackTarget = entity.Target;
                    byte attackTargetType = entity.TargetType;

                    if (attackTarget > 0)
                    {                    
                        if (GetPlayerMap(attackTarget) == mapNum)
                        {
                            // Placeholder for attack logic
                        }
                        else
                        {
                            entity.Target = 0;
                            entity.TargetType = 0;
                        }                        
                    }

                    // Placeholder for Regen logic

                    // Check if the npc is dead or not
                    if (entity.Vital[(byte)Vital.Health] < 0 && entity.SpawnWait > 0)
                    {
                        entity.Num = 0;
                        entity.SpawnWait = General.GetTimeMs();
                        entity.Vital[(byte)Vital.Health] = 0;
                    }

                    // Spawning an Npc
                    if (entity.Type == Core.Globals.Entity.EntityType.Npc)
                    {
                        if (entity.Num == -1)
                        {
                            if (entity.SpawnSecs > 0)
                            {
                                if (tickCount > entity.SpawnWait + entity.SpawnSecs * 1000)
                                {
                                    Server.Npc.SpawnNpc(x, mapNum);
                                }
                            }
                        }
                    }
                }
            }
        }

        var now = General.GetTimeMs();
        var itemCount = Constant.MaxMapItems;
        var mapCount = Constant.MaxMaps;

        for (int mapNum = 0; mapNum < mapCount; mapNum++)
        {
            // Handle map items (public/despawn)
            for (int i = 0; i < itemCount; i++)
            {
                var item = Data.MapItem[mapNum, i];
                if (item.Num >= 0 && !string.IsNullOrEmpty(item.PlayerName))
                {
                    if (item.PlayerTimer < now)
                    {
                        item.PlayerName = "";
                        item.PlayerTimer = 0;
                        Server.Item.SendMapItemsToAll(mapNum);
                    }
                    if (item.CanDespawn && item.DespawnTimer < now)
                    {
                        Database.ClearMapItem(i, mapNum);
                        Server.Item.SendMapItemsToAll(mapNum);
                    }
                }
            }

            // Respawn resources
            var mapResource = Data.MapResource[mapNum];
            if (mapResource.ResourceCount > 0)
            {
                for (int i = 0; i < mapResource.ResourceCount; i++)
                {
                    var resData = mapResource.ResourceData[i];
                    int resourceindex = Data.Map[mapNum].Tile[resData.X, resData.Y].Data1;
                    if (resourceindex > 0)
                    {
                        if (resData.State == 1 || resData.Health < 1)
                        {
                            if (resData.Timer + Data.Resource[resourceindex].RespawnTime * 1000 < now)
                            {
                                resData.Timer = now;
                                resData.State = 0;
                                resData.Health = (byte)Data.Resource[resourceindex].Health;
                                Server.Resource.SendMapResourceToMap(mapNum);
                            }
                        }
                    }
                }
            }
        }
    }

    public void CheckPlayerLevelUp(int index)
    {
        int expRollover;
        int level_count;

        level_count = 0;

        while (GetPlayerExp(index) >= GetPlayerNextLevel(index))
        {
            expRollover = GetPlayerExp(index) - GetPlayerNextLevel(index);
            SetPlayerLevel(index, GetPlayerLevel(index) + 1);
            SetPlayerPoints(index, GetPlayerPoints(index) + Server.Constant.StatPerLevel);
            SetPlayerExp(index, expRollover);
            level_count += 1;
        }

        if (level_count > 0)
        {
            if (level_count == 1)
            {
                // singular
                NetworkSend.GlobalMsg(GetPlayerName(index) + " has gained " + level_count + " level!");
            }
            else
            {
                // plural
                NetworkSend.GlobalMsg(GetPlayerName(index) + " has gained " + level_count + " levels!");
            }
            NetworkSend.SendActionMsg(GetPlayerMap(index), "Level Up", (int) ColorName.Yellow, 1, GetPlayerX(index) * 32, GetPlayerY(index) * 32);
            NetworkSend.SendExp(index);
            NetworkSend.SendPlayerData(index);
        }
    }
}