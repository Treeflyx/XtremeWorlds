using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;

namespace Core.Net;

public ref struct PacketReader(ReadOnlyMemory<byte> memory)
{
    private ReadOnlyMemory<byte> _memory = memory;

    private void EnsureBytesAvailable(int count)
    {
        if (count > _memory.Length)
        {
            throw new EndOfStreamException("Not enough data available");
        }
    }

    public ReadOnlySpan<byte> ReadBlock(int size)
    {
        if (size < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "Size cannot be negative.");
        }

        EnsureBytesAvailable(size);

        var span = _memory[..size];

        _memory = _memory[size..];

        return span.Span;
    }

    public ReadOnlySpan<byte> ReadBytes()
    {
        var length = ReadInt32();
        return length switch
        {
            < 0 => throw new IOException("Invalid negative length received for byte array."),
            0 => ReadOnlySpan<byte>.Empty,
            _ => ReadBlock(length)
        };
    }

    public string ReadString()
    {
        var bytes = ReadBytes();

        return bytes.Length > 0
            ? Encoding.UTF8.GetString(bytes)
            : string.Empty;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private T Read<T>(Func<ReadOnlySpan<byte>, T> converter) where T : allows ref struct
    {
        EnsureBytesAvailable(Unsafe.SizeOf<T>());
        var value = converter(_memory.Span);
        _memory = _memory[Unsafe.SizeOf<T>()..];
        return value;
    }

    public char ReadChar() => BitConverter.ToChar(ReadBlock(sizeof(char)));
    public byte ReadByte() => ReadBlock(sizeof(byte))[0];
    public bool ReadBoolean() => ReadBlock(sizeof(bool))[0] != 0;
    public short ReadInt16() => Read(BinaryPrimitives.ReadInt16LittleEndian);
    public ushort ReadUInt16() => Read(BinaryPrimitives.ReadUInt16LittleEndian);
    public int ReadInt32() => Read(BinaryPrimitives.ReadInt32LittleEndian);
    public uint ReadUInt32() => Read(BinaryPrimitives.ReadUInt32LittleEndian);
    public float ReadSingle() => Read(BinaryPrimitives.ReadSingleLittleEndian);
    public long ReadInt64() => Read(BinaryPrimitives.ReadInt64LittleEndian);
    public ulong ReadUInt64() => Read(BinaryPrimitives.ReadUInt64LittleEndian);
    public double ReadDouble() => Read(BinaryPrimitives.ReadDoubleLittleEndian);
}