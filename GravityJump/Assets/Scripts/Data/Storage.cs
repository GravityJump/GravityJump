using System.Net;

namespace Data
{
    public static class Storage
    {
        public static Network.Connection Connection = null;
        public static bool IsHost = false;
        // Global speed variable representing the game speed (ex: the scrolling speed, player speed,...)
        public static float GameSpeed = 1f;
        // Speed factor defining how fast is the player compared to the game speed
        public static float PlayerSpeedFactor = 1f;
        // Readonly speed variable to sync player animation with player speed
        public static float PlayerSpeed => GameSpeed * PlayerSpeedFactor;
    }
}
