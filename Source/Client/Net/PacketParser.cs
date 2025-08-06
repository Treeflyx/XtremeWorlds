namespace Client.Net;

public abstract class PacketParser<TPacketId> where TPacketId : Enum
{
    private readonly Dictionary<int, Action<ReadOnlyMemory<byte>>> _handlers = [];

    protected void Bind(TPacketId packetId, Action<ReadOnlyMemory<byte>> handler)
    {
        _handlers[Convert.ToInt32(packetId)] = handler;
    }

    public int Parse(ReadOnlyMemory<byte> bytes)
    {
        var totalNumberOfBytes = bytes.Length;

        while (bytes.Length >= 4)
        {
            var packetSize = BitConverter.ToInt32(bytes.Span);

            bytes = bytes[4..];
            if (packetSize == 0)
            {
                continue;
            }

            Handle(bytes[..packetSize]);

            bytes = bytes[packetSize..];
        }

        var bytesLeft = bytes.Length;
        var bytesProcessed = totalNumberOfBytes - bytesLeft;

        return bytesProcessed;
    }

    private void Handle(ReadOnlyMemory<byte> bytes)
    {
        if (bytes.Length < 4)
        {
            return;
        }

        var packetId = BitConverter.ToInt32(bytes.Span);
        var packetData = bytes[4..];

        if (!Enum.IsDefined(typeof(TPacketId), packetId))
        {
            return;
        }

        if (_handlers.TryGetValue(packetId, out var handler))
        {
            handler(packetData);
        }
    }
}