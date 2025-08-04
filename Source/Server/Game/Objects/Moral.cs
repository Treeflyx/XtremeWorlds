using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Mirage.Sharp.Asfw;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Game.Net;
using Server.Net;
using static Core.Packets;
using static Core.Type;
using static Core.Global.Command;

namespace Server
{
    public class Moral
    {
        #region Database
        public static void ClearMoral(int moralNum)
        {
            Core.Data.Moral[moralNum].Name = "";
            Core.Data.Moral[moralNum].Color = 0;
            Core.Data.Moral[moralNum].CanCast = false;
            Core.Data.Moral[moralNum].CanDropItem = false;
            Core.Data.Moral[moralNum].CanPk = false;
            Core.Data.Moral[moralNum].CanPickupItem = false;
            Core.Data.Moral[moralNum].CanUseItem = false;
            Core.Data.Moral[moralNum].DropItems = false;
            Core.Data.Moral[moralNum].LoseExp = false;
            Core.Data.Moral[moralNum].NpcBlock = false;
            Core.Data.Moral[moralNum].PlayerBlock = false;
        }

        public static async System.Threading.Tasks.Task LoadMoralAsync(int moralNum)
        {
            JObject data;

            data = await Database.SelectRowAsync(moralNum, "moral", "data");

            if (data is null)
            {
                ClearMoral(moralNum);
                return;
            }

            var moralData = JObject.FromObject(data).ToObject<Core.Type.Moral>();
            Core.Data.Moral[moralNum] = moralData;
        }

        public static async System.Threading.Tasks.Task LoadMoralsAsync()
        {
            int i;

            var loopTo = Core.Constant.MaxMorals;
            for (i = 0; i < loopTo; i++)
                await System.Threading.Tasks.Task.Run(() => LoadMoralAsync(i));
        }

        public static void SaveMoral(int moralNum)
        {
            string json = JsonConvert.SerializeObject(Core.Data.Moral[moralNum]).ToString();

            if (Database.RowExists(moralNum, "moral"))
            {
                Database.UpdateRow(moralNum, json, "moral", "data");
            }
            else
            {
                Database.InsertRow(moralNum, json, "moral");
            }
        }

        public static void SaveMorals()
        {
            int i;

            var loopTo = Core.Constant.MaxMorals;
            for (i = 0; i < loopTo; i++)
                SaveMoral(i);
        }

        public static byte[] MoralData(int moralNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32(moralNum);
            buffer.WriteString(Core.Data.Moral[moralNum].Name);
            buffer.WriteByte(Core.Data.Moral[moralNum].Color);
            buffer.WriteBoolean(Core.Data.Moral[moralNum].NpcBlock);
            buffer.WriteBoolean(Core.Data.Moral[moralNum].PlayerBlock);
            buffer.WriteBoolean(Core.Data.Moral[moralNum].DropItems);
            buffer.WriteBoolean(Core.Data.Moral[moralNum].CanCast);
            buffer.WriteBoolean(Core.Data.Moral[moralNum].CanDropItem);
            buffer.WriteBoolean(Core.Data.Moral[moralNum].CanPickupItem);
            buffer.WriteBoolean(Core.Data.Moral[moralNum].CanPk);
            buffer.WriteBoolean(Core.Data.Moral[moralNum].DropItems);
            buffer.WriteBoolean(Core.Data.Moral[moralNum].LoseExp);

            return buffer.ToArray();
        }

        #endregion

        #region Outgoing Packets

        public static void SendMorals(int index)
        {
            int i;

            var loopTo = Core.Constant.MaxMorals;
            for (i = 0; i < loopTo; i++)
            {
                if (Strings.Len(Core.Data.Moral[i].Name) > 0)
                {
                    SendUpdateMoralTo(index, i);
                }
            }

        }

        public static void SendUpdateMoralTo(int index, int moralNum)
        {
            ByteStream buffer;
            buffer = new ByteStream(4);
            buffer.WriteInt32((int) ServerPackets.SUpdateMoral);

            buffer.WriteBlock(MoralData(moralNum));

            NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendUpdateMoralToAll(int moralNum)
        {
            ByteStream buffer;
            buffer = new ByteStream(4);
            buffer.WriteInt32((int) ServerPackets.SUpdateMoral);

            buffer.WriteBlock(MoralData(moralNum));

            NetworkConfig.SendDataToAll(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }


        #endregion

        #region Incoming Packets
        public static void Packet_RequestEditMoral(GameSession session, ReadOnlySpan<byte> bytes)
        {
            var buffer = new ByteStream(4);

            if (GetPlayerAccess(session.Id) < (byte) Core.AccessLevel.Developer)
                return;

            var user = IsEditorLocked(session.Id, (byte) Core.EditorType.Moral);
            if (!string.IsNullOrEmpty(user))
            {
                NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) Core.Color.BrightRed);
                return;
            }

            SendMorals(session.Id);

            Core.Data.TempPlayer[session.Id].Editor = (byte) Core.EditorType.Moral;

            buffer.WriteInt32((int) ServerPackets.SMoralEditor);
            NetworkConfig.SendDataTo(session.Id, buffer.UnreadData, buffer.WritePosition);

            buffer.Dispose();

        }

        public static void Packet_SaveMoral(GameSession session, ReadOnlySpan<byte> bytes)
        {
            int i;
            var buffer = new PacketReader(bytes);

            // Prevent hacking
            if (GetPlayerAccess(session.Id) < (byte) Core.AccessLevel.Developer)
                return;

            var moralNum = buffer.ReadInt32();

            // Prevent hacking
            if (moralNum < 0 | moralNum > Core.Constant.MaxMorals)
                return;

            {
                ref var withBlock = ref Core.Data.Moral[moralNum];
                withBlock.Name = buffer.ReadString();
                withBlock.Color = buffer.ReadByte();
                withBlock.CanCast = buffer.ReadBoolean();
                withBlock.CanPk = buffer.ReadBoolean();
                withBlock.CanDropItem = buffer.ReadBoolean();
                withBlock.CanPickupItem = buffer.ReadBoolean();
                withBlock.CanUseItem = buffer.ReadBoolean();
                withBlock.DropItems = buffer.ReadBoolean();
                withBlock.LoseExp = buffer.ReadBoolean();
                withBlock.PlayerBlock = buffer.ReadBoolean();
                withBlock.NpcBlock = buffer.ReadBoolean();
            }

            // Save it
            SendUpdateMoralToAll(moralNum);
            SaveMoral(moralNum);
            Core.Log.Add(GetAccountLogin(session.Id) + " saved moral #" + moralNum + ".", Constant.AdminLog);
            SendMorals(session.Id);
        }

        public static void Packet_RequestMoral(GameSession session, ReadOnlySpan<byte> bytes)
        {
            SendMorals(session.Id);
        }
        #endregion
    }
}