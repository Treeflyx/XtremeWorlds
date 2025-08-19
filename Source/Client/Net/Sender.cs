using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Client.Game.UI;
using Core;
using Core.Globals;
using Core.Net;
using static Core.Globals.Command;

namespace Client.Net;

public static class Sender
{
    private static readonly int StatCount = Enum.GetValues<Stat>().Length;

    public static void SendAddChar(string characterName, int sexNum, int jobNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CAddChar);
        packetWriter.WriteByte(GameState.CharNum);
        packetWriter.WriteString(characterName);
        packetWriter.WriteInt32(sexNum);
        packetWriter.WriteInt32(jobNum);

        Network.Send(packetWriter);
    }

    public static void SendUseChar(byte slot)
    {
        var packetWriter = new PacketWriter(5);

        packetWriter.WriteEnum(Packets.ClientPackets.CUseChar);
        packetWriter.WriteByte(slot);

        Network.Send(packetWriter);
    }

    public static void SendDelChar(byte slot)
    {
        var packetWriter = new PacketWriter(5);

        packetWriter.WriteEnum(Packets.ClientPackets.CDelChar);
        packetWriter.WriteByte(slot);

        Network.Send(packetWriter);
    }

    public static void SendLogout()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CLogout);

        Network.Send(packetWriter);
    }

    private static byte[] Encrypt(byte[] data)
    {
        using var aes = Aes.Create();

        aes.Key = General.AesKey;
        aes.IV = General.AesIV;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

        cryptoStream.Write(data, 0, data.Length);
        cryptoStream.FlushFinalBlock();

        return memoryStream.ToArray();
    }

    private static string GetVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
    }

    public static void SendLogin(string username, string password)
    {
        var usernameBytes = Encrypt(Encoding.UTF8.GetBytes(username));
        var passwordBytes = Encrypt(Encoding.UTF8.GetBytes(password));
        var versionBytes = Encrypt(Encoding.UTF8.GetBytes(GetVersion()));

        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CLogin);
        packetWriter.WriteBytes(usernameBytes);
        packetWriter.WriteBytes(passwordBytes);
        packetWriter.WriteBytes(versionBytes);

        Network.Send(packetWriter);
    }

    public static void SendRegister(string username, string password)
    {
        var usernameBytes = Encrypt(Encoding.UTF8.GetBytes(username));
        var passwordBytes = Encrypt(Encoding.UTF8.GetBytes(password));
        var versionBytes = Encrypt(Encoding.UTF8.GetBytes(GetVersion()));

        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CRegister);
        packetWriter.WriteBytes(usernameBytes);
        packetWriter.WriteBytes(passwordBytes);
        packetWriter.WriteBytes(versionBytes);

        Network.Send(packetWriter);
    }
    
    public static void GetPing()
    {
        var packetWriter = new PacketWriter(4);

        GameState.PingStart = General.GetTickCount();

        packetWriter.WriteEnum(Packets.ClientPackets.CCheckPing);

        Network.Send(packetWriter);
    }

    public static void SendPlayerMove()
    {
        var packetWriter = new PacketWriter(14);

        packetWriter.WriteEnum(Packets.ClientPackets.CPlayerMove);
        packetWriter.WriteByte(GetPlayerDir(GameState.MyIndex));
        packetWriter.WriteByte(Data.Player[GameState.MyIndex].Moving);
        packetWriter.WriteInt32(Data.Player[GameState.MyIndex].X);
        packetWriter.WriteInt32(Data.Player[GameState.MyIndex].Y);

        Network.Send(packetWriter);
    }

    public static void SendStopPlayerMove()
    {
        var packetWriter = new PacketWriter(5);

        packetWriter.WriteEnum(Packets.ClientPackets.CStopPlayerMove);
        packetWriter.WriteByte(GetPlayerDir(GameState.MyIndex));

        Network.Send(packetWriter);
    }

    public static void SayMsg(string text)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CSayMsg);
        packetWriter.WriteString(text);

        Network.Send(packetWriter);
    }

    public static void SendKick(string name)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CKickPlayer);
        packetWriter.WriteString(name);

        Network.Send(packetWriter);
    }

    public static void SendBan(string name)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CBanPlayer);
        packetWriter.WriteString(name);

        Network.Send(packetWriter);
    }

    public static void WarpMeTo(string name)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CWarpMeTo);
        packetWriter.WriteString(name);

        Network.Send(packetWriter);
    }

    public static void WarpToMe(string name)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CWarpToMe);
        packetWriter.WriteString(name);

        Network.Send(packetWriter);
    }

    public static void WarpTo(int mapNum)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CWarpTo);
        packetWriter.WriteInt32(mapNum);

        Network.Send(packetWriter);
    }

    public static void SendRequestLevelUp()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestLevelUp);

        Network.Send(packetWriter);
    }

    public static void SendSpawnItem(int tmpItem, int tmpAmount)
    {
        var packetWriter = new PacketWriter(12);

        packetWriter.WriteEnum(Packets.ClientPackets.CSpawnItem);
        packetWriter.WriteInt32(tmpItem);
        packetWriter.WriteInt32(tmpAmount);

        Network.Send(packetWriter);
    }

    public static void SendSetSprite(int spriteNum)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CSetSprite);
        packetWriter.WriteInt32(spriteNum);

        Network.Send(packetWriter);
    }

    public static void SendSetAccess(string name, byte access)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CSetAccess);
        packetWriter.WriteString(name);
        packetWriter.WriteInt32(access);

        Network.Send(packetWriter);
    }

    public static void SendAttack()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CAttack);

        Network.Send(packetWriter);
    }

    public static void SendPlayerDir()
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CPlayerDir);
        packetWriter.WriteInt32(GetPlayerDir(GameState.MyIndex));

        Network.Send(packetWriter);
    }

    public static void SendRequestNpc(int npcNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestNpc);
        packetWriter.WriteDouble(npcNum);

        Network.Send(packetWriter);
    }

    public static void SendRequestSkill(int skillNum)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestSkill);
        packetWriter.WriteInt32(skillNum);

        Network.Send(packetWriter);
    }

    public static void SendTrainStat(byte statNum)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CTrainStat);
        packetWriter.WriteInt32(statNum);

        Network.Send(packetWriter);
    }

    public static void BroadcastMsg(string text)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CBroadcastMsg);
        packetWriter.WriteString(text);

        Network.Send(packetWriter);
    }

    public static void PlayerMsg(string text, string msgTo)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CPlayerMsg);
        packetWriter.WriteString(msgTo);
        packetWriter.WriteString(text);

        Network.Send(packetWriter);
    }

    public static void AdminMsg(string text)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CAdminMsg);
        packetWriter.WriteString(text);

        Network.Send(packetWriter);
    }

    public static void SendWhosOnline()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CWhosOnline);

        Network.Send(packetWriter);
    }

    public static void SendPlayerInfo(string name)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CPlayerInfoRequest);
        packetWriter.WriteString(name);

        Network.Send(packetWriter);
    }

    public static void SendMotdChange(string welcome)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CSetMotd);
        packetWriter.WriteString(welcome);

        Network.Send(packetWriter);
    }

    public static void SendBanList()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CBanList);

        Network.Send(packetWriter);
    }

    public static void SendBanDestroy()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CBanDestroy);

        Network.Send(packetWriter);
    }

    public static void SendChangeInvSlots(int oldSlot, int newSlot)
    {
        var buffer = new PacketWriter(4);

        buffer.WriteInt32((int) Packets.ClientPackets.CSwapInvSlots);
        buffer.WriteInt32(oldSlot);
        buffer.WriteInt32(newSlot);

        Network.Send(buffer);
    }

    public static void SendChangeSkillSlots(int oldSlot, int newSlot)
    {
        var packetWriter = new PacketWriter(12);

        packetWriter.WriteEnum(Packets.ClientPackets.CSwapSkillSlots);
        packetWriter.WriteInt32(oldSlot);
        packetWriter.WriteInt32(newSlot);

        Network.Send(packetWriter);
    }

    public static void SendUseItem(int invNum)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CUseItem);
        packetWriter.WriteInt32(invNum);

        Network.Send(packetWriter);
    }

    public static void SendDropItem(int invNum, int amount)
    {
        if (GameState.InBank || GameState.InShop >= 0)
        {
            return;
        }

        if (invNum is < 0 or > Constant.MaxInv)
        {
            return;
        }

        if (Data.Player[GameState.MyIndex].Inv[invNum].Num < 0 ||
            Data.Player[GameState.MyIndex].Inv[invNum].Num > Constant.MaxItems)
        {
            return;
        }

        if (Data.Item[GetPlayerInv(GameState.MyIndex, invNum)].Type == (byte) ItemCategory.Currency ||
            Data.Item[GetPlayerInv(GameState.MyIndex, invNum)].Stackable == 1)
        {
            if (amount < 0 || amount > Data.Player[GameState.MyIndex].Inv[invNum].Value)
            {
                return;
            }
        }

        var packetWriter = new PacketWriter(12);

        packetWriter.WriteEnum(Packets.ClientPackets.CMapDropItem);
        packetWriter.WriteInt32(invNum);
        packetWriter.WriteInt32(amount);

        Network.Send(packetWriter);
    }

    public static void PlayerSearch(int curX, int curY, byte rClick)
    {
        if (!GameLogic.IsInBounds())
        {
            return;
        }

        var packetWriter = new PacketWriter(16);

        packetWriter.WriteEnum(Packets.ClientPackets.CSearch);
        packetWriter.WriteInt32(GameState.CurX);
        packetWriter.WriteInt32(GameState.CurY);
        packetWriter.WriteInt32(rClick);

        Network.Send(packetWriter);
    }

    public static void AdminWarp(int x, int y)
    {
        var packetWriter = new PacketWriter(12);

        packetWriter.WriteEnum(Packets.ClientPackets.CAdminWarp);
        packetWriter.WriteInt32(x);
        packetWriter.WriteInt32(y);

        Network.Send(packetWriter);
    }

    public static void SendUnequip(int eqNum)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CUnequip);
        packetWriter.WriteInt32(eqNum);

        Network.Send(packetWriter);
    }

    public static void ForgetSkill(int skillSlot)
    {
        // Check for subscript out of range
        if (skillSlot is < 0 or > Constant.MaxPlayerSkills)
        {
            return;
        }

        // Dont let them forget a skill which is in CD
        if (Data.Player[GameState.MyIndex].Skill[skillSlot].Cd > 0)
        {
            TextRenderer.AddText("Cannot forget a skill which is cooling down!", (int) ColorName.Red);
            return;
        }

        // Dont let them forget a skill which is buffered
        if (GameState.SkillBuffer == skillSlot)
        {
            TextRenderer.AddText("Cannot forget a skill which you are casting!", (int) ColorName.Red);
            return;
        }

        if (Data.Player[GameState.MyIndex].Skill[skillSlot].Num < 0)
        {
            TextRenderer.AddText("No skill found.", (int) ColorName.Red);
            return;
        }

        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CForgetSkill);
        packetWriter.WriteInt32(skillSlot);

        Network.Send(packetWriter);
    }

    public static void SendRequestMapReport()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CMapReport);

        Network.Send(packetWriter);
    }

    public static void SendRequestAdmin()
    {
        if (GetPlayerAccess(GameState.MyIndex) < (int) AccessLevel.Moderator)
        {
            return;
        }

        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CAdmin);

        Network.Send(packetWriter);
    }

    public static void SendUseEmote(int emote)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CEmote);
        packetWriter.WriteInt32(emote);

        Network.Send(packetWriter);
    }

    public static void SendRequestEditResource()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestEditResource);

        Network.Send(packetWriter);
    }

    public static void SendSaveResource(int resourceNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CSaveResource);
        packetWriter.WriteInt32(resourceNum);
        packetWriter.WriteInt32(Data.Resource[resourceNum].Animation);
        packetWriter.WriteString(Data.Resource[resourceNum].EmptyMessage);
        packetWriter.WriteInt32(Data.Resource[resourceNum].ExhaustedImage);
        packetWriter.WriteInt32(Data.Resource[resourceNum].Health);
        packetWriter.WriteInt32(Data.Resource[resourceNum].ExpReward);
        packetWriter.WriteInt32(Data.Resource[resourceNum].ItemReward);
        packetWriter.WriteString(Data.Resource[resourceNum].Name);
        packetWriter.WriteInt32(Data.Resource[resourceNum].ResourceImage);
        packetWriter.WriteInt32(Data.Resource[resourceNum].ResourceType);
        packetWriter.WriteInt32(Data.Resource[resourceNum].RespawnTime);
        packetWriter.WriteString(Data.Resource[resourceNum].SuccessMessage);
        packetWriter.WriteInt32(Data.Resource[resourceNum].LvlRequired);
        packetWriter.WriteInt32(Data.Resource[resourceNum].ToolRequired);
        packetWriter.WriteBoolean(Data.Resource[resourceNum].Walkthrough);

        Network.Send(packetWriter);
    }

    public static void SendRequestEditNpc()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestEditNpc);

        Network.Send(packetWriter);
    }

    public static void SendSaveNpc(int npcNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CSaveNpc);
        packetWriter.WriteInt32(npcNum);
        packetWriter.WriteInt32(Data.Npc[npcNum].Animation);
        packetWriter.WriteString(Data.Npc[npcNum].AttackSay);
        packetWriter.WriteByte(Data.Npc[npcNum].Behaviour);

        for (var i = 0; i < Constant.MaxDropItems; i++)
        {
            packetWriter.WriteInt32(Data.Npc[npcNum].DropChance[i]);
            packetWriter.WriteInt32(Data.Npc[npcNum].DropItem[i]);
            packetWriter.WriteInt32(Data.Npc[npcNum].DropItemValue[i]);
        }

        packetWriter.WriteInt32(Data.Npc[npcNum].Exp);
        packetWriter.WriteByte(Data.Npc[npcNum].Faction);
        packetWriter.WriteInt32(Data.Npc[npcNum].Hp);
        packetWriter.WriteString(Data.Npc[npcNum].Name);
        packetWriter.WriteByte(Data.Npc[npcNum].Range);
        packetWriter.WriteByte(Data.Npc[npcNum].SpawnTime);
        packetWriter.WriteInt32(Data.Npc[npcNum].SpawnSecs);
        packetWriter.WriteInt32(Data.Npc[npcNum].Sprite);

        for (var i = 0; i < StatCount; i++)
        {
            packetWriter.WriteByte(Data.Npc[npcNum].Stat[i]);
        }

        for (var i = 0; i < Constant.MaxNpcSkills; i++)
        {
            packetWriter.WriteByte(Data.Npc[npcNum].Skill[i]);
        }

        packetWriter.WriteInt32(Data.Npc[npcNum].Level);
        packetWriter.WriteInt32(Data.Npc[npcNum].Damage);

        Network.Send(packetWriter);
    }

    public static void SendRequestEditSkill()
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestEditSkill);

        Network.Send(packetWriter);
    }

    public static void SendSaveSkill(int skillNum)
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CSaveSkill);
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

        Network.Send(packetWriter);
    }

    public static void SendSaveShop(int shopNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CSaveShop);
        packetWriter.WriteInt32(shopNum);
        packetWriter.WriteInt32(Data.Shop[shopNum].BuyRate);
        packetWriter.WriteString(Data.Shop[shopNum].Name);

        for (var i = 0; i < Constant.MaxTrades; i++)
        {
            packetWriter.WriteInt32(Data.Shop[shopNum].TradeItem[i].CostItem);
            packetWriter.WriteInt32(Data.Shop[shopNum].TradeItem[i].CostValue);
            packetWriter.WriteInt32(Data.Shop[shopNum].TradeItem[i].Item);
            packetWriter.WriteInt32(Data.Shop[shopNum].TradeItem[i].ItemValue);
        }

        Network.Send(packetWriter);
    }

    public static void SendRequestEditShop()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestEditShop);

        Network.Send(packetWriter);
    }

    public static void SendSaveAnimation(int animationNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CSaveAnimation);
        packetWriter.WriteInt32(animationNum);

        foreach (var frame in Data.Animation[animationNum].Frames)
        {
            packetWriter.WriteInt32(frame);
        }

        foreach (var loopCount in Data.Animation[animationNum].LoopCount)
        {
            packetWriter.WriteInt32(loopCount);
        }

        foreach (var loopTime in Data.Animation[animationNum].LoopTime)
        {
            packetWriter.WriteInt32(loopTime);
        }

        packetWriter.WriteString(Data.Animation[animationNum].Name);
        packetWriter.WriteString(Data.Animation[animationNum].Sound);

        foreach (var sprite in Data.Animation[animationNum].Sprite)
        {
            packetWriter.WriteInt32(sprite);
        }

        Network.Send(packetWriter);
    }

    public static void SendRequestEditAnimation()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestEditAnimation);

        Network.Send(packetWriter);
    }

    public static void SendRequestEditJob()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestEditJob);

        Network.Send(packetWriter);
    }

    public static void SendSaveJob(int jobNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CSaveJob);
        packetWriter.WriteInt32(jobNum);
        packetWriter.WriteString(Data.Job[jobNum].Name);
        packetWriter.WriteString(Data.Job[jobNum].Desc);
        packetWriter.WriteInt32(Data.Job[jobNum].MaleSprite);
        packetWriter.WriteInt32(Data.Job[jobNum].FemaleSprite);

        for (var i = 0; i < StatCount; i++)
        {
            packetWriter.WriteInt32(Data.Job[jobNum].Stat[i]);
        }

        for (var i = 0; i < Constant.MaxStartItems; i++)
        {
            packetWriter.WriteInt32(Data.Job[jobNum].StartItem[i]);
            packetWriter.WriteInt32(Data.Job[jobNum].StartValue[i]);
        }

        packetWriter.WriteInt32(Data.Job[jobNum].StartMap);
        packetWriter.WriteByte(Data.Job[jobNum].StartX);
        packetWriter.WriteByte(Data.Job[jobNum].StartY);
        packetWriter.WriteInt32(Data.Job[jobNum].BaseExp);

        Network.Send(packetWriter);
    }

    public static void SendSaveItem(int itemNum)
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CSaveItem);
        packetWriter.WriteInt32(itemNum);
        packetWriter.WriteInt32(Data.Item[itemNum].AccessReq);

        for (var i = 0; i < StatCount; i++)
        {
            packetWriter.WriteInt32(Data.Item[itemNum].AddStat[i]);
        }

        packetWriter.WriteInt32(Data.Item[itemNum].Animation);
        packetWriter.WriteInt32(Data.Item[itemNum].BindType);
        packetWriter.WriteInt32(Data.Item[itemNum].JobReq);
        packetWriter.WriteInt32(Data.Item[itemNum].Data1);
        packetWriter.WriteInt32(Data.Item[itemNum].Data2);
        packetWriter.WriteInt32(Data.Item[itemNum].Data3);
        packetWriter.WriteInt32(Data.Item[itemNum].LevelReq);
        packetWriter.WriteInt32(Data.Item[itemNum].Mastery);
        packetWriter.WriteString(Data.Item[itemNum].Name);
        packetWriter.WriteInt32(Data.Item[itemNum].Paperdoll);
        packetWriter.WriteInt32(Data.Item[itemNum].Icon);
        packetWriter.WriteInt32(Data.Item[itemNum].Price);
        packetWriter.WriteInt32(Data.Item[itemNum].Rarity);
        packetWriter.WriteInt32(Data.Item[itemNum].Speed);

        packetWriter.WriteInt32(Data.Item[itemNum].Stackable);
        packetWriter.WriteString(Data.Item[itemNum].Description);

        for (var i = 0; i < StatCount; i++)
        {
            packetWriter.WriteInt32(Data.Item[itemNum].StatReq[i]);
        }

        packetWriter.WriteInt32(Data.Item[itemNum].Type);
        packetWriter.WriteInt32(Data.Item[itemNum].SubType);

        packetWriter.WriteInt32(Data.Item[itemNum].ItemLevel);

        packetWriter.WriteInt32(Data.Item[itemNum].KnockBack);
        packetWriter.WriteInt32(Data.Item[itemNum].KnockBackTiles);

        packetWriter.WriteInt32(Data.Item[itemNum].Projectile);
        packetWriter.WriteInt32(Data.Item[itemNum].Ammo);

        Network.Send(packetWriter);
    }

    public static void SendRequestEditItem()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestEditItem);

        Network.Send(packetWriter);
    }

    public static void SendCloseEditor()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CCloseEditor);

        Network.Send(packetWriter);
    }

    public static void SendSetHotbarSlot(int type, int newSlot, int oldSlot, int num)
    {
        var packetWriter = new PacketWriter(20);

        packetWriter.WriteEnum(Packets.ClientPackets.CSetHotbarSlot);
        packetWriter.WriteInt32(type);
        packetWriter.WriteInt32(newSlot);
        packetWriter.WriteInt32(oldSlot);
        packetWriter.WriteInt32(num);

        Network.Send(packetWriter);
    }

    public static void SendDeleteHotbar(int slot)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CDeleteHotbarSlot);
        packetWriter.WriteInt32(slot);

        Network.Send(packetWriter);
    }

    public static void SendUseHotbarSlot(int slot)
    {
        switch (Data.Player[GameState.MyIndex].Hotbar[slot].SlotType)
        {
            case (byte) DraggablePartType.Skill:
            {
                Player.PlayerCastSkill(Player.FindSkill(Data.Player[GameState.MyIndex].Hotbar[slot].Slot));
                return;
            }
        }

        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CUseHotbarSlot);
        packetWriter.WriteInt32(slot);

        Network.Send(packetWriter);
    }

    public static void SendLearnSkill(int tmpSkill)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CSkillLearn);
        packetWriter.WriteInt32(tmpSkill);

        Network.Send(packetWriter);
    }

    public static void SendCast(int skillSlot)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CCast);
        packetWriter.WriteInt32(skillSlot);

        Network.Send(packetWriter);

        GameState.SkillBuffer = skillSlot;
        GameState.SkillBufferTimer = General.GetTickCount();
    }

    public static void SendRequestMoral(int moralNum)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestMoral);
        packetWriter.WriteInt32(moralNum);

        Network.Send(packetWriter);
    }

    public static void SendRequestEditMoral()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestEditMoral);

        Network.Send(packetWriter);
    }

    public static void SendSaveMoral(int moralNum)
    {
        ref var moral = ref Data.Moral[moralNum];

        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CSaveMoral);
        packetWriter.WriteInt32(moralNum);
        packetWriter.WriteString(moral.Name);
        packetWriter.WriteByte(moral.Color);
        packetWriter.WriteBoolean(moral.CanCast);
        packetWriter.WriteBoolean(moral.CanPk);
        packetWriter.WriteBoolean(moral.CanDropItem);
        packetWriter.WriteBoolean(moral.CanPickupItem);
        packetWriter.WriteBoolean(moral.CanUseItem);
        packetWriter.WriteBoolean(moral.DropItems);
        packetWriter.WriteBoolean(moral.LoseExp);
        packetWriter.WriteBoolean(moral.PlayerBlock);
        packetWriter.WriteBoolean(moral.NpcBlock);

        Network.Send(packetWriter);
    }

    public static void SendCloseShop()
    {
        var packetWriter = new PacketWriter(4);

        packetWriter.WriteEnum(Packets.ClientPackets.CCloseShop);

        Network.Send(packetWriter);
    }

    public static void SendRequestEditScript(int lineNumber = 0)
    {
        var packetWriter = new PacketWriter(8);

        packetWriter.WriteEnum(Packets.ClientPackets.CRequestEditScript);
        packetWriter.WriteInt32(lineNumber);

        Network.Send(packetWriter);
    }

    public static void SendSaveScript()
    {
        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(Packets.ClientPackets.CSaveScript);
        packetWriter.WriteString(string.Join(Environment.NewLine, Data.Script.Code));

        Network.Send(packetWriter);
    }
}