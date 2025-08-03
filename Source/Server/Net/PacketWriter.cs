using System.Buffers.Binary;
using System.Text;

namespace Server.Net;

public sealed class PacketWriter(int capacity = PacketWriter.InitialCapacity)
{
    private const int InitialCapacity = 64;

    private byte[] _buffer = new byte[capacity];
    private int _offset;

    private void EnsureSpaceAvailable(int space)
    {
        var requiredSize = _offset + space;
        if (requiredSize <= _buffer.Length)
        {
            return;
        }

        var desiredCapacity = Math.Max(requiredSize, _buffer.Length * 2);

        Array.Resize(ref _buffer, desiredCapacity);
    }

    public byte[] GetBytes()
    {
        var packet = new byte[4 + _offset];
        
        BinaryPrimitives.WriteInt32LittleEndian(packet.AsSpan(0, 4), _offset);
        if (_offset > 0)
        {
            _buffer.AsSpan(0, _offset).CopyTo(packet.AsSpan(4));
        }

        return packet;
    }
    
    public void WriteRaw(ReadOnlySpan<byte> bytes)
    {
        if (bytes.IsEmpty)
        {
            return;
        }

        EnsureSpaceAvailable(bytes.Length);

        bytes.CopyTo(_buffer.AsSpan(_offset));

        _offset += bytes.Length;
    }

    public void WriteBytes(ReadOnlySpan<byte> value)
    {
        WriteInt32(value.Length);
        WriteRaw(value);
    }

    public void WriteString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            WriteInt32(0);

            return;
        }

        var length = Encoding.UTF8.GetByteCount(value);

        EnsureSpaceAvailable(4 + length);

        WriteInt32(length);

        var bytesWritten = Encoding.UTF8.GetBytes(value, _buffer.AsSpan(_offset));

        _offset += bytesWritten;
    }

    public void WriteChar(char value)
    {
        EnsureSpaceAvailable(sizeof(char));

        BinaryPrimitives.WriteUInt16LittleEndian(_buffer.AsSpan(_offset), value);

        _offset += sizeof(char);
    }

    public void WriteByte(byte value)
    {
        EnsureSpaceAvailable(sizeof(byte));

        _buffer[_offset++] = value;
    }

    public void WriteBoolean(bool value)
    {
        WriteByte((byte) (value ? 1 : 0));
    }

    public void WriteInt16(short value)
    {
        EnsureSpaceAvailable(sizeof(short));
        BinaryPrimitives.WriteInt16LittleEndian(_buffer.AsSpan(_offset), value);
        _offset += sizeof(short);
    }

    public void WriteUInt16(ushort value)
    {
        EnsureSpaceAvailable(sizeof(ushort));
        BinaryPrimitives.WriteUInt16LittleEndian(_buffer.AsSpan(_offset), value);
        _offset += sizeof(ushort);
    }

    public void WriteInt32(int value)
    {
        EnsureSpaceAvailable(sizeof(int));
        BinaryPrimitives.WriteInt32LittleEndian(_buffer.AsSpan(_offset), value);
        _offset += sizeof(int);
    }
    
    public void WriteEnum<T>(T value) where T : Enum
    {
        WriteInt32(Convert.ToInt32(value));
    }

    public void WriteUInt32(uint value)
    {
        EnsureSpaceAvailable(sizeof(uint));
        BinaryPrimitives.WriteUInt32LittleEndian(_buffer.AsSpan(_offset), value);
        _offset += sizeof(uint);
    }

    public void WriteSingle(float value)
    {
        EnsureSpaceAvailable(sizeof(float));
        BinaryPrimitives.WriteSingleLittleEndian(_buffer.AsSpan(_offset), value);
        _offset += sizeof(float);
    }

    public void WriteInt64(long value)
    {
        EnsureSpaceAvailable(sizeof(long));
        BinaryPrimitives.WriteInt64LittleEndian(_buffer.AsSpan(_offset), value);
        _offset += sizeof(long);
    }

    public void WriteUInt64(ulong value)
    {
        EnsureSpaceAvailable(sizeof(ulong));
        BinaryPrimitives.WriteUInt64LittleEndian(_buffer.AsSpan(_offset), value);
        _offset += sizeof(ulong);
    }

    public void WriteDouble(double value)
    {
        EnsureSpaceAvailable(sizeof(double));
        BinaryPrimitives.WriteDoubleLittleEndian(_buffer.AsSpan(_offset), value);
        _offset += sizeof(double);
    }
}