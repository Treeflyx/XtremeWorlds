using System;
using Core;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Mirage.Sharp.Asfw;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Game.Net;
using Server.Net;
using static Core.Global.Command;
using static Core.Packets;

namespace Server
{
    public class Resource
    {
        #region Database

        public static void SaveResource(int resourceNum)
        {
            string json = JsonConvert.SerializeObject(Data.Resource[resourceNum]).ToString();

            if (Database.RowExists(resourceNum, "resource"))
            {
                Database.UpdateRow(resourceNum, json, "resource", "data");
            }
            else
            {
                Database.InsertRow(resourceNum, json, "resource");
            }
        }

        public static async System.Threading.Tasks.Task LoadResourcesAsync()
        {
            var tasks = Enumerable.Range(0, Core.Constant.MaxResources).Select(i => Task.Run(() => LoadResourceAsync(i)));
            await System.Threading.Tasks.Task.WhenAll(tasks);
        }

        public static async System.Threading.Tasks.Task LoadResourceAsync(int resourceNum)
        {
            JObject data;

            data = await Database.SelectRowAsync(resourceNum, "resource", "data");

            if (data is null)
            {
                ClearResource(resourceNum);
                return;
            }

            var resourceData = JObject.FromObject(data).ToObject<Core.Type.Resource>();
            Data.Resource[resourceNum] = resourceData;
        }

        public static void ClearResource(int index)
        {
            Data.Resource[index].Name = "";
            Data.Resource[index].EmptyMessage = "";
            Data.Resource[index].SuccessMessage = "";
        }

        public static void CacheResources(int mapNum)
        {
            int x;
            int y;
            var resourceCount = default(int);

            var loopTo = (int)Data.Map[mapNum].MaxX;
            for (x = 0; x < (int)loopTo; x++)
            {
                var loopTo1 = (int)Data.Map[mapNum].MaxY;
                for (y = 0; y < (int)loopTo1; y++)
                {
                    if (Core.Data.Map[mapNum].Tile[x, y].Type == Core.TileType.Resource || Data.Map[mapNum].Tile[x, y].Type2 == Core.TileType.Resource)
                    {
                        resourceCount += 1;
                        Array.Resize(ref Data.MapResource[mapNum].ResourceData, resourceCount);
                        Data.MapResource[mapNum].ResourceData[resourceCount - 1].X = x;
                        Data.MapResource[mapNum].ResourceData[resourceCount - 1].Y = y;
                        Data.MapResource[mapNum].ResourceData[resourceCount - 1].Health = (byte)Data.Resource[Data.Map[mapNum].Tile[x, y].Data1].Health;
                    }

                }
            }

            Data.MapResource[mapNum].ResourceCount = resourceCount;
        }

        public static byte[] ResourcesData()
        {
            var buffer = new ByteStream(4);
            for (int i = 0, loopTo = Core.Constant.MaxResources; i < loopTo; i++)
            {
                if (!(Strings.Len(Data.Resource[i].Name) > 0))
                    continue;
                buffer.WriteBlock(ResourceData(i));
            }
            return buffer.ToArray();
        }

        public static byte[] ResourceData(int resourceNum)
        {
            var buffer = new ByteStream(4);
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
            return buffer.ToArray();
        }

        #endregion

        #region Gather Skills
        public static void CheckResourceLevelUp(int index, int skillSlot)
        {
            int expRollover;
            int levelCount;

            levelCount = 0;

            if (GetPlayerGatherSkillLvl(index, skillSlot) == Core.Constant.MaxLevel)
                return;

            while (GetPlayerGatherSkillExp(index, skillSlot) >= GetPlayerGatherSkillMaxExp(index, skillSlot))
            {
                expRollover = GetPlayerGatherSkillExp(index, skillSlot) - GetPlayerGatherSkillMaxExp(index, skillSlot);
                SetPlayerGatherSkillLvl(index, skillSlot, GetPlayerGatherSkillLvl(index, skillSlot) + 1);
                SetPlayerGatherSkillExp(index, skillSlot, expRollover);
                SetPlayerGatherSkillMaxExp(index, skillSlot, GetSkillNextLevel(index, skillSlot));
                levelCount =+ 1;
            }

            if (levelCount > 0)
            {
                if (levelCount == 1)
                {
                    // singular
                    NetworkSend.PlayerMsg(index, string.Format("Your {0} has gone up a level!", GetResourceSkillName((Core.ResourceSkill)skillSlot)), (int) Core.Color.BrightGreen);
                }
                else
                {
                    // plural
                    NetworkSend.PlayerMsg(index, string.Format("Your {0} has gone up by {1} levels!", GetResourceSkillName((Core.ResourceSkill)skillSlot), levelCount), (int) Core.Color.BrightGreen);
                }

                NetworkSend.SendPlayerData(index);
            }
        }

        #endregion

        #region Incoming Packets

        public static void Packet_RequestEditResource(GameSession session, ReadOnlySpan<byte> bytes)
        {
            var buffer = new ByteStream(4);

            // Prevent hacking
            if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
                return;

            var user = IsEditorLocked(session.Id, (byte) EditorType.Resource);
            if (!string.IsNullOrEmpty(user))
            {
                NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) Color.BrightRed);
                return;
            }

            Core.Data.TempPlayer[session.Id].Editor = (byte) EditorType.Resource;

            Item.SendItems(session.Id);
            Animation.SendAnimations(session.Id);
            SendResources(session.Id);

            buffer.WriteInt32((int) ServerPackets.SResourceEditor);
            NetworkConfig.SendDataTo(session.Id, buffer.UnreadData, buffer.WritePosition);

            buffer.Dispose();
        }

        public static void Packet_SaveResource(GameSession session, ReadOnlySpan<byte> bytes)
        {
            var buffer = new PacketReader(bytes);

            // Prevent hacking
            if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Developer)
                return;

            var resourcenum = buffer.ReadInt32();

            // Prevent hacking
            if (resourcenum < 0 | resourcenum > Core.Constant.MaxResources)
                return;

            Data.Resource[resourcenum].Animation = buffer.ReadInt32();
            Data.Resource[resourcenum].EmptyMessage = buffer.ReadString();
            Data.Resource[resourcenum].ExhaustedImage = buffer.ReadInt32();
            Data.Resource[resourcenum].Health = buffer.ReadInt32();
            Data.Resource[resourcenum].ExpReward = buffer.ReadInt32();
            Data.Resource[resourcenum].ItemReward = buffer.ReadInt32();
            Data.Resource[resourcenum].Name = buffer.ReadString();
            Data.Resource[resourcenum].ResourceImage = buffer.ReadInt32();
            Data.Resource[resourcenum].ResourceType = buffer.ReadInt32();
            Data.Resource[resourcenum].RespawnTime = buffer.ReadInt32();
            Data.Resource[resourcenum].SuccessMessage = buffer.ReadString();
            Data.Resource[resourcenum].LvlRequired = buffer.ReadInt32();
            Data.Resource[resourcenum].ToolRequired = buffer.ReadInt32();
            Data.Resource[resourcenum].Walkthrough = Conversions.ToBoolean(buffer.ReadInt32());

            // Save it
            SendUpdateResourceToAll(resourcenum);
            SaveResource(resourcenum);

            Core.Log.Add(GetAccountLogin(session.Id) + " saved Resource #" + resourcenum + ".", Constant.AdminLog);
        }

        public static void Packet_RequestResource(GameSession session, ReadOnlySpan<byte> bytes)
        {
            var buffer = new PacketReader(bytes);

            var n = buffer.ReadInt32();

            if (n < 0 | n > Core.Constant.MaxResources)
                return;

            SendUpdateResourceTo(session.Id, n);
        }

        #endregion

        #region Outgoing Packets

        public static void SendMapResourceTo(int index, long resourceNum)
        {
            int i;
            int mapnum;
            var buffer = new ByteStream(4);

            mapnum = GetPlayerMap(index);

            buffer.WriteInt32((int) ServerPackets.SMapResource);
            buffer.WriteInt32(Data.MapResource[mapnum].ResourceCount);

            if (Data.MapResource[mapnum].ResourceCount > 0)
            {           
                var loopTo = Data.MapResource[mapnum].ResourceCount;
                for (i = 0; i < loopTo; i++)
                {
                    buffer.WriteByte(Data.MapResource[mapnum].ResourceData[i].State);
                    buffer.WriteInt32(Data.MapResource[mapnum].ResourceData[i].X);
                    buffer.WriteInt32(Data.MapResource[mapnum].ResourceData[i].Y);
                }

            }

            NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendMapResourceToMap(int mapNum, int resourceNum)
        {
            int i;
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) ServerPackets.SMapResource);
            buffer.WriteInt32(Data.MapResource[mapNum].ResourceCount);

            if (Data.MapResource[mapNum].ResourceCount > 0)
            {

                var loopTo = Data.MapResource[mapNum].ResourceCount;
                for (i = 0; i < loopTo; i++)
                {
                    buffer.WriteByte(Data.MapResource[mapNum].ResourceData[i].State);
                    buffer.WriteInt32(Data.MapResource[mapNum].ResourceData[i].X);
                    buffer.WriteInt32(Data.MapResource[mapNum].ResourceData[i].Y);
                }

            }

            NetworkConfig.SendDataToMap(mapNum, buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendResources(int index)
        {
            var loopTo = Core.Constant.MaxResources;
            for (int i = 0; i < loopTo; i++)
            {
                if (Strings.Len(Data.Resource[i].Name) > 0)
                {
                    SendUpdateResourceTo(index, i);
                }

            }
        }

        public static void SendUpdateResourceTo(int index, int resourceNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) ServerPackets.SUpdateResource);

            buffer.WriteBlock(ResourceData(resourceNum));

            NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendUpdateResourceToAll(int resourceNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) ServerPackets.SUpdateResource);

            buffer.WriteBlock(ResourceData(resourceNum));

            NetworkConfig.SendDataToAll(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        #endregion

        #region Functions

        public static void CheckResource(int index, int x, int y)
        {
            int resourceNum;
            byte resourceType;
            int resourceIndex;
            int rX;
            int rY;
            int damage;
            int mapNum;

            mapNum = GetPlayerMap(index);

            if (x < 0 || y < 0 || x >= Data.MyMap.MaxX || y >= Data.MyMap.MaxY)
                return;

            if (Data.Map[mapNum].Tile[x, y].Type == Core.TileType.Resource | Data.Map[mapNum].Tile[x, y].Type2 == Core.TileType.Resource)
            {
                resourceNum = 0;
                resourceIndex = Data.Map[mapNum].Tile[x, y].Data1;
                resourceType = (byte)Data.Resource[resourceIndex].ResourceType;

                // Get the cache number
                for (int i = 0, loopTo = Data.MapResource[mapNum].ResourceCount; i < loopTo; i++)
                {
                    if (Data.MapResource[mapNum].ResourceData[i].X == x)
                    {
                        if (Data.MapResource[mapNum].ResourceData[i].Y == y)
                        {
                            resourceNum = i;
                        }
                    }
                }

                if (resourceNum >= 0)
                {
                    if (GetPlayerEquipment(index, Core.Equipment.Weapon) >= 0 | Core.Data.Resource[resourceIndex].ToolRequired == 0)
                    {
                        if (Core.Data.Item[GetPlayerEquipment(index, Core.Equipment.Weapon)].Data3 == Core.Data.Resource[resourceIndex].ToolRequired)
                        {

                            // inv space?
                            if (Core.Data.Resource[resourceIndex].ItemReward > 0)
                            {
                                if (Player.FindOpenInvSlot(index, Core.Data.Resource[resourceIndex].ItemReward) == 0)
                                {
                                    NetworkSend.PlayerMsg(index, "You have no inventory space.", (int) Color.Yellow);
                                    return;
                                }
                            }

                            // required lvl?
                            if (Data.Resource[resourceIndex].LvlRequired > GetPlayerGatherSkillLvl(index, resourceType))
                            {
                                NetworkSend.PlayerMsg(index, "Your level is too low!", (int) Color.Yellow);
                                return;
                            }

                            // check if already cut down
                            if (Data.MapResource[mapNum].ResourceData[resourceNum].State == 0)
                            {

                                rX = Data.MapResource[mapNum].ResourceData[resourceNum].X;
                                rY = Data.MapResource[mapNum].ResourceData[resourceNum].Y;

                                if (Data.Resource[resourceIndex].ToolRequired == 0)
                                {
                                    damage = 1 * GetPlayerGatherSkillLvl(index, resourceType);
                                }
                                else
                                {
                                    damage = Core.Data.Item[GetPlayerEquipment(index, Equipment.Weapon)].Data2;
                                }

                                // check if damage is more than health
                                if (damage > 0)
                                {
                                    // cut it down!
                                    if (Data.MapResource[mapNum].ResourceData[resourceNum].Health - damage < 0)
                                    {
                                        Data.MapResource[mapNum].ResourceData[resourceNum].State = 0; // Cut
                                        Data.MapResource[mapNum].ResourceData[resourceNum].Timer = General.GetTimeMs();
                                        SendMapResourceToMap(mapNum, resourceNum);
                                        NetworkSend.SendActionMsg(mapNum, Data.Resource[resourceIndex].SuccessMessage, (int) Color.BrightGreen, 1, GetPlayerX(index) * 32, GetPlayerY(index) * 32);
                                        Player.GiveInv(index, Data.Resource[resourceIndex].ItemReward, 1);
                                        Animation.SendAnimation(mapNum, Data.Resource[resourceIndex].Animation, rX, rY);
                                        SetPlayerGatherSkillExp(index, resourceType, GetPlayerGatherSkillExp(index, resourceType) + Data.Resource[resourceIndex].ExpReward);
                                        // send msg
                                        NetworkSend.PlayerMsg(index, string.Format("Your {0} has earned {1} experience. ({2}/{3})", GetResourceSkillName((ResourceSkill)resourceType), Core.Data.Resource[resourceIndex].ExpReward, GetPlayerGatherSkillExp(index, resourceType), GetPlayerGatherSkillMaxExp(index, resourceType)), (int) Core.Color.BrightGreen);
                                        NetworkSend.SendPlayerData(index);

                                        CheckResourceLevelUp(index, resourceType);
                                    }
                                    else
                                    {
                                        // just do the damage
                                        Data.MapResource[mapNum].ResourceData[resourceNum].Health = (byte)(Data.MapResource[mapNum].ResourceData[resourceNum].Health - damage);
                                        NetworkSend.SendActionMsg(mapNum, "-" + damage, (int) Color.BrightRed, 1, rX * 32, rY * 32);
                                        Animation.SendAnimation(mapNum, Data.Resource[resourceIndex].Animation, rX, rY);
                                    }
                                }
                                else
                                {
                                    // too weak
                                    NetworkSend.SendActionMsg(mapNum, "Miss!", (int) Color.BrightRed, 1, rX * 32, rY * 32);
                                }
                            }
                            else
                            {
                                NetworkSend.SendActionMsg(mapNum, Data.Resource[resourceIndex].EmptyMessage, (int) Color.BrightRed, 1, GetPlayerX(index) * 32, GetPlayerY(index) * 32);
                            }
                        }
                        else
                        {
                            NetworkSend.PlayerMsg(index, "You have the wrong type of tool equiped.", (int) Color.Yellow);
                        }
                    }
                    else
                    {
                        NetworkSend.PlayerMsg(index, "You need a tool to gather this resource.", (int) Color.Yellow);
                    }
                }
            }
        }

        #endregion

    }
}