using UnityEngine;

namespace Players
{
    public class Spawner : MonoBehaviour
    {
        protected GameObject Prefab;
        public GameObject PlayerObject;

        public void Clear()
        {
            if (this.PlayerObject != null)
            {
                Destroy(this.PlayerObject);
            }
        }
    }
}
