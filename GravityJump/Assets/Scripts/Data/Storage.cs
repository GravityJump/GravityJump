using System.Net;

namespace Data
{
    public static class Storage
    {
        public static Network.Connection Connection = null;
        public static bool IsHost = false;
        // Global speed factor to sync player animation with player speed
        public static float SpeedFactor = 1f;
    }
}
