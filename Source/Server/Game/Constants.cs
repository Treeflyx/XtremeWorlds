
namespace Server
{
    public class Constant
    {
        // Path constants
        public const string AdminLog = "admin.log";
        public const string PlayerLog = "player.log";
        public const string PacketLog = "packet.log";

        public const long ItemSpawnTime = 30000L; // 30 seconds
        public const long ItemDespawnTime = 90000L; // 1:30 seconds

        public const byte StatPerLevel = 5;
    }
}