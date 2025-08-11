using Core.Net;
using Server.Net;

namespace Server.Game.Net.Protocol;

public sealed record AesPacket(byte[] Key, byte[] Iv) : IPacket
{
    public void Serialize(PacketWriter writer)
    {
        writer.WriteEnum(Packets.ServerPackets.SAes);
        writer.WriteByte((byte) Key.Length);
        writer.WriteRaw(Key);
        writer.WriteByte((byte) Iv.Length);
        writer.WriteRaw(Iv);
    }
}