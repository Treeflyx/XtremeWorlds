using Core;
using Core.Globals;
using Core.Net;
using Server.Game;
using Server.Game.Net;
using Server.Net;
using static Core.Net.Packets;
using static Core.Globals.Command;

namespace Server
{
    public class Party
    {
        #region Outgoing Packets

        public static void SendDataToParty(int partyNum, byte[] data)
        {
            var loopTo = Data.Party[partyNum].MemberCount;
            for (var i = 0; i < loopTo; i++)
            {
                if (Data.Party[partyNum].Member[i] > 0)
                {
                    PlayerService.Instance.SendDataTo(Data.Party[partyNum].Member[i], data);
                }
            }
        }

        public static void SendPartyInvite(int playerId, int target)
        {
            var packetWriter = new PacketWriter();

            packetWriter.WriteEnum(ServerPackets.SPartyInvite);
            packetWriter.WriteString(Data.Player[target].Name);

            PlayerService.Instance.SendDataTo(playerId, packetWriter.GetBytes());
        }

        public static void SendPartyUpdate(int partyNum)
        {
            var packetWriter = new PacketWriter();

            packetWriter.WriteEnum(ServerPackets.SPartyUpdate);
            packetWriter.WriteInt32(Data.Party[partyNum].Leader == -1 ? 0 : 1);
            packetWriter.WriteInt32(Data.Party[partyNum].Leader);

            for (var i = 0; i < Core.Globals.Constant.MaxPartyMembers; i++)
            {
                packetWriter.WriteInt32(Data.Party[partyNum].Member[i]);
            }

            packetWriter.WriteInt32(Data.Party[partyNum].MemberCount);

            SendDataToParty(partyNum, packetWriter.GetBytes());
        }

        public static void SendPartyUpdateTo(int index)
        {
            var packetWriter = new PacketWriter();

            packetWriter.WriteEnum(ServerPackets.SPartyUpdate);

            var partyNum = Data.TempPlayer[index].InParty;
            if (partyNum >= 0)
            {
                packetWriter.WriteInt32(1);
                packetWriter.WriteInt32(Data.Party[partyNum].Leader);

                for (var i = 0; i < Core.Globals.Constant.MaxPartyMembers; i++)
                {
                    packetWriter.WriteInt32(Data.Party[partyNum].Member[i]);
                }

                packetWriter.WriteInt32(Data.Party[partyNum].MemberCount);
            }
            else
            {
                packetWriter.WriteInt32(0);
            }

            PlayerService.Instance.SendDataTo(index, packetWriter.GetBytes());
        }

        private static readonly int VitalCount = Enum.GetNames<Vital>().Length;

        public static void SendPartyVitals(int partyNum, int playerId)
        {
            var packetWriter = new PacketWriter();

            packetWriter.WriteEnum(ServerPackets.SPartyVitals);
            packetWriter.WriteInt32(playerId);

            for (var i = 0; i < VitalCount; i++)
            {
                packetWriter.WriteInt32(Data.Player[playerId].Vital[i]);
            }

            SendDataToParty(partyNum, packetWriter.GetBytes());
        }

        #endregion

        #region Incoming Packets

        public static void Packet_PartyRquest(GameSession session, ReadOnlyMemory<byte> bytes)
        {
            // Prevent partying with self
            if (Data.TempPlayer[session.Id].Target == session.Id)
                return;

            // make sure it's a valid target
            if (Data.TempPlayer[session.Id].TargetType != (byte) TargetType.Player)
                return;

            // make sure they're connected and on the same map
            if (GetPlayerMap(Data.TempPlayer[session.Id].Target) != GetPlayerMap(session.Id))
                return;

            // init the request
            Invite(session.Id, Data.TempPlayer[session.Id].Target);
        }

        public static void Packet_AcceptParty(GameSession session, ReadOnlyMemory<byte> bytes)
        {
            InviteAccept(Data.TempPlayer[session.Id].PartyInvite, session.Id);
        }

        public static void Packet_DeclineParty(GameSession session, ReadOnlyMemory<byte> bytes)
        {
            InviteDecline(Data.TempPlayer[session.Id].PartyInvite, session.Id);
        }

        public static void Packet_LeaveParty(GameSession session, ReadOnlyMemory<byte> bytes)
        {
            PlayerLeave(session.Id);
        }

        public static void Packet_PartyChatMsg(GameSession session, ReadOnlyMemory<byte> bytes)
        {
            var buffer = new PacketReader(bytes);

            PartyMsg(session.Id, buffer.ReadString());
        }

        #endregion

        public static void ClearParty(int partyNum)
        {
            Data.Party[partyNum].Leader = -1;
            Data.Party[partyNum].MemberCount = 0;
            Data.Party[partyNum].Member = new int[Core.Globals.Constant.MaxPartyMembers];
        }

        public static void PartyMsg(int partyNum, string msg)
        {
            for (var i = 0; i < Core.Globals.Constant.MaxPartyMembers; i++)
            {
                if (Data.Party[partyNum].Member[i] >= 0)
                {
                    NetworkSend.PlayerMsg(Data.Party[partyNum].Member[i], msg, (int) ColorName.BrightBlue);
                }
            }
        }

        private static void RemoveFromParty(int index, int partyNum)
        {
            for (var i = 0; i < Core.Globals.Constant.MaxPartyMembers; i++)
            {
                if (Data.Party[partyNum].Member[i] == index)
                {
                    Data.Party[partyNum].Member[i] = -1;
                    Data.TempPlayer[index].InParty = -1;
                    Data.TempPlayer[index].PartyInvite = -1;
                    break;
                }
            }

            CountMembers(partyNum);
            SendPartyUpdate(partyNum);
            SendPartyUpdateTo(index);
        }

        public static void PlayerLeave(int index)
        {
            int i;

            var partyNum = Data.TempPlayer[index].InParty;

            if (partyNum >= 0)
            {
                // find out how many members we have
                CountMembers(partyNum);

                // make sure there's more than 2 people
                if (Data.Party[partyNum].MemberCount > 2)
                {
                    // check if leader
                    if (Data.Party[partyNum].Leader == index)
                    {
                        // set next person down as leader
                        for (i = 0; i < Core.Globals.Constant.MaxPartyMembers; i++)
                        {
                            if (Data.Party[partyNum].Member[i] >= 0 & Data.Party[partyNum].Member[i] != index)
                            {
                                Data.Party[partyNum].Leader = Data.Party[partyNum].Member[i];
                                PartyMsg(partyNum, string.Format("{0} is now the party leader.", GetPlayerName(i)));
                                break;
                            }
                        }

                        // leave party
                        PartyMsg(partyNum, string.Format("{0} has left the party.", GetPlayerName(index)));
                        RemoveFromParty(index, partyNum);
                    }
                    else
                    {
                        // not the leader, just leave
                        PartyMsg(partyNum, string.Format("{0} has left the party.", GetPlayerName(index)));
                        RemoveFromParty(index, partyNum);
                    }
                }
                else
                {
                    // only 2 people, disband
                    PartyMsg(partyNum, "The party has been disbanded.");

                    // remove leader
                    RemoveFromParty(Data.Party[partyNum].Leader, partyNum);

                    // clear out everyone's party
                    for (i = 0; i < Core.Globals.Constant.MaxPartyMembers; i++)
                    {
                        index = Data.Party[partyNum].Member[i];
                        // player exist?
                        if (index > 0)
                        {
                            RemoveFromParty(index, partyNum);
                        }
                    }

                    // clear out the party itself
                    ClearParty(partyNum);
                }
            }
        }

        public static void Invite(int index, int target)
        {
            // make sure they're not busy
            if (Data.TempPlayer[target].PartyInvite >= 0 | Data.TempPlayer[target].TradeRequest >= 0)
            {
                // they've already got a request for trade/party
                NetworkSend.PlayerMsg(index, "This player is busy.", (int) ColorName.BrightRed);
                return;
            }

            // make syure they're not in a party
            if (Data.TempPlayer[target].InParty >= 0)
            {
                // they're already in a party
                NetworkSend.PlayerMsg(index, "This player is already in a party.", (int) ColorName.BrightRed);
                return;
            }

            // check if we're in a party
            if (Data.TempPlayer[index].InParty >= 0)
            {
                var partyNum = Data.TempPlayer[index].InParty;
                // make sure we're the leader
                if (Data.Party[partyNum].Leader == index)
                {
                    // got a blank slot?
                    var loopTo = Core.Globals.Constant.MaxPartyMembers;
                    for (var i = 0; i < loopTo; i++)
                    {
                        if (Data.Party[partyNum].Member[i] == -1)
                        {
                            // send the invitation
                            SendPartyInvite(target, index);

                            // set the invite target
                            Data.TempPlayer[target].PartyInvite = index;

                            // let them know
                            NetworkSend.PlayerMsg(index, "Party invitation sent.", (int) ColorName.Pink);
                            return;
                        }
                    }

                    // no room
                    NetworkSend.PlayerMsg(index, "Party is full.", (int) ColorName.BrightRed);
                    return;
                }

                // not the leader
                NetworkSend.PlayerMsg(index, "You are not the party leader.", (int) ColorName.BrightRed);
                return;
            }

            // not in a party - doesn't matter!
            SendPartyInvite(target, index);

            // set the invite target
            Data.TempPlayer[target].PartyInvite = index;

            // let them know
            NetworkSend.PlayerMsg(index, "Party invitation sent.", (int) ColorName.Pink);
        }

        public static void InviteAccept(int index, int target)
        {
            var partyNum = 0;
            int i;

            // check if already in a party
            if (Data.TempPlayer[index].InParty >= 0)
            {
                // get the partynumber
                partyNum = Data.TempPlayer[index].InParty;
                // got a blank slot?
                for (i = 0; i < Core.Globals.Constant.MaxPartyMembers; i++)
                {
                    if (Data.Party[partyNum].Member[i] == -1)
                    {
                        // add to the party
                        Data.Party[partyNum].Member[i] = target;

                        // recount party
                        CountMembers(partyNum);

                        // send update to all - including new player
                        SendPartyUpdate(partyNum);
                        SendPartyVitals(partyNum, target);

                        // let everyone know they've joined
                        PartyMsg(partyNum, string.Format("{0} has joined the party.", GetPlayerName(target)));

                        // add them in
                        Data.TempPlayer[target].InParty = (byte) partyNum;
                        return;
                    }
                }

                // no empty slots - let them know
                NetworkSend.PlayerMsg(index, "Party is full.", (int) ColorName.BrightRed);
                NetworkSend.PlayerMsg(target, "Party is full.", (int) ColorName.BrightRed);
                return;
            }

            // not in a party. Create one with the new person.
            for (i = 0; i < Core.Globals.Constant.MaxParty; i++)
            {
                // find blank party
                if (!(Data.Party[i].Leader > -1))
                {
                    partyNum = i;
                    break;
                }
            }

            // create the party
            Data.Party[partyNum].MemberCount = 2;
            Data.Party[partyNum].Leader = index;
            Data.Party[partyNum].Member[0] = index;
            Data.Party[partyNum].Member[1] = target;

            SendPartyUpdate(partyNum);
            SendPartyVitals(partyNum, index);
            SendPartyVitals(partyNum, target);

            // let them know it's created
            PartyMsg(partyNum, "Party created.");
            PartyMsg(partyNum, string.Format("{0} has joined the party.", GetPlayerName(index)));

            // clear the invitation
            Data.TempPlayer[target].PartyInvite = -1;

            // add them to the party
            Data.TempPlayer[index].InParty = (byte) partyNum;
            Data.TempPlayer[target].InParty = (byte) partyNum;
        }

        public static void InviteDecline(int index, int target)
        {
            NetworkSend.PlayerMsg(index, string.Format("{0} has declined to join your party.", GetPlayerName(target)), (int) ColorName.BrightRed);
            NetworkSend.PlayerMsg(target, "You declined to join the party.", (int) ColorName.Yellow);

            // clear the invitation
            Data.TempPlayer[target].PartyInvite = -1;
        }

        public static void CountMembers(int partyNum)
        {
            int i;
            var highindex = 0;

            // find the high index
            for (i = Core.Globals.Constant.MaxPartyMembers - 1; i >= 0; i -= 1)
            {
                if (Data.Party[partyNum].Member[i] >= 0)
                {
                    highindex = i;
                    break;
                }
            }

            // count the members
            for (i = 0; i < Core.Globals.Constant.MaxPartyMembers; i++)
            {
                // we've got a blank member
                if (Data.Party[partyNum].Member[i] == -1)
                {
                    // is it lower than the high index?
                    if (i < highindex)
                    {
                        // move everyone down a slot
                        var loopTo1 = Core.Globals.Constant.MaxPartyMembers - 1;
                        for (var x = i; x < (int) loopTo1; x++)
                        {
                            Data.Party[partyNum].Member[x] = Data.Party[partyNum].Member[x + 1];
                            Data.Party[partyNum].Member[x + 1] = 0;
                        }
                    }
                    else
                    {
                        // not lower - highindex is count
                        Data.Party[partyNum].MemberCount = highindex + 1;
                        return;
                    }
                }

                // check if we've reached the max party members
                if (i == Core.Globals.Constant.MaxPartyMembers - 1)
                {
                    if (highindex == i)
                    {
                        Data.Party[partyNum].MemberCount = Core.Globals.Constant.MaxPartyMembers;
                        return;
                    }
                }
            }

            // if we're here it means that we need to re-count again
            CountMembers(partyNum);
        }

        public static void ShareExp(int partyNum, int exp, int index, int mapNum)
        {
            int expShare;
            int leftOver;
            int i;
            int tmpindex;
            var loseMemberCount = default(byte);

            // check if it's worth sharing
            if (!(exp >= Data.Party[partyNum].MemberCount))
            {
                // no party - keep exp for self
                SetPlayerExp(index, exp);
                return;
            }

            // check members in others maps
            var loopTo = Core.Globals.Constant.MaxPartyMembers;
            for (i = 0; i < loopTo; i++)
            {
                tmpindex = Data.Party[partyNum].Member[i];
                if (tmpindex > -1)
                {
                    if (PlayerService.Instance.IsConnected(tmpindex) & NetworkConfig.IsPlaying(tmpindex))
                    {
                        if (GetPlayerMap(tmpindex) != mapNum)
                        {
                            loseMemberCount = +1;
                        }
                    }
                }
            }

            // find out the equal share
            if (Data.Party[partyNum].MemberCount > 0)
            {
                expShare = exp / (Data.Party[partyNum].MemberCount - loseMemberCount);
                leftOver = exp % (Data.Party[partyNum].MemberCount - loseMemberCount);
            }
            else
            {
                expShare = exp;
                leftOver = 0;
            }

            // loop through and give everyone exp
            var loopTo1 = Core.Globals.Constant.MaxPartyMembers;
            for (i = 0; i < loopTo1; i++)
            {
                tmpindex = Data.Party[partyNum].Member[i];
                // existing member?
                if (tmpindex > -1)
                {
                    // playing?
                    if (PlayerService.Instance.IsConnected(tmpindex) & NetworkConfig.IsPlaying(tmpindex))
                    {
                        if (GetPlayerMap(tmpindex) == mapNum)
                        {
                            // give them their share
                            SetPlayerExp(tmpindex, expShare);
                        }
                    }
                }
            }

            // give the remainder to a random member
            if (!(leftOver == 0))
            {
                tmpindex = Data.Party[partyNum].Member[(int) Math.Round(General.GetRandom.NextDouble(1d, Data.Party[partyNum].MemberCount))];
                // give the exp
                SetPlayerExp(tmpindex, leftOver);
            }
        }

        public static void PartyWarp(int index, int mapNum, int x, int y)
        {
            if (Data.TempPlayer[index].InParty >= 0)
            {
                if (Data.Party[Data.TempPlayer[index].InParty].Leader >= 0)
                {
                    var loopTo = Data.Party[Data.TempPlayer[index].InParty].MemberCount;
                    for (var i = 0; i < loopTo; i++)
                        Player.PlayerWarp(Data.Party[Data.TempPlayer[index].InParty].Member[i], mapNum, x, y, (byte) Direction.Down);
                }
            }
        }

        public static bool IsPlayerInParty(int index)
        {
            bool isPlayerInParty = false;
            if (index < 0 | index >= Core.Globals.Constant.MaxPlayers | !Data.TempPlayer[index].InGame)
                return isPlayerInParty;

            if (Data.TempPlayer[index].InParty >= 0)
                isPlayerInParty = true;
            return isPlayerInParty;
        }

        public static int GetPlayerParty(int index)
        {
            int getPlayerParty = 0;
            if (index < 0 | index >= Core.Globals.Constant.MaxPlayers | !Data.TempPlayer[index].InGame)
                return getPlayerParty;
            getPlayerParty = Data.TempPlayer[index].InParty;
            return getPlayerParty;
        }
    }
}