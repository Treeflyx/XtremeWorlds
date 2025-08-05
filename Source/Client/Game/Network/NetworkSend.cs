using System.Reflection;
using System.Security.Cryptography;
using Core;
using static Core.Global.Command;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Mirage.Sharp.Asfw;

namespace Client
{
    public class NetworkSend
    {
        public static void SendAddChar(string name, int sexNum, int jobNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CAddChar);
            buffer.WriteByte(GameState.CharNum);
            buffer.WriteString(name);
            buffer.WriteInt32(sexNum);
            buffer.WriteInt32(jobNum);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);

            buffer.Dispose();
        }

        public static void SendUseChar(byte slot)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CUseChar);
            buffer.WriteByte(slot);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);

            buffer.Dispose();
        }

        public static void SendDelChar(byte slot)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CDelChar);
            buffer.WriteByte(slot);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);

            buffer.Dispose();
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

        public static void SendLogin(string name, string pass)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CLogin);

            byte[] encryptedName = Encrypt(System.Text.Encoding.UTF8.GetBytes(name));
            byte[] encryptedPass = Encrypt(System.Text.Encoding.UTF8.GetBytes(pass));

            // Get the current executing assembly
            var assembly = Assembly.GetExecutingAssembly();

            // Retrieve the version information
            var version = assembly?.GetName()?.Version;
            byte[] encryptedVersion = Encrypt(System.Text.Encoding.UTF8.GetBytes(version?.ToString()));

            buffer.WriteBytes(encryptedName);
            buffer.WriteBytes(encryptedPass);
            buffer.WriteBytes(encryptedVersion);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);

            buffer.Dispose();
        }

        public static void SendRegister(string name, string pass)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRegister);

            byte[] encryptedName = Encrypt(System.Text.Encoding.UTF8.GetBytes(name));
            byte[] encryptedPass = Encrypt(System.Text.Encoding.UTF8.GetBytes(pass));

            // Get the current executing assembly
            var assembly = Assembly.GetExecutingAssembly();

            // Retrieve the version information
            var version = assembly?.GetName()?.Version;
            byte[] encryptedVersion = Encrypt(System.Text.Encoding.UTF8.GetBytes(version?.ToString()));

            buffer.WriteBytes(encryptedName);
            buffer.WriteBytes(encryptedPass);
            buffer.WriteBytes(encryptedVersion);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);

            buffer.Dispose();
        }

        public static void btnLogin_Click()
        {
            string user;
            string pass;

            {
                var withBlock = Gui.Windows[Gui.GetWindowIndex("winLogin")];
                user = withBlock.Controls[(int) Gui.GetControlIndex("winLogin", "txtUsername")].Text;
                pass = withBlock.Controls[(int) Gui.GetControlIndex("winLogin", "txtPassword")].Text;

                if (NetworkConfig.IsConnected == true)
                {
                    SendLogin(user, pass);
                }
                else
                {
                    GameLogic.Dialogue("Invalid Connection", "Cannot connect to game server.", "Please try again.", (byte) DialogueType.Alert);
                }
            }
        }

        public static void GetPing()
        {
            var buffer = new ByteStream(4);
            GameState.PingStart = General.GetTickCount();

            buffer.WriteInt32((int) Packets.ClientPackets.CCheckPing);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);

            buffer.Dispose();
        }

        public static void SendPlayerMove()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CPlayerMove);
            buffer.WriteByte(GetPlayerDir(GameState.MyIndex));
            buffer.WriteByte(Core.Data.Player[GameState.MyIndex].Moving);
            buffer.WriteInt32(Core.Data.Player[GameState.MyIndex].X);
            buffer.WriteInt32(Core.Data.Player[GameState.MyIndex].Y);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendStopPlayerMove()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CStopPlayerMove);
            buffer.WriteByte(GetPlayerDir(GameState.MyIndex));

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SayMsg(string text)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSayMsg);
            buffer.WriteString(text);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendKick(string name)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CKickPlayer);
            buffer.WriteString(name);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendBan(string name)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CBanPlayer);
            buffer.WriteString(name);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void WarpMeTo(string name)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CWarpMeTo);
            buffer.WriteString(name);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void WarpToMe(string name)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CWarpToMe);
            buffer.WriteString(name);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void WarpTo(int mapNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CWarpTo);
            buffer.WriteInt32(mapNum);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestLevelUp()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestLevelUp);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendSpawnItem(int tmpItem, int tmpAmount)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSpawnItem);
            buffer.WriteInt32(tmpItem);
            buffer.WriteInt32(tmpAmount);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendSetSprite(int spriteNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSetSprite);
            buffer.WriteInt32(spriteNum);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendSetAccess(string name, byte access)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSetAccess);
            buffer.WriteString(name);
            buffer.WriteInt32(access);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendAttack()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CAttack);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendPlayerDir()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CPlayerDir);
            buffer.WriteInt32(GetPlayerDir(GameState.MyIndex));

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestNpc(int npcNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestNpc);
            buffer.WriteDouble(npcNum);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestSkill(int skillNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestSkill);
            buffer.WriteInt32(skillNum);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendTrainStat(byte statNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CTrainStat);
            buffer.WriteInt32(statNum);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestPlayerData()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestPlayerData);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void BroadcastMsg(string text)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CBroadcastMsg);
            buffer.WriteString(text);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void PlayerMsg(string text, string msgTo)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CPlayerMsg);
            buffer.WriteString(msgTo);
            buffer.WriteString(text);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void AdminMsg(string text)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CAdminMsg);
            buffer.WriteString(text);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendWhosOnline()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CWhosOnline);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendPlayerInfo(string name)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CPlayerInfoRequest);
            buffer.WriteString(name);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendMotdChange(string welcome)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSetMotd);
            buffer.WriteString(welcome);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendBanList()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CBanList);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendBanDestroy()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CBanDestroy);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendChangeInvSlots(int oldSlot, int newSlot)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSwapInvSlots);
            buffer.WriteInt32(oldSlot);
            buffer.WriteInt32(newSlot);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendChangeSkillSlots(int oldSlot, int newSlot)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSwapSkillSlots);
            buffer.WriteInt32(oldSlot);
            buffer.WriteInt32(newSlot);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendUseItem(int invNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CUseItem);
            buffer.WriteInt32(invNum);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendDropItem(int invNum, int amount)
        {
            var buffer = new ByteStream(4);

            if (GameState.InBank | GameState.InShop >= 0)
                return;

            // do basic checks
            if (invNum < 0 | invNum > Constant.MaxInv)
                return;

            if (Core.Data.Player[GameState.MyIndex].Inv[invNum].Num < 0 | Core.Data.Player[GameState.MyIndex].Inv[invNum].Num > Constant.MaxItems)
                return;

            if (Core.Data.Item[(int) GetPlayerInv(GameState.MyIndex, invNum)].Type == (byte) ItemCategory.Currency | Core.Data.Item[(int) GetPlayerInv(GameState.MyIndex, invNum)].Stackable == 1)
            {
                if (amount < 0 | amount > Core.Data.Player[GameState.MyIndex].Inv[invNum].Value)
                    return;
            }

            buffer.WriteInt32((int) Packets.ClientPackets.CMapDropItem);
            buffer.WriteInt32(invNum);
            buffer.WriteInt32(amount);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void PlayerSearch(int curX, int curY, byte rClick)
        {
            var buffer = new ByteStream(4);

            if (GameLogic.IsInBounds())
            {
                buffer.WriteInt32((int) Packets.ClientPackets.CSearch);
                buffer.WriteInt32(GameState.CurX);
                buffer.WriteInt32(GameState.CurY);
                buffer.WriteInt32(rClick);
                NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            }

            buffer.Dispose();
        }

        public static void AdminWarp(int x, int y)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CAdminWarp);
            buffer.WriteInt32(x);
            buffer.WriteInt32(y);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendUnequip(int eqNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CUnequip);
            buffer.WriteInt32(eqNum);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void ForgetSkill(int skillSlot)
        {
            var buffer = new ByteStream(4);

            // Check for subscript out of range
            if (skillSlot < 0 | skillSlot > Constant.MaxPlayerSkills)
                return;

            // dont let them forget a skill which is in CD
            if (Core.Data.Player[GameState.MyIndex].Skill[skillSlot].Cd > 0)
            {
                Text.AddText("Cannot forget a skill which is cooling down!", (int) Core.Color.Red);
                return;
            }

            // dont let them forget a skill which is buffered
            if (GameState.SkillBuffer == skillSlot)
            {
                Text.AddText("Cannot forget a skill which you are casting!", (int) Core.Color.Red);
                return;
            }

            if (Core.Data.Player[GameState.MyIndex].Skill[skillSlot].Num >= 0)
            {
                buffer.WriteInt32((int) Packets.ClientPackets.CForgetSkill);
                buffer.WriteInt32(skillSlot);
                NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            }
            else
            {
                Text.AddText("No skill found.", (int) Core.Color.Red);
            }

            buffer.Dispose();
        }

        public static void SendRequestMapReport()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CMapReport);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestAdmin()
        {
            if (GetPlayerAccess(GameState.MyIndex) < (int) AccessLevel.Moderator)
                return;

            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CAdmin);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendUseEmote(int emote)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CEmote);
            buffer.WriteInt32(emote);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestEditResource()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestEditResource);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendSaveResource(int resourceNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSaveResource);

            buffer.WriteInt32(resourceNum);
            buffer.WriteInt32(Data.Resource[resourceNum].Animation);
            buffer.WriteString(Data.Resource[resourceNum].EmptyMessage);
            buffer.WriteInt32(Data.Resource[resourceNum].ExhaustedImage);
            buffer.WriteInt32(Data.Resource[resourceNum].Health);
            buffer.WriteInt32(Data.Resource[resourceNum].ExpReward);
            buffer.WriteInt32(Data.Resource[resourceNum].ItemReward);
            buffer.WriteString(Data.Resource[resourceNum].Name);
            buffer.WriteInt32(Data.Resource[resourceNum].ResourceImage);
            buffer.WriteInt32(Data.Resource[resourceNum].ResourceType);
            buffer.WriteInt32(Data.Resource[resourceNum].RespawnTime);
            buffer.WriteString(Data.Resource[resourceNum].SuccessMessage);
            buffer.WriteInt32(Data.Resource[resourceNum].LvlRequired);
            buffer.WriteInt32(Data.Resource[resourceNum].ToolRequired);
            buffer.WriteInt32(Conversions.ToInteger(Data.Resource[resourceNum].Walkthrough));

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestEditNpc()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestEditNpc);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendSaveNpc(int npcNum)
        {
            var buffer = new ByteStream(4);
            int i;

            buffer.WriteInt32((int) Packets.ClientPackets.CSaveNpc);
            buffer.WriteInt32(npcNum);

            buffer.WriteInt32(Data.Npc[(int) npcNum].Animation);
            buffer.WriteString(Data.Npc[(int) npcNum].AttackSay);
            buffer.WriteByte(Data.Npc[(int) npcNum].Behaviour);

            for (i = 0; i < Constant.MaxDropItems; i++)
            {
                buffer.WriteInt32(Data.Npc[(int) npcNum].DropChance[i]);
                buffer.WriteInt32(Data.Npc[(int) npcNum].DropItem[i]);
                buffer.WriteInt32(Data.Npc[(int) npcNum].DropItemValue[i]);
            }

            buffer.WriteInt32(Data.Npc[(int) npcNum].Exp);
            buffer.WriteByte(Data.Npc[(int) npcNum].Faction);
            buffer.WriteInt32(Data.Npc[(int) npcNum].Hp);
            buffer.WriteString(Data.Npc[(int) npcNum].Name);
            buffer.WriteByte(Data.Npc[(int) npcNum].Range);
            buffer.WriteByte(Data.Npc[(int) npcNum].SpawnTime);
            buffer.WriteInt32(Data.Npc[(int) npcNum].SpawnSecs);
            buffer.WriteInt32(Data.Npc[(int) npcNum].Sprite);

            var statCount = System.Enum.GetValues(typeof(Stat)).Length;
            for (i = 0; i < statCount; i++)
                buffer.WriteByte(Data.Npc[(int) npcNum].Stat[i]);

            for (i = 0; i < Constant.MaxNpcSkills; i++)
                buffer.WriteByte(Data.Npc[(int) npcNum].Skill[i]);

            buffer.WriteInt32(Data.Npc[(int) npcNum].Level);
            buffer.WriteInt32(Data.Npc[(int) npcNum].Damage);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestEditSkill()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestEditSkill);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendSaveSkill(int skillNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSaveSkill);
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

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);

            buffer.Dispose();
        }

        public static void SendSaveShop(int shopNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSaveShop);
            buffer.WriteInt32(shopNum);

            buffer.WriteInt32(Data.Shop[shopNum].BuyRate);
            buffer.WriteString(Data.Shop[shopNum].Name);

            for (int i = 0; i < Constant.MaxTrades; i++)
            {
                buffer.WriteInt32(Data.Shop[shopNum].TradeItem[i].CostItem);
                buffer.WriteInt32(Data.Shop[shopNum].TradeItem[i].CostValue);
                buffer.WriteInt32(Data.Shop[shopNum].TradeItem[i].Item);
                buffer.WriteInt32(Data.Shop[shopNum].TradeItem[i].ItemValue);
            }

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestEditShop()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestEditShop);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendSaveAnimation(int animationNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSaveAnimation);
            buffer.WriteInt32(animationNum);

            for (int i = 0, loopTo = Information.UBound(Data.Animation[animationNum].Frames); i < loopTo; i++)
                buffer.WriteInt32(Data.Animation[animationNum].Frames[i]);

            for (int i = 0, loopTo1 = Information.UBound(Data.Animation[animationNum].LoopCount); i < loopTo1; i++)
                buffer.WriteInt32(Data.Animation[animationNum].LoopCount[i]);

            for (int i = 0, loopTo2 = Information.UBound(Data.Animation[animationNum].LoopTime); i < loopTo2; i++)
                buffer.WriteInt32(Data.Animation[animationNum].LoopTime[i]);

            buffer.WriteString(Data.Animation[animationNum].Name);
            buffer.WriteString(Data.Animation[animationNum].Sound);

            for (int i = 0, loopTo3 = Information.UBound(Data.Animation[animationNum].Sprite); i < loopTo3; i++)
                buffer.WriteInt32(Data.Animation[animationNum].Sprite[i]);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestEditAnimation()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestEditAnimation);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestEditJob()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestEditJob);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendSaveJob(int jobNum)
        {
            int i;
            int q;
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSaveJob);

            buffer.WriteInt32(jobNum);

            buffer.WriteString(Data.Job[jobNum].Name);
            buffer.WriteString(Data.Job[jobNum].Desc);

            buffer.WriteInt32(Data.Job[jobNum].MaleSprite);
            buffer.WriteInt32(Data.Job[jobNum].FemaleSprite);

            int statCount = System.Enum.GetValues(typeof(Stat)).Length;
            for (i = 0; i < statCount; i++)
                buffer.WriteInt32(Data.Job[jobNum].Stat[i]);

            for (q = 0; q < Core.Constant.MaxStartItems; q++)
            {
                buffer.WriteInt32(Data.Job[jobNum].StartItem[q]);
                buffer.WriteInt32(Data.Job[jobNum].StartValue[q]);
            }

            buffer.WriteInt32(Data.Job[jobNum].StartMap);
            buffer.WriteByte(Data.Job[jobNum].StartX);
            buffer.WriteByte(Data.Job[jobNum].StartY);

            buffer.WriteInt32(Data.Job[jobNum].BaseExp);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendSaveItem(int itemNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSaveItem);
            buffer.WriteInt32(itemNum);
            buffer.WriteInt32(Core.Data.Item[itemNum].AccessReq);

            int statCount = System.Enum.GetValues(typeof(Stat)).Length;
            for (int i = 0; i < statCount; i++)
                buffer.WriteInt32(Core.Data.Item[itemNum].AddStat[i]);

            buffer.WriteInt32(Core.Data.Item[itemNum].Animation);
            buffer.WriteInt32(Core.Data.Item[itemNum].BindType);
            buffer.WriteInt32(Core.Data.Item[itemNum].JobReq);
            buffer.WriteInt32(Core.Data.Item[itemNum].Data1);
            buffer.WriteInt32(Core.Data.Item[itemNum].Data2);
            buffer.WriteInt32(Core.Data.Item[itemNum].Data3);
            buffer.WriteInt32(Core.Data.Item[itemNum].LevelReq);
            buffer.WriteInt32(Core.Data.Item[itemNum].Mastery);
            buffer.WriteString(Core.Data.Item[itemNum].Name);
            buffer.WriteInt32(Core.Data.Item[itemNum].Paperdoll);
            buffer.WriteInt32(Core.Data.Item[itemNum].Icon);
            buffer.WriteInt32(Core.Data.Item[itemNum].Price);
            buffer.WriteInt32(Core.Data.Item[itemNum].Rarity);
            buffer.WriteInt32(Core.Data.Item[itemNum].Speed);

            buffer.WriteInt32(Core.Data.Item[itemNum].Stackable);
            buffer.WriteString(Core.Data.Item[itemNum].Description);

            for (int i = 0; i < statCount; i++)
                buffer.WriteInt32(Core.Data.Item[itemNum].StatReq[i]);

            buffer.WriteInt32(Core.Data.Item[itemNum].Type);
            buffer.WriteInt32(Core.Data.Item[itemNum].SubType);

            buffer.WriteInt32(Core.Data.Item[itemNum].ItemLevel);

            buffer.WriteInt32(Core.Data.Item[itemNum].KnockBack);
            buffer.WriteInt32(Core.Data.Item[itemNum].KnockBackTiles);

            buffer.WriteInt32(Core.Data.Item[itemNum].Projectile);
            buffer.WriteInt32(Core.Data.Item[itemNum].Ammo);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestEditItem()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestEditItem);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendCloseEditor()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CCloseEditor);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendSetHotbarSlot(int @type, int newSlot, int oldSlot, int num)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSetHotbarSlot);

            buffer.WriteInt32(type);
            buffer.WriteInt32(newSlot);
            buffer.WriteInt32(oldSlot);
            buffer.WriteInt32(num);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendDeleteHotbar(int slot)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CDeleteHotbarSlot);

            buffer.WriteInt32(slot);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendUseHotbarSlot(int slot)
        {
            switch (Core.Data.Player[GameState.MyIndex].Hotbar[slot].SlotType)
            {
                case (byte) DraggablePartType.Skill:
                {
                    Player.PlayerCastSkill(Player.FindSkill((int) Core.Data.Player[GameState.MyIndex].Hotbar[slot].Slot));
                    return;
                }
            }

            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CUseHotbarSlot);

            buffer.WriteInt32(slot);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendLearnSkill(int tmpSkill)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSkillLearn);
            buffer.WriteInt32(tmpSkill);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendCast(int skillSlot)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CCast);
            buffer.WriteInt32(skillSlot);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();

            GameState.SkillBuffer = skillSlot;
            GameState.SkillBufferTimer = General.GetTickCount();
        }

        public static void SendRequestMoral(int moralNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestMoral);
            buffer.WriteInt32(moralNum);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestEditMoral()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestEditMoral);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendSaveMoral(int moralNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CSaveMoral);
            buffer.WriteInt32(moralNum);

            {
                ref var withBlock = ref Data.Moral[moralNum];
                buffer.WriteString(withBlock.Name);
                buffer.WriteByte(withBlock.Color);
                buffer.WriteBoolean(withBlock.CanCast);
                buffer.WriteBoolean(withBlock.CanPk);
                buffer.WriteBoolean(withBlock.CanDropItem);
                buffer.WriteBoolean(withBlock.CanPickupItem);
                buffer.WriteBoolean(withBlock.CanUseItem);
                buffer.WriteBoolean(withBlock.DropItems);
                buffer.WriteBoolean(withBlock.LoseExp);
                buffer.WriteBoolean(withBlock.PlayerBlock);
                buffer.WriteBoolean(withBlock.NpcBlock);
            }

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendCloseShop()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CCloseShop);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendRequestEditScript()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) Packets.ClientPackets.CRequestEditScript);
            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }
    }
}