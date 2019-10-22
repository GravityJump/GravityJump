using UnityEngine;


namespace Controllers
{
    public abstract class Player : MonoBehaviour
    {
        protected GameObject Prefab;
        public GameObject PlayerObject;

        void Awake()
        {
            this.PlayerObject = null;
        }

        public abstract void InstantiatePlayer(SpawningPoint point);

        public void Clear()
        {
            if (this.PlayerObject != null)
            {
                Destroy(this.PlayerObject);
            }
        }
    }

}
