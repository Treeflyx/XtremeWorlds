using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Server.Net;

public abstract class PacketParser<TPacketId, TSession> where TPacketId : Enum
{
    private const uint CompressionFlag = 1u << 31;

    private readonly Dictionary<int, Action<TSession, ReadOnlyMemory<byte>>> _handlers = [];

    protected void Bind(TPacketId packetId, Action<TSession, ReadOnlyMemory<byte>> handler)
    {
        _handlers[Convert.ToInt32(packetId)] = handler;
    }

    public int Parse(TSession session, ReadOnlyMemory<byte> bytes)
    {
        var totalNumberOfBytes = bytes.Length;

        while (bytes.Length >= 4)
        {
            var packetSize = BitConverter.ToInt32(bytes.Span);
            if (packetSize > bytes.Length - 4)
            {
                break;
            }
            
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

    private void Handle(TSession session, ReadOnlyMemory<byte> bytes)
    {
        if (bytes.Length < 4)
        {
            return;
        }

        var packetId = BitConverter.ToInt32(bytes.Span);
        var packetData = bytes[4..];

        var compressed = IsCompressed(packetId);
        if (compressed)
        {
            packetId = (int) (packetId & ~CompressionFlag);
        }

        if (!Enum.IsDefined(typeof(TPacketId), packetId))
        {
            return;
        }

        if (!_handlers.TryGetValue(packetId, out var handler))
        {
            return;
        }

        if (compressed)
        {
            HandleCompressed(session, packetData, handler);

            return;
        }

        handler(session, packetData);
    }

    private static void HandleCompressed(TSession session, ReadOnlyMemory<byte> bytes, Action<TSession, ReadOnlyMemory<byte>> handler)
    {
        if (bytes.Length < 4)
        {
            return;
        }

        var decompressedSize = BitConverter.ToInt32(bytes.Span);
        if (decompressedSize == 0)
        {
            return;
        }

        var buffer = new byte[decompressedSize];
        if (!Decompress(bytes[4..], buffer))
        {
            return;
        }

        handler(session, buffer.AsMemory());
    }

    private static bool IsCompressed(int packetId)
    {
        return (packetId & CompressionFlag) == CompressionFlag;
    }

    public static bool Decompress(ReadOnlyMemory<byte> src, byte[] dest)
    {
        if (!MemoryMarshal.TryGetArray(src, out var segment) || segment.Array is null)
        {
            return false;
        }

        using var memoryStream = new MemoryStream(segment.Array, segment.Offset, segment.Count);
        using var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);

        int bytesRead, totalBytesRead = 0;
        while ((bytesRead = gzipStream.Read(dest, totalBytesRead, dest.Length - totalBytesRead)) > 0)
        {
            totalBytesRead += bytesRead;
        }

        return totalBytesRead == dest.Length;
    }
}