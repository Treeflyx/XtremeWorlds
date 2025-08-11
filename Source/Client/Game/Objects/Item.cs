using Client.Net;
using Core;
using Core.Globals;
using Core.Net;
using Microsoft.VisualBasic.CompilerServices;
using Type = Core.Globals.Type;

namespace Client
{

    public class Item
    {

        #region Database
        public static void ClearItem(int index)
        {   
            Data.Item[index] = default;

            var statCount = Enum.GetNames(typeof(Stat)).Length;
            Data.Item[index].AddStat = new byte[statCount];
            Data.Item[index].StatReq = new byte[statCount];          

            Data.Item[index].Name = "";
            Data.Item[index].Description = "";
            GameState.ItemLoaded[index] = 0;
        }

        public static void ClearItems()
        {
            int i;

            Data.Item = new Type.Item[Constant.MaxItems];

            for (i = 0; i < Constant.MaxItems; i++)
                ClearItem(i);

        }

        public static void ClearChangedItem()
        {
            GameState.ItemChanged = new bool[Constant.MaxItems];
        }

        public static void StreamItem(int itemNum)
        {
            if (itemNum >= 0 && string.IsNullOrEmpty(Data.Item[itemNum].Name) && GameState.ItemLoaded[itemNum] == 0)
            {
                GameState.ItemLoaded[itemNum] = 1;
                SendRequestItem(itemNum);
            }
        }

        #endregion

        #region Incoming Packets

        public static void Packet_UpdateItem(ReadOnlyMemory<byte> data)
        {
            int n;
            int i;
            var buffer = new PacketReader(data);

            n = buffer.ReadInt32();

            // Update the item
            Data.Item[n].AccessReq = buffer.ReadInt32();

            int statCount = System.Enum.GetValues(typeof(Stat)).Length;
            for (i = 0; i < statCount; i++)
                Data.Item[n].AddStat[i] = (byte)buffer.ReadInt32();

            Data.Item[n].Animation = buffer.ReadInt32();
            Data.Item[n].BindType = (byte)buffer.ReadInt32();
            Data.Item[n].JobReq = buffer.ReadInt32();
            Data.Item[n].Data1 = buffer.ReadInt32();
            Data.Item[n].Data2 = buffer.ReadInt32();
            Data.Item[n].Data3 = buffer.ReadInt32();
            Data.Item[n].LevelReq = buffer.ReadInt32();
            Data.Item[n].Mastery = (byte)buffer.ReadInt32();
            Data.Item[n].Name = buffer.ReadString();
            Data.Item[n].Paperdoll = buffer.ReadInt32();
            Data.Item[n].Icon = buffer.ReadInt32();
            Data.Item[n].Price = buffer.ReadInt32();
            Data.Item[n].Rarity = (byte)buffer.ReadInt32();
            Data.Item[n].Speed = buffer.ReadInt32();

            Data.Item[n].Stackable = (byte)buffer.ReadInt32();
            Data.Item[n].Description = buffer.ReadString();

            for (i = 0; i < statCount; i++)
                Data.Item[n].StatReq[i] = (byte)buffer.ReadInt32();

            Data.Item[n].Type = (byte)buffer.ReadInt32();
            Data.Item[n].SubType = (byte)buffer.ReadInt32();

            Data.Item[n].KnockBack = (byte)buffer.ReadInt32();
            Data.Item[n].KnockBackTiles = (byte)buffer.ReadInt32();

            Data.Item[n].Projectile = buffer.ReadInt32();
            Data.Item[n].Ammo = buffer.ReadInt32();

            if (n == GameState.DescLastItem)
            {
                GameState.DescLastType = 0;
                GameState.DescLastItem = 0L;
            }
        }

        #endregion

        #region Outgoing Packets

        public static void SendRequestItem(int itemNum)
        {
            var packetWriter = new PacketWriter(8);

            packetWriter.WriteEnum(Packets.ClientPackets.CRequestItem);
            packetWriter.WriteInt32(itemNum);

            Network.Send(packetWriter);
        }

        #endregion

    }
}