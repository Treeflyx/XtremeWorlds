using Client.Net;
using Core;
using Core.Localization;
using Mirage.Sharp.Asfw;

namespace Client
{

    public class Trade
    {
        public static void CloseTrade()
        {
            InTrade = -1;
            Gui.HideWindow(Gui.GetWindowIndex("winTrade"));
        }

        #region Globals & Type

        public static int InTrade;
        public static int TradeX;
        public static int TradeY;
        public static string TheirWorth;
        public static string YourWorth;

        #endregion

        #region Incoming Packets
        public static void Packet_TradeInvite(ReadOnlyMemory<byte> data)
        {
            int requester;
            var buffer = new PacketReader(data);

            requester = buffer.ReadInt32();
            GameLogic.Dialogue("Trade Invite", string.Format(LocalesManager.Get("Request"), Core.Data.Player[requester].Name), "", (byte)DialogueType.Trade, (byte)DialogueStyle.YesNo);
        }

        public static void Packet_Trade(ReadOnlyMemory<byte> data)
        {
            var buffer = new PacketReader(data);

            InTrade = buffer.ReadInt32();

            GameLogic.ShowTrade();
        }

        public static void Packet_CloseTrade(ReadOnlyMemory<byte> data)
        {
            CloseTrade();
        }

        public static void Packet_TradeUpdate(ReadOnlyMemory<byte> data)
        {
            int datatype;
            var buffer = new PacketReader(data);

            datatype = buffer.ReadInt32();

            if (datatype == 0) // ours!
            {
                for (int i = 0; i < Constant.MaxInv; i++)
                {
                    Data.TradeYourOffer[i].Num = buffer.ReadInt32();
                    Data.TradeYourOffer[i].Value = buffer.ReadInt32();
                }
                YourWorth = buffer.ReadInt32().ToString();
                Gui.Windows[Gui.GetWindowIndex("winTrade")].Controls[(int)Gui.GetControlIndex("winTrade", "lblYourValue")].Text = YourWorth + "g";
            }
            else if (datatype == 1) // theirs
            {
                for (int i = 0; i < Constant.MaxInv; i++)
                {
                    Data.TradeTheirOffer[i].Num = buffer.ReadInt32();
                    Data.TradeTheirOffer[i].Value = buffer.ReadInt32();
                }
                TheirWorth = buffer.ReadInt32().ToString();
                Gui.Windows[Gui.GetWindowIndex("winTrade")].Controls[(int)Gui.GetControlIndex("winTrade", "lblTheirValue")].Text = TheirWorth + "g";
            }
        }

        public static void Packet_TradeStatus(ReadOnlyMemory<byte> data)
        {
            int tradestatus;
            var buffer = new PacketReader(data);

            tradestatus = buffer.ReadInt32();

            switch (tradestatus)
            {
                case 0: // clear
                    {
                        Gui.Windows[Gui.GetWindowIndex("winTrade")].Controls[(int)Gui.GetControlIndex("winTrade", "lblStatus")].Text = "Choose items to offer.";
                        break;
                    }
                case 1: // they've accepted
                    {
                        Gui.Windows[Gui.GetWindowIndex("winTrade")].Controls[(int)Gui.GetControlIndex("winTrade", "lblStatus")].Text = "Other player has accepted.";
                        break;
                    }
                case 2: // you've accepted
                    {
                        Gui.Windows[Gui.GetWindowIndex("winTrade")].Controls[(int)Gui.GetControlIndex("winTrade", "lblStatus")].Text = "Waiting for other player to accept.";
                        break;
                    }
                case 3: // no room
                    {
                        Gui.Windows[Gui.GetWindowIndex("winTrade")].Controls[(int)Gui.GetControlIndex("winTrade", "lblStatus")].Text = "Not enough inventory space.";
                        break;
                    }
            }
        }

        #endregion

        #region Outgoing Packets

        public static void SendAcceptTrade()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int)Packets.ClientPackets.CAcceptTrade);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendDeclineTrade()
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int)Packets.ClientPackets.CDeclineTrade);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendTradeRequest(string name)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int)Packets.ClientPackets.CTradeInvite);
            buffer.WriteString(name);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();

        }

        public static void SendHandleTradeInvite(byte answer)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int)Packets.ClientPackets.CHandleTradeInvite);
            buffer.WriteInt32(answer);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();

        }

        public static void TradeItem(int invslot, int amount)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int)Packets.ClientPackets.CTradeItem);
            buffer.WriteInt32(invslot);
            buffer.WriteInt32(amount);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void UntradeItem(int invslot)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int)Packets.ClientPackets.CUntradeItem);
            buffer.WriteInt32(invslot);

            NetworkConfig.SendData(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        #endregion

    }
}