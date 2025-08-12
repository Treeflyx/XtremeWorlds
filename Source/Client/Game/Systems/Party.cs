using Client.Game.UI;
using Client.Game.UI.Windows;
using Client.Net;
using Core;
using Core.Globals;
using Core.Net;
using Type = Core.Globals.Type;

namespace Client
{

    public class Party
    {

        #region Database

        public static void ClearParty()
        {
            Data.MyParty = new Type.Party()
            {
                Leader = 0,
                MemberCount = 0
            };
            Data.MyParty.Member = new int[5];
        }

        #endregion

        #region Incoming Packets

        public static void Packet_PartyInvite(ReadOnlyMemory<byte> data)
        {
            string name;
            var buffer = new PacketReader(data);

            name = buffer.ReadString();
            GameLogic.Dialogue("Party Invite", name + " has invited you to a party.", "Would you like to join?", DialogueType.PartyInvite, DialogueStyle.YesNo);
        }

        public static void Packet_PartyUpdate(ReadOnlyMemory<byte> data)
        {
            int i;
            int inParty;
            var buffer = new PacketReader(data);

            inParty = buffer.ReadInt32();

            // exit out if we're not in a party
            if (inParty == -1)
            {
                ClearParty();
                WinParty.Update();
                // exit out early
                return;
            }

            // carry on otherwise
            Data.MyParty.Leader = buffer.ReadInt32();
            for (i = 0; i < Constant.MaxPartyMembers; i++)
                Data.MyParty.Member[i] = buffer.ReadInt32();
            Data.MyParty.MemberCount = buffer.ReadInt32();

            WinParty.Update();
        }

        public static void Packet_PartyVitals(ReadOnlyMemory<byte> data)
        {
            int playerNum;
            var partyindex = -1;
            var buffer = new PacketReader(data);

            // which player?
            playerNum = buffer.ReadInt32();

            // find the party number
            for (int i = 0; i < Constant.MaxPartyMembers; i++)
            {
                if (Data.MyParty.Member[i] == playerNum)
                {
                    partyindex = i;
                }
            }

            // exit out if wrong data
            if (partyindex < 0 | partyindex >= Constant.MaxPartyMembers)
                return;

            // set vitals
            var vitalCount = Enum.GetNames(typeof(Vital)).Length;
            for (int i = 0; i < vitalCount; i++)
                Data.Player[playerNum].Vital[i] = buffer.ReadInt32();

            GameLogic.UpdatePartyBars();
        }

        #endregion

        #region Outgoing Packets

        public static void SendPartyRequest(string name)
        {
            var packetWriter = new PacketWriter();
            
            packetWriter.WriteEnum(Packets.ClientPackets.CRequestParty);
            packetWriter.WriteString(name);

            Network.Send(packetWriter);
        }

        public static void SendAcceptParty()
        {
            var packetWriter = new PacketWriter(4);

            packetWriter.WriteEnum(Packets.ClientPackets.CAcceptParty);

            Network.Send(packetWriter);
        }

        public static void SendDeclineParty()
        {
            var packetWriter = new PacketWriter(4);

            packetWriter.WriteEnum(Packets.ClientPackets.CDeclineParty);

            Network.Send(packetWriter);
        }

        public static void SendLeaveParty()
        {
            var packetWriter = new PacketWriter(4);

            packetWriter.WriteEnum(Packets.ClientPackets.CLeaveParty);

            Network.Send(packetWriter);
        }

        public static void SendPartyChatMsg(string text)
        {
            var packetWriter = new PacketWriter();

            packetWriter.WriteInt32((int)Packets.ClientPackets.CPartyChatMsg);
            packetWriter.WriteString(text);

            Network.Send(packetWriter);
        }

        #endregion

    }
}