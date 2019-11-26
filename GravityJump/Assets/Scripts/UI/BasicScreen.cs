using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // BasicScreen implements an IGameState that just hide its elements when stopped or paused, and show them when started or resumed.
    public abstract class BasicScreen : MonoBehaviour, IGameState
    {
        protected GameObject Panel { get; set; }
        public Names.Menu Name { get; set; }

        public abstract void Awake();

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
