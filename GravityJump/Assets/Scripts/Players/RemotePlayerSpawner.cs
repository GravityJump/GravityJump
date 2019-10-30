using UnityEngine;

namespace Players
{
    public class RemotePlayerSpawner : Spawner
    {
        private void Awake()
        {
            this.Prefab = Resources.Load("Prefabs/Characters/RemotePlayer") as GameObject;
            this.PlayerObject = null;
        }

        public void InstantiatePlayer(Physic.Coordinates2D coordinates)
        {
            this.PlayerObject = Instantiate(this.Prefab, new Vector3(coordinates.X, coordinates.Y, 0), Quaternion.Euler(0, 0, coordinates.ZAngle));
        }
    }
}
