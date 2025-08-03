namespace Server.Net;

public interface INetworkChannel
{
    /// <summary>
    /// Gets the IP address of the remote client.
    /// </summary>
    string IpAddress { get; }
    
    /// <summary>
    /// Sends the specified bytes to the remote client.
    /// </summary>
    /// <param name="bytes">The bytes to send.</param>
    void Send(byte[] bytes);
    
    /// <summary>
    /// Sends the specified packet to the remote client.
    /// </summary>
    /// <param name="packet">The packet to send.</param>
    /// <typeparam name="TPacket">The packet type.</typeparam>
    void Send<TPacket>(TPacket packet) where TPacket : IPacket;
    
    /// <summary>
    /// Closes the channel.
    /// </summary>
    void Close();
}