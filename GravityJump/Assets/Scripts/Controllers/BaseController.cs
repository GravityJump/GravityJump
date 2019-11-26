using UnityEngine;

namespace Controllers
{
    // This abstract class implements the commun logic of all controllers
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
