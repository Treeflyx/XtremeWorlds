using System;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Mirage.Sharp.Asfw;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Core.Packets;
using static Core.Type;
using static Core.Global.Command;
using System.Threading.Tasks;
using Server.Game.Net;
using Server.Net;

namespace Server
{
    public class Animation
    {
        #region Database
        public static void SaveAnimation(int animationNum)
        {
            string json = JsonConvert.SerializeObject(Core.Data.Animation[animationNum]).ToString();

            if (Database.RowExists(animationNum, "animation"))
            {
                Database.UpdateRow(animationNum, json, "animation", "data");
            }
            else
            {
                Database.InsertRow(animationNum, json, "animation");
            }
        }

        public static async System.Threading.Tasks.Task LoadAnimationsAsync()
        {
            int i;
            var loopTo = Core.Constant.MaxAnimations - 1;
            for (i = 0; i < loopTo; i++)
                await System.Threading.Tasks.Task.Run(() => LoadAnimation(i));
        }

        public static async System.Threading.Tasks.Task LoadAnimation(int animationNum)
        {
            JObject data;

            data = await Database.SelectRowAsync(animationNum, "animation", "data");

            if (data is null)
            {
                ClearAnimation(animationNum);
                return;
            }

            var animationData = JObject.FromObject(data).ToObject<Core.Type.Animation>();
            Core.Data.Animation[animationNum] = animationData;
        }

        public static void ClearAnimation(int index)
        {
            Core.Data.Animation[index].Name = "";
            Core.Data.Animation[index].Sound = "";
            Core.Data.Animation[index].Sprite = new int[2];
            Core.Data.Animation[index].Frames = new int[2];
            Core.Data.Animation[index].LoopCount = new int[2];
            Core.Data.Animation[index].LoopTime = new int[2];
            Core.Data.Animation[index].LoopCount[0] = 0;
            Core.Data.Animation[index].LoopCount[1] = 0;
            Core.Data.Animation[index].LoopTime[0] = 0;
            Core.Data.Animation[index].LoopTime[1] = 0;
        }

        public static void ClearAnimations()
        {
            for (int i = 0, loopTo = Core.Constant.MaxAnimations; i < loopTo; i++)
                ClearAnimation(i);
        }

        public static byte[] AnimationData(int animationNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32(animationNum);
            for (int i = 0, loopTo = Information.UBound(Core.Data.Animation[animationNum].Frames); i < loopTo; i++)
                buffer.WriteInt32(Core.Data.Animation[animationNum].Frames[i]);

            for (int i = 0, loopTo1 = Information.UBound(Core.Data.Animation[animationNum].LoopCount); i < loopTo1; i++)
                buffer.WriteInt32(Core.Data.Animation[animationNum].LoopCount[i]);

            for (int i = 0, loopTo2 = Information.UBound(Core.Data.Animation[animationNum].LoopTime); i < loopTo2; i++)
                buffer.WriteInt32(Core.Data.Animation[animationNum].LoopTime[i]);

            buffer.WriteString(Core.Data.Animation[animationNum].Name);
            buffer.WriteString(Core.Data.Animation[animationNum].Sound);

            for (int i = 0, loopTo3 = Information.UBound(Core.Data.Animation[animationNum].Sprite); i < loopTo3; i++)
                buffer.WriteInt32(Core.Data.Animation[animationNum].Sprite[i]);

            return buffer.ToArray();
        }

        #endregion

        #region Incoming Packets

        public static void Packet_RequestEditAnimation(GameSession session, ReadOnlySpan<byte> bytes)
        {
            // Prevent hacking
            if (GetPlayerAccess(session.Id) < (byte) Core.AccessLevel.Developer)
                return;

            var user = IsEditorLocked(session.Id, (byte) Core.EditorType.Animation);
            if (!string.IsNullOrEmpty(user))
            {
                NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) Core.Color.BrightRed);
                return;
            }

            Core.Data.TempPlayer[session.Id].Editor = (byte) Core.EditorType.Animation;

            SendAnimations(session.Id);

            var buffer = new ByteStream(4);
            buffer.WriteInt32((int) ServerPackets.SAnimationEditor);
            NetworkConfig.SendDataTo(session.Id, buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void Packet_SaveAnimation(GameSession session, ReadOnlySpan<byte> bytes)
        {
            var buffer = new PacketReader(bytes);

            var animNum = buffer.ReadInt32();

            // Update the Animation
            for (int i = 0, loopTo = Information.UBound(Core.Data.Animation[animNum].Frames); i < loopTo; i++)
                Core.Data.Animation[animNum].Frames[i] = buffer.ReadInt32();

            for (int i = 0, loopTo1 = Information.UBound(Core.Data.Animation[animNum].LoopCount); i < loopTo1; i++)
                Core.Data.Animation[animNum].LoopCount[i] = buffer.ReadInt32();

            for (int i = 0, loopTo2 = Information.UBound(Core.Data.Animation[animNum].LoopTime); i < loopTo2; i++)
                Core.Data.Animation[animNum].LoopTime[i] = buffer.ReadInt32();

            Core.Data.Animation[animNum].Name = buffer.ReadString();
            Core.Data.Animation[animNum].Sound = buffer.ReadString();

            for (int i = 0, loopTo3 = Information.UBound(Core.Data.Animation[animNum].Sprite); i < loopTo3; i++)
                Core.Data.Animation[animNum].Sprite[i] = buffer.ReadInt32();
            
            // Save it
            SaveAnimation(animNum);
            SendUpdateAnimationToAll(animNum);
            Core.Log.Add(GetAccountLogin(session.Id) + " saved Animation #" + animNum + ".", Constant.AdminLog);

        }

        public static void Packet_RequestAnimation(GameSession session, ReadOnlySpan<byte> bytes)
        {
            var buffer = new PacketReader(bytes);

            var n = buffer.ReadInt32();

            if (n < 0 | n > Core.Constant.MaxAnimations)
                return;

            SendUpdateAnimationTo(session.Id, n);
        }

        #endregion

        #region Outgoing Packets

        public static void SendAnimation(int mapNum, int anim, int x, int y, byte lockType = 0, int lockindex = 0)
        {
            var buffer = new ByteStream(4);
            buffer.WriteInt32((int) ServerPackets.SAnimation);
            buffer.WriteInt32(anim);
            buffer.WriteInt32(x);
            buffer.WriteInt32(y);
            buffer.WriteInt32(lockType);
            buffer.WriteInt32(lockindex);

            NetworkConfig.SendDataToMap(mapNum, buffer.UnreadData, buffer.WritePosition);

            buffer.Dispose();
        }

        public static void SendAnimations(int index)
        {
            int i;

            var loopTo = Core.Constant.MaxAnimations - 1;
            for (i = 0; i < loopTo; i++)
            {

                if (Strings.Len(Core.Data.Animation[i].Name) > 0)
                {
                    SendUpdateAnimationTo(index, i);
                }

            }

        }

        public static void SendUpdateAnimationTo(int index, int animationNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) ServerPackets.SUpdateAnimation);

            buffer.WriteBlock(AnimationData(animationNum));

            NetworkConfig.SendDataTo(index, buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        public static void SendUpdateAnimationToAll(int animationNum)
        {
            var buffer = new ByteStream(4);

            buffer.WriteInt32((int) ServerPackets.SUpdateAnimation);

            buffer.WriteBlock(AnimationData(animationNum));

            NetworkConfig.SendDataToAll(buffer.UnreadData, buffer.WritePosition);
            buffer.Dispose();
        }

        #endregion

    }
}