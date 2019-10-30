using UnityEngine;

namespace Players
{
    public class LocalPlayerSpawner : Spawner
    {
        private void Awake()
        {
            this.Prefab = Resources.Load("Prefabs/Characters/LocalPlayer") as GameObject;
            this.PlayerObject = null;
        }

        public void InstantiatePlayer(Planets.SpawningPoint point)
        {
            this.SetClosestAttractiveBody(point);
            this.PlayerObject = Instantiate(this.Prefab, new Vector3(point.X, point.Y, 0), Quaternion.Euler(0, 0, Random.value * 360));
        }

        private void SetClosestAttractiveBody(Planets.SpawningPoint point)
        {
            this.Prefab.GetComponent<Local>().closestAttractiveBody = point.Planet.GetComponent<Physic.AttractiveBody>();
        }
    }
}
