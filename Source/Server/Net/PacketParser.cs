namespace Server.Net;

public abstract class PacketParser<TPacketId, TSession> where TPacketId : Enum
{
    private readonly Dictionary<int, Action<TSession, ReadOnlySpan<byte>>> _handlers = [];

    protected void Bind(TPacketId packetId, Action<TSession, ReadOnlySpan<byte>> handler)
    {
        _handlers[Convert.ToInt32(packetId)] = handler;
    }

    public int Parse(TSession session, ReadOnlySpan<byte> bytes)
    {
        var totalNumberOfBytes = bytes.Length;

        while (bytes.Length >= 4)
        {
            var packetSize = BitConverter.ToInt32(bytes);

            bytes = bytes[4..];
            if (packetSize == 0)
            {
                continue;
            }

            Handle(session, bytes[..packetSize]);

            bytes = bytes[packetSize..];
        }

        var bytesLeft = bytes.Length;
        var bytesProcessed = totalNumberOfBytes - bytesLeft;

        return bytesProcessed;
    }

    private void Handle(TSession session, ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length < 4)
        {
            return;
        }

        var packetId = BitConverter.ToInt32(bytes);
        var packetData = bytes[4..];

        if (!Enum.IsDefined(typeof(TPacketId), packetId))
        {
            return;
        }

        if (_handlers.TryGetValue(packetId, out var handler))
        {
            handler(session, packetData);
        }
    }
}