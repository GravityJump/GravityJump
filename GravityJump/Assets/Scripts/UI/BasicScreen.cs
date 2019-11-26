using UnityEngine;

namespace UI
{
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
