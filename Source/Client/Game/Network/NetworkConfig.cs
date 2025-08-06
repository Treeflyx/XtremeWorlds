using Client.Net;
using Core;

namespace Client;

public static class NetworkConfig
{
    private sealed class NetworkEventHandler : INetworkEventHandler
    {
        private const int BufferSize = 0x2000;
        private readonly GamePacketParser _parser = new();
        private readonly byte[] _buffer = new byte[BufferSize];
        private int _bufferOffset;

        public Task OnConnectedAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Connection success.");
            Console.WriteLine("Connection established. Starting packet router...");

            return Task.CompletedTask;
        }

        public Task OnDisconnectedAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Connection lost.");

            return Task.CompletedTask;
        }

        public Task OnBytesReceivedAsync(ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken)
        {
            var space = BufferSize - _bufferOffset;
            if (bytes.Length > space)
            {
                throw new InvalidOperationException("Buffer is full");
            }

            bytes.Span.CopyTo(_buffer.AsSpan(_bufferOffset));

            _bufferOffset += bytes.Length;
            if (_bufferOffset == 0)
            {
                return Task.CompletedTask;
            }

            var count = _parser.Parse(_buffer.AsMemory(0, _bufferOffset));
            if (count == 0)
            {
                return Task.CompletedTask;
            }

            var bytesLeft = _bufferOffset - count;
            if (bytesLeft > 0)
            {
                _buffer.AsSpan(_bufferOffset, bytesLeft).CopyTo(_buffer.AsSpan(0));
            }

            _bufferOffset = bytesLeft;
            
            return Task.CompletedTask;
        }
    }

    private static readonly NetworkClient Client = new();
    private static readonly NetworkEventHandler EventHandler = new();
    private static readonly CancellationTokenSource CancellationTokenSource = new();

    public static bool IsConnected => Client.Connected;
    
    public static async Task InitNetwork()
    {
        await Client.StartAsync(
            SettingsManager.Instance.Ip,
            SettingsManager.Instance.Port,
            EventHandler,
            CancellationTokenSource.Token);
    }

    public static void DestroyNetwork()
    {
        CancellationTokenSource.Cancel();
    }

    public static void SendData(byte[] data)
    {
        Client.Send(data);
    }

    public static void SendData(ReadOnlySpan<byte> data)
    {
        Client.Send(data.ToArray());
    }

    public static void SendData(ReadOnlySpan<byte> data, int head)
    {
        if (data.Length < head)
        {
            Console.WriteLine("Invalid data length.");
            return;
        }

        var buffer = new byte[head + 4];
        
        Buffer.BlockCopy(BitConverter.GetBytes(head), 0, buffer, 0, 4);
        Buffer.BlockCopy(data[..head].ToArray(), 0, buffer, 4, head);
        
        SendData(buffer);
    }
    
    private static void Socket_ConnectionFailed()
    {
        Console.WriteLine("Failed to connect to the server. Retrying...");
    }

    private static void Socket_ConnectionLost()
    {
    }

    private static void Socket_CrashReport(string err)
    {
        GameLogic.LogoutGame();
        GameLogic.DialogueAlert((byte) SystemMessage.Crashed);

        var currentDateTime = DateTime.Now;
        string timestampForFileName = currentDateTime.ToString("yyyyMMdd_HHmmss");
        string logFileName = $"{timestampForFileName}.txt";

        Log.Add(err, logFileName);
    }

    private static void Socket_TrafficReceived(int size, ref byte[] data)
    {
        Console.WriteLine("Traffic Received : [Size: " + size + "]");
    }

    private static void Socket_PacketReceived(int size, int header, ref byte[] data)
    {
        Console.WriteLine("Packet Received : [Size: " + size + "| Packet: " + ((Packets.ServerPackets) header).ToString() + "]");
    }
}