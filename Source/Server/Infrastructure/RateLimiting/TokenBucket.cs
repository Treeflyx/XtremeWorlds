// Server/Infrastructure/RateLimiting/TokenBucket.cs
using System;
using System.Diagnostics;

namespace XtremeWorlds.Infrastructure.RateLimiting
{
    /// <summary>
    /// Simple token-bucket limiter: capacity tokens max; refills "perSecond".
    /// TryConsume(n) succeeds only if at least n tokens are available.
    /// </summary>
    public sealed class TokenBucket
    {
        private readonly double _capacity;
        private readonly double _perSecond;
        private double _tokens;
        private long _lastTicks;

        public TokenBucket(int capacity, int refillPerSecond)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            if (refillPerSecond < 0) throw new ArgumentOutOfRangeException(nameof(refillPerSecond));

            _capacity = capacity;
            _perSecond = refillPerSecond;
            _tokens = capacity;
            _lastTicks = Stopwatch.GetTimestamp();
        }

        /// <summary>Returns true if the requested amount was consumed.</summary>
        public bool TryConsume(int amount = 1)
        {
            if (amount <= 0) return true;

            var now = Stopwatch.GetTimestamp();
            var dt = (now - _lastTicks) / (double)Stopwatch.Frequency;
            _lastTicks = now;

            // Refill
            _tokens = Math.Min(_capacity, _tokens + dt * _perSecond);

            if (_tokens >= amount)
            {
                _tokens -= amount;
                return true;
            }

            return false;
        }
    }
}
