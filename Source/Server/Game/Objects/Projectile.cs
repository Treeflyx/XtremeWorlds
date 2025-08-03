using System;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Mirage.Sharp.Asfw;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Core.Type;
using static Core.Global.Command;
using static Core.Packets;
using Core;
using Server.Game.Net;
using Server.Net;

namespace Server
{

    public class Projectile
    {

        #region Database
        public static void SaveProjectile(int projectileNum)
        {
            string json = JsonConvert.SerializeObject(Data.Projectile[projectileNum]).ToString();

            if (Database.RowExists(projectileNum, "projectile"))
            {
                Database.UpdateRow(projectileNum, json, "projectile", "data");
            }
            else
            {
                Database.InsertRow(projectileNum, json, "projectile");
            }
        }

        public static async System.Threading.Tasks.Task LoadProjectilesAsync()
        {
            int i;

            var loopTo = Core.Constant.MaxProjectiles;
            for (i = 0; i < loopTo; i++)
                await LoadProjectile(i);
        }

        public static async System.Threading.Tasks.Task LoadProjectile(int projectileNum)
        {
            JObject data;

            data = await Database.SelectRowAsync(projectileNum, "projectile", "data");

            if (data is null)
            {
                ClearProjectile(projectileNum);
                return;
            }

            var projectileData = data.ToObject<Core.Type.Projectile>();
            Core.Data.Projectile[projectileNum] = projectileData;
        }

        public static void ClearMapProjectile(int mapNum, int index)
        {

            Data.MapProjectile[mapNum, index].ProjectileNum = 0;
            Data.MapProjectile[mapNum, index].Owner = 0;
            Data.MapProjectile[mapNum, index].OwnerType = 0;
            Data.MapProjectile[mapNum, index].X = 0;
            Data.MapProjectile[mapNum, index].Y = 0;
            Data.MapProjectile[mapNum, index].Dir = 0;
            Data.MapProjectile[mapNum, index].Timer = 0;

        }

        public static void ClearProjectile(int index)
        {

            Data.Projectile[index].Name = "";
            Data.Projectile[index].Sprite = 0;
            Data.Projectile[index].Range = 0;
            Data.Projectile[index].Speed = 0;
            Data.Projectile[index].Damage = 0;

        }

        #endregion

        #region Incoming

        public static void HandleRequestEditProjectile(GameSession session, ReadOnlySpan<byte> bytes)
        {
            var buffer = new ByteStream(4);

            // Prevent hacking
            if (GetPlayerAccess(session.Id) < (byte)AccessLevel.Developer)
                return;

            string user;

            user = IsEditorLocked(session.Id, (byte)EditorType.Projectile);

            if (!string.IsNullOrEmpty(user))
            {
                NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) Color.BrightRed);
                return;
            }

            SendProjectiles(session.Id);

            Core.Data.TempPlayer[session.Id].Editor = (byte)EditorType.Projectile;

            buffer.WriteInt32((int) ServerPackets.SProjectileEditor);

            NetworkConfig.SendDataTo(session.Id, buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();

        }

        public static void HandleSaveProjectile(GameSession session, ReadOnlySpan<byte> bytes)
        {
            int projectileNum;
            var buffer = new PacketReader(bytes);

            if (GetPlayerAccess(session.Id) < (byte)AccessLevel.Developer)
                return;

            projectileNum = buffer.ReadInt32();

            // Prevent hacking
            if (projectileNum < 0 | projectileNum > Core.Constant.MaxProjectiles)
            {
                return;
            }

            Data.Projectile[projectileNum].Name = buffer.ReadString();
            Data.Projectile[projectileNum].Sprite = buffer.ReadInt32();
            Data.Projectile[projectileNum].Range = (byte)buffer.ReadInt32();
            Data.Projectile[projectileNum].Speed = buffer.ReadInt32();
            Data.Projectile[projectileNum].Damage = buffer.ReadInt32();

            // Save it
            SendUpdateProjectileToAll(projectileNum);
            SaveProjectile(projectileNum);
            Core.Log.Add(GetAccountLogin(session.Id) + " saved Projectile #" + projectileNum + ".", Constant.AdminLog);
        }

        public static void HandleRequestProjectile(GameSession session, ReadOnlySpan<byte> bytes)
        {
            int projectileNum;

            var buffer = new PacketReader(bytes);
            projectileNum = buffer.ReadInt32();

            SendProjectile(session.Id, projectileNum);
        }

        public static void HandleClearProjectile(GameSession session, ReadOnlySpan<byte> bytes)
        {
            int projectileNum;
            int targetindex;
            Core.TargetType targetType;
            int targetZone;
            int mapNum;
            int damage;
            int armor;
            int npcNum;
            var buffer = new PacketReader(bytes);
            projectileNum = buffer.ReadInt32();
            targetindex = buffer.ReadInt32();
            targetType = (Core.TargetType)buffer.ReadInt32();
            targetZone = buffer.ReadInt32();

            mapNum = GetPlayerMap(session.Id);

            ClearMapProjectile(mapNum, projectileNum);

        }

        #endregion

        #region Outgoing

        public static void SendUpdateProjectileToAll(int projectileNum)
        {
            ByteStream buffer;

            buffer = new ByteStream(4);

            buffer.WriteInt32((int) ServerPackets.SUpdateProjectile);
            buffer.WriteInt32(projectileNum);
            buffer.WriteString(Data.Projectile[projectileNum].Name);
            buffer.WriteInt32(Data.Projectile[projectileNum].Sprite);
            buffer.WriteInt32(Data.Projectile[projectileNum].Range);
            buffer.WriteInt32(Data.Projectile[projectileNum].Speed);
            buffer.WriteInt32(Data.Projectile[projectileNum].Damage);

            NetworkConfig.SendDataToAll(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();

        }

        public static void SendUpdateProjectileTo(int index, int projectileNum)
        {
            ByteStream buffer;

            buffer = new ByteStream(4);

            buffer.WriteInt32((int) ServerPackets.SUpdateProjectile);
            buffer.WriteInt32(projectileNum);
            buffer.WriteString(Data.Projectile[projectileNum].Name);
            buffer.WriteInt32(Data.Projectile[projectileNum].Sprite);
            buffer.WriteInt32(Data.Projectile[projectileNum].Range);
            buffer.WriteInt32(Data.Projectile[projectileNum].Speed);
            buffer.WriteInt32(Data.Projectile[projectileNum].Damage);

            NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();

        }

        public static void SendProjectile(int index, int projectileNum)
        {
            SendUpdateProjectileTo(index, projectileNum);
        }

        public static void SendProjectiles(int index)
        {
            var loopTo = Core.Constant.MaxProjectiles;
            for (int i = 0; i < loopTo; i++)
            {
                if (Strings.Len(Data.Projectile[i].Name) > 0)
                {
                    SendUpdateProjectileTo(index, i);
                }
            }

        }

        public static void SendProjectileToMap(int mapNum, int projectileNum)
        {
            ByteStream buffer;

            buffer = new ByteStream(4);
            buffer.WriteInt32((int) ServerPackets.SMapProjectile);

            var withBlock = Data.MapProjectile[mapNum, projectileNum];
            buffer.WriteInt32(projectileNum);
            buffer.WriteInt32(withBlock.ProjectileNum);
            buffer.WriteInt32(withBlock.Owner);
            buffer.WriteByte(withBlock.OwnerType);
            buffer.WriteByte(withBlock.Dir);
            buffer.WriteInt32(withBlock.X);
            buffer.WriteInt32(withBlock.Y);          

            NetworkConfig.SendDataToMap(mapNum, buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();

        }

        #endregion

        #region Functions

        public static void PlayerFireProjectile(int index, int isSkill = 0)
        {
            var projectileSlot = default(int);
            int projectileNum;
            int mapNum;
            int i;

            mapNum = GetPlayerMap(index);

            // Find a free projectile
            var loopTo = Core.Constant.MaxProjectiles;
            for (i = 0; i < loopTo; i++)
            {
                if (Data.MapProjectile[mapNum, i].ProjectileNum == -1) // Free Projectile
                {
                    projectileSlot = i;
                    break;
                }
            }

            // Check for skill, if so then load data acordingly
            if (isSkill > 0)
            {
                projectileNum = Data.Skill[isSkill].Projectile;
            }
            else
            {
                projectileNum = Core.Data.Item[(int)GetPlayerEquipment(index, Equipment.Weapon)].Projectile;
            }

            if (projectileNum == -1)
                return;

            {
                var withBlock = Data.MapProjectile[mapNum, projectileSlot];
                withBlock.ProjectileNum = projectileNum;
                withBlock.Owner = index;
                withBlock.OwnerType = (byte)TargetType.Player;
                withBlock.Dir = GetPlayerDir(index);
                withBlock.X = GetPlayerX(index);
                withBlock.Y = GetPlayerY(index);
                withBlock.Timer = General.GetTimeMs() + 60000;
            }

            SendProjectileToMap(mapNum, projectileSlot);

        }

        public static float Engine_GetAngle(int centerX, int centerY, int targetX, int targetY)
        {
            float engineGetAngleRet = default;
            // ************************************************************
            // Gets the angle between two points in a 2d plane
            // ************************************************************
            float sideA;
            float sideC;
            try
            {

                // Check for horizontal lines (90 or 270 degrees)
                if (centerY == targetY)
                {
                    // Check for going right (90 degrees)
                    if (centerX < targetX)
                    {
                        engineGetAngleRet = 90f;
                    }
                    // Check for going left (270 degrees)
                    else
                    {
                        engineGetAngleRet = 270f;
                    }

                    // Exit the function
                    return engineGetAngleRet;
                }

                // Check for horizontal lines (360 or 180 degrees)
                if (centerX == targetX)
                {
                    // Check for going up (360 degrees)
                    if (centerY > targetY)
                    {
                        engineGetAngleRet = 360f;
                    }

                    // Check for going down (180 degrees)
                    else
                    {
                        engineGetAngleRet = 180f;
                    }

                    // Exit the function
                    return engineGetAngleRet;
                }

                // Calculate Side C
                sideC = (float)Math.Sqrt(Math.Pow(Math.Abs(targetX - centerX), 2d) + Math.Pow(Math.Abs(targetY - centerY), 2d));

                // Side B = CenterY

                // Calculate Side A
                sideA = (float)Math.Sqrt(Math.Pow(Math.Abs(targetX - centerX), 2d) + Math.Pow(targetY, 2d));

                // Calculate the angle
                engineGetAngleRet = (float)((Math.Pow((double)sideA, 2d) - Math.Pow(centerY, 2d) - Math.Pow((double)sideC, 2d)) / (double)(centerY * sideC * -2));
                engineGetAngleRet = (float)((Math.Atan((double)-engineGetAngleRet / Math.Sqrt((double)(-engineGetAngleRet * engineGetAngleRet + 1f))) + 1.5708d) * 57.29583d);

                // If the angle is >180, subtract from 360
                if (targetX < centerX)
                    engineGetAngleRet = 360f - engineGetAngleRet;

                // Exit function

                // Check for error
                return engineGetAngleRet;
            }
            catch
            {


                // Return a 0 saying there was an error
                engineGetAngleRet = 0f;

                return engineGetAngleRet;
            }
        }

        #endregion

    }
}