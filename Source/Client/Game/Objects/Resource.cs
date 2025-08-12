using System;
using System.Drawing;
using Client.Net;
using Core;
using Core.Globals;
using Core.Net;
using Microsoft.VisualBasic.CompilerServices;

namespace Client
{

    public class MapResource
    {

        #region Database

        public static void ClearResource(int index)
        {
            Data.Resource[index] = default;
            Data.Resource[index].Name = "";
            GameState.ResourceLoaded[index] = 0;
        }

        public static void ClearResources()
        {
            Array.Resize(ref Data.Resource, Constant.MaxResources);

            for (int i = 0; i < Constant.MaxResources; i++)
                ClearResource(i);

        }

        public static void StreamResource(int resourceNum)
        {
            if (resourceNum >= 0 && string.IsNullOrEmpty(Data.Resource[resourceNum].Name) && GameState.ResourceLoaded[resourceNum] == 0)
            {
                GameState.ResourceLoaded[resourceNum] = 1;
                SendRequestResource(resourceNum);
            }
        }

        #endregion

        #region Incoming Packets

        public static void Packet_MapResource(ReadOnlyMemory<byte> data)
        {
            int i;
            var buffer = new PacketReader(data);
            GameState.ResourceIndex = buffer.ReadInt32();
            GameState.ResourcesInit = false;

            if (GameState.ResourceIndex > 0)
            {
                Array.Resize(ref Data.MapResource, GameState.ResourceIndex);
                Array.Resize(ref Data.MyMapResource, GameState.ResourceIndex);

                var loopTo = GameState.ResourceIndex;
                for (i = 0; i < loopTo; i++)
                {
                    Data.MyMapResource[i].State = buffer.ReadByte();
                    Data.MyMapResource[i].X = buffer.ReadInt32();
                    Data.MyMapResource[i].Y = buffer.ReadInt32();
                }

                GameState.ResourcesInit = true;
            }
        }

        public static void Packet_UpdateResource(ReadOnlyMemory<byte> data)
        {
            int resourceNum;
            var buffer = new PacketReader(data);
            resourceNum = buffer.ReadInt32();

            Data.Resource[resourceNum].Animation = buffer.ReadInt32();
            Data.Resource[resourceNum].EmptyMessage = buffer.ReadString();
            Data.Resource[resourceNum].ExhaustedImage = buffer.ReadInt32();
            Data.Resource[resourceNum].Health = buffer.ReadInt32();
            Data.Resource[resourceNum].ExpReward = buffer.ReadInt32();
            Data.Resource[resourceNum].ItemReward = buffer.ReadInt32();
            Data.Resource[resourceNum].Name = buffer.ReadString();
            Data.Resource[resourceNum].ResourceImage = buffer.ReadInt32();
            Data.Resource[resourceNum].ResourceType = buffer.ReadInt32();
            Data.Resource[resourceNum].RespawnTime = buffer.ReadInt32();
            Data.Resource[resourceNum].SuccessMessage = buffer.ReadString();
            Data.Resource[resourceNum].LvlRequired = buffer.ReadInt32();
            Data.Resource[resourceNum].ToolRequired = buffer.ReadInt32();
            Data.Resource[resourceNum].Walkthrough = buffer.ReadBoolean();
        }

        #endregion

        #region Outgoing Packets

        public static void SendRequestResource(int resourceNum)
        {
            var packetWriter = new PacketWriter(8);

            packetWriter.WriteInt32((int)Packets.ClientPackets.CRequestResource);
            packetWriter.WriteInt32(resourceNum);
            
            Network.Send(packetWriter);
        }

        #endregion

        #region Drawing

        public static void DrawResource(int resource, int dx, int dy, Rectangle rec)
        {
            int x;
            int y;
            int width;
            int height;

            if (resource < 1 | resource > GameState.NumResources)
                return;

            x = GameLogic.ConvertMapX(dx);
            y = GameLogic.ConvertMapY(dy);
            width = rec.Right - rec.Left;
            height = rec.Bottom - rec.Top;

            if (rec.Width < 0 | rec.Height < 0)
                return;

            string argpath = System.IO.Path.Combine(DataPath.Resources, resource.ToString());
            GameClient.RenderTexture(ref argpath, x, y, rec.X, rec.Y, rec.Width, rec.Height, rec.Width, rec.Height);
        }

        public static void DrawMapResource(int resourceNum)
        {
            int mapResourceNum;
            int resourceState;
            var resourceSprite = default(int);
            var rec = default(Rectangle);
            int x;
            int y;

            if (GameState.GettingMap)
                return;

            if (!GameState.MapData)
                return;

            if (Data.MyMapResource[resourceNum].X > Data.MyMap.MaxX | Data.MyMapResource[resourceNum].Y > Data.MyMap.MaxY)
                return;

            mapResourceNum = Data.MyMap.Tile[Data.MyMapResource[resourceNum].X, Data.MyMapResource[resourceNum].Y].Data1;

            if (mapResourceNum == 0)
                mapResourceNum = Data.MyMap.Tile[Data.MyMapResource[resourceNum].X, Data.MyMapResource[resourceNum].Y].Data1_2;

            StreamResource(mapResourceNum);

            if (Data.Resource[mapResourceNum].ResourceImage == 0)
                return;

            // Get the Resource state
            resourceState = Data.MyMapResource[resourceNum].State;

            if (resourceState == 0) // normal
            {
                resourceSprite = Data.Resource[mapResourceNum].ResourceImage;
            }
            else if (resourceState == 1) // used
            {
                resourceSprite = Data.Resource[mapResourceNum].ExhaustedImage;
            }

            // src rect
            rec.Y = 0;
            rec.Height = GameClient.GetGfxInfo(System.IO.Path.Combine(DataPath.Resources, resourceSprite.ToString())).Height;
            rec.X = 0;
            rec.Width = GameClient.GetGfxInfo(System.IO.Path.Combine(DataPath.Resources, resourceSprite.ToString())).Width;

            // Set base x + y, then the offset due to size
            x = (int)Math.Round(Data.MyMapResource[resourceNum].X * GameState.SizeX - GameClient.GetGfxInfo(System.IO.Path.Combine(DataPath.Resources, resourceSprite.ToString())).Width / 2d + 16d);
            y = Data.MyMapResource[resourceNum].Y * GameState.SizeY - GameClient.GetGfxInfo(System.IO.Path.Combine(DataPath.Resources, resourceSprite.ToString())).Height + 32;

            DrawResource(resourceSprite, x, y, rec);
        }

        #endregion

    }
}