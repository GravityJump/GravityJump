using System.Net;

namespace Data
{
    public static class Storage
    {
        public static Network.Connection Connection = null;
        public static bool IsHost = false;
        public static float LocalScore = 0;
        public static float RemoteScore = 0;
        // Global speed factor to sync player animation with player speed
        public static float SpeedFactor = 1f;
    }
}
