using System.Net;

namespace Data
{
    // Storage is a local in-memory storage used to share common data between scenes.
    public static class Storage
    {
        public static Network.Connection Connection = null;
        public static bool IsHost = false;
    }
}
