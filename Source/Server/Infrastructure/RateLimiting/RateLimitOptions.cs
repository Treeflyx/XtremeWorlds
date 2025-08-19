// Server/Configuration/RateLimitOptions.cs
using System.Collections.Generic;
using XtremeWorlds.Infrastructure.RateLimiting;

namespace XtremeWorlds.Configuration
{
    public sealed class RateLimitRule
    {
        public int Capacity { get; set; } = 10;
        public int PerSecond { get{ return PerSecondBacking < 0 ? 0 : PerSecondBacking; } set{ PerSecondBacking = value; } }
        private int PerSecondBacking = 10;
    }

    public sealed class RateLimitOptions
    {
        public Dictionary<PacketKind, RateLimitRule> Rules { get; set; } = new()
        {
            { PacketKind.Chat,    new RateLimitRule { Capacity = 5,  PerSecond = 3  } }, // ~3 msgs/s bursts up to 5
            { PacketKind.Move,    new RateLimitRule { Capacity = 30, PerSecond = 30 } }, // 30 Hz movement
            { PacketKind.JoinMap, new RateLimitRule { Capacity = 1,  PerSecond = 1  } }, // 1 per second
            { PacketKind.UseItem, new RateLimitRule { Capacity = 4,  PerSecond = 2  } },
            { PacketKind.Attack,  new RateLimitRule { Capacity = 6,  PerSecond = 4  } },
        };
    }
}
