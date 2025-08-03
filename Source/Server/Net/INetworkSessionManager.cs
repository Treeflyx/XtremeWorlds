using System.Diagnostics.CodeAnalysis;

namespace Server.Net;

public interface INetworkSessionManager<TSession> where TSession : IDisposable
{
    /// <summary>
    ///     <para>
    ///         Attempts to create a new session for the specified <paramref name="channel"/>.
    ///     </para>
    ///     <para>
    ///         This can fail if the maximum number of sessions has been reached.
    ///     </para>
    /// </summary>
    /// <param name="channel">The channel for which to create the session.</param>
    /// <param name="session">The newly created session.</param>
    /// <returns>True if a new session was created; otherwise, false.</returns>
    bool TryCreate(INetworkChannel channel, [NotNullWhen(true)] out TSession? session);
}