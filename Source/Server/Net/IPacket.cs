using Core.Net;

namespace Server.Net;

public interface IPacket
{
    public void Serialize(PacketWriter writer);
}

public interface IPacket<out TSelf> : IPacket where TSelf : IPacket<TSelf>
{
    public static abstract TSelf Deserialize(PacketReader reader);
}