using UnityEngine;

namespace Controllers
{
    // BaseController defines a music and connection property.
    public abstract class BaseController : MonoBehaviour
    {
        protected Audio.MusicPlayer MusicPlayer { get; set; }
        protected Network.Connection Connection { get; set; }

        protected void Awake()
        {
            this.MusicPlayer = this.gameObject.AddComponent<Audio.MusicPlayer>();
        }
    }
}
