using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class BasicScreen : MonoBehaviour, IGameState
    {
        public GameObject Panel;

        public virtual void Awake()
        {
            this.Panel = null;
        }

        public virtual void Clear()
        {
            this.Panel.SetActive(false);
        }

        public virtual void OnStart()
        {
            this.Panel.SetActive(true);
        }

        public virtual void OnStop()
        {
            this.Panel.SetActive(false);
        }

        public virtual void OnPause()
        {
            this.OnStop();
        }

        public virtual void OnResume()
        {
            this.OnStart();
        }
    }
}
