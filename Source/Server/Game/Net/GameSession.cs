using System.Security.Cryptography;
using Server.Net;

namespace Server.Game.Net;

public sealed class GameSession(int id, INetworkChannel channel, GameSessionManager sessionManager) : IDisposable
{
    private const int BufferSize = 0xFFFF;

    private readonly GamePacketParser _parser = new();
    private readonly byte[] _buffer = new byte[BufferSize];
    private int _bufferOffset;
    private bool _disposed;

    public int Id { get; } = id;
    public INetworkChannel Channel { get; } = channel;
    public Aes Aes { get; set; } = Aes.Create();

    public byte[] Decrypt(byte[] bytes)
    {
        using var aes = Aes.Create();
        
        aes.Key = Aes.Key;
        aes.IV = Aes.IV;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

        cryptoStream.Write(bytes, 0, bytes.Length);
        cryptoStream.FlushFinalBlock();

        return memoryStream.ToArray();
    }
    
    public void Parse(ReadOnlySpan<byte> bytes)
    {
        var space = BufferSize - _bufferOffset;
        if (bytes.Length > space)
        {
            throw new InvalidOperationException("Buffer is full");
        }

        bytes.CopyTo(_buffer.AsSpan(_bufferOffset));

        _bufferOffset += bytes.Length;
        if (_bufferOffset == 0)
        {
            return;
        }

        var count = _parser.Parse(this, _buffer.AsMemory(0, _bufferOffset));
        if (count == 0)
        {
            return;
        }

        var bytesLeft = _bufferOffset - count;
        if (bytesLeft > 0)
        {
            _buffer.AsSpan(_bufferOffset, bytesLeft).CopyTo(_buffer.AsSpan(0));
        }

        _bufferOffset = bytesLeft;
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, true))
        {
            return;
        }

        Aes.Dispose();
        
        sessionManager.Destroy(this);
    }
}