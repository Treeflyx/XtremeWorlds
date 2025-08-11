using Client.Net;
using Core;
using Core.Net;
using static Core.Global.Command;
using Microsoft.VisualBasic.CompilerServices;

namespace Client
{

    public class Bank
    {

        #region Database

        public static void ClearBanks()
        {
            int i;
            int x;

            for (x = 0; x < Constant.MaxPlayers; x++)
            {
                Core.Data.Bank[x].Item = new Core.Type.PlayerInv[(Constant.MaxBank + 1)];

                for (i = 0; i < Constant.MaxBank; i++)
                {
                    Core.Data.Bank[x].Item[i].Num = -1;
                    Core.Data.Bank[x].Item[i].Value = 0;
                }
            }
        }

        #endregion

        #region Incoming Packets

        public static void Packet_OpenBank(ReadOnlyMemory<byte> data)
        {
            int i;
            var buffer = new PacketReader(data);

            for (i = 0; i < Constant.MaxBank; i++)
            {
                SetBank(GameState.MyIndex, (byte)i, buffer.ReadInt32());
                SetBankValue(GameState.MyIndex, (byte)i, buffer.ReadInt32());
            }

            GameState.InBank = true;

            if (!(Gui.Windows[Gui.GetWindowIndex("winBank")].Visible == true))
            {
                Gui.ShowWindow(Gui.GetWindowIndex("winBank"), resetPosition: false);
            }
        }

        #endregion

        #region Outgoing Packets

        public static void DepositItem(int invslot, int amount)
        {
            var packetWriter = new PacketWriter(12);

            packetWriter.WriteEnum(Packets.ClientPackets.CDepositItem);
            packetWriter.WriteInt32(invslot);
            packetWriter.WriteInt32(amount);

            Network.Send(packetWriter);
        }

        public static void WithdrawItem(byte bankSlot, int amount)
        {
            var packetWriter = new PacketWriter(9);

            packetWriter.WriteEnum(Packets.ClientPackets.CWithdrawItem);
            packetWriter.WriteByte(bankSlot);
            packetWriter.WriteInt32(amount);

            Network.Send(packetWriter);
        }

        public static void ChangeBankSlots(int oldSlot, int newSlot)
        {
            var packetWriter = new PacketWriter(12);

            packetWriter.WriteEnum(Packets.ClientPackets.CChangeBankSlots);
            packetWriter.WriteInt32(oldSlot);
            packetWriter.WriteInt32(newSlot);

            Network.Send(packetWriter);
        }

        public static void CloseBank()
        {
            if (Gui.Windows[Gui.GetWindowIndex("winBank")].Visible == true)
            {
                Gui.HideWindow(Gui.GetWindowIndex("winBank"));
                Gui.HideWindow(Gui.GetWindowIndex("winDescription"));
            }

            var packetWriter = new PacketWriter(4);

            packetWriter.WriteEnum(Packets.ClientPackets.CCloseBank);

            Network.Send(packetWriter);

            GameState.InBank = false;
        }

        #endregion

    }
}