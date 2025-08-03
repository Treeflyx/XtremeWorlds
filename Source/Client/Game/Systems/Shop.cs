using Core;
using Microsoft.VisualBasic.CompilerServices;
using Mirage.Sharp.Asfw;

namespace Client
{

    public class Shop
    {
        public static void CloseShop()
        {
            NetworkSend.SendCloseShop();
            Gui.HideWindow(Gui.GetWindowIndex("winShop"));
            Gui.HideWindow(Gui.GetWindowIndex("winDescription"));
            GameState.ShopSelectedSlot = 0L;
            GameState.ShopSelectedItem = 0L;
            GameState.ShopIsSelling = false;
            GameState.InShop = -1;
        }

        #region Database

        public static void ClearShop(int index)
        {
            Data.Shop[index] = default;
            Data.Shop[index].Name = "";
            Data.Shop[index].TradeItem = new Core.Type.TradeItem[Constant.MaxTrades];
            for (int x = 0; x < Constant.MaxTrades; x++)
            {            
                Data.Shop[index].TradeItem[x].Item = -1;
                Data.Shop[index].TradeItem[x].CostItem = - 1;
            }
            GameState.ShopLoaded[index] = 0;
        }

        public static void ClearShops()
        {
            int i;

            Data.Shop = new Core.Type.Shop[Constant.MaxShops];

            for (i = 0; i < Constant.MaxShops; i++)
                ClearShop(i);

        }

        public static void StreamShop(int shopNum)
        {
            if (shopNum >= 0 && string.IsNullOrEmpty(Data.Shop[shopNum].Name) && GameState.ShopLoaded[shopNum] == 0)
            {
                GameState.ShopLoaded[shopNum] = 1;
                SendRequestShop(shopNum);
            }
        }

        #endregion

        #region Incoming Packets

        public static void Packet_OpenShop(ref byte[] data)
        {
            int shopnum;
            var buffer = new ByteStream(data);

            shopnum = buffer.ReadInt32();

            GameLogic.OpenShop(shopnum);

            buffer.Dispose();
        }

        public static void Packet_ResetShopAction(ref byte[] data)
        {
            GameState.ShopAction = 0;
        }

        public static void Packet_UpdateShop(ref byte[] data)
        {
            int shopnum;
            var buffer = new ByteStream(data);
            shopnum = buffer.ReadInt32();

            Data.Shop[shopnum].BuyRate = buffer.ReadInt32();
            Data.Shop[shopnum].Name = buffer.ReadString();

            for (int i = 0; i < Constant.MaxTrades; i++)
            {
                Data.Shop[shopnum].TradeItem[i].CostItem = buffer.ReadInt32();
                Data.Shop[shopnum].TradeItem[i].CostValue = buffer.ReadInt32();
                Data.Shop[shopnum].TradeItem[i].Item = buffer.ReadInt32();
                Data.Shop[shopnum].TradeItem[i].ItemValue = buffer.ReadInt32();
            }

            if (Data.Shop[shopnum].Name is null)
                Data.Shop[shopnum].Name = "";

            buffer.Dispose();
        }

        #endregion

        #region Outgoing Packets

        public static void SendRequestShop(int shopNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int)Packets.ClientPackets.CRequestShop);
            buffer.WriteInt32(shopNum);

            NetworkConfig.Socket.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void BuyItem(int shopSlot)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int)Packets.ClientPackets.CBuyItem);
            buffer.WriteInt32(shopSlot);

            NetworkConfig.Socket.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SellItem(int invslot)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int)Packets.ClientPackets.CSellItem);
            buffer.WriteInt32(invslot);

            NetworkConfig.Socket.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        #endregion

    }
}