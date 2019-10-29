using UnityEngine;

namespace Players
{
    public class Spawner : MonoBehaviour
    {
        protected GameObject Prefab;
        public GameObject PlayerObject;

        void Awake()
        {
            this.Prefab = Resources.Load("Prefabs/Characters/LocalPlayer") as GameObject;
            this.PlayerObject = null;
        }

        public void InstantiatePlayer(Planets.SpawningPoint point)
        {
            this.SetClosestAttractiveBody(point);
            this.PlayerObject = Instantiate(this.Prefab, new Vector3(point.X, point.Y, 0), Quaternion.Euler(0, 0, Random.value * 360));
        }

        protected virtual void SetClosestAttractiveBody(Planets.SpawningPoint point)
        {
            // Override method to set closestAttractiveBody when required.
        }

        public void Clear()
        {
            if (this.PlayerObject != null)
            {
                Destroy(this.PlayerObject);
            }
        }
    }
}
