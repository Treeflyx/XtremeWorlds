using Core;
using Core.Configurations;
using Core.Net;

namespace Client.Net;

public static class Network
{
    private sealed class NetworkEventHandler : INetworkEventHandler
    {
        private const int BufferSize = 0xFFFF;
    private readonly GamePacketParser _parser = new();
    private byte[] _buffer = new byte[BufferSize];
        private int _bufferOffset;
        
        public Task OnBytesReceivedAsync(ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken)
        {
            // Ensure capacity for incoming bytes (allow dynamic growth beyond initial BufferSize)
            var required = _bufferOffset + bytes.Length;
            if (required > _buffer.Length)
            {
                var newCapacity = Math.Max(required, _buffer.Length * 2);
                Array.Resize(ref _buffer, newCapacity);
            }

            // Append new bytes
            bytes.Span.CopyTo(_buffer.AsSpan(_bufferOffset));
            _bufferOffset += bytes.Length;
            if (_bufferOffset == 0)
            {
                return Task.CompletedTask;
            }

            // Parse as many packets as possible
            var count = _parser.Parse(_buffer.AsMemory(0, _bufferOffset));
            if (count == 0)
            {
                return Task.CompletedTask;
            }

            // Move any leftover bytes to the beginning of the buffer for the next read
            var bytesLeft = _bufferOffset - count;
            if (bytesLeft > 0)
            {
                _buffer.AsSpan(count, bytesLeft).CopyTo(_buffer.AsSpan(0));
            }

            _bufferOffset = bytesLeft;
            return Task.CompletedTask;
        }
    }

    private static readonly NetworkClient Client = new();
    private static readonly NetworkEventHandler EventHandler = new();
    private static readonly CancellationTokenSource CancellationTokenSource = new();

    public static bool IsConnected => Client.Connected;
    
    public static async Task Start()
    {
        await Client.StartAsync(
            SettingsManager.Instance.Ip,
            SettingsManager.Instance.Port,
            EventHandler,
            CancellationTokenSource.Token);
    }

    public static void Stop()
    {
        CancellationTokenSource.Cancel();
    }

    public static void Send(byte[] data)
    {
        Client.Send(data);
    }
    
    public static void Send(PacketWriter data)
    {
        Send(data.GetBytes());
    }
}