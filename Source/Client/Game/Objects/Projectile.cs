using System;
using Client.Net;
using Core;
using Core.Globals;
using Core.Net;
using static Core.Globals.Command;
using Microsoft.VisualBasic.CompilerServices;
using Type = Core.Globals.Type;

namespace Client
{
    public class Projectile
    {
        #region Sending

        public static void SendRequestEditProjectiles()
        {
            var packetWriter = new PacketWriter(4);

            packetWriter.WriteEnum(Packets.ClientPackets.CRequestEditProjectile);

            Network.Send(packetWriter);
        }

        public static void SendSaveProjectile(int projectileNum)
        {
            var packetWriter = new PacketWriter();

            packetWriter.WriteEnum(Packets.ClientPackets.CSaveProjectile);
            packetWriter.WriteInt32(projectileNum);
            packetWriter.WriteString(Data.Projectile[projectileNum].Name);
            packetWriter.WriteInt32(Data.Projectile[projectileNum].Sprite);
            packetWriter.WriteInt32(Data.Projectile[projectileNum].Range);
            packetWriter.WriteInt32(Data.Projectile[projectileNum].Speed);
            packetWriter.WriteInt32(Data.Projectile[projectileNum].Damage);

            Network.Send(packetWriter);
        }

        public static void SendRequestProjectile(int projectileNum)
        {
            var packetWriter = new PacketWriter(8);

            packetWriter.WriteEnum(Packets.ClientPackets.CRequestProjectile);
            packetWriter.WriteInt32(projectileNum);

            Network.Send(packetWriter);
        }

        public static void SendClearProjectile(int projectileNum, int collisionindex, byte collisionType, int collisionZone)
        {
            var packetWriter = new PacketWriter(20);

            packetWriter.WriteEnum(Packets.ClientPackets.CClearProjectile);
            packetWriter.WriteInt32(projectileNum);
            packetWriter.WriteInt32(collisionindex);
            packetWriter.WriteInt32(collisionType);
            packetWriter.WriteInt32(collisionZone);

            Network.Send(packetWriter);
        }

        #endregion

        #region Recieving

        public static void HandleUpdateProjectile(ReadOnlyMemory<byte> data)
        {
            int projectileNum;
            var buffer = new PacketReader(data);
            projectileNum = buffer.ReadInt32();

            Data.Projectile[projectileNum].Name = buffer.ReadString();
            Data.Projectile[projectileNum].Sprite = buffer.ReadInt32();
            Data.Projectile[projectileNum].Range = (byte) buffer.ReadInt32();
            Data.Projectile[projectileNum].Speed = buffer.ReadInt32();
            Data.Projectile[projectileNum].Damage = buffer.ReadInt32();
        }

        public static void HandleMapProjectile(ReadOnlyMemory<byte> data)
        {
            int i;
            var buffer = new PacketReader(data);
            i = buffer.ReadInt32();

            {
                ref var withBlock = ref Data.MapProjectile[Data.Player[GameState.MyIndex].Map, i];
                withBlock.ProjectileNum = buffer.ReadInt32();
                withBlock.Owner = buffer.ReadInt32();
                withBlock.OwnerType = buffer.ReadByte();
                withBlock.Dir = buffer.ReadByte();
                withBlock.X = buffer.ReadInt32();
                withBlock.Y = buffer.ReadInt32();
                withBlock.Range = 0;
                withBlock.Timer = General.GetTickCount() + 60000;
            }
        }

        #endregion

        #region Database

        public static void ClearProjectile()
        {
            int i;

            for (i = 0; i < Constant.MaxProjectiles; i++)
                ClearProjectile(i);
        }

        public static void ClearProjectile(int index)
        {
            Data.Projectile[index].Name = "";
            Data.Projectile[index].Sprite = 0;
            Data.Projectile[index].Range = 0;
            Data.Projectile[index].Speed = 0;
            Data.Projectile[index].Damage = 0;
        }

        public static void ClearMapProjectile(int projectileNum)
        {
            Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].ProjectileNum = 0;
            Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Owner = 0;
            Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].OwnerType = 0;
            Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].X = 0;
            Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Y = 0;
            Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Dir = 0;
            Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Timer = 0;
        }

        public static void StreamProjectile(int projectileNum)
        {
            if (projectileNum >= 0 & string.IsNullOrEmpty(Data.Projectile[projectileNum].Name) && GameState.ProjectileLoaded[projectileNum] == 0)
            {
                GameState.ProjectileLoaded[projectileNum] = 1;
                SendRequestProjectile(projectileNum);
            }
        }

        #endregion

        #region Drawing

        public static void DrawProjectile(int projectileNum)
        {
            Type.Rect rec;
            var canClearProjectile = default(bool);
            var collisionindex = default(int);
            var collisionType = default(byte);
            var collisionZone = default(int);
            int xOffset;
            int yOffset;
            int x;
            int y;
            int i;
            int sprite;

            StreamProjectile(projectileNum);

            // check to see if it's time to move the Projectile
            if (General.GetTickCount() > Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].TravelTime)
            {
                switch (Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Dir)
                {
                    case (byte) Direction.Up:
                    {
                        Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Y -= 1;
                        break;
                    }
                    case (byte) Direction.Down:
                    {
                        Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Y += 1;
                        break;
                    }
                    case (byte) Direction.Left:
                    {
                        Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].X -= 1;
                        break;
                    }
                    case (byte) Direction.Right:
                    {
                        Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].X += 1;
                        break;
                    }
                }

                Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].TravelTime = General.GetTickCount() + Data.Projectile[Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].ProjectileNum].Speed;
                Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Range = Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Range + 1;
            }

            x = Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].X;
            y = Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Y;

            // Check if its been going for over 1 minute, if so clear.
            if (Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Timer < General.GetTickCount())
                canClearProjectile = true;

            if (x > Data.MyMap.MaxX | x < 0)
                canClearProjectile = true;

            if (y > Data.MyMap.MaxY | y < 0)
                canClearProjectile = true;

            // Check for blocked wall collision
            if (canClearProjectile == false) // Add a check to prevent crashing
            {
                if (Data.MyMap.Tile[x, y].Type == TileType.Blocked | Data.MyMap.Tile[x, y].Type2 == TileType.Blocked)
                {
                    canClearProjectile = true;
                }
            }

            // Check for Npc collision
            for (i = 0; i < Constant.MaxMapNpcs; i++)
            {
                if (Data.MyMapNpc[i].X == x & Data.MyMapNpc[i].Y == y)
                {
                    canClearProjectile = true;
                    collisionindex = i;
                    collisionType = (byte) TargetType.Npc;
                    collisionZone = -1;
                    break;
                }
            }

            // Check for player collision
            for (i = 0; i < Constant.MaxPlayers; i++)
            {
                if (IsPlaying(i) & GetPlayerMap(i) == GetPlayerMap(GameState.MyIndex))
                {
                    if (GetPlayerX(i) == x & GetPlayerY(i) == y)
                    {
                        canClearProjectile = true;
                        collisionindex = i;
                        collisionType = (byte) TargetType.Player;
                        collisionZone = -1;
                        if (Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].OwnerType == (byte) TargetType.Player)
                        {
                            if (Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Owner == i)
                                canClearProjectile = false; // Reset if its the owner of projectile
                        }

                        break;
                    }
                }
            }

            // Check if it has hit its maximum range
            if (Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Range >= Data.Projectile[Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].ProjectileNum].Range + 1)
                canClearProjectile = true;

            // Clear the projectile if possible
            if (Conversions.ToInteger(canClearProjectile) == 1)
            {
                // Only send the clear to the server if you're the projectile caster or the one hit (only if owner is not a player)
                if (Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].OwnerType == (byte) TargetType.Player & Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Owner == GameState.MyIndex)
                {
                    SendClearProjectile(projectileNum, collisionindex, collisionType, collisionZone);
                }

                ClearMapProjectile(projectileNum);
                return;
            }

            sprite = Data.Projectile[Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].ProjectileNum].Sprite;
            if (sprite < 1 | sprite > GameState.NumProjectiles)
                return;

            // src rect
            rec.Top = 0d;
            var gfxInfo = GameClient.GetGfxInfo(System.IO.Path.Combine(DataPath.Projectiles, sprite.ToString()));
            if (gfxInfo == null)
            {
                return;
            }

            rec.Bottom = gfxInfo.Height;
            rec.Left = Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Dir * GameState.SizeX;
            rec.Right = rec.Left + GameState.SizeX;

            // Find the offset
            switch (Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].Dir)
            {
                case (byte) Direction.Up:
                {
                    yOffset = (int) Math.Round((Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].TravelTime - General.GetTickCount()) / (double) Data.Projectile[Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].ProjectileNum].Speed * GameState.SizeY);
                    break;
                }
                case (byte) Direction.Down:
                {
                    yOffset = (int) Math.Round(-((Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].TravelTime - General.GetTickCount()) / (double) Data.Projectile[Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].ProjectileNum].Speed) * GameState.SizeY);
                    break;
                }
                case (byte) Direction.Left:
                {
                    xOffset = (int) Math.Round((Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].TravelTime - General.GetTickCount()) / (double) Data.Projectile[Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].ProjectileNum].Speed);
                    break;
                }
                case (byte) Direction.Right:
                {
                    xOffset = (int) Math.Round(-((Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].TravelTime - General.GetTickCount()) / (double) Data.Projectile[Data.MapProjectile[Data.Player[GameState.MyIndex].Map, projectileNum].ProjectileNum].Speed));
                    break;
                }
            }

            // Convert coordinates
            x = GameLogic.ConvertMapX(x * 32);
            y = GameLogic.ConvertMapY(y * 32);

            // Render texture
            string argpath = System.IO.Path.Combine(DataPath.Projectiles, sprite.ToString());
            GameClient.RenderTexture(ref argpath, x, y, (int) Math.Round(rec.Left), (int) Math.Round(rec.Top), 32, 32);
        }

        #endregion
    }
}