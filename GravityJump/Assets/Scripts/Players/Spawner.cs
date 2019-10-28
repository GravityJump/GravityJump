using UnityEngine;

namespace Players
{
    public class Spawner : MonoBehaviour
    {
        private GameObject Prefab;
        public GameObject PlayerObject;

        void Awake()
        {
            this.Prefab = Resources.Load("Prefabs/Characters/Player") as GameObject;
            this.PlayerObject = null;
        }

        public void InstantiatePlayer(Planets.SpawningPoint point)
        {
            this.Prefab.GetComponent<Local>().closestAttractiveBody = point.Planet.GetComponent<Physic.AttractiveBody>();
            this.PlayerObject = Instantiate(this.Prefab, new Vector3(point.X, point.Y, 0), Quaternion.Euler(0, 0, Random.value * 360));
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
