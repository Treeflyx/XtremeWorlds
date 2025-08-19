// Server/Infrastructure/RateLimiting/PerClientRateLimiter.cs
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace XtremeWorlds.Infrastructure.RateLimiting
{
    public enum PacketKind
    {
        Chat,
        Move,
        JoinMap,
        UseItem,
        Attack,
        // add more kinds as needed
    }

    /// <summary>
    /// Keeps a token bucket per (sessionId, packetKind).
    /// </summary>
    public sealed class PerClientRateLimiter
    {
        private readonly ConcurrentDictionary<(Guid, PacketKind), TokenBucket> _buckets = new();
        private readonly IReadOnlyDictionary<PacketKind, (int capacity, int perSecond)> _rules;
        private readonly (int capacity, int perSecond) _defaultRule;

        public PerClientRateLimiter(
            IReadOnlyDictionary<PacketKind, (int capacity, int perSecond)> rules,
            (int capacity, int perSecond)? defaultRule = null)
        {
            _rules = rules ?? throw new ArgumentNullException(nameof(rules));
            _defaultRule = defaultRule ?? (10, 10); // default 10 tokens, 10/s
        }

        public bool Allow(Guid sessionId, PacketKind kind, int cost = 1)
        {
            var rule = _rules.TryGetValue(kind, out var r) ? r : _defaultRule;
            var bucket = _buckets.GetOrAdd((sessionId, kind), _ => new TokenBucket(rule.capacity, rule.perSecond));
            return bucket.TryConsume(cost);
        }

        public void RemoveSession(Guid sessionId)
        {
            foreach (var key in _buckets.Keys)
            {
                if (key.Item1 == sessionId)
                {
                    _buckets.TryRemove(key, out _);
                }
            }
        }
    }
}
